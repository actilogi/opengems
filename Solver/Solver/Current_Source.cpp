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

#include "Current_Source.h"

#include "GEMS_Constant.h"
#include "FDTD_Common_Func.h"
#include "Global_variable.h"

#include <mpi.h>
#include <cstdlib>
#include <vector>

CCurrent_Source::CCurrent_Source(void)
{
	m_sourceNum++ ;
	m_cellNum = 0 ;

	m_verticalIndex = 1 ;
	m_path = NULL ;
}

CCurrent_Source::~CCurrent_Source(void)
{
	m_sourceNum-- ;
	next = NULL ;
	if( m_path != NULL )
	{
		delete[] m_path ;
		m_path = NULL ;
	}
}

int CCurrent_Source::init(void)
{
	return FDTD_SUCCESS;
}


int CCurrent_Source::readIn(FILE *in)
{
	struct CurrentIndex{
		int x ;
		int y ;
	} ;

	int verticalIndex ;
	int vertexNum ;
	char direction ;
	CurrentIndex *currentIndex = NULL ;
	char endMark[2] ;

	char name[MAX_OUTPUTNAME_LENGTH+1] ;
	fread( name, sizeof( char ), MAX_OUTPUTNAME_LENGTH, in ) ;
	name[MAX_OUTPUTNAME_LENGTH] = 0 ;
	m_objectName = name ;

	fread( &direction, sizeof( char ), 1, in ) ;
	fread( &verticalIndex, sizeof( int ), 1, in ) ;

	fread( &vertexNum, sizeof( int ), 1, in ) ;

	currentIndex = new CurrentIndex[vertexNum] ;
	// read in polygon pathSingle indexes , the indexes are colck wise 
	// after read in, we need to change them into cell index pair

	fread( currentIndex, sizeof( CurrentIndex ), vertexNum, in ) ;

	fread( &m_current, sizeof( float ), 1, in ) ;
	fread( &m_delay, sizeof( float ), 1, in ) ;

	fread( endMark, sizeof( char ), 2, in ) ;

	MPI_Bcast( &direction , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &verticalIndex , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	
	MPI_Bcast( &vertexNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( currentIndex , vertexNum * 2 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( &m_current , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_delay , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	m_dir = static_cast<CurrenDirection> (direction ) ;
	m_verticalIndex = verticalIndex ;

	delete[] currentIndex ;

	return FDTD_SUCCESS ;
}


int CCurrent_Source::readIn_Solver( )
{
	struct CurrentIndex{
		int x ;
		int y ;
	} ;

	int i , j ;
	int verticalIndex ;
	int vertexNum ;
	char direction ;
	CurrentIndex *currentIndex = NULL ;

	MPI_Bcast( &direction , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &verticalIndex , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	
	MPI_Bcast( &vertexNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	currentIndex = new CurrentIndex[vertexNum] ;

	MPI_Bcast( currentIndex , vertexNum * 2 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( &m_current , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_delay , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	std::vector< CCurrentPath > path ;
	CCurrentPath pathSingle ;

	CurrentIndex curIndex, nextIndex ;

	// calculate current outline loop length
	float PolygonLeng = 0 ;		// the length of the current loop

	if( CurrenDirection( direction ) == DIRECTION_Z || CurrenDirection( direction ) == DIRECTION_Z_MINUS )	// Z direction
	{
		for( i = 0 ; i < vertexNum ; i++ )
		{
			curIndex = currentIndex[i] ;
			if( i < vertexNum - 1 )
				nextIndex = currentIndex[i+1] ;
			else
				nextIndex = currentIndex[0] ;

			if( curIndex.y == nextIndex.y )		// alone x+ direction
			{
				if( curIndex.x < nextIndex.x )		
				{
					for( j = curIndex.x ; j < nextIndex.x ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dx_Grid_Global[j] * 0.5f ;
						else if( j == m_nx_Global )
							PolygonLeng += m_Dx_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dx_Grid_Global[j-1] + m_Dx_Grid_Global[j] ) * 0.5f ;
					}
				}
				else								// x- direction
				{
					for( j = nextIndex.x ; j < curIndex.x ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dx_Grid_Global[j] * 0.5f ;
						else if( j == m_nx_Global )
							PolygonLeng += m_Dx_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dx_Grid_Global[j-1] + m_Dx_Grid_Global[j] ) * 0.5f ;
					}
				}
			}
			else
			{
				if( curIndex.y < nextIndex.y )		// alone y+ direction
				{
					for( j = curIndex.y ; j < nextIndex.y ; j++ )		
					{
						if( j == 0 )
							PolygonLeng += m_Dy_Grid_Global[j] * 0.5f ;
						else if( j == m_ny_Global )
							PolygonLeng += m_Dy_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dy_Grid_Global[j-1] + m_Dy_Grid_Global[j] ) * 0.5f ;
					}
				}
				else								// alone y- direction							
				{
					for( j = nextIndex.y ; j < curIndex.y ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dy_Grid_Global[j] * 0.5f ;
						else if( j == m_ny_Global )
							PolygonLeng += m_Dy_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dy_Grid_Global[j-1] + m_Dy_Grid_Global[j] ) * 0.5f ;
					}
				}
			}
		}
	}
	else if( CurrenDirection( direction ) == DIRECTION_X || CurrenDirection( direction ) == DIRECTION_X_MINUS )	
	{		// x direction, yz plane

		for( i = 0 ; i < vertexNum ; i++ )
		{
			curIndex = currentIndex[i] ;
			if( i < vertexNum - 1 )
				nextIndex = currentIndex[i+1] ;
			else
				nextIndex = currentIndex[0] ;

			if( curIndex.y == nextIndex.y )		// alone y+ direction
			{
				if( curIndex.x < nextIndex.x )
				{
					for( j = curIndex.x ; j < nextIndex.x ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dy_Grid_Global[j] * 0.5f ;
						else if( j == m_ny_Global )
							PolygonLeng += m_Dy_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dy_Grid_Global[j-1] + m_Dy_Grid_Global[j] ) * 0.5f ;
					}

				}
				else						// alone y- direction
				{
					for( j = nextIndex.x ; j < curIndex.x ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dy_Grid_Global[j] * 0.5f ;
						else if( j == m_ny_Global )
							PolygonLeng += m_Dy_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dy_Grid_Global[j-1] + m_Dy_Grid_Global[j] ) * 0.5f ;
					}

				}
			}
			else			// alone z direction
			{
				if( curIndex.y < nextIndex.y )		
				{
					for( j = curIndex.y ; j < nextIndex.y ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dz_Grid_Global[j] * 0.5f ;
						else if( j == m_nz_Global )
							PolygonLeng += m_Dz_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dz_Grid_Global[j-1] + m_Dz_Grid_Global[j] ) * 0.5f ;
					}

				}
				else
				{
					for( j = nextIndex.y ; j < curIndex.y ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dz_Grid_Global[j] * 0.5f ;
						else if( j == m_nz_Global )
							PolygonLeng += m_Dz_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dz_Grid_Global[j-1] + m_Dz_Grid_Global[j] ) * 0.5f ;
					}

				}
			}
		}
	}
	else if( CurrenDirection( direction ) == DIRECTION_Y || CurrenDirection( direction ) == DIRECTION_Y_MINUS )	// y direction
	{
		for( i = 0 ; i < vertexNum ; i++ )
		{
			curIndex = currentIndex[i] ;
			if( i < vertexNum - 1 )
				nextIndex = currentIndex[i+1] ;
			else
				nextIndex = currentIndex[0] ;

			if( curIndex.y == nextIndex.y )		// alone x direction
			{
				if( curIndex.x < nextIndex.x )
				{

					for( j = curIndex.x ; j < nextIndex.x ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dx_Grid_Global[j] * 0.5f ;
						else if( j == m_nx_Global )
							PolygonLeng += m_Dx_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dx_Grid_Global[j-1] + m_Dx_Grid_Global[j] ) * 0.5f ;
					}
					
					
				}
				else
				{
					for( j = nextIndex.x ; j < curIndex.x ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dx_Grid_Global[j] * 0.5f ;
						else if( j == m_nx_Global )
							PolygonLeng += m_Dx_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dx_Grid_Global[j-1] + m_Dx_Grid_Global[j] ) * 0.5f ;
					}
					
				}
			}
			else				// alone z direction
			{
				if( curIndex.y < nextIndex.y )		
				{
					for( j = curIndex.y ; j < nextIndex.y ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dz_Grid_Global[j] * 0.5f ;
						else if( j == m_nz_Global )
							PolygonLeng += m_Dz_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dz_Grid_Global[j-1] + m_Dz_Grid_Global[j] ) * 0.5f ;
					}

				}
				else
				{
					for( j = nextIndex.y ; j < curIndex.y ; j++ )
					{
						if( j == 0 )
							PolygonLeng += m_Dz_Grid_Global[j] * 0.5f ;
						else if( j == m_nz_Global )
							PolygonLeng += m_Dz_Grid_Global[j-1] * 0.5f ;
						else
							PolygonLeng += ( m_Dz_Grid_Global[j-1] + m_Dz_Grid_Global[j] ) * 0.5f ;
					}

				}
			}
		}
	}

	// end of calculate current outline length

	// get H field

	m_current /= PolygonLeng ;

	// fill current out path array
	for( i = 0 ; i < vertexNum ; i++ )
	{
		curIndex = currentIndex[i] ;
		if( i < vertexNum - 1 )
			nextIndex = currentIndex[i+1] ;
		else
			nextIndex = currentIndex[0] ;

		if( CurrenDirection( direction ) == DIRECTION_Z || CurrenDirection( direction ) == DIRECTION_Z_MINUS )	// Z direction
		{
			if( curIndex.x >= m_localDomainIndex[idSolver].startX && curIndex.x <= m_localDomainIndex[idSolver].endX + 1 &&
				curIndex.y >= m_localDomainIndex[idSolver].startY && curIndex.y <= m_localDomainIndex[idSolver].endY + 1 &&
				nextIndex.x >= m_localDomainIndex[idSolver].startX && nextIndex.x <= m_localDomainIndex[idSolver].endX + 1 &&
				nextIndex.y >= m_localDomainIndex[idSolver].startY && nextIndex.y <= m_localDomainIndex[idSolver].endY + 1 &&
				verticalIndex >= m_localDomainIndex[idSolver].startZ && verticalIndex <= m_localDomainIndex[idSolver].endZ )
			{
				pathSingle.hStart = curIndex.x ;
				pathSingle.hEnd = nextIndex.x ;

				pathSingle.vStart = curIndex.y ;
				pathSingle.vEnd = nextIndex.y ;


				path.push_back( pathSingle ) ;
			}
		}
		else if( CurrenDirection( direction ) == DIRECTION_X || CurrenDirection( direction ) == DIRECTION_X_MINUS )	// x direction
		{
			if( curIndex.x >= m_localDomainIndex[idSolver].startY && curIndex.x <= m_localDomainIndex[idSolver].endY + 1 &&
				curIndex.y >= m_localDomainIndex[idSolver].startZ && curIndex.y <= m_localDomainIndex[idSolver].endZ + 1 &&
				nextIndex.x >= m_localDomainIndex[idSolver].startY && nextIndex.x <= m_localDomainIndex[idSolver].endY + 1 &&
				nextIndex.y >= m_localDomainIndex[idSolver].startZ && nextIndex.y <= m_localDomainIndex[idSolver].endZ + 1  &&
				verticalIndex >= m_localDomainIndex[idSolver].startX && verticalIndex <= m_localDomainIndex[idSolver].endX )
			{
				pathSingle.hStart = curIndex.x ;
				pathSingle.hEnd = nextIndex.x ;

				pathSingle.vStart = curIndex.y ;
				pathSingle.vEnd = nextIndex.y ;


				path.push_back( pathSingle ) ;
			}
		}
		else if( CurrenDirection( direction ) == DIRECTION_Y || CurrenDirection( direction ) == DIRECTION_Y_MINUS )	// y direction
		{
			if( curIndex.x >= m_localDomainIndex[idSolver].startZ && curIndex.x <= m_localDomainIndex[idSolver].endZ + 1 &&
				curIndex.y >= m_localDomainIndex[idSolver].startX && curIndex.y <= m_localDomainIndex[idSolver].endX + 1 &&
				nextIndex.x >= m_localDomainIndex[idSolver].startZ && nextIndex.x <= m_localDomainIndex[idSolver].endZ + 1 &&
				nextIndex.y >= m_localDomainIndex[idSolver].startX && nextIndex.y <= m_localDomainIndex[idSolver].endX + 1  &&
				verticalIndex >= m_localDomainIndex[idSolver].startY && verticalIndex <= m_localDomainIndex[idSolver].endY )
			{
				pathSingle.hStart = curIndex.x ;
				pathSingle.hEnd = nextIndex.x ;

				pathSingle.vStart = curIndex.y ;
				pathSingle.vEnd = nextIndex.y ;


				path.push_back( pathSingle ) ;
			}
		}
	}


	m_cellNum = static_cast<int> ( path.size() ) ;
	if( m_cellNum != 0 )
	{
		m_insideFlag = true ;

		m_path = new CCurrentPath[m_cellNum] ;
		for( i = 0 ; i < m_cellNum ; i++ )
			m_path[i] = path[i] ;

	}

	//int verticalIndex ;
	//int vertexNum ;
	//int direction ;

	int lx1, ly1, lz1 ;
	m_dir = static_cast<CurrenDirection> (direction ) ;
	switch( m_dir )
	{
		case DIRECTION_X :
		case DIRECTION_X_MINUS :
			Global2Local( verticalIndex, 0, 0, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			m_verticalIndex = lx1 ;
			for( i = 0 ; i < m_cellNum ; i++ )
			{
				Global2Local( verticalIndex, m_path[i].hStart , m_path[i].vStart , lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
				m_path[i].hStart = ly1 ; m_path[i].vStart = lz1 ;

				Global2Local( verticalIndex, m_path[i].hEnd , m_path[i].vEnd , lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
				m_path[i].hEnd = ly1 ; m_path[i].vEnd = lz1 ;
			}

			break ;
		case DIRECTION_Y :
		case DIRECTION_Y_MINUS :
			Global2Local( 0,verticalIndex,  0, lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			m_verticalIndex = ly1 ;

			for( i = 0 ; i < m_cellNum ; i++ )
			{
				Global2Local( m_path[i].vStart, verticalIndex , m_path[i].hStart ,   lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
				m_path[i].hStart = lz1 ; m_path[i].vStart = lx1 ;

				Global2Local( m_path[i].vEnd, verticalIndex , m_path[i].hEnd , lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
				m_path[i].hEnd = lz1 ; m_path[i].vEnd = lx1 ;
			}
			break ;
		case DIRECTION_Z :
		case DIRECTION_Z_MINUS :
			Global2Local( 0, 0,verticalIndex,  lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
			m_verticalIndex = lz1 ;
			for( i = 0 ; i < m_cellNum ; i++ )
			{
				Global2Local( m_path[i].hStart , m_path[i].vStart ,verticalIndex,  lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
				m_path[i].hStart = lx1 ; m_path[i].vStart = ly1 ;

				Global2Local( m_path[i].hEnd , m_path[i].vEnd ,verticalIndex,  lx1, ly1, lz1 , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;
				m_path[i].hEnd = lx1 ; m_path[i].vEnd = ly1 ;
			}

			break ;
	}


	delete[] currentIndex ;
	path.clear() ;

	return FDTD_SUCCESS ;
}


void CCurrent_Source::update_H_Field(int n, float*** (&Hx), float*** (&Hy), float*** (&Hz), int sourceIndex, CFarField *farField )
{
	int i ;
	float H_J_density, H_J_density_sum ;
	float temp ;

	//int x1, y1, z1, x2, y2, z2 ;

	int lx1, lx2, ly1, ly2, lz1, lz2 ;

	float Coeffhxp,Coeffhyp,Coeffhzp, temp_field, temp_field_sum ;

	if( id == 0 )
	{
		int inFlag = 0 , inFlagSum = 0 ;

		MPI_Reduce( &inFlag, &inFlagSum, 1 , MPI_INT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Reduce( &temp_field, &temp_field_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Reduce( &H_J_density, &H_J_density_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Barrier( MPI_COMM_WORLD ) ;

		H_J_density_sum /= inFlagSum ;

		float exp_h_real, exp_h_imag ;
		int freqIndex ;
		if( farField != NULL )
		{
			if( farField->getEnabled() )
			{
				float sourceReal, sourceImag, powerReal, powerImag ;

				for ( freqIndex = 0 ; freqIndex < farField->getFreqNum() ; freqIndex++)  //incident power DFT
				{
					exp_h_real =   cos(2 * PI * farField->getFreq( freqIndex ) * ( n ) * m_Dt ) ;
					exp_h_imag = - sin(2 * PI * farField->getFreq( freqIndex ) * ( n ) * m_Dt ) ;

					farField->getSourcePower( sourceReal, sourceImag, powerReal, powerImag , freqIndex, sourceIndex ) ;
					
					powerReal += exp_h_real * temp_field_sum ;
					powerImag += exp_h_imag * temp_field_sum ;
					sourceReal += exp_h_real * H_J_density_sum ;
					sourceImag += exp_h_imag * H_J_density_sum ;

					farField->setSourcePower( sourceReal, sourceImag, powerReal, powerImag , freqIndex, sourceIndex ) ;

				}
			}
		}
	}
	else
	{
		CCurrentPath *path = m_path ;

		if( m_insideFlag == true )
			H_J_density = m_current * Excitation_Source( n, m_delay , m_pulse, m_pulseLen, m_Dt ) ;
		else
			H_J_density = 0 ;
		
		temp_field = 0.0 ;
	

		if( m_insideFlag )
		{
			for( i = 0 ; i < m_cellNum ; i++ )
			{

				if( m_dir == DIRECTION_Z )
				{
					lx1 = path->hStart ;
					ly1 = path->vStart ;

					lx2 = path->hEnd ;
					ly2 = path->vEnd ;
				
					lz1 = lz2 = m_verticalIndex - 1 ;


					if (lx1 < lx2) 
					{
						Coeffhxp = m_Dt * Mu0_Inv;
						

						temp = Hx[lx1][ly1 - 1][lz1] * m_Dx_Half[lx1] * m_Dy_Grid[ly1 - 1] * m_Dz_Grid[lz1] ;
						
						// if current cell in across boundary, multiply it by 0.5f
						if( ( lx1 == m_Index_E_Boundary[XMIN] && lx2 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMIN] + 1 && lx2 == m_Index_E_Boundary[XMIN] ) ||
							( lx1 == m_Index_E_Boundary[XMAX] && lx2 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMAX] + 1 && lx2 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hx[lx1][ly1 - 1][lz1] += Coeffhxp * H_J_density ;


					} 
					else if (lx1 > lx2)
					{
						Coeffhxp = m_Dt * Mu0_Inv ;
						

						temp = Hx[lx2][ly1 - 1][lz1] * m_Dx_Half[lx2] * m_Dy_Grid[ly1 - 1] * m_Dz_Grid[lz1]  ;
						
						if( ( lx2 == m_Index_E_Boundary[XMIN] && lx1 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx2 == m_Index_E_Boundary[XMIN] + 1 && lx1 == m_Index_E_Boundary[XMIN] ) ||
							( lx2 == m_Index_E_Boundary[XMAX] && lx1 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx2 == m_Index_E_Boundary[XMAX] + 1 && lx1 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;
						
					
						Hx[lx2][ly1 - 1][lz1] -= Coeffhxp * H_J_density ;

					}

					if (ly1 < ly2) 
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						

						temp = Hy[lx1 - 1][ly1][lz1] * m_Dx_Half[lx1 - 1] * m_Dy_Grid[ly1] * m_Dz_Grid[lz1] ;
						
						if( ( ly1 == m_Index_E_Boundary[YMIN] && ly2 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly1 == m_Index_E_Boundary[YMIN] + 1 && ly2 == m_Index_E_Boundary[YMIN] ) ||
							( ly1 == m_Index_E_Boundary[YMAX] && ly2 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly1 == m_Index_E_Boundary[YMAX] + 1 && ly2 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hy[lx1 - 1][ly1][lz1] += Coeffhyp * H_J_density ;


					} else if (ly1 > ly2)
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						

						temp = Hy[lx1 - 1][ly2][lz1] * m_Dx_Grid[lx1 - 1] * m_Dy_Half[ly2] * m_Dz_Grid[lz1] ;
						
						if( ( ly2 == m_Index_E_Boundary[YMIN] && ly1 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMIN] + 1 && ly1 == m_Index_E_Boundary[YMIN] ) ||
							( ly2 == m_Index_E_Boundary[YMAX] && ly1 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMAX] + 1 && ly1 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hy[lx1 - 1][ly2][lz1] -= Coeffhyp * H_J_density ;


					}

				} 	
				else if( m_dir == DIRECTION_Y )
				{
					lz1 = path->hStart ;
					lx1 = path->vStart ;

					lz2 = path->hEnd ;
					lx2 = path->vEnd ;
					
					ly1 = ly2 = m_verticalIndex - 1 ;

					if (lx1 < lx2) 
					{
						Coeffhxp = m_Dt * Mu0_Inv ;

						temp = Hx[lx1][ly1][lz1 - 1] * m_Dx_Half[lx1] * m_Dy_Grid[ly1] * m_Dz_Grid[lz1 - 1]  ;
						
						if( ( lx1 == m_Index_E_Boundary[XMIN] && lx2 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMIN] + 1 && lx2 == m_Index_E_Boundary[XMIN] ) ||
							( lx1 == m_Index_E_Boundary[XMAX] && lx2 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMAX] + 1 && lx2 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hx[lx1][ly1][lz1 - 1] += Coeffhxp * H_J_density ;


					} else if (lx1 > lx2)
					{
						Coeffhxp = m_Dt * Mu0_Inv ;
						
						temp = Hx[lx2][ly1][lz1 - 1] * m_Dx_Half[lx2] * m_Dy_Grid[ly1] * m_Dz_Grid[lz1 - 1] ;
						
						if( ( lx2 == m_Index_E_Boundary[XMIN] && lx1 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx2 == m_Index_E_Boundary[XMIN] + 1 && lx1 == m_Index_E_Boundary[XMIN]  ) ||
							( lx2 == m_Index_E_Boundary[XMAX] && lx1 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx2 == m_Index_E_Boundary[XMAX] + 1 && lx1 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hx[lx2][ly1][lz1 - 1] -= Coeffhxp * H_J_density ;


					}

					if (lz1 < lz2) 
					{
						Coeffhzp = m_Dt * Mu0_Inv ;

						temp = Hz[lx1 - 1][ly1][lz1] * m_Dx_Grid[lx1 - 1] * m_Dy_Grid[ly1] * m_Dz_Half[lz1] ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1 - 1][ly1][lz1] += Coeffhzp * H_J_density ;


					} else if (lz1 > lz2)
					{
						Coeffhzp = m_Dt * Mu0_Inv ;
						

						temp = Hz[lx1 - 1][ly1][lz2] * m_Dx_Grid[lx1 - 1] * m_Dy_Grid[ly1] * m_Dz_Half[lz2]  ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1 - 1][ly1][lz2] -= Coeffhzp * H_J_density ;
					}

				}
				else  if( m_dir == DIRECTION_X )
				{
					ly1 = path->hStart ;
					lz1 = path->vStart ;

					ly2 = path->hEnd ;
					lz2 = path->vEnd ;

					lx1 = lx2 = m_verticalIndex - 1;


					if (ly1 < ly2) 
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						

						temp = Hy[lx1][ly1][lz1 - 1] * m_Dx_Grid[lx1] * m_Dy_Half[ly1] * m_Dz_Grid[lz1 - 1] ;
						
						if( ( ly2 == m_Index_E_Boundary[YMIN] && ly1 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMIN] + 1 && ly1 == m_Index_E_Boundary[YMIN] ) ||
							( ly2 == m_Index_E_Boundary[YMAX] && ly1 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMAX] + 1 && ly1 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hy[lx1][ly1][lz1 - 1] += Coeffhyp * H_J_density ;

					} 
					else if (ly1 > ly2)
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						

						temp = Hy[lx1][ly2][lz1 - 1] * m_Dx_Grid[lx1] * m_Dy_Half[ly2] * m_Dz_Grid[lz1 - 1] ;
						
						if( ( ly2 == m_Index_E_Boundary[YMIN] && ly1 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMIN] + 1 && ly1 == m_Index_E_Boundary[YMIN] ) ||
							( ly2 == m_Index_E_Boundary[YMAX] && ly1 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMAX] + 1 && ly1 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hy[lx1][ly2][lz1 - 1] -= Coeffhyp * H_J_density ;

					}

					if (lz1 < lz2) 
					{
						Coeffhzp = m_Dt * Mu0_Inv ;
						
						temp = Hz[lx1][ly1 - 1][lz1] * m_Dx_Grid[lx1] * m_Dy_Grid[ly1 - 1] * m_Dz_Half[lz1] ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1][ly1 - 1][lz1] += Coeffhzp * H_J_density ;

					} 
					else if (lz1 > lz2)
					{
						Coeffhzp = m_Dt * Mu0_Inv ;
						
						temp = Hz[lx1][ly1 - 1][lz2] * m_Dx_Grid[lx1] * m_Dy_Grid[ly1 - 1] * m_Dz_Half[lz2] ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1][ly1 - 1][lz2] -= Coeffhzp * H_J_density ;
					}

				}
				else  if( m_dir == DIRECTION_Z_MINUS )
				{

					lx1 = path->hStart ;
					ly1 = path->vStart ;

					lx2 = path->hEnd ;
					ly2 = path->vEnd ;

					lz1 = lz2 = m_verticalIndex - 1 ;
				

					if (lx1 < lx2) 
					{
						Coeffhxp = m_Dt * Mu0_Inv;
						
						
						temp = Hx[lx1][ly1 - 1][lz1] * m_Dx_Half[lx1] * m_Dy_Grid[ly1 - 1] * m_Dz_Grid[lz1] ;
						
						if( ( lx1 == m_Index_E_Boundary[XMIN] && lx2 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMIN] + 1 && lx2 == m_Index_E_Boundary[XMIN] ) ||
							( lx1 == m_Index_E_Boundary[XMAX] && lx2 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMAX] + 1 && lx2 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hx[lx1][ly1 - 1][lz1] += Coeffhxp * H_J_density ;

					} 
					else if (lx1 > lx2)
					{
						Coeffhxp = m_Dt * Mu0_Inv ;
						

						temp = Hx[lx2][ly1 - 1][lz1] * m_Dx_Half[lx2] * m_Dy_Grid[ly1 - 1] * m_Dz_Grid[lz1]  ;
						
						if( ( lx1 == m_Index_E_Boundary[XMIN] && lx2 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMIN] + 1 && lx2 == m_Index_E_Boundary[XMIN] ) ||
							( lx1 == m_Index_E_Boundary[XMAX] && lx2 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMAX] + 1 && lx2 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hx[lx2][ly1 - 1][lz1] -= Coeffhxp * H_J_density ;
					}

					if (ly1 < ly2) 
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						

						temp = Hy[lx1 - 1][ly1][lz1] * m_Dx_Grid[lx1 - 1] * m_Dy_Half[ly1] * m_Dz_Grid[lz1] ;
						
						if( ( ly1 == m_Index_E_Boundary[YMIN] && ly2 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly1 == m_Index_E_Boundary[YMIN] + 1 && ly2 == m_Index_E_Boundary[YMIN] ) ||
							( ly1 == m_Index_E_Boundary[YMAX] && ly2 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly1 == m_Index_E_Boundary[YMAX] + 1 && ly2 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hy[lx1 - 1][ly1][lz1] += Coeffhyp * H_J_density ;

					} 
					else if (ly1 > ly2)
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						

						temp = Hy[lx1 - 1][ly2][lz1] * m_Dx_Grid[lx1 - 1] * m_Dy_Half[ly2] * m_Dz_Grid[lz1] ;
						
						if( ( ly1 == m_Index_E_Boundary[YMIN] && ly2 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly1 == m_Index_E_Boundary[YMIN] + 1 && ly2 == m_Index_E_Boundary[YMIN] ) ||
							( ly1 == m_Index_E_Boundary[YMAX] && ly2 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly1 == m_Index_E_Boundary[YMAX] + 1 && ly2 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hy[lx1 - 1][ly2][lz1] -= Coeffhyp * H_J_density ;
					}
				}
				else if( m_dir == DIRECTION_Y_MINUS )
				{

					lz1 = path->hStart ;
					lx1 = path->vStart ;

					lz2 = path->hEnd ;
					lx2 = path->vEnd ;
					
					ly1 = ly2 = m_verticalIndex - 1;

					
					if (lx1 < lx2) 
					{
						Coeffhxp = m_Dt * Mu0_Inv ;
						

						temp = Hx[lx1][ly1][lz1 - 1] * m_Dx_Half[lx1] * m_Dy_Grid[ly1] * m_Dz_Grid[lz1 - 1] ;
						
						if( ( lx1 == m_Index_E_Boundary[XMIN] && lx2 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMIN] + 1 && lx2 == m_Index_E_Boundary[XMIN] ) ||
							( lx1 == m_Index_E_Boundary[XMAX] && lx2 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMAX] + 1 && lx2 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hx[lx1][ly1][lz1 - 1] += Coeffhxp * H_J_density ;
					} 
					else if (lx1 > lx2)
					{
						Coeffhxp = m_Dt * Mu0_Inv ;
						
						temp = Hx[lx2][ly1][lz1 - 1] * m_Dx_Half[lx2] * m_Dy_Grid[ly1] * m_Dz_Grid[lz1 - 1] ;
						
						if( ( lx1 == m_Index_E_Boundary[XMIN] && lx2 == m_Index_E_Boundary[XMIN] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMIN] + 1 && lx2 == m_Index_E_Boundary[XMIN] ) ||
							( lx1 == m_Index_E_Boundary[XMAX] && lx2 == m_Index_E_Boundary[XMAX] + 1 ) || 
							( lx1 == m_Index_E_Boundary[XMAX] + 1 && lx2 == m_Index_E_Boundary[XMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;

						Hx[lx2][ly1][lz1 - 1] -= Coeffhxp * H_J_density ;
					}

					if (lz1 < lz2) 
					{
						Coeffhzp = m_Dt * Mu0_Inv ;
						

						temp = Hy[lx1 - 1][ly2][lz1] * m_Dx_Grid[lx1 - 1] * m_Dy_Grid[ly1] * m_Dz_Half[lz1] ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1 - 1][ly1][lz1] += Coeffhzp * H_J_density ;
					} 
					else if (lz1 > lz2)
					{
						Coeffhzp = m_Dt * Mu0_Inv ;
						
						
						temp = Hz[lx1 - 1][ly1][lz2] * m_Dx_Grid[lx1 - 1] * m_Dy_Grid[ly1] * m_Dz_Half[lz2] ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1 - 1][ly1][lz2] -= Coeffhzp * H_J_density ;
					}
				}
				else if( m_dir == DIRECTION_X_MINUS )
				{
					ly1 = path->hStart ;
					lz1 = path->vStart ;

					ly2 = path->hEnd ;
					lz2 = path->vEnd ;

					lx1 = lx2 = m_verticalIndex - 1;

					if (ly1 < ly2) 
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						
					
						temp = Hy[lx1][ly1][lz1 - 1] * m_Dx_Grid[lx1] * m_Dy_Half[ly1] * m_Dz_Grid[lz1 - 1] ;
						
						if( ( ly2 == m_Index_E_Boundary[YMIN] && ly1 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMIN] + 1 && ly1 == m_Index_E_Boundary[YMIN] ) ||
							( ly2 == m_Index_E_Boundary[YMAX] && ly1 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMAX] + 1 && ly1 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hy[lx1][ly1][lz1 - 1] += Coeffhyp * H_J_density ;

					} 
					else if (ly1 > ly2)
					{
						Coeffhyp = m_Dt * Mu0_Inv ;
						
						temp = Hy[lx1][ly2][lz1 - 1] * m_Dx_Grid[lx1] * m_Dy_Half[ly2] * m_Dz_Grid[lz1 - 1] ;
						
						if( ( ly2 == m_Index_E_Boundary[YMIN] && ly1 == m_Index_E_Boundary[YMIN] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMIN] + 1 && ly1 == m_Index_E_Boundary[YMIN] ) ||
							( ly2 == m_Index_E_Boundary[YMAX] && ly1 == m_Index_E_Boundary[YMAX] + 1 ) || 
							( ly2 == m_Index_E_Boundary[YMAX] + 1 && ly1 == m_Index_E_Boundary[YMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hy[lx1][ly2][lz1 - 1] -= Coeffhyp * H_J_density ;
					}

					if (lz1 < lz2) 
					{
						Coeffhzp = m_Dt * Mu0_Inv ;
						

						temp = Hz[lx1][ly1 - 1][lz1] * m_Dx_Grid[lx1] * m_Dy_Grid[ly1 - 1] * m_Dz_Half[lz1] ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1][ly1 - 1][lz1] += Coeffhzp * H_J_density ;

					} 
					else if (lz1 > lz2)
					{
						Coeffhzp = m_Dt * Mu0_Inv ;

						temp = Hz[lx1][ly1 - 1][lz2] * m_Dx_Grid[lx1] * m_Dy_Grid[ly1 - 1] * m_Dz_Half[lz2] ;
						
						if( ( lz1 == m_Index_E_Boundary[ZMIN] && lz2 == m_Index_E_Boundary[ZMIN] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMIN] + 1 && lz2 == m_Index_E_Boundary[ZMIN] ) ||
							( lz1 == m_Index_E_Boundary[ZMAX] && lz2 == m_Index_E_Boundary[ZMAX] + 1 ) || 
							( lz1 == m_Index_E_Boundary[ZMAX] + 1 && lz2 == m_Index_E_Boundary[ZMAX] ) )
						{
							temp *= 0.5f ;
						}

						temp_field += temp ;


						Hz[lx1][ly1 - 1][lz2] -= Coeffhzp * H_J_density ;

					}

				}
				path++ ;
			}
			// end of for( i = 0 ; i < m_cellNum ; i++ )
		}// end of  if( m_insideFlag == true )

		int inFlag = 0 , inFlagSum = 0 ;
		if( m_insideFlag == true )
			inFlag = 1 ;
		else
			inFlag = 0 ;

		MPI_Reduce( &inFlag, &inFlagSum, 1 , MPI_INT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Reduce( &temp_field, &temp_field_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Reduce( &H_J_density, &H_J_density_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		MPI_Barrier( MPI_COMM_WORLD ) ;
	}
// end of while
}



