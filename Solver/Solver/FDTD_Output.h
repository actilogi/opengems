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
#include "./xml/xmlwriter.h"

#include <fstream>
#include <string>

class CFDTD_Output
{
public:
	CFDTD_Output(void);
	virtual ~CFDTD_Output(void) = 0 ;
protected:

	int m_timeStep ;
	// next output element in the output link
	CFDTD_Output *next;

	float *m_Dx_Grid , *m_Dy_Grid , *m_Dz_Grid ;		// These variables can't be deleted in deconstructor, because memory will not be allocated
	float *m_Dx_Half , *m_Dy_Half , *m_Dz_Half ;		// These variables can't be deleted in deconstructor, because memory will not be allocated

	float *m_X_Grid , *m_Y_Grid , *m_Z_Grid ;		// These variables can't be deleted in deconstructor, because memory will not be allocated
	float *m_X_Half , *m_Y_Half , *m_Z_Half ;		// These variables can't be deleted in deconstructor, because memory will not be allocated

	float *m_Dx_Half_Inv , *m_Dy_Half_Inv , *m_Dz_Half_Inv ;		// These variables can't be deleted in deconstructor, because memory will not be allocated

	// domain index of all processors
	DomainIndex *m_localDomainIndex ;

	int m_boundaryLayerNum[6] ;

	// Boundary Condition location
	int m_Index_E_Boundary[6] ; 
	int m_Index_H_Boundary[6] ; 

	// the name of output object
	std::string m_objectName ;

public:
	// get the next element of current output element
	CFDTD_Output *getNext(void) const ;
	virtual int init(void) = 0 ;
	virtual void collectResult(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) ) = 0 ;
	virtual void output( XML_Writer &outFile ) = 0 ;

	virtual int readIn_Solver( )= 0 ;
	virtual int readIn(FILE *in) = 0;

	void setNext(CFDTD_Output* next);

	void setDGrid(float* Dx_Grid, float* Dy_Guid, float* Dz_Guid);

	void setDHalf(float* Dx_Half, float* Dy_Half, float* Dz_Half);

	void setGrid(float* X_Grid, float* Y_Grid, float* Z_Grid);
	void setHalf(float* X_Half, float* Y_Half, float* Z_Half);
	void setDHalf_Inv(float* Dx_Half_Inv, float* Dy_Half_Inv, float* Dz_Half_Inv);

	void setBoundaryLayerNum( int *boundaryLayerNum ) ;

	// copy boundary index information
	void setBoundaryIndex(int* index_E_Boundary, int* index_H_Boundary);

	void setDomainIndex( DomainIndex *localDomainIndex ) ;

	void setTimeStep( int timeStep ) ;
};
