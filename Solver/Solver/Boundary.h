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

#include <fstream>

// boundary condition type defination
enum Boundary_Condition{ BC_PEC= 0 , BC_PMC= 2 , BC_UPML = 4,BC_MUR= 5 , BC_EXCHANGE ,  BC_NONE} ;

class CBoundary
{
public:
	CBoundary(void);
	~CBoundary(void);

	// update boundary of E and H Field
	void update_H_Boundary(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) );
	void update_E_Boundary(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) );
private:
	Boundary_Condition m_boundary[6];
	Boundary_Condition m_boundary_Global[6];

	// Mur boundary E Field
	float ***m_Ex_Z_Mur, ***m_Ey_Z_Mur ;
	float ***m_Ex_Y_Mur, ***m_Ez_Y_Mur ;
	float ***m_Ey_X_Mur, ***m_Ez_X_Mur ;

	// UPML boundary Field
	float *m_coeff_xxm, *m_coeff_xxp, *m_coeff_xx, *m_coeff_xxp_inv ;
	float *m_coeff_yym, *m_coeff_yyp, *m_coeff_yy, *m_coeff_yyp_inv ;
	float *m_coeff_zzm, *m_coeff_zzp, *m_coeff_zz, *m_coeff_zzp_inv ;

	float *m_coeff_xyzm, *m_coeff_xyzp, *m_coeff_xyz, *m_coeff_xyzp_inv ;
	float *m_coeff_yxzm, *m_coeff_yxzp, *m_coeff_yxz, *m_coeff_yxzp_inv ;
	float *m_coeff_zxym, *m_coeff_zxyp, *m_coeff_zxy, *m_coeff_zxyp_inv ;
	 
	float ***m_Dex_Xmin, ***m_Pex_Xmin;
	float ***m_Dey_Xmin, ***m_Pey_Xmin;
	float ***m_Dez_Xmin, ***m_Pez_Xmin;
	float ***m_Dhx_Xmin, ***m_Phx_Xmin;
	float ***m_Dhy_Xmin, ***m_Phy_Xmin;
	float ***m_Dhz_Xmin, ***m_Phz_Xmin;

	float ***m_Dex_Xmax, ***m_Pex_Xmax;
	float ***m_Dey_Xmax, ***m_Pey_Xmax;
	float ***m_Dez_Xmax, ***m_Pez_Xmax;
	float ***m_Dhx_Xmax, ***m_Phx_Xmax;
	float ***m_Dhy_Xmax, ***m_Phy_Xmax;
	float ***m_Dhz_Xmax, ***m_Phz_Xmax;

	float ***m_Dex_Ymin, ***m_Pex_Ymin;
	float ***m_Dey_Ymin, ***m_Pey_Ymin;
	float ***m_Dez_Ymin, ***m_Pez_Ymin;
	float ***m_Dhx_Ymin, ***m_Phx_Ymin;
	float ***m_Dhy_Ymin, ***m_Phy_Ymin;
	float ***m_Dhz_Ymin, ***m_Phz_Ymin;

	float ***m_Dex_Ymax, ***m_Pex_Ymax;
	float ***m_Dey_Ymax, ***m_Pey_Ymax;
	float ***m_Dez_Ymax, ***m_Pez_Ymax;
	float ***m_Dhx_Ymax, ***m_Phx_Ymax;
	float ***m_Dhy_Ymax, ***m_Phy_Ymax;
	float ***m_Dhz_Ymax, ***m_Phz_Ymax;

	float ***m_Dex_Zmin, ***m_Pex_Zmin;
	float ***m_Dey_Zmin, ***m_Pey_Zmin;
	float ***m_Dez_Zmin, ***m_Pez_Zmin;
	float ***m_Dhx_Zmin, ***m_Phx_Zmin;
	float ***m_Dhy_Zmin, ***m_Phy_Zmin;
	float ***m_Dhz_Zmin, ***m_Phz_Zmin;

	float ***m_Dex_Zmax, ***m_Pex_Zmax;
	float ***m_Dey_Zmax, ***m_Pey_Zmax;
	float ***m_Dez_Zmax, ***m_Pez_Zmax;
	float ***m_Dhx_Zmax, ***m_Phx_Zmax;
	float ***m_Dhy_Zmax, ***m_Phy_Zmax;
	float ***m_Dhz_Zmax, ***m_Phz_Zmax;

	// end of UPML boundary Field defination 


	//Coefficient Combination for computational domain
	float *m_pHi_Coeff_M_Lossless ;
	float *m_pHj_Coeff_M_Lossless ;
	float *m_pHk_Coeff_M_Lossless ;

	float *m_pEi_Coeff ;
	float *m_pEj_Coeff ;
	float *m_pEk_Coeff ;

	// This two variables are for PML boundary 
	int m_boundaryLayerNum[6] ;
	int m_boundaryLayerNum_Global[6] ;


	// if PML boundary is enabled or not
	bool m_UMPL_Enabled ;

	int m_nx, m_ny , m_nz ;
	int m_nx_Global, m_ny_Global, m_nz_Global ; 
public:
	int init(void);
	void readIn(FILE *in);
	void setCellNum( int nx, int ny, int nz ) ;
	void getCellNum( int &nx, int &ny, int &nz ) ;
	void getBoundaryLayerNum( int boundaryLayerNum[6], int boundaryLayerNum_Global[6]) ;

	bool getUPML_Enabled() { return m_UMPL_Enabled ; } ;
private:
	float m_Dt ;
	float *m_Dx_Grid , *m_Dy_Grid , *m_Dz_Grid ;		// These variables can't be deleted in deconstructor

	// domain index of all processors
	DomainIndex *m_localDomainIndex ;

	CMaterial ****m_eMatType ;		// This variable can't be deleted in deconstructor

	// Boundary Condition location
	int m_Index_E_Boundary[6] ; 
	int m_Index_H_Boundary[6] ; 

	// boundary update  with PEC boundary condition
	void x_PEC_Update(int n, int boundary_Index, float*** Ex);
	void y_PEC_Update(int n, int boundary_Index, float*** Ey);
	void z_PEC_Update(int n, int boundary_Index, float*** Ez);

	// boundary update  with Mur boundary condition
	void x_Mur_Update(int n, int boundary_Index, float*** Ex);
	void y_Mur_Update(int n, int boundary_Index, float*** Ey);
	void z_Mur_Update(int n, int boundary_Index, float*** Ez);

	// boundary update  with PMC boundary condition
	void x_PMC_Update(int n, int boundary_Index, float*** Hx);
	void y_PMC_Update(int n, int boundary_Index, float*** Hy);
	void z_PMC_Update(int n, int boundary_Index, float*** Hz);



	// boundary update  with PMC boundary condition
	void UPML_Hfield_Lossless_Update(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) );
	void UPML_Efield_Update(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) );


	// end of // boundary update  with PMC boundary condition

public:

	void setH_Coeff( float *pHi_Coeff_M_Lossless, float *pHj_Coeff_M_Lossless, float *pHk_Coeff_M_Lossless ) ;
	void setE_Coeff( float *pEi_Coeff, float *pEj_Coeff, float *pEk_Coeff ) ;

	void setDt(float Dt);
	void setDGrid(float* Dx_Grid, float* Dy_Guid, float* Dz_Guid);

	// set global cell number 
	void setCellNumberGlobal( int nx_Global, int ny_Global, int nz_Global ) ;

	// set the e Material distribution array point to the pointer in FDTD class
	void setMaterial(CMaterial**** eMatType);

	// copy boundary index information
	void getBoundaryIndex(int* index_E_Boundary, int* index_H_Boundary);

	void setDomainIndex( DomainIndex *localDomainIndex ) ;

	// return boundary condition of the domain
	void getBoundary( Boundary_Condition* boundary );
	void  getBoundary_Global( Boundary_Condition* boundary );
};
