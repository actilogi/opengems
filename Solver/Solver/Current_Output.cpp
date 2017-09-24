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

#include "Current_Output.h"

#include "GEMS_Constant.h"
#include "FDTD_Common_Func.h"
#include "GEMS_Memory.h"

#include <mpi.h>
#include <cstdlib>
#include <vector>

CCurrent_Output::CCurrent_Output(void)
{
	m_cellNum = 0 ;

	m_verticalIndex = 1 ;
	m_path = NULL ;

	m_current = NULL ;

}

CCurrent_Output::~CCurrent_Output(void)
{
	next = NULL ;
	if( m_path != NULL )
	{
		delete[] m_path ;
		m_path = NULL ;
	}
	Free_1D( m_current ) ;

}

int CCurrent_Output::init(void)
{
	if( id == 0 )
	{
		m_current = Allocate_1D( m_current, m_timeStep ) ;
		if( m_current == NULL )
			return FDTD_NO_MEMORY ;
	}

	return FDTD_SUCCESS;
}

void CCurrent_Output::output( XML_Writer &outFile )
{
	std::string str ;
	char c_string[64] = "";
	outFile.AddAtributes( "objectName", m_objectName ) ;
	outFile.Createtag( "CurrentOutput" ) ;

		// write Position
		outFile.Createtag( "Position" ) ;

			// write PlaneIndex
			sprintf_s( c_string, "%d", m_verticalIndex ) ;
			str = c_string ;
			outFile.AddAtributes( "index", str ) ;

			outFile.Createtag( "PlaneIndex" ) ;
			outFile.CloseSingletag();
			// end of write PlaneIndex


			// write Direction
			sprintf_s( c_string, "%d", m_dir ) ;
			str = c_string ;
			outFile.AddAtributes( "direction", str ) ;

			outFile.Createtag( "Direction" ) ;
			outFile.CloseSingletag();
			// end of Direction

		outFile.CloseLasttag();
		// end of write Position

		// write voltage
		outFile.Createtag( "Currents" ) ;
		int i ;
		for( i = 0 ; i < m_timeStep ; i++ )
		{
			sprintf_s( c_string, "%e", m_current[i] ) ;
			str = c_string ;
			outFile.AddAtributes( "value", str ) ;
			outFile.Createtag( "Current" ) ;
			outFile.CloseSingletag();
		}

		outFile.CloseLasttag();
		// end of write voltage


	outFile.CloseLasttag();
}


void CCurrent_Output::collectResult(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	
	CCurrentOutPath *path = NULL ;
	
	// local index of path point
	int lx1, ly1, lz1, lx2, ly2, lz2 ;

	float current , sum_current ;
	int i ;

	current = 0.0f ;

	if( id == 0 )
	{
		MPI_Reduce( &current, &sum_current, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		m_current[n] = sum_current ;
	}
	else
	{
		path = m_path ;

		if( path != NULL )
		{
			for( i = 0 ; i < m_cellNum ; i++ )
			{
				// get start and end point of path
				if( m_dir == DIRECTION_X || m_dir == DIRECTION_X_MINUS )						// yz plane
				{
					//Assume the specified magnetic field position is above the electric field 
					lx1 = m_verticalIndex - 1;
					lx2 = m_verticalIndex;

					ly1 = path->hStart ;
					lz1 = path->vStart ;

					ly2 = path->hEnd ;
					lz2 = path->vEnd ;

					// transfer global index to local index
					//Global2Local( x1, y1, z1, lx1, ly1, lz1 ) ;
					//Global2Local( x2, y2, z2, lx2, ly2, lz2 ) ;


					float dx1, dx2 ;
					dx1 = m_X_Grid[lx2] - m_X_Half[lx1] ;
					dx2 = m_X_Half[lx2] - m_X_Grid[lx2] ;

					// path along y direction, in this case lz1 == lz2
					if( ly1 < ly2 )
					{
						if( m_dir == DIRECTION_X )
							current += ( Hy[lx1][ly1][lz1 - 1] * dx2 + Hy[lx2][ly1][lz2 - 1] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dy_Half[ly1] * path->factor ;
						else 
							current -= ( Hy[lx1][ly1][lz1 - 1] * dx2 + Hy[lx2][ly1][lz2 - 1] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dy_Half[ly1] * path->factor ;
					}
					else if( ly1 > ly2 )
					{
						if( m_dir == DIRECTION_X )
							current -= ( Hy[lx1][ly2][lz1 - 1] * dx2 + Hy[lx2][ly2][lz2 - 1] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dy_Half[ly2] * path->factor ;
						else
							current += ( Hy[lx1][ly2][lz1 - 1] * dx2 + Hy[lx2][ly2][lz2 - 1] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dy_Half[ly2] * path->factor ;
					}

					// path along z directon, in this case ly1 == ly2
					if( lz1 < lz2 )
					{
						if( m_dir == DIRECTION_X )
							current += ( Hz[lx1][ly1 - 1][lz1] * dx2 + Hz[lx2][ly2 - 1][lz1] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dz_Half[lz1] * path->factor ;
						else 
							current -= ( Hz[lx1][ly1 - 1][lz1] * dx2 + Hz[lx2][ly2 - 1][lz1] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dz_Half[lz1] * path->factor ;
					}
					else if( lz1 > lz2 )
					{
						if( m_dir == DIRECTION_X )
							current -= ( Hz[lx1][ly1 - 1][lz2] * dx2 + Hz[lx2][ly2 - 1][lz2] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dz_Half[lz2] * path->factor ;
						else
							current += ( Hz[lx1][ly1 - 1][lz2] * dx2 + Hz[lx2][ly2 - 1][lz2] * dx1 ) * m_Dx_Half_Inv[lx2] * m_Dz_Half[lz2] * path->factor ;
					}
				}
				else if( m_dir == DIRECTION_Y || m_dir == DIRECTION_Y_MINUS )						// zx plane
				{
					ly1 = m_verticalIndex - 1 ;
					ly2 = m_verticalIndex ;

					lz1 = path->hStart ;
					lx1 = path->vStart ;

					lz2 = path->hEnd ;
					lx2 = path->vEnd ;

					// transfer global index to local index
					//Global2Local( x1, y1, z1, lx1, ly1, lz1 ) ;
					//Global2Local( x2, y2, z2, lx2, ly2, lz2 ) ;

					float dy1, dy2 ;

					dy1 = m_Y_Grid[ly2] - m_Y_Half[ly1] ;
					dy2 = m_Y_Half[ly2] - m_Y_Grid[ly2] ;

					// path along z direction, in this case lx1 == lx2
					if( lz1 < lz2 )
					{
						if( m_dir == DIRECTION_Y )
							current += ( Hz[lx1 - 1][ly1][lz1] * dy2 + Hz[lx2 - 1][ly2][lz1] * dy1 ) * m_Dy_Half_Inv[ly1] * m_Dz_Half[lz1] * path->factor ;
						else 
							current -= ( Hz[lx1 - 1][ly1][lz1] * dy2 + Hz[lx2 - 1][ly2][lz1] * dy1 ) * m_Dy_Half_Inv[ly1] * m_Dz_Half[lz1] * path->factor ;
					}
					else if( lz1 > lz2 )
					{
						if( m_dir == DIRECTION_Y )
							current -= ( Hz[lx1 - 1][ly2][lz2] * dy2 + Hz[lx2 - 1][ly2][lz2] * dy1 ) * m_Dy_Half_Inv[ly2] * m_Dz_Half[lz2] * path->factor ;
						else
							current += ( Hz[lx1 - 1][ly2][lz2] * dy2 + Hz[lx2 - 1][ly2][lz2] * dy1 ) * m_Dy_Half_Inv[ly2] * m_Dz_Half[lz2] * path->factor ;
					}

					// path along x directon, in this case lz1 == lz2
					if( lx1 < lx2 )
					{
						if( m_dir == DIRECTION_Y )
							current += ( Hx[lx1][ly1][lz1 - 1] * dy2 + Hx[lx1][ly2][lz1 - 1] * dy1 ) * m_Dy_Half_Inv[ly1] * m_Dx_Half[lx1] * path->factor ;
						else 
							current -= ( Hx[lx1][ly1][lz1 - 1] * dy2 + Hx[lx1][ly2][lz1 - 1] * dy1 ) * m_Dy_Half_Inv[ly1] * m_Dx_Half[lx1] * path->factor ;
					}
					else if( lx1 > lx2 )
					{
						if( m_dir == DIRECTION_Y )
							current -= ( Hx[lx2][ly1][lz1 - 1] * dy2 + Hx[lx2 + 1][ly2][lz2 - 1] * dy1 ) * m_Dy_Half_Inv[ly2] * m_Dx_Half[lx2] * path->factor ;
						else
							current += ( Hx[lx2][ly1][lz1 - 1] * dy2 + Hx[lx2 + 1][ly2][lz2 - 1] * dy1 ) * m_Dy_Half_Inv[ly2] * m_Dx_Half[lx2] * path->factor ;
					}
				}
				else if( m_dir == DIRECTION_Z || m_dir == DIRECTION_Z_MINUS )						// xy plane
				{

					lx1 = path->hStart ;
					ly1 = path->vStart ;

					lx2 = path->hEnd ;
					ly2 = path->vEnd ;

					lz1 = m_verticalIndex - 1 ;
					lz2 = m_verticalIndex ;

					// transfer global index to local index
					//Global2Local( x1, y1, z1, lx1, ly1, lz1 ) ;
					//Global2Local( x2, y2, z2, lx2, ly2, lz2 ) ;
					

					float dz1, dz2 ;

					dz1 = m_Z_Grid[lz2] - m_Z_Half[lz1] ;
					dz2 = m_Z_Half[lz2] - m_Z_Grid[lz2] ;

					// path along x direction, in this case ly1 == ly2
					if( lx1 < lx2 )
					{

						if( m_dir == DIRECTION_Z )
							current += ( Hx[lx1][ly1 - 1][lz1] * dz2 + Hx[lx1][ly2 - 1][lz2] * dz1 ) * m_Dz_Half_Inv[lz1] * m_Dx_Half[lx1] * path->factor ;
						else 
							current -= ( Hx[lx1][ly1 - 1][lz1] * dz2 + Hx[lx1][ly2 - 1][lz2] * dz1 ) * m_Dz_Half_Inv[lz1] * m_Dx_Half[lx1] * path->factor ;
					}
					else if( lx1 > lx2 )
					{
						if( m_dir == DIRECTION_Z )
							current -= ( Hx[lx2][ly1 - 1][lz1] * dz2 + Hx[lx2][ly2 - 1][lz2] * dz1 ) * m_Dz_Half_Inv[lz2] * m_Dx_Half[lx2] * path->factor ;
						else
							current += ( Hx[lx2][ly1 - 1][lz1] * dz2 + Hx[lx2][ly2 - 1][lz2] * dz1 ) * m_Dz_Half_Inv[lz2] * m_Dx_Half[lx2] * path->factor ;
					}

					// path along y directon, in this case lx1 == lx2
					if( ly1 < ly2 )
					{
						if( m_dir == DIRECTION_Z )
							current += ( Hy[lx1 - 1][ly1][lz1] * dz2 + Hy[lx2 - 1][ly1][lz2] * dz1 ) * m_Dz_Half_Inv[lz1] * m_Dy_Half[ly1] * path->factor ;
						else 
							current -= ( Hy[lx1 - 1][ly1][lz1] * dz2 + Hy[lx2 - 1][ly1][lz2] * dz1 ) * m_Dz_Half_Inv[lz1] * m_Dy_Half[ly1] * path->factor ;
					}
					else if( ly1 > ly2 )
					{
						if( m_dir == DIRECTION_Z )
							current -= ( Hy[lx1 - 1][ly2][lz1] * dz2 + Hy[lx2 - 1][ly2][lz2] * dz1 ) * m_Dz_Half_Inv[lz2] * m_Dy_Half[ly2] * path->factor ;
						else
							current += ( Hy[lx1 - 1][ly2][lz1] * dz2 + Hy[lx2 - 1][ly2][lz2] * dz1 ) * m_Dz_Half_Inv[lz2] * m_Dy_Half[ly2] * path->factor ;
					}
				}

				path++ ;
			} // end of for( i = 0 ; i < ...........
		
		}
		// end of if( path != NULL )


		MPI_Reduce( &current, &sum_current, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
		//MPI_Barrier( MPI_COMM_WORLD ) ;

	}

}

int CCurrent_Output::readIn(FILE *in)
{
	struct CurrentIndex{
		int x ;
		int y ;
	} ;

	int vertexNum ;
	char direction ;
	CurrentIndex *currentIndex = NULL ;
	char endMark[2] ;

	char name[MAX_OUTPUTNAME_LENGTH+1] ;
	fread( name, sizeof( char ), MAX_OUTPUTNAME_LENGTH, in ) ;
	name[MAX_OUTPUTNAME_LENGTH] = 0 ;
	m_objectName = name ;

	fread( &direction, sizeof( char ), 1, in ) ;
	fread( &m_verticalIndex, sizeof( int ), 1, in ) ;

	fread( &vertexNum, sizeof( int ), 1, in ) ;

	currentIndex = new CurrentIndex[vertexNum] ;
	// read in polygon pathSingle indexes , the indexes are colck wise 
	// after read in, we need to change them into cell index pair

	fread( currentIndex, sizeof( CurrentIndex ), vertexNum, in ) ;


	fread( endMark, sizeof( char ), 2, in ) ;

	MPI_Bcast( &direction , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_verticalIndex , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	
	MPI_Bcast( &vertexNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( currentIndex , vertexNum * 2 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	
	m_dir = static_cast<CurrenDirection> (direction ) ;

	delete[] currentIndex ;

	return FDTD_SUCCESS ;
}


int CCurrent_Output::readIn_Solver( )
{
	struct CurrentIndex{
		int x ;
		int y ;
	} ;

	int i ;
	int verticalIndex ;
	int vertexNum ;
	char direction ;
	CurrentIndex *currentIndex = NULL ;

	MPI_Bcast( &direction , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &verticalIndex , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	
	MPI_Bcast( &vertexNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	currentIndex = new CurrentIndex[vertexNum] ;

	MPI_Bcast( currentIndex , vertexNum * 2 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

	std::vector< CCurrentOutPath > path ;
	CCurrentOutPath pathSingle ;

	CurrentIndex curIndex, nextIndex ;


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

				pathSingle.factor = 1.0f ;

				if( ( curIndex.x == nextIndex.x ) ) // this cell is along y directio
				{
					if( ( curIndex.y == m_localDomainIndex[idSolver].startY && nextIndex.y == m_localDomainIndex[idSolver].startY + 1 ) || 
						( curIndex.y == m_localDomainIndex[idSolver].startY + 1 && nextIndex.y == m_localDomainIndex[idSolver].startY ) ||
					    ( curIndex.y == m_localDomainIndex[idSolver].endY && nextIndex.y == m_localDomainIndex[idSolver].endY + 1 ) || 
					    ( curIndex.y == m_localDomainIndex[idSolver].endY + 1 && nextIndex.y == m_localDomainIndex[idSolver].endY ) )
					{
						pathSingle.factor *= 0.5 ;
					}
				}

				if( ( curIndex.y == nextIndex.y ) ) // this cell is along x directio
				{
					if( ( curIndex.x == m_localDomainIndex[idSolver].startX && nextIndex.x == m_localDomainIndex[idSolver].startX + 1 ) || 
						( curIndex.x == m_localDomainIndex[idSolver].startX + 1 && nextIndex.x == m_localDomainIndex[idSolver].startX ) ||
					    ( curIndex.x == m_localDomainIndex[idSolver].endX && nextIndex.x == m_localDomainIndex[idSolver].endX + 1 ) || 
					    ( curIndex.x == m_localDomainIndex[idSolver].endX + 1 && nextIndex.x == m_localDomainIndex[idSolver].endX ) )
					{
						pathSingle.factor *= 0.5 ;
					}
				}

				if( verticalIndex == m_localDomainIndex[idSolver].startZ || verticalIndex == m_localDomainIndex[idSolver].endZ )
					pathSingle.factor *= 0.5 ;

				if( ( curIndex.x == nextIndex.x ) && ( ( curIndex.x == m_localDomainIndex[idSolver].startX ) || ( curIndex.x == m_localDomainIndex[idSolver].endX + 1 )) )
					pathSingle.factor = 0.0 ;

				if( ( curIndex.y == nextIndex.y ) && ( ( curIndex.y == m_localDomainIndex[idSolver].startY ) || ( curIndex.y == m_localDomainIndex[idSolver].endY + 1 )) )
					pathSingle.factor = 0.0 ;

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

				pathSingle.factor = 1.0f ;

				if( ( curIndex.x == nextIndex.x ) ) // this cell is along Z directio
				{
					if( ( curIndex.y == m_localDomainIndex[idSolver].startZ && nextIndex.y == m_localDomainIndex[idSolver].startZ + 1 ) || 
						( curIndex.y == m_localDomainIndex[idSolver].startZ + 1 && nextIndex.y == m_localDomainIndex[idSolver].startZ ) ||
					    ( curIndex.y == m_localDomainIndex[idSolver].endZ && nextIndex.y == m_localDomainIndex[idSolver].endZ + 1 ) || 
					    ( curIndex.y == m_localDomainIndex[idSolver].endZ + 1 && nextIndex.y == m_localDomainIndex[idSolver].endZ ) )
					{
						pathSingle.factor *= 0.5 ;
					}
				}

				if( ( curIndex.y == nextIndex.y ) ) // this cell is along Y directio
				{
					if( ( curIndex.x == m_localDomainIndex[idSolver].startY && nextIndex.x == m_localDomainIndex[idSolver].startY + 1 ) || 
						( curIndex.x == m_localDomainIndex[idSolver].startY + 1 && nextIndex.x == m_localDomainIndex[idSolver].startY ) ||
					    ( curIndex.x == m_localDomainIndex[idSolver].endY && nextIndex.x == m_localDomainIndex[idSolver].endY + 1 ) || 
					    ( curIndex.x == m_localDomainIndex[idSolver].endY + 1 && nextIndex.x == m_localDomainIndex[idSolver].endY ) )
					{
						pathSingle.factor *= 0.5 ;
					}
				}

				if( verticalIndex == m_localDomainIndex[idSolver].startX || verticalIndex == m_localDomainIndex[idSolver].endX )
					pathSingle.factor *= 0.5 ;

				if( ( curIndex.x == nextIndex.x ) && ( ( curIndex.x == m_localDomainIndex[idSolver].startY ) || ( curIndex.x == m_localDomainIndex[idSolver].endY + 1 )) )
					pathSingle.factor = 0.0 ;

				if( ( curIndex.y == nextIndex.y ) && ( ( curIndex.y == m_localDomainIndex[idSolver].startZ ) || ( curIndex.y == m_localDomainIndex[idSolver].endZ + 1 )) )
					pathSingle.factor = 0.0 ;

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

				pathSingle.factor = 1.0f ;

				if( ( curIndex.x == nextIndex.x ) ) // this cell is along x directio
				{
					if( ( curIndex.y == m_localDomainIndex[idSolver].startX && nextIndex.y == m_localDomainIndex[idSolver].startX + 1 ) || 
						( curIndex.y == m_localDomainIndex[idSolver].startX + 1 && nextIndex.y == m_localDomainIndex[idSolver].startX ) ||
					    ( curIndex.y == m_localDomainIndex[idSolver].endX && nextIndex.y == m_localDomainIndex[idSolver].endX + 1 ) || 
					    ( curIndex.y == m_localDomainIndex[idSolver].endX + 1 && nextIndex.y == m_localDomainIndex[idSolver].endX ) )
					{
						pathSingle.factor *= 0.5 ;
					}
				}

				if( ( curIndex.y == nextIndex.y ) ) // this cell is along z directio
				{
					if( ( curIndex.x == m_localDomainIndex[idSolver].startZ && nextIndex.x == m_localDomainIndex[idSolver].startZ + 1 ) || 
						( curIndex.x == m_localDomainIndex[idSolver].startZ + 1 && nextIndex.x == m_localDomainIndex[idSolver].startZ ) ||
					    ( curIndex.x == m_localDomainIndex[idSolver].endZ && nextIndex.x == m_localDomainIndex[idSolver].endZ + 1 ) || 
					    ( curIndex.x == m_localDomainIndex[idSolver].endZ + 1 && nextIndex.x == m_localDomainIndex[idSolver].endZ ) )
					{
						pathSingle.factor *= 0.5 ;
					}
				}

				if( verticalIndex == m_localDomainIndex[idSolver].startY || verticalIndex == m_localDomainIndex[idSolver].endY )
					pathSingle.factor *= 0.5 ;
				
				if( ( curIndex.x == nextIndex.x ) && ( ( curIndex.x == m_localDomainIndex[idSolver].startZ ) || ( curIndex.x == m_localDomainIndex[idSolver].endZ + 1 )) )
					pathSingle.factor = 0.0 ;

				if( ( curIndex.y == nextIndex.y ) && ( ( curIndex.y == m_localDomainIndex[idSolver].startX ) || ( curIndex.y == m_localDomainIndex[idSolver].endX + 1 )) )
					pathSingle.factor = 0.0 ;

				path.push_back( pathSingle ) ;
			}
		}
	}


	m_cellNum = static_cast<int> ( path.size() ) ;
	if( m_cellNum != 0 )
	{
		m_path = new CCurrentOutPath[m_cellNum] ;
		for( i = 0 ; i < m_cellNum ; i++ )
			m_path[i] = path[i] ;

	}


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
