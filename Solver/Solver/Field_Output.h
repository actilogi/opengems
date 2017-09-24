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

#include <string>

class CField_Output :
	public CFDTD_Output
{
public:
	CField_Output(void);
	~CField_Output(void);
	int init(void);
	void collectResult(int n, float ***(&Ex), float ***(&Ey), float ***(&Ez), float ***(&Hx), float ***(&Hy), float ***(&Hz) );
	void output( XML_Writer &outFile ) ;

private:
	int m_xIndex;
	int m_yIndex;
	int m_zIndex;

public:

	// return the index of the point in local domain
	void getIndex(int& x, int& y, int& z);

	// set the index of point in local domain
	void setIndex(int x, int y, int z);
private:
	// which component is selected for outputting
	long m_outFlag;

	// whether the output point is in this domain or not
	bool m_insideFlag;

	float **m_field ;

public:
	// get and set output flag of the Field
	long getOutFlag(void);
	void setOutFlag(long flag);

	// get and set inside flag of the point
	bool getInsideFlag(void);
	void setInsideFlag(bool flag);


	int readIn_Solver( ) ;
	int readIn(FILE *in);
};
