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
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;

namespace GEMS.Designer.Direct3D
{
    public abstract class GeometryRender : Direct3dRender
    {
        protected GeometryModel source = null;

        public GeometryRender(Direct3d d3d,GeometryModel source)
            :base(d3d)
        {
            this.source = source;
        }
        
        public abstract bool Intersect(Vector3 near, Vector3 far, out IntersectInformation closestHit);
   
        public static GeometryRender Create(Direct3d d3d, GeometryModel source)
        {
            GeometryRender render = null;

             //Create the geometry object based type
            switch (source.GetType().Name)
            {
                case "ConeModel":
                    render = new ConeRender(d3d, source);
                    break;
                case "CylinderModel":
                    render = new CylinderRender(d3d, source);
                    break;
                case "CuboidModel":
                    render = new CuboidRender(d3d, source);
                    break;
                case "LineModel":
                    render = new LineRender(d3d, source);
                    break;
                case "PointModel":
                    render = new PointRender(d3d, source);
                    break;
                case "RectangleModel":
                    render = new RectangleRender(d3d, source);
                    break;
                case "SphereModel":
                    render = new SphereRender(d3d, source);
                    break;
                case "RoundModel":
                    render = new RoundRender(d3d, source);
                    break;
                case "SingleEOSymbolModel":
                    render = new SingleEOSymbolRender(d3d, source);
                    break;
                default:
                    break;
            }

            if (render != null)
                render.Initialize ( );

            return render;
        }


        public List<Vector3> GetIntersectedVectors(Mesh sourceMeth, IntersectInformation[] hits)
        {
            List<Vector3> allHitPoints = new List<Vector3>();

            //Recover the vector information from the intersection information
            for (int index = 0; index < hits.Length; index++)
            {
                IntersectInformation hit = hits[index];
                Vector3 intersectVector = this.ComputeIntersectedVector(sourceMeth, hit);
                allHitPoints.Add(intersectVector);
            }

            return allHitPoints;
        }

        private Vector3 ComputeIntersectedVector(Mesh sourceMeth, IntersectInformation ii)
        {
            // create an array to hold the indices for the intersected face
            short[] intersectedIndices = new short[3];

            // fetch indices for the intersected face from the mesh
            short[] indices = (short[])sourceMeth.LockIndexBuffer(typeof(short),
                LockFlags.ReadOnly, sourceMeth.NumberFaces * 3);
            Array.Copy(indices, ii.FaceIndex * 3, intersectedIndices, 0, 3);
            sourceMeth.UnlockIndexBuffer();

            // create an array to hold the vertices for the intersected face
            Vector3[] intersectVertices = new Vector3[3];

            // extract vertex data from mesh, using our indices we obtained earlier
            Vector3[] meshVertices =
                (Vector3[])sourceMeth.LockVertexBuffer(typeof(Vector3), LockFlags.ReadOnly, sourceMeth.NumberVertices);
            intersectVertices[0] = meshVertices[intersectedIndices[0]];
            intersectVertices[1] = meshVertices[intersectedIndices[1]];
            intersectVertices[2] = meshVertices[intersectedIndices[2]];
            sourceMeth.UnlockVertexBuffer();

            //Compute the interset vector on the face
            Vector3 intersectVector = intersectVertices[0]
                                    + ii.U * (intersectVertices[1] - intersectVertices[0])
                                    + ii.V * (intersectVertices[2] - intersectVertices[0]);

            //Transform it to the world space
            intersectVector.TransformCoordinate(source.WorldMatrix);

            return intersectVector;
        }

    }
}
