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

using System;
using System.Collections.Generic;
using System.Text;

namespace GEMS.Designer.Models
{
    class GEMSPreCalculateFileTags
    {      

        public const int MAX_OUTPUTNAME_LENGTH = 256;

        public const byte TAG_STAMP = 0x02;
        public const byte TAG_TIMESTEP = 0x03;
        public const byte TAG_LENUNITRATIO = 0x04;
        public const byte TAG_PULSE = 0x05 ; //Excitation Source
        public const byte TAG_CELLCOUNT =  0x10 ;
        public const byte TAG_PARALLEL = 0x20 ;
        public const byte TAG_DOMAIN = 0x30 ;
        public const byte TAG_HUYGENSBOX = 0x32 ;

        //Mesh
        public const byte TAG_CELLSIZE_X = 0x41 ;
        public const byte TAG_CELLSIZE_Y = 0x42 ;
        public const byte TAG_CELLSIZE_Z = 0x43 ;

        public const byte TAG_EMATERIAL_START =0x5E ;
        public const byte TAG_EMATERIAL_END   =0x5F ;

        public const byte TAG_EXCITATION_VOLTAGE  =0x71 ;
        public const byte TAG_EXCITATION_CURRENT  =0x72 ;

 
        public const byte TAG_OUTPUT_VOLTAGE      =0x81 ;
        public const byte TAG_OUTPUT_CURRENT      =0x82 ;
        public const byte TAG_OUTPUT_FIELDONPOINT =0x84 ;

        public const byte TAG_DIRECTION_X         =0x1 ;
        public const byte TAG_DIRECTION_X_MINUS   =0x2 ;
        public const byte TAG_DIRECTION_Y         =0x3 ;
        public const byte TAG_DIRECTION_Y_MINUS   =0x4 ;
        public const byte TAG_DIRECTION_Z         =0x5 ;
        public const byte TAG_DIRECTION_Z_MINUS   =0x6 ;

        public const byte TAG_END_FILE            =0xF0 ;
        public const byte TAG_ELEMENT_END = 0xFF ;

    }
}
