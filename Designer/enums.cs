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

//We defined all the enum and constant members in this file

using System;

namespace GEMS.Designer
{
    public enum Axis
    {
        X = 0,
        Y,
        Z
    }

    public enum GridPlane
    {
        XY = 0,
        XZ,
        YZ
    }

    public enum PluseType
    {
        None = -1,
        Gaussian = 0,
        Differentiated_Gaussian = 1
    }

    public enum BoundaryCondition
    {
        PEC = 0,
        PMC = 2,
        UPML=4 ,
        Mur=5 ,
    }
    


}

