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

#ifndef FDTD_COMMOM_FUNCTION_HEAD_FILE
#define FDTD_COMMOM_FUNCTION_HEAD_FILE

#include "Global_variable.h"
#include "Material.h"


float Excitation_Source( int n , float delay , float *pulse, int pulseLen , float Dt ) ;


// convert global index to local indexx
void Global2Local( int xs, int ys, int zs, int &xt, int &yt, int &zt , DomainIndex localDomainIndex , int boundaryLayerNum[6] ) ;

// convert local index to global index
void Local2Global( int xl, int yl, int zl, int &xg, int &yg, int &zg , DomainIndex localDomainIndex , int boundaryLayerNum[6] ) ;

// output sigma distribution for material test
void OutputSigma( CMaterial ****eMatType , int *Index_E_Boundary, int boundaryLayerNum[6],
				 float *X_Grid, float *Y_Grid, float *Z_Grid , float *Dx_Grid, float *Dy_Grid, float *Dz_Grid , 
				 float *X_Grid_Global, float *Y_Grid_Global, float *Z_Grid_Global ,
				 int *Nx_Local2Global, int *Ny_Local2Global, int *Nz_Local2Global,
				 Point domainRef , float unit ) ;

#endif
