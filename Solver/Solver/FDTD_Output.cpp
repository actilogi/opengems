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

#include "FDTD_Output.h"

#include <cstdlib>

CFDTD_Output::CFDTD_Output(void)
{
	m_objectName = "" ;

	next = NULL ;

	m_Dx_Grid = NULL ;
	m_Dy_Grid = NULL ;
	m_Dz_Grid = NULL ;

	m_Dx_Half = NULL ;
	m_Dy_Half = NULL ;
	m_Dz_Half = NULL ;

	int i ;
	for( i = 0 ; i < 6 ; i++ )
		m_boundaryLayerNum[i] = 0 ;
}

CFDTD_Output::~CFDTD_Output(void)
{
}

// // get the next element of current output element
CFDTD_Output * CFDTD_Output::getNext(void) const
{
	return next;
}

void CFDTD_Output::setTimeStep( int timeStep ) 
{
	m_timeStep = timeStep ;
}


void CFDTD_Output::setNext(CFDTD_Output* next)
{
	this->next = next ;
}

void CFDTD_Output::setBoundaryLayerNum( int *boundaryLayerNum ) 
{
	int i ;
	for( i = 0 ; i < 6 ; i++ )
		m_boundaryLayerNum[i] = boundaryLayerNum[i] ;
}


void CFDTD_Output::setDGrid(float* Dx_Grid, float* Dy_Guid, float* Dz_Guid)
{
	m_Dx_Grid = Dx_Grid ;
	m_Dy_Grid = Dy_Guid ;
	m_Dz_Grid = Dz_Guid ;
}

void CFDTD_Output::setDHalf(float* Dx_Half, float* Dy_Half, float* Dz_Half) 
{
	m_Dx_Half = Dx_Half ;
	m_Dy_Half = Dy_Half ;
	m_Dz_Half = Dz_Half ;
}


void CFDTD_Output::setGrid(float* X_Grid, float* Y_Grid, float* Z_Grid)
{
	m_X_Grid = X_Grid ;
	m_Y_Grid = Y_Grid ;
	m_Z_Grid = Z_Grid ;
}


void CFDTD_Output::setHalf(float* X_Half, float* Y_Half, float* Z_Half)
{
	m_X_Half = X_Half ;
	m_Y_Half = Y_Half ;
	m_Z_Half = Z_Half ;
}

void CFDTD_Output::setDHalf_Inv(float* Dx_Half_Inv, float* Dy_Half_Inv, float* Dz_Half_Inv)
{
	m_Dx_Half_Inv = Dx_Half_Inv ;
	m_Dy_Half_Inv = Dy_Half_Inv ;
	m_Dz_Half_Inv = Dz_Half_Inv ;
}


void CFDTD_Output::setBoundaryIndex(int* index_E_Boundary, int* index_H_Boundary)
{
	memcpy( m_Index_E_Boundary, index_E_Boundary, 6 * sizeof( int ) ) ;
	memcpy( m_Index_H_Boundary, index_H_Boundary, 6 * sizeof( int ) ) ;
}




void CFDTD_Output::setDomainIndex( DomainIndex *localDomainIndex ) 
{
	m_localDomainIndex = localDomainIndex ;
}
