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

#include "FDTD_Source.h"

#include "GEMS_Memory.h"

#include <cstdlib>

int CFDTD_Source::m_sourceNum = 0 ;

CFDTD_Source::CFDTD_Source(void)
: next(NULL) 
{
	m_eMatType = NULL ;		// This variable can't be deleted in deconstructor

	m_Dx_Grid = m_Dy_Grid = m_Dz_Grid = NULL ;		// These variables can't be deleted in deconstructor
	m_Dx_Half = m_Dy_Half = m_Dz_Half = NULL ;		// These variables can't be deleted in deconstructor

	m_localDomainIndex = NULL ;

	m_pulse = NULL ;
	m_insideFlag = false ;

	m_Dx_Grid_Global = NULL ;
	m_Dy_Grid_Global = NULL ;
	m_Dz_Grid_Global = NULL ;

	int i ;
	for( i = 0 ; i < 6 ; i++ )
		m_boundaryLayerNum[i] = 0 ;
}

CFDTD_Source::~CFDTD_Source(void)
{
}

void CFDTD_Source::update_E_Field(int n, float*** (&Ex), float*** (&Ey), float*** (&Ez), int sourceIndex, CFarField *farField)
{
}


void CFDTD_Source::update_H_Field(int n, float*** (&Hx), float*** (&Hy), float*** (&Hz), int sourceIndex, CFarField *farField)
{
}


void CFDTD_Source::setBoundaryLayerNum( int *boundaryLayerNum ) 
{
	int i ;
	for( i = 0 ; i < 6 ; i++ )
		m_boundaryLayerNum[i] = boundaryLayerNum[i] ;
}


// get next source element of current source
CFDTD_Source * CFDTD_Source::getNext(void) const
{
	return next;
}



void CFDTD_Source::setNext(CFDTD_Source* next)
{
	this->next = next ;
}

void CFDTD_Source::setDomainIndex( DomainIndex *localDomainIndex ) 
{
	m_localDomainIndex = localDomainIndex ;
}

void CFDTD_Source::setDt(float Dt)
{
	m_Dt = Dt ;
}

void CFDTD_Source::setPulseData( float *pulse, int pulseLen )
{
	m_pulse = pulse ;
	m_pulseLen = pulseLen ;
}

void CFDTD_Source::setDGrid(float* Dx_Grid, float* Dy_Grid, float* Dz_Grid)
{
	m_Dx_Grid = Dx_Grid ;
	m_Dy_Grid = Dy_Grid ;
	m_Dz_Grid = Dz_Grid ;
}

void CFDTD_Source::setDHalf(float* Dx_Half, float* Dy_Half, float* Dz_Half) 
{
	m_Dx_Half = Dx_Half ;
	m_Dy_Half = Dy_Half ;
	m_Dz_Half = Dz_Half ;
}

void CFDTD_Source::setDGrid_Global(float* Dx_Grid_Global, float* Dy_Grid_Global, float* Dz_Grid_Global)
{
	m_Dx_Grid_Global = Dx_Grid_Global ;
	m_Dy_Grid_Global = Dy_Grid_Global ;
	m_Dz_Grid_Global = Dz_Grid_Global ;
}



void CFDTD_Source::setMaterial(CMaterial**** eMatType)
{
	m_eMatType = eMatType ;
}

void CFDTD_Source::setBoundaryIndex(int* index_E_Boundary, int* index_H_Boundary)
{
	memcpy( m_Index_E_Boundary, index_E_Boundary, 6 * sizeof( int ) ) ;
	memcpy( m_Index_H_Boundary, index_H_Boundary, 6 * sizeof( int ) ) ;
}



int CFDTD_Source::getSourceNum( ) 
{
	return m_sourceNum ;
}
