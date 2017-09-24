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
#include "FDTD_Source.h"
#include "CurrentPath.h"

class CCurrent_Source :
	public CFDTD_Source
{
public:
	CCurrent_Source(void);
	~CCurrent_Source(void);
	int init(void);
	void update_H_Field(int n, float*** (&Hx), float*** (&Hy), float*** (&Hz), int sourceIndex = 0 , CFarField *farField = NULL ) ;

	int readIn_Solver( ) ;
	int readIn(FILE *in);
private:
	CurrenDirection m_dir ;			// current direction
	int m_verticalIndex ;			// current loop plane index in local domain

	int m_cellNum ;					// number of cell in loop
	CCurrentPath *m_path ;

	float m_current ;
	float m_delay ;

};
