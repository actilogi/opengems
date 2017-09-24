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

class CMaterial
{
public:
	CMaterial(void);
	~CMaterial(void);
	// Electric properties
	float Eps_x, Eps_y, Eps_z ;
	float Sig_x, Sig_y, Sig_z ;

	char xyzBits ;

	float AEx_Coeff, AEy_Coeff, AEz_Coeff;
	float AEx_p_Inv, AEy_p_Inv, AEz_p_Inv;

private:
	CMaterial* next;

public:

	void setDt(float Dt) ;

	CMaterial* getNext(void);
	void CalculateEMaterialCoeff( float dt );
	void setNext(CMaterial* next);
};


struct InputMaterialInfo{
	int direction ;
	int xIndex , yIndex, zIndex ;

	float epsilon, sigma;

	InputMaterialInfo *next ;
} ;



// common routine for Material Processing
CMaterial *CMaterial_Locate( CMaterial *(&eMaterials_Common), CMaterial ****eMatType, char ***eMatTypeFlag, int x, int y, int z, int direction, float epsilon, float sigma );

void SetEMatFreeSpace( CMaterial *eMaterials_Common, CMaterial ****eMatType, char ***eMatTypeFlag , int *Index_E_Boundary , int boundaryLayerNum[6]) ;

void SetEMatPML( CMaterial ****eMatType, int *Index_E_Boundary , int boundaryLayerNum[6] , int nx, int ny, int nz ) ;

