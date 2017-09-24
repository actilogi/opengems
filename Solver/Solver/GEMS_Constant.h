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

// This header file define constant needed by CFDTD simulator

#ifndef GEMS_CONSTANT_HEADER_FILE
#define GEMS_CONSTANT_HEADER_FILE

#define MAX_OUTPUTNAME_LENGTH 256

// Constants
const float C_Speed		= 2.997956e8f ;
const float C_Speed_Inv	= 0.3335605e-8f ;
const float Eps0			= 8.854817187e-12f ;
const float Eps0_Inv		= 1.12932879232e11f;

const float Mu0			= 1.256637016e-06f ;
const float Mu0_Inv		= 7.957747442e5f ;

const float PI				= 3.141592654f ;
const float Eta0			= 376.733341f ;
const float Eta0_Inv		= 0.002654397f ;
const float Sig_PEC		= 1.0e20f ;
const float SmallNumber	= 0.00001f ;


const float Factor_Rad_Deg	= 57.295779513082320876798154814105f ;		// 180 / PI
const float Factor_Deg_Rad	= 0.017453292519943295769236907684886f ;	// PI / 180

// constants for boundary conditions
const int XMIN = 0 ;
const int XMAX = 1 ;
const int YMIN = 2 ;
const int YMAX = 3 ;
const int ZMIN = 4 ;
const int ZMAX = 5 ;

// Error code for FDTD 
const int FDTD_SUCCESS = 0 ;
const int FDTD_NO_MEMORY = 1 ;
const int OTHER_ERROR = 2 ;

const int PML_LAYERNUM = 6 ;

#define TOLERANCE 0.001
#define ZERO 1e-6


enum { X_DIRECTION, Y_DIRECTION, Z_DIRECTION , NON_2D_DIRECTION };

#endif
