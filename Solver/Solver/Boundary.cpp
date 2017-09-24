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

#include "Boundary.h"
#include "Global_variable.h"

#include "GEMS_Memory.h"
#include "GEMS_Constant.h"

#include <cmath>
#include <mpi.h>

CBoundary::CBoundary(void)
{
	int i ;
	for( i = 0 ; i < 6 ; i++ )
	{
		m_boundary[i] = BC_NONE ;
		m_boundary_Global[i] = BC_NONE ;
		m_boundaryLayerNum[i] = 0 ;
		m_boundaryLayerNum_Global[i] = 0 ;
	}

	m_Ex_Z_Mur = NULL , m_Ey_Z_Mur = NULL ;
	m_Ex_Y_Mur = NULL , m_Ez_Y_Mur = NULL ;
	m_Ey_X_Mur = NULL , m_Ez_X_Mur = NULL ;
	m_nx = m_ny = m_nz = 1 ;

	m_eMatType = NULL ;

	m_localDomainIndex = NULL ;
	m_eMatType = NULL ;
	m_Dx_Grid = m_Dy_Grid = m_Dz_Grid = NULL ;

	// UPML boundary Field
	m_coeff_xxm = m_coeff_xxp = m_coeff_xx = m_coeff_xxp_inv = NULL ;
	m_coeff_yym = m_coeff_yyp = m_coeff_yy = m_coeff_yyp_inv = NULL ;
	m_coeff_zzm = m_coeff_zzp = m_coeff_zz = m_coeff_zzp_inv = NULL ;

	m_coeff_xyzm = m_coeff_xyzp = m_coeff_xyz = m_coeff_xyzp_inv = NULL ;
	m_coeff_yxzm = m_coeff_yxzp = m_coeff_yxz = m_coeff_yxzp_inv = NULL ;
	m_coeff_zxym = m_coeff_zxyp = m_coeff_zxy = m_coeff_zxyp_inv = NULL ;
	 
	m_Dex_Xmin = m_Pex_Xmin = NULL ;
	m_Dey_Xmin = m_Pey_Xmin = NULL ;
	m_Dez_Xmin = m_Pez_Xmin = NULL ;
	m_Dhx_Xmin = m_Phx_Xmin = NULL ;
	m_Dhy_Xmin = m_Phy_Xmin = NULL ;
	m_Dhz_Xmin = m_Phz_Xmin = NULL ;

	m_Dex_Xmax = m_Pex_Xmax = NULL ;
	m_Dey_Xmax = m_Pey_Xmax = NULL ;
	m_Dez_Xmax = m_Pez_Xmax = NULL ;
	m_Dhx_Xmax = m_Phx_Xmax = NULL ;
	m_Dhy_Xmax = m_Phy_Xmax = NULL ;
	m_Dhz_Xmax = m_Phz_Xmax = NULL ;

	m_Dex_Ymin = m_Pex_Ymin = NULL ;
	m_Dey_Ymin = m_Pey_Ymin = NULL ;
	m_Dez_Ymin = m_Pez_Ymin = NULL ;
	m_Dhx_Ymin = m_Phx_Ymin = NULL ;
	m_Dhy_Ymin = m_Phy_Ymin = NULL ;
	m_Dhz_Ymin = m_Phz_Ymin = NULL ;

	m_Dex_Ymax = m_Pex_Ymax = NULL ;
	m_Dey_Ymax = m_Pey_Ymax = NULL ;
	m_Dez_Ymax = m_Pez_Ymax = NULL ;
	m_Dhx_Ymax = m_Phx_Ymax = NULL ;
	m_Dhy_Ymax = m_Phy_Ymax = NULL ;
	m_Dhz_Ymax = m_Phz_Ymax = NULL ;

	m_Dex_Zmin = m_Pex_Zmin = NULL ;
	m_Dey_Zmin = m_Pey_Zmin = NULL ;
	m_Dez_Zmin = m_Pez_Zmin = NULL ;
	m_Dhx_Zmin = m_Phx_Zmin = NULL ;
	m_Dhy_Zmin = m_Phy_Zmin = NULL ;
	m_Dhz_Zmin = m_Phz_Zmin = NULL ;

	m_Dex_Zmax = m_Pex_Zmax = NULL ;
	m_Dey_Zmax = m_Pey_Zmax = NULL ;
	m_Dez_Zmax = m_Pez_Zmax = NULL ;
	m_Dhx_Zmax = m_Phx_Zmax = NULL ;
	m_Dhy_Zmax = m_Phy_Zmax = NULL ;
	m_Dhz_Zmax = m_Phz_Zmax = NULL ;


	m_UMPL_Enabled = false ;

	// end of UPML boundary Field

}

CBoundary::~CBoundary(void)
{
	// Mur boundary variable
	Free_3D( m_Ey_X_Mur, 4 ) ;
	Free_3D( m_Ez_X_Mur, 4 ) ;

	Free_3D( m_Ex_Y_Mur, m_nx ) ;
	Free_3D( m_Ez_Y_Mur, m_nx ) ;

	Free_3D( m_Ex_Z_Mur, m_nx ) ;
	Free_3D( m_Ey_Z_Mur, m_nx ) ;

	// UMPL boundary variable
	Free_1D( m_coeff_xxm ) ;
	Free_1D( m_coeff_xxp ) ;
	Free_1D( m_coeff_xx ) ;
	Free_1D( m_coeff_xxp_inv ) ;

	Free_1D( m_coeff_yym ) ;
	Free_1D( m_coeff_yyp ) ;
	Free_1D( m_coeff_yy ) ;
	Free_1D( m_coeff_yyp_inv ) ;

	Free_1D( m_coeff_zzm ) ;
	Free_1D( m_coeff_zzp ) ;
	Free_1D( m_coeff_zz ) ;
	Free_1D( m_coeff_zzp_inv ) ;

	Free_1D( m_coeff_xyzm ) ;
	Free_1D( m_coeff_xyzp ) ;
	Free_1D( m_coeff_xyz ) ;
	Free_1D( m_coeff_xyzp_inv ) ;

	Free_1D( m_coeff_yxzm ) ;
	Free_1D( m_coeff_yxzp ) ;
	Free_1D( m_coeff_yxz ) ;
	Free_1D( m_coeff_yxzp_inv ) ;

	Free_1D( m_coeff_zxym ) ;
	Free_1D( m_coeff_zxyp ) ;
	Free_1D( m_coeff_zxy ) ;
	Free_1D( m_coeff_zxyp_inv ) ;

	Free_3D( m_Pex_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Pey_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Pez_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Phx_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Phy_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Phz_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Dex_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Dey_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Dez_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Dhx_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Dhy_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;
	Free_3D( m_Dhz_Xmin, m_boundaryLayerNum[XMIN] + 2 ) ;


	Free_3D( m_Pex_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Pey_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Pez_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Phx_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Phy_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Phz_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Dex_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Dey_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Dez_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Dhx_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Dhy_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;
	Free_3D( m_Dhz_Xmax, m_boundaryLayerNum[XMAX] + 2 ) ;

	Free_3D( m_Pex_Ymin, m_nx + 1 ) ;
	Free_3D( m_Pey_Ymin, m_nx + 1 ) ;
	Free_3D( m_Pez_Ymin, m_nx + 1 ) ;
	Free_3D( m_Phx_Ymin, m_nx + 1 ) ;
	Free_3D( m_Phy_Ymin, m_nx + 1 ) ;
	Free_3D( m_Phz_Ymin, m_nx + 1 ) ;
	Free_3D( m_Dex_Ymin, m_nx + 1 ) ;
	Free_3D( m_Dey_Ymin, m_nx + 1 ) ;
	Free_3D( m_Dez_Ymin, m_nx + 1 ) ;
	Free_3D( m_Dhx_Ymin, m_nx + 1 ) ;
	Free_3D( m_Dhy_Ymin, m_nx + 1 ) ;
	Free_3D( m_Dhz_Ymin, m_nx + 1 ) ;
	
	Free_3D( m_Pex_Ymax, m_nx + 1 )  ;
	Free_3D( m_Pey_Ymax, m_nx + 1 )  ;
	Free_3D( m_Pez_Ymax, m_nx + 1 )  ;
	Free_3D( m_Phx_Ymax, m_nx + 1 )  ;
	Free_3D( m_Phy_Ymax, m_nx + 1 )  ;
	Free_3D( m_Phz_Ymax, m_nx + 1 )  ;
	Free_3D( m_Dex_Ymax, m_nx + 1 )  ;
	Free_3D( m_Dey_Ymax, m_nx + 1 )  ;
	Free_3D( m_Dez_Ymax, m_nx + 1 )  ;
	Free_3D( m_Dhx_Ymax, m_nx + 1 )  ;
	Free_3D( m_Dhy_Ymax, m_nx + 1 )  ;
	Free_3D( m_Dhz_Ymax, m_nx + 1 )  ;
	
	Free_3D( m_Pex_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Pey_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Pez_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Phx_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Phy_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Phz_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Dex_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Dey_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Dez_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Dhx_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Dhy_Zmin,  m_nx + 1 ) ;
	Free_3D( m_Dhz_Zmin,  m_nx + 1 ) ;
	
	Free_3D( m_Pex_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Pey_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Pez_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Phx_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Phy_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Phz_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Dex_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Dey_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Dez_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Dhx_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Dhy_Zmax,  m_nx + 1 ) ;
	Free_3D( m_Dhz_Zmax,  m_nx + 1 ) ;
}

void CBoundary::setCellNum( int nx, int ny, int nz )
{
	m_nx = nx , m_ny = ny, m_nz = nz ;
}

void CBoundary::getCellNum( int &nx, int &ny, int &nz )
{
	nx = m_nx , ny = m_ny, nz = m_nz ;
}


void CBoundary::setH_Coeff( float *pHi_Coeff_M_Lossless, float *pHj_Coeff_M_Lossless, float *pHk_Coeff_M_Lossless )
{
	m_pHi_Coeff_M_Lossless = pHi_Coeff_M_Lossless ;
	m_pHj_Coeff_M_Lossless = pHj_Coeff_M_Lossless ;
	m_pHk_Coeff_M_Lossless = pHk_Coeff_M_Lossless ;
}

void CBoundary::setE_Coeff( float *pEi_Coeff, float *pEj_Coeff, float *pEk_Coeff )
{
	m_pEi_Coeff = pEi_Coeff ;
	m_pEj_Coeff = pEj_Coeff ;
	m_pEk_Coeff = pEk_Coeff ;
}

void CBoundary::setMaterial( CMaterial ****eMatType )
{
	m_eMatType = eMatType ;
}



void CBoundary::update_H_Boundary(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	int i ;

	// Update UPML boundary first , UPML update must be called befoer PMC update
	if( m_UMPL_Enabled )
	{
		UPML_Hfield_Lossless_Update( n, Ex, Ey, Ez , Hx, Hy, Hz ) ;
	}

	for( i = 0 ; i < 6 ; i++ )
	{
		switch( i )
		{
			case XMIN :
				if( m_boundary[XMIN] == BC_PMC )
				{
					y_PMC_Update( n , XMIN, Hy ) ;
					z_PMC_Update( n , XMIN, Hz ) ;
				}
				break ;
			case XMAX :
				if( m_boundary[XMAX] == BC_PMC )
				{
					y_PMC_Update( n , XMAX, Hy ) ;
					z_PMC_Update( n , XMAX, Hz ) ;
				}
				break ;
			case YMIN :
				if( m_boundary[YMIN] == BC_PMC )
				{
					x_PMC_Update( n , YMIN, Hx ) ;
					z_PMC_Update( n , YMIN, Hz ) ;
				}
				break ;
			case YMAX :
				if( m_boundary[YMAX] == BC_PMC )
				{
					x_PMC_Update( n , YMAX, Hx ) ;
					z_PMC_Update( n , YMAX, Hz ) ;
				}
				break ;
			case ZMIN :
				if( m_boundary[ZMIN] == BC_PMC )
				{
					x_PMC_Update( n , ZMIN, Hx ) ;
					y_PMC_Update( n , ZMIN, Hy ) ;
				}
				break ;
			case ZMAX :
				if( m_boundary[ZMAX] == BC_PMC )
				{
					x_PMC_Update( n , ZMAX, Hx ) ;
					y_PMC_Update( n , ZMAX, Hy ) ;
				}
				break ;
		}
	}
}


void CBoundary::update_E_Boundary(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	int i ;

	// Update UPML boundary first , UPML update must be called befoer Mur and PEC update
	if( m_UMPL_Enabled )
	{
		UPML_Efield_Update( n, Ex, Ey, Ez , Hx, Hy, Hz ) ;
	}

	// Update Mur boundary , Mur update must be called befoer PEC update
	for( i = 0 ; i < 6 ; i++ )
	{
		switch( i )
		{
			case XMIN :
				if( m_boundary[XMIN] == BC_MUR )
				{
					y_Mur_Update( n , XMIN, Ey ) ;
					z_Mur_Update( n , XMIN, Ez ) ;
				}
				break ;
			case XMAX :
				if( m_boundary[XMAX] == BC_MUR )
				{
					y_Mur_Update( n , XMAX, Ey ) ;
					z_Mur_Update( n , XMAX, Ez ) ;
				}
				break ;
			case YMIN :
				if( m_boundary[YMIN] == BC_MUR )
				{
					x_Mur_Update( n , YMIN, Ex ) ;
					z_Mur_Update( n , YMIN, Ez ) ;
				}
				break ;
			case YMAX :
				if( m_boundary[YMAX] == BC_MUR )
				{
					x_Mur_Update( n , YMAX, Ex ) ;
					z_Mur_Update( n , YMAX, Ez ) ;
				}
				break ;
			case ZMIN :
				if( m_boundary[ZMIN] == BC_MUR )
				{
					x_Mur_Update( n , ZMIN, Ex ) ;
					y_Mur_Update( n , ZMIN, Ey ) ;
				}
				break ;
			case ZMAX :
				if( m_boundary[ZMAX] == BC_MUR )
				{
					x_Mur_Update( n , ZMAX, Ex ) ;
					y_Mur_Update( n , ZMAX, Ey ) ;
				}
				break ;
		}
	}

	// Update PEC boundary
	for( i = 0 ; i < 6 ; i++ )
	{
		switch( i )
		{
			case XMIN :
				if( m_boundary[XMIN] == BC_PEC )
				{
					y_PEC_Update( n , XMIN, Ey ) ;
					z_PEC_Update( n , XMIN, Ez ) ;
				}
				break ;
			case XMAX :
				if( m_boundary[XMAX] == BC_PEC )
				{
					y_PEC_Update( n , XMAX, Ey ) ;
					z_PEC_Update( n , XMAX, Ez ) ;
				}
				break ;
			case YMIN :
				if( m_boundary[YMIN] == BC_PEC )
				{
					x_PEC_Update( n , YMIN, Ex ) ;
					z_PEC_Update( n , YMIN, Ez ) ;
				}
				break ;
			case YMAX :
				if( m_boundary[YMAX] == BC_PEC )
				{
					x_PEC_Update( n , YMAX, Ex ) ;
					z_PEC_Update( n , YMAX, Ez ) ;
				}
				break ;
			case ZMIN :
				if( m_boundary[ZMIN] == BC_PEC )
				{
					x_PEC_Update( n , ZMIN, Ex ) ;
					y_PEC_Update( n , ZMIN, Ey ) ;
				}
				break ;
			case ZMAX :
				if( m_boundary[ZMAX] == BC_PEC )
				{
					x_PEC_Update( n , ZMAX, Ex ) ;
					y_PEC_Update( n , ZMAX, Ey ) ;
				}
				break ;
		}
	}
}

int CBoundary::init(void)
{
	int i ;

	for( i = 0 ; i < 6 ; i++ )
	{
		if( m_boundary[i] == BC_UPML )
		{
			m_UMPL_Enabled = true ;
			break ;
		}
	}
	if( m_boundary[XMIN] == BC_MUR ||  m_boundary[XMAX] == BC_MUR )
	{
		m_Ey_X_Mur = Allocate_3D( m_Ey_X_Mur, 4 , m_ny , m_nz ) ;
		if( m_Ey_X_Mur == NULL )
		{
			return FDTD_NO_MEMORY  ;
		}

		m_Ez_X_Mur = Allocate_3D( m_Ez_X_Mur, 4 , m_ny , m_nz ) ;
		if( m_Ez_X_Mur == NULL )
		{
			return FDTD_NO_MEMORY  ;
		}
	}

	if( m_boundary[YMIN] == BC_MUR ||  m_boundary[YMAX] == BC_MUR )
	{
		m_Ex_Y_Mur = Allocate_3D( m_Ex_Y_Mur, m_nx , 4 , m_nz ) ;
		if( m_Ex_Y_Mur == NULL )
		{
			return FDTD_NO_MEMORY  ;
		}

		m_Ez_Y_Mur = Allocate_3D( m_Ez_Y_Mur, m_nx , 4 , m_nz ) ;
		if( m_Ez_Y_Mur == NULL )
		{
			return FDTD_NO_MEMORY  ;
		}
	}

	if( m_boundary[ZMIN] == BC_MUR ||  m_boundary[ZMAX] == BC_MUR )
	{
		m_Ex_Z_Mur = Allocate_3D( m_Ex_Z_Mur, m_nx , m_ny, 4 ) ;
		if( m_Ex_Z_Mur == NULL )
		{
			return FDTD_NO_MEMORY  ;
		}

		m_Ey_Z_Mur = Allocate_3D( m_Ey_Z_Mur, m_nx , m_ny, 4 ) ;
		if( m_Ey_Z_Mur == NULL )
		{
			return FDTD_NO_MEMORY  ;
		}
	}


	//
	if( m_UMPL_Enabled )
	{
		m_coeff_xxm = Allocate_1D( m_coeff_xxm, m_nx + 1 ) ;
		if( m_coeff_xxm == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_xxp = Allocate_1D( m_coeff_xxp, m_nx + 1 ) ;
		if( m_coeff_xxp == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_xx = Allocate_1D( m_coeff_xx, m_nx + 1 ) ;
		if( m_coeff_xx == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_xxp_inv = Allocate_1D( m_coeff_xxp_inv, m_nx + 1 ) ;
		if( m_coeff_xxp_inv == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_xyzm = Allocate_1D( m_coeff_xyzm, m_nx + 1 ) ;
		if( m_coeff_xyzm == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_xyzp = Allocate_1D( m_coeff_xyzp, m_nx + 1 ) ;
		if( m_coeff_xyzp == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_xyz = Allocate_1D( m_coeff_xyz, m_nx + 1 ) ;
		if( m_coeff_xyz == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_xyzp_inv = Allocate_1D( m_coeff_xyzp_inv, m_nx + 1 ) ;
		if( m_coeff_xyzp_inv == NULL )
			return FDTD_NO_MEMORY ;

		// init to 1.0
		for( i = 0 ; i < m_nx + 1 ; i++ )
		{
			m_coeff_xxm[i] = 1.0f ;
			m_coeff_xxp[i] = 1.0f ;
			m_coeff_xx[i] = 1.0f ;
			m_coeff_xxp_inv[i] = 1.0f ;
			m_coeff_xyzm[i] = 1.0f ;
			m_coeff_xyzp[i] = 1.0f ;
			m_coeff_xyz[i] = 1.0f ;
			m_coeff_xyzp_inv[i] = 1.0f ;
		}

		m_coeff_yym = Allocate_1D( m_coeff_yym, m_ny + 1 ) ;
		if( m_coeff_yym == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_yyp = Allocate_1D( m_coeff_yyp, m_ny + 1 ) ;
		if( m_coeff_yyp == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_yy = Allocate_1D( m_coeff_yy, m_ny + 1 ) ;
		if( m_coeff_yy == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_yyp_inv = Allocate_1D( m_coeff_yyp_inv, m_ny + 1 ) ;
		if( m_coeff_yyp_inv == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_yxzm = Allocate_1D( m_coeff_yxzm, m_ny + 1 ) ;
		if( m_coeff_yxzm == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_yxzp = Allocate_1D( m_coeff_yxzp, m_ny + 1 ) ;
		if( m_coeff_yxzp == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_yxz = Allocate_1D( m_coeff_yxz, m_ny + 1 ) ;
		if( m_coeff_yxz == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_yxzp_inv = Allocate_1D( m_coeff_yxzp_inv, m_ny + 1 ) ;
		if( m_coeff_yxzp_inv == NULL )
			return FDTD_NO_MEMORY ;

		for( i = 0 ; i < m_ny + 1 ; i++ )
		{
			m_coeff_yym[i] = 1.0f ;
			m_coeff_yyp[i] = 1.0f ;
			m_coeff_yy[i] = 1.0f ;
			m_coeff_yyp_inv[i] = 1.0f ;
			m_coeff_yxzm[i] = 1.0f ;
			m_coeff_yxzp[i] = 1.0f ;
			m_coeff_yxz[i] = 1.0f ;
			m_coeff_yxzp_inv[i] = 1.0f ;
		}

		m_coeff_zzm = Allocate_1D( m_coeff_zzm, m_nz + 1 ) ;
		if( m_coeff_zzm == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_zzp = Allocate_1D( m_coeff_zzp, m_nz + 1 ) ;
		if( m_coeff_zzp == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_zz = Allocate_1D( m_coeff_zz, m_nz + 1 ) ;
		if( m_coeff_zz == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_zzp_inv = Allocate_1D( m_coeff_zzp_inv, m_nz + 1 ) ;
		if( m_coeff_zzp_inv == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_zxym = Allocate_1D( m_coeff_zxym, m_nz + 1 ) ;
		if( m_coeff_zxym == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_zxyp = Allocate_1D( m_coeff_zxyp, m_nz + 1 ) ;
		if( m_coeff_zxyp == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_zxy = Allocate_1D( m_coeff_zxy, m_nz + 1 ) ;
		if( m_coeff_zxy == NULL )
			return FDTD_NO_MEMORY ;

		m_coeff_zxyp_inv = Allocate_1D( m_coeff_zxyp_inv, m_nz + 1 ) ;
		if( m_coeff_zxyp_inv == NULL )
			return FDTD_NO_MEMORY ;

		for( i = 0 ; i < m_nz + 1 ; i++ )
		{
			m_coeff_zzm[i] = 1.0f ;
			m_coeff_zzp[i] = 1.0f ;
			m_coeff_zz[i] = 1.0f ;
			m_coeff_zzp_inv[i] = 1.0f ;
			m_coeff_zxym[i] = 1.0f ;
			m_coeff_zxyp[i] = 1.0f ;
			m_coeff_zxy[i] = 1.0f ;
			m_coeff_zxyp_inv[i] = 1.0f ;
		}
	}

	if( m_boundary[XMIN] == BC_UPML )
	{
		m_Pex_Xmin = Allocate_3D( m_Pex_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Pex_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pey_Xmin = Allocate_3D( m_Pey_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Pey_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pez_Xmin = Allocate_3D( m_Pez_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Pez_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phx_Xmin = Allocate_3D( m_Phx_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Phx_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phy_Xmin = Allocate_3D( m_Phy_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Phy_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phz_Xmin = Allocate_3D( m_Phz_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Phz_Xmin == NULL )
			return FDTD_NO_MEMORY  ;


		m_Dex_Xmin = Allocate_3D( m_Dex_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dex_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dey_Xmin = Allocate_3D( m_Dey_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dey_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dez_Xmin = Allocate_3D( m_Dez_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dez_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhx_Xmin = Allocate_3D( m_Dhx_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dhx_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhy_Xmin = Allocate_3D( m_Dhy_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dhy_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhz_Xmin = Allocate_3D( m_Dhz_Xmin, m_boundaryLayerNum[XMIN] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dhz_Xmin == NULL )
			return FDTD_NO_MEMORY  ;

	}

	if( m_boundary[XMAX] == BC_UPML )
	{
		m_Pex_Xmax = Allocate_3D( m_Pex_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Pex_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pey_Xmax = Allocate_3D( m_Pey_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Pey_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pez_Xmax = Allocate_3D( m_Pez_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Pez_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phx_Xmax = Allocate_3D( m_Phx_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Phx_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phy_Xmax = Allocate_3D( m_Phy_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Phy_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phz_Xmax = Allocate_3D( m_Phz_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Phz_Xmax == NULL )
			return FDTD_NO_MEMORY  ;


		m_Dex_Xmax = Allocate_3D( m_Dex_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dex_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dey_Xmax = Allocate_3D( m_Dey_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dey_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dez_Xmax = Allocate_3D( m_Dez_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dez_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhx_Xmax = Allocate_3D( m_Dhx_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dhx_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhy_Xmax = Allocate_3D( m_Dhy_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dhy_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhz_Xmax = Allocate_3D( m_Dhz_Xmax, m_boundaryLayerNum[XMAX] + 2, m_ny+1 , m_nz+1 ) ;
		if( m_Dhz_Xmax == NULL )
			return FDTD_NO_MEMORY  ;

	}

	if( m_boundary[YMIN] == BC_UPML )
	{

		m_Pex_Ymin = Allocate_3D( m_Pex_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Pex_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pey_Ymin = Allocate_3D( m_Pey_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Pey_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pez_Ymin = Allocate_3D( m_Pez_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Pez_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phx_Ymin = Allocate_3D( m_Phx_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Phx_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phy_Ymin = Allocate_3D( m_Phy_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Phy_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phz_Ymin = Allocate_3D( m_Phz_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Phz_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dex_Ymin = Allocate_3D( m_Dex_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Dex_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dey_Ymin = Allocate_3D( m_Dey_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Dey_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dez_Ymin = Allocate_3D( m_Dez_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Dez_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhx_Ymin = Allocate_3D( m_Dhx_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Dhx_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhy_Ymin = Allocate_3D( m_Dhy_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Dhy_Ymin == NULL )
			return FDTD_NO_MEMORY  ;
		
		m_Dhz_Ymin = Allocate_3D( m_Dhz_Ymin, m_nx + 1 , m_boundaryLayerNum[YMIN] + 2 , m_nz+1  ) ;
		if( m_Dhz_Ymin == NULL )
			return FDTD_NO_MEMORY  ;

	}

	if( m_boundary[YMAX] == BC_UPML )
	{

		m_Pex_Ymax = Allocate_3D( m_Pex_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Pex_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pey_Ymax = Allocate_3D( m_Pey_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Pey_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Pez_Ymax = Allocate_3D( m_Pez_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Pez_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phx_Ymax = Allocate_3D( m_Phx_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Phx_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phy_Ymax = Allocate_3D( m_Phy_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Phy_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Phz_Ymax = Allocate_3D( m_Phz_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Phz_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dex_Ymax = Allocate_3D( m_Dex_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Dex_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dey_Ymax = Allocate_3D( m_Dey_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Dey_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dez_Ymax = Allocate_3D( m_Dez_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Dez_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhx_Ymax = Allocate_3D( m_Dhx_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Dhx_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhy_Ymax = Allocate_3D( m_Dhy_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Dhy_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

		m_Dhz_Ymax = Allocate_3D( m_Dhz_Ymax, m_nx + 1 , m_boundaryLayerNum[YMAX] + 2 , m_nz+1  )  ;
		if( m_Dhz_Ymax == NULL )
			return FDTD_NO_MEMORY  ;

	}

	if( m_boundary[ZMIN] == BC_UPML )
	{

		m_Pex_Zmin = Allocate_3D( m_Pex_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Pex_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Pey_Zmin = Allocate_3D( m_Pey_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Pey_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Pez_Zmin = Allocate_3D( m_Pez_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Pez_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Phx_Zmin = Allocate_3D( m_Phx_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Phx_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Phy_Zmin = Allocate_3D( m_Phy_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Phy_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Phz_Zmin = Allocate_3D( m_Phz_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Phz_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dex_Zmin = Allocate_3D( m_Dex_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Dex_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dey_Zmin = Allocate_3D( m_Dey_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Dey_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dez_Zmin = Allocate_3D( m_Dez_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Dez_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dhx_Zmin = Allocate_3D( m_Dhx_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Dhx_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dhy_Zmin = Allocate_3D( m_Dhy_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Dhy_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dhz_Zmin = Allocate_3D( m_Dhz_Zmin,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMIN] + 2  ) ;
		if( m_Dhz_Zmin == NULL )
				return FDTD_NO_MEMORY  ;

	}

	if( m_boundary[ZMAX] == BC_UPML )
	{

		m_Pex_Zmax = Allocate_3D( m_Pex_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Pex_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Pey_Zmax = Allocate_3D( m_Pey_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Pey_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Pez_Zmax = Allocate_3D( m_Pez_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Pez_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Phx_Zmax = Allocate_3D( m_Phx_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Phx_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Phy_Zmax = Allocate_3D( m_Phy_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Phy_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Phz_Zmax = Allocate_3D( m_Phz_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Phz_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dex_Zmax = Allocate_3D( m_Dex_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Dex_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dey_Zmax = Allocate_3D( m_Dey_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Dey_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dez_Zmax = Allocate_3D( m_Dez_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Dez_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dhx_Zmax = Allocate_3D( m_Dhx_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Dhx_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dhy_Zmax = Allocate_3D( m_Dhy_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Dhy_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

		m_Dhz_Zmax = Allocate_3D( m_Dhz_Zmax,  m_nx + 1 , m_ny + 1 , m_boundaryLayerNum[ZMAX] + 2  ) ;
		if( m_Dhz_Zmax == NULL )
				return FDTD_NO_MEMORY  ;

	}

	float A,pmllossx1,pmllossx2,pmllossy1,pmllossy2,pmllossz1,pmllossz2 ;
	int m, j, k ;
	float pmlepsmax, tep1, tep2 ;

	m = 4;

    pmllossx1 = 0.75f * (m + 1) / ( 150.0f * PI * m_Dx_Grid[1] ) ;
	pmllossy1 = 0.75f * (m + 1) / ( 150.0f * PI * m_Dy_Grid[1] ) ;
	pmllossz1 = 0.75f * (m + 1) / ( 150.0f * PI * m_Dz_Grid[1] ) ;

    pmllossx2 = 0.75f * (m + 1) / ( 150.0f * PI * m_Dx_Grid[m_nx-1] ) ;
	pmllossy2 = 0.75f * (m + 1) / ( 150.0f * PI * m_Dy_Grid[m_ny-1] ) ;
	pmllossz2 = 0.75f * (m + 1) / ( 150.0f * PI * m_Dz_Grid[m_nz-1] ) ;

	pmlepsmax = 1 ;

	if(m_boundaryLayerNum[XMIN] > 0) 
	{
		for(i = m_Index_H_Boundary[XMIN]; i < m_Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN]; i++) 
		{
			A = fabs(float(i - 0.5f - m_boundaryLayerNum[XMIN]) / float(m_boundaryLayerNum[XMIN]));
			tep1 = pmllossx1 * pow(A, m) ;
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m);
			m_coeff_xxm[i] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xxp[i] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xxp_inv[i] = 1.0f / m_coeff_xxp[i] ;
			m_coeff_xx[i] = m_coeff_xxm[i] * m_coeff_xxp_inv[i] ;

		}
	}

	if(m_boundaryLayerNum[XMAX] > 0)
	{
		for( i = m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1 ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++ ) 
		{
			A = fabs(float(m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] - i + 0.5f) / float(m_boundaryLayerNum[XMAX])) ; 
			tep1 = pmllossx2 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m);
			m_coeff_xxm[i] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xxp[i] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xxp_inv[i] = 1.0f / m_coeff_xxp[i] ;
			m_coeff_xx[i] = m_coeff_xxm[i] * m_coeff_xxp_inv[i] ;
		}
	}

	if(m_boundaryLayerNum[XMIN] > 0)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i++ ) 
		{
			A = fabs(float(m_Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN] - i) / float(m_boundaryLayerNum[XMIN])) ;
			tep1 = pmllossx1 * pow(A, m) ;
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_xyzm[i] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xyzp[i] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xyzp_inv[i] = 1.0f / m_coeff_xyzp[i] ;
			m_coeff_xyz[i] = m_coeff_xyzm[i] * m_coeff_xyzp_inv[i] ;
		}
	}

	if(m_boundaryLayerNum[XMAX] > 0)
	{
		for(i = m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] ; i <= m_Index_E_Boundary[XMAX] + 1 ; i++) 
		{
			A = fabs(float(m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] - i) / float(m_boundaryLayerNum[XMAX])) ; 
			tep1 = pmllossx2 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_xyzm[i] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xyzp[i] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_xyzp_inv[i] = 1.0f / m_coeff_xyzp[i] ;
			m_coeff_xyz[i] = m_coeff_xyzm[i] * m_coeff_xyzp_inv[i] ;
		}
	}


	if(m_boundaryLayerNum[YMIN] > 0) 
	{
		for(j = m_Index_H_Boundary[YMIN]; j < m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j++) 
		{
			A=fabs(float(j - 0.5f - m_boundaryLayerNum[YMIN]) / float(m_boundaryLayerNum[YMIN])) ;
			tep1 = pmllossy1 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_yym[j] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yyp[j] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yyp_inv[j] = 1.0f / m_coeff_yyp[j] ;
			m_coeff_yy[j] = m_coeff_yym[j] * m_coeff_yyp_inv[j] ;
		}
	}

	if(m_boundaryLayerNum[YMAX] > 0)
	{
		for(j = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1; j <= m_Index_H_Boundary[YMAX] + 1 ; j++) 
		{
			A = fabs(float(m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] - j + 0.5f) / float(m_boundaryLayerNum[YMAX])) ;
			tep1 = pmllossy2 * pow(A, m) ;
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ;
			m_coeff_yym[j] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yyp[j] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yyp_inv[j] = 1.0f / m_coeff_yyp[j] ;
			m_coeff_yy[j] = m_coeff_yym[j] * m_coeff_yyp_inv[j] ;
		}
	}

	if(m_boundaryLayerNum[YMIN] > 0)
	{
		for(j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j++) 
		{
			A = fabs(float(m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN] - j ) / float(m_boundaryLayerNum[YMIN])) ; 
			tep1 = pmllossy1 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_yxzm[j] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yxzp[j] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yxzp_inv[j] = 1.0f / m_coeff_yxzp[j] ;
			m_coeff_yxz[j] = m_coeff_yxzm[j] * m_coeff_yxzp_inv[j] ;
		}
	}

	if(m_boundaryLayerNum[YMAX] > 0) 
	{
		for(j = m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX]; j <= m_Index_E_Boundary[YMAX] + 1 ; j++ ) 
		{
			A = fabs(float(m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] - j) / float(m_boundaryLayerNum[YMAX])) ; 
			tep1 = pmllossy2 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_yxzm[j] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yxzp[j] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_yxzp_inv[j] = 1.0f / m_coeff_yxzp[j] ;
			m_coeff_yxz[j] = m_coeff_yxzm[j] * m_coeff_yxzp_inv[j] ;
		}
	}


	if(m_boundaryLayerNum[ZMIN] > 0)
	{
		for(k = m_Index_H_Boundary[ZMIN]; k < m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k++) 
		{
			A = fabs(float(k - 0.5f - m_boundaryLayerNum[ZMIN]) / float(m_boundaryLayerNum[ZMIN])) ; 
			tep1 = pmllossz1 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_zzm[k] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zzp[k] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zzp_inv[k] = 1.0f / m_coeff_zzp[k] ;
			m_coeff_zz[k] = m_coeff_zzm[k] * m_coeff_zzp_inv[k] ;
		}
	}

	if(m_boundaryLayerNum[ZMAX] > 0) 
	{
		for(k = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1; k <= m_Index_H_Boundary[ZMAX] + 1 ; k++) 
		{
			A = fabs(float(m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] - k + 0.5f) / float(m_boundaryLayerNum[ZMAX])) ; 
			tep1 = pmllossz2 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_zzm[k] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zzp[k] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zzp_inv[k] = 1.0f / m_coeff_zzp[k] ;
			m_coeff_zz[k] = m_coeff_zzm[k] * m_coeff_zzp_inv[k] ;
		}
	}

	if(m_boundaryLayerNum[ZMIN] > 0)
	{
		for(k = m_Index_E_Boundary[ZMIN] ; k <= m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k++ ) 
		{
			A = fabs(float(m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] - k ) / float(m_boundaryLayerNum[ZMIN])) ; 
			tep1 = pmllossz1 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_zxym[k] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zxyp[k] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zxyp_inv[k] = 1.0f / m_coeff_zxyp[k] ;
			m_coeff_zxy[k] = m_coeff_zxym[k] * m_coeff_zxyp_inv[k] ;
		}
	}

	if(m_boundaryLayerNum[ZMAX] > 0)
	{
		for(k = m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k <= m_Index_E_Boundary[ZMAX] + 1 ; k++ ) 
		{
			A = fabs(float(m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] - k) / float(m_boundaryLayerNum[ZMAX])) ; 
			tep1 = pmllossz2 * pow(A, m) ; 
			tep2 = 1.0f + (pmlepsmax - 1.0f) * pow(A, m) ; 
			m_coeff_zxym[k] = tep2 - 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zxyp[k] = tep2 + 0.5f * m_Dt * tep1 * Eps0_Inv ;
			m_coeff_zxyp_inv[k] = 1.0f / m_coeff_zxyp[k] ;
			m_coeff_zxy[k] = m_coeff_zxym[k] * m_coeff_zxyp_inv[k] ;
		}
	}


	//
	return FDTD_SUCCESS;
}

void CBoundary::setDomainIndex( DomainIndex *localDomainIndex ) 
{
	m_localDomainIndex = localDomainIndex ;
}

void CBoundary::readIn(FILE *in)
{
	int i ;
	int bc[6] ;
	char endMark[2] ;
	if( id == 0 )
	{
		fread( bc, sizeof( int ), 6, in ) ;
		fread( endMark, sizeof( char ), 2, in ) ;

	}


	MPI_Bcast( bc, 6 , MPI_INT , 0 , MPI_COMM_WORLD ) ;
	for( i = 0 ; i < 6 ; i++ )
	{
		m_boundary_Global[i] = static_cast< Boundary_Condition> ( bc[i] ) ;
		m_boundary[i] = static_cast< Boundary_Condition> ( bc[i] ) ;

		// if boundary is PML, set boundary PML layer number to 6
		if( m_boundary_Global[i] == BC_UPML )
		{
			m_boundaryLayerNum[i] = PML_LAYERNUM ;
			m_boundaryLayerNum_Global[i] = PML_LAYERNUM ;
		}
	}


	// found the neighbor of current domain
	int neighbor[6] ;
	for( i = 0 ; i < 6 ; i++ )
		neighbor[i] = -1 ;
	if( id != 0 )
	{
		for( i = 0 ; i < numSolver ; i++ )
		{
			// neighbors in x direction
			if( m_localDomainIndex[idSolver].startY == m_localDomainIndex[i].startY &&
				m_localDomainIndex[idSolver].endY == m_localDomainIndex[i].endY &&
				m_localDomainIndex[idSolver].startZ == m_localDomainIndex[i].startZ &&
				m_localDomainIndex[idSolver].endZ == m_localDomainIndex[i].endZ )
			{
				if( m_localDomainIndex[idSolver].startX == m_localDomainIndex[i].endX )
					neighbor[XMIN] = i ; 
				else if( m_localDomainIndex[idSolver].endX == m_localDomainIndex[i].startX )
					neighbor[XMAX] = i ; 
			}

			// neighbors in y direction
			if( m_localDomainIndex[idSolver].startX == m_localDomainIndex[i].startX &&
				m_localDomainIndex[idSolver].endX == m_localDomainIndex[i].endX &&
				m_localDomainIndex[idSolver].startZ == m_localDomainIndex[i].startZ &&
				m_localDomainIndex[idSolver].endZ == m_localDomainIndex[i].endZ )
			{
				if( m_localDomainIndex[idSolver].startY == m_localDomainIndex[i].endY )
					neighbor[YMIN] = i ; 
				else if( m_localDomainIndex[idSolver].endY == m_localDomainIndex[i].startY )
					neighbor[YMAX] = i ; 
			}

			// neighbors in z direction
			if( m_localDomainIndex[idSolver].startX == m_localDomainIndex[i].startX &&
				m_localDomainIndex[idSolver].endX == m_localDomainIndex[i].endX &&
				m_localDomainIndex[idSolver].startY == m_localDomainIndex[i].startY &&
				m_localDomainIndex[idSolver].endY == m_localDomainIndex[i].endY )
			{
				if( m_localDomainIndex[idSolver].startZ == m_localDomainIndex[i].endZ )
					neighbor[ZMIN] = i ; 
				else if( m_localDomainIndex[idSolver].endZ == m_localDomainIndex[i].startZ )
					neighbor[ZMAX] = i ; 
			}
		}

		// Re-calculate Boundary for each processor
		if( m_localDomainIndex[idSolver].startX != 0 )
		{
			if( neighbor[XMIN] != -1 )
				m_boundary[XMIN] = BC_EXCHANGE ;

			m_boundaryLayerNum[XMIN] = 0 ;
		}


		if( m_localDomainIndex[idSolver].endX != m_nx_Global )
		{
			if( neighbor[XMAX] != -1 )
				m_boundary[XMAX] = BC_EXCHANGE ;

			m_boundaryLayerNum[XMAX] = 0 ;
		}

		if( m_localDomainIndex[idSolver].startY != 0 )
		{
			if( neighbor[YMIN] != -1 )
				m_boundary[YMIN] = BC_EXCHANGE ;

			m_boundaryLayerNum[YMIN] = 0 ;
		}

		if( m_localDomainIndex[idSolver].endY != m_ny_Global )
		{
			if( neighbor[YMAX] != -1 )
				m_boundary[YMAX] = BC_EXCHANGE ;

			m_boundaryLayerNum[YMAX] = 0 ;
		}

		if( m_localDomainIndex[idSolver].startZ != 0 )
		{
			if( neighbor[ZMIN] != -1 )
				m_boundary[ZMIN] = BC_EXCHANGE ;

			m_boundaryLayerNum[ZMIN] = 0 ;
		}

		if( m_localDomainIndex[idSolver].endZ != m_nz_Global )
		{
			if( neighbor[ZMAX] != -1 )
				m_boundary[ZMAX] = BC_EXCHANGE ;

			m_boundaryLayerNum[ZMAX] = 0 ;
		}

		// calculate cell number of local domain
		m_nx = m_localDomainIndex[idSolver].endX - m_localDomainIndex[idSolver].startX + 2 ;
		m_ny = m_localDomainIndex[idSolver].endY - m_localDomainIndex[idSolver].startY + 2 ;
		m_nz = m_localDomainIndex[idSolver].endZ - m_localDomainIndex[idSolver].startZ + 2 ;
		//the number 2 above is two additional cells attached to the computational domain.
		
		// add PML layer number
		m_nx += m_boundaryLayerNum[XMIN] + m_boundaryLayerNum[XMAX] ;
		m_ny += m_boundaryLayerNum[YMIN] + m_boundaryLayerNum[YMAX] ;
		m_nz += m_boundaryLayerNum[ZMIN] + m_boundaryLayerNum[ZMAX] ;
		
		// get Field update Index
		m_Index_E_Boundary[XMIN] = 1 ;
		m_Index_E_Boundary[XMAX] = m_nx - 1 ;
		m_Index_E_Boundary[YMIN] = 1 ;
		m_Index_E_Boundary[YMAX] = m_ny - 1 ;
		m_Index_E_Boundary[ZMIN] = 1 ;
		m_Index_E_Boundary[ZMAX] = m_nz - 1 ;

		m_Index_H_Boundary[XMIN] = m_Index_E_Boundary[XMIN] ;
		m_Index_H_Boundary[XMAX] = m_Index_E_Boundary[XMAX] - 1 ;
		m_Index_H_Boundary[YMIN] = m_Index_E_Boundary[YMIN] ;
		m_Index_H_Boundary[YMAX] = m_Index_E_Boundary[YMAX] - 1 ;
		m_Index_H_Boundary[ZMIN] = m_Index_E_Boundary[ZMIN] ;
		m_Index_H_Boundary[ZMAX] = m_Index_E_Boundary[ZMAX] - 1 ;
	}
}


void CBoundary::x_PEC_Update(int n, int boundary_Index, float*** Ex)
{
	int i , j , k ;

	switch( boundary_Index )
	{

		case YMIN : 
			for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k <= m_Index_E_Boundary[ZMAX] ; k++ )
					Ex[i][m_Index_E_Boundary[YMIN]][k] = 0.0 ;
			}
			break ;
		case YMAX : 
			for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k <= m_Index_E_Boundary[ZMAX] ; k++ )
					Ex[i][m_Index_E_Boundary[YMAX]][k] = 0.0 ;
			}
			break ;
		case ZMIN : 
			for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++ )
					Ex[i][j][m_Index_E_Boundary[ZMIN]] = 0.0 ;
			}
			break ;
		case ZMAX : 
			for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++ )
					Ex[i][j][m_Index_E_Boundary[ZMAX]] = 0.0 ;
			}
			break;
		case XMIN :
		case XMAX :
			break;
	}
}

void CBoundary::y_PEC_Update(int n, int boundary_Index, float*** Ey)
{
	int i , j , k ;

	switch( boundary_Index )
	{
		case XMIN :
			for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMAX] ; j++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k <= m_Index_E_Boundary[ZMAX] ; k++ )
					Ey[m_Index_E_Boundary[XMIN]][j][k] = 0.0 ;
			}
			break ;
		case XMAX : 
			for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMAX] ; j++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k <= m_Index_E_Boundary[ZMAX] ; k++ )
					Ey[m_Index_E_Boundary[XMAX]][j][k] = 0.0 ;
			}
			break ;
		case ZMIN : 
			for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMAX] ; j++ )
					Ey[i][j][m_Index_E_Boundary[ZMIN]] = 0.0 ;
			}
			break ;
		case ZMAX : 
			for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMAX] ; j++ )
					Ey[i][j][m_Index_E_Boundary[ZMAX]] = 0.0 ;
			}
			break ;
		case YMIN :
		case YMAX :
			break;
	}
}

void CBoundary::z_PEC_Update(int n, int boundary_Index, float*** Ez)
{
	int i , j , k ;

	switch( boundary_Index )
	{
		case XMIN :
			for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k < m_Index_E_Boundary[ZMAX] ; k++ )
					Ez[m_Index_E_Boundary[XMIN]][j][k] = 0.0 ;
			}
			break ;
		case XMAX : 
			for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k < m_Index_E_Boundary[ZMAX] ; k++ )
					Ez[m_Index_E_Boundary[XMAX]][j][k] = 0.0 ;
			}
			break ;
		case YMIN : 
			for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k < m_Index_E_Boundary[ZMAX] ; k++ )
					Ez[i][m_Index_E_Boundary[YMIN]][k] = 0.0 ;
			}
			break ;
		case YMAX : 
			for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++ )
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k < m_Index_E_Boundary[ZMAX] ; k++ )
					Ez[i][m_Index_E_Boundary[YMAX]][k] = 0.0 ;
			}
			break ;
		case ZMIN :
		case ZMAX :
			break;
	}
}


void CBoundary::x_Mur_Update(int n, int boundary_Index, float*** Ex)
{
	float A, B;
	int i, j, k;

	switch( boundary_Index )
	{
		case YMIN : 
			j = m_Index_E_Boundary[YMIN]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX] - 1 ; i++) 
			{
				for (k = m_Index_E_Boundary[ZMIN]; k <= m_Index_E_Boundary[ZMAX]; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_y) * m_Dt  - m_Dy_Grid[j];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_y) * m_Dt  + m_Dy_Grid[j];

					Ex[i][j][k] = m_Ex_Y_Mur[i][1][k] + A / B * (Ex[i][j + 1][k] - m_Ex_Y_Mur[i][0][k]);

					m_Ex_Y_Mur[i][0][k] = Ex[i][j][k];
					m_Ex_Y_Mur[i][1][k] = Ex[i][j + 1][k];
				}
			}
			break ;
		case YMAX : 
			j = m_Index_E_Boundary[YMAX]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX] - 1 ; i++) 
			{
				for (k = m_Index_E_Boundary[ZMIN]; k <= m_Index_E_Boundary[ZMAX]; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j - 1][k]->Eps_y) * m_Dt - m_Dy_Grid[j - 1];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j - 1][k]->Eps_y) * m_Dt + m_Dy_Grid[j - 1];

					Ex[i][j][k] = m_Ex_Y_Mur[i][2][k] + A / B * (Ex[i][j - 1][k] - m_Ex_Y_Mur[i][3][k]);

					m_Ex_Y_Mur[i][3][k] = Ex[i][j][k];
					m_Ex_Y_Mur[i][2][k] = Ex[i][j - 1][k];
				}
			}
			break ;
		case ZMIN : 
			k = m_Index_E_Boundary[ZMIN]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX] - 1 ; i++) 
			{
				for (j = m_Index_E_Boundary[YMIN]; j <= m_Index_E_Boundary[YMAX]; j++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_z) * m_Dt - m_Dz_Grid[k];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_z) * m_Dt + m_Dz_Grid[k];

					Ex[i][j][k] = m_Ex_Z_Mur[i][j][1] + A / B * (Ex[i][j][k + 1] - m_Ex_Z_Mur[i][j][0]);
					
					m_Ex_Z_Mur[i][j][0] = Ex[i][j][k];
					m_Ex_Z_Mur[i][j][1] = Ex[i][j][k + 1];
				}
			}
			break ;
		case ZMAX : 
			k = m_Index_E_Boundary[ZMAX]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX] - 1 ; i++) 
			{
				for (j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX]; j++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k - 1]->Eps_z) * m_Dt - m_Dz_Grid[k - 1];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k - 1]->Eps_z) * m_Dt + m_Dz_Grid[k - 1];


					Ex[i][j][k] = m_Ex_Z_Mur[i][j][2] + A / B * (Ex[i][j][k - 1] - m_Ex_Z_Mur[i][j][3]);

					m_Ex_Z_Mur[i][j][3] = Ex[i][j][k];
					m_Ex_Z_Mur[i][j][2] = Ex[i][j][k - 1];
				}
			}
			break ;
		case XMIN :
		case XMAX :
			break;
	}
}

void CBoundary::y_Mur_Update(int n, int boundary_Index, float*** Ey)
{
	float A, B;
	int i, j, k;


	switch( boundary_Index )
	{
		case XMIN : 
			i = m_Index_E_Boundary[XMIN]; 

			for (j = m_Index_E_Boundary[YMIN]; j <= m_Index_E_Boundary[YMAX] - 1 ; j++) 
			{
				for (k = m_Index_E_Boundary[ZMIN]; k <= m_Index_E_Boundary[ZMAX]; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_x) * m_Dt - m_Dx_Grid[i];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_x) * m_Dt + m_Dx_Grid[i];

					Ey[i][j][k] = m_Ey_X_Mur[1][j][k] + A / B * (Ey[i + 1][j][k] - m_Ey_X_Mur[0][j][k]);

					m_Ey_X_Mur[0][j][k] = Ey[i][j][k];
					m_Ey_X_Mur[1][j][k] = Ey[i + 1][j][k];
				}
			}
			break ;
		case XMAX : 
			i = m_Index_E_Boundary[XMAX];
			for (j = m_Index_E_Boundary[YMIN]; j <= m_Index_E_Boundary[YMAX] - 1 ; j++) 
			{
				for (k = m_Index_E_Boundary[ZMIN]; k <= m_Index_E_Boundary[ZMAX]; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i - 1][j][k]->Eps_x) * m_Dt - m_Dx_Grid[i - 1];
					B = C_Speed * sqrt(1.0f / m_eMatType[i - 1][j][k]->Eps_x) * m_Dt + m_Dx_Grid[i - 1];

					Ey[i][j][k] = m_Ey_X_Mur[2][j][k] + A / B * (Ey[i - 1][j][k] - m_Ey_X_Mur[3][j][k]);

					m_Ey_X_Mur[3][j][k] = Ey[i][j][k];
					m_Ey_X_Mur[2][j][k] = Ey[i - 1][j][k];
				}
			}
			break ;
		case ZMIN : 
			k = m_Index_E_Boundary[ZMIN]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX]; i++) 
			{
				for (j = m_Index_E_Boundary[YMIN]; j <= m_Index_E_Boundary[YMAX] - 1 ; j++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_z) * m_Dt - m_Dz_Grid[k];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_z) * m_Dt + m_Dz_Grid[k];

					Ey[i][j][k] = m_Ey_Z_Mur[i][j][1] + A / B * (Ey[i][j][k + 1] - m_Ey_Z_Mur[i][j][0]);

					m_Ey_Z_Mur[i][j][0] = Ey[i][j][k];
					m_Ey_Z_Mur[i][j][1] = Ey[i][j][k + 1];
				}
			}
			break ;
		case ZMAX : 
			k = m_Index_E_Boundary[ZMAX]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX]; i++) 
			{
				for (j = m_Index_E_Boundary[YMIN]; j <= m_Index_E_Boundary[YMAX] - 1 ; j++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k - 1]->Eps_z) * m_Dt - m_Dz_Grid[k - 1];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k - 1]->Eps_z) * m_Dt + m_Dz_Grid[k - 1];

					Ey[i][j][k] = m_Ey_Z_Mur[i][j][2] + A / B * (Ey[i][j][k - 1] - m_Ey_Z_Mur[i][j][3]);

					m_Ey_Z_Mur[i][j][3] = Ey[i][j][k];
					m_Ey_Z_Mur[i][j][2] = Ey[i][j][k - 1];
				}
			}
			break ;
		case YMIN :
		case YMAX :
			break;
	}
}

void CBoundary::z_Mur_Update(int n, int boundary_Index, float*** Ez)
{
	float A, B ;
	int i, j, k;

	switch( boundary_Index )
	{
		case XMIN : 
			i = m_Index_E_Boundary[XMIN]; 

			for (j = m_Index_E_Boundary[YMIN]; j <= m_Index_E_Boundary[YMAX]; j++) 
			{
				for (k = m_Index_E_Boundary[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - 1 ; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_x) * m_Dt - m_Dx_Grid[i];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_x) * m_Dt + m_Dx_Grid[i];
				
					Ez[i][j][k] = m_Ez_X_Mur[1][j][k] + A / B * (Ez[i + 1][j][k] - m_Ez_X_Mur[0][j][k]);

					m_Ez_X_Mur[0][j][k] = Ez[i][j][k];
					m_Ez_X_Mur[1][j][k] = Ez[i + 1][j][k];
				}
			}
			break ;
		case XMAX : 
			i = m_Index_E_Boundary[XMAX];
			for (j = m_Index_E_Boundary[YMIN]; j <= m_Index_E_Boundary[YMAX]; j++) 
			{
				for (k = m_Index_E_Boundary[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - 1 ; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i - 1][j][k]->Eps_x) * m_Dt - m_Dx_Grid[i - 1];
					B = C_Speed * sqrt(1.0f / m_eMatType[i - 1][j][k]->Eps_x) * m_Dt + m_Dx_Grid[i - 1];

					Ez[i][j][k] = m_Ez_X_Mur[2][j][k] + A / B * (Ez[i - 1][j][k] - m_Ez_X_Mur[3][j][k]);

					m_Ez_X_Mur[3][j][k] = Ez[i][j][k];
					m_Ez_X_Mur[2][j][k] = Ez[i - 1][j][k];

				}
			}
			break ;
		case YMIN : 
			j = m_Index_E_Boundary[YMIN]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX]; i++) 
			{
				for (k = m_Index_E_Boundary[ZMIN] ; k <= m_Index_E_Boundary[ZMAX] - 1 ; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_y) * m_Dt - m_Dy_Grid[j];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j][k]->Eps_y) * m_Dt + m_Dy_Grid[j];

					Ez[i][j][k] = m_Ez_Y_Mur[i][1][k] + A / B * (Ez[i][j + 1][k] - m_Ez_Y_Mur[i][0][k]);

					m_Ez_Y_Mur[i][0][k] = Ez[i][j][k];
					m_Ez_Y_Mur[i][1][k] = Ez[i][j + 1][k];

				}
			}
			break ;
		case YMAX : 
			j = m_Index_E_Boundary[YMAX]; 
			for (i = m_Index_E_Boundary[XMIN]; i <= m_Index_E_Boundary[XMAX]; i++) 
			{
				for (k = m_Index_E_Boundary[ZMIN] ; k <= m_Index_E_Boundary[ZMAX] - 1 ; k++) 
				{
					A = C_Speed * sqrt(1.0f / m_eMatType[i][j - 1][k]->Eps_y) * m_Dt - m_Dy_Grid[j - 1];
					B = C_Speed * sqrt(1.0f / m_eMatType[i][j - 1][k]->Eps_y) * m_Dt + m_Dy_Grid[j - 1];

					Ez[i][j][k] = m_Ez_Y_Mur[i][2][k] + A / B * (Ez[i][j - 1][k] - m_Ez_Y_Mur[i][3][k]);

					m_Ez_Y_Mur[i][3][k] = Ez[i][j][k];
					m_Ez_Y_Mur[i][2][k] = Ez[i][j - 1][k];
				}
			}
			break ;
		case ZMIN :
		case ZMAX :
			break;
	}
}


void CBoundary::x_PMC_Update(int n, int boundary_Index, float*** Hx)
{
	int i , j , k ;

	switch( boundary_Index )
	{
		case YMIN : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] ; k++ )
					Hx[i][m_Index_H_Boundary[YMIN] - 1][k] = - Hx[i][m_Index_H_Boundary[YMIN]][k] ;
			}
			break ;
		case YMAX : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] ; k++ )
					Hx[i][m_Index_H_Boundary[YMAX] + 1][k] = - Hx[i][m_Index_H_Boundary[YMAX]][k] ;
			}
			break ;
		case ZMIN : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++ )
			{
				for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++ )
					Hx[i][j][m_Index_H_Boundary[ZMIN] - 1] = - Hx[i][j][m_Index_H_Boundary[ZMIN]] ;
			}
			break ;
		case ZMAX : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++ )
			{
				for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++ )
					Hx[i][j][m_Index_H_Boundary[ZMAX] + 1] = - Hx[i][j][m_Index_H_Boundary[ZMAX]] ;
			}
			break ;
		case XMIN :
		case XMAX :
			break;
	}
}

void CBoundary::y_PMC_Update(int n, int boundary_Index, float*** Hy)
{
	int i , j , k ;

	switch( boundary_Index )
	{
		case XMIN : 
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] + 1 ; j++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] ; k++ )
					Hy[m_Index_H_Boundary[XMIN] - 1][j][k] = - Hy[m_Index_H_Boundary[XMIN]][j][k] ;
			}
			break ;
		case XMAX : 
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] + 1 ; j++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] ; k++ )
					Hy[m_Index_H_Boundary[XMAX] + 1][j][k] = - Hy[m_Index_H_Boundary[XMAX]][j][k] ;
			}
			break ;
		case ZMIN : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++ )
			{
				for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] + 1 ; j++ )
					Hy[i][j][m_Index_H_Boundary[ZMIN] - 1] = - Hy[i][j][m_Index_H_Boundary[ZMIN]] ;
			}
			break ;
		case ZMAX : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++ )
			{
				for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] + 1 ; j++ )
					Hy[i][j][m_Index_H_Boundary[ZMAX] + 1] = - Hy[i][j][m_Index_H_Boundary[ZMAX]] ;
			}
			break ;
		case YMIN :
		case YMAX :
			break;
	}
}

void CBoundary::z_PMC_Update(int n, int boundary_Index, float*** Hz)
{
	int i , j , k ;

	switch( boundary_Index )
	{
		case XMIN : 
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] + 1 ; k++ )
					Hz[m_Index_H_Boundary[XMIN] - 1][j][k] = - Hz[m_Index_H_Boundary[XMIN]][j][k] ;
			}
			break ;
		case XMAX : 
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] + 1 ; k++ )
					Hz[m_Index_H_Boundary[XMAX] + 1][j][k] = - Hz[m_Index_H_Boundary[XMAX]][j][k] ;
			}
			break ;
		case YMIN : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] + 1 ; k++ )
					Hz[i][m_Index_H_Boundary[YMIN] - 1][k] = - Hz[i][m_Index_H_Boundary[YMIN]][k] ;
			}
			break ;
		case YMAX : 
			for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++ )
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] + 1 ; k++ )
					Hz[i][m_Index_H_Boundary[YMAX] + 1][k] = - Hz[i][m_Index_H_Boundary[YMAX]][k] ;
			}
			break ;
		case ZMIN :
		case ZMAX :
			break;
	}
}

void CBoundary::setDt(float Dt)
{
	m_Dt = Dt ;
}

void CBoundary::setDGrid(float* Dx_Grid, float* Dy_Guid, float* Dz_Guid)
{
	m_Dx_Grid = Dx_Grid ;
	m_Dy_Grid = Dy_Guid ;
	m_Dz_Grid = Dz_Guid ;
}




void CBoundary::getBoundaryIndex(int* index_E_Boundary, int* index_H_Boundary)
{
	memcpy( index_E_Boundary, m_Index_E_Boundary, 6 * sizeof( int ) ) ;
	memcpy( index_H_Boundary, m_Index_H_Boundary, 6 * sizeof( int ) ) ;
}


void CBoundary::getBoundary( Boundary_Condition* boundary )
{
	memcpy( boundary, m_boundary, 6 * sizeof( Boundary_Condition ) ) ;
}

void CBoundary::getBoundary_Global( Boundary_Condition* boundary )
{
	memcpy( boundary, m_boundary_Global, 6 * sizeof( Boundary_Condition ) ) ;

}

// set global cell number 
void CBoundary::setCellNumberGlobal( int nx_Global, int ny_Global, int nz_Global ) 
{
	m_nx_Global = nx_Global ;
	m_ny_Global = ny_Global ;
	m_nz_Global = nz_Global ; 
}



void CBoundary::getBoundaryLayerNum( int boundaryLayerNum[6], int boundaryLayerNum_Global[6])
{
	int i ;
	for( i = 0 ; i < 6 ; i++ )
	{
		boundaryLayerNum[i] = m_boundaryLayerNum[i] ;
		boundaryLayerNum_Global[i] = m_boundaryLayerNum_Global[i] ;
	}
}
