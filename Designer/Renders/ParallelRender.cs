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

using GEMS.Designer.Models;

namespace GEMS.Designer.Direct3D
{
    public class ParallelRender : Direct3dRender
    {
        private List<BoundingBoxRender> boundingBoxRenders = new List<BoundingBoxRender>();
        private GEMSParallel parallel;
        private GEMSMesh mesh;

        public ParallelRender(Direct3d d3d, GEMSParallel parallel, GEMSMesh mesh)
            :base(d3d)
        {
            this.parallel = parallel;
            this.parallel.GEMSParallel_DataChanged += new GEMSParallel.GEMSParallel_DataChangedEventHandler(OnGEMSParallelDataChanged);

            this.mesh = mesh;
        }

        void OnGEMSParallelDataChanged(object sender, EventArgs e)
        {
            Initialize();
        }

        public override void Initialize()
        {
            boundingBoxRenders.Clear();

            foreach (GEMSParallelArea area in parallel.AreaList)
            {
                Vector3 minVector = Vector3.Empty;
                Vector3 maxVector = Vector3.Empty;

                if (mesh.MeshPointsInX.Count > 0)
                {
                    minVector.X = mesh.MeshPointsInX[area.Start.X];
                    maxVector.X = mesh.MeshPointsInX[area.End.X];
                }

                if(mesh.MeshPointsInY.Count > 0)
                {
                    minVector.Y = mesh.MeshPointsInY[area.Start.Y];
                    maxVector.Y = mesh.MeshPointsInY[area.End.Y];
                }

                if (mesh.MeshPointsInZ.Count > 0)
                {
                    minVector.Z = mesh.MeshPointsInZ[area.Start.Z];
                    maxVector.Z = mesh.MeshPointsInZ[area.End.Z];
                }

                BoundingBoxRender painter = new BoundingBoxRender(d3d, minVector, maxVector);
                painter.Initialize ( );

                this.boundingBoxRenders.Add(painter);
            }
        }

        protected override void PerformRender()
        {
            foreach (BoundingBoxRender boxRender in boundingBoxRenders)
            {
                boxRender.Render();
            }
        }


        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            this.parallel.GEMSParallel_DataChanged -= new GEMSParallel.GEMSParallel_DataChangedEventHandler ( OnGEMSParallelDataChanged );

            foreach (BoundingBoxRender boxRender in boundingBoxRenders)
            {
                boxRender.Dispose ( );
            }
        } 

        public GEMSParallel Parallel
        {
            get { return parallel; }
        }
    }
}
