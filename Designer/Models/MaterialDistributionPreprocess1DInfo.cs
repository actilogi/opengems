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

using Microsoft.DirectX;

namespace GEMS.Designer.Models
{
    public class MaterialDistributionPreprocess1DInfo
    {
        Vector3 meshPoint1;
        Vector3 meshPoint2;
        MeshPointKey meshPoint1Key;
        MeshPointKey meshPoint2Key;

        Axis alineAxis;

        public Vector3 MeshPoint1
        {
            get { return meshPoint1; }
            set { meshPoint1 = value; }
        }

        public Vector3 MeshPoint2
        {
            get { return meshPoint2; }
            set { meshPoint2 = value; }
        }

        public MeshPointKey MeshPoint1Key
        {
            get { return meshPoint1Key; }
            set { meshPoint1Key = value; }
        }

        public MeshPointKey MeshPoint2Key
        {
            get { return meshPoint2Key; }
            set { meshPoint2Key = value; }
        }

        public Axis AlineAxis
        {
            get { return alineAxis; }
            set { alineAxis = value; }
        }

    }
}
