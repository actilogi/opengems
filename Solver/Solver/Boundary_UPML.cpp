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
#include "GEMS_Constant.h"


void CBoundary::UPML_Hfield_Lossless_Update(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	int i,j,k ;
	int ii , jj , kk ;
	float tep2 , tep1 ;
	//int iMax, jMax, kMax ;

//	-----------------------------------------------------------------------
//	HX Component 
//	-----------------------------------------------------------------------

	if( m_boundary[XMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ;
		//jMax = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i < m_Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{
					tep1 = m_Phx_Xmin[i][j][k] ;
					tep2 = m_Dhx_Xmin[i][j][k] ; 

					m_Phx_Xmin[i][j][k] += m_pHk_Coeff_M_Lossless[k] * ( Ey[i][j][k+1] - Ey[i][j][k])
												 - m_pHj_Coeff_M_Lossless[j] * (Ez[i][j+1][k] - Ez[i][j][k]);
					
					Hx[i][j][k] *= m_coeff_zz[k] ;

					m_Dhx_Xmin[i][j][k] = m_coeff_yy[j] * tep2 + ( m_Phx_Xmin[i][j][k] - tep1 ) * m_coeff_yyp_inv[j] ; 


					Hx[i][j][k] += ( m_coeff_xyzp[i] * m_Dhx_Xmin[i][j][k] - m_coeff_xyzm[i] * tep2 ) * m_coeff_zzp_inv[k] ;  

				}
			}
		}
	}

	if( m_boundary[XMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] + 1 ;
		//jMax = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = ( m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 2 ) ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++)
		{
			ii = i - ( m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 2 );
			for( j = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{

					tep1 = m_Phx_Xmax[ii][j][k] ;
					tep2 = m_Dhx_Xmax[ii][j][k] ; 

					m_Phx_Xmax[ii][j][k] += m_pHk_Coeff_M_Lossless[k] * (Ey[i][j][k+1] - Ey[i][j][k])
												 - m_pHj_Coeff_M_Lossless[j] * (Ez[i][j+1][k] - Ez[i][j][k]);

					Hx[i][j][k] *= m_coeff_zz[k] ;


					m_Dhx_Xmax[ii][j][k] = m_coeff_yy[j] * tep2 + ( m_Phx_Xmax[ii][j][k] - tep1 ) * m_coeff_yyp_inv[j] ; 

					Hx[i][j][k] += ( m_coeff_xyzp[i] * m_Dhx_Xmax[ii][j][k] - m_coeff_xyzm[i] * tep2 ) * m_coeff_zzp_inv[k] ;  

				}
			}
		}
	}


	if( m_boundary[YMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] + 1 ;
		//jMax = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN]; j < m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{
					tep1 = m_Phx_Ymin[i][j][k] ;
					tep2 = m_Dhx_Ymin[i][j][k] ;

					m_Phx_Ymin[i][j][k] += m_pHk_Coeff_M_Lossless[k] * (Ey[i][j][k+1] - Ey[i][j][k])
												 - m_pHj_Coeff_M_Lossless[j] * (Ez[i][j+1][k] - Ez[i][j][k]);


					Hx[i][j][k] *= m_coeff_zz[k] ;

					m_Dhx_Ymin[i][j][k] = m_coeff_yy[j] * tep2 + ( m_Phx_Ymin[i][j][k] - tep1 ) * m_coeff_yyp_inv[j] ; 

					Hx[i][j][k] += (m_coeff_xyzp[i] * m_Dhx_Ymin[i][j][k] - m_coeff_xyzm[i] * tep2) * m_coeff_zzp_inv[k] ;  
 
				}
			}
		}
	}

	if( m_boundary[YMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] + 1 ;
		//jMax = m_Index_H_Boundary[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++)
		{
			for( j = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ; j <= m_Index_H_Boundary[YMAX] ; j++)
			{
				jj = j - ( m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 );
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{

					tep1 = m_Phx_Ymax[i][jj][k] ;

					tep2 = m_Dhx_Ymax[i][jj][k] ;

					m_Phx_Ymax[i][jj][k] += m_pHk_Coeff_M_Lossless[k] * (Ey[i][j][k+1] - Ey[i][j][k])
												 - m_pHj_Coeff_M_Lossless[j] * (Ez[i][j+1][k] - Ez[i][j][k]);

					Hx[i][j][k] *= m_coeff_zz[k] ;

					m_Dhx_Ymax[i][jj][k] = m_coeff_yy[j] * tep2 + ( m_Phx_Ymax[i][jj][k] - tep1 ) * m_coeff_yyp_inv[j] ; 

					Hx[i][j][k] += (m_coeff_xyzp[i] * m_Dhx_Ymax[i][jj][k] - m_coeff_xyzm[i] * tep2) * m_coeff_zzp_inv[k] ;  
 
				}
			}
		}
	}

	if( m_boundary[ZMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] + 1 ;
		//jMax = m_Index_H_Boundary[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k < m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k++)
				{
					tep1 = m_Phx_Zmin[i][j][k] ;

					tep2 = m_Dhx_Zmin[i][j][k] ;

					m_Phx_Zmin[i][j][k] += m_pHk_Coeff_M_Lossless[k] * (Ey[i][j][k+1] - Ey[i][j][k])
												 - m_pHj_Coeff_M_Lossless[j] * (Ez[i][j+1][k] - Ez[i][j][k]);
					
					Hx[i][j][k] *= m_coeff_zz[k] ;

					m_Dhx_Zmin[i][j][k] = m_coeff_yy[j] * tep2 + (m_Phx_Zmin[i][j][k] - tep1 ) * m_coeff_yyp_inv[j] ; 

					Hx[i][j][k] += ( m_coeff_xyzp[i] * m_Dhx_Zmin[i][j][k] - m_coeff_xyzm[i] * tep2) * m_coeff_zzp_inv[k] ;  

				}
			}
		}
	}

	if( m_boundary[ZMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] + 1 ;
		//jMax = m_Index_H_Boundary[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] + 1 ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k <= m_Index_H_Boundary[ZMAX] ; k++)
				{
					kk = k - ( m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 );

					tep1 = m_Phx_Zmax[i][j][kk] ;
					tep2 = m_Dhx_Zmax[i][j][kk] ;


					m_Phx_Zmax[i][j][kk] += m_pHk_Coeff_M_Lossless[k] * (Ey[i][j][k+1] - Ey[i][j][k])
												 - m_pHj_Coeff_M_Lossless[j] * (Ez[i][j+1][k] - Ez[i][j][k]);

					Hx[i][j][k] *= m_coeff_zz[k] ;

					m_Dhx_Zmax[i][j][kk] = m_coeff_yy[j] * tep2 + ( m_Phx_Zmax[i][j][kk] - tep1 ) * m_coeff_yyp_inv[j] ; 

					Hx[i][j][k] += ( m_coeff_xyzp[i] * m_Dhx_Zmax[i][j][kk] - m_coeff_xyzm[i] * tep2) * m_coeff_zzp_inv[k] ;  

				}
			}
		}
	}

//	-----------------------------------------------------------------------
//	HY Component 
//	-----------------------------------------------------------------------

	if( m_boundary[XMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ;
		//jMax = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i < m_Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{
					tep1 = m_Phy_Xmin[i][j][k] ;
					tep2 = m_Dhy_Xmin[i][j][k] ;



					m_Phy_Xmin[i][j][k] += m_pHi_Coeff_M_Lossless[i] * (Ez[i+1][j][k] - Ez[i][j][k])
							 - m_pHk_Coeff_M_Lossless[k] * (Ex[i][j][k+1] - Ex[i][j][k]);

					Hy[i][j][k] *= m_coeff_xx[i] ;
					m_Dhy_Xmin[i][j][k] = m_coeff_zz[k] * tep2 + (m_Phy_Xmin[i][j][k] - tep1 ) * m_coeff_zzp_inv[k] ; 

					Hy[i][j][k] += ( m_coeff_yxzp[j] * m_Dhy_Xmin[i][j][k] - m_coeff_yxzm[j] * tep2) * m_coeff_xxp_inv[i] ;   
				}
			}
		}
	}

	if( m_boundary[XMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1 ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			ii = i - ( m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1 );
			for( j = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{

					tep1 = m_Phy_Xmax[ii][j][k] ;

					tep2 = m_Dhy_Xmax[ii][j][k] ; 


					m_Phy_Xmax[ii][j][k] += m_pHi_Coeff_M_Lossless[i] * (Ez[i+1][j][k] - Ez[i][j][k])
							 - m_pHk_Coeff_M_Lossless[k] * (Ex[i][j][k+1] - Ex[i][j][k]);

					Hy[i][j][k] *= m_coeff_xx[i] ;
					m_Dhy_Xmax[ii][j][k] = m_coeff_zz[k] * tep2 + ( m_Phy_Xmax[ii][j][k] - tep1 ) * m_coeff_zzp_inv[k] ; 

					Hy[i][j][k] += ( m_coeff_yxzp[j] * m_Dhy_Xmax[ii][j][k] - m_coeff_yxzm[j] * tep2) * m_coeff_xxp_inv[i] ;   

				}
			}
		}
	}

	if( m_boundary[YMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j < m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{
					tep1 = m_Phy_Ymin[i][j][k] ;
					tep2 = m_Dhy_Ymin[i][j][k] ; 



					m_Phy_Ymin[i][j][k] +=  m_pHi_Coeff_M_Lossless[i] * (Ez[i+1][j][k] - Ez[i][j][k])
							 - m_pHk_Coeff_M_Lossless[k] * (Ex[i][j][k+1] - Ex[i][j][k]);

					Hy[i][j][k] *= m_coeff_xx[i] ;

					m_Dhy_Ymin[i][j][k] = m_coeff_zz[k] * tep2 + (m_Phy_Ymin[i][j][k] - tep1 ) * m_coeff_zzp_inv[k] ; 

					Hy[i][j][k] += ( m_coeff_yxzp[j] * m_Dhy_Ymin[i][j][k] - m_coeff_yxzm[j] * tep2) * m_coeff_xxp_inv[i] ;   
				}
			}
		}
	}

	if( m_boundary[YMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] + 1 ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 2 ; j <= m_Index_H_Boundary[YMAX] + 1 ; j++)
			{
				jj = j - ( m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 2 );
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{

					tep1 = m_Phy_Ymax[i][jj][k] ;

					tep2 = m_Dhy_Ymax[i][jj][k] ; 

					m_Phy_Ymax[i][jj][k] += m_pHi_Coeff_M_Lossless[i] * (Ez[i+1][j][k] - Ez[i][j][k])
							 - m_pHk_Coeff_M_Lossless[k] * (Ex[i][j][k+1] - Ex[i][j][k]);
						

					Hy[i][j][k] *= m_coeff_xx[i] ;
					m_Dhy_Ymax[i][jj][k] = m_coeff_zz[k] * tep2 + ( m_Phy_Ymax[i][jj][k] - tep1 ) * m_coeff_zzp_inv[k] ; 

					Hy[i][j][k] += ( m_coeff_yxzp[j] * m_Dhy_Ymax[i][jj][k] - m_coeff_yxzm[j] * tep2 ) * m_coeff_xxp_inv[i] ;   

				}
			}
		}
	}

	if( m_boundary[ZMIN] == BC_UPML)
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] + 1 ;
		//kMax = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] + 1 ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN]; k < m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k++)
				{
					tep1 = m_Phy_Zmin[i][j][k] ;
					tep2 = m_Dhy_Zmin[i][j][k] ; 


					m_Phy_Zmin[i][j][k] +=  m_pHi_Coeff_M_Lossless[i] * (Ez[i+1][j][k] - Ez[i][j][k])
							 - m_pHk_Coeff_M_Lossless[k] * (Ex[i][j][k+1] - Ex[i][j][k]);


					Hy[i][j][k] *= m_coeff_xx[i] ;
					m_Dhy_Zmin[i][j][k] = m_coeff_zz[k] * tep2 + (m_Phy_Zmin[i][j][k] - tep1 ) * m_coeff_zzp_inv[k] ; 

					Hy[i][j][k] += ( m_coeff_yxzp[j] * m_Dhy_Zmin[i][j][k] - m_coeff_yxzm[j] * tep2) * m_coeff_xxp_inv[i] ;   

				}
			}
		}
	}

	if( m_boundary[ZMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] + 1 ;
		//kMax = m_Index_H_Boundary[ZMAX] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] + 1 ; j++)
			{
				for( k = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k <= m_Index_H_Boundary[ZMAX] ; k++)
				{
					kk = k - (m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]+1);
						
					tep1 = m_Phy_Zmax[i][j][kk] ;
					tep2 = m_Dhy_Zmax[i][j][kk] ;


					m_Phy_Zmax[i][j][kk] +=  m_pHi_Coeff_M_Lossless[i] * (Ez[i+1][j][k] - Ez[i][j][k])
							 - m_pHk_Coeff_M_Lossless[k] * (Ex[i][j][k+1] - Ex[i][j][k]);


					Hy[i][j][k] *= m_coeff_xx[i] ;
					m_Dhy_Zmax[i][j][kk] = m_coeff_zz[k] * tep2 + ( m_Phy_Zmax[i][j][kk] - tep1 ) * m_coeff_zzp_inv[k] ; 

					Hy[i][j][k] += ( m_coeff_yxzp[j] * m_Dhy_Zmax[i][j][kk] - m_coeff_yxzm[j] * tep2) * m_coeff_xxp_inv[i] ;   

				}
			}
		}
	}

//	-----------------------------------------------------------------------
//	HZ Component 
//	-----------------------------------------------------------------------

	if( m_boundary[XMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ;
		//jMax = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ;

		for( i = m_Index_H_Boundary[XMIN] ; i < m_Index_H_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k++)
				{
					tep1 = m_Phz_Xmin[i][j][k] ;
					tep2 = m_Dhz_Xmin[i][j][k] ; 


					m_Phz_Xmin[i][j][k] +=  m_pHj_Coeff_M_Lossless[j] * (Ex[i][j+1][k] - Ex[i][j][k])
						     - m_pHi_Coeff_M_Lossless[i] * (Ey[i+1][j][k] - Ey[i][j][k]);

					Hz[i][j][k] *= m_coeff_yy[j] ;

					m_Dhz_Xmin[i][j][k] = m_coeff_xx[i] * tep2 + ( m_Phz_Xmin[i][j][k] - tep1 ) * m_coeff_xxp_inv[i] ;

					Hz[i][j][k] += ( m_coeff_zxyp[k] * m_Dhz_Xmin[i][j][k] - m_coeff_zxym[k] * tep2) * m_coeff_yyp_inv[j] ;  

				}
			}
		}
	}

	if( m_boundary[XMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ;

		for( i = m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			ii = i - (m_Index_H_Boundary[XMAX] - m_boundaryLayerNum[XMAX]+1);
			for( j = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k++)
				{

					tep1 = m_Phz_Xmax[ii][j][k] ;
					tep2 = m_Dhz_Xmax[ii][j][k] ; 


					m_Phz_Xmax[ii][j][k] +=  m_pHj_Coeff_M_Lossless[j] * (Ex[i][j+1][k] - Ex[i][j][k])
						     - m_pHi_Coeff_M_Lossless[i] * (Ey[i+1][j][k] - Ey[i][j][k]);

					Hz[i][j][k] *= m_coeff_yy[j] ;

					m_Dhz_Xmax[ii][j][k] = m_coeff_xx[i] * tep2 + ( m_Phz_Xmax[ii][j][k] - tep1 ) * m_coeff_xxp_inv[i] ; 

					Hz[i][j][k] += ( m_coeff_zxyp[k] * m_Dhz_Xmax[ii][j][k] - m_coeff_zxym[k] * tep2) * m_coeff_yyp_inv[j] ;  

				}
			}
		}
	}

	if( m_boundary[YMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j < m_Index_H_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k++)
				{
					tep1 = m_Phz_Ymin[i][j][k] ;
					tep2 = m_Dhz_Ymin[i][j][k] ; 


					m_Phz_Ymin[i][j][k] +=  m_pHj_Coeff_M_Lossless[j] * (Ex[i][j+1][k] - Ex[i][j][k])
						     - m_pHi_Coeff_M_Lossless[i] * (Ey[i+1][j][k] - Ey[i][j][k]);

					Hz[i][j][k] *= m_coeff_yy[j] ;

					m_Dhz_Ymin[i][j][k] = m_coeff_xx[i] * tep2 + ( m_Phz_Ymin[i][j][k] - tep1 ) * m_coeff_xxp_inv[i] ; 

					Hz[i][j][k] += ( m_coeff_zxyp[k] * m_Dhz_Ymin[i][j][k] - m_coeff_zxym[k] * tep2 ) * m_coeff_yyp_inv[j] ;  

				}
			}
		}
	}

	if( m_boundary[YMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ; j <= m_Index_H_Boundary[YMAX] ; j++)
			{
				jj = j - (m_Index_H_Boundary[YMAX] - m_boundaryLayerNum[YMAX]+1);
				for( k = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k++)
				{

					tep1 = m_Phz_Ymax[i][jj][k] ;
					tep2 = m_Dhz_Ymax[i][jj][k] ; 


					m_Phz_Ymax[i][jj][k] +=  m_pHj_Coeff_M_Lossless[j] * (Ex[i][j+1][k] - Ex[i][j][k])
						     - m_pHi_Coeff_M_Lossless[i] * (Ey[i+1][j][k] - Ey[i][j][k]);

					Hz[i][j][k] *= m_coeff_yy[j] ;

					m_Dhz_Ymax[i][jj][k] = m_coeff_xx[i] * tep2 + ( m_Phz_Ymax[i][jj][k] - tep1 ) * m_coeff_xxp_inv[i] ; 

					Hz[i][j][k] += ( m_coeff_zxyp[k] * m_Dhz_Ymax[i][jj][k] - m_coeff_zxym[k] * tep2 ) * m_coeff_yyp_inv[j] ;  

				}
			}
		}
	}

	if( m_boundary[ZMIN] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMIN] ; k < m_Index_H_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k++)
				{
					tep1 = m_Phz_Zmin[i][j][k] ;
					tep2 = m_Dhz_Zmin[i][j][k] ; 


					m_Phz_Zmin[i][j][k] +=  m_pHj_Coeff_M_Lossless[j] * (Ex[i][j+1][k] - Ex[i][j][k])
						     - m_pHi_Coeff_M_Lossless[i] * (Ey[i+1][j][k] - Ey[i][j][k]);

					Hz[i][j][k] *= m_coeff_yy[j] ;

					m_Dhz_Zmin[i][j][k] = m_coeff_xx[i] * tep2 + ( m_Phz_Zmin[i][j][k] - tep1 ) * m_coeff_xxp_inv[i] ; 

					Hz[i][j][k] += ( m_coeff_zxyp[k] * m_Dhz_Zmin[i][j][k] - m_coeff_zxym[k] * tep2) * m_coeff_yyp_inv[j] ;  

				}
			}
		}
	}

	if( m_boundary[ZMAX] == BC_UPML) 
	{
		//iMax = m_Index_H_Boundary[XMAX] ;
		//jMax = m_Index_H_Boundary[YMAX] ;
		//kMax = m_Index_H_Boundary[ZMAX] + 1 ;

		for( i = m_Index_H_Boundary[XMIN] ; i <= m_Index_H_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_H_Boundary[YMIN] ; j <= m_Index_H_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 2 ; k <= m_Index_H_Boundary[ZMAX] + 1 ; k++)
				{
					kk = k - ( m_Index_H_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 2 );

					tep1 = m_Phz_Zmax[i][j][kk] ;
					tep2 = m_Dhz_Zmax[i][j][kk] ; 
					

					m_Phz_Zmax[i][j][kk] +=  m_pHj_Coeff_M_Lossless[j] * (Ex[i][j+1][k] - Ex[i][j][k])
						     - m_pHi_Coeff_M_Lossless[i] * (Ey[i+1][j][k] - Ey[i][j][k]);

					Hz[i][j][k] *= m_coeff_yy[j] ;

					m_Dhz_Zmax[i][j][kk] = m_coeff_xx[i] * tep2 + ( m_Phz_Zmax[i][j][kk] - tep1 ) * m_coeff_xxp_inv[i] ;

					Hz[i][j][k] += ( m_coeff_zxyp[k] * m_Dhz_Zmax[i][j][kk] - m_coeff_zxym[k] * tep2) * m_coeff_yyp_inv[j] ;  

				}
			}
		}
	}

}

void CBoundary::UPML_Efield_Update(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) )
{
	int i, j, k ;
	int ii , jj , kk ;
	float tep1, tep2 ;


//	-----------------------------------------------------------------------
//	EX Component 
//	-----------------------------------------------------------------------
	if( m_boundary[XMIN] == BC_UPML ) 
	{
		for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j <= m_Index_E_Boundary[YMAX]- m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k <= m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]; k++)
				{
					tep2 = m_Pex_Xmin[i][j][k] ;

					tep1 = m_Dex_Xmin[i][j][k] ;

					m_Pex_Xmin[i][j][k] = m_eMatType[i][j][k]->AEx_Coeff * tep2
							 + m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEx_p_Inv * (Hz[i][j][k] - Hz[i][j-1][k])
							 - m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEx_p_Inv * (Hy[i][j][k] - Hy[i][j][k-1]);

					Ex[i][j][k] *= m_coeff_yxz[j] ;

					m_Dex_Xmin[i][j][k] = m_coeff_zxy[k] * tep1 + ( m_Pex_Xmin[i][j][k] - tep2 ) * m_coeff_zxyp_inv[k] ;

					Ex[i][j][k] += ( m_coeff_xxp[i] * m_Dex_Xmin[i][j][k] - m_coeff_xxm[i] * tep1) * m_coeff_yxzp_inv[j] ;  

				}
			}
		}
	}

	if( m_boundary[XMAX] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] ; i < m_Index_E_Boundary[XMAX] ; i++)
		{
			ii = i - (m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX]);
			for( j = m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j <= m_Index_E_Boundary[YMAX] -m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]; k++)
				{

					tep2 = m_Pex_Xmax[ii][j][k] ;

					tep1 = m_Dex_Xmax[ii][j][k] ;

					m_Pex_Xmax[ii][j][k] = m_eMatType[i][j][k]->AEx_Coeff * tep2 
							 + m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEx_p_Inv * (Hz[i][j][k] - Hz[i][j-1][k])
							 - m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEx_p_Inv * (Hy[i][j][k] - Hy[i][j][k-1]);

					Ex[i][j][k] *= m_coeff_yxz[j] ;

					m_Dex_Xmax[ii][j][k] = m_coeff_zxy[k] * tep1 + ( m_Pex_Xmax[ii][j][k] - tep2 ) * m_coeff_zxyp_inv[k] ;

					Ex[i][j][k] += ( m_coeff_xxp[i] * m_Dex_Xmax[ii][j][k] - m_coeff_xxm[i] * tep1) * m_coeff_yxzp_inv[j] ;  
					
				}
			}
		}
	}

	if( m_boundary[YMIN] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]  ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k ++)
				{
					tep2 = m_Pex_Ymin[i][j][k] ;

					tep1 = m_Dex_Ymin[i][j][k] ;

					m_Pex_Ymin[i][j][k] = m_eMatType[i][j][k]->AEx_Coeff * tep2 
							 + m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEx_p_Inv * (Hz[i][j][k] - Hz[i][j-1][k])
							 - m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEx_p_Inv * (Hy[i][j][k] - Hy[i][j][k-1]);

					Ex[i][j][k] *= m_coeff_yxz[j] ;

					m_Dex_Ymin[i][j][k] = m_coeff_zxy[k] * tep1 + ( m_Pex_Ymin[i][j][k] - tep2 ) * m_coeff_zxyp_inv[k] ;

					Ex[i][j][k] += ( m_coeff_xxp[i] * m_Dex_Ymin[i][j][k] - m_coeff_xxm[i] * tep1) * m_coeff_yxzp_inv[j] ;  
					
				}
			}
		}

	}

	if( m_boundary[YMAX] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ; j <= m_Index_E_Boundary[YMAX] ; j++)
			{
				jj = j - (m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX]+1);
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k <= m_Index_E_Boundary[ZMAX] -m_boundaryLayerNum[ZMAX] ; k ++)
				{

					tep2 = m_Pex_Ymax[i][jj][k] ;

					tep1 = m_Dex_Ymax[i][jj][k] ;

					m_Pex_Ymax[i][jj][k] = m_eMatType[i][j][k]->AEx_Coeff * tep2 
							 + m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEx_p_Inv * (Hz[i][j][k] - Hz[i][j-1][k])
							 - m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEx_p_Inv * (Hy[i][j][k] - Hy[i][j][k-1]);

					Ex[i][j][k] *= m_coeff_yxz[j] ;

					m_Dex_Ymax[i][jj][k] = m_coeff_zxy[k] * tep1 + ( m_Pex_Ymax[i][jj][k] - tep2 ) * m_coeff_zxyp_inv[k] ;

					Ex[i][j][k] += ( m_coeff_xxp[i] * m_Dex_Ymax[i][jj][k] - m_coeff_xxm[i] * tep1) * m_coeff_yxzp_inv[j] ;  
					
				}
			}
		}
	}


	if( m_boundary[ZMIN] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k < m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k ++)
				{
					tep2 = m_Pex_Zmin[i][j][k] ;
					
					tep1 = m_Dex_Zmin[i][j][k] ;

					m_Pex_Zmin[i][j][k] = m_eMatType[i][j][k]->AEx_Coeff * tep2 
							 + m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEx_p_Inv * (Hz[i][j][k] - Hz[i][j-1][k])
							 - m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEx_p_Inv * (Hy[i][j][k] - Hy[i][j][k-1]);

					Ex[i][j][k] *= m_coeff_yxz[j] ;

					m_Dex_Zmin[i][j][k] = m_coeff_zxy[k] * tep1 + ( m_Pex_Zmin[i][j][k] - tep2 ) * m_coeff_zxyp_inv[k] ;

					Ex[i][j][k] += ( m_coeff_xxp[i] * m_Dex_Zmin[i][j][k] - m_coeff_xxm[i] * tep1) * m_coeff_yxzp_inv[j] ;  
					
				}
			}
		}
	}

	if( m_boundary[ZMAX] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k <= m_Index_E_Boundary[ZMAX] ; k++)
				{
					kk = k - ( m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 );
 
					tep2 = m_Pex_Zmax[i][j][kk] ;

					tep1 = m_Dex_Zmax[i][j][kk] ;

					m_Pex_Zmax[i][j][kk] = m_eMatType[i][j][k]->AEx_Coeff * tep2 
							 + m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEx_p_Inv * (Hz[i][j][k] - Hz[i][j-1][k])
							 - m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEx_p_Inv * (Hy[i][j][k] - Hy[i][j][k-1]);

					Ex[i][j][k] *= m_coeff_yxz[j] ;

					m_Dex_Zmax[i][j][kk] = m_coeff_zxy[k] * tep1 + ( m_Pex_Zmax[i][j][kk] - tep2 ) * m_coeff_zxyp_inv[k] ;

					Ex[i][j][k] += ( m_coeff_xxp[i] * m_Dex_Zmax[i][j][kk] - m_coeff_xxm[i] * tep1) * m_coeff_yxzp_inv[j] ;  
					
				}
			}
		}
	}

//	-----------------------------------------------------------------------
//	EY Component 
//	-----------------------------------------------------------------------

	if( m_boundary[XMIN] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j < m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]; k++)
				{
					tep2 = m_Pey_Xmin[i][j][k] ;

					tep1 = m_Dey_Xmin[i][j][k] ;

					m_Pey_Xmin[i][j][k] = m_eMatType[i][j][k]->AEy_Coeff * tep2 
							 + m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEy_p_Inv * (Hx[i][j][k] - Hx[i][j][k-1])
						     - m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEy_p_Inv * (Hz[i][j][k] - Hz[i-1][j][k]);

					Ey[i][j][k] *= m_coeff_zxy[k] ;
		
					m_Dey_Xmin[i][j][k] = m_coeff_xyz[i] * tep1 + ( m_Pey_Xmin[i][j][k] - tep2 ) * m_coeff_xyzp_inv[i] ;
		
					Ey[i][j][k] += ( m_coeff_yyp[j] * m_Dey_Xmin[i][j][k] - m_coeff_yym[j] * tep1) * m_coeff_zxyp_inv[k] ;
					

				}
			}
		}
	}
		 
	if( m_boundary[XMAX] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1 ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			ii = i - ( m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1 );
			for( j = m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j < m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX]; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{
					
					tep2 = m_Pey_Xmax[ii][j][k] ;

					tep1 = m_Dey_Xmax[ii][j][k] ;

					m_Pey_Xmax[ii][j][k] = m_eMatType[i][j][k]->AEy_Coeff * tep2
							 + m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEy_p_Inv * (Hx[i][j][k] - Hx[i][j][k-1])
						     - m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEy_p_Inv * (Hz[i][j][k] - Hz[i-1][j][k]);

					Ey[i][j][k] *=  m_coeff_zxy[k] ;
	
					m_Dey_Xmax[ii][j][k] = m_coeff_xyz[i] * tep1 + ( m_Pey_Xmax[ii][j][k] - tep2 ) * m_coeff_xyzp_inv[i] ;
		
					Ey[i][j][k] += ( m_coeff_yyp[j] * m_Dey_Xmax[ii][j][k] - m_coeff_yym[j] * tep1) * m_coeff_zxyp_inv[k] ;
					

				}
			}
		}
	}

	if( m_boundary[YMIN] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]; k ++)
				{
					tep2 = m_Pey_Ymin[i][j][k] ;

					tep1 = m_Dey_Ymin[i][j][k] ;

					m_Pey_Ymin[i][j][k] = m_eMatType[i][j][k]->AEy_Coeff * tep2
							 + m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEy_p_Inv * (Hx[i][j][k] - Hx[i][j][k-1])
						     - m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEy_p_Inv * (Hz[i][j][k] - Hz[i-1][j][k]);

					Ey[i][j][k] *= m_coeff_zxy[k] ;
					
					m_Dey_Ymin[i][j][k] = m_coeff_xyz[i] * tep1 + ( m_Pey_Ymin[i][j][k] - tep2 ) * m_coeff_xyzp_inv[i] ;
		
					Ey[i][j][k] += ( m_coeff_yyp[j] * m_Dey_Ymin[i][j][k] - m_coeff_yym[j] * tep1) * m_coeff_zxyp_inv[k] ;
					

				}
			}
		}
	}

	if( m_boundary[YMAX] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j < m_Index_E_Boundary[YMAX] ; j++)
			{
				jj = j - ( m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX]);
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN]; k <= m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]; k ++)
				{
					

					tep2 = m_Pey_Ymax[i][jj][k] ;

					tep1 = m_Dey_Ymax[i][jj][k] ;

					m_Pey_Ymax[i][jj][k] = m_eMatType[i][j][k]->AEy_Coeff * tep2 
							 + m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEy_p_Inv * (Hx[i][j][k] - Hx[i][j][k-1])
						     - m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEy_p_Inv * (Hz[i][j][k] - Hz[i-1][j][k]);

					Ey[i][j][k] *= m_coeff_zxy[k] ;
					
					m_Dey_Ymax[i][jj][k] = m_coeff_xyz[i] * tep1 + ( m_Pey_Ymax[i][jj][k] - tep2 ) * m_coeff_xyzp_inv[i] ;
		
					Ey[i][j][k] += ( m_coeff_yyp[j] * m_Dey_Ymax[i][jj][k] - m_coeff_yym[j] * tep1) * m_coeff_zxyp_inv[k] ;
					

				}
			}
		}
	}

	if( m_boundary[ZMIN] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k < m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k ++)
				{
					tep2 = m_Pey_Zmin[i][j][k] ;

					tep1 = m_Dey_Zmin[i][j][k] ;

					m_Pey_Zmin[i][j][k] = m_eMatType[i][j][k]->AEy_Coeff * tep2 
							 + m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEy_p_Inv * (Hx[i][j][k] - Hx[i][j][k-1])
						     - m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEy_p_Inv * (Hz[i][j][k] - Hz[i-1][j][k]);

					Ey[i][j][k] *= m_coeff_zxy[k] ;
					
					m_Dey_Zmin[i][j][k] = m_coeff_xyz[i] * tep1 + ( m_Pey_Zmin[i][j][k] - tep2 ) * m_coeff_xyzp_inv[i] ;
		
					Ey[i][j][k] += ( m_coeff_yyp[j] * m_Dey_Zmin[i][j][k] - m_coeff_yym[j] * tep1) * m_coeff_zxyp_inv[k] ;
					
				}
			}
		}
	}

	if( m_boundary[ZMAX] == BC_UPML)
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ; k <= m_Index_E_Boundary[ZMAX] ; k ++)
				{
					kk = k - ( m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] + 1 ) ;
 
					tep2 = m_Pey_Zmax[i][j][kk] ;

					tep1 = m_Dey_Zmax[i][j][kk] ;

					m_Pey_Zmax[i][j][kk] = m_eMatType[i][j][k]->AEy_Coeff * tep2 
							 + m_pEk_Coeff[k] * m_eMatType[i][j][k]->AEy_p_Inv * (Hx[i][j][k] - Hx[i][j][k-1])
						     - m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEy_p_Inv * (Hz[i][j][k] - Hz[i-1][j][k]);

					Ey[i][j][k] *= m_coeff_zxy[k] ;
					
					m_Dey_Zmax[i][j][kk] = m_coeff_xyz[i] * tep1 + ( m_Pey_Zmax[i][j][kk] - tep2 ) * m_coeff_xyzp_inv[i] ;
		
					Ey[i][j][k] += ( m_coeff_yyp[j] * m_Dey_Zmax[i][j][kk] - m_coeff_yym[j] * tep1) * m_coeff_zxyp_inv[k] ;
					

				}
			}
		}
	}

//	-----------------------------------------------------------------------
//	EZ Component 
//	-----------------------------------------------------------------------
	if( m_boundary[XMIN] == BC_UPML) 
	{
		for( i = m_Index_E_Boundary[XMIN] ; i < m_Index_E_Boundary[XMIN] + m_boundaryLayerNum[XMIN] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k < m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{
					tep2 = m_Pez_Xmin[i][j][k] ;

					tep1 = m_Dez_Xmin[i][j][k] ; 

					m_Pez_Xmin[i][j][k] = m_eMatType[i][j][k]->AEz_Coeff * tep2 
							 + m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEz_p_Inv * (Hy[i][j][k] - Hy[i-1][j][k])
							 - m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEz_p_Inv * (Hx[i][j][k] - Hx[i][j-1][k]);

					Ez[i][j][k] *= m_coeff_xyz[i] ;
					
					m_Dez_Xmin[i][j][k] = m_coeff_yxz[j] * tep1 + ( m_Pez_Xmin[i][j][k] - tep2 ) * m_coeff_yxzp_inv[j] ; 

					Ez[i][j][k] += ( m_coeff_zzp[k] * m_Dez_Xmin[i][j][k] - m_coeff_zzm[k] * tep1) * m_coeff_xyzp_inv[i] ;   
					
				}
			}
		}
	}

	if( m_boundary[XMAX] == BC_UPML) 
	{
		for( i = m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1 ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			ii = i - ( m_Index_E_Boundary[XMAX] - m_boundaryLayerNum[XMAX] + 1 );
			for( j = m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN] ; j <= m_Index_E_Boundary[YMAX] -  m_boundaryLayerNum[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k < m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k++)
				{

					tep2 = m_Pez_Xmax[ii][j][k] ;

					tep1 = m_Dez_Xmax[ii][j][k] ; 

					m_Pez_Xmax[ii][j][k] = m_eMatType[i][j][k]->AEz_Coeff * tep2
							 + m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEz_p_Inv * (Hy[i][j][k] - Hy[i-1][j][k])
							 - m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEz_p_Inv * (Hx[i][j][k] - Hx[i][j-1][k]);
					
					Ez[i][j][k] *=  m_coeff_xyz[i] ;
					
					m_Dez_Xmax[ii][j][k] = m_coeff_yxz[j] * tep1 + ( m_Pez_Xmax[ii][j][k] - tep2 ) * m_coeff_yxzp_inv[j] ; 

					Ez[i][j][k] += ( m_coeff_zzp[k] * m_Dez_Xmax[ii][j][k] - m_coeff_zzm[k] * tep1) * m_coeff_xyzp_inv[i] ;   
					
				}
			}
		}
	}

	if( m_boundary[YMIN] == BC_UPML) 
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j < m_Index_E_Boundary[YMIN] + m_boundaryLayerNum[YMIN]; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k < m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k ++)
				{
					tep2 = m_Pez_Ymin[i][j][k] ;

					tep1 = m_Dez_Ymin[i][j][k] ; 

					m_Pez_Ymin[i][j][k] = m_eMatType[i][j][k]->AEz_Coeff * tep2 
							 + m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEz_p_Inv * (Hy[i][j][k] - Hy[i-1][j][k])
							 - m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEz_p_Inv * (Hx[i][j][k] - Hx[i][j-1][k]);

					Ez[i][j][k] *= m_coeff_xyz[i] ;
					
					m_Dez_Ymin[i][j][k] = m_coeff_yxz[j] * tep1 + ( m_Pez_Ymin[i][j][k] - tep2 ) * m_coeff_yxzp_inv[j] ; 

					Ez[i][j][k] += ( m_coeff_zzp[k] * m_Dez_Ymin[i][j][k] - m_coeff_zzm[k] * tep1) * m_coeff_xyzp_inv[i] ;   
					
				}
			}
		}
	}

	if( m_boundary[YMAX] == BC_UPML) 
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ; j <= m_Index_E_Boundary[YMAX] ; j++)
			{
				jj = j - ( m_Index_E_Boundary[YMAX] - m_boundaryLayerNum[YMAX] + 1 ) ;
				for( k = m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k < m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k ++)
				{
					

					tep2 = m_Pez_Ymax[i][jj][k] ;

					tep1 = m_Dez_Ymax[i][jj][k] ; 

					m_Pez_Ymax[i][jj][k] = m_eMatType[i][j][k]->AEz_Coeff * tep2 
							 + m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEz_p_Inv * (Hy[i][j][k] - Hy[i-1][j][k])
							 - m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEz_p_Inv * (Hx[i][j][k] - Hx[i][j-1][k]);

					Ez[i][j][k] *=  m_coeff_xyz[i] ;
					
					m_Dez_Ymax[i][jj][k] = m_coeff_yxz[j] * tep1 + ( m_Pez_Ymax[i][jj][k] - tep2 ) * m_coeff_yxzp_inv[j] ; 

					Ez[i][j][k] += ( m_coeff_zzp[k] * m_Dez_Ymax[i][jj][k] - m_coeff_zzm[k] * tep1) * m_coeff_xyzp_inv[i] ;   
					
				}
			}
		}
	}

	if( m_boundary[ZMIN] == BC_UPML) 
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMIN] ; k < m_Index_E_Boundary[ZMIN] + m_boundaryLayerNum[ZMIN] ; k ++)
				{
					tep2 = m_Pez_Zmin[i][j][k] ;

					tep1 = m_Dez_Zmin[i][j][k] ; 

					m_Pez_Zmin[i][j][k] = m_eMatType[i][j][k]->AEz_Coeff * tep2 
							 + m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEz_p_Inv * (Hy[i][j][k] - Hy[i-1][j][k])
							 - m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEz_p_Inv * (Hx[i][j][k] - Hx[i][j-1][k]);

					Ez[i][j][k] *= m_coeff_xyz[i] ;
					
					m_Dez_Zmin[i][j][k] = m_coeff_yxz[j] * tep1 + ( m_Pez_Zmin[i][j][k] - tep2 ) * m_coeff_yxzp_inv[j] ; 

					Ez[i][j][k] += ( m_coeff_zzp[k] * m_Dez_Zmin[i][j][k] - m_coeff_zzm[k] * tep1) * m_coeff_xyzp_inv[i] ;   
					
				}
			}
		}
	}

	if( m_boundary[ZMAX] == BC_UPML) 
	{
		for( i = m_Index_E_Boundary[XMIN] ; i <= m_Index_E_Boundary[XMAX] ; i++)
		{
			for( j = m_Index_E_Boundary[YMIN] ; j <= m_Index_E_Boundary[YMAX] ; j++)
			{
				for( k = m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX] ; k < m_Index_E_Boundary[ZMAX] ; k ++)
				{
					kk = k - (m_Index_E_Boundary[ZMAX] - m_boundaryLayerNum[ZMAX]);
 
					tep2 = m_Pez_Zmax[i][j][kk] ;

					tep1 = m_Dez_Zmax[i][j][kk] ; 

					m_Pez_Zmax[i][j][kk] = m_eMatType[i][j][k]->AEz_Coeff * tep2 
							 + m_pEi_Coeff[i] * m_eMatType[i][j][k]->AEz_p_Inv * (Hy[i][j][k] - Hy[i-1][j][k])
							 - m_pEj_Coeff[j] * m_eMatType[i][j][k]->AEz_p_Inv * (Hx[i][j][k] - Hx[i][j-1][k]);

					Ez[i][j][k] *=  m_coeff_xyz[i] ;
					
					m_Dez_Zmax[i][j][kk] = m_coeff_yxz[j] * tep1 + ( m_Pez_Zmax[i][j][kk] - tep2 ) * m_coeff_yxzp_inv[j] ; 

					Ez[i][j][k] += ( m_coeff_zzp[k] * m_Dez_Zmax[i][j][kk] - m_coeff_zzm[k] * tep1) * m_coeff_xyzp_inv[i] ;   
					
				}
			}
		}
	}
}

