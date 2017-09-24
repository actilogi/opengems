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

#include "Field_Output.h"

#include "GEMS_Constant.h"
#include "FDTD_Common_Func.h"
#include "GEMS_Memory.h"

#include <mpi.h>
#include <cstdlib>

CField_Output::CField_Output(void)
: m_xIndex(0)
, m_yIndex(0)
, m_zIndex(0)
, m_outFlag(0x0)
, m_insideFlag(false)
{
	m_field = NULL ;
}

CField_Output::~CField_Output(void)
{
	next = NULL ;

	Free_2D( m_field ) ;
}

int CField_Output::init(void)
{
	if( id == 0 )
	{
		m_field = Allocate_2D( m_field, m_timeStep, 6 ) ;
		if( m_field == NULL )
			return FDTD_NO_MEMORY ;

	}
	return FDTD_SUCCESS;
}

void CField_Output::output( XML_Writer &outFile )
{
	std::string str ;
	char c_string[64] = "";
	outFile.AddAtributes( "objectName", m_objectName ) ;
	outFile.Createtag( "FieldAtPointOutput" ) ;

		// write Position
		sprintf_s( c_string, "%d", m_zIndex ) ;
		str = c_string ;
		outFile.AddAtributes( "zIndex", str ) ;

		sprintf_s( c_string, "%d", m_yIndex ) ;
		str = c_string ;
		outFile.AddAtributes( "yIndex", str ) ;

		sprintf_s( c_string, "%d", m_xIndex ) ;
		str = c_string ;
		outFile.AddAtributes( "xIndex", str ) ;

		outFile.Createtag( "Position" ) ;
		outFile.CloseLasttag();
		// end of write Position

		// write voltage
		outFile.Createtag( "Fields" ) ;
		int i ;
		for( i = 0 ; i < m_timeStep ; i++ )
		{
			sprintf_s( c_string, "%e", m_field[i][5] ) ;
			str = c_string ;
			outFile.AddAtributes( "Hz", str ) ;

			sprintf_s( c_string, "%e", m_field[i][4] ) ;
			str = c_string ;
			outFile.AddAtributes( "Hy", str ) ;

			sprintf_s( c_string, "%e", m_field[i][3] ) ;
			str = c_string ;
			outFile.AddAtributes( "Hx", str ) ;

			sprintf_s( c_string, "%e", m_field[i][2] ) ;
			str = c_string ;
			outFile.AddAtributes( "Ez", str ) ;

			sprintf_s( c_string, "%e", m_field[i][1] ) ;
			str = c_string ;
			outFile.AddAtributes( "Ey", str ) ;

			sprintf_s( c_string, "%e", m_field[i][0] ) ;
			str = c_string ;
			outFile.AddAtributes( "Ex", str ) ;

			outFile.Createtag( "Field" ) ;
			outFile.CloseSingletag();
		}

		outFile.CloseLasttag();
		// end of write voltage


	outFile.CloseLasttag();
}


void CField_Output::collectResult(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	int root = 0;

	float field[6] , totalField[6] ;
	int xEx , yEx , zEx ;
	int xEy , yEy , zEy ;
	int xEz , yEz , zEz ;

	int xHx , yHx , zHx ;
	int xHy , yHy , zHy ;
	int xHz , yHz , zHz ;

	if( id == 0 )
	{
		memset( field, 0 , sizeof( float ) * 6 ) ;
		MPI_Reduce( field , totalField , 6, MPI_FLOAT, MPI_SUM, root, MPI_COMM_WORLD ) ;
		int i ;
		for( i = 0 ; i < 6 ; i++ )
			m_field[n][i] = totalField[i] ;
	}
	else
	{
		// Ex
		if( m_outFlag & 0x02 )
		{
			xEx = m_xIndex - 1 ;
			yEx = m_yIndex ;
			zEx = m_zIndex ;
		}
		else
		{
			xEx = m_xIndex ;
			yEx = m_yIndex ;
			zEx = m_zIndex ;
		}

		//Ey
		if( m_outFlag & 0x0200 )
		{
			xEy = m_xIndex;
			yEy = m_yIndex - 1  ;
			zEy = m_zIndex ;
		}
		else
		{
			xEy = m_xIndex ;
			yEy = m_yIndex ;
			zEy = m_zIndex ;
		}

		// Ez
		if( m_outFlag & 0x020000 )
		{
			xEz = m_xIndex;
			yEz = m_yIndex ;
			zEz = m_zIndex - 1 ;
		}
		else
		{
			xEz = m_xIndex ;
			yEz = m_yIndex ;
			zEz = m_zIndex ;
		}

		// Hx
		if( m_outFlag & 0x02 )
		{
			xHx = m_xIndex - 1 ;
			yHx = m_yIndex ;
			zHx = m_zIndex ;
		}
		else
		{
			xHx = m_xIndex ;
			yHx = m_yIndex ;
			zHx = m_zIndex ;
		}

		//Hy
		if( m_outFlag & 0x0200 )
		{
			xHy = m_xIndex;
			yHy = m_yIndex - 1  ;
			zHy = m_zIndex ;
		}
		else
		{
			xHy = m_xIndex ;
			yHy = m_yIndex ;
			zHy = m_zIndex ;
		}

		// Hz
		if( m_outFlag & 0x020000 )
		{
			xHz = m_xIndex;
			yHz = m_yIndex ;
			zHz = m_zIndex - 1 ;
		}
		else
		{
			xHz = m_xIndex ;
			yHz = m_yIndex ;
			zHz = m_zIndex ;
		}
		// End of
		if( m_insideFlag )
		{
			field[0] = Ex[xEx][yEx][zEx] ; 
			field[1] = Ey[xEy][yEy][zEy] ; 
			field[2] = Ez[xEz][yEz][zEz] ; 
			field[3] = Hx[xHx][yHx][zHx] ; 
			field[4] = Hy[xHy][yHy][zHy] ; 
			field[5] = Hz[xHz][yHz][zHz] ; 
		}
		else
			memset( field, 0 , sizeof( float ) * 6 ) ;

		MPI_Reduce( field , totalField , 6, MPI_FLOAT, MPI_SUM, root, MPI_COMM_WORLD ) ;
	}
}

void CField_Output::getIndex(int& x, int& y, int& z)
{
	x = m_xIndex ;
	y = m_yIndex ;
	z = m_zIndex ;
}

void CField_Output::setIndex(int x, int y, int z)
{
	m_xIndex = x ;
	m_yIndex = y ;
	m_zIndex = z ;
}

// get output flag of the Field
long CField_Output::getOutFlag(void)
{
	return m_outFlag;
}

void CField_Output::setOutFlag(long flag)
{
	m_outFlag = flag ;
}


bool CField_Output::getInsideFlag(void)
{
	return m_insideFlag;
}

void CField_Output::setInsideFlag(bool flag)
{
	m_insideFlag = flag ;
}

int CField_Output::readIn(FILE *in)
{
	char endMark[2] ;

	char name[MAX_OUTPUTNAME_LENGTH+1] ;
	fread( name, sizeof( char ), MAX_OUTPUTNAME_LENGTH, in ) ;
	name[MAX_OUTPUTNAME_LENGTH] = 0 ;
	m_objectName = name ;

	fread( &m_outFlag, sizeof( long ), 1, in ) ;

	fread( &m_xIndex, sizeof( int ), 1, in ) ;
	fread( &m_yIndex, sizeof( int ), 1, in ) ;
	fread( &m_zIndex, sizeof( int ), 1, in ) ;

	fread( endMark, sizeof( char ), 2, in ) ;

	// broadcast field at point information
	MPI_Bcast( &m_outFlag, 1 , MPI_LONG, 0, MPI_COMM_WORLD ) ; 
	
	MPI_Bcast( &m_xIndex, 1 , MPI_INT, 0, MPI_COMM_WORLD ) ; 
	
	MPI_Bcast( &m_yIndex, 1 , MPI_INT, 0, MPI_COMM_WORLD ) ; 

	MPI_Bcast( &m_zIndex, 1 , MPI_INT, 0, MPI_COMM_WORLD ) ; 

	return FDTD_SUCCESS ;
}


int CField_Output::readIn_Solver( ) 
{
	int xIndex_local , yIndex_local , zIndex_local ;

	// broadcast field at point information
	MPI_Bcast( &m_outFlag, 1 , MPI_LONG, 0, MPI_COMM_WORLD ) ; 
	
	MPI_Bcast( &m_xIndex, 1 , MPI_INT, 0, MPI_COMM_WORLD ) ; 
	
	MPI_Bcast( &m_yIndex, 1 , MPI_INT, 0, MPI_COMM_WORLD ) ; 

	MPI_Bcast( &m_zIndex, 1 , MPI_INT, 0, MPI_COMM_WORLD ) ; 


	// check if the point is in current domain or not
	if( m_xIndex < m_localDomainIndex[idSolver].startX || ( m_xIndex >= m_localDomainIndex[idSolver].endX && numSolver > 1 ) ||
		m_yIndex < m_localDomainIndex[idSolver].startY || ( m_yIndex >= m_localDomainIndex[idSolver].endY && numSolver > 1 ) ||
		m_zIndex < m_localDomainIndex[idSolver].startZ || ( m_zIndex >= m_localDomainIndex[idSolver].endZ && numSolver > 1 ) )
	{
		m_insideFlag = false ;
	}
	else
		m_insideFlag = true ;

	// convert global index to local index
	Global2Local( m_xIndex, m_yIndex, m_zIndex, xIndex_local, yIndex_local, zIndex_local , m_localDomainIndex[idSolver] , m_boundaryLayerNum ) ;

	m_xIndex = xIndex_local ;
	m_yIndex = yIndex_local ;
	m_zIndex = zIndex_local ;

	return FDTD_SUCCESS ;
}
