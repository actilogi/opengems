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

using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using GEMS.Designer.Models.GeometryModels;
using GEMS.Designer.Models;
using System.Drawing;

namespace GEMS.Designer.Direct3D
{
    public class BoundingBoxRender : Direct3dRender    {

        private Vector3[] positions = new Vector3[16];

        private Line line;  //Draw device

        public BoundingBoxRender(Direct3d d3d, Vector3 minVector3, Vector3 maxVector3)
            :base(d3d)
        {
            base.minVector3 = minVector3;
            base.maxVector3 = maxVector3;

            line = new Line(d3d.Dx);
            line.Width = Direct3dRender.DefaultLineWidth;
        }

        public override void Initialize ( )
        {
            float boxWidth = maxVector3.X - minVector3.X;
            float boxDepth = maxVector3.Y - minVector3.Y;

            positions[0] = maxVector3;
            positions[1] = Vector3.Subtract(maxVector3, new Vector3(boxWidth, 0, 0));
            positions[2] = Vector3.Subtract(positions[1], new Vector3(0, boxDepth, 0));
            positions[3] = Vector3.Add(positions[2], new Vector3(boxWidth, 0, 0));
            positions[4] = Vector3.Add(minVector3, new Vector3(boxWidth, 0, 0));
            positions[5] = Vector3.Add(positions[4], new Vector3(0, boxDepth, 0));
            positions[6] = Vector3.Add(minVector3, new Vector3(0, boxDepth, 0));
            positions[7] = minVector3;   
            positions[8] = positions[4];
            positions[9] = positions[7];
            positions[10] = positions[2];
            positions[11] = positions[1];
            positions[12] = positions[6];
            positions[13] = positions[5];
            positions[14] = positions[0];
            positions[15] = positions[3];

        }

        protected override void PerformRender()
        {
            Matrix transformMatrix = d3d.View * d3d.Projection;

            line.Antialias = true;
            line.DrawTransform(positions, transformMatrix, Direct3dRender.DefaultLineColor);            
        }


        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            line.Dispose ( );
            positions = null;
        } 
    }
}
