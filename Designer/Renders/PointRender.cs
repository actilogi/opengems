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
using GEMS.Designer.Models.GeometryModels;
using GEMS.Designer.Models;

namespace GEMS.Designer.Direct3D
{
    public class PointRender : GeometryRender
    {
        private AutoVertexBuffer pointBuffer;
        private PointModel model;

        public PointRender(Direct3d d3d, GeometryModel source)
            : base(d3d, source)
        {
        }

        public PointRender(Direct3d d3d, Vector3 position,Color color)
            : base(d3d, null)
        {
            model = new PointModel();
            model.Position = position;
            model.ModelColor = GeometryRender.DefaultPointColor;

            base.source = model;

            Initialize();
        }

        public override void Initialize ( )
        {
            if (pointBuffer != null)
                pointBuffer.Dispose();

            model = source as PointModel;

             //Create the vertex buffer
            pointBuffer = new AutoVertexBuffer(d3d, typeof(CustomVertex.PositionNormal), 1,
                 Usage.Dynamic, CustomVertex.PositionNormal.Format, Pool.Default);

            CustomVertex.PositionNormal[] verts = new CustomVertex.PositionNormal[1];

            verts[0].Position = model.Position;
            verts[0].Normal = Direct3dRender.DefaultVertexNormal;            
            
            pointBuffer.VB.SetData(verts, 0, LockFlags.None);
        }

        protected override void PerformRender()
        {
            d3d.Dx.Transform.World = Matrix.Identity;
            d3d.Dx.RenderState.PointSize = Direct3dRender.DefaultPointWidth;
            
            d3d.Dx.VertexFormat = pointBuffer.VB.Description.VertexFormat;
            d3d.Dx.SetStreamSource(0, pointBuffer.VB, 0);
            d3d.Dx.DrawPrimitives(PrimitiveType.PointList, 0, 1);
        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            pointBuffer.Dispose ( );
            model = null;
        }

        public override bool Intersect(Vector3 near, Vector3 far, out IntersectInformation closestHit)
        {
            //Get a clone of this point , because we'll changed its value
            Vector3 clone = model.Position;

            //Project the point to the viewport
            near.Project(d3d.Dx.Viewport, d3d.Projection, d3d.View, Matrix.Identity);
            far.Project(d3d.Dx.Viewport, d3d.Projection, d3d.View, Matrix.Identity);
            clone.Project(d3d.Dx.Viewport, d3d.Projection, d3d.View, model.WorldMatrix);

            closestHit = new IntersectInformation();
            
            //Determine whether the two points is the same by a litte error
            if (Math.Abs(((short)clone.X - (short)near.X)) <= GeometryRender.DefaultPointWidth
                && (Math.Abs((short)clone.Y - (short)near.Y)) <= GeometryRender.DefaultPointWidth)
            {
                closestHit.Dist = clone.Z;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateDisplayedPoint(Vector3 newPoint)
        {
            CustomVertex.PositionNormalColored vertex = new CustomVertex.PositionNormalColored();

            model.Position = newPoint;

            vertex.Position = model.Position;
            vertex.Color = model.ModelColor.ToArgb();
            vertex.Normal = Direct3dRender.DefaultVertexNormal;
            pointBuffer.VB.SetData(vertex, 0, LockFlags.None);
        }

        
    }
}
