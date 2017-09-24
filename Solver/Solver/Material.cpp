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

#include "Material.h"

#include "GEMS_Constant.h"
#include "Global_variable.h"

#include <cstdlib>
#include <cmath>
#include <cstdio>

CMaterial::CMaterial(void)
: Eps_x(1), Eps_y(1), Eps_z(1), Sig_x(0), Sig_y(0), Sig_z(0), xyzBits(0) ,
AEx_Coeff(1), AEy_Coeff(1), AEz_Coeff(1), AEx_p_Inv(1), AEy_p_Inv(1), AEz_p_Inv(1)
, next(NULL)
{
}

CMaterial::~CMaterial(void)
{
}

CMaterial *CMaterial::getNext(void)
{
	return next ;
}



void CMaterial::CalculateEMaterialCoeff(float dt )
{

	float factor_temp;
	
	float AEx_m, AEy_m, AEz_m;
	float AEx_p, AEy_p, AEz_p;

	factor_temp = 0.5;


	if( xyzBits == 7 )
	{
		AEx_m = Eps_x * Eps0 - factor_temp * dt * Sig_x  ;
		AEx_p = Eps_x * Eps0 + factor_temp * dt * Sig_x  ;
		AEy_m = Eps_y * Eps0 - factor_temp * dt * Sig_y  ;
		AEy_p = Eps_y * Eps0 + factor_temp * dt * Sig_y  ;
		AEz_m = Eps_z * Eps0 - factor_temp * dt * Sig_z  ;
		AEz_p = Eps_z * Eps0 + factor_temp * dt * Sig_z  ;

		AEx_p_Inv = 1.0f / AEx_p ;
		AEy_p_Inv = 1.0f / AEy_p ;
		AEz_p_Inv = 1.0f / AEz_p ;

		AEx_Coeff = AEx_m * AEx_p_Inv  ;
		AEy_Coeff = AEy_m * AEy_p_Inv  ;
		AEz_Coeff = AEz_m * AEz_p_Inv  ;

	
	}

}

void CMaterial::setNext(CMaterial* next)
{
	this->next = next ;
}


CMaterial *CMaterial_Locate( CMaterial *(&eMaterials_Common), CMaterial ****eMatType, char ***eMatTypeFlag, int x, int y, int z, int direction, float epsilon, float sigma )
{
	CMaterial *p, *lastp, *po;
	lastp = p = eMaterials_Common;
	po = eMatType[x][y][z];

	char xyzBits = eMatTypeFlag[x][y][z];		

	bool flag = false;
	while ( p != NULL ) 
	{

		double divE, divS;

		switch ( direction ) 
		{
			case X_DIRECTION : 
				{
					if ( p->xyzBits & 0x1 ) 
					{
						divE = fabs( p->Eps_x ) + fabs( epsilon );
						divS = fabs( p->Sig_x ) + fabs( sigma );

						if ( ( ( divE < ZERO ) || ( fabs( p->Eps_x - epsilon ) / divE < TOLERANCE ) ) && ( ( divS < ZERO ) || ( fabs( p->Sig_x - sigma ) / divS < TOLERANCE ) ) ) 
						{
							flag = true;
						}

					} 
					else 
					{
						flag = true;
					}
					break;
				}
			case Y_DIRECTION : 
				{
					if ( p->xyzBits & 0x2 ) 
					{
						divE = fabs( p->Eps_y ) + fabs( epsilon );
						divS = fabs( p->Sig_y ) + fabs( sigma );

						if ( ( ( divE < ZERO ) || ( fabs( p->Eps_y - epsilon ) / divE < TOLERANCE ) ) && ( ( divS < ZERO ) || ( fabs( p->Sig_y - sigma ) / divS < TOLERANCE ) ) ) 
						{
							flag = true;
						}

					} 
					else 
					{
						flag = true;
					}

					break;
				}
			case Z_DIRECTION : 
				{
					if ( p->xyzBits & 0x4 ) 
					{
						divE = fabs( p->Eps_z ) + fabs( epsilon );
						divS = fabs( p->Sig_z ) + fabs( sigma );


						if ( ( ( divE < ZERO ) || ( fabs( p->Eps_z - epsilon ) / divE < TOLERANCE ) ) && ( ( divS < ZERO ) || ( fabs( p->Sig_z - sigma ) / divS < TOLERANCE ) ) ) 
						{
							flag = true;
						}

					} 
					else 
					{
						flag = true;
					}
				}
		}

		if ( flag && ( xyzBits & 0x1 ) ) 
		{
			if ( p->xyzBits & 0x1 ) 
			{
				divE = fabs( p->Eps_x ) + fabs( po->Eps_x );
				divS = fabs( p->Sig_x ) + fabs( po->Sig_x );

				if ( ( ( divE > ZERO ) && ( fabs( p->Eps_x - po->Eps_x ) / divE > TOLERANCE ) ) || ( ( divS > ZERO ) && ( fabs( p->Sig_x - po->Sig_x ) / divS > TOLERANCE ) ) ) 
				{
					flag = false;
				}
			} 
			else 
			{
				flag = false;
			}
		}

		if ( flag && ( xyzBits & 0x2 ) ) 
		{
			if ( p->xyzBits & 0x2 ) {
				divE = fabs( p->Eps_y ) + fabs( po->Eps_y );
				divS = fabs( p->Sig_y ) + fabs( po->Sig_y );

				if ( ( ( divE > ZERO ) && ( fabs( p->Eps_y - po->Eps_y ) / divE > TOLERANCE ) ) || ( ( divS > ZERO ) && ( fabs( p->Sig_y - po->Sig_y ) / divS > TOLERANCE ) ) ) {
					flag = false;
				}
			} else {
				flag = false;
			}
		}

		if ( flag && ( xyzBits & 0x4 ) ) 
		{
			if ( p->xyzBits & 0x4 ) 
			{
				divE = fabs( p->Eps_z ) + fabs( po->Eps_z );
				divS = fabs( p->Sig_z ) + fabs( po->Sig_z );

				if ( ( ( divE > ZERO ) && ( fabs( p->Eps_z - po->Eps_z ) / divE > TOLERANCE ) ) || ( ( divS > ZERO ) && ( fabs( p->Sig_z - po->Sig_z ) / divS > TOLERANCE ) ) ) 
				{
					flag = false;
				}
			} 
			else 
			{
				flag = false;
			}
		}

		if ( flag ) break;

		lastp = p;
		p = p->getNext();
	}

	if ( p == NULL ) 
	{
		p = new CMaterial;
		p->xyzBits = 0;

		if ( xyzBits & 0x1 ) {
			p->xyzBits |= 0x1;
			p->Eps_x = po->Eps_x;
			p->Sig_x = po->Sig_x;
		}

		if ( xyzBits & 0x2 ) {
			p->xyzBits |= 0x2;
			p->Eps_y = po->Eps_y;
			p->Sig_y = po->Sig_y;	
		}

		if ( xyzBits & 0x4 ) {
			p->xyzBits |= 0x4;
			p->Eps_z = po->Eps_z;
			p->Sig_z = po->Sig_z;			
		}

		if ( lastp != NULL ) 
		{
			lastp->setNext( p ) ;
		} 
		else 
		{
			eMaterials_Common = p;
		}

	}

	switch ( direction ) {
		case X_DIRECTION : {
			xyzBits |= 0x1;
			p->xyzBits |= 0x1;
			p->Eps_x = epsilon;
			p->Sig_x = sigma;
			
			break;
		}
		case Y_DIRECTION : {
			xyzBits |= 0x2;
			p->xyzBits |= 0x2;
			p->Eps_y = epsilon;
			p->Sig_y = sigma;
			
			break;
		}
		case Z_DIRECTION : {
			xyzBits |= 0x4;
			p->xyzBits |= 0x4;
			p->Eps_z = epsilon;
			p->Sig_z = sigma;
		}
	}
	

	eMatTypeFlag[x][y][z] = xyzBits;

	return p;
}


void SetEMatFreeSpace( CMaterial *eMaterials_Common, CMaterial ****eMatType, char ***eMatTypeFlag , int *Index_E_Boundary , int boundaryLayerNum[6] )
{
	int x, y, z;

	CMaterial *p;


	for ( x = Index_E_Boundary[XMIN] + boundaryLayerNum[XMIN] ; x < Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ; x ++ ) 
	{
		for ( y = Index_E_Boundary[YMIN] + boundaryLayerNum[YMIN] ; y < Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ; y ++ ) 
		{
			for ( z = Index_E_Boundary[ZMIN] + boundaryLayerNum[ZMIN] ; z < Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ; z ++ ) 
			{
				if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {
					p = CMaterial_Locate( eMaterials_Common, eMatType, eMatTypeFlag ,x, y, z, X_DIRECTION, 1.0f, 0.0f );					
					eMatType[x][y][z] = p;					
				}
				if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
					p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 1.0f, 0.0f );
					eMatType[x][y][z] = p;					
				}
				if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
					p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 1.0f, 0.0f );
					eMatType[x][y][z] = p;					
				}				
			}
		}
	}	
	
	//XMAX
	x = Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ;
	for ( y = Index_E_Boundary[YMIN] + boundaryLayerNum[YMIN] ; y < Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ; y ++ ) 
	{
		for ( z = Index_E_Boundary[ZMIN] + boundaryLayerNum[ZMIN] ; z < Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ; z ++ ) 
		{
			//if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {
			if ( true ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, X_DIRECTION, 
					eMatType[x-1][y][z]->Eps_x, eMatType[x-1][y][z]->Sig_x );
				eMatType[x][y][z] = p;				
			}
			if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 1.0f, 0.0f );
				eMatType[x][y][z] = p;				
			}
			if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 1.0f, 0.0f );
				eMatType[x][y][z] = p;				
			}
		}
	}

	//YMAX
	y = Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ;
	for ( z = Index_E_Boundary[ZMIN] + boundaryLayerNum[ZMIN] ; z < Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ; z ++ ) 
	{
		for ( x = Index_E_Boundary[XMIN] + boundaryLayerNum[XMIN] ; x < Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ; x ++ ) 
		{
			if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, X_DIRECTION, 1.0f, 0.0f );
				eMatType[x][y][z] = p;				
			}
			//if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
			if ( true ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 
					eMatType[x][y-1][z]->Eps_y, eMatType[x][y-1][z]->Sig_y );
				eMatType[x][y][z] = p;				
			}
			if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 1.0f, 0.0f );
				eMatType[x][y][z] = p;				
			}
		}
	}
	//ZMAX
	z = Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ;
	for ( x = Index_E_Boundary[XMIN] + boundaryLayerNum[XMIN]; x < Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ; x ++ ) 
	{
		for ( y = Index_E_Boundary[YMIN] + boundaryLayerNum[YMIN]; y < Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ; y ++ ) 
		{
			if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, X_DIRECTION, 1.0f, 0.0f );
				eMatType[x][y][z] = p;				
			}
			if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 1.0f, 0.0f );
				eMatType[x][y][z] = p;				
			}
			//if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
			if ( true ) {
				p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 
					eMatType[x][y][z-1]->Eps_z, eMatType[x][y][z-1]->Sig_z );
				eMatType[x][y][z] = p;				
			}
		}
	}

	//Line	
	y = Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ;
	z = Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ;
	for ( x = Index_E_Boundary[XMIN] + boundaryLayerNum[XMIN] ; x < Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ; x ++ ) 
	{		
		if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {
			p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, X_DIRECTION, 1.0f, 0.0f );
			eMatType[x][y][z] = p;				
		}
		//if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
		p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 
			eMatType[x][y-1][z]->Eps_y, eMatType[x][y-1][z]->Sig_y );
		eMatType[x][y][z] = p;			
		//if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
		p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 
			eMatType[x][y][z-1]->Eps_z, eMatType[x][y][z-1]->Sig_z );
		eMatType[x][y][z] = p;			
	}

	x = Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ;	
	z = Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ;
	for ( y = Index_E_Boundary[YMIN] + boundaryLayerNum[YMIN] ; y < Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ; y ++ ) 
	{
		//if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {
		p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, X_DIRECTION, 
			eMatType[x-1][y][z]->Eps_x, eMatType[x-1][y][z]->Sig_x );
		eMatType[x][y][z] = p;			
		if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
			p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 1.0f, 0.0f );
			eMatType[x][y][z] = p;				
		}
		//if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
		p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 
			eMatType[x][y][z-1]->Eps_z, eMatType[x][y][z-1]->Sig_z );
		eMatType[x][y][z] = p;			

	}

	x = Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ;
	y = Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ;	
	for ( z = Index_E_Boundary[ZMIN] + boundaryLayerNum[ZMIN] ; z < Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ; z ++ ) 
	{
		//if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {
		p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, X_DIRECTION, 
			eMatType[x-1][y][z]->Eps_x, eMatType[x-1][y][z]->Sig_x );
		eMatType[x][y][z] = p;			

		//if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
		p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 
			eMatType[x][y-1][z]->Eps_y, eMatType[x][y-1][z]->Sig_y );
		eMatType[x][y][z] = p;			

		if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
			p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 1.0f, 0.0f );
			eMatType[x][y][z] = p;				
		}
	}
	
	//Vertex
	x = Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX] ;
	y = Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX] ;
	z = Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX] ;

	//if ( !( eMatTypeFlag[x][y][z] & 0x1 ) ) {				
	p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, X_DIRECTION, 
		eMatType[x-1][y][z]->Eps_x, eMatType[x-1][y][z]->Sig_x );
	eMatType[x][y][z] = p;		

	//if ( !( eMatTypeFlag[x][y][z] & 0x2 ) ) {
	p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Y_DIRECTION, 
		eMatType[x][y-1][z]->Eps_y, eMatType[x][y-1][z]->Sig_y );
	eMatType[x][y][z] = p;		

	//if ( !( eMatTypeFlag[x][y][z] & 0x4 ) ) {
	p = CMaterial_Locate( eMaterials_Common, eMatType,eMatTypeFlag ,x, y, z, Z_DIRECTION, 
		eMatType[x][y][z-1]->Eps_z, eMatType[x][y][z-1]->Sig_z );
	eMatType[x][y][z] = p;		

}

void SetEMatPML( CMaterial ****eMatType, int *Index_E_Boundary , int boundaryLayerNum[6] , int nx, int ny, int nz )
{	
	int x, y, z, x1, y1, z1, x2, y2, z2;

	x1 = Index_E_Boundary[XMIN] + boundaryLayerNum[XMIN];
	y1 = Index_E_Boundary[YMIN] + boundaryLayerNum[YMIN];
	z1 = Index_E_Boundary[ZMIN] + boundaryLayerNum[ZMIN];
	x2 = Index_E_Boundary[XMAX] - boundaryLayerNum[XMAX];
	y2 = Index_E_Boundary[YMAX] - boundaryLayerNum[YMAX];
	z2 = Index_E_Boundary[ZMAX] - boundaryLayerNum[ZMAX];
	

	// YZ- plane
	for ( x = 0; x < x1; x ++ ) {
		for ( y = 0; y < ny ; y ++ ) {
			for ( z = 0; z < nz ; z ++ ) {
				eMatType[x][y][z] = eMatType[x1][y][z];
			}
		}
	}

	// XZ- plane
	for ( y = 0; y < y1; y ++ ) {
		for ( x = 0; x < nx ; x ++ ) {
			for ( z = 0; z < nz ; z ++ ) {
				eMatType[x][y][z] = eMatType[x][y1][z];
			}
		}
	}

	// XY- plane
	for ( z = 0; z < z1; z ++ ) {
		for ( x = 0; x < nx ; x ++ ) {
			for ( y = 0; y < ny ; y ++ ) {
				eMatType[x][y][z] = eMatType[x][y][z1];				
			}
		}
	}

	// YZ+ plane
	for ( x = x2+1; x < nx  ; x ++ ) {
		for ( y = 0; y < ny  ; y ++ ) {
			for ( z = 0; z < nz ; z ++ ) {			
				eMatType[x][y][z] = eMatType[x2][y][z];				
			}
		}
	}

	// XZ+ plane
	for ( y = y2+1; y < ny ; y ++ ) {
		for ( x = 0; x < nx ; x ++ ) {
			for ( z = 0; z < nz ; z ++ ) {
				eMatType[x][y][z] = eMatType[x][y2][z];				
			}
		}
	}

	// XY+ plane
	for ( z = z2+1; z < nz ; z ++ ) {
		for ( x = 0; x < nx ; x ++ ) {
			for ( y = 0; y < ny ; y ++ ) {
				eMatType[x][y][z] = eMatType[x][y][z2];				
			}
		}
	}

}
