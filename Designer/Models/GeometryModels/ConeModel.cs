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
    public class ConeModel : GeometryModel
    {
        private float height;
        private float radius1;
        private float radius2;
        private Vector3 centerVector3;
        private Axis alineAxis;
        
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Radius1
        {
            get { return radius1; }
            set { radius1 = value; }
        }

        public float Radius2
        {
            get { return radius2; }
            set { radius2 = value; }
        }

        public Vector3 CenterVector3
        {
            get { return centerVector3; }
            set { centerVector3 = value; }
        }

        public Axis AlineAxis
        {
            get { return alineAxis; }
            set { alineAxis = value; }
        }

       

        public override bool PositionRelation(Vector3 vertex)
        {
            //Transform the vertex to the object space
            Matrix inver = Matrix.Invert(worldMatrix);
            vertex.TransformCoordinate(inver);

            //Calcluate the distance from vertex to z axis
            float distance1 = (float)Math.Pow(vertex.X * vertex.X + vertex.Y * vertex.Y, 0.5);

            //                               radius1
            //                         |--------------------
            //                         |                  /
            //                height1  |                 /
            //                         |     radius3    /
            //                        -|---------------/--------vertex
            //                height2  |              /distance1
            //                         |             /
            //                         |            /
            //                         |-----------/
            //                            radius2

            if (Math.Abs(vertex.Z) > height / 2 + GeometryModel.DefaultDistanceDelta)
                return false;

            float height1 = height / 2.0f - vertex.Z;
            float height2 = height - height1;
            float radius3;
            if (radius2 > radius1)
            {
                radius3 = (height2 / height) * (radius2 - radius1) + radius1;
            }
            else
                radius3 = (height1 / height) * (radius1 - radius2) + radius2;

            return distance1 < radius3 + GeometryModel.DefaultDistanceDelta;
        }
    }
}
