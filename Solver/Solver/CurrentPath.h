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

// defination of current
enum CurrenDirection{ DIRECTION_X=1, DIRECTION_X_MINUS, DIRECTION_Y, DIRECTION_Y_MINUS, DIRECTION_Z, DIRECTION_Z_MINUS } ;

class CCurrentPath
{
public:
	CCurrentPath(void);
	~CCurrentPath(void);

	// index of curren path segment in local domain
	int hStart , hEnd ;
	int vStart , vEnd ;
};

class CCurrentOutPath :
	public CCurrentPath
{
public:
	CCurrentOutPath(void);
	~CCurrentOutPath(void);
	float factor ;
};
