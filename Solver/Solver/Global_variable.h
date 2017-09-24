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

#ifndef GLOBAL_VARIABLE_HEAD_FILE
#define GLOBAL_VARIABLE_HEAD_FILE

#include <cstdlib>

#include <mpi.h>

struct Point
{
	float x , y , z ;
} ;

// domain index in global domain
struct DomainIndex{
	int startX , endX ;
	int startY , endY ;
	int startZ , endZ ;
} ;

// processor ID of current processor in FDTD Solver
extern int idSolver ;

// the number of processor of FDTD Solver
extern int numSolver ;

// processor ID of current processor in both Master and FDTD Solver
extern int id ;

// the number of processor of in both Master and FDTD Solver
extern int processorNum ;


extern MPI_Group MPI_COMM_WORLD_Group , Solver_Group ;
extern MPI_Comm Solver_Comm_World ;

#endif
