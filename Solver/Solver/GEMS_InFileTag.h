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

#ifndef GEMS_INPUT_FILE_TAG_HEADER_FILE
#define GEMS_INPUT_FILE_TAG_HEADER_FILE

// tag for input file

#define TAG_STAMP char( 0x2 )

#define TAG_PULSE char( 0x5 )

#define TAG_CELLCOUNT char( 0x10 )

#define TAG_PARALLEL char( 0x20 )
#define TAG_TIMESTEP char( 0x3 )
#define TAG_LENUNITRATIO char( 0x4 )


#define TAG_DOMAIN char( 0x30 )			// boundary condition

#define TAG_HUYGENSBOX char( 0x32 )


#define TAG_CELLSIZE_X char( 0x41 )
#define TAG_CELLSIZE_Y char( 0x42 )
#define TAG_CELLSIZE_Z char( 0x43 )

#define TAG_EMATERIAL_START char( 0x5E )
#define TAG_EMATERIAL_END char( 0x5F )

#define TAG_EXCITATION_VOLTAGE char( 0x71 )
#define TAG_EXCITATION_CURRENT char( 0x72 )

#define TAG_OUTPUT_VOLTAGE char( 0x81 )
#define TAG_OUTPUT_CURRENT char( 0x82 )
#define TAG_OUTPUT_FIELDONPOINT char( 0x84 )

// tag for current excitation and output
#define TAG_DIRECTION_X char( 0x1 )
#define TAG_DIRECTION_X_MINUS char( 0x2 )
#define TAG_DIRECTION_Y v( 0x3 )
#define TAG_DIRECTION_Y_MINUS char( 0x4 )
#define TAG_DIRECTION_Z char( 0x5 )
#define TAG_DIRECTION_Z_MINUS char( 0x6 )

#define TAG_END_FILE char( 0xF0 )

#endif

