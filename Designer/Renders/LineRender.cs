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
using System.Drawing;

using GEMS.Designer.Models.GeometryModels;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;

namespace GEMS.Designer.Direct3D
{
    public class LineRender : GeometryRender
    {
        protected Line line;
        private LineModel model;

        private List<Vector3> nodes = new List<Vector3>(); 

        public LineRender(Direct3d d3d, GeometryModel source)
            : base(d3d,source)
        {
            line = new Line(d3d.Dx);
            line.Width = Direct3dRender.DefaultLineWidth;
        }

        public override void Initialize ( )
        {
            nodes.Clear();

            model = source as LineModel;

            nodes.Add(model.Node1);
            nodes.Add(model.Node2);

            this.minVector3 = model.MinVector3;
            this.maxVector3 = model.MaxVector3;
        }

        protected override void PerformRender()
        {
            line.Antialias = true;

            d3d.Dx.Transform.World = Matrix.Identity;
            line.DrawTransform(nodes.ToArray(),d3d.View * d3d.Projection, this.source.ModelColor.ToArgb());
        }

        public override bool Intersect(Vector3 near, Vector3 far, out IntersectInformation closestHit)
        {
            //Get a clone of this point , because we'll changed its value
            Vector3 clone1 = model.Node1;
            Vector3 clone2 = model.Node2;

            float dist = 0f;            
            
            //Project the point to the viewport
            near.Project(d3d.Dx.Viewport, d3d.Projection, d3d.View, Matrix.Identity);
            far.Project(d3d.Dx.Viewport, d3d.Projection, d3d.View, Matrix.Identity);
            clone1.Project(d3d.Dx.Viewport, d3d.Projection, d3d.View, model.WorldMatrix);
            clone2.Project(d3d.Dx.Viewport, d3d.Projection, d3d.View, model.WorldMatrix);

            closestHit = new IntersectInformation();

            //These two values is used to detemine whether the near point is betweem the clone1 and clone2
            int value3 = (int)(clone1.X - near.X) * (int)(clone2.X - near.X);
            int value4 = (int)(clone1.Y - near.Y) * (int)(clone2.Y - near.Y);

            bool intersectResult = false;

            //These two values is used to detemine whether the near point is on the line of clone1 and clone2
            bool isAlongY = (int)Math.Abs((clone1.X - near.X)) <= 1 && (int)Math.Abs((clone2.X - near.X)) <= 1;
            bool isAlongX = (int)Math.Abs((clone1.Y - near.Y)) <= 1 && (int)Math.Abs((clone2.Y - near.Y)) <= 1;
            if(isAlongX && isAlongY)
            {
                intersectResult = true;
            }
            else if(isAlongX && !isAlongY)
            {
                intersectResult = value3 < 0;
            }
            else if(!isAlongX && isAlongY)
            {
                intersectResult =  value4 < 0;
            }
            else
            {
                int value1 = (int)((clone1.X - near.X) / (clone1.Y - near.Y));
                int value2 = (int)((near.X - clone2.X) / (near.Y - clone2.Y));

                if(value3 < 0 && value4 < 0 && Math.Abs(value1 - value2) <= 1)                
                {
                    intersectResult = true;
                }
            }

            if (intersectResult)
            {   
                //                               clone1.Z
                //point projected by clone1|--------------------clone1
                //                         |                  /
                //                   dis2  |                 /
                //                         |     dist       /
                //                        -|---------------/picked point on the line
                //                   dis1  |              /
                //                         |             /
                //                         |            /
                //point projected by clone2|-----------/clone2
                //                            clone2.Z

                float dis1 = (float)Math.Pow((clone1.X - near.X) * (clone1.X - near.X) + (clone1.Y - near.Y) * (clone1.Y - near.Y), 0.5);
                float dis2 = (float)Math.Pow((clone2.X - near.X) * (clone2.X - near.X) + (clone2.Y - near.Y) * (clone2.Y - near.Y), 0.5);

                if (clone1.Z > clone2.Z)
                {
                    dist = dis2 / (dis1 + dis2) * (clone1.Z - clone2.Z) + clone2.Z;                    
                }
                else
                {
                    dist = dis1 / (dis1 + dis2) * (clone2.Z - clone1.Z) + clone1.Z;
                }

                closestHit.Dist = dist;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            line.Dispose ( );
            model = null;
        }
    }
}
