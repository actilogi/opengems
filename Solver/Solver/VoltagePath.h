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

// a voltage is represent by a line, in FDTD, this line is divided into several segments
// this struct define the segment of a line
// start is the start point index of the segment
// end is the end point index of the segment
// next is the address of the next segment, if the segment is the last segment of the line, next is null

class CVoltagePath
{
public:
	CVoltagePath(void);
	~CVoltagePath(void);
public:

	// index of start and end point in local domain
	int startX , endX ;
	int startY , endY ;
	int startZ , endZ ;

	CVoltagePath *next;
};

class CVoltageOutPath :
	public CVoltagePath
{
public:
	CVoltageOutPath(void);
	~CVoltageOutPath(void);
	
public:
	// In FDTD, if voltage lies on the parallel boundary, we need multiply the value with a factor 0.5
	// so we collect result from all processors, the result will not be added twice
	float factor;
};

