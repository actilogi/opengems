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
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;
using GEMS.Designer.Utility;

namespace GEMS.Designer.Direct3D
{
    public class RectangleRender : GeometryRender
    {
        private AutoMesh rectangleMesh;
        private RectangleModel model;

        public RectangleRender(Direct3d d3d, GeometryModel source)
            : base(d3d, source)
        {
        }

        /// <summary>
        /// Create the primitives to be drawed
        /// X ---- Width
        /// Z ---- Height
        /// </summary>
        public override void Initialize ( )
        {
            if (rectangleMesh != null)
                rectangleMesh.Dispose();

            model = base.source as RectangleModel;            
            
            //Create the mesh
            rectangleMesh = new AutoMesh(d3d, Mesh.Box(d3d.Dx, model.Width, model.Height, 0.0f));

            //Get the bounding box
            rectangleMesh.BoundingBox(out minVector3, out maxVector3);

            minVector3.TransformCoordinate(model.WorldMatrix);
            maxVector3.TransformCoordinate(model.WorldMatrix);

            Vector3 minClone = minVector3;
            Vector3 maxClone = maxVector3;

            minVector3.X = minClone.X < maxClone.X ? minClone.X : maxClone.X;
            minVector3.Y = minClone.Y < maxClone.Y ? minClone.Y : maxClone.Y;
            minVector3.Z = minClone.Z < maxClone.Z ? minClone.Z : maxClone.Z;
            maxVector3.X = minClone.X < maxClone.X ? maxClone.X : minClone.X;
            maxVector3.Y = minClone.Y < maxClone.Y ? maxClone.Y : minClone.Y;
            maxVector3.Z = minClone.Z < maxClone.Z ? maxClone.Z : minClone.Z;

            model.MaxVector3 = maxVector3;
            model.MinVector3 = minVector3;
        }
        
        /// <summary>
        /// Draw the primitives 
        /// </summary>
        protected override void PerformRender()
        {
            d3d.Dx.Transform.World = model.WorldMatrix;
            rectangleMesh.M.DrawSubset(0);
        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            rectangleMesh.Dispose ( );
            model = null;
        }

        public override bool Intersect(Vector3 near, Vector3 far, out IntersectInformation closestHit)
        {
            //Copy the vector
            Vector3 near1 = near;
            Vector3 far1 = far;

            Matrix inver = Matrix.Invert(model.WorldMatrix);
            near.TransformCoordinate(inver);
            far.TransformCoordinate(inver);

            return rectangleMesh.M.Intersect(near, far - near, out closestHit);
        }      
    }
}
