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
    public class RoundModel : GeometryModel, IEOModel, ITwoDimensionEO
    {
        private float radius;
        private Vector3 centerVector3;
        private Axis alineAxis;

        private Vector3 eoStart;
        private Vector3 eoEnd;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
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

        public Vector3 EOStart
        {
            get { return eoStart; }
            set { eoStart = value; }
        }

        public Vector3 EOEnd
        {
            get { return eoEnd; }
            set { eoEnd = value; }
        }

    
        public override bool PositionRelation(Vector3 vertex)
        {
            //Transform the vertex to the object space
            Matrix inver = Matrix.Invert(worldMatrix);
            vertex.TransformCoordinate(inver);

            //Compute the position relation
            double positionToCenter = Math.Pow(vertex.X * vertex.X + vertex.Y * vertex.Y, 0.5);

            if (positionToCenter <= radius + GeometryModel.DefaultDistanceDelta
                && Math.Abs(vertex.Z) <= GeometryModel.DefaultDistanceDelta)
                return true;
            else
                return false;

        }

        public bool PositionRelationOneXYPlane(Vector3 vertex)
        {
            //Transform the vertex to the object space
            Matrix inver = Matrix.Invert(worldMatrix);
            vertex.TransformCoordinate(inver);

            //Compute the position relation
            double positionToCenter = Math.Pow(vertex.X * vertex.X + vertex.Y * vertex.Y, 0.5);

            if (positionToCenter <= radius + GeometryModel.DefaultDistanceDelta)
                return true;
            else
                return false;
        }
    }
}
