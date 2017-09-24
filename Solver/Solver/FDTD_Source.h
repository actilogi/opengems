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

#include "Material.h"
#include "Global_variable.h"
#include "FarField.h"

#include <fstream>
#include <string>

class CFDTD_Source
{
public:
	CFDTD_Source(void);
	virtual ~CFDTD_Source(void) ;

	virtual int init(void)= 0 ;
protected:

	static int m_sourceNum ;
	// Boundary Condition location
	int m_Index_E_Boundary[6] ; 
	int m_Index_H_Boundary[6] ; 

	float *m_pulse ;		// this class will not allocate memory for this pointer
	int   m_pulseLen ;

	// next source element  in source link 
	CFDTD_Source *next;

	float m_Dt;

	CMaterial ****m_eMatType ;		// This variable can't be deleted in deconstructor, because memory will not be allocated

	// reference of Dx , Dy, Dz , these variabbles can't be deleted in deconstructor
	float *m_Dx_Grid_Global ;
	float *m_Dy_Grid_Global ;
	float *m_Dz_Grid_Global ;

	//float *m_Dx_Half_Global ;
	//float *m_Dy_Half_Global ;
	//float *m_Dz_Half_Global ;

	float *m_Dx_Grid , *m_Dy_Grid , *m_Dz_Grid ;		// These variables can't be deleted in deconstructor, because memory will not be allocated
	float *m_Dx_Half , *m_Dy_Half , *m_Dz_Half ;		// These variables can't be deleted in deconstructor, because memory will not be allocated

	// if the excitation is in current domain or not
	bool m_insideFlag ;

	int m_boundaryLayerNum[6] ;

	// domain index of all processors
	DomainIndex *m_localDomainIndex ;

	int m_nx_Global ;
	int m_ny_Global ;
	int m_nz_Global ;

	std::string m_objectName ;

public:

	// set pulse data
	void setPulseData( float *pulse, int pulseLen ) ;
	void setDt(float Dt);
	void setDGrid(float* Dx_Grid, float* Dy_Grid, float* Dz_Grid);
	void setDHalf(float* Dx_Half, float* Dy_Half, float* Dz_Half);
	
	void setDGrid_Global(float* Dx_Grid_Global, float* Dy_Grid_Global, float* Dz_Grid_Global);

	// set the e Material distribution array point to the pointer in FDTD class
	void setMaterial(CMaterial**** eMatType);

	// copy boundary index information
	void setBoundaryIndex(int* index_E_Boundary, int* index_H_Boundary);

	void setDomainIndex( DomainIndex *localDomainIndex ) ;

	void setBoundaryLayerNum( int *boundaryLayerNum ) ;

	// get next source element of current source
	CFDTD_Source *getNext(void) const;
	void setNext(CFDTD_Source* next);

	virtual void update_E_Field(int n, float*** (&Ex), float*** (&Ey), float*** (&Ez), int sourceIndex = 0 , CFarField *farField = NULL ) ;
	virtual void update_H_Field(int n, float*** (&Hx), float*** (&Hy), float*** (&Hz), int sourceIndex = 0 , CFarField *farField = NULL ) ;

	// read data
	virtual int readIn_Solver( )= 0 ;
	virtual int readIn(FILE *in)= 0 ;
	int getSourceNum( ) ;
};
