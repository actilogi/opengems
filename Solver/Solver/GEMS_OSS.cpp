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

#include <iostream>
#include <mpi.h>
#include <ctime>

#include "FDTD.h"
#include "GEMS_Constant.h"

int main( int argc, char *argv[] )
{
	int result = MPI_Init( &argc, &argv ) ;
	if( result != MPI_SUCCESS )
	{
		return false ;
	}

	MPI_Comm_rank( MPI_COMM_WORLD, &id ) ;
	MPI_Comm_size( MPI_COMM_WORLD, &processorNum ) ;

	if( processorNum < 2 )
	{
		MPI_Finalize( ) ;
		if( id == 0 )
		{
			std::cout << "Usage : mpiexec -n x GEMS_OSS inputfile " << std::endl ;
			std::cout << "        x is the number of processors, it must be larger than 1. " << std::endl ;
		}
		exit( 1 ) ;
	}

	numSolver = processorNum - 1 ;

	int i ;

	// create solver comm world and solver id
	int *solverRank = NULL ;
	solverRank = new int[numSolver] ;
	for( i = 0 ; i < numSolver ; i++ )
		solverRank[i] = i + 1 ;

	MPI_Comm_group( MPI_COMM_WORLD, &MPI_COMM_WORLD_Group ) ;
	MPI_Group_incl( MPI_COMM_WORLD_Group, numSolver , solverRank, &Solver_Group ) ;

	MPI_Comm_create( MPI_COMM_WORLD, Solver_Group, &Solver_Comm_World ) ;

	if( id != 0 )
		MPI_Comm_rank( Solver_Comm_World, &idSolver ) ;

	delete[] solverRank ;

	CFDTD fdtd ;
	if( argc == 1 )
	{
		if( id == 0 )
			std::cout << "Usage : mpiexec -n x GEMS_OSS inputfile " << std::endl ;
		MPI_Finalize( ) ;
		return 1 ;
	}
	else
	{
		if( fdtd.setFileName( argv[1] ) == false )
		{
			if( id == 0 )
				std::cout << "Error happened when openning project file." << std::endl ;
			MPI_Finalize( ) ;
			return 1 ;
		}
	}

	if( id == 0 )
		std::cout << "Project pre-processing......." << std::endl ;
	
	result = fdtd.readIn( ) ;
	if( result != FDTD_SUCCESS )
	{
		MPI_Finalize( ) ;
		if( result == FDTD_NO_MEMORY )
			return 1 ;
		else
			return 2 ;
	}

	if( id == 0 )
		std::cout << "Project initiailzing." << std::endl ;

	fdtd.setOutFileName( "GEMS_Output.xml") ;
	result = fdtd.init( ) ;

	int sum = 0 ;
	result = FDTD_SUCCESS ;
	MPI_Allreduce( &result, &sum, 1, MPI_INT, MPI_SUM, MPI_COMM_WORLD ) ;
	if( sum != FDTD_SUCCESS )
	{
		if( id == 0 )
			std::cout << "GEMS initialize error." << std::endl ;
		MPI_Finalize( ) ;
		return 2 ;
	}

	MPI_Barrier ( MPI_COMM_WORLD ) ;       // make all processes syma....

	if( id == 0 )
		std::cout << "Start project simulation......" << std::endl << std::endl ;

	time_t ltimeStart, ltimeEnd ;
	long Time ;

	time( &ltimeStart );


	// fdtd update befin
	fdtd.update( ) ;

	if( id == 0 )
		std::cout << std::endl << std::endl << "Update procedure completed. " << std::endl ;

	// output result
	fdtd.output( ) ;



	if( id == 0 )
		std::cout << std::endl << "Simulation completed. " << std::endl ;

	if( id == 0 )
	{
		time( &ltimeEnd );
		Time = static_cast<long>( ltimeEnd - ltimeStart ) ;

		// output run time
		fprintf(stdout, "\n\nTotal Simulation time : << %2d:%2d:%2d >>\n\n",Time / 3600, (Time % 3600) / 60, Time% 60);
		fflush(stdout);
	}

	MPI_Barrier ( MPI_COMM_WORLD ) ;       // make all processes syma....
	

	MPI_Finalize( ) ;
	return 0 ;
}
