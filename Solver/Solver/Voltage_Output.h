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
#include "FDTD_Output.h"
#include "VoltagePath.h"

class CVoltage_Output :
	public CFDTD_Output
{
public:
	CVoltage_Output(void);
	~CVoltage_Output(void);
	int init(void);
private:
	CVoltageOutPath* m_path;

	// start and end point index of the voltage line
	int m_startX , m_endX ;
	int m_startY , m_endY ;
	int m_startZ , m_endZ ;

	int insertVoltagePath(int x1, int y1, int z1, int x2, int y2, int z2);

	float *m_voltage ;
public:
	// get the index of start point in local domain
	void getStartIndex(int& x, int& y, int& z);

	// get the index of end point in local domain
	void getEndIndex(int& x, int& y, int& z);

	// output field at a point at time step n
	void collectResult(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) );
	void output( XML_Writer &outFile ) ;

	// set start and end point index
	void setStartIndex(int x, int y, int z);
	void setEndIndex(int x, int y, int z);


	int readIn(FILE *in);
	int readIn_Solver( ) ;
};
