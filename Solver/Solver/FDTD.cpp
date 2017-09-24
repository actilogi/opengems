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

#include "FDTD.h"

#include "GEMS_Memory.h"
#include "GEMS_Constant.h"
#include "GEMS_InFileTag.h"
#include "FDTD_Common_Func.h"

#include "Current_Output.h"
#include "Voltage_Output.h"
#include "Field_Output.h"

#include "Voltage_Source.h"
#include "Current_Source.h"

#include <cstdio>
#include <cstdlib>
#include <cmath>
#include <mpi.h>
#include <vector>
#include <iostream>
#include <ctime>

CFDTD::CFDTD(void)
{
	timeStep = 1 ;

	int i ;
	for( i = 0 ; i < 6 ; i++ )
	{
		boundary[i] = BC_NONE ;
		boundary_Global[i] = BC_NONE ;
		m_boundaryLayerNum[i] = 0 ;
		m_boundaryLayerNum_Global[i] = 0 ;
	}

	in = NULL ;

	// initialzie all the pointer to NULL
	Ex = Ey = Ez = NULL ;
	Hx = Hy = Hz = NULL ;

	Dx_Grid = Dy_Grid = Dz_Grid = NULL ;
	Dx_Half = Dy_Half = Dz_Half = NULL ;

	Dx_Grid_Inv = Dy_Grid_Inv = Dz_Grid_Inv = NULL ;
	Dx_Half_Inv = Dy_Half_Inv = Dz_Half_Inv = NULL ;

	X_Grid = Y_Grid = Z_Grid = NULL ;
	X_Half = Y_Half = Z_Half = NULL ;

	Dx_Grid_Global = Dy_Grid_Global = Dz_Grid_Global = NULL ;
	Dx_Half_Global = Dy_Half_Global = Dz_Half_Global = NULL ;
	X_Half_Global = Y_Half_Global = Z_Half_Global = NULL ;
	X_Grid_Global = Y_Grid_Global = Z_Grid_Global = NULL ;

	Nx_Local2Global = NULL ;
	Ny_Local2Global = NULL ;
	Nz_Local2Global = NULL ;

	pHi_Coeff_M_Lossless = NULL ;
	pHj_Coeff_M_Lossless = NULL ;
	pHk_Coeff_M_Lossless = NULL ;

	pEi_Coeff = NULL ;
	pEj_Coeff = NULL ;
	pEk_Coeff = NULL ;

	eMaterials_Common = NULL ;
	eMatType = NULL ;
	eMatTypeFlag = NULL ;

	m_farField = NULL ;
	m_source = NULL ;

	m_output = NULL ;
	m_outFileName = "" ;

	pulse = NULL ;


	m_nx = 1 ;
	m_ny = 1 ;
	m_nz = 1 ;

	// cell number in global domain
	m_nx_Global = 1 ;
	m_ny_Global = 1 ;
	m_nz_Global = 1 ;

}

CFDTD::~CFDTD(void)
{
	int i , j ;

	// release material
	if( eMatType != NULL )
	{
		for( i = 0 ; i < m_nx + 3 ; i++ )
		{
			for( j = 0 ; j < m_ny + 3 ; j++ )
			{
				delete []eMatType[i][j] ;
				eMatType[i][j] = NULL ;
			}
			delete []eMatType[i] ;
			eMatType[i] = NULL ;
		}

		delete[] eMatType ;
		eMatType = NULL ;
	}

	if( eMaterials_Common != NULL )
	{
		CMaterial *current, *next ;
		current = eMaterials_Common ;
		next = eMaterials_Common->getNext() ;

		while( current != NULL )
		{
			delete current ;
			current = next ;
			if( current != NULL )
				next = current->getNext() ;
		}
	}
	Free_3D( eMatTypeFlag, m_nx + 3 ) ;

	Free_1D( pulse ) ;

	Free_1D( Nx_Local2Global ) ;
	Free_1D( Ny_Local2Global ) ;
	Free_1D( Nz_Local2Global ) ;

	Free_1D( pHi_Coeff_M_Lossless ) ;
	Free_1D( pHj_Coeff_M_Lossless ) ;
	Free_1D( pHk_Coeff_M_Lossless ) ;

	Free_1D( pEi_Coeff ) ;
	Free_1D( pEj_Coeff ) ;
	Free_1D( pEk_Coeff ) ;
	
	Free_1D( Dx_Grid ) ;
	Free_1D( Dy_Grid ) ;
	Free_1D( Dz_Grid ) ;

	Free_1D( Dx_Half ) ;
	Free_1D( Dy_Half ) ;
	Free_1D( Dz_Half ) ;
	
	Free_1D( Dx_Grid_Inv ) ;
	Free_1D( Dy_Grid_Inv ) ;
	Free_1D( Dz_Grid_Inv ) ;
	
	Free_1D( Dx_Half_Inv ) ;
	Free_1D( Dy_Half_Inv ) ;
	Free_1D( Dz_Half_Inv ) ;

	Free_1D( X_Grid ) ;
	Free_1D( Y_Grid ) ;
	Free_1D( Z_Grid ) ;
	
	Free_1D( X_Half ) ;
	Free_1D( Y_Half ) ;
	Free_1D( Z_Half ) ;

	Free_1D( Dx_Grid_Global ) ;
	Free_1D( Dy_Grid_Global ) ;
	Free_1D( Dz_Grid_Global ) ;

	Free_1D( Dx_Half_Global ) ;
	Free_1D( Dy_Half_Global ) ;
	Free_1D( Dz_Half_Global ) ;

	Free_1D( X_Half_Global ) ;
	Free_1D( Y_Half_Global ) ;
	Free_1D( Z_Half_Global ) ;

	Free_1D( X_Grid_Global ) ;
	Free_1D( Y_Grid_Global ) ;
	Free_1D( Z_Grid_Global ) ;

	Free_3D( Ex, m_nx + 1 ) ;
	Free_3D( Ey, m_nx + 1 ) ;
	Free_3D( Ez, m_nx + 1 ) ;

	Free_3D( Hx, m_nx + 1 ) ;
	Free_3D( Hy, m_nx + 1 ) ;
	Free_3D( Hz, m_nx + 1 ) ;

	// Free source
	if( m_source != NULL )
	{
		CFDTD_Source *currentSouce = m_source, *nextSource = m_source->getNext() ;
		while( currentSouce != NULL )
		{
			delete currentSouce ;
			currentSouce = nextSource ;

			if( currentSouce != NULL )
				nextSource = currentSouce->getNext() ;
		}
	}
	

	// Free output
	if( m_output != NULL )
	{
		CFDTD_Output *cOutput = m_output, *nextOutput = m_output->getNext() ;
		while( cOutput != NULL )
		{
			delete cOutput ;
			cOutput = nextOutput ;

			if( cOutput != NULL )
				nextOutput = cOutput->getNext() ;
		}
	}

	if( m_farField != NULL )
		delete m_farField ;
}



void CFDTD::update( )
{
	int n , nn = 0 ;
	int percent[101] ;
	if( id == 0 )
	{
		for ( n = 0 ; n < 100 ; n++ )
		{
			if( timeStep < 100 )
			{
				percent[n] = n ;
			} 
			else
			{
				percent[n] = int ( n * (float) timeStep / 100.0) ;
			}
		}
	}


	time_t ltimeStart, ltimeEnd ;
	long Time ;

	time( &ltimeStart );

	for( n = 0 ; n < timeStep ; n++ )
	{
		if( id == 0 )
		{
			
			if( percent[nn] == n )
			{
				std::cout << "n = " << n << " ,  " << static_cast< int>( n * 100 / static_cast< float>( timeStep ) ) << "% \r" ;
				std::flush( std::cout ) ;
				nn++ ;
			}
			

			
			//std::cout << "n = " << n << " ,  " << static_cast< int>( n * 100 / static_cast< float>( timeStep ) ) <<  "% \r" ;
			//std::flush( std::cout ) ;
			
			update_H_Source( n ) ;
			update_E_Source( n ) ;
		}
		else
		{

			// Update H Field
			update_H_Field( n ) ;
			
			
			// Update H boundary
			m_boundary.update_H_Boundary( n , Ex, Ey, Ez, Hx, Hy, Hz ) ;

			// Update current source
			update_H_Source( n ) ;

			// parallel exchange H field
			if( numSolver > 1 )
				m_parallelExchange.exchange( Hx, Hy, Hz ) ;


			// Update E Field
			update_E_Field( n ) ;

			
			// Update UPML, Mur and PEC boundary
			m_boundary.update_E_Boundary( n ,Ex, Ey, Ez, Hx, Hy, Hz ) ;


			
			// Update voltage source
			update_E_Source( n ) ;

			// update far field
			if( m_farField->getEnabled())
			{
				m_farField->farField_EM_Current( n , Ex, Ey, Ez, Hx, Hy, Hz ) ;
			}
			
			
		}// else of if( id == 0 )
		
		// collect curren or voltage or field at a point
		if( m_output != NULL )
		{
			CFDTD_Output *cOutput = m_output, *nextOutput = m_output->getNext() ;
			while( cOutput != NULL )
			{
				cOutput->collectResult( n , Ex, Ey, Ez, Hx, Hy, Hz ) ;
				cOutput = nextOutput ;

				if( cOutput != NULL )
					nextOutput = cOutput->getNext() ;
			}
		}
		
	} // end of for( n = 0 ; n < timeStep ; n++ )

	if( id == 0 )
	{
		std::cout << "n = " << n - 1 << " ,  " <<  "100% \r" ;
		std::flush( std::cout ) ;

		time( &ltimeEnd );
		Time = static_cast<long>( ltimeEnd - ltimeStart ) ;

		// output run time
		fprintf(stdout, "\n\nTotal updating time : << %2d:%2d:%2d >>\n\n",Time / 3600, (Time % 3600) / 60, Time% 60);
		fflush(stdout);
	}


}


void CFDTD::output( )
{
	if( id == 0 )
	{
		// output time domain voltage, current or field at a point
		std::string str ;
		char c_string[64] = "";
		sprintf_s( c_string, "%e", Dt ) ;
		str = c_string ;
		m_outFile.AddAtributes("deltaTime", str );	
		
		sprintf_s( c_string, "%d", timeStep ) ;
		str = c_string ;
		m_outFile.AddAtributes("timeStep", str );	

		m_outFile.AddAtributes("type","Output");	
		m_outFile.AddAtributes("version","1.00.00");	
		m_outFile.Createtag("Document");

		
		m_outFile.AddAtributes( "value", m_stamp );	
		m_outFile.Createtag( "Stamp" ) ;
		m_outFile.CloseSingletag(); // close boby


		if( m_output != NULL )
		{
			CFDTD_Output *cOutput = m_output, *nextOutput = m_output->getNext() ;
			while( cOutput != NULL )
			{
				cOutput->output( m_outFile ) ;
				cOutput = nextOutput ;

				if( cOutput != NULL )
					nextOutput = cOutput->getNext() ;
			}
		}
	}

	// calculate Far Field 
	if( m_farField->getEnabled())
	{
		m_farField->farField_Near2Far( m_outFile ) ;
	}

	if( id == 0 )
	{
		m_outFile.CloseLasttag() ;
		m_outFile.CloseAlltags();
	}
}

void CFDTD::update_H_Field(int n)
{
	int i , j , k ;

	// Update field first 

	// Update Hx
    for ( i = Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN]; i <= Index_H_Boundary[XMAX] + 1 - m_boundaryLayerNum[XMAX] ; i++ ) 
	{
	    for ( j = Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++) 
		{
			for ( k = Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= Index_H_Boundary[ZMAX]- m_boundaryLayerNum[ZMAX] ; k++ ) 
			{
				Hx[i][j][k] += pHk_Coeff_M_Lossless[k] * (Ey[i][j][k+1] - Ey[i][j][k])
							 - pHj_Coeff_M_Lossless[j] * (Ez[i][j+1][k] - Ez[i][j][k]);
			}
		}
	}

	// Update Hy
	for ( i = Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i <= Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] ; i++) 
	{
	    for ( j = Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= Index_H_Boundary[YMAX] + 1 - m_boundaryLayerNum[YMAX] ; j++) 
		{
			for (k = Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++ ) 
			{

				Hy[i][j][k] += pHi_Coeff_M_Lossless[i] * (Ez[i+1][j][k] - Ez[i][j][k])
							 - pHk_Coeff_M_Lossless[k] * (Ex[i][j][k+1] - Ex[i][j][k]);
			}
		}
	}

	// Update Hz
    for ( i = Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i <= Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] ; i++) 
	{
	    for ( j = Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++ ) 
		{
			for ( k = Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= Index_H_Boundary[ZMAX] + 1 - m_boundaryLayerNum[ZMAX]; k++ ) 
			{

				Hz[i][j][k] += pHj_Coeff_M_Lossless[j] * (Ex[i][j+1][k] - Ex[i][j][k])
						     - pHi_Coeff_M_Lossless[i] * (Ey[i+1][j][k] - Ey[i][j][k]);
			}
		}
	}

	// update boundary H field
	//m_boundary.update_H_Boundary( n , Hx, Hy, Hz ) ;
}

void CFDTD::update_E_Field(int n)
{
	// Update field first 
	int i,j,k;


	// Update Ex
    for (i = Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN]; i < Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] ; i++) 
	{
	    for (j = Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j <= Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++) 
		{
			for (k = Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]; k++) 
			{

				Ex[i][j][k] =  eMatType[i][j][k]->AEx_Coeff * Ex[i][j][k] 
							 + pEj_Coeff[j] * eMatType[i][j][k]->AEx_p_Inv * (Hz[i][j][k] - Hz[i][j-1][k])
							 - pEk_Coeff[k] * eMatType[i][j][k]->AEx_p_Inv * (Hy[i][j][k] - Hy[i][j][k-1]);
			}
		}
	}

	//Update Ey
	for (i = Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i <= Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] ; i++) 
	{
	    for (j = Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j < Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX]; j++) 
		{
			for (k = Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++) 
			{
				Ey[i][j][k] =  eMatType[i][j][k]->AEy_Coeff * Ey[i][j][k] 
							 + pEk_Coeff[k] * eMatType[i][j][k]->AEy_p_Inv * (Hx[i][j][k] - Hx[i][j][k-1])
						     - pEi_Coeff[i] * eMatType[i][j][k]->AEy_p_Inv * (Hz[i][j][k] - Hz[i-1][j][k]);
			}
		}
	}

	// Update Ez
    for (i = Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i <= Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] ; i++) 
	{
	    for (j = Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++) 
		{
			for (k = Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k < Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++) 
			{

				Ez[i][j][k] =  eMatType[i][j][k]->AEz_Coeff * Ez[i][j][k] 
							 + pEi_Coeff[i] * eMatType[i][j][k]->AEz_p_Inv * (Hy[i][j][k] - Hy[i-1][j][k])
							 - pEj_Coeff[j] * eMatType[i][j][k]->AEz_p_Inv * (Hx[i][j][k] - Hx[i][j-1][k]);
			}
		}
	}
}


void CFDTD::update_H_Source(int n)
{
	int index = 0 ;
	if( m_source != NULL )
	{
		CFDTD_Source *cSouce = m_source, *nextSource = m_source->getNext() ;
		while( cSouce != NULL )
		{
			if( id == 0 )
				cSouce->update_H_Field( n , Hx, Hy, Hz , index, m_farField ) ;
			else
				cSouce->update_H_Field( n,  Hx,  Hy,  Hz);

			cSouce = nextSource ;
			if( cSouce != NULL )
				nextSource = cSouce->getNext() ;
			index++ ;
		}
	}

}

void CFDTD::update_E_Source(int n)
{
	
	int index = 0 ;
	if( m_source != NULL )
	{
		CFDTD_Source *cSouce = m_source, *nextSource = m_source->getNext() ;
		while( cSouce != NULL )
		{
			if( id == 0 )
				cSouce->update_E_Field( n , Ex, Ey, Ez , index, m_farField ) ;
			else
				cSouce->update_E_Field( n,  Ex, Ey, Ez);

			cSouce = nextSource ;
			if( cSouce != NULL )
				nextSource = cSouce->getNext() ;
			index++ ;
		}
	}
	
}

int CFDTD::init(void)
{

	int init_Result ;


	init_Result = FDTD_SUCCESS ;
	if( id != 0 )
		init_Result = initMemory( ) ;

	int sum = 0 ;
	MPI_Allreduce( &init_Result, &sum, 1, MPI_INT, MPI_SUM, MPI_COMM_WORLD ) ;

	if( sum != FDTD_SUCCESS )
	{
		return FDTD_NO_MEMORY ;
	}

	if( id != 0 )
		index_Local2Global( ) ;

	// Init boundary condition
	init_Result = FDTD_SUCCESS ;
	if( id != 0 )
	{
		m_boundary.setMaterial( eMatType ) ;
		m_boundary.setDGrid( Dx_Grid, Dy_Grid, Dz_Grid ) ;
		m_boundary.setDt( Dt ) ;

		m_boundary.setH_Coeff( pHi_Coeff_M_Lossless, pHj_Coeff_M_Lossless, pHk_Coeff_M_Lossless ) ;
		m_boundary.setE_Coeff( pEi_Coeff, pEj_Coeff, pEk_Coeff ) ;

		m_boundary.setMaterial( eMatType ) ;

		init_Result = m_boundary.init() ;
	}
	sum = 0 ;
	MPI_Allreduce( &init_Result, &sum, 1, MPI_INT, MPI_SUM, MPI_COMM_WORLD ) ;
	if( sum != FDTD_SUCCESS )
	{
		return OTHER_ERROR ;
	}
	// end of Init boundary condition

	// init Source
	init_Result = FDTD_SUCCESS ;
	if( m_source != NULL )
	{
		CFDTD_Source *cSource, *nextSource ;
		cSource = m_source ;
		nextSource = m_source->getNext() ;

		while( cSource != NULL )
		{
			cSource->setMaterial( eMatType ) ;
			cSource->setBoundaryIndex( Index_E_Boundary, Index_H_Boundary ) ;
			cSource->setDt( Dt ) ;

			cSource->setPulseData( pulse, pulseLen ) ;
			cSource->setDGrid( Dx_Grid , Dy_Grid , Dz_Grid ) ;
			cSource->setDHalf( Dx_Half , Dy_Half , Dz_Half ) ;

			init_Result = cSource->init() ;
			sum = 0 ;
			MPI_Allreduce( &init_Result, &sum, 1, MPI_INT, MPI_SUM, MPI_COMM_WORLD ) ;
			if( sum != FDTD_SUCCESS )
			{
				return OTHER_ERROR ;
			}

			cSource = nextSource ;

			if( cSource != NULL )
				nextSource = cSource->getNext() ;
		}
	}
	// end of Source initialization

	
	// init output
	init_Result = FDTD_SUCCESS ;
	if( m_output != NULL )
	{
		CFDTD_Output *cOutput, *nextOutput ;
		cOutput = m_output ;
		nextOutput = m_output->getNext() ;

		while( cOutput != NULL )
		{
			cOutput->setBoundaryIndex( Index_E_Boundary, Index_H_Boundary ) ;

			cOutput->setDGrid( Dx_Grid , Dy_Grid , Dz_Grid ) ;
			cOutput->setDHalf( Dx_Half , Dy_Half , Dz_Half ) ;
			
			cOutput->setGrid( X_Grid, Y_Grid, Z_Grid);
			cOutput->setHalf( X_Half, Y_Half, Z_Half);
			cOutput->setDHalf_Inv( Dx_Half_Inv, Dy_Half_Inv, Dz_Half_Inv);
			cOutput->setTimeStep( timeStep ) ;

			init_Result = cOutput->init() ;
			sum = 0 ;
			MPI_Allreduce( &init_Result, &sum, 1, MPI_INT, MPI_SUM, MPI_COMM_WORLD ) ;
			if( sum != FDTD_SUCCESS )
			{
				return OTHER_ERROR ;
			}

			cOutput = nextOutput ;

			if( cOutput != NULL )
				nextOutput = cOutput->getNext() ;
		}
	}

	// end of Output Initialization

	
	
	// Init Far Field
	// set Far Field parameter
	m_farField->setDt( Dt ) ;
	m_farField->setCellNum( m_nx, m_ny, m_nz ) ;
	m_farField->setDomainIndex( LocalDomainIndex ) ;
	
	m_farField->setDGrid( Dx_Grid , Dy_Grid , Dz_Grid ) ;
	m_farField->setDGrid_Global( Dx_Grid_Global, Dy_Grid_Global, Dz_Grid_Global);
	m_farField->setHalf_Global( X_Half_Global, Y_Half_Global, Z_Half_Global);
	m_farField->setGrid_Global( X_Grid_Global, Y_Grid_Global, Z_Grid_Global);

	m_farField->setN_Local2Global( Nx_Local2Global, Ny_Local2Global, Nz_Local2Global);

	m_farField->setSourceNum( m_source->getSourceNum( ) ) ;


	init_Result = m_farField->init() ;
	sum = 0 ;
	MPI_Allreduce( &init_Result, &sum, 1, MPI_INT, MPI_SUM, MPI_COMM_WORLD ) ;
	if( sum != FDTD_SUCCESS )
	{
		return OTHER_ERROR ;
	}
	// end of Far Field initialization
	

	if( id != 0 )
	{
		m_parallelExchange.setCellNum( m_nx , m_ny, m_nz ) ;
		m_parallelExchange.setDomainIndex( LocalDomainIndex ) ;
		m_parallelExchange.setIndex_H_Boundary( Index_H_Boundary ) ;

		m_parallelExchange.init() ;
	}


	if( id != 0 )
	{

		int i , j , k ;
		//------------------------------------------------------------------------------------------------------
		// for HField_Update_MagnetcLossless
		for (i = Index_H_Boundary[XMIN]; i <= Index_H_Boundary[XMAX]; i++)  
		{
			pHi_Coeff_M_Lossless[i] = Dt * Mu0_Inv * Dx_Grid_Inv[i] ;
		}
		for (j = Index_H_Boundary[YMIN]; j <= Index_H_Boundary[YMAX] ; j++) 
		{
			pHj_Coeff_M_Lossless[j] = Dt * Mu0_Inv * Dy_Grid_Inv[j] ;
		}
		for (k = Index_H_Boundary[ZMIN]; k <= Index_H_Boundary[ZMAX] ; k++)	
		{
			pHk_Coeff_M_Lossless[k] = Dt * Mu0_Inv * Dz_Grid_Inv[k] ;
		}

		// EField_Update Coeff
		for (i = Index_E_Boundary[XMIN]; i <= Index_E_Boundary[XMAX]; i++)  
		{
			pEi_Coeff[i] = Dt / Dx_Half[i] ;
		}

		for (j = Index_E_Boundary[YMIN]; j <= Index_E_Boundary[YMAX] ; j++) 
		{
			pEj_Coeff[j] = Dt / Dy_Half[j] ;
		}
		for (k = Index_E_Boundary[ZMIN]; k <= Index_E_Boundary[ZMAX] ; k++)	
		{
			pEk_Coeff[k] = Dt / Dz_Half[k] ;
		}

		// end of free memory

		//----------------------------------------------------------------------------------------

	}
	
	// output sigma distribution
	if( id != 0 )
	{
		OutputSigma( eMatType , Index_E_Boundary, m_boundaryLayerNum, 
					 X_Grid, Y_Grid, Z_Grid , Dx_Grid, Dy_Grid, Dz_Grid , 
					 X_Grid_Global, Y_Grid_Global, Z_Grid_Global ,
					 Nx_Local2Global, Ny_Local2Global, Nz_Local2Global,
					 m_domainRef , unit ) ;
	}
	

	return FDTD_SUCCESS;
}

void CFDTD::index_Local2Global( )
{
	int xIndex = 0 , yIndex = 0 , zIndex = 0 ;

	int i , j , k ;

	// 
	for( i = 0 ; i < m_nx ; i++ )
	{
		xIndex = i - 1 - m_boundaryLayerNum[XMIN];
		xIndex += LocalDomainIndex[idSolver].startX ;
		Nx_Local2Global[i] = xIndex ;

	}

	for( j = 0 ; j < m_ny ; j++ )
	{
		yIndex = j - 1 - m_boundaryLayerNum[YMIN] ;
		yIndex += LocalDomainIndex[idSolver].startY ;
		Ny_Local2Global[j] = yIndex ;
	}

	for( k = 0 ; k < m_nz ; k++ )
	{
		zIndex = k - 1 - m_boundaryLayerNum[ZMIN];
		zIndex += LocalDomainIndex[idSolver].startZ ;
		Nz_Local2Global[k] = zIndex ;
	}
}


int CFDTD::initMemory( ) 
{
	// Allocate memory
	Ex = Allocate_3D( Ex, m_nx + 1 , m_ny + 1 , m_nz + 1 ) ;
	if( Ex == NULL )
		return FDTD_NO_MEMORY ;

	Ey = Allocate_3D( Ey, m_nx + 1 , m_ny + 1 , m_nz + 1 ) ;
	if( Ey == NULL )
		return FDTD_NO_MEMORY;

	Ez = Allocate_3D( Ez, m_nx + 1 , m_ny + 1 , m_nz + 1 ) ;
	if( Ez == NULL )
		return FDTD_NO_MEMORY;


	Hx = Allocate_3D( Hx, m_nx + 1 , m_ny + 1 , m_nz + 1 ) ;
	if( Hx == NULL )
		return FDTD_NO_MEMORY;

	Hy = Allocate_3D( Hy, m_nx + 1 , m_ny + 1 , m_nz + 1 ) ;
	if( Hy == NULL )
		return FDTD_NO_MEMORY;

	Hz = Allocate_3D( Hz, m_nx + 1 , m_ny + 1 , m_nz + 1 ) ;
	if( Hz == NULL )
		return FDTD_NO_MEMORY;

	// ---------------------------------------------------------------------------------
	pHi_Coeff_M_Lossless = Allocate_1D( pHi_Coeff_M_Lossless, m_nx+1 ) ;
	if( pHi_Coeff_M_Lossless == NULL )
		return FDTD_NO_MEMORY;
	
	pHj_Coeff_M_Lossless = Allocate_1D( pHj_Coeff_M_Lossless, m_ny+1 ) ;
	if( pHj_Coeff_M_Lossless == NULL )
		return FDTD_NO_MEMORY;
	
	pHk_Coeff_M_Lossless = Allocate_1D( pHk_Coeff_M_Lossless, m_nz+1 ) ;
	if( pHk_Coeff_M_Lossless == NULL )
		return FDTD_NO_MEMORY;
	
	pEi_Coeff = Allocate_1D( pEi_Coeff, m_nx+1 ) ;
	if( pEi_Coeff == NULL )
		return FDTD_NO_MEMORY;
	
	pEj_Coeff = Allocate_1D( pEj_Coeff, m_ny+1 ) ;
	if( pEj_Coeff == NULL )
		return FDTD_NO_MEMORY;
	
	pEk_Coeff = Allocate_1D( pEk_Coeff, m_nz+1 ) ;
	if( pEk_Coeff == NULL )
		return FDTD_NO_MEMORY;


	Nx_Local2Global = Allocate_1D( Nx_Local2Global, m_nx + 3 ) ;
	if( Nx_Local2Global == NULL )
		return FDTD_NO_MEMORY;

	Ny_Local2Global = Allocate_1D( Ny_Local2Global, m_ny + 3 ) ;
	if( Ny_Local2Global == NULL )
		return FDTD_NO_MEMORY;

	Nz_Local2Global = Allocate_1D( Nz_Local2Global, m_nz + 3 ) ;
	if( Nz_Local2Global == NULL )
		return FDTD_NO_MEMORY;

	return FDTD_SUCCESS;
}

int CFDTD::readIn( ) 
{
	int i ;
	size_t readSize = 0 ;
	int endFlag = 0 ;
	char tag ;
	char endMark[2] ;
	while( endFlag == 0 )
	{
		if( id == 0 )
		{
			readSize = fread( &tag, sizeof( char ), 1, in ) ;

			if( readSize == 0 )
				tag = TAG_END_FILE ;
		}

		MPI_Barrier ( MPI_COMM_WORLD ) ;       // make all processes syma....

		// broadcast this information to all processors
		MPI_Bcast( &tag , 1 , MPI_CHAR, 0 , MPI_COMM_WORLD ) ;

		/*
		if( id == 0 )
		{
			fprintf( stdout, "id = %d, tag = %x\n", id, static_cast<unsigned char >(tag) ) ;
			fflush( stdout ) ;
		}
		*/

		switch( tag )
		{
			case TAG_STAMP :			// m_stamp
				if( id == 0 )
				{
					char c_stamp[37] ;
					fread( c_stamp, sizeof( char ), 36, in ) ;
					c_stamp[36] = 0 ;
					m_stamp = c_stamp ;

					fread( endMark, sizeof( char ), 2, in ) ;
				}
				break ;

			case TAG_LENUNITRATIO :			// unit
				if( id == 0 )
				{
					fread( &unit, sizeof( float), 1, in ) ;
					fread( endMark, sizeof( char ), 2, in ) ;
				}
				MPI_Bcast( &unit , 1 , MPI_FLOAT, 0 , MPI_COMM_WORLD ) ;


				break ;
			case TAG_TIMESTEP:
				if( id == 0 )
				{
					fread( &timeStep, sizeof( int ), 1, in ) ;

					fread( endMark, sizeof( char ), 2, in ) ;

				}
				MPI_Bcast( &timeStep , 1 , MPI_INT, 0 , MPI_COMM_WORLD ) ;
				break ;

			case TAG_CELLCOUNT :			// cell number in global domain
				if( id == 0 )
				{
					fread( &m_nx_Global, sizeof( int ), 1, in ) ;
					fread( &m_ny_Global, sizeof( int ), 1, in ) ;
					fread( &m_nz_Global, sizeof( int ), 1, in ) ;

					fread( endMark, sizeof( char ), 2, in ) ;
					m_nx = m_nx_Global ;
					m_ny = m_ny_Global ;
					m_nz = m_nz_Global ;
				}


				MPI_Bcast( &m_nx_Global , 1 , MPI_INT, 0 , MPI_COMM_WORLD ) ;
				MPI_Bcast( &m_ny_Global , 1 , MPI_INT, 0 , MPI_COMM_WORLD ) ;
				MPI_Bcast( &m_nz_Global , 1 , MPI_INT, 0 , MPI_COMM_WORLD ) ;

				
				break ;
			case TAG_PARALLEL:				// parallel set information
				{
					int cpuNum , startx , endx , starty , endy , startz , endz ;
					if( id == 0 )
					{
						fread( &cpuNum, sizeof( int ), 1, in ) ;
					}

					MPI_Bcast( &cpuNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

					if( cpuNum != numSolver )
					{
						return OTHER_ERROR ;
					}

					LocalDomainIndex = new DomainIndex[numSolver] ;

					// read in parallel setting information
					for( i = 0 ; i < cpuNum ; i++ )
					{
						if( id == 0 )
						{
							fread( &startx, sizeof( int ), 1, in ) ;
							fread( &endx, sizeof( int ), 1, in ) ;
							fread( &starty, sizeof( int ), 1, in ) ;
							fread( &endy, sizeof( int ), 1, in ) ;
							fread( &startz, sizeof( int ), 1, in ) ;
							fread( &endz, sizeof( int ), 1, in ) ;

						}

						MPI_Bcast( &startx , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
						MPI_Bcast( &endx , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
						MPI_Bcast( &starty , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
						MPI_Bcast( &endy , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
						MPI_Bcast( &startz , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
						MPI_Bcast( &endz , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;

						LocalDomainIndex[i].startX = startx ;
						LocalDomainIndex[i].endX = endx ;
						LocalDomainIndex[i].startY = starty ;
						LocalDomainIndex[i].endY = endy ;
						LocalDomainIndex[i].startZ = startz ;
						LocalDomainIndex[i].endZ = endz ;

					}

					if( id == 0 )
						fread( endMark, sizeof( char ), 2, in ) ;
				}
				
				break ;
			case TAG_PULSE :		// read PULSE information

				if( id == 0 )
				{
					fread( &Dt, sizeof( float), 1, in ) ;
					fread( endMark, sizeof( char ), 1, in ) ;

					fread( &pulseLen, sizeof( int ), 1, in ) ;
					fread( endMark, sizeof( char ), 1, in ) ;


					pulse = new float[pulseLen] ;

					fread( pulse, sizeof( float ), pulseLen , in ) ;

					fread( endMark, sizeof( char ), 2, in ) ;

				}

				MPI_Bcast( &Dt , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
				Dt = Dt ;
				
				MPI_Bcast( &pulseLen , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
				
				if( id != 0 )
					pulse = new float[pulseLen] ;
				MPI_Bcast( pulse, pulseLen , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
				
				break ;

			case TAG_DOMAIN:		// read boundary condition
				{
					// set global cell number
					m_boundary.setCellNumberGlobal( m_nx_Global, m_ny_Global, m_nz_Global) ;
					m_boundary.setDomainIndex( LocalDomainIndex ) ;
					
					// read boundary information and re-calculate boundary and update index
					m_boundary.readIn( in ) ;
					
					// get new boundary
					m_boundary.getBoundary( boundary ) ;
					m_boundary.getBoundary_Global( boundary_Global ) ;
					m_boundary.getBoundaryLayerNum( m_boundaryLayerNum, m_boundaryLayerNum_Global ) ;

					if( id != 0 )
					{
						m_boundary.getBoundaryIndex( Index_E_Boundary, Index_H_Boundary ) ;
						m_boundary.getCellNum( m_nx, m_ny, m_nz ) ;
					}

					// allocate memory for grid and half cell
					int result = cellAllocateMemory( ) ;
					int sum = 0 ;
					MPI_Allreduce( &result, &sum, 1, MPI_INT, MPI_SUM, MPI_COMM_WORLD ) ;
					if( sum )
						return FDTD_NO_MEMORY ;

					// allocate memory for material
					result = FDTD_SUCCESS ;
					if( id != 0 )
						result = materialAllocateMemory( ) ;
					MPI_Allreduce( &result, &sum, 1, MPI_INT, MPI_MAX, MPI_COMM_WORLD ) ;
					if( sum )
						return FDTD_NO_MEMORY ;
						
				}
				break ;
			case TAG_HUYGENSBOX :
				m_farField = new CFarField ;
				m_farField->setBoundaryLayerNum( m_boundaryLayerNum ) ;
				if( id == 0 )
					m_farField->readIn( in ) ;
				else
					m_farField->readIn_Solver() ;
				break ;
			case TAG_CELLSIZE_X :		// cell number and size in X direction 
				if( id == 0 )
				{
					fread( &(m_domainRef.x), sizeof( float), 1, in ) ;
				}
				MPI_Bcast( &(m_domainRef.x), 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
				
				if( id == 0 )
				{
					fread( Dx_Grid_Global, sizeof( float), m_nx_Global, in ) ;

					fread( endMark, sizeof( char ), 2, in ) ;
				}
				MPI_Bcast( Dx_Grid_Global, m_nx_Global , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;


				calculateCellPara( m_nx, m_nx_Global, LocalDomainIndex[idSolver].startX, LocalDomainIndex[idSolver].endX, Dx_Grid_Global, Dx_Half_Global, X_Grid_Global, X_Half_Global, 
					Dx_Grid, Dx_Half, X_Grid, X_Half, Dx_Grid_Inv, Dx_Half_Inv , m_boundaryLayerNum[XMIN], m_boundaryLayerNum[XMAX] ) ;


				break ;
			case TAG_CELLSIZE_Y :		// cell number and size in X direction 
				if( id == 0 )
					fread( &(m_domainRef.y), sizeof( float), 1, in ) ;

				MPI_Bcast( &(m_domainRef.y), 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;

				if( id == 0 )
				{
					fread( Dy_Grid_Global, sizeof( float), m_ny_Global, in ) ;
					fread( endMark, sizeof( char ), 2, in ) ;
				}
				MPI_Bcast( Dy_Grid_Global, m_ny_Global , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;

				calculateCellPara( m_ny, m_ny_Global, LocalDomainIndex[idSolver].startY, LocalDomainIndex[idSolver].endY, Dy_Grid_Global, Dy_Half_Global, Y_Grid_Global, Y_Half_Global, 
					Dy_Grid, Dy_Half, Y_Grid, Y_Half, Dy_Grid_Inv, Dy_Half_Inv , m_boundaryLayerNum[YMIN], m_boundaryLayerNum[YMAX] ) ;

				break ;
			case TAG_CELLSIZE_Z :		// cell number and size in X direction 
				if( id == 0 )
					fread( &(m_domainRef.z), sizeof( float), 1, in ) ;

				MPI_Bcast( &(m_domainRef.z), 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;

				if( id == 0 )
				{
					fread( Dz_Grid_Global, sizeof( float), m_nz_Global, in ) ;
					fread( endMark, sizeof( char ), 2, in ) ;
				}
				MPI_Bcast( Dz_Grid_Global, m_nz_Global , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;

				calculateCellPara( m_nz, m_nz_Global, LocalDomainIndex[idSolver].startZ, LocalDomainIndex[idSolver].endZ, Dz_Grid_Global, Dz_Half_Global, Z_Grid_Global, Z_Half_Global, 
					Dz_Grid, Dz_Half, Z_Grid, Z_Half, Dz_Grid_Inv, Dz_Half_Inv , m_boundaryLayerNum[ZMIN], m_boundaryLayerNum[ZMAX] ) ;

				break ;

			case TAG_EMATERIAL_START :
				{
					int result = readMaterial( in ) ;
					int sum = 0 ;
					MPI_Allreduce( &result, &sum, 1, MPI_INT, MPI_MAX, MPI_COMM_WORLD ) ;
					if( sum )
						return FDTD_NO_MEMORY ;
				}
				break ;
			case TAG_EXCITATION_VOLTAGE :
				{
					CFDTD_Source *source ;
					source = new CVoltage_Source ;
					source->setDGrid_Global( Dx_Grid_Global , Dy_Grid_Global , Dz_Grid_Global ) ;
					source->setDomainIndex( LocalDomainIndex ) ;
					source->setBoundaryLayerNum( m_boundaryLayerNum ) ;

					if( id == 0 )
						source->readIn( in ) ;
					else
						source->readIn_Solver( ) ;

					if( m_source == NULL )
					{
						m_source = source ;
					}
					else
					{
						CFDTD_Source *p, *next ;
						p = m_source ;
						next = m_source->getNext() ;
						while( next != NULL )
						{
							p = next ;
							if( p != NULL )
								next = p->getNext() ;
						}
						p->setNext( source ) ;
					}
					
				}
				break ;
			case TAG_EXCITATION_CURRENT :
				{
					
					CFDTD_Source *source ;
					source = new CCurrent_Source ;
					source->setDGrid_Global( Dx_Grid_Global , Dy_Grid_Global , Dz_Grid_Global ) ;
					source->setDomainIndex( LocalDomainIndex ) ;
					source->setBoundaryLayerNum( m_boundaryLayerNum ) ;

					if( id == 0 )
						source->readIn( in ) ;
					else
						source->readIn_Solver( ) ;

					if( m_source == NULL )
					{
						m_source = source ;
					}
					else
					{
						CFDTD_Source *p, *next ;
						p = m_source ;
						next = m_source->getNext() ;
						while( next != NULL )
						{
							p = next ;
							if( p != NULL )
								next = p->getNext() ;
						}
						p->setNext( source ) ;
					}
					
				}
				break ;
			case TAG_OUTPUT_VOLTAGE :
				{
					CFDTD_Output *output ;
					output = new CVoltage_Output ;
					output->setDomainIndex( LocalDomainIndex ) ;
					output->setBoundaryLayerNum( m_boundaryLayerNum ) ;

					if( id == 0 )
						output->readIn( in ) ;
					else
						output->readIn_Solver( ) ;

					if( m_output == NULL )
					{
						m_output = output ;
					}
					else
					{
						CFDTD_Output *p, *next ;
						p = m_output ;
						next = m_output->getNext() ;
						while( next != NULL )
						{
							p = next ;
							if( p != NULL )
								next = p->getNext() ;
						}
						p->setNext( output ) ;
					}
				}
				break ;
			case TAG_OUTPUT_CURRENT :
				{
					CFDTD_Output *output ;
					output = new CCurrent_Output ;
					output->setDomainIndex( LocalDomainIndex ) ;
					output->setBoundaryLayerNum( m_boundaryLayerNum ) ;

					if( id == 0 )
						output->readIn( in ) ;
					else
						output->readIn_Solver( ) ;

					if( m_output == NULL )
					{
						m_output = output ;
					}
					else
					{
						CFDTD_Output *p, *next ;
						p = m_output ;
						next = m_output->getNext() ;
						while( next != NULL )
						{
							p = next ;
							if( p != NULL )
								next = p->getNext() ;
						}
						p->setNext( output ) ;
					}
				}
				break ;
			case TAG_OUTPUT_FIELDONPOINT :
				{
					CFDTD_Output *output ;
					output = new CField_Output ;
					output->setDomainIndex( LocalDomainIndex ) ;
					output->setBoundaryLayerNum( m_boundaryLayerNum ) ;

					if( id == 0 )
						output->readIn( in ) ;
					else
						output->readIn_Solver( ) ;

					if( m_output == NULL )
					{
						m_output = output ;
					}
					else
					{
						CFDTD_Output *p, *next ;
						p = m_output ;
						next = m_output->getNext() ;
						while( next != NULL )
						{
							p = next ;
							if( p != NULL )
								next = p->getNext() ;
						}
						p->setNext( output ) ;
					}
				}
				break ;
			case TAG_END_FILE:
				endFlag = 1 ;
				break ;
		}
		
	}


	// release eMatTypeFlag
	Free_3D( eMatTypeFlag, m_nx + 3 ) ;

	// close project file
	if( id == 0 )
		fclose( in ) ;

	return FDTD_SUCCESS ;
}

bool CFDTD::setFileName( std::string fileName  )
{
	m_fileName = fileName ;
	errno_t err ;
	int errNo = 0 ;

	if( id == 0 )
	{
		err = fopen_s( &in,m_fileName.c_str(), "rb" ) ;
		if( err != 0 )
			errNo = 1 ;
	}

	MPI_Bcast( &errNo , 1 , MPI_INT, 0 , MPI_COMM_WORLD ) ;
	
	if( errNo != 0 )
		return false ;

	return true ;
}

bool CFDTD::setOutFileName( std::string fileName  ) 
{
	m_outFileName = fileName ;

	if( id == 0 )
	{
		if( m_outFile.openFile( m_outFileName.c_str() ) == false )
			return false ;
	}


	return true ;

}

int CFDTD::cellAllocateMemory( ) 
{
	// x global
	Dx_Grid_Global = Allocate_1D( Dx_Grid_Global, m_nx_Global + 3 ) ;
	if( Dx_Grid_Global == 0 )
		return FDTD_NO_MEMORY ;

	Dx_Half_Global = Allocate_1D( Dx_Half_Global, m_nx_Global + 3 ) ;
	if( Dx_Half_Global == 0 )
		return FDTD_NO_MEMORY ;

	X_Half_Global = Allocate_1D( X_Half_Global, m_nx_Global + 3 ) ;
	if( X_Half_Global == 0 )
		return FDTD_NO_MEMORY ;

	X_Grid_Global = Allocate_1D( X_Grid_Global, m_nx_Global + 3 ) ;
	if( X_Grid_Global == 0 )
		return FDTD_NO_MEMORY ;

	// y global
	Dy_Grid_Global = Allocate_1D( Dy_Grid_Global, m_ny_Global + 3 ) ;
	if( Dx_Grid_Global == 0 )
		return FDTD_NO_MEMORY ;

	Dy_Half_Global = Allocate_1D( Dy_Half_Global, m_ny_Global + 3 ) ;
	if( Dy_Half_Global == 0 )
		return FDTD_NO_MEMORY ;

	Y_Half_Global = Allocate_1D( Y_Half_Global, m_ny_Global + 3 ) ;
	if( Y_Half_Global == 0 )
		return FDTD_NO_MEMORY ;

	Y_Grid_Global = Allocate_1D( Y_Grid_Global, m_ny_Global + 3 ) ;
	if( Y_Grid_Global == 0 )
		return FDTD_NO_MEMORY ;

	// z global
	Dz_Grid_Global = Allocate_1D( Dz_Grid_Global, m_nz_Global + 3 ) ;
	if( Dz_Grid_Global == 0 )
		return FDTD_NO_MEMORY ;

	Dz_Half_Global = Allocate_1D( Dz_Half_Global, m_nz_Global + 3 ) ;
	if( Dz_Half_Global == 0 )
		return FDTD_NO_MEMORY ;

	Z_Half_Global = Allocate_1D( Z_Half_Global, m_nz_Global + 3 ) ;
	if( Z_Half_Global == 0 )
		return FDTD_NO_MEMORY ;

	Z_Grid_Global = Allocate_1D( Z_Grid_Global, m_nz_Global + 3 ) ;
	if( Z_Grid_Global == 0 )
		return FDTD_NO_MEMORY ;


	// local cell memory allocation 
	Dx_Grid = Allocate_1D( Dx_Grid, m_nx + 3 ) ;
	if( Dx_Grid == 0 )
		return FDTD_NO_MEMORY ;

	Dy_Grid = Allocate_1D( Dy_Grid, m_ny + 3 ) ;
	if( Dy_Grid == 0 )
		return FDTD_NO_MEMORY ;

	Dz_Grid = Allocate_1D( Dz_Grid, m_nz + 3 ) ;
	if( Dz_Grid == 0 )
		return FDTD_NO_MEMORY ;

	Dx_Half = Allocate_1D( Dx_Half, m_nx + 3 ) ;
	if( Dx_Half == 0 )
		return FDTD_NO_MEMORY ;

	Dy_Half = Allocate_1D( Dy_Half, m_ny + 3 ) ;
	if( Dy_Half == 0 )
		return FDTD_NO_MEMORY ;

	Dz_Half = Allocate_1D( Dz_Half, m_nz + 3 ) ;
	if( Dz_Half == 0 )
		return FDTD_NO_MEMORY ;

	// inv 
	Dx_Grid_Inv = Allocate_1D( Dx_Grid_Inv, m_nx + 3 ) ;
	if( Dx_Grid_Inv == 0 )
		return FDTD_NO_MEMORY ;

	Dy_Grid_Inv = Allocate_1D( Dy_Grid_Inv, m_ny + 3 ) ;
	if( Dy_Grid_Inv == 0 )
		return FDTD_NO_MEMORY ;

	Dz_Grid_Inv = Allocate_1D( Dz_Grid_Inv, m_nz + 3 ) ;
	if( Dz_Grid_Inv == 0 )
		return FDTD_NO_MEMORY ;

	Dx_Half_Inv = Allocate_1D( Dx_Half_Inv, m_nx + 3 ) ;
	if( Dx_Half_Inv == 0 )
		return FDTD_NO_MEMORY ;

	Dy_Half_Inv = Allocate_1D( Dy_Half_Inv, m_ny + 3 ) ;
	if( Dy_Half_Inv == 0 )
		return FDTD_NO_MEMORY ;

	Dz_Half_Inv = Allocate_1D( Dz_Half_Inv, m_nz + 3 ) ;
	if( Dz_Half_Inv == 0 )
		return FDTD_NO_MEMORY ;

	// grid and half
	X_Grid = Allocate_1D( X_Grid, m_nx + 3 ) ;
	if( X_Grid == 0 )
		return FDTD_NO_MEMORY ;

	Y_Grid = Allocate_1D( Y_Grid, m_ny + 3 ) ;
	if( Y_Grid == 0 )
		return FDTD_NO_MEMORY ;

	Z_Grid = Allocate_1D( Z_Grid, m_nz + 3 ) ;
	if( Z_Grid == 0 )
		return FDTD_NO_MEMORY ;

	X_Half = Allocate_1D( X_Half, m_nx + 3 ) ;
	if( X_Half == 0 )
		return FDTD_NO_MEMORY ;

	Y_Half = Allocate_1D( Y_Half, m_ny + 3 ) ;
	if( Y_Half == 0 )
		return FDTD_NO_MEMORY ;

	Z_Half = Allocate_1D( Z_Half, m_nz + 3 ) ;
	if( Z_Half == 0 )
		return FDTD_NO_MEMORY ;


	return FDTD_SUCCESS ;
}

int CFDTD::materialAllocateMemory( ) 
{
	int i, j , k ;

	eMatType = new CMaterial***[m_nx+3] ;
	if( eMatType == NULL )
		return FDTD_NO_MEMORY ;

	for( i = 0 ; i <= m_nx + 2 ; i++ )
	{
		eMatType[i] = new CMaterial**[m_ny+3] ;
		if( eMatType[i]  == 0 ) 
		{
			for( j = 0 ; j < i ; j++ )
			{
				delete []eMatType[j] ;
				eMatType[j] = NULL ;
			}
			return FDTD_NO_MEMORY ;
		}
	}

	for( i = 0 ; i <= m_nx + 2  ; i++ )
	{
		for( j = 0 ; j <= m_ny + 2 ; j++ )
		{
			eMatType[i][j] = new CMaterial*[m_nz+3] ;
			if( eMatType[i][j]  == 0 ) 
			{
				int m ;
				for( m = 0 ; m <= m_nx + 2 ; m++ )
				{
					for( k = 0 ; k < j ; k++ )
					{
						delete []eMatType[m][k] ;
						eMatType[m][k] = NULL ;
					}
				}
				return FDTD_NO_MEMORY ;
			}
		}
	}

	for( i = 0 ; i <= m_nx + 2  ; i++ )
	{
		for( j = 0 ; j <= m_ny + 2  ; j++ )
		{
			for( k = 0 ; k <= m_nz + 2  ; k++ )
				eMatType[i][j][k] = NULL ;
		}
	}

	eMatTypeFlag = Allocate_3D( eMatTypeFlag, m_nx+3, m_ny+3, m_nz+3 );
	if ( eMatTypeFlag == NULL ) 
		return FDTD_NO_MEMORY ;

	return FDTD_SUCCESS ;

}


void CFDTD::calculateCellPara( int n, int n_Global, int startIndex, int endIndex, float *D_Grid_Glboal, float * D_Half_Global, 
	float *Grid_Global, float *Half_Global, float *D_Grid, float *D_Half, float *Grid, float *Half , float *D_Grid_Inv, float *D_Half_Inv , int minLayerNum, int maxLayerNum )
{
	int i , j ;

	// this variavle is for debug use, need to be deleted when  code released
	static int index = 0 ;

	// calculate global grid  
	Grid_Global[0] = 0 ; 
	for( i = 1 ; i <=  n_Global ; i++ )
		Grid_Global[i] = Grid_Global[i-1] +  D_Grid_Glboal[i-1] ;

	// find the coordinates of the half-grid points
	for ( i = 0; i <= n_Global ; i++ ) 
		Half_Global[i] = Grid_Global[i] + 0.5f * D_Grid_Glboal[i] ;

	// calculate D_Half global
	D_Half_Global[0] = D_Grid_Glboal[0] ;
	for ( i = 1; i < n_Global ; i++ ) 
		D_Half_Global[i] = Half_Global[i] - Half_Global[i-1] ;


	if( id != 0 )
	{
		// get local dx grid size
		int localSize = endIndex - startIndex  ;
		for( i = 1 + minLayerNum, j = 0 ; j < localSize ; i++, j++ )
		{
			D_Grid[i] = D_Grid_Glboal[j+startIndex] ;
		}

		// determine the cell size between subdomain grid points
		// if this subdomain is at left side of global domain
		if( startIndex == 0 )
		{
			//D_Grid[0] = D_Grid[1] ;
			for( i = 0 ; i < minLayerNum+1 ; i++ )
			{
				D_Grid[i] = D_Grid[minLayerNum+1] ;
			}
		}
		else
		{
			// parallel or pec boundary
			D_Grid[0] = D_Grid_Glboal[startIndex-1] ;
		}


		// if this subdomain is at right side of global domain
		if( endIndex == n_Global )
		{
			for( i = 0 , j = localSize + 1 + minLayerNum ; i <= maxLayerNum ; i++ , j++)
			{
				D_Grid[j] = D_Grid[localSize + minLayerNum] ;
			}

		}
		else
		{
			// parallel or pec boundary
			D_Grid[localSize+ minLayerNum+1] = D_Grid_Glboal[endIndex] ;
			D_Grid[localSize+ minLayerNum+2] = D_Grid_Glboal[endIndex] ;
		}

		// get grid x
		Grid[0] = 0 ; 
		for( i = 1 ; i <= localSize + 2 + minLayerNum + maxLayerNum; i++ )
		{
			Grid[i] = Grid[i-1] +  D_Grid[i-1] ;
		}
		// get Dx grid inverse
		for( i = 0 ; i < localSize + 2 + minLayerNum + maxLayerNum ; i++ )
		{
			D_Grid_Inv[i] = 1.0f / D_Grid[i] ;
		}

		// find the coordinates of the half-grid points
		for ( i = 0; i <= localSize + 2 + minLayerNum + maxLayerNum ; i++ ) 
		{
			Half[i] =  Grid[i] + 0.5f * D_Grid[i] ;
		}
		// calculate dx half
		D_Half[0] = D_Grid[0] ;
		for ( i = 1; i < localSize + 2 + minLayerNum + maxLayerNum; i++ ) 
			D_Half[i] = 0.5f * ( D_Grid[i] + D_Grid[i-1]) ;

		// calculate dx half inv
		for ( i = 0; i < localSize + 2 + minLayerNum + maxLayerNum ; i++ ) 
			D_Half_Inv[i] = 1.0f / D_Half[i] ;
	}
	index++ ;
}

int CFDTD::readMaterial( FILE *in )
{
	int i , j , k , n ;
	char endMark[3] ;

	struct Material{
		int i , j , k ;
		float sigmaX , sigmaY, sigmaZ ;
		float epsilonX, epsilonY, epsilonZ ;
	} ;

	struct ZIndex
	{
		int start, end ;
	} ;

	Material *inBuf = NULL ;
	if( id == 0 )
		inBuf = new (std::nothrow) Material[(m_nz_Global+5) ] ;
	else
		inBuf = new (std::nothrow) Material[(m_nz+5)] ;

	std::vector< Material> matList ;
	Material mat ;

	std::vector< ZIndex > zIndexList ;		// parallel setting index in z direction
	ZIndex cIndex, nIndex ;

	if( id == 0 )
	{
		cIndex.start = LocalDomainIndex[0].startZ ;
		cIndex.end = LocalDomainIndex[0].endZ ;

		zIndexList.push_back( cIndex ) ;
		for( k = 1 ; k < numSolver ; k++ )
		{
			nIndex.start = LocalDomainIndex[k].startZ ;
			nIndex.end = LocalDomainIndex[k].endZ ;
			if( nIndex.start == cIndex.end )
			{
				cIndex = nIndex ;
				zIndexList.push_back( cIndex ) ;
			}
		}
	}

	if( id == 0 )
	{
		MPI_Request *request ;
		MPI_Status *status ;
		request = new MPI_Request[numSolver*2] ;
		status = new MPI_Status[numSolver*2] ;


		for( i = 0 ; i <= m_nx_Global ; i++ )
		{
			for( j = 0 ; j <= m_ny_Global ; j++ )
			{
				fread( inBuf, sizeof( Material ) , ( m_nz_Global + 1 ), in ) ;

				for( k = 0 ; k < numSolver ; k++ )
				{
					if( i >= LocalDomainIndex[k].startX - 1  && i <= LocalDomainIndex[k].endX + 1 && 
						j >= LocalDomainIndex[k].startY - 1 && j <= LocalDomainIndex[k].endY  + 1 )
					{
						for( n = 0 ; n < static_cast<int>( zIndexList.size() ) ; n++ )
						{
							if( zIndexList[n].start == LocalDomainIndex[k].startZ && zIndexList[n].end == LocalDomainIndex[k].endZ ) 
							{

								int num ;
								if( numSolver == 1 || static_cast<int>( zIndexList.size() ) == 1 )
								{
									num = zIndexList[n].end - zIndexList[n].start + 1 ;
								}
								else
								{
									if( n == 0 || n == static_cast<int>( zIndexList.size() ) - 1 )
										num = zIndexList[n].end - zIndexList[n].start + 2 ;
									else
										num = zIndexList[n].end - zIndexList[n].start + 3 ;
								}

								if( num )
								{
									int tag = k ;
									int target = k + 1 ;
									MPI_Isend( &num , 1 , MPI_INT, target, tag, MPI_COMM_WORLD, &request[2*k] ) ;
									MPI_Wait( &request[2*k], &status[2*k] ) ;
					
									tag++ ;
									if( n == 0 )
										MPI_Isend( inBuf + zIndexList[n].start , num * sizeof( Material ), MPI_CHAR, target, tag, MPI_COMM_WORLD, &request[2*k+1] ) ;
									else
										MPI_Isend( inBuf + zIndexList[n].start - 1 , num * sizeof( Material ), MPI_CHAR, target, tag, MPI_COMM_WORLD, &request[2*k+1] ) ;
									MPI_Wait( &request[2*k+1], &status[2*k+1] ) ;
								}
							}
 						}
					}
				} // end of for( k = 0 ; k < numSolver ; k++ )
				
			}
		}

		fread( endMark, sizeof( char ), 3, in ) ;

		// send end flag to all processor
		for( k = 0 ; k < numSolver ; k++ )
		{
			int target = k + 1 ;
			int num = 0 ;
			int tag = k ;
			MPI_Isend( &num , 1 , MPI_INT, target, tag, MPI_COMM_WORLD, &request[2*k] ) ;
			MPI_Wait( &request[0], &status[0] ) ;
		}

		delete[] request ;
		delete[] status ;


	}
	else
	{
		MPI_Request request[2] ;
		MPI_Status status[2] ;
		while( 1 )
		{
			int num = 0 ;
			int tag = idSolver ;
			MPI_Irecv( &num , 1 , MPI_INT, 0, tag , MPI_COMM_WORLD, &request[0] ) ;
			MPI_Wait( &request[0], &status[0] ) ;
			if( num == 0 )
			{
				break ;
			}
			else
			{

				tag++ ;
				MPI_Irecv( inBuf, num * sizeof( Material ), MPI_CHAR, 0, tag, MPI_COMM_WORLD, &request[1] ) ;
				MPI_Wait( &request[1], &status[1] ) ;

				// save material to cache for further use
				for( n = 0 ; n < num ; n++ )
				{
					memcpy( &mat, &( inBuf[n] ) , sizeof( Material ) ) ;
					matList.push_back( mat ) ;
				}
			}
		}
	}

	//exit(1) ;

	if( inBuf != NULL )
		delete[] inBuf ;

	// process Material information
	if( id == 0 )
		updateUnit() ;
	else
	{

		int x , y , z ;
		for( n = 0 ; n < static_cast<int>( matList.size() ) ; n++ )
		{

			mat = matList[n] ;
			Global2Local( mat.i, mat.j, mat.k, x, y, z , LocalDomainIndex[idSolver] , m_boundaryLayerNum ) ;

			CMaterial *p = CMaterial_Locate( eMaterials_Common,eMatType, eMatTypeFlag, x, y, z, X_DIRECTION, mat.epsilonX, mat.sigmaX );
			eMatType[x][y][z] = p;		
			
			p = CMaterial_Locate( eMaterials_Common,eMatType, eMatTypeFlag, x, y, z, Y_DIRECTION, mat.epsilonY, mat.sigmaY );
			eMatType[x][y][z] = p;		

			p = CMaterial_Locate( eMaterials_Common,eMatType, eMatTypeFlag, x, y, z, Z_DIRECTION, mat.epsilonZ, mat.sigmaZ );
			eMatType[x][y][z] = p;		
		}

		SetEMatPML( eMatType,Index_E_Boundary , m_boundaryLayerNum , m_nx, m_ny, m_nz ) ;
		

		// calculate material coffeicent
		updateUnit() ;

		CMaterial *p = eMaterials_Common , *next = NULL ;
		next = p->getNext() ;
		while( p != NULL )
		{
			p->CalculateEMaterialCoeff( Dt ) ;

			p = next ;
			if( p != NULL )
				next = p->getNext() ;
		}

	}
	// end of process Material information

	zIndexList.clear() ;
	matList.clear() ;

	return FDTD_SUCCESS ;
}


void CFDTD::updateUnit()
{
	int i , j , k ;

	m_domainRef.x *= unit ;
	m_domainRef.y *= unit ;
	m_domainRef.z *= unit ;

	// update global variables
	for( i = 0 ; i < m_nx_Global + 3 ; i++ )
	{
		Dx_Grid_Global[i] *= unit ;
		Dx_Half_Global[i] *= unit ;
		X_Half_Global[i] *= unit ;
		X_Grid_Global[i] *= unit ;
	}

	for( j = 0 ; j < m_ny_Global + 3 ; j++ )
	{
		Dy_Grid_Global[j] *= unit ;
		Dy_Half_Global[j] *= unit ;
		Y_Half_Global[j] *= unit ;
		Y_Grid_Global[j] *= unit ;
	}

	for( k = 0 ; k < m_nz_Global + 3 ; k++ )
	{
		Dz_Grid_Global[k] *= unit ;
		Dz_Half_Global[k] *= unit ;
		Z_Half_Global[k] *= unit ;
		Z_Grid_Global[k] *= unit ;
	}


	for( i = 0 ; i < m_nx + 3 ; i++ )
	{
		Dx_Grid[i] *= unit ;
		Dx_Half[i] *= unit ;
		X_Half[i] *= unit ;
		X_Grid[i] *= unit ;
		Dx_Grid_Inv[i] /= unit ;
		Dx_Half_Inv[i] /= unit ;
	}

	for( j = 0 ; j < m_ny + 3 ; j++ )
	{
		Dy_Grid[j] *= unit ;
		Dy_Half[j] *= unit ;
		Y_Half[j] *= unit ;
		Y_Grid[j] *= unit ;
		Dy_Grid_Inv[j] /= unit ;
		Dy_Half_Inv[j] /= unit ;
	}

	for( k = 0 ; k < m_nz + 3 ; k++ )
	{
		Dz_Grid[k] *= unit ;
		Dz_Half[k] *= unit ;
		Z_Half[k] *= unit ;
		Z_Grid[k] *= unit ;
		Dz_Grid_Inv[k] /= unit ;
		Dz_Half_Inv[k] /= unit ;
	}
}
