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

#include "FarField.h"

#include "GEMS_Constant.h"
#include "GEMS_Memory.h"

#include <mpi.h>
#include <cmath>
#include <iostream>

CFarField::CFarField(void)
{
	m_enabled = false ;

	m_freqUnit = GHz ;

	m_freqList = NULL ;
	m_freqList_Inv = NULL ;
	m_freqNum = 0 ;
	m_huygensBox_Ref.x = m_huygensBox_Ref.y = m_huygensBox_Ref.z = 0.0f ;

	int i ;
	for( i = 0 ; i < 6 ; i++ )
	{
		m_huy_FaceFlag[i] = false ;
		m_huy_FaceFlag_Global[i] = false ;
		m_boundaryLayerNum[i] = 0 ;
	}

	m_phiEnabled = false ;
	m_thetaEnabled = false ;
	m_phiList = NULL ;
	m_thetaList = NULL ;
	m_thetaStart = m_thetaEnd = m_thetaStep = 1 ;
	m_phiStart = m_phiEnd = m_phiStep = 1 ;
	m_phiNum = 0 ;
	m_thetaNum = 0 ;


	m_use_MJ = true ;

	m_Huy_S1 = m_Huy_S2 = m_Huy_S3 = NULL ;
	m_Huy_S4 = m_Huy_S5 = m_Huy_S6 = NULL ;

	m_radiationPower = NULL ;

	m_sourceNum = 1 ;
	m_sourceReal = NULL ;
	m_sourceImag = NULL ;

	m_powerReal = NULL ;
	m_powerImag = NULL ;

	m_x_Center = m_y_Center = m_z_Center = NULL ;
	m_x_Interface = m_y_Interface = m_z_Interface = NULL ;
	m_x_Interface_Global = m_y_Interface_Global = m_z_Interface_Global = NULL ;


	// Intermediate variables used in DFT calculation
	m_Huy_xMin_Wg1 = m_Huy_xMin_Wg2 = m_Huy_xMax_Wg1 = m_Huy_xMax_Wg2 = 1.0 ;
	m_Huy_yMin_Wg1 = m_Huy_yMin_Wg2 = m_Huy_yMax_Wg1 = m_Huy_yMax_Wg2 = 1.0 ;
	m_Huy_zMin_Wg1 = m_Huy_zMin_Wg2 = m_Huy_zMax_Wg1 = m_Huy_zMax_Wg2 = 1.0 ;



	m_Dx_Grid_Global = m_Dy_Grid_Global = m_Dz_Grid_Global = NULL ;

	m_localDomainIndex = NULL ;
	
}

CFarField::~CFarField(void)
{
	Free_2D( m_sourceReal ) ;
	Free_2D( m_sourceImag ) ;
	Free_2D( m_powerReal ) ;
	Free_2D( m_powerImag ) ;

	Free_1D( m_radiationPower ) ;

	Free_1D( m_freqList_Inv ) ;
	Free_1D( m_freqList ) ;
	Free_1D( m_phiList ) ;
	Free_1D( m_thetaList ) ;

	Free_1D( m_x_Center ) ;
	Free_1D( m_y_Center ) ;
	Free_1D( m_z_Center ) ;

	Free_1D( m_x_Interface ) ;
	Free_1D( m_y_Interface ) ;
	Free_1D( m_z_Interface ) ;

	Free_1D( m_x_Interface_Global ) ;
	Free_1D( m_y_Interface_Global ) ;
	Free_1D( m_z_Interface_Global ) ;

	Free_3D( m_Huy_S1 , m_freqNum  ) ;
	Free_3D( m_Huy_S2 , m_freqNum ) ;
	Free_3D( m_Huy_S3 , m_freqNum ) ;

	Free_3D( m_Huy_S4 , m_freqNum ) ;
	Free_3D( m_Huy_S5 , m_freqNum ) ;
	Free_3D( m_Huy_S6 , m_freqNum ) ;

	
}

bool CFarField::getEnabled( ) 
{
	return m_enabled ;
}


void CFarField::setDt( float Dt )
{
	m_Dt = Dt ;
}


int CFarField::readIn_Solver( )
{
	int i ;
	char enabled ;
	char user_MJ ;
	char faceFlag ;
	char phiEnabled ;
	char thetaEnabled ;

	MPI_Bcast( &enabled , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	m_enabled = enabled != 0 ? true : false ;

	if( enabled == false )
		return FDTD_SUCCESS ;

	MPI_Bcast( &user_MJ , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &faceFlag , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( &m_huygensBox_Global , 6 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_huygensBox_Ref , 3 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	
	int unit ;
	MPI_Bcast( &unit , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	m_freqUnit = static_cast<FrequencyUnit>( unit ) ;

	MPI_Bcast( &m_freqNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	if( m_freqNum > 0 )
	{
		m_freqList = new float[m_freqNum] ;
		MPI_Bcast( m_freqList , m_freqNum , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
		m_freqList_Inv = new float[m_freqNum] ;
		for( i = 0 ; i < m_freqNum ; i++ )
			m_freqList_Inv[i] = 1.0f / m_freqList[i] ;
	}

	// phi 2D
	MPI_Bcast( &phiEnabled , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_phiNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	if( m_phiNum > 0 )
	{
		m_phiList = new float[m_phiNum] ;
		MPI_Bcast( m_phiList , m_phiNum , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	}

	MPI_Bcast( &m_thetaStart , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_thetaEnd , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_thetaStep , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;


	// theta 2D
	MPI_Bcast( &thetaEnabled , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_thetaNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	if( m_thetaNum > 0 )
	{
		m_thetaList = new float[m_thetaNum] ;
		MPI_Bcast( m_thetaList , m_thetaNum , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	}

	MPI_Bcast( &m_phiStart , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_phiEnd , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_phiStep , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;


	m_use_MJ = user_MJ != 0 ? true : false ;

	m_phiEnabled = phiEnabled != 0 ? true : false ;
	m_thetaEnabled = thetaEnabled != 0 ? true : false ;

	m_huy_FaceFlag_Global[XMIN] = faceFlag & 0x01 ? true : false ;
	m_huy_FaceFlag_Global[XMAX] = faceFlag & 0x08 ? true : false ;
	m_huy_FaceFlag_Global[YMIN] = faceFlag & 0x02 ? true : false ;
	m_huy_FaceFlag_Global[YMAX] = faceFlag & 0x10 ? true : false ;
	m_huy_FaceFlag_Global[ZMIN] = faceFlag & 0x04 ? true : false ;
	m_huy_FaceFlag_Global[ZMAX] = faceFlag & 0x20 ? true : false ;

	return FDTD_SUCCESS ;
}

int CFarField::readIn(FILE *in)
{
	int i ;
	char enabled ;
	char user_MJ ;
	char faceFlag ;
	char endMark[2] ;

	fread( &enabled, sizeof( char ), 1, in ) ;
	fread( &user_MJ, sizeof( char ), 1, in ) ;
	fread( &faceFlag, sizeof( char ), 1, in ) ;
	fread( endMark, sizeof( char ), 1, in ) ;

	fread( &m_huygensBox_Global, sizeof( HuygensBox ), 1, in ) ;

	fread( endMark, sizeof( char ), 1, in ) ;


	fread( &m_huygensBox_Ref, sizeof( Point ), 1, in ) ;

	fread( endMark, sizeof( char ), 1, in ) ;

	
	// Far Field 2D Pattern
	char phiEnabled ;
	fread( &phiEnabled, sizeof( char ), 1, in ) ;

	fread( &m_phiNum, sizeof( int ), 1, in ) ;
	if( m_phiNum > 0 )
	{
		m_phiList = new float[m_phiNum] ;
		fread( m_phiList, sizeof( int ), m_phiNum, in ) ;
	}
	fread( &m_thetaStart, sizeof( float ), 1, in ) ;
	fread( &m_thetaEnd, sizeof( float ), 1, in ) ;
	fread( &m_thetaStep, sizeof( float ), 1, in ) ;

	fread( endMark, sizeof( char ), 1, in ) ;


	char thetaEnabled ;
	fread( &thetaEnabled, sizeof( char ), 1, in ) ;

	fread( &m_thetaNum, sizeof( int ), 1, in ) ;
	if( m_thetaNum > 0 )
	{
		m_thetaList = new float[m_thetaNum] ;
		fread( m_thetaList, sizeof( int ), m_thetaNum, in ) ;
	}

	fread( &m_phiStart, sizeof( float ), 1, in ) ;
	fread( &m_phiEnd, sizeof( float ), 1, in ) ;
	fread( &m_phiStep, sizeof( float ), 1, in ) ;

	fread( endMark, sizeof( char ), 1, in ) ;

	// input frequency information
	int unit ;
	fread( &unit, sizeof( int ), 1, in ) ;
	m_freqUnit = static_cast<FrequencyUnit>( unit ) ;

	fread( &m_freqNum, sizeof( int ), 1, in ) ;
	if( m_freqNum > 0 )
	{
		m_freqList = new float[m_freqNum] ;
		m_freqList_Inv = new float[m_freqNum] ;
		fread( m_freqList, sizeof( int ), m_freqNum, in ) ;
		for( i = 0 ; i < m_freqNum ; i++ )
		{
			m_freqList_Inv[i] = 1.0f / m_freqList[i] ;
		}
	}
	fread( endMark, sizeof( char ), 2, in ) ;

	if( m_freqNum <= 0 )
		enabled = 0 ;

	if( m_phiNum <= 0 )
		phiEnabled = 0 ;

	if( m_thetaNum <= 0 )
		thetaEnabled = 0 ;

	m_enabled = enabled != 0 ? true : false ;

	MPI_Bcast( &enabled , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	if( m_enabled == false )
		return FDTD_SUCCESS ;

	MPI_Bcast( &user_MJ , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &faceFlag , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;

	MPI_Bcast( &m_huygensBox_Global , 6 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_huygensBox_Ref , 3 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;

	// freq
	MPI_Bcast( &unit , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_freqNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	if( m_freqNum )
		MPI_Bcast( m_freqList , m_freqNum , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;

	// phi 2D
	MPI_Bcast( &phiEnabled , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_phiNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	if( m_phiNum )
		MPI_Bcast( m_phiList , m_phiNum , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_thetaStart , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_thetaEnd , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_thetaStep , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;


	// theta 2D
	MPI_Bcast( &thetaEnabled , 1 , MPI_CHAR , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_thetaNum , 1 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	if( m_thetaNum )
		MPI_Bcast( m_thetaList , m_thetaNum , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_phiStart , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_phiEnd , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;
	MPI_Bcast( &m_phiStep , 1 , MPI_FLOAT , 0 , MPI_COMM_WORLD ) ;


	//
	m_use_MJ = user_MJ != 0 ? true : false ;
	
	m_phiEnabled = phiEnabled != 0 ? true : false ;
	m_thetaEnabled = thetaEnabled != 0 ? true : false ;


	m_huy_FaceFlag_Global[XMIN] = faceFlag & 0x01 ? true : false ;
	m_huy_FaceFlag_Global[XMAX] = faceFlag & 0x08 ? true : false ;
	m_huy_FaceFlag_Global[YMIN] = faceFlag & 0x02 ? true : false ;
	m_huy_FaceFlag_Global[YMAX] = faceFlag & 0x10 ? true : false ;
	m_huy_FaceFlag_Global[ZMIN] = faceFlag & 0x04 ? true : false ;
	m_huy_FaceFlag_Global[ZMAX] = faceFlag & 0x20 ? true : false ;

	m_huy_FaceFlag[XMIN] = faceFlag & 0x01 ? true : false ;
	m_huy_FaceFlag[XMAX] = faceFlag & 0x08 ? true : false ;
	m_huy_FaceFlag[YMIN] = faceFlag & 0x02 ? true : false ;
	m_huy_FaceFlag[YMAX] = faceFlag & 0x10 ? true : false ;
	m_huy_FaceFlag[ZMIN] = faceFlag & 0x04 ? true : false ;
	m_huy_FaceFlag[ZMAX] = faceFlag & 0x20 ? true : false ;

	return FDTD_SUCCESS ;
}

void CFarField::setDomainIndex( DomainIndex *localDomainIndex ) 
{
	m_localDomainIndex = localDomainIndex ;
}

void CFarField::setSourceNum( int num ) 
{
	m_sourceNum = num ;
}

int CFarField::getFreqNum() const
{
	return m_freqNum ;
}


float CFarField::getFreq( int index )
{
	return m_freqList[index] ;
}


void CFarField::getSourcePower( float &sourceReal, float &sourceImag, float &powerReal, float &powerImag , int freqIndex, int sourceIndex)
{
	sourceReal = m_sourceReal[freqIndex][sourceIndex] ;
	sourceImag = m_sourceImag[freqIndex][sourceIndex] ;
	powerReal = m_powerReal[freqIndex][sourceIndex] ;
	powerImag = m_powerImag[freqIndex][sourceIndex] ;
}

void CFarField::setSourcePower( float sourceReal, float sourceImag, float powerReal, float powerImag , int freqIndex, int sourceIndex )
{
	m_sourceReal[freqIndex][sourceIndex] = sourceReal ;
	m_sourceImag[freqIndex][sourceIndex] = sourceImag ;
	m_powerReal[freqIndex][sourceIndex] = powerReal ;
	m_powerImag[freqIndex][sourceIndex] = powerImag ;
}


// Initialize the parameter of farfield
int CFarField::init(void)
{

	if( m_enabled == false )
		return FDTD_SUCCESS ;

	if( id == 0 )
	{
		m_sourceReal = Allocate_2D( m_sourceReal, m_freqNum , m_sourceNum ) ;
		if( m_sourceReal == NULL )
			return FDTD_NO_MEMORY ;

		m_sourceImag = Allocate_2D( m_sourceImag, m_freqNum , m_sourceNum ) ;
		if( m_sourceImag == NULL )
			return FDTD_NO_MEMORY ;

		m_powerReal = Allocate_2D( m_powerReal, m_freqNum , m_sourceNum ) ;
		if( m_powerReal == NULL )
			return FDTD_NO_MEMORY ;

		m_powerImag = Allocate_2D( m_powerImag, m_freqNum , m_sourceNum ) ;
		if( m_powerImag == NULL )
			return FDTD_NO_MEMORY ;
		
		m_radiationPower = Allocate_1D( m_radiationPower, m_freqNum ) ;
		if( m_radiationPower == NULL )
			return FDTD_NO_MEMORY ;
	}
	else
	{
		
		int i, j, k, ind ;
		int nx0_tmp, nx1_tmp, nx2_tmp, ny0_tmp, ny1_tmp, ny2_tmp, nz0_tmp, nz1_tmp, nz2_tmp;

		// Compute the Huygen's box range in the local domain
		nx1_tmp = m_huygensBox_Global.startX ;
		nx2_tmp = m_localDomainIndex[idSolver].startX ;
		m_huygensBox.startX = nx1_tmp < nx2_tmp ? 0 : nx1_tmp - nx2_tmp;


		nx0_tmp = m_localDomainIndex[idSolver].startX ;
		nx1_tmp = m_huygensBox_Global.endX ;
		nx2_tmp = m_localDomainIndex[idSolver].endX ;
		m_huygensBox.endX = nx1_tmp >= nx2_tmp ? nx2_tmp - nx0_tmp : nx1_tmp - nx0_tmp;

		ny1_tmp = m_huygensBox_Global.startY ;
		ny2_tmp = m_localDomainIndex[idSolver].startY ;
		m_huygensBox.startY = ny1_tmp < ny2_tmp ? 0 : ny1_tmp - ny2_tmp;

		ny0_tmp = m_localDomainIndex[idSolver].startY ;
		ny1_tmp = m_huygensBox_Global.endY ;
		ny2_tmp = m_localDomainIndex[idSolver].endY ;
		m_huygensBox.endY = ny1_tmp >= ny2_tmp ? ny2_tmp - ny0_tmp : ny1_tmp - ny0_tmp;

		nz1_tmp = m_huygensBox_Global.startZ ;
		nz2_tmp = m_localDomainIndex[idSolver].startZ ;
		m_huygensBox.startZ = nz1_tmp < nz2_tmp ? 0 : nz1_tmp - nz2_tmp;

		nz0_tmp = m_localDomainIndex[idSolver].startZ ;
		nz1_tmp = m_huygensBox_Global.endZ ;
		nz2_tmp = m_localDomainIndex[idSolver].endZ ;
		m_huygensBox.endZ = nz1_tmp >= nz2_tmp ? 	nz2_tmp-nz0_tmp : nz1_tmp - nz0_tmp;

		//the boundary thinkness is taken into account in each subdomain, but is not in the global domain.
		m_huygensBox.startX += 1 + m_boundaryLayerNum[XMIN] ;
		if( m_huygensBox.startX < 0 ) m_huygensBox.startX = 0 ;
		if( m_huygensBox.startX > m_nx ) m_huygensBox.startX = m_nx ;

		m_huygensBox.startY += 1 + m_boundaryLayerNum[YMIN] ;
		if( m_huygensBox.startY < 0 ) m_huygensBox.startY = 0 ;
		if( m_huygensBox.startY > m_ny ) m_huygensBox.startY = m_ny ;

		m_huygensBox.startZ += 1 + m_boundaryLayerNum[ZMIN] ;
		if( m_huygensBox.startZ < 0 ) m_huygensBox.startZ = 0 ;
		if( m_huygensBox.startZ > m_nz ) m_huygensBox.startZ = m_nz ;


		m_huygensBox.endX += 1 + m_boundaryLayerNum[XMIN] ;
		if( m_huygensBox.endX < 0 ) m_huygensBox.endX = 0 ;
		if( m_huygensBox.endX > m_nx ) m_huygensBox.endX = m_nx ;

		m_huygensBox.endY += 1 + m_boundaryLayerNum[YMIN] ;
		if( m_huygensBox.endY < 0 ) m_huygensBox.endY = 0 ;
		if( m_huygensBox.endY > m_ny ) m_huygensBox.endY = m_ny ;

		m_huygensBox.endZ += 1 + m_boundaryLayerNum[ZMIN] ;
		if( m_huygensBox.endZ < 0 ) m_huygensBox.endZ = 0 ;
		if( m_huygensBox.endZ > m_nz ) m_huygensBox.endZ = m_nz ;

		//The flags are set up in the GUI by the user	
		for ( ind = 0; ind < 6; ind++ )
		{
			m_huy_FaceFlag[ind] = m_huy_FaceFlag_Global[ind] ;
		}

		//-----------------------------------------------------
		//XMIN: 0; XMAX: 1; YMIN: 2; YMAX: 3; ZMIN: 4; ZMAX: 5.
		//idSolver: index of subdomain in parallel processing.
		//m_localDomainIndex[idSolver].startX, Y, Z: Range of subdomain in the global domain. 
		//-----------------------------------------------------
		//find the face contribution of the Huygens' box in the local domain
		if ( m_huygensBox_Global.startX   > m_localDomainIndex[idSolver].endX 
			|| m_huygensBox_Global.endX   < m_localDomainIndex[idSolver].startX 
			|| m_huygensBox_Global.startY > m_localDomainIndex[idSolver].endY
			|| m_huygensBox_Global.endY   < m_localDomainIndex[idSolver].startY
			|| m_huygensBox_Global.startZ > m_localDomainIndex[idSolver].endZ
			|| m_huygensBox_Global.endZ   < m_localDomainIndex[idSolver].startZ)
		{
			for ( ind = 0; ind < 6; ind++ )
			{
				m_huy_FaceFlag[ind] = false ;
			}	
		}

	//-----------------------------------------------------------------------------------
		if ( m_huygensBox_Global.startX   <  m_localDomainIndex[idSolver].startX )
		{
				m_huy_FaceFlag[XMIN] = false ;
		}
		if ( m_huygensBox_Global.startY   <  m_localDomainIndex[idSolver].startY)
		{
				m_huy_FaceFlag[YMIN] = false ;
		}
		if ( m_huygensBox_Global.startZ   <  m_localDomainIndex[idSolver].startZ )
		{
				m_huy_FaceFlag[ZMIN] = false ;
		}

		if ( m_huygensBox_Global.endX   > m_localDomainIndex[idSolver].endX)
		{
				m_huy_FaceFlag[XMAX] = false ;
		}
		if ( m_huygensBox_Global.endY   > m_localDomainIndex[idSolver].endY)
		{
				m_huy_FaceFlag[YMAX] = false ;
		}
		if ( m_huygensBox_Global.endZ   > m_localDomainIndex[idSolver].endZ)
		{
				m_huy_FaceFlag[ZMAX] = false ;
		}
	//-----------------------------------------------------------------------------------

		if (   m_huygensBox_Global.startX ==  m_localDomainIndex[idSolver].startX)
		{
				m_huy_FaceFlag[XMIN] = false ;
		}
		if (   m_huygensBox_Global.endX ==  m_localDomainIndex[idSolver].startX)
		{
				m_huy_FaceFlag[XMAX] = false ;
		}

		if (   m_huygensBox_Global.startY ==  m_localDomainIndex[idSolver].startY)
		{
				m_huy_FaceFlag[YMIN] = false ;
		}
		if (   m_huygensBox_Global.endY ==  m_localDomainIndex[idSolver].startY)
		{
				m_huy_FaceFlag[YMAX] = false ;
		}
		if ( m_huygensBox_Global.startZ   ==  m_localDomainIndex[idSolver].startZ )
		{
				m_huy_FaceFlag[ZMIN] = false ;
		}
		if ( m_huygensBox_Global.endZ   ==  m_localDomainIndex[idSolver].startZ )
		{
				m_huy_FaceFlag[ZMAX] = false ;
		}

		if ( m_freqNum == 0 )
			return OTHER_ERROR ;

		// Compute the cell number in each direction on huygen's box
		m_nHuy_Curr[0] = m_huygensBox.endX - m_huygensBox.startX ;
		m_nHuy_Curr[1] = m_huygensBox.endY - m_huygensBox.startY ;
		m_nHuy_Curr[2] = m_huygensBox.endZ - m_huygensBox.startZ ;
		
		if( m_nHuy_Curr[0] < 0 ) m_nHuy_Curr[0] = 0 ;
		if( m_nHuy_Curr[1] < 0 ) m_nHuy_Curr[1] = 0 ;
		if( m_nHuy_Curr[2] < 0 ) m_nHuy_Curr[2] = 0 ;


		m_x_Center = new float[m_nHuy_Curr[0] + 1] ;
		m_y_Center = new float[m_nHuy_Curr[1] + 1] ;
		m_z_Center = new float[m_nHuy_Curr[2] + 1] ;

		m_x_Interface = new float[m_nHuy_Curr[0] + 1] ;
		m_y_Interface = new float[m_nHuy_Curr[1] + 1] ;
		m_z_Interface = new float[m_nHuy_Curr[2] + 1] ;

		m_x_Interface_Global = new float[m_nHuy_Curr[0] + 1] ;
		m_y_Interface_Global = new float[m_nHuy_Curr[1] + 1] ;
		m_z_Interface_Global = new float[m_nHuy_Curr[2] + 1] ;


		for (i = 0; i < m_nHuy_Curr[0]; i++)
		{
			m_x_Center[i] = 0.0; 
			m_x_Interface[i] = 0.0;
			m_x_Interface_Global[i] = 0.0;
		}
		for (j = 0; j < m_nHuy_Curr[1]; j++)
		{
			m_y_Center[j] = 0.0; 
			m_y_Interface[j] = 0.0;
			m_y_Interface_Global[j] = 0.0;
		}
		for (k = 0; k < m_nHuy_Curr[2]; k++)
		{
			m_z_Center[k] = 0.0; 
			m_z_Interface[k] = 0.0;
			m_z_Interface_Global[k] = 0.0;
		}

		//              --------------------
		for ( i = 0 ; i < m_nHuy_Curr[0] ; i++ )
		{
			m_x_Center[i] = m_X_Half_Global[m_Nx_Local2Global[i + m_huygensBox.startX]] ;
		} 

		for ( j = 0 ; j < m_nHuy_Curr[1] ; j++ )
		{
			m_y_Center[j] = m_Y_Half_Global[m_Ny_Local2Global[j + m_huygensBox.startY]] ;
		}
		
		for ( k = 0 ; k < m_nHuy_Curr[2] ; k++ )
		{
			m_z_Center[k] = m_Z_Half_Global[m_Nz_Local2Global[k + m_huygensBox.startZ]] ;
		}

		// interface
		for ( i = 0 ; i < m_nHuy_Curr[0] + 1 ; i++ )
		{
			m_x_Interface_Global[i] = m_X_Grid_Global[m_Nx_Local2Global[i + m_huygensBox.startX]] ;
		} 

		for ( j = 0 ; j < m_nHuy_Curr[1] + 1 ; j++ )
		{
			m_y_Interface_Global[j] = m_Y_Grid_Global[m_Ny_Local2Global[j + m_huygensBox.startY]] ;
		}
		
		for ( k = 0 ; k < m_nHuy_Curr[2] + 1 ; k++ )
		{
			m_z_Interface_Global[k] = m_Z_Grid_Global[m_Nz_Local2Global[k + m_huygensBox.startZ]] ;
		}

	//----------------------

		// Allocate memory for all intermediate variables 
		if( AllocateMem( ) == false )
			return FDTD_NO_MEMORY ;

		float dx_tmp_m, dy_tmp_m, dz_tmp_m;
		float dx_tmp, dy_tmp, dz_tmp;

		dx_tmp_m = m_Dx_Grid_Global[m_huygensBox_Global.startX - 1] ;
		dx_tmp   = m_Dx_Grid_Global[m_huygensBox_Global.startX] ;
		m_Huy_xMin_Wg1 = 0.5f * dx_tmp_m / ( dx_tmp_m + dx_tmp ) ;

		dx_tmp_m = m_Dx_Grid_Global[m_huygensBox_Global.startX - 1] ;
		dx_tmp   = m_Dx_Grid_Global[m_huygensBox_Global.startX] ;
		m_Huy_xMin_Wg2 = 0.5f * dx_tmp / (dx_tmp_m + dx_tmp) ; 

		dx_tmp_m = m_Dx_Grid_Global[m_huygensBox_Global.endX - 1] ;
		dx_tmp   = m_Dx_Grid_Global[m_huygensBox_Global.endX] ;
		m_Huy_xMax_Wg1 = 0.5f * dx_tmp_m / (dx_tmp_m + dx_tmp ) ; 

		dx_tmp_m = m_Dx_Grid_Global[m_huygensBox_Global.endX - 1] ;
		dx_tmp   = m_Dx_Grid_Global[m_huygensBox_Global.endX] ;
		m_Huy_xMax_Wg2 = 0.5f * dx_tmp / (dx_tmp_m + dx_tmp) ;

		dy_tmp_m = m_Dy_Grid_Global[m_huygensBox_Global.startY - 1] ;
		dy_tmp   = m_Dy_Grid_Global[m_huygensBox_Global.startY] ;
		m_Huy_yMin_Wg1 = 0.5f * dy_tmp_m / (dy_tmp_m + dy_tmp);

		dy_tmp_m = m_Dy_Grid_Global[m_huygensBox_Global.startY - 1] ;
		dy_tmp   = m_Dy_Grid_Global[m_huygensBox_Global.startY] ;
		m_Huy_yMin_Wg2 = 0.5f * dy_tmp / (dy_tmp_m + dy_tmp);

		dy_tmp_m = m_Dy_Grid_Global[m_huygensBox_Global.endY - 1] ;
		dy_tmp   = m_Dy_Grid_Global[m_huygensBox_Global.endY] ;
		m_Huy_yMax_Wg1 = 0.5f * dy_tmp_m / (dy_tmp_m + dy_tmp);
		
		dy_tmp_m = m_Dy_Grid_Global[m_huygensBox_Global.endY - 1] ;
		dy_tmp   = m_Dy_Grid_Global[m_huygensBox_Global.endY] ;
		m_Huy_yMax_Wg2 = 0.5f * dy_tmp / ( dy_tmp_m + dy_tmp ) ;

		dz_tmp_m = m_Dz_Grid_Global[m_huygensBox_Global.startZ - 1] ;
		dz_tmp   = m_Dz_Grid_Global[m_huygensBox_Global.startZ] ;
		m_Huy_zMin_Wg1 = 0.5f * dz_tmp_m / (dz_tmp_m + dz_tmp);
		
		dz_tmp_m = m_Dz_Grid_Global[m_huygensBox_Global.startZ - 1] ;
		dz_tmp   = m_Dz_Grid_Global[m_huygensBox_Global.startZ] ;
		m_Huy_zMin_Wg2 = 0.5f * dz_tmp / (dz_tmp_m + dz_tmp);

		dz_tmp_m = m_Dz_Grid_Global[m_huygensBox_Global.endZ - 1] ;
		dz_tmp   = m_Dz_Grid_Global[m_huygensBox_Global.endZ] ;
		m_Huy_zMax_Wg1 = 0.5f * dz_tmp_m / (dz_tmp_m + dz_tmp);

		dz_tmp_m = m_Dz_Grid_Global[m_huygensBox_Global.endZ - 1] ;
		dz_tmp   = m_Dz_Grid_Global[m_huygensBox_Global.endZ] ;
		m_Huy_zMax_Wg2 = 0.5f * dz_tmp / (dz_tmp_m + dz_tmp);
	}


	return FDTD_SUCCESS;
}


bool CFarField::AllocateMem( )
{


	m_radiationPower = Allocate_1D( m_radiationPower, m_freqNum ) ;
	if( m_radiationPower == NULL )
		return false ;

	if( m_huy_FaceFlag[XMIN] )
	{
		m_Huy_S1 = Allocate_3D( m_Huy_S1, m_freqNum , m_nHuy_Curr[1]+1 , m_nHuy_Curr[2]+1 );
		if( m_Huy_S1 == NULL )
			return false ;
	}


	if( m_huy_FaceFlag[XMAX] )
	{
		m_Huy_S2 = Allocate_3D( m_Huy_S2, m_freqNum , m_nHuy_Curr[1]+1 , m_nHuy_Curr[2]+1 );
		if( m_Huy_S2 == NULL )
			return false ;
	}

	if( m_huy_FaceFlag[YMIN] )
	{
		m_Huy_S3 = Allocate_3D( m_Huy_S3, m_freqNum , m_nHuy_Curr[2]+1 , m_nHuy_Curr[0]+1 );
		if( m_Huy_S3 == NULL )
			return false ;
	}

	if( m_huy_FaceFlag[YMAX] )
	{
		m_Huy_S4 = Allocate_3D( m_Huy_S4, m_freqNum , m_nHuy_Curr[2]+1 , m_nHuy_Curr[0]+1 );
		if( m_Huy_S4 == NULL )
			return false ;

	}


	if( m_huy_FaceFlag[ZMIN] )
	{
		m_Huy_S5 = Allocate_3D( m_Huy_S5, m_freqNum , m_nHuy_Curr[0]+1 , m_nHuy_Curr[1]+1 );
		if( m_Huy_S5 == NULL )
			return false ;


	}

	if( m_huy_FaceFlag[ZMAX] )
	{
		m_Huy_S6 = Allocate_3D( m_Huy_S6, m_freqNum , m_nHuy_Curr[0]+1 , m_nHuy_Curr[1]+1 );
		if( m_Huy_S6 == NULL )
			return false ;
	}


	return true ;
}


void CFarField::farField_EM_Current(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	int freqIndex, i, j, k, i1, j1, k1;		// loop index
	float ex_tmp, ey_tmp, ez_tmp, hx_tmp, hy_tmp, hz_tmp;
	
	float dt_tmp_e, dt_tmp_h;
	
	std::complex<float> exp_e, exp_h, comp_tmp; 

	// --------------------------------------------------------------------------------
	for ( freqIndex = 0; freqIndex < m_freqNum ; freqIndex ++ )
	{

		float period_temp = 1.0f / m_freqList[freqIndex];
		int n_temp ;

		n_temp = 1 ;

		dt_tmp_e = 2 * PI * m_freqList[freqIndex] * ( n + 0.5f ) * m_Dt ;
		exp_e = std::complex<float>( n_temp * cos( dt_tmp_e ), - n_temp * sin( dt_tmp_e )) ;

		dt_tmp_h = 2 * PI * m_freqList[freqIndex] * n * m_Dt ;
		exp_h = std::complex<float>(n_temp * cos( dt_tmp_h ), - n_temp * sin( dt_tmp_h )) ;

		// xmin
		if ( m_huy_FaceFlag[XMIN] )
		{
			i = m_huygensBox.startX ;

			for ( j = m_huygensBox.startY ; j < m_huygensBox.endY ; j ++ )
			{
				for ( k = m_huygensBox.startZ ; k < m_huygensBox.endZ ; k ++ )
				{
					j1 = j - m_huygensBox.startY ;
					k1 = k - m_huygensBox.startZ ;

					//move the factor 0.5f of electric and magnetic currents 
					//from here to subroutine Convert_HuyCurr to avoid multiplying at each time step
					ey_tmp = 0.5f * (Ey[i][j][k] + Ey[i][j][k + 1]) ;	
					ez_tmp = 0.5f * (Ez[i][j][k] + Ez[i][j + 1][k]) ;

					hy_tmp = ( Hy[i - 1][j][k] + Hy[i - 1][j + 1][k]) * m_Huy_xMin_Wg1 + 
     					     ( Hy[i][j][k]     + Hy[i][j + 1][k])     * m_Huy_xMin_Wg2 ;
					hz_tmp = ( Hz[i - 1][j][k] + Hz[i - 1][j][k + 1]) * m_Huy_xMin_Wg1 + 
    					     ( Hz[i][j][k]     + Hz[i][j][k + 1])     * m_Huy_xMin_Wg2 ;


					m_Huy_S1[freqIndex][j1][k1].J1 += exp_h * hz_tmp ;	// Jy
					m_Huy_S1[freqIndex][j1][k1].J2 -= exp_h * hy_tmp ;	// Jz	
					m_Huy_S1[freqIndex][j1][k1].M1 -= exp_e * ez_tmp ;	// My
					m_Huy_S1[freqIndex][j1][k1].M2 += exp_e * ey_tmp ;	// Mz
					
				}	
			}	
		}	

		// xmax
		if ( m_huy_FaceFlag[XMAX] )
		{
			i = m_huygensBox.endX ;

			for ( j = m_huygensBox.startY ; j < m_huygensBox.endY ; j ++ )
			{
				for ( k = m_huygensBox.startZ ; k < m_huygensBox.endZ ; k ++ )
				{
					j1 = j - m_huygensBox.startY ;
					k1 = k - m_huygensBox.startZ ;

					//move the factor 0.5f from here to subroutine Convert_HuyCurr to avoid multiplying at each time step
					ey_tmp = 0.5f * (Ey[i][j][k] + Ey[i][j][k + 1]) ;	
					ez_tmp = 0.5f * (Ez[i][j][k] + Ez[i][j + 1][k]) ;

					hy_tmp = (Hy[i - 1][j][k] + Hy[i - 1][j + 1][k]) * m_Huy_xMax_Wg1 + 
     					     (Hy[i][j][k]     + Hy[i][j + 1][k])     * m_Huy_xMax_Wg2 ;
					hz_tmp = (Hz[i - 1][j][k] + Hz[i - 1][j][k + 1]) * m_Huy_xMax_Wg1 + 
    					     (Hz[i][j][k]     + Hz[i][j][k + 1])     * m_Huy_xMax_Wg2 ;

					m_Huy_S2[freqIndex][j1][k1].J1 -= exp_h * hz_tmp ;	// Jy
					m_Huy_S2[freqIndex][j1][k1].J2 += exp_h * hy_tmp ;	// Jz	
					m_Huy_S2[freqIndex][j1][k1].M1 += exp_e * ez_tmp ;	// My
					m_Huy_S2[freqIndex][j1][k1].M2 -= exp_e * ey_tmp ;	// Mz	
					
				}	
			}	
		}	

	// ymin
		if ( m_huy_FaceFlag[YMIN] )
		{
			j = m_huygensBox.startY ;

			for ( k = m_huygensBox.startZ ; k < m_huygensBox.endZ ; k ++ )
			{
				for ( i = m_huygensBox.startX ; i < m_huygensBox.endX ; i ++ )
				{
					i1 = i - m_huygensBox.startX ;
					k1 = k - m_huygensBox.startZ ;

					//move the factor 0.5f from here to subroutine Convert_HuyCurr to avoid multiplying at each time step
					ez_tmp = 0.5f * (Ez[i][j][k] + Ez[i + 1][j][k]) ;	
					ex_tmp = 0.5f * (Ex[i][j][k] + Ex[i][j][k + 1]) ;

					hz_tmp = (Hz[i][j - 1][k] + Hz[i][j - 1][k + 1]) * m_Huy_yMin_Wg1 + 
     					     (Hz[i][j][k]     + Hz[i][j][k + 1])     * m_Huy_yMin_Wg2 ;
					hx_tmp = (Hx[i][j - 1][k] + Hx[i + 1][j - 1][k]) * m_Huy_yMin_Wg1 + 
    					     (Hx[i][j][k]     + Hx[i + 1][j][k])     * m_Huy_yMin_Wg2 ;

					m_Huy_S3[freqIndex][k1][i1].J1 += exp_h * hx_tmp ;	// Jz
					m_Huy_S3[freqIndex][k1][i1].J2 -= exp_h * hz_tmp ;	// Jx	
					m_Huy_S3[freqIndex][k1][i1].M1 -= exp_e * ex_tmp ;	// Mz
					m_Huy_S3[freqIndex][k1][i1].M2 += exp_e * ez_tmp ;	// Mx

			
				}	
			}
		}	

		// ymax
		if ( m_huy_FaceFlag[YMAX] )
		{
			j = m_huygensBox.endY ;

			for ( k = m_huygensBox.startZ ; k < m_huygensBox.endZ ; k ++ )
			{
				for ( i = m_huygensBox.startX ; i < m_huygensBox.endX ; i ++ )
				{
					i1 = i - m_huygensBox.startX ;
					k1 = k - m_huygensBox.startZ ;

					//move the factor 0.5f from here to subroutine Convert_HuyCurr to avoid multiplying at each time step
					ez_tmp = 0.5f * (Ez[i][j][k] + Ez[i + 1][j][k]) ;	
					ex_tmp = 0.5f * (Ex[i][j][k] + Ex[i][j][k + 1]) ;

					hz_tmp = (Hz[i][j - 1][k] + Hz[i][j - 1][k + 1]) * m_Huy_yMax_Wg1 + 
     					     (Hz[i][j][k]     + Hz[i][j][k + 1])     * m_Huy_yMax_Wg2 ;
					hx_tmp = (Hx[i][j - 1][k] + Hx[i + 1][j - 1][k]) * m_Huy_yMax_Wg1 + 
    					     (Hx[i][j][k]     + Hx[i + 1][j][k])     * m_Huy_yMax_Wg2 ;

					m_Huy_S4[freqIndex][k1][i1].J1 -= exp_h * hx_tmp ;	// Jz
					m_Huy_S4[freqIndex][k1][i1].J2 += exp_h * hz_tmp ;	// Jx	
					m_Huy_S4[freqIndex][k1][i1].M1 += exp_e * ex_tmp ;	// Mz
					m_Huy_S4[freqIndex][k1][i1].M2 -= exp_e * ez_tmp ;	// Mx
					
				}	
			}	
		}	

		// zmin
		if ( m_huy_FaceFlag[ZMIN] )
		{
			k = m_huygensBox.startZ ;

			for ( i = m_huygensBox.startX ; i < m_huygensBox.endX ; i ++ )
			{
				for ( j = m_huygensBox.startY ; j < m_huygensBox.endY ; j ++ )
				{
					i1 = i - m_huygensBox.startX ;
					j1 = j - m_huygensBox.startY ;

					//move the factor 0.5f from here to subroutine Convert_HuyCurr to avoid multiplying at each time step
					ex_tmp = 0.5f * (Ex[i][j][k] + Ex[i][j + 1][k]) ;	
					ey_tmp = 0.5f * (Ey[i][j][k] + Ey[i + 1][j][k]) ;

					hx_tmp = (Hx[i][j][k - 1] + Hx[i + 1][j][k - 1]) * m_Huy_zMin_Wg1 + 
     					     (Hx[i][j][k]     + Hx[i + 1][j][k])     * m_Huy_zMin_Wg2 ;
					hy_tmp = (Hy[i][j][k - 1] + Hy[i][j + 1][k - 1]) * m_Huy_zMin_Wg1 + 
    					     (Hy[i][j][k]     + Hy[i][j + 1][k])     * m_Huy_zMin_Wg2 ;

					m_Huy_S5[freqIndex][i1][j1].J1 += exp_h * hy_tmp ;	// Jx
					m_Huy_S5[freqIndex][i1][j1].J2 -= exp_h * hx_tmp ;	// Jy	
					m_Huy_S5[freqIndex][i1][j1].M1 -= exp_e * ey_tmp ;	// Mx
					m_Huy_S5[freqIndex][i1][j1].M2 += exp_e * ex_tmp ;	// My

				}	
			}	
		}	


		// zmax
		if ( m_huy_FaceFlag[ZMAX] )
		{
			k = m_huygensBox.endZ ;

			for ( i = m_huygensBox.startX ; i < m_huygensBox.endX ; i ++ )
			{
				for ( j = m_huygensBox.startY ; j < m_huygensBox.endY ; j ++ )
				{
					i1 = i - m_huygensBox.startX ;
					j1 = j - m_huygensBox.startY ;

					//move the factor 0.5f from here to subroutine Convert_HuyCurr to avoid multiplying at each time step
					ex_tmp = 0.5f * (Ex[i][j][k] + Ex[i][j+1][k]) ;	
					ey_tmp = 0.5f * (Ey[i][j][k] + Ey[i+1][j][k]) ;

					hx_tmp = (Hx[i][j][k - 1] + Hx[i + 1][j][k - 1]) * m_Huy_zMax_Wg1 + 
     					     (Hx[i][j][k]     + Hx[i + 1][j][k])     * m_Huy_zMax_Wg2 ;
					hy_tmp = (Hy[i][j][k - 1] + Hy[i][j + 1][k - 1]) * m_Huy_zMax_Wg1 + 
    					     (Hy[i][j][k]     + Hy[i][j + 1][k])     * m_Huy_zMax_Wg2 ;

					m_Huy_S6[freqIndex][i1][j1].J1 -= exp_h * hy_tmp ;	// Jx
					m_Huy_S6[freqIndex][i1][j1].J2 += exp_h * hx_tmp ;	// Jy	
					m_Huy_S6[freqIndex][i1][j1].M1 += exp_e * ey_tmp ;	// Mx
					m_Huy_S6[freqIndex][i1][j1].M2 -= exp_e * ex_tmp ;	// My
					
				}	
			}	
		}

	} 

}



void CFarField::farField_Near2Far( XML_Writer &outFile )
{
	int i_theta, i_phi, freqIndex ;		// loop index
	
	float phi_end ;
	float theta_end ;

	float theta, phi ;

	float EthReal, EthImage, EphReal, EphImage ;
	float EthReal_sum, EthImage_sum, EphReal_sum, EphImage_sum ;

	float EthAbs, EthAngle, EphAbs, EphAngle , E_Total_Abs, E_Total_Phase;

	float Eth_directivity, Ephi_directivity, Etotal_directivity ;
	float Eth_gain , Ephi_gain , Etotal_gain ;
	float Eth_directivity_dB, Ephi_directivity_dB, Etotal_directivity_dB ;
	float Eth_gain_dB , Ephi_gain_dB , Etotal_gain_dB ;

	int i ;

	float power_incident ;
	//---------------------------------------------------------------
	if( id == 0 )
	{
		std::cout << std::endl << std::endl << "Start Far Field Calculation.." << std::endl ;
		std::flush( std::cout ) ;
	}

	// 
	convert_HuyCurrent();

	//---------------------------------------------------------------------------

	if( id == 0 )
	{
		std::string str ;
		char c_string[64] = "";

		switch( m_freqUnit )
		{
			case Hz :
				str = "Hz" ;
				break ;
			case KHz :
				str = "KHz" ;
				break ;
			case MHz :
				str = "MHz" ;
				break ;
			case GHz :
				str = "GHz" ;
				break ;
			case THz :
				str = "THz" ;
				break ;
			case PHz :
				str = "PHz" ;
				break ;
		}

		outFile.AddAtributes( "frequency_Unit", str ) ;

		sprintf_s( c_string, "%d", m_freqNum ) ;
		str = c_string ;
		outFile.AddAtributes( "frequency_Number", str ) ;
		outFile.Createtag( "FarFieldOutput" ) ;

		for ( freqIndex = 0; freqIndex < m_freqNum ; freqIndex++ )
		{
			sprintf_s( c_string, "%e", m_freqList[freqIndex] ) ;
			str = c_string ;
			outFile.AddAtributes( "frequency", str ) ;
			outFile.Createtag( "FarField" ) ;

			Eth_gain    = 0 ;
			Ephi_gain   = 0 ;
			Etotal_gain = 0 ;

			Eth_directivity = 0 ;
			Ephi_directivity = 0 ;
			Etotal_directivity = 0 ;

			Eth_gain_dB    = 0 ;
			Ephi_gain_dB   = 0 ;
			Etotal_gain_dB = 0 ;

			Eth_directivity_dB = 0 ;
			Ephi_directivity_dB = 0 ;
			Etotal_directivity_dB = 0 ;

			power_incident = 0 ;
			for( i = 0 ; i < m_sourceNum ; i++ )
			{
				power_incident += 0.5f * ( m_sourceReal[freqIndex][i] * m_powerReal[freqIndex][i] 
					+ m_powerImag[freqIndex][i] * m_sourceImag[freqIndex][i] );
			}

			power_incident = fabs( power_incident ) ;

			if ( m_phiEnabled ) 
			{
				outFile.Createtag( "PhiCut" ) ;

				theta_end = m_thetaEnd + 0.1f * m_thetaStep ;

				//far field at phi cuts
				for ( i_phi = 0; i_phi < m_phiNum ; i_phi ++ )
				{	
					sprintf_s( c_string, "%e", m_phiList[i_phi] ) ;
					str = c_string ;
					outFile.AddAtributes( "phi", str ) ;
					outFile.Createtag( "PhiCutFarField" ) ;

					theta = m_thetaStart;
					do
					{
						EthReal = 0 ; EthImage = 0 ;
						EphReal = 0 ; EphImage = 0 ;

						MPI_Reduce( &EthReal, &EthReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EthImage, &EthImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphReal, &EphReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphImage, &EphImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;


						EthAbs   = sqrt(  EthReal_sum  *  EthReal_sum + EthImage_sum * EthImage_sum);
						EthAngle = atan2(EthImage_sum,EthReal_sum) * Factor_Rad_Deg ;

						EphAbs   = sqrt( EphReal_sum * EphReal_sum + EphImage_sum * EphImage_sum);
						EphAngle = atan2(EphImage_sum,EphReal_sum) * Factor_Rad_Deg ;

						E_Total_Abs = sqrt( EphAbs * EphAbs + EthAbs * EthAbs ) ;
						E_Total_Phase = atan2( sqrt( EthImage_sum * EthImage_sum + EphImage_sum * EphImage_sum ), 
												sqrt( EthReal_sum * EthReal_sum + EphReal_sum * EphReal_sum ) ) * Factor_Rad_Deg ;


						Eth_directivity    = 0.0f ;
						Ephi_directivity   = 0.0f ;
						Etotal_directivity = 0.0f ;
					
						Eth_directivity_dB    = 0.0f ;
						Ephi_directivity_dB   = 0.0f ;
						Etotal_directivity_dB = 0.0f ;
						
						// directivity
						if( fabs( m_radiationPower[freqIndex] ) > 1.0e-60 )
						{
							Eth_directivity    = (0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs / m_radiationPower[freqIndex]);
							Ephi_directivity   = (0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / m_radiationPower[freqIndex]);
							Etotal_directivity = (0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / m_radiationPower[freqIndex]);
						
							Eth_directivity_dB    = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs / m_radiationPower[freqIndex]);
							Ephi_directivity_dB   = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / m_radiationPower[freqIndex]);
							Etotal_directivity_dB = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / m_radiationPower[freqIndex]);
						}


						Eth_gain    = 0.0f ;
						Ephi_gain   = 0.0f ;
						Etotal_gain = 0.0f ;
						
						Eth_gain_dB    = 0.0f ;
						Ephi_gain_dB   = 0.0f ;
						Etotal_gain_dB = 0.0f ;
						
						// gain
						if( fabs( m_radiationPower[freqIndex] ) > 1.0e-60 )
						{
							Eth_gain    = (0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs/ power_incident );
							Ephi_gain   = (0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / power_incident );
							Etotal_gain = (0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / power_incident );
							
							Eth_gain_dB    = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs/ power_incident );
							Ephi_gain_dB   = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / power_incident );
							Etotal_gain_dB = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / power_incident );
						}

						sprintf_s( c_string, "%e", theta ) ;
						str = c_string ;
						outFile.AddAtributes( "theta", str ) ;
						outFile.Createtag( "Value" ) ;
						{
							// E theta
							sprintf_s( c_string, "%e", Eth_gain_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "GainDB", str ) ;

							sprintf_s( c_string, "%e", Eth_gain ) ;
							str = c_string ;
							outFile.AddAtributes( "Gain", str ) ;

							sprintf_s( c_string, "%e", Eth_directivity_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "DirectivityDB", str ) ;

							sprintf_s( c_string, "%e", Eth_directivity ) ;
							str = c_string ;
							outFile.AddAtributes( "Directivity", str ) ;

							sprintf_s( c_string, "%e", EthAngle ) ;
							str = c_string ;
							outFile.AddAtributes( "Phase", str ) ;

							sprintf_s( c_string, "%e", EthAbs / power_incident ) ;
							str = c_string ;
							outFile.AddAtributes( "Amp", str ) ;
							
							outFile.Createtag( "Etheta" ) ;
							outFile.CloseSingletag();


							// E phi
							sprintf_s( c_string, "%e", Ephi_gain_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "GainDB", str ) ;

							sprintf_s( c_string, "%e", Ephi_gain ) ;
							str = c_string ;
							outFile.AddAtributes( "Gain", str ) ;

							sprintf_s( c_string, "%e", Ephi_directivity_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "DirectivityDB", str ) ;

							sprintf_s( c_string, "%e", Ephi_directivity ) ;
							str = c_string ;
							outFile.AddAtributes( "Directivity", str ) ;

							sprintf_s( c_string, "%e", EphAngle ) ;
							str = c_string ;
							outFile.AddAtributes( "Phase", str ) ;

							sprintf_s( c_string, "%e", EphAbs / power_incident ) ;
							str = c_string ;
							outFile.AddAtributes( "Amp", str ) ;
							
							outFile.Createtag( "Ephi" ) ;
							outFile.CloseSingletag();


							// E total
							sprintf_s( c_string, "%e", Etotal_gain_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "GainDB", str ) ;

							sprintf_s( c_string, "%e", Etotal_gain ) ;
							str = c_string ;
							outFile.AddAtributes( "Gain", str ) ;

							sprintf_s( c_string, "%e", Etotal_directivity_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "DirectivityDB", str ) ;

							sprintf_s( c_string, "%e", Etotal_directivity ) ;
							str = c_string ;
							outFile.AddAtributes( "Directivity", str ) ;

							sprintf_s( c_string, "%e", E_Total_Phase ) ;
							str = c_string ;
							outFile.AddAtributes( "Phase", str ) ;

							sprintf_s( c_string, "%e", E_Total_Abs / power_incident ) ;
							str = c_string ;
							outFile.AddAtributes( "Amp", str ) ;
							
							outFile.Createtag( "Etotal" ) ;
							outFile.CloseSingletag();
						}
						// close outFile.Createtag( "Value" ) ;
						outFile.CloseLasttag();

						theta += m_thetaStep ;
					}while( theta < theta_end ) ;
					
					// close outFile.Createtag( "PhiCutFarField" ) ;
					outFile.CloseLasttag();

				} // end of for ( i_phi = 0; i_phi < m_phiNum ; i_phi ++ )

				// close outFile.Createtag( "PhiCut" ) ;
				outFile.CloseLasttag();
			} // end of if ( m_phiEnabled ) 


			Eth_gain    = 0 ;
			Ephi_gain   = 0 ;
			Etotal_gain = 0 ;

			Eth_directivity = 0 ;
			Ephi_directivity = 0 ;
			Etotal_directivity = 0 ;

			Eth_gain_dB    = 0 ;
			Ephi_gain_dB   = 0 ;
			Etotal_gain_dB = 0 ;

			Eth_directivity_dB = 0 ;
			Ephi_directivity_dB = 0 ;
			Etotal_directivity_dB = 0 ;

			if( m_thetaEnabled ) 
			{

				outFile.Createtag( "ThetaCut" ) ;

				//far field at theta cuts
				phi_end = m_phiEnd + 0.1f * m_phiStep ;

				for ( i_theta = 0; i_theta < m_thetaNum ; i_theta ++ )
				{	
					sprintf_s( c_string, "%e", m_thetaList[i_theta] ) ;
					str = c_string ;
					outFile.AddAtributes( "theta", str ) ;
					outFile.Createtag( "ThetaCutFarField" ) ;

					phi = m_phiStart;
					do
					{
						EthReal = 0 ; EthImage = 0 ;
						EphReal = 0 ; EphImage = 0 ;

						MPI_Reduce( &EthReal, &EthReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EthImage, &EthImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphReal, &EphReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphImage, &EphImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;

						EthAbs   = sqrt( EthReal_sum * EthReal_sum + EthImage_sum * EthImage_sum);
						EthAngle = atan2(EthImage_sum,EthReal_sum) * Factor_Rad_Deg ;

						EphAbs   = sqrt( EphReal_sum * EphReal_sum + EphImage_sum * EphImage_sum);
						EphAngle = atan2(EphImage_sum,EphReal_sum) * Factor_Rad_Deg ;

						E_Total_Abs = sqrt( EphAbs * EphAbs + EthAbs * EthAbs ) ;
						E_Total_Phase = atan2( sqrt( EthImage_sum * EthImage_sum + EphImage_sum * EphImage_sum ), 
												sqrt( EthReal_sum * EthReal_sum + EphReal_sum * EphReal_sum ) ) * Factor_Rad_Deg ;
	
						Eth_directivity    = 0.0f ;
						Ephi_directivity   = 0.0f ;
						Etotal_directivity = 0.0f ;
					
						Eth_directivity_dB    = 0.0f ;
						Ephi_directivity_dB   = 0.0f ;
						Etotal_directivity_dB = 0.0f ;
						// directivity
						if( fabs( m_radiationPower[freqIndex] ) > 1.0e-60 )
						{
							Eth_directivity    = (0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs / m_radiationPower[freqIndex] );
							Ephi_directivity   = (0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / m_radiationPower[freqIndex] );
							Etotal_directivity = (0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / m_radiationPower[freqIndex] );
						
							Eth_directivity_dB    = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs / m_radiationPower[freqIndex] );
							Ephi_directivity_dB   = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / m_radiationPower[freqIndex] );
							Etotal_directivity_dB = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / m_radiationPower[freqIndex] );
						}

						Eth_gain    = 0.0f ;
						Ephi_gain   = 0.0f ;
						Etotal_gain = 0.0f ;
						
						Eth_gain_dB    = 0.0f ;
						Ephi_gain_dB   = 0.0f ;
						Etotal_gain_dB = 0.0f ;
						// gain
						if( fabs( power_incident ) > 1.0e-60 )
						{
							Eth_gain    = (0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs / power_incident );
							Ephi_gain   = (0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / power_incident );
							Etotal_gain = (0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / power_incident );
							
							Eth_gain_dB    = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EthAbs * EthAbs / power_incident );
							Ephi_gain_dB   = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * EphAbs * EphAbs / power_incident );
							Etotal_gain_dB = 10.0f * log10(0.5f * Eta0_Inv * 4.0f * PI * E_Total_Abs * E_Total_Abs / power_incident );
						}

						sprintf_s( c_string, "%e", phi ) ;
						str = c_string ;
						outFile.AddAtributes( "phi", str ) ;
						outFile.Createtag( "Value" ) ;
						{
							// E theta
							sprintf_s( c_string, "%e", Eth_gain_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "GainDB", str ) ;

							sprintf_s( c_string, "%e", Eth_gain ) ;
							str = c_string ;
							outFile.AddAtributes( "Gain", str ) ;

							sprintf_s( c_string, "%e", Eth_directivity_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "DirectivityDB", str ) ;

							sprintf_s( c_string, "%e", Eth_directivity ) ;
							str = c_string ;
							outFile.AddAtributes( "Directivity", str ) ;

							sprintf_s( c_string, "%e", EthAngle ) ;
							str = c_string ;
							outFile.AddAtributes( "Phase", str ) ;

							sprintf_s( c_string, "%e", EthAbs / power_incident ) ;
							str = c_string ;
							outFile.AddAtributes( "Amp", str ) ;
							
							outFile.Createtag( "Etheta" ) ;
							outFile.CloseSingletag();


							// E phi
							sprintf_s( c_string, "%e", Ephi_gain_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "GainDB", str ) ;

							sprintf_s( c_string, "%e", Ephi_gain ) ;
							str = c_string ;
							outFile.AddAtributes( "Gain", str ) ;

							sprintf_s( c_string, "%e", Ephi_directivity_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "DirectivityDB", str ) ;

							sprintf_s( c_string, "%e", Ephi_directivity ) ;
							str = c_string ;
							outFile.AddAtributes( "Directivity", str ) ;

							sprintf_s( c_string, "%e", EphAngle ) ;
							str = c_string ;
							outFile.AddAtributes( "Phase", str ) ;

							sprintf_s( c_string, "%e", EphAbs / power_incident ) ;
							str = c_string ;
							outFile.AddAtributes( "Amp", str ) ;
							
							outFile.Createtag( "Ephi" ) ;
							outFile.CloseSingletag();


							// E total
							sprintf_s( c_string, "%e", Etotal_gain_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "GainDB", str ) ;

							sprintf_s( c_string, "%e", Etotal_gain ) ;
							str = c_string ;
							outFile.AddAtributes( "Gain", str ) ;

							sprintf_s( c_string, "%e", Etotal_directivity_dB ) ;
							str = c_string ;
							outFile.AddAtributes( "DirectivityDB", str ) ;

							sprintf_s( c_string, "%e", Etotal_directivity ) ;
							str = c_string ;
							outFile.AddAtributes( "Directivity", str ) ;

							sprintf_s( c_string, "%e", E_Total_Phase ) ;
							str = c_string ;
							outFile.AddAtributes( "Phase", str ) ;

							sprintf_s( c_string, "%e", E_Total_Abs / power_incident ) ;
							str = c_string ;
							outFile.AddAtributes( "Amp", str ) ;
							
							outFile.Createtag( "Etotal" ) ;
							outFile.CloseSingletag();
						}
						// close outFile.Createtag( "Value" ) ;
						outFile.CloseLasttag();
					
						phi += m_phiStep ;

					}while( phi <= phi_end ) ;

					// close outFile.Createtag( "ThetaCutFarField" ) ;
					outFile.CloseLasttag();
				}
				// close outFile.Createtag( "ThetaCut" ) ;
				outFile.CloseLasttag();
			}

			std::cout << "Frequency =  " << m_freqList[freqIndex] / 1.0e9 << " GHz" << std::endl ;
			std::flush( std::cout ) ;
			
			// close outFile.Createtag( "FarField" ) ;
			outFile.CloseLasttag();

		}	// end of for ( freqIndex = 0; freqIndex < m_freqNum ; freqIndex++ )


		// close outFile.Createtag( "FarFieldOutput" ) ;
		outFile.CloseLasttag();
	}
	else
	{
	
		for ( freqIndex = 0; freqIndex < m_freqNum ; freqIndex++ )
		{

			if ( m_phiEnabled ) 
			{
				theta_end = m_thetaEnd + 0.1f * m_thetaStep ;

				//far field at phi cuts
				for ( i_phi = 0; i_phi < m_phiNum ; i_phi ++ )
				{	
					theta = m_thetaStart;
					do
					{
						farField_Culculation( theta * Factor_Deg_Rad, m_phiList[i_phi] * Factor_Deg_Rad, freqIndex );


						EthReal = m_Far_Etheta.real() ; EthImage = m_Far_Etheta.imag() ;
						EphReal = m_Far_Ephi.real() ;   EphImage = m_Far_Ephi.imag() ;

						MPI_Reduce( &EthReal, &EthReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EthImage, &EthImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphReal, &EphReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphImage, &EphImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;

						theta += m_thetaStep ;

					}while( theta < theta_end ) ;
				}
			}

			if( m_thetaEnabled ) 
			{

				//far field at theta cuts
				phi_end = m_phiEnd + 0.1f * m_phiStep ;

				for ( i_theta = 0; i_theta < m_thetaNum ; i_theta ++ )
				{	
					phi = m_phiStart;
					do
					{
					
						farField_Culculation( m_thetaList[i_theta] * Factor_Deg_Rad, phi * Factor_Deg_Rad, freqIndex );

						EthReal = m_Far_Etheta.real() ; EthImage = m_Far_Etheta.imag() ;
						EphReal = m_Far_Ephi.real() ; EphImage = m_Far_Ephi.imag() ;

						MPI_Reduce( &EthReal, &EthReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EthImage, &EthImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphReal, &EphReal_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
						MPI_Reduce( &EphImage, &EphImage_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;

						phi += m_phiStep ;

					}while( phi <= phi_end ) ;
				}
			}
		}
	}
}



void CFarField::convert_HuyCurrent(void)
{
	// Convert the huygen's currents into the form used in
	// far-field calculation. Do spatial sampling if necessary.

	int freqIndex, i, j, k ;	// loop index
	float num_zero, area_tmp,negative_sign;
	float tmp_power = 0 ;
	std::complex<float> J1, J2, M1, M2, comp_tmp;

	num_zero = 0;
	negative_sign = -1 ;


	if( id == 0 )
	{
		float m_radiationPower_tmp = 0  ;

		for ( freqIndex = 0; freqIndex < m_freqNum ; freqIndex++ )
		{	
			m_radiationPower_tmp = 0  ;
			MPI_Reduce( &m_radiationPower_tmp, &m_radiationPower[freqIndex], 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;
			m_radiationPower[freqIndex] = fabs( m_radiationPower[freqIndex] ) ;
		}
	}
	else
	{

		// Multiply the Huygen's Currents by k0/(4*pi) and its area
		// For J currents, an additional factor of eta0 is multiplied.
		for ( freqIndex = 0; freqIndex < m_freqNum ; freqIndex++ )
		{	

			if ( m_huy_FaceFlag[XMIN] == true )
			{
				for ( j = 0; j < m_nHuy_Curr[1] ; j++ )
				{
					for ( k = 0; k < m_nHuy_Curr[2] ; k++ ) 
					{
						area_tmp = m_Dy_Grid[j+m_huygensBox.startY] * m_Dz_Grid[k+m_huygensBox.startZ] ;


						comp_tmp = ( - ( m_Huy_S1[freqIndex][j][k].M2 * std::conj( m_Huy_S1[freqIndex][j][k].J1) ) + 
							( m_Huy_S1[freqIndex][j][k].M1 * std::conj( m_Huy_S1[freqIndex][j][k].J2 ) ) ) * area_tmp * 0.5f ;
						m_radiationPower[freqIndex] += comp_tmp.real() ;  
						
						//Huy_S is electric and magnetic cutterents before it multiplys area and Eta0.
						m_Huy_S1[freqIndex][j][k].J1 *= area_tmp * Eta0 ; 
						m_Huy_S1[freqIndex][j][k].J2 *= area_tmp * Eta0 ; 
						m_Huy_S1[freqIndex][j][k].M1 *= area_tmp ; 
						m_Huy_S1[freqIndex][j][k].M2 *= area_tmp ; 

					}	 
				}	
			}


			if ( m_huy_FaceFlag[XMAX] == true )
			{
				for ( j = 0; j < m_nHuy_Curr[1] ; j++ )
				{
					for ( k = 0; k < m_nHuy_Curr[2] ; k++ ) 
					{
						area_tmp = m_Dy_Grid[j+m_huygensBox.startY] * m_Dz_Grid[k+m_huygensBox.startZ] ;
					

						comp_tmp = ( m_Huy_S2[freqIndex][j][k].M2 * conj( m_Huy_S2[freqIndex][j][k].J1 ) 
								   - m_Huy_S2[freqIndex][j][k].M1 * conj( m_Huy_S2[freqIndex][j][k].J2 ) ) * area_tmp * 0.5f ;
						m_radiationPower[freqIndex] +=  comp_tmp.real() ;

						//Huy_S is electric and magnetic cutterents before it multiplys area and Eta0.
						m_Huy_S2[freqIndex][j][k].J1 *= area_tmp * Eta0 ; 
						m_Huy_S2[freqIndex][j][k].J2 *= area_tmp * Eta0 ; 
						m_Huy_S2[freqIndex][j][k].M1 *= area_tmp ; 
						m_Huy_S2[freqIndex][j][k].M2 *= area_tmp ; 

					} 
				}	
			}	

			if ( m_huy_FaceFlag[YMIN] == true )
			{
				for ( k = 0; k < m_nHuy_Curr[2] ; k++ )
				{
					for ( i = 0; i < m_nHuy_Curr[0] ; i++ ) 
					{
						area_tmp = m_Dx_Grid[i+m_huygensBox.startX] * m_Dz_Grid[k+m_huygensBox.startZ] ;

						comp_tmp = ( m_Huy_S3[freqIndex][k][i].M1 * conj( m_Huy_S3[freqIndex][k][i].J2 )  
								   - m_Huy_S3[freqIndex][k][i].M2 * conj( m_Huy_S3[freqIndex][k][i].J1 ) ) * area_tmp * 0.5f ;
						m_radiationPower[freqIndex] +=  comp_tmp.real()  ;

						//Huy_S is electric and magnetic cutterents before it multiplys area and Eta0.
						m_Huy_S3[freqIndex][k][i].J1 *= area_tmp * Eta0 ; 
						m_Huy_S3[freqIndex][k][i].J2 *= area_tmp * Eta0 ; 
						m_Huy_S3[freqIndex][k][i].M1 *= area_tmp ; 
						m_Huy_S3[freqIndex][k][i].M2 *= area_tmp ; 

					}	
				}	
			}	

			if ( m_huy_FaceFlag[YMAX] == true )
			{
				for ( k = 0; k < m_nHuy_Curr[2] ; k++ )
				{
					for ( i = 0; i < m_nHuy_Curr[0] ; i++ ) 
					{
						area_tmp = m_Dx_Grid[i+m_huygensBox.startX] * m_Dz_Grid[k+m_huygensBox.startZ] ;
					
						comp_tmp = ( - m_Huy_S4[freqIndex][k][i].M1 * conj( m_Huy_S4[freqIndex][k][i].J2 ) 
									 + m_Huy_S4[freqIndex][k][i].M2 * conj( m_Huy_S4[freqIndex][k][i].J1 ) ) * area_tmp * 0.5f ;
						m_radiationPower[freqIndex] += comp_tmp.real() ;

						//Huy_S is electric and magnetic cutterents before it multiplys area and Eta0.
						m_Huy_S4[freqIndex][k][i].J1 *= area_tmp * Eta0 ; 
						m_Huy_S4[freqIndex][k][i].J2 *= area_tmp * Eta0 ; 
						m_Huy_S4[freqIndex][k][i].M1 *= area_tmp ; 
						m_Huy_S4[freqIndex][k][i].M2 *= area_tmp ; 

					}	
				}	
			}	

			if ( m_huy_FaceFlag[ZMIN] == true )
			{
				for ( i = 0; i < m_nHuy_Curr[0] ; i++ ) 
				{
					for ( j = 0; j < m_nHuy_Curr[1] ; j++ )
					{
						area_tmp = m_Dx_Grid[i+m_huygensBox.startX] * m_Dy_Grid[j+m_huygensBox.startY] ;
					
						comp_tmp = ( - m_Huy_S5[freqIndex][i][j].M2 * conj( m_Huy_S5[freqIndex][i][j].J1 ) 
								   + m_Huy_S5[freqIndex][i][j].M1 *conj( m_Huy_S5[freqIndex][i][j].J2 )) * area_tmp * 0.5f ;
						m_radiationPower[freqIndex] += comp_tmp.real() ;

						//Huy_S is electric and magnetic cutterents before it multiplys area and Eta0.
						m_Huy_S5[freqIndex][i][j].J1 *= area_tmp * Eta0 ; 
						m_Huy_S5[freqIndex][i][j].J2 *= area_tmp * Eta0 ; 
						m_Huy_S5[freqIndex][i][j].M1 *= area_tmp ; 
						m_Huy_S5[freqIndex][i][j].M2 *= area_tmp ; 

					}	 
				}	
			}	

			if ( m_huy_FaceFlag[ZMAX] == true )
			{
				for ( i = 0; i < m_nHuy_Curr[0] ; i++ ) 
				{
					for ( j = 0; j < m_nHuy_Curr[1] ; j++ )
					{
						area_tmp = m_Dx_Grid[i+m_huygensBox.startX] * m_Dy_Grid[j+m_huygensBox.startY] ;

						comp_tmp = ( m_Huy_S6[freqIndex][i][j].M2 * conj( m_Huy_S6[freqIndex][i][j].J1 ) 
								   - m_Huy_S6[freqIndex][i][j].M1 * conj( m_Huy_S6[freqIndex][i][j].J2 ) ) * area_tmp * 0.5f ;
						m_radiationPower[freqIndex] += comp_tmp.real() ;

						//Huy_S is electric and magnetic cutterents before it multiplys area and Eta0.
						m_Huy_S6[freqIndex][i][j].J1 *= area_tmp * Eta0 ; 
						m_Huy_S6[freqIndex][i][j].J2 *= area_tmp * Eta0 ; 
						m_Huy_S6[freqIndex][i][j].M1 *= area_tmp ; 
						m_Huy_S6[freqIndex][i][j].M2 *= area_tmp ; 
					}	 
				}	
			}	


			//-----------------------------------------------------------------------------
			float m_radiationPower_sum = 0  ;

			MPI_Reduce( &m_radiationPower[freqIndex], &m_radiationPower_sum, 1 , MPI_FLOAT, MPI_SUM, 0 , MPI_COMM_WORLD ) ;

		}	
	}
}



void CFarField::farField_Culculation(float theta, float phi, int freqIndex )
{
	int i, j, k;

	float cosTheta, sinTheta, cosPhi, sinPhi, cTheta_cPhi, cTheta_sPhi;
	float cosTheta2, sinTheta2, cosPhi2, sinPhi2, cTheta_cPhi2, cTheta_sPhi2; 
	float k0, kx, ky, kz, kr1, kr2, kr3;
	float num_zero ;

	std::complex<float> cexp1p, cexp1m, cexp2p, cexp2m, cexp3p, cexp3m, Jx, Jy, Jz, Mx, My, Mz,tmp_factor_Eth,tmp_factor_Eph;


	num_zero = 0.0;

	m_Far_Etheta = num_zero ;
	m_Far_Ephi = num_zero ; 

	k0 = 2 * PI * m_freqList[freqIndex] * C_Speed_Inv ;
	tmp_factor_Eth =  - std::complex<float>(0.0, 1.0) * 0.5f * C_Speed_Inv * m_freqList[freqIndex] ;     //-j*ko/4pi=-j2pif/4cpic=-jf/2c
	tmp_factor_Eph =    std::complex<float>(0.0, 1.0) * 0.5f * C_Speed_Inv * m_freqList[freqIndex] ;     //j*ko/4pi=2pif/4cpic=f/2c

	cosTheta = cos(theta) ;
	sinTheta = sin(theta) ;
	cosPhi   = cos(phi) ;
	sinPhi   = sin(phi) ;

	cTheta_cPhi = cosTheta * cosPhi ; 
	cTheta_sPhi = cosTheta * sinPhi ;

	kx = k0 * sinTheta * cosPhi ;
	ky = k0 * sinTheta * sinPhi ;
	kz = k0 * cosTheta ;

	if ( m_use_MJ == 0 )	// Both J and M currents to calculate the far field pattern
	{
		cosTheta2 = cosTheta ;
		sinTheta2 = sinTheta ;
		cosPhi2 = cosPhi ;
		sinPhi2 = sinPhi ;
		cTheta_cPhi2 = cTheta_cPhi ; 
		cTheta_sPhi2 = cTheta_sPhi ;
	}
	else			// Use M currents only to calcualte the far field pattern
	{
		cosTheta = cosTheta * 2 ;	// The factor of 2 comes from using M currents only
		sinTheta = sinTheta * 2 ;
		cosPhi   = cosPhi * 2 ;
		sinPhi   = sinPhi * 2 ;
		cTheta_cPhi = cTheta_cPhi * 2 ; 
		cTheta_sPhi = cTheta_sPhi * 2 ;
	}

	// xmin
	if ( m_huy_FaceFlag[XMIN] == true )
	{
		//Contribution from real source
		kr1    = kx * ( m_X_Grid_Global[m_huygensBox_Global.startX] - m_huygensBox_Ref.x );
		cexp1p = std::complex<float>( cos(kr1), sin(kr1) );
		cexp1m = cexp1p ;


		for ( j = 0; j < m_nHuy_Curr[1] ; j ++ )
		{
			//Contribution from real source
			kr2 = ky * ( m_y_Center[j] - m_huygensBox_Ref.y );
			cexp2p = std::complex<float>( cos(kr2), sin(kr2) );
			cexp2m = cexp2p ;


			for ( k = 0; k < m_nHuy_Curr[2] ; k ++ ) 
			{
				//Contribution from real source
				kr3    = kz * ( m_z_Center[k] - m_huygensBox_Ref.z );
				cexp3p = std::complex<float>( cos( kr3 ), sin( kr3 ) );
				cexp3m = cexp3p ;


				//Ey = m_Huy_S1[freqIndex][j1][k1].M2;
				//Hz = m_Huy_S1[freqIndex][j1][k1].J1;
				//Ez = - m_Huy_S1[freqIndex][j1][k1].M1;
				//Hy = - m_Huy_S1[freqIndex][j1][k1].J2;
				Jy = m_Huy_S1[freqIndex][j][k].J1 * cexp1m * cexp2p * cexp3m ;
				Jz = m_Huy_S1[freqIndex][j][k].J2 * cexp1m * cexp2m * cexp3p ;

				My = m_Huy_S1[freqIndex][j][k].M1 * cexp1p * cexp2m * cexp3p ;
				Mz = m_Huy_S1[freqIndex][j][k].M2 * cexp1p * cexp2p * cexp3m ;

				if ( m_use_MJ == 0 ) 
				{
					m_Far_Etheta += ( My  * cosPhi2      + Jy * cTheta_sPhi2 - Jz * sinTheta2 )  ;
					m_Far_Ephi   += ( My  * cTheta_sPhi2 - Mz * sinTheta2    - Jy * cosPhi2   ) ;
				} 
				else 
				{	 
					m_Far_Etheta +=   My  * cosPhi ;
					m_Far_Ephi   += ( My  * cTheta_sPhi - Mz * sinTheta );
				}
			}	
		}	
	}	
						
	// xmax
	if ( m_huy_FaceFlag[XMAX] == true )
	{
		//Contribution from real source
		kr1    = kx * ( m_X_Grid_Global[m_huygensBox_Global.endX] - m_huygensBox_Ref.x );
		cexp1p = std::complex<float>( cos(kr1), sin(kr1) );
		cexp1m = cexp1p ;


		for ( j = 0; j < m_nHuy_Curr[1] ; j ++ )
		{
			//Contribution from real source
			kr2    = ky * ( m_y_Center[j] - m_huygensBox_Ref.y );
			cexp2p = std::complex<float>( cos(kr2), sin(kr2) );
			cexp2m = cexp2p ;


			for ( k = 0; k < m_nHuy_Curr[2] ; k ++ ) 
			{
				//Contribution from real source
				kr3    = kz * ( m_z_Center[k] - m_huygensBox_Ref.z );
				cexp3p = std::complex<float>( cos( kr3 ), sin( kr3 ) );
				cexp3m = cexp3p ;


				//Ey = - m_Huy_S2[freqIndex][j1][k1].M2;
				//Hz = - m_Huy_S2[freqIndex][j1][k1].J1;
				//Ez =   m_Huy_S2[freqIndex][j1][k1].M1;
				//Hy =   m_Huy_S2[freqIndex][j1][k1].J2;
				Jy = m_Huy_S2[freqIndex][j][k].J1 *  cexp1m * cexp2p * cexp3m ;
				Jz = m_Huy_S2[freqIndex][j][k].J2 *  cexp1m * cexp2m * cexp3p ;

				My = m_Huy_S2[freqIndex][j][k].M1 *  cexp1p * cexp2m * cexp3p ;
				Mz = m_Huy_S2[freqIndex][j][k].M2 *  cexp1p * cexp2p * cexp3m ;


				if ( m_use_MJ == 0 ) 
				{
					m_Far_Etheta += ( My  * cosPhi2      + Jy * cTheta_sPhi2 - Jz * sinTheta2 )  ;
					m_Far_Ephi   += ( My  * cTheta_sPhi2 - Mz * sinTheta2    - Jy * cosPhi2   )  ;
				} 
				else 
				{	 
					m_Far_Etheta +=   My  * cosPhi ;
					m_Far_Ephi   += ( My  * cTheta_sPhi - Mz * sinTheta );
				}
			}	 
		}	
	}	
						
	// ymin
	if ( m_huy_FaceFlag[YMIN] == true )
	{
		//Contribution from real source
		kr2    = ky * ( m_Y_Grid_Global[m_huygensBox_Global.startY] - m_huygensBox_Ref.y );
		cexp2p = std::complex<float>( cos(kr2), sin(kr2) );
		cexp2m = cexp2p ;


		for ( k = 0; k < m_nHuy_Curr[2] ; k ++ )
		{
			//Contribution from real source
			kr3    = kz * ( m_z_Center[k] - m_huygensBox_Ref.z );
			cexp3p = std::complex<float>( cos( kr3 ), sin( kr3 ) );
			cexp3m = cexp3p ;


			for ( i = 0; i < m_nHuy_Curr[0] ; i ++ ) 
			{
				//Contribution from real source
				kr1    = kx * ( m_x_Center[i] - m_huygensBox_Ref.x );
				cexp1p = std::complex<float>( cos( kr1 ), sin( kr1 ) );
				cexp1m = cexp1p ;


				//Ex = - m_Huy_S3[freqIndex][k1][i1].M1;
				//Hz = - m_Huy_S3[freqIndex][k1][i1].J2;
				//Ez = m_Huy_S3[freqIndex][k1][i1].M2;
				//Hx = m_Huy_S3[freqIndex][k1][i1].J1;
				Jz = m_Huy_S3[freqIndex][k][i].J1 *  cexp1m * cexp2m * cexp3p ;
				Jx = m_Huy_S3[freqIndex][k][i].J2 *  cexp1p * cexp2m * cexp3m ;

				Mz = m_Huy_S3[freqIndex][k][i].M1 *  cexp1p * cexp2p * cexp3m ;
				Mx = m_Huy_S3[freqIndex][k][i].M2 *  cexp1m * cexp2p * cexp3p ;

				if ( m_use_MJ == 0 ) 
				{
					m_Far_Etheta += ( Jx * cTheta_cPhi2 - Jz * sinTheta2 - Mx * sinPhi2 );
					m_Far_Ephi   += ( Mx * cTheta_cPhi2 - Mz * sinTheta2 + Jx * sinPhi2 );
				} 
				else 
				{	 
					m_Far_Etheta += - Mx * sinPhi;
					m_Far_Ephi   += ( Mx * cTheta_cPhi - Mz * sinTheta );
				}
			}	 
		}	
	}	

	// ymax
	if ( m_huy_FaceFlag[YMAX] == true )
	{
		//Contribution from real source
		kr2    = ky * ( m_Y_Grid_Global[m_huygensBox_Global.endY] - m_huygensBox_Ref.y );
		cexp2p = std::complex<float>( cos(kr2), sin(kr2) );
		cexp2m = cexp2p ;


		for ( k = 0; k < m_nHuy_Curr[2] ; k ++ )
		{
			//Contribution from real source
			kr3    = kz * ( m_z_Center[k] - m_huygensBox_Ref.z );
			cexp3p = std::complex<float>( cos( kr3 ), sin( kr3 ) );
			cexp3m = cexp3p ;


			for ( i = 0; i < m_nHuy_Curr[0] ; i ++ ) 
			{
				//Contribution from real source
				kr1    = kx * ( m_x_Center[i] - m_huygensBox_Ref.x );
				cexp1p = std::complex<float>( cos( kr1 ), sin( kr1 ) );
				cexp1m = cexp1p ;


				//Ex =   m_Huy_S4[freqIndex][k1][i1].M1;
				//Hz =   m_Huy_S4[freqIndex][k1][i1].J2;
				//Ez = - m_Huy_S4[freqIndex][k1][i1].M2;
				//Hx = - m_Huy_S4[freqIndex][k1][i1].J1;
				Jz = m_Huy_S4[freqIndex][k][i].J1 *  cexp1m * cexp2m * cexp3p ;
				Jx = m_Huy_S4[freqIndex][k][i].J2 *  cexp1p * cexp2m * cexp3m ;

				Mz = m_Huy_S4[freqIndex][k][i].M1 *  cexp1p * cexp2p * cexp3m ;
				Mx = m_Huy_S4[freqIndex][k][i].M2 *  cexp1m * cexp2p * cexp3p ;

				if ( m_use_MJ == 0 ) 
				{
					m_Far_Etheta += ( Jx * cTheta_cPhi2 - Jz * sinTheta2 - Mx * sinPhi2 );
					m_Far_Ephi   += ( Mx * cTheta_cPhi2 - Mz * sinTheta2 + Jx * sinPhi2 );
				} 
				else 
				{	 
					m_Far_Etheta +=  - Mx * sinPhi;
					m_Far_Ephi   += (  Mx * cTheta_cPhi - Mz * sinTheta );
				}
			}	 
		}	
	}	

	// zmin
	if ( m_huy_FaceFlag[ZMIN] == true )
	{
		//Contribution from real source
		kr3    = kz * ( m_Z_Grid_Global[m_huygensBox_Global.startZ] - m_huygensBox_Ref.z );
		cexp3p = std::complex<float>( cos(kr3), sin(kr3) );
		cexp3m = cexp3p ;

		for ( i = 0; i < m_nHuy_Curr[0] ; i ++ )
		{
			//Contribution from real source
			kr1    = kx * ( m_x_Center[i] - m_huygensBox_Ref.x );
			cexp1p = std::complex<float>( cos( kr1 ), sin( kr1 ) );
			cexp1m = cexp1p ;


			for ( j = 0; j < m_nHuy_Curr[1] ; j ++ ) 
			{
				//Contribution from real source
				kr2    = ky * ( m_y_Center[j] - m_huygensBox_Ref.y );
				cexp2p = std::complex<float>( cos(kr2), sin(kr2) );
				cexp2m = cexp2p ;


				//Ex = m_Huy_S5[freqIndex][i1][j1].M2;
				//Hy = m_Huy_S5[freqIndex][i1][j1].J1;
				//Ey = - m_Huy_S5[freqIndex][i1][j1].M1;
				//Hx = - m_Huy_S5[freqIndex][i1][j1].J2;
				Jx = m_Huy_S5[freqIndex][i][j].J1 *  cexp1p * cexp2m * cexp3m ;
				Jy = m_Huy_S5[freqIndex][i][j].J2 *  cexp1m * cexp2p * cexp3m ;

				Mx = m_Huy_S5[freqIndex][i][j].M1 *  cexp1m * cexp2p * cexp3p ;
				My = m_Huy_S5[freqIndex][i][j].M2 *  cexp1p * cexp2m * cexp3p ;

				if ( m_use_MJ == 0 ) 
				{
					m_Far_Etheta += ( Jx * cTheta_cPhi2 + Jy * cTheta_sPhi2 - Mx * sinPhi2 + My * cosPhi2 ) ;
					m_Far_Ephi   += ( Mx * cTheta_cPhi2 + My * cTheta_sPhi2 + Jx * sinPhi2 - Jy * cosPhi2 ) ;
				} 
				else 
				{	 
					m_Far_Etheta += ( - Mx * sinPhi      + My * cosPhi      ) ;
					m_Far_Ephi   += (   Mx * cTheta_cPhi + My * cTheta_sPhi ) ;
				}
			}	
		}	
	}	

	// zmax
	if ( m_huy_FaceFlag[ZMAX] == true )
	{
		//Contribution from real source
		kr3    = kz * ( m_Z_Grid_Global[m_huygensBox_Global.endZ] - m_huygensBox_Ref.z );
		cexp3p = std::complex<float>( cos(kr3), sin(kr3) );
		cexp3m = cexp3p ;


		for ( i = 0; i < m_nHuy_Curr[0] ; i ++ )
		{
			//Contribution from real source
			kr1    = kx * ( m_x_Center[i] - m_huygensBox_Ref.x );
			cexp1p = std::complex<float>( cos( kr1 ), sin( kr1 ) );
			cexp1m = cexp1p ;


			for ( j = 0; j < m_nHuy_Curr[1] ; j ++ ) 
			{
				//Contribution from real source
				kr2    = ky * ( m_y_Center[j] - m_huygensBox_Ref.y );
				cexp2p = std::complex<float>( cos(kr2), sin(kr2) );
				cexp2m = cexp2p ;


				//Ex = - m_Huy_S6[freqIndex][i1][j1].M2;
				//Hy = - m_Huy_S6[freqIndex][i1][j1].J1;
				//Ey = m_Huy_S6[freqIndex][i1][j1].M1;
				//Hx = m_Huy_S6[freqIndex][i1][j1].J2;
				Jx = m_Huy_S6[freqIndex][i][j].J1 *  cexp1p * cexp2m * cexp3m ;
				Jy = m_Huy_S6[freqIndex][i][j].J2 *  cexp1m * cexp2p * cexp3m ;

				Mx = m_Huy_S6[freqIndex][i][j].M1 *  cexp1m * cexp2p * cexp3p ;
				My = m_Huy_S6[freqIndex][i][j].M2 *  cexp1p * cexp2m * cexp3p ;

				if ( m_use_MJ == 0 ) 
				{
					m_Far_Etheta += ( Jx * cTheta_cPhi2 + Jy * cTheta_sPhi2 - Mx * sinPhi2 + My * cosPhi2 ) ;
					m_Far_Ephi   += ( Mx * cTheta_cPhi2 + My * cTheta_sPhi2 + Jx * sinPhi2 - Jy * cosPhi2 ) ;
				} 
				else 
				{	 
					m_Far_Etheta += ( - Mx * sinPhi      + My * cosPhi      ) ;
					m_Far_Ephi   += (   Mx * cTheta_cPhi + My * cTheta_sPhi ) ;
				}
			}	
		}	
	}
	
	//multiply the factor of the far field (see Balanis, Advanced Enginerring EM, pp. 288-289.
	m_Far_Etheta *= tmp_factor_Eth ;
	m_Far_Ephi   *= tmp_factor_Eph ;
	
}

void CFarField::setCellNum(int nx, int ny, int nz)
{
	m_nx = nx ;
	m_ny = ny ;
	m_nz = nz ;

}

void CFarField::setN_Local2Global(int* Nx_Local2Global, int* Ny_Local2Global, int* Nz_Local2Global)
{
	m_Nx_Local2Global = Nx_Local2Global ;
	m_Ny_Local2Global = Ny_Local2Global ;
	m_Nz_Local2Global = Nz_Local2Global ;
}



void CFarField::setDGrid(float* Dx_Grid, float* Dy_Grid, float* Dz_Grid)
{
	m_Dx_Grid = Dx_Grid ;
	m_Dy_Grid = Dy_Grid ;
	m_Dz_Grid = Dz_Grid ;
}

void CFarField::setDGrid_Global(float* Dx_Grid_Global, float* Dy_Grid_Global, float* Dz_Grid_Global)
{
	m_Dx_Grid_Global = Dx_Grid_Global ;
	m_Dy_Grid_Global = Dy_Grid_Global ;
	m_Dz_Grid_Global = Dz_Grid_Global ;
}

void CFarField::setHalf_Global(float* X_Half_Global, float* Y_Half_Global, float* Z_Half_Global)
{
	m_X_Half_Global = X_Half_Global ;
	m_Y_Half_Global = Y_Half_Global ;
	m_Z_Half_Global = Z_Half_Global ;
}


void CFarField::setGrid_Global(float* X_Grid_Global, float* Y_Grid_Global, float* Z_Grid_Global)
{
	m_X_Grid_Global = X_Grid_Global ;
	m_Y_Grid_Global = Y_Grid_Global ;
	m_Z_Grid_Global = Z_Grid_Global ;
}



void CFarField::setBoundaryLayerNum( int boundaryLayerNum[6] )
{
	int i ;
	for( i = 0 ; i < 6 ; i++ )
		m_boundaryLayerNum[i] = boundaryLayerNum[i] ;
}

