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

#include <string>
#include <cstdlib>


#include "ParallelExchange.h"
#include "Boundary.h"
#include "FDTD_Output.h"
#include "FDTD_Source.h"
#include "FarField.h"
#include "./xml/xmlwriter.h"


class CFDTD
{
public:
	CFDTD(void);
	~CFDTD(void);
private:

	int timeStep ;
	float Dt ;

	// Far Field
	CFarField *m_farField;

	// FDTD Boundary
	CBoundary m_boundary ;

	CParallelExchange m_parallelExchange ;

	// FDTD Source
	CFDTD_Source *m_source ;

	// FDTD Output
	CFDTD_Output *m_output ;
	XML_Writer m_outFile ;
	std::string m_outFileName ;

	float*** Ex;
	float*** Ey;
	float*** Ez;

	float*** Hx;
	float*** Hy;
	float*** Hz;

	// Material 
	CMaterial *eMaterials_Common ;
	CMaterial ****eMatType ;
	char ***eMatTypeFlag ;

	float *pulse ;
	int pulseLen ;

	//Coefficient Combination for computational domain
	float *pHi_Coeff_M_Lossless ;
	float *pHj_Coeff_M_Lossless ;
	float *pHk_Coeff_M_Lossless ;

	float *pEi_Coeff ;
	float *pEj_Coeff ;
	float *pEk_Coeff ;

	float *Dx_Grid_Global , *Dy_Grid_Global , *Dz_Grid_Global ;
	float *Dx_Half_Global , *Dy_Half_Global , *Dz_Half_Global ;
	float *X_Half_Global , *Y_Half_Global , *Z_Half_Global ;
	float *X_Grid_Global , *Y_Grid_Global , *Z_Grid_Global ;

	// These variables define cell location and cell size
	float *Dx_Grid , *Dy_Grid , *Dz_Grid ;
	float *Dx_Half , *Dy_Half , *Dz_Half ;

	float *Dx_Grid_Inv , *Dy_Grid_Inv , *Dz_Grid_Inv ;
	float *Dx_Half_Inv , *Dy_Half_Inv , *Dz_Half_Inv ;

	float *X_Grid , *Y_Grid , *Z_Grid ;
	float *X_Half , *Y_Half , *Z_Half ;

	// array store global index of local index
	int *Nx_Local2Global ;
	int *Ny_Local2Global ;
	int *Nz_Local2Global ;

	// cell number in each direction, these indice in the designer are number of grids).
	int m_nx ;
	int m_ny ;
	int m_nz ;

	// cell number in global domain
	int m_nx_Global ;
	int m_ny_Global ;
	int m_nz_Global ;

	// Boundary Condition location
	//for E field update
	//Index_H_Boundary[XMIN]: index of the first H field in the x-direction
	//Index_H_Boundary[XMAX]: index of the last H field  in the x-direction
	//Index_H_Boundary[YMIN]: index of the first H field  in the y-direction
	//Index_H_Boundary[YMAX]: index of the last H field  in the y-direction
	//Index_H_Boundary[ZMIN]: index of the first H field  in the z-direction
	//Index_H_Boundary[ZMAX]: index of the last H field  in the z-direction

	//for H field update
	//Index_E_Boundary[XMIN]: index of the first E field in the x-direction
	//Index_E_Boundary[XMAX]: index of the last E field  in the x-direction
	//Index_E_Boundary[YMIN]: index of the first E field  in the y-direction
	//Index_E_Boundary[YMAX]: index of the last E field  in the y-direction
	//Index_E_Boundary[ZMIN]: index of the first E field  in the z-direction
	//Index_E_Boundary[ZMAX]: index of the last E field  in the z-direction

	int Index_E_Boundary[6] ; 
	int Index_H_Boundary[6] ; 

	int m_boundaryLayerNum[6] ;
	int m_boundaryLayerNum_Global[6] ;

	// domain index of all processors
	DomainIndex *LocalDomainIndex ;

	Boundary_Condition boundary[6];
	Boundary_Condition boundary_Global[6];

	Point m_domainRef ;

	std::string m_fileName ;
	FILE *in ;
	std::string m_stamp ;

	float unit ;
public:
	void update_H_Field(int n);
	void update_E_Field(int n);
	int init(void);
	void update_H_Source(int n);
	void update_E_Source(int n);



	// This routine is used to convert local index to global index, and store them in an array
	// it should be called after initMemory( ) ;
	void index_Local2Global( ) ;

	int readIn( ) ;

	bool setFileName( std::string fileName  ) ;
	bool setOutFileName( std::string fileName  ) ;

	void update( ) ;
	void output( ) ;

private:

	// allocate memory
	void updateUnit() ;
	int initMemory( ) ;

	int readMaterial( FILE *in ) ;
	int cellAllocateMemory( ) ;
	int materialAllocateMemory( ) ;
	void calculateCellPara( int n, int n_Global, int startIndex, int endIndex, float *D_Grid_Glboal, float * D_Half_Global, 
		float *Grid_Global, float *Half_Global, float *D_Grid, float *D_Half, float *Grid, float *Half , float *D_Grid_Inv, float *D_Half_Inv , int minLayerNum, int maxLayerNum ) ;
};
