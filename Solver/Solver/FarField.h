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
#include <complex>


enum FrequencyUnit
        {
            Hz = 0,
            KHz,
            MHz,
            GHz,
            THz,
            PHz,
        } ;
struct HuygensBox
{
	int startX , startY , startZ ;
	int endX , endY , endZ ;
} ;



// Tangential Currents on Huygen's box
// e.g. For normal vector = x, 1 and 2 represent y and z respectively.
struct Tang_Current
{
	std::complex<float> J1, J2;
	std::complex<float> M1, M2;
} ;

class CFarField
{
public:
	CFarField(void);
	~CFarField(void);

	int readIn_Solver( );
	int readIn(FILE *in);
	// Initialize the parameter of farfield
	int init(void);
private:
	// huygens box in local domain
	HuygensBox m_huygensBox;

	// Hutgens Box in global domain 
	HuygensBox m_huygensBox_Global;

	// Huygens Box face flag in local domain 
	bool m_huy_FaceFlag[6] ;

	// Huygens Box face flag in global domain 
	bool m_huy_FaceFlag_Global[6] ;

	// far field output flag
	bool m_enabled ;
	
	// 
	float m_Dt ;

	// far Field frequency setting
	FrequencyUnit m_freqUnit ;
	int m_freqNum ;
	float *m_freqList ;
	float *m_freqList_Inv ;

	// Far Field 2D Pattern
	bool m_phiEnabled ;
	int m_phiNum ;
	float *m_phiList ;
	float m_thetaStart , m_thetaEnd , m_thetaStep ;

	bool m_thetaEnabled ;
	int m_thetaNum ;
	float *m_thetaList ;
	float m_phiStart , m_phiEnd , m_phiStep ;

	bool m_use_MJ ;	// Flag for using M or both currents, if true, user only M current, if false, user both

	// left corner of Huygens Box
	Point m_huygensBox_Ref ;

	int m_boundaryLayerNum[6] ;

	// Radiation Power 
	float *m_radiationPower ;

	// variables for calculating incident power
	float **m_sourceReal ;
	float **m_sourceImag ;

	float **m_powerReal ;
	float **m_powerImag ;

	// excitation source number
	int m_sourceNum ;

	// storage of far-field in far-field calculation
	std::complex<float> m_Far_Etheta, m_Far_Ephi;

	// variables for far field calculation

	// Huygen's currents 
	Tang_Current ***m_Huy_S1 , ***m_Huy_S2 , ***m_Huy_S3 ;
	Tang_Current ***m_Huy_S4 , ***m_Huy_S5 , ***m_Huy_S6 ;

	int m_nHuy_Curr[3] ;		// Number of points on huygen box after sampling

	float *m_x_Center, *m_y_Center, *m_z_Center;
	float *m_x_Interface , *m_y_Interface , *m_z_Interface ;
	float *m_x_Interface_Global, *m_y_Interface_Global, *m_z_Interface_Global ;


	// Intermediate variables used in DFT calculation
	float m_Huy_xMin_Wg1, m_Huy_xMin_Wg2, m_Huy_xMax_Wg1, m_Huy_xMax_Wg2;
	float m_Huy_yMin_Wg1, m_Huy_yMin_Wg2, m_Huy_yMax_Wg1, m_Huy_yMax_Wg2;
	float m_Huy_zMin_Wg1, m_Huy_zMin_Wg2, m_Huy_zMax_Wg1, m_Huy_zMax_Wg2;


	float *m_Dx_Grid_Global , *m_Dy_Grid_Global , *m_Dz_Grid_Global ;
	float *m_X_Half_Global , *m_Y_Half_Global , *m_Z_Half_Global ;
	float *m_X_Grid_Global , *m_Y_Grid_Global , *m_Z_Grid_Global ;

	float *m_Dx_Grid , *m_Dy_Grid , *m_Dz_Grid ;

	DomainIndex *m_localDomainIndex ;

	// array store global index of local index
	int *m_Nx_Local2Global ;
	int *m_Ny_Local2Global ;
	int *m_Nz_Local2Global ;

	int m_nx , m_ny , m_nz ;

public:
	bool getEnabled( ) ;

	void farField_EM_Current(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) );
	void farField_Near2Far( XML_Writer &outFile );
private:
	void convert_HuyCurrent(void);
	void farField_Culculation(float theta, float phi, int freqIndex);
	bool AllocateMem( ) ;
public:
	void setDt( float Dt ) ;
	void setDomainIndex( DomainIndex *localDomainIndex ) ;

	void setBoundaryLayerNum( int boundaryLayerNum[6] ) ;

	void setCellNum(int nx, int ny, int nz);
	void setN_Local2Global(int* Nx_Local2Global, int* Ny_Local2Global, int* Nz_Local2Global);

	void setDGrid(float* Dx_Grid, float* Dy_Guid, float* Dz_Guid);
	void setDGrid_Global(float* Dx_Grid_Global, float* Dy_Grid_Global, float* Dz_Grid_Global);

	void setHalf_Global(float* X_Half_Global, float* Y_Half_Global, float* Z_Half_Global);
	void setGrid_Global(float* X_Grid_Global, float* Y_Grid_Global, float* Z_Grid_Global);

	// set the number of source
	void setSourceNum( int num ) ;

	// get far field frequency number
	int getFreqNum() const ;

	// get frequency at index
	float getFreq( int index ) ;

	void getSourcePower( float &sourceReal, float &sourceImag, float &powerReal, float &powerImag , int freqIndex, int sourceIndex) ;
	void setSourcePower( float sourceReal, float sourceImag, float powerReal, float powerImag , int freqIndex, int sourceIndex ) ;

};
