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

#include "ParallelExchange.h"

#include "GEMS_Constant.h"
#include "GEMS_Memory.h"


CParallelExchange::CParallelExchange(void)
{
	m_nx = m_ny = m_nz = 1 ; 
	m_localDomainIndex = NULL ;
	for( int i = 0 ; i < 6 ; i++ )
	{
		m_neighbor[i] = -1 ;
		m_boundary[i] = BC_NONE ;
	}

	m_xType = MPI_FLOAT ;
	m_yType = MPI_FLOAT ;
	m_zType = MPI_FLOAT ;
}

CParallelExchange::~CParallelExchange(void)
{
}

void CParallelExchange::setCellNum(int nx, int ny, int nz)
{
	m_nx = nx ;
	m_ny = ny ;
	m_nz = nz ;
}

void CParallelExchange::setDomainIndex( DomainIndex *localDomainIndex ) 
{
	m_localDomainIndex = localDomainIndex ;
}

int CParallelExchange::init( ) 
{
	int i ;

	// create a new data type for m_yType
	MPI_Type_vector( m_nx + 1, m_nz + 1, ( m_ny + 1 ) * ( m_nz + 1 ) , MPI_FLOAT, &m_yType ) ;
	MPI_Type_commit( &m_yType ) ;


	// create a new data type for m_zType
	MPI_Datatype tempType ;
	int *blocklens = NULL ;
	MPI_Aint *indices = NULL ;
	MPI_Datatype *oldType = NULL ;

	MPI_Type_vector( m_ny + 1, 1, m_nz + 1 , MPI_FLOAT, &tempType ) ;
	MPI_Type_commit( &tempType ) ;


	blocklens = new int[m_nx + 1] ;
	indices = new MPI_Aint[m_nx + 1] ;
	oldType = new MPI_Datatype[m_nx + 1] ;

	for( i = 0 ; i < m_nx + 1 ; i++ )
	{
		blocklens[i] = 1 ;
		indices[i] = i * ( m_ny + 1 ) * ( m_nz + 1 ) * sizeof( float ) ;
		oldType[i] = tempType ;
	}

	MPI_Type_struct( m_nx + 1, blocklens, indices, oldType, &m_zType ) ;
	MPI_Type_commit( &m_zType ) ;

	// free type data type
	//MPI_Type_free( &tempType ) ;

	delete[] blocklens ;
	delete[] indices ;
	delete[] oldType ;

	// find the neighbor of current domain
	//	! Find the neighbors
	int n ;
	for( n = 0 ; n < numSolver ; n++ )
	{
		// neighbors in x direction
		if( m_localDomainIndex[n].startY == m_localDomainIndex[idSolver].startY &&
			m_localDomainIndex[n].endY == m_localDomainIndex[idSolver].endY &&
			m_localDomainIndex[n].startZ == m_localDomainIndex[idSolver].startZ &&
			m_localDomainIndex[n].endZ == m_localDomainIndex[idSolver].endZ )
		{
			if( m_localDomainIndex[n].endX == m_localDomainIndex[idSolver].startX )
				m_neighbor[XMIN] = n ; 
			else if( m_localDomainIndex[n].startX == m_localDomainIndex[idSolver].endX )
				m_neighbor[XMAX] = n ; 

		}


		// neighbors in y direction
		if( m_localDomainIndex[n].startX == m_localDomainIndex[idSolver].startX &&
			m_localDomainIndex[n].endX == m_localDomainIndex[idSolver].endX &&
			m_localDomainIndex[n].startZ == m_localDomainIndex[idSolver].startZ &&
			m_localDomainIndex[n].endZ == m_localDomainIndex[idSolver].endZ )
		{
			if( m_localDomainIndex[n].endY == m_localDomainIndex[idSolver].startY )
				m_neighbor[YMIN] = n ; 
			else if( m_localDomainIndex[n].startY == m_localDomainIndex[idSolver].endY )
				m_neighbor[YMAX] = n ; 
			
		}

		// neighbors in z direction
		if( m_localDomainIndex[n].startX == m_localDomainIndex[idSolver].startX &&
			m_localDomainIndex[n].endX == m_localDomainIndex[idSolver].endX &&
			m_localDomainIndex[n].startY == m_localDomainIndex[idSolver].startY &&
			m_localDomainIndex[n].endY == m_localDomainIndex[idSolver].endY )
		{
			if( m_localDomainIndex[n].endZ == m_localDomainIndex[idSolver].startZ )
				m_neighbor[ZMIN] = n ; 
			else if( m_localDomainIndex[n].startZ == m_localDomainIndex[idSolver].endZ )
				m_neighbor[ZMAX] = n ; 

		}
	}
	// end of 	! Find the neighbors  for( n = 0 ; n < numSolver ; n++ )



	return FDTD_SUCCESS;
}


void CParallelExchange::setIndex_H_Boundary( int *index_H_Boundary )
{
	memcpy( m_Index_H_Boundary , index_H_Boundary , sizeof( int ) * 6 ) ;
}

void CParallelExchange::exchange( float ***Hx, float ***Hy, float ***Hz ) 
{
	int  tag ;

	int i ;

	MPI_Status  status[24] ;
	MPI_Request  request[24] ;

	MPI_Barrier( Solver_Comm_World ) ;
	// x Min side
	if( m_neighbor[XMIN] != -1 )
	{
		// send Hy and Hz field to x Left
		tag = 1 ;
		MPI_Isend( &Hy[m_Index_H_Boundary[XMIN]][0][0] , ( m_ny + 1 ) * ( m_nz + 1 ), m_xType,  
			m_neighbor[XMIN], tag, Solver_Comm_World, &request[0] ) ;
		
		tag++ ;
		MPI_Isend( &Hz[m_Index_H_Boundary[XMIN]][0][0] , ( m_ny + 1 ) * ( m_nz + 1 ), m_xType,  
			m_neighbor[XMIN], tag, Solver_Comm_World, &request[1] ) ;
		

		// receive Hy and Hz field from x left
		tag = 1 ;
		MPI_Irecv( &Hy[m_Index_H_Boundary[XMIN]-1][0][0], ( m_ny + 1 ) * ( m_nz + 1 ) , m_xType,
			m_neighbor[XMIN], tag, Solver_Comm_World, &request[2] ) ;

		tag++ ;
		MPI_Irecv( &Hz[m_Index_H_Boundary[XMIN]-1][0][0], ( m_ny + 1 ) * ( m_nz + 1 ) , m_xType,
			m_neighbor[XMIN], tag, Solver_Comm_World, &request[3] ) ;
	}

	// x MAX side
	if( m_neighbor[XMAX] != -1 )
	{
		// send Hy and Hz field to x Left
		tag = 1 ;
		MPI_Isend( &Hy[m_Index_H_Boundary[XMAX]][0][0] , ( m_ny + 1 ) * ( m_nz + 1 ), m_xType,  
			m_neighbor[XMAX], tag, Solver_Comm_World, &request[4] ) ;
		
		tag++ ;
		MPI_Isend( &Hz[m_Index_H_Boundary[XMAX]][0][0] , ( m_ny + 1 ) * ( m_nz + 1 ), m_xType,  
			m_neighbor[XMAX], tag, Solver_Comm_World, &request[5] ) ;
		

		// receive Hy and Hz field from x left
		tag = 1 ;
		MPI_Irecv( &Hy[m_Index_H_Boundary[XMAX]+1][0][0], ( m_ny + 1 ) * ( m_nz + 1 ) , m_xType,
			m_neighbor[XMAX], tag, Solver_Comm_World, &request[6] ) ;

		tag++ ;
		MPI_Irecv( &Hz[m_Index_H_Boundary[XMAX]+1][0][0], ( m_ny + 1 ) * ( m_nz + 1 ) , m_xType,
			m_neighbor[XMAX], tag, Solver_Comm_World, &request[7] ) ;
	}


	// y Min side
	if( m_neighbor[YMIN] != -1 )
	{
		// send Hy and Hz field to x Left
		tag = 1 ;
		MPI_Isend( &Hx[0][m_Index_H_Boundary[YMIN]][0] , 1, m_yType,  
			m_neighbor[YMIN], tag, Solver_Comm_World, &request[8] ) ;
		
		tag++ ;
		MPI_Isend( &Hz[0][m_Index_H_Boundary[YMIN]][0] , 1, m_yType,  
			m_neighbor[YMIN], tag, Solver_Comm_World, &request[9] ) ;
		

		// receive Hy and Hz field from x left
		tag = 1 ;
		MPI_Irecv( &Hx[0][m_Index_H_Boundary[YMIN]-1][0], 1 , m_yType,
			m_neighbor[YMIN], tag, Solver_Comm_World, &request[10] ) ;

		tag++ ;
		MPI_Irecv( &Hz[0][m_Index_H_Boundary[YMIN]-1][0], 1 , m_yType,
			m_neighbor[YMIN], tag, Solver_Comm_World, &request[11] ) ;
	}

	// y MAX side
	if( m_neighbor[YMAX] != -1 )
	{
		// send Hy and Hz field to x Left
		tag = 1 ;
		MPI_Isend( &Hx[0][m_Index_H_Boundary[YMAX]][0] , 1, m_yType,  
			m_neighbor[YMAX], tag, Solver_Comm_World, &request[12] ) ;
		
		tag++ ;
		MPI_Isend( &Hz[0][m_Index_H_Boundary[YMAX]][0] , 1, m_yType,  
			m_neighbor[YMAX], tag, Solver_Comm_World, &request[13] ) ;
		

		// receive Hy and Hz field from x left
		tag = 1 ;
		MPI_Irecv( &Hx[0][m_Index_H_Boundary[YMAX]+1][0], 1 , m_yType,
			m_neighbor[YMAX], tag, Solver_Comm_World, &request[14] ) ;

		tag++ ;
		MPI_Irecv( &Hz[0][m_Index_H_Boundary[YMAX]+1][0], 1 , m_yType,
			m_neighbor[YMAX], tag, Solver_Comm_World, &request[15] ) ;
	}

	

	// z Min side
	if( m_neighbor[ZMIN] != -1 )
	{
		// send Hy and Hz field to x Left
		tag = 1 ;
		MPI_Isend( &Hx[0][0][m_Index_H_Boundary[ZMIN]] , 1, m_zType,  
			m_neighbor[ZMIN], tag, Solver_Comm_World, &request[16] ) ;
		
		tag++ ;
		MPI_Isend( &Hy[0][0][m_Index_H_Boundary[ZMIN]] , 1, m_zType,  
			m_neighbor[ZMIN], tag, Solver_Comm_World, &request[17] ) ;
		

		// receive Hy and Hz field from x left
		tag = 1 ;
		MPI_Irecv( &Hx[0][0][m_Index_H_Boundary[ZMIN]-1], 1 , m_zType,
			m_neighbor[ZMIN], tag, Solver_Comm_World, &request[18] ) ;

		tag++ ;
		MPI_Irecv( &Hy[0][0][m_Index_H_Boundary[ZMIN]-1], 1 , m_zType,
			m_neighbor[ZMIN], tag, Solver_Comm_World, &request[19] ) ;
	}

	// z MAX side
	if( m_neighbor[ZMAX] != -1 )
	{
		// send Hy and Hz field to x Left
		tag = 1 ;
		MPI_Isend( &Hx[0][0][m_Index_H_Boundary[ZMAX]] , 1, m_zType,  
			m_neighbor[ZMAX], tag, Solver_Comm_World, &request[20] ) ;
		
		tag++ ;
		MPI_Isend( &Hy[0][0][m_Index_H_Boundary[ZMAX]] , 1, m_zType,  
			m_neighbor[ZMAX], tag, Solver_Comm_World, &request[21] ) ;
		

		// receive Hy and Hz field from x left
		tag = 1 ;
		MPI_Irecv( &Hx[0][0][m_Index_H_Boundary[ZMAX]+1], 1 , m_zType,
			m_neighbor[ZMAX], tag, Solver_Comm_World, &request[22] ) ;

		tag++ ;
		MPI_Irecv( &Hy[0][0][m_Index_H_Boundary[ZMAX]+1], 1 , m_zType,
			m_neighbor[ZMAX], tag, Solver_Comm_World, &request[23] ) ;
	}
	

	// x Min side
	if( m_neighbor[XMIN] != -1 )
	{
		for( i = 0 ; i < 4 ; i++ )
			MPI_Wait( &request[i], &status[i] ) ;
	}

	if( m_neighbor[XMAX] != -1 )
	{
		for( i = 4 ; i < 8 ; i++ )
			MPI_Wait( &request[i], &status[i] ) ;
	}

	if( m_neighbor[YMIN] != -1 )
	{
		for( i = 8 ; i < 12 ; i++ )
			MPI_Wait( &request[i], &status[i] ) ;
	}

	if( m_neighbor[YMAX] != -1 )
	{
		for( i = 12 ; i < 16 ; i++ )
			MPI_Wait( &request[i], &status[i] ) ;
	}
	

	if( m_neighbor[ZMIN] != -1 )
	{
		for( i = 16 ; i < 20 ; i++ )
			MPI_Wait( &request[i], &status[i] ) ;
	}

	if( m_neighbor[ZMAX] != -1 )
	{
		for( i = 20 ; i < 24 ; i++ )
			MPI_Wait( &request[i], &status[i] ) ;
	}
	

	MPI_Barrier( Solver_Comm_World ) ;
}
