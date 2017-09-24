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

#include "Global_variable.h"

// processor ID of current processor in FDTD Solver
int idSolver = -1 ;

// the number of processor of FDTD Solver
int numSolver = 1;

// processor ID of current processor in both Master and FDTD Solver
int id = 0 ;

// the number of processor of in both Master and FDTD Solver
int processorNum = 2 ;

MPI_Group MPI_COMM_WORLD_Group , Solver_Group ;
MPI_Comm Solver_Comm_World ;
