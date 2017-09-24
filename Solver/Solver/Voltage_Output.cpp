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

#include "Voltage_Output.h"

#include "GEMS_Constant.h"
#include "FDTD_Common_Func.h"
#include "GEMS_Memory.h"

#include <mpi.h>
#include <cstdlib>
#include <cmath>


CVoltage_Output::CVoltage_Output(void)
: m_path(NULL) ,
	m_startX( 1 ) ,
	m_endX( 1 ) ,
	m_startY( 1 ) ,
	m_endY( 1 ) ,
	m_startZ( 1 ) ,
	m_endZ( 1 ) 
{
	m_voltage = NULL ;
}

CVoltage_Output::~CVoltage_Output(void)
{
	
	next = NULL ;
	// release memory allocated by path
	
	CVoltageOutPath *p = m_path, *nextPath ;
	if( p != NULL )
	{
		nextPath = static_cast<CVoltageOutPath*>(p->next );
		while( p != NULL )
		{
			delete p ;
			p = nextPath ;

			// get next segment of the path
			if( p != NULL )
				nextPath = static_cast<CVoltageOutPath*>(p->next );
		}
	}
	
	Free_1D( m_voltage ) ;

}

int CVoltage_Output::init(void)
{
	if( id == 0 )
	{
		m_voltage = Allocate_1D( m_voltage, m_timeStep ) ;
		if( m_voltage == NULL )
			return FDTD_NO_MEMORY ;
	}

	return FDTD_SUCCESS;
}



void CVoltage_Output::getStartIndex(int& x, int& y, int& z)
{
	x = m_startX ;
	y = m_startY ;
	z = m_startZ ;
}

void CVoltage_Output::getEndIndex(int& x, int& y, int& z)
{
	x = m_endX ;
	y = m_endY ;
	z = m_endZ ;
}



void CVoltage_Output::setStartIndex(int x, int y, int z)
{
	m_startX = x ;
	m_startY = y ;
	m_startZ = z ;
}

void CVoltage_Output::setEndIndex(int x, int y, int z)
{
	m_endX = x ;
	m_endY = y ;
	m_endZ = z ;
}

void CVoltage_Output::collectResult(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	
	// local index of path point
	int lx1, ly1, lz1, lx2, ly2, lz2 ;


	float v , sum_v ;
	v = 0.0f ;
	if( id == 0 )
	{
		MPI_Reduce( &v, &sum_v, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		m_voltage[n] = sum_v ;
	}
	else
	{
		CVoltageOutPath *path = NULL ;
		path = m_path ;

		while( path != NULL )
		{
			// get start and end point of path
			lx1 = path->startX ;
			ly1 = path->startY ;
			lz1 = path->startZ ;

			lx2 = path->endX ;
			ly2 = path->endY ;
			lz2 = path->endZ ;

			// path along x direction
			if( lx2 > lx1 )
			{
				v += Ex[lx1][ly1][lz1] * m_Dx_Grid[lx1] * path->factor ;
			}
			else if( lx1 > lx2 )
			{
				v -= Ex[lx2][ly2][lz2] * m_Dx_Grid[lx2] * path->factor ;
			}

			// path along y direction
			if( ly2 > ly1 )
			{
				v += Ey[lx1][ly1][lz1] * m_Dy_Grid[ly1] * path->factor ;
			}
			else if( ly1 > ly2 )
			{
				v -= Ey[lx2][ly2][lz2] * m_Dy_Grid[ly2] * path->factor ;
			}

			// path along z direction
			if( lz2 > lz1 )
			{
				v += Ez[lx1][ly1][lz1] * m_Dz_Grid[lz1] * path->factor ;
			}
			else if( lz1 > lz2 )
			{
				v -= Ez[lx2][ly2][lz2] * m_Dz_Grid[lz2] * path->factor ;
			}

			// get voltage path segment
			path = static_cast<CVoltageOutPath*>(path->next );
		}
		// end of while( path != NULL )


		v *= -1 ;

		MPI_Reduce( &v, &sum_v, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;

	}

}

void CVoltage_Output::output( XML_Writer &outFile )
{
	std::string str ;
	char c_string[64] = "";
	outFile.AddAtributes( "objectName", m_objectName ) ;
	outFile.Createtag( "VoltageOutput" ) ;

		// write Position
		outFile.Createtag( "Position" ) ;

			// write start position
			sprintf_s( c_string, "%d", m_startZ ) ;
			str = c_string ;
			outFile.AddAtributes( "z", str ) ;
			
			sprintf_s( c_string, "%d", m_startY ) ;
			str = c_string ;
			outFile.AddAtributes( "y", str ) ;
			
			sprintf_s( c_string, "%d", m_startX ) ;
			str = c_string ;
			outFile.AddAtributes( "x", str ) ;

			outFile.Createtag( "Startpoint" ) ;
			outFile.CloseSingletag();
			// end of write start position


			// write end position
			sprintf_s( c_string, "%d", m_endZ ) ;
			str = c_string ;
			outFile.AddAtributes( "z", str ) ;

			sprintf_s( c_string, "%d", m_endY ) ;
			str = c_string ;
			outFile.AddAtributes( "y", str ) ;
			
			sprintf_s( c_string, "%d", m_endX ) ;
			str = c_string ;
			outFile.AddAtributes( "x", str ) ;

			outFile.Createtag( "Endpoint" ) ;
			outFile.CloseSingletag();
			// end of write end position

		outFile.CloseLasttag();
		// end of write Position

		// write voltage
		outFile.Createtag( "Voltages" ) ;
		int i ;
		for( i = 0 ; i < m_timeStep ; i++ )
		{
			sprintf_s( c_string, "%e", m_voltage[i] ) ;
			str = c_string ;
			outFile.AddAtributes( "value", str ) ;
			outFile.Createtag( "Voltage" ) ;
			outFile.CloseSingletag();
		}

		outFile.CloseLasttag();
		// end of write voltage


	outFile.CloseLasttag();
}

int CVoltage_Output::readIn(FILE *in)
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

	fread( endMark, sizeof( char ), 2, in ) ;


	MPI_Bcast( &m_startX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	return FDTD_SUCCESS ;
}

int CVoltage_Output::readIn_Solver( )
{
	MPI_Bcast( &m_startX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_startZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endX , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endY , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_endZ , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;


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

	return FDTD_SUCCESS ;
}

int CVoltage_Output::insertVoltagePath(int x1, int y1, int z1, int x2, int y2, int z2)
{
	if ( ( x1 < m_localDomainIndex[idSolver].startX ) || ( x1 > m_localDomainIndex[idSolver].endX ) ) return FDTD_SUCCESS;
	if ( ( y1 < m_localDomainIndex[idSolver].startY ) || ( y1 > m_localDomainIndex[idSolver].endY ) ) return FDTD_SUCCESS;
	if ( ( z1 < m_localDomainIndex[idSolver].startZ ) || ( z1 > m_localDomainIndex[idSolver].endZ ) ) return FDTD_SUCCESS;
	if ( ( x2 < m_localDomainIndex[idSolver].startX ) || ( x2 > m_localDomainIndex[idSolver].endX ) ) return FDTD_SUCCESS;
	if ( ( y2 < m_localDomainIndex[idSolver].startY ) || ( y2 > m_localDomainIndex[idSolver].endY ) ) return FDTD_SUCCESS;
	if ( ( z2 < m_localDomainIndex[idSolver].startZ ) || ( z2 > m_localDomainIndex[idSolver].endZ ) ) return FDTD_SUCCESS;

	CVoltageOutPath *oldPath, *path;
	int lx1, ly1, lz1, lx2, ly2, lz2 ;

	oldPath = path = m_path;
	while ( path != NULL ) {
		oldPath = path;
		path = static_cast<CVoltageOutPath*>( path->next ) ;
	}

	if ( ( x1 >= m_localDomainIndex[idSolver].startX ) && ( x1 <= m_localDomainIndex[idSolver].endX ) &&
		 ( x2 >= m_localDomainIndex[idSolver].startX ) && ( x2 <= m_localDomainIndex[idSolver].endX ) 
		) 
	{
		if ( x1 != x2 ) {
			path = new CVoltageOutPath;
			if( path == 0 )
				return FDTD_NO_MEMORY ;

			Global2Local( x1, y1, z1, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			Global2Local( x2, y2, z2, lx2, ly2, lz2 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;


			path->startX = lx1; path->startY = ly1; path->startZ = lz1;
			path->endX = lx2; path->endY = ly1; path->endZ = lz1;
			path->next = NULL;

			if( ( y1 == m_localDomainIndex[idSolver].startY ) || 
				( y1 == m_localDomainIndex[idSolver].endY ) )
				path->factor *= 0.5 ;

			if( ( z1 == m_localDomainIndex[idSolver].startZ ) || 
				( z1 == m_localDomainIndex[idSolver].endZ ) )
				path->factor *= 0.5 ;

			if ( oldPath == NULL ) {
				m_path = path;
			} else {
				oldPath->next = path;
			}
			oldPath = path;
			x1 = x2;
		}
	}

	if ( ( y1 >= m_localDomainIndex[idSolver].startY ) && ( y1 <= m_localDomainIndex[idSolver].endY ) &&
		 ( y2 >= m_localDomainIndex[idSolver].startY ) && ( y2 <= m_localDomainIndex[idSolver].endY ) 
		) 
	{
	
		if ( y1 != y2 ) {
			path = new CVoltageOutPath;
			if( path == 0 )
				return FDTD_NO_MEMORY ;

			Global2Local( x1, y1, z1, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			Global2Local( x2, y2, z2, lx2, ly2, lz2 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;

			path->startX = lx1; path->startY = ly1; path->startZ = lz1;
			path->endX = lx1; path->endY = ly2; path->endZ = lz1;
			path->next = NULL;

			if( ( x2 == m_localDomainIndex[idSolver].startX ) || 
				( x2 == m_localDomainIndex[idSolver].endX ) )
				path->factor *= 0.5 ;

			if( ( z1 == m_localDomainIndex[idSolver].startZ ) || 
				( z1 == m_localDomainIndex[idSolver].endZ ) )
				path->factor *= 0.5 ;

			if ( oldPath == NULL ) {
				m_path = path;
			} else {
				oldPath->next = path;
			}
			oldPath = path;
			y1 = y2;
		}
	}

	if ( ( z1 >= m_localDomainIndex[idSolver].startZ ) && ( z1 <= m_localDomainIndex[idSolver].endZ ) &&
		 ( z2 >= m_localDomainIndex[idSolver].startZ ) && ( z2 <= m_localDomainIndex[idSolver].endZ ) 
		) 
	{
		if ( z1 != z2 ) {
			path = new CVoltageOutPath;
			if( path == 0 )
				return FDTD_NO_MEMORY ;

			Global2Local( x1, y1, z1, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			Global2Local( x2, y2, z2, lx2, ly2, lz2 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;

			path->startX = lx1; path->startY = ly1; path->startZ = lz1;
			path->endX = lx1; path->endY = ly1; path->endZ = lz2;
			path->next = NULL;

			if( ( x2 == m_localDomainIndex[idSolver].startX ) || 
				( x2 == m_localDomainIndex[idSolver].endX ) )
				path->factor *= 0.5 ;

			if( ( y2 == m_localDomainIndex[idSolver].startY ) || 
				( y2 == m_localDomainIndex[idSolver].endY ) )
				path->factor *= 0.5 ;

			if ( oldPath == NULL ) {
				m_path = path;
			} else {
				oldPath->next = path;
			}
			oldPath = path;
			z1 = z2;
		}
	}

	return FDTD_SUCCESS ;
}
