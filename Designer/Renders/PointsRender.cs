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
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;

namespace GEMS.Designer.Direct3D
{
    public class PointsRender : Direct3dRender
    {
        private const int defaultPointWidth = 5;
        private readonly int defaultPointColor = Color.Black.ToArgb();

        private AutoVertexBuffer pointBuffer;
        private List<CustomVertex.PositionColored> positions;

        public PointsRender(Direct3d d3d, List<CustomVertex.PositionColored> positions)
            : base(d3d)
        {
            this.positions = positions;
        }

        public override void Initialize ( )
        {
            if (pointBuffer != null)
                pointBuffer.Dispose();

             //Create the vertex buffer
            pointBuffer = new AutoVertexBuffer(d3d, typeof(CustomVertex.PositionNormalColored), positions.Count,
                 Usage.Dynamic, CustomVertex.PositionNormalColored.Format, Pool.Default);

            CustomVertex.PositionNormalColored[] verts = new CustomVertex.PositionNormalColored[positions.Count];

            int index = 0;
            foreach (CustomVertex.PositionColored vertex in positions)
            {
                verts[index].Position = vertex.Position;
                verts[index].Color = vertex.Color;
                verts[index].Normal = new Vector3(0, 0, 1);

                index++;
             }              
            
            pointBuffer.VB.SetData(verts, 0, LockFlags.None);

        }
        

        protected override void PerformRender()
        {
            d3d.Dx.Transform.World = Matrix.Identity;
            d3d.Dx.RenderState.PointSize = defaultPointWidth;


            d3d.Dx.VertexFormat = pointBuffer.VB.Description.VertexFormat;
            d3d.Dx.SetStreamSource(0, pointBuffer.VB, 0);
            d3d.Dx.DrawPrimitives(PrimitiveType.PointList, 0, positions.Count);
        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            pointBuffer.Dispose ( );
            positions.Clear ( );
            positions = null;
        }

    }
}
