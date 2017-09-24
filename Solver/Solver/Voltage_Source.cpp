/*
 OpenGEMS is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 OpenGEMS is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

  Copyright 2007 by Computer and Communication Unlimited
*/

#include "Voltage_Source.h"

#include "GEMS_Constant.h"
#include "FDTD_Common_Func.h"

#include <mpi.h>
#include <cstdlib>
#include <cmath>

CVoltage_Source::CVoltage_Source(void)
: m_path(NULL) ,
	m_startX( 1 ) ,
	m_endX( 1 ) ,
	m_startY( 1 ) ,
	m_endY( 1 ) ,
	m_startZ( 1 ) ,
	m_endZ( 1 )
{
	m_eMatType = NULL ;
	m_sourceNum++ ;
}

CVoltage_Source::~CVoltage_Source(void)
{
	m_sourceNum-- ;
	next = 0 ;

	// release memory allocated by path
	
	CVoltagePath *p = m_path, *nextPath ;
	if( p != NULL )
	{
		nextPath = p->next ;
		while( p != NULL )
		{
			delete p ;
			p = nextPath ;

			// get next segment of the path
			if( p != NULL )
				nextPath = p->next ;
		}
	}
}

int CVoltage_Source::init(void)
{
	return FDTD_SUCCESS;
}



void CVoltage_Source::getStartIndex(int& x, int& y, int& z)
{
	x = m_startX ;
	y = m_startY ;
	z = m_startZ ;
}

void CVoltage_Source::getEndIndex(int& x, int& y, int& z)
{
	x = m_endX ;
	y = m_endY ;
	z = m_endZ ;
}



void CVoltage_Source::setStartIndex(int x, int y, int z)
{
	m_startX = x ;
	m_startY = y ;
	m_startZ = z ;
}

void CVoltage_Source::setEndIndex(int x, int y, int z)
{
	m_endX = x ;
	m_endY = y ;
	m_endZ = z ;
}

void CVoltage_Source::update_E_Field(int n, float*** (&Ex), float*** (&Ey), float*** (&Ez), int sourceIndex, CFarField *farField) 
{
	float E_J_density = 0.0f , E_J_density_sum = 0.0f ;

	int lx1, lx2, ly1, ly2, lz1, lz2 ;

	float Coeffexp,Coeffeyp,Coeffezp, temp_field = 0.0f ,temp_field_sum = 0.0f ;
	float temp ;


	if( id == 0 )
	{
		
		int inFlag = 0 , inFlagSum = 0 ;
		// count the number of domains that contains this voltage source
		// after E_J_density is collected, the sum of E_J_density should be divided by number of domains that contains this voltage source
		MPI_Reduce( &inFlag, &inFlagSum, 1 , MPI_INT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;

		MPI_Reduce( &temp_field, &temp_field_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Reduce( &E_J_density, &E_J_density_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Barrier( MPI_COMM_WORLD ) ;

		E_J_density_sum /= inFlagSum ;

		int freqIndex ;
		float exp_e_real, exp_e_imag ;
		if( farField != NULL )
		{
			if( farField->getEnabled() )
			{
				float sourceReal, sourceImag, powerReal, powerImag ;

				for ( freqIndex = 0 ; freqIndex < farField->getFreqNum() ; freqIndex++ )  //incident power DFT
				{
					exp_e_real =   cos(2 * PI * farField->getFreq( freqIndex ) * ( n + 0.5f ) * m_Dt ) ;
					exp_e_imag = - sin(2 * PI * farField->getFreq( freqIndex ) * ( n + 0.5f ) * m_Dt ) ;

					farField->getSourcePower( sourceReal, sourceImag, powerReal, powerImag , freqIndex, sourceIndex ) ;
					
					powerReal += exp_e_real * temp_field_sum ;
					powerImag += exp_e_imag * temp_field_sum ;
					sourceReal += exp_e_real * E_J_density_sum ;
					sourceImag += exp_e_imag * E_J_density_sum ;

					farField->setSourcePower( sourceReal, sourceImag, powerReal, powerImag , freqIndex, sourceIndex ) ;
				}
			}
		}
	}
	else
	{
		CVoltagePath *path = m_path ;

		if( m_insideFlag == true )
			E_J_density = m_voltage * Excitation_Source( n, m_delay , m_pulse, m_pulseLen, m_Dt ) ;
		else
			E_J_density = 0 ;
		
		temp_field = 0.0 ;



		if( m_insideFlag )
		{
			while( path != NULL )
			{

				lx1 = path->startX ;
				ly1 = path->startY ;
				lz1 = path->startZ ;

				lx2 = path->endX ;
				ly2 = path->endY ;
				lz2 = path->endZ ;


				if (lx1 < lx2) 
				{
					Coeffexp = m_Dt * m_eMatType[lx1][ly1][lz1]->AEx_p_Inv ;


					temp = Ex[lx1][ly1][lz1] * m_Dx_Grid[lx1] * m_Dy_Half[ly1] * m_Dz_Half[lz1];

					// if the voltage lies on boundary, multiply by 0.5
					if( ( ly1 == m_Index_E_Boundary[YMIN] ) || ( ly1 == m_Index_E_Boundary[YMAX] ) )
						temp *= 0.5 ;

					if( ( lz1 == m_Index_E_Boundary[ZMIN] ) || ( lz1 == m_Index_E_Boundary[ZMAX] ) )
						temp *= 0.5 ;

					temp_field += temp ;


					Ex[lx1][ly1][lz1] += Coeffexp * E_J_density ;

				} else if (lx1 > lx2)
				{
					Coeffexp = m_Dt * m_eMatType[lx2][ly1][lz1]->AEx_p_Inv ;

					temp = Ex[lx2][ly1][lz1] * m_Dx_Grid[lx2] * m_Dy_Half[ly1] * m_Dz_Half[lz1];

					// if the voltage lies on boundary, multiply by 0.5
					if( ( ly1 == m_Index_E_Boundary[YMIN] ) || ( ly1 == m_Index_E_Boundary[YMAX] ) )
						temp *= 0.5 ;

					if( ( lz1 == m_Index_E_Boundary[ZMIN] ) || ( lz1 == m_Index_E_Boundary[ZMAX] ) )
						temp *= 0.5 ;

					temp_field += temp ;

					Ex[lx2][ly1][lz1] -= Coeffexp * E_J_density ;
					
				}

				if (ly1 < ly2) 
				{
					Coeffeyp = m_Dt * m_eMatType[lx1][ly1][lz1]->AEy_p_Inv ;

					temp = Ey[lx1][ly1][lz1] * m_Dx_Half[lx1] * m_Dy_Grid[ly1] * m_Dz_Half[lz1];

					// if the voltage lies on boundary, multiply by 0.5
					if( ( lx1 == m_Index_E_Boundary[XMIN] ) || ( lx1 == m_Index_E_Boundary[XMAX] ) )
						temp *= 0.5 ;

					if( ( lz1 == m_Index_E_Boundary[ZMIN] ) || ( lz1 == m_Index_E_Boundary[ZMAX] ) )
						temp *= 0.5 ;

					temp_field += temp ;

					Ey[lx1][ly1][lz1] += Coeffeyp * E_J_density ;

				} else if (ly1 > ly2) 
				{
					Coeffeyp = m_Dt * m_eMatType[lx1][ly2][lz1]->AEy_p_Inv ;

					temp = Ey[lx1][ly2][lz1] * m_Dx_Half[lx1] * m_Dy_Grid[ly2] * m_Dz_Half[lz1];

					// if the voltage lies on boundary, multiply by 0.5
					if( ( lx1 == m_Index_E_Boundary[XMIN] ) || ( lx1 == m_Index_E_Boundary[XMAX] ) )
						temp *= 0.5 ;

					if( ( lz1 == m_Index_E_Boundary[ZMIN] ) || ( lz1 == m_Index_E_Boundary[ZMAX] ) )
						temp *= 0.5 ;

					temp_field += temp ;

					Ey[lx1][ly2][lz1] -= Coeffeyp * E_J_density ;

				}

				if (lz1 < lz2) 
				{
					Coeffezp = m_Dt * m_eMatType[lx1][ly1][lz1]->AEz_p_Inv ;

					temp = Ez[lx1][ly1][lz1] * m_Dx_Half[lx1] * m_Dy_Half[ly1] * m_Dz_Grid[lz1];

					// if the voltage lies on boundary, multiply by 0.5
					if( ( lx1 == m_Index_E_Boundary[XMIN] ) || ( lx1 == m_Index_E_Boundary[XMAX] ) )
						temp *= 0.5 ;

					if( ( ly1 == m_Index_E_Boundary[YMIN] ) || ( ly1 == m_Index_E_Boundary[YMAX] ) )
						temp *= 0.5 ;

					temp_field += temp ;

					Ez[lx1][ly1][lz1] += Coeffezp * E_J_density ; //Excitation source

				} 
				else if (lz1 > lz2)
				{
					Coeffezp = m_Dt * m_eMatType[lx1][ly1][lz2]->AEz_p_Inv ;

					temp = Ez[lx1][ly1][lz2] * m_Dx_Half[lx1] * m_Dy_Half[ly1] * m_Dz_Grid[lz2];

					// if the voltage lies on boundary, multiply by 0.5
					if( ( lx1 == m_Index_E_Boundary[XMIN] ) || ( lx1 == m_Index_E_Boundary[XMAX] ) )
						temp *= 0.5 ;

					if( ( ly1 == m_Index_E_Boundary[YMIN] ) || ( ly1 == m_Index_E_Boundary[YMAX] ) )
						temp *= 0.5 ;

					temp_field += temp ;

					Ez[lx1][ly1][lz2] -= Coeffezp * E_J_density ;
				}

				path = path->next ;

			}
		}

		// 
		int inFlag = 0 , inFlagSum = 0 ;
		if( m_insideFlag == true )
			inFlag = 1 ;
		else
			inFlag = 0 ;
	
		// count the number of domains that contains this voltage source
		// after E_J_density is collected, the sum of E_J_density should be divided by number of domains that contains this voltage source
		MPI_Reduce( &inFlag, &inFlagSum, 1 , MPI_INT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;

		MPI_Reduce( &temp_field, &temp_field_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Reduce( &E_J_density, &E_J_density_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Barrier( MPI_COMM_WORLD ) ;

		
	}
}

int CVoltage_Source::readIn(FILE *in)
{
	char endMark[2] ;

	char name[MAX_OUTPUTNAME_LENGTH+1] ;
	fread( name, sizeof( char ), MAX_OUTPUTNAME_LENGTH, in ) ;
	name[MAX_OUTPUTNAME_LENGTH] = 0 ;
	m_objectName = name ;

	fread( &m_startX, sizeof( int ), 1, in ) ;
	fread( &m_startY, sizeof( int ), 1, in ) ;
	fread( &m_startZ, sizeof( int ), 1, in ) ;

	fread( &m_endX, sizeof( int ), 1, in ) ;
	fread( &m_endY, sizeof( int ), 1, in ) ;
	fread( &m_endZ, sizeof( int ), 1, in ) ;

	fread( &m_voltage, sizeof( float ), 1, in ) ;
	fread( &m_delay, sizeof( float ), 1, in ) ;

	fread( endMark, sizeof( char ), 2, in ) ;


	MPI_Bcast( &m_startX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( &m_voltage , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_delay , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;


	return FDTD_SUCCESS ;
}

int CVoltage_Source::readIn_Solver( )
{
	// broadcasting information to all processor
	MPI_Bcast( &m_startX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( &m_voltage , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_delay , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	

	// calculate the voltage of unit
	float lengthx = 0 , lengthy = 0 , lengthz = 0 , length ;
	if( m_startX < m_endX )
	{
		for( int i = m_startX ; i < m_endX ; i++ )
			lengthx += m_Dx_Grid_Global[i] ;
	}
	else
	{
		for( int i = m_endX ; i < m_startX ; i++ )
			lengthx += m_Dx_Grid_Global[i] ;
	}

	if( m_startY < m_endY )
	{
		for( int i = m_startY ; i < m_endY ; i++ )
			lengthy += m_Dy_Grid_Global[i] ;
	}
	else
	{
		for( int i = m_endY ; i < m_startY ; i++ )
			lengthy += m_Dy_Grid_Global[i] ;
	}

	if( m_startZ < m_endZ )
	{
		for( int i = m_startZ ; i < m_endZ ; i++ )
			lengthz += m_Dz_Grid_Global[i] ;
	}
	else
	{
		for( int i = m_endZ ; i < m_startZ ; i++ )
			lengthz += m_Dz_Grid_Global[i] ;
	}

	//length = sqrt( lengthx * lengthx + lengthy * lengthy + lengthz * lengthz ) ;
	length = lengthx + lengthy + lengthz ;

	m_voltage /= length ;


	float Torelant = 1.0e-3f ;

	float fx, fy, fz;
	int x , y , z ;
	int nextx, nexty, nextz ;

	int iDx, iDy, iDz , iMaxD ;
	float fDx, fDy, fDz, fMaxD ;
	float stepx, stepy, stepz ;

	iDx = m_endX - m_startX ;
	iDy = m_endY - m_startY ; 
	iDz = m_endZ - m_startZ ; 
	fDx = static_cast<float>(iDx) ;
	fDy = static_cast<float>(iDy) ;
	fDz = static_cast<float>(iDz) ;

	fMaxD = fabs( fDx ) > fabs( fDy ) ? fabs( fDx ) : fabs( fDy ) ;
	iMaxD = static_cast<int>( fMaxD > fabs( fDz ) ? fMaxD : fabs( fDz ) ) ;

	
	if( iDx == 0 && iDy == 0 && iDz == 0 )
	{
		stepx = stepy = stepz = 0.0 ;
	}
	else
	{
		stepx = float( iDx ) / iMaxD ;
		stepy = float( iDy ) / iMaxD ;
		stepz = float( iDz ) / iMaxD ;
	}

	x = m_startX;
	y = m_startY;
	z = m_startZ;

	fx = static_cast<float>( x ) ;
	fy = static_cast<float>( y ) ;
	fz = static_cast<float>( z ) ;

	do {
		fx += stepx;
		fy += stepy;
		fz += stepz;
		nextx = static_cast<int>( fx + Torelant );
		nexty = static_cast<int>( fy + Torelant );
		nextz = static_cast<int>( fz + Torelant );
		
		int result = insertVoltagePath( x, y, z, nextx, nexty, nextz );
		if( result == FDTD_NO_MEMORY )
			return FDTD_NO_MEMORY ;

		x = nextx;
		y = nexty;
		z = nextz;
	} while ( ( x != m_endX ) || ( y != m_endY ) || ( z != m_endZ ) );

	// insert path
	//
	if( m_path != NULL )
	{
		m_insideFlag = true ;
	}
	else
	{
		m_insideFlag = false ;
	}

	return FDTD_SUCCESS ;
}

int CVoltage_Source::insertVoltagePath(int x1, int y1, int z1, int x2, int y2, int z2)
{
	if ( ( x1 < m_localDomainIndex[idSolver].startX ) || ( x1 > m_localDomainIndex[idSolver].endX ) ) return FDTD_SUCCESS;
	if ( ( y1 < m_localDomainIndex[idSolver].startY ) || ( y1 > m_localDomainIndex[idSolver].endY ) ) return FDTD_SUCCESS;
	if ( ( z1 < m_localDomainIndex[idSolver].startZ ) || ( z1 > m_localDomainIndex[idSolver].endZ ) ) return FDTD_SUCCESS;
	if ( ( x2 < m_localDomainIndex[idSolver].startX ) || ( x2 > m_localDomainIndex[idSolver].endX ) ) return FDTD_SUCCESS;
	if ( ( y2 < m_localDomainIndex[idSolver].startY ) || ( y2 > m_localDomainIndex[idSolver].endY ) ) return FDTD_SUCCESS;
	if ( ( z2 < m_localDomainIndex[idSolver].startZ ) || ( z2 > m_localDomainIndex[idSolver].endZ ) ) return FDTD_SUCCESS;

	CVoltagePath *oldPath, *path;
	int lx1, ly1, lz1, lx2, ly2, lz2 ;

	oldPath = path = m_path;
	while ( path != NULL )
	{
		oldPath = path;
		path = path->next;
	}


	if ( ( x1 >= m_localDomainIndex[idSolver].startX ) && ( x1 <= m_localDomainIndex[idSolver].endX ) &&
		 ( x2 >= m_localDomainIndex[idSolver].startX ) && ( x2 <= m_localDomainIndex[idSolver].endX ) ) 
	{
		if ( x1 != x2 ) 
		{
			path = new CVoltagePath;
			if( path == 0 )
				return FDTD_NO_MEMORY ;

			Global2Local( x1, y1, z1, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			Global2Local( x2, y2, z2, lx2, ly2, lz2 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;

			path->startX = lx1; path->startY = ly1; path->startZ = lz1;
			path->endX = lx2; path->endY = ly1; path->endZ = lz1;
			path->next = NULL;
			
			if ( oldPath == NULL ) 
			{
				m_path = path;
			} 
			else 
			{
				oldPath->next = path;
			}
			oldPath = path;
			x1 = x2;
		}
	}

	if ( ( y1 >= m_localDomainIndex[idSolver].startY ) && ( y1 <= m_localDomainIndex[idSolver].endY ) &&
		 ( y2 >= m_localDomainIndex[idSolver].startY ) && ( y2 <= m_localDomainIndex[idSolver].endY ) ) 
	{
	
		if ( y1 != y2 ) 
		{
			path = new CVoltagePath;
			if( path == 0 )
				return FDTD_NO_MEMORY ;

			Global2Local( x1, y1, z1, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			Global2Local( x2, y2, z2, lx2, ly2, lz2 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;

			path->startX = lx1; path->startY = ly1; path->startZ = lz1;
			path->endX = lx1; path->endY = ly2; path->endZ = lz1;
			path->next = NULL;
			if ( oldPath == NULL ) 
			{
				m_path = path;
			} 
			else 
			{
				oldPath->next = path;
			}
			oldPath = path;
			y1 = y2;
		}
	}

	if ( ( z1 >= m_localDomainIndex[idSolver].startZ ) && ( z1 <= m_localDomainIndex[idSolver].endZ ) &&
		 ( z2 >= m_localDomainIndex[idSolver].startZ ) && ( z2 <= m_localDomainIndex[idSolver].endZ ) ) 
	{
		if ( z1 != z2 ) 
		{
			path = new CVoltagePath;
			if( path == 0 )
				return FDTD_NO_MEMORY ;




			Global2Local( x1, y1, z1, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			Global2Local( x2, y2, z2, lx2, ly2, lz2 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;

			path->startX = lx1; path->startY = ly1; path->startZ = lz1;
			path->endX = lx1; path->endY = ly1; path->endZ = lz2;
			path->next = NULL;
			if ( oldPath == NULL ) 
			{
				m_path = path;
			} 
			else 
			{
				oldPath->next = path;
			}

			oldPath = path;
			z1 = z2;
		}
	}

	return FDTD_SUCCESS ;
}
