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
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace GEMS.Designer.Models.GeometryModels
{
    public class CuboidModel : GeometryModel
    {
        private float width;
        private float height;
        private float depth;
        private Vector3 centerVector3;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public Vector3 CenterVector3
        {
            get { return centerVector3; }
            set { centerVector3 = value; }
        }

    
        public override bool PositionRelation(Vector3 vertex)
        {
            //Transform the vertex to the object space
            Matrix inver = Matrix.Invert(worldMatrix);
            vertex.TransformCoordinate(inver);

            bool isInX = Math.Abs(vertex.X) <= (width / 2) + GeometryModel.DefaultDistanceDelta;
            bool isInY = Math.Abs(vertex.Y) <= (depth / 2) + GeometryModel.DefaultDistanceDelta;
            bool isInZ = Math.Abs(vertex.Z) <= (height / 2) + GeometryModel.DefaultDistanceDelta;

            return isInX && isInY && isInZ;
        }
    }
}
