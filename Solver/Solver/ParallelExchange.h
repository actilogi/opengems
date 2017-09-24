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

#pragma once

#include "Global_variable.h"
#include "Boundary.h"

#include <mpi.h>

class CParallelExchange
{
public:
	CParallelExchange(void);
	~CParallelExchange(void);
private :

	// cell number in each direction
	int m_nx ;
	int m_ny ;
	int m_nz ;

	int m_Index_H_Boundary[6] ; 

	DomainIndex *m_localDomainIndex ;

	Boundary_Condition m_boundary[6] ;
	int m_neighbor[6] ;

	// From GEMS_Memory.h, we know that for a 3D array, the data is continuous on yz plane, z direction,
	// so when exchanging data for Parallel FDTD, m_xType is the same as float data type, for m_yType and m_zType
	// because they are not continuous in coresponding plane, we need define new MPI data type for them
	MPI_Datatype m_xType ;	// data type for exchange in x direction, yz plane, data is continuous on yz plane
	MPI_Datatype m_yType ;  // data type for exchange in y direction, xz plane
	MPI_Datatype m_zType ;  // data type for exchange in z direction, xy plane

public:
	int init( ) ;

	// Exchange H Field between CPUs
	void exchange( float ***Hx, float ***Hy, float ***Hz ) ;


	void setCellNum(int nx, int ny, int nz);
	void setDomainIndex( DomainIndex *localDomainIndex ) ;
	void setIndex_H_Boundary( int *index_H_Boundary ) ;

};
