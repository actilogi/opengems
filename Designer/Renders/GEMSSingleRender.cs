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
    public class GEMSSingleRenderComparer : IComparer<GEMSSingleRender>
    {
        #region IComparer<GEMSSingleRender> Members

        public int Compare ( GEMSSingleRender x , GEMSSingleRender y )
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    return x.DistanceToCamera.CompareTo ( y.DistanceToCamera );
                }
            }
        }

        #endregion
    }

    public class GEMSSingleRender : Direct3dRender
    {
        private GEMSSingle source;

        private GEMSProjectRender parentRender;
        private GeometryRender primaryModelRender;
        private SingleEOSymbolRender eoSymbolRender;

        private Material unSelectedMaterial; //Full transparent
        private Material selectedMaterial;   //Transparency changed by user       
        private float distanceToCamera = 0.0f;

        public GEMSSingleRender(Direct3d d3d,GEMSSingle source,GEMSProjectRender render)
            :base(d3d)
        {
            if(source != null)
            {
                this.source = source;
                this.source.GEMSSingle_DataChanged += new GEMSSingle.GEMSSingle_DataChangedEventHandler(GEMSSingle_DataChanged);
                this.parentRender = render;
            }
        }

        //Update the painter when correspounding single's data changed
        private void GEMSSingle_DataChanged(object sender, GEMSSingle.SingleDataChangedEventArgs e)
        {
            if (sender is GEMSSingle && (sender as GEMSSingle) == source)
            {
                switch (e.changedType)
                {
                    case GEMSSingle.SingleDataChangedEventArgs.DataChangeType.DisplayStyleChanged:
                        {
                            //Re-create all the material
                            CreateMaterial();
                        }
                        break;
                    case GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged:
                        {
                            //Re-create all the material
                            CreateMaterial();

                            //Re-create the eo single model
                            if (source.EOSymbolModel != null)
                            {
                                eoSymbolRender = new SingleEOSymbolRender ( d3d , source.EOSymbolModel );
                                eoSymbolRender.Initialize ( );
                            }
                            else
                            {
                                eoSymbolRender.Dispose ( );
                                eoSymbolRender = null;
                            }
                        }
                        break;
                    case GEMSSingle.SingleDataChangedEventArgs.DataChangeType.GeometryChanged:
                        {
                            //Re-create the primaryModelRender
                            if(source.PrimaryModel != null)
                                primaryModelRender = GeometryRender.Create(d3d, source.PrimaryModel);

                            if (source.EOSymbolModel != null)
                            {
                                eoSymbolRender = new SingleEOSymbolRender ( d3d , source.EOSymbolModel );
                                eoSymbolRender.Initialize ( );
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
                return;
        }

        protected override void PerformRender()
        {
            if (primaryModelRender == null)
                return;

            if (parentRender.CurrentMode != GEMSProjectRender.SceneMode.Modeling)
            {
                d3d.Dx.Material = unSelectedMaterial;
            }
            else
            {
                if (parentRender.SingleRenderMode == GEMSProjectRender.RenderMode.Modeling)
                {

                    if (source == source.Parent.CurrentSelectedObject
                || source.CreateOperation == source.Parent.CurrentSelectedObject)
                    {
                        d3d.Dx.Material = selectedMaterial;
                    }
                    else
                    {
                        d3d.Dx.Material = unSelectedMaterial;
                    }
                }
                else
                    d3d.Dx.Material = selectedMaterial;
            }

            //Render the primary geometry model
            if (primaryModelRender != null)
                primaryModelRender.Render();            

            //Render the eo symbol model
            if (eoSymbolRender != null)
                eoSymbolRender.Render();
        }

        public override void Initialize ( )
        {
            if (source.PrimaryModel != null)
            {
                primaryModelRender = GeometryRender.Create(d3d, source.PrimaryModel);

                if (source.EOSymbolModel != null)
                {
                    eoSymbolRender = new SingleEOSymbolRender(d3d, source.EOSymbolModel);
                    eoSymbolRender.Initialize ( );
                }
            }

            CreateMaterial();
        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            this.source.GEMSSingle_DataChanged -= new GEMSSingle.GEMSSingle_DataChangedEventHandler ( GEMSSingle_DataChanged );

            if (primaryModelRender != null)
                primaryModelRender.Dispose ( );

            if(eoSymbolRender != null)
                eoSymbolRender.Dispose ( );            
        } 


        private void CreateMaterial()
        {
            if (source.PrimaryModel != null)
            {
                //Create a material
                unSelectedMaterial = GraphicsUtility.InitMaterial(Color.FromArgb(60,source.PrimaryModel.ModelColor));

                selectedMaterial = GraphicsUtility.InitMaterial(source.PrimaryModel.ModelColor);
            }
        }      

        public bool Intersect(Vector3 near, Vector3 far, out IntersectInformation closestHit)
        {
            IntersectInformation primaryModelHitInfo;
            bool primaryModelHitResult = primaryModelRender.Intersect(near, far, out primaryModelHitInfo);            

            bool eoSymbolModelHitResult = false;
            IntersectInformation eoSymbolModelHitInfo = new IntersectInformation();
            if (eoSymbolRender != null)
                eoSymbolModelHitResult = eoSymbolRender.Intersect(near, far, out eoSymbolModelHitInfo);

            if (eoSymbolModelHitResult)
            {
                if (primaryModelHitResult)
                {
                    if (eoSymbolModelHitInfo.Dist > primaryModelHitInfo.Dist)
                        closestHit = primaryModelHitInfo;
                    else
                        closestHit = eoSymbolModelHitInfo;

                    return true;
                }
                else
                {
                    closestHit = eoSymbolModelHitInfo;
                    return true;
                }
            }
            else
            {
                closestHit = primaryModelHitInfo;
                return primaryModelHitResult;
            }
        }

        public void ComputeDistanceToCamera ( Vector3 camera )
        {
            Vector3 maxVector3 = primaryModelRender.MaxVector3;
            Vector3 minVector3 = primaryModelRender.MinVector3;

            List<Vector3> vectors = new List<Vector3> ( );

            //Get the eight coners of bounding box
            vectors.Add ( maxVector3 );
            vectors.Add ( minVector3 );
            vectors.Add ( new Vector3 ( maxVector3.X , maxVector3.Y , minVector3.Z ) );
            vectors.Add ( new Vector3 ( maxVector3.X , minVector3.Y , minVector3.Z ) );
            vectors.Add ( new Vector3 ( maxVector3.X , minVector3.Y , maxVector3.Z ) );
            vectors.Add ( new Vector3 ( minVector3.X , minVector3.Y , maxVector3.Z ) );
            vectors.Add ( new Vector3 ( minVector3.X , maxVector3.Y , maxVector3.Z ) );
            vectors.Add ( new Vector3 ( minVector3.X , maxVector3.Y , minVector3.Z ) );

            vectors[0] = Vector3.Project (vectors[0], d3d.Dx.Viewport , d3d.Projection , d3d.View , Matrix.Identity );

            double min = Math.Abs(vectors[0].Z);
            for (int i = 1 ; i < vectors.Count ; i++)
            {
                vectors[i] = Vector3.Project ( vectors[i],d3d.Dx.Viewport , d3d.Projection , d3d.View , Matrix.Identity );
                if (min > Math.Abs ( vectors[i].Z ))
                    min = Math.Abs ( vectors[i].Z );
            }

            this.distanceToCamera = (float)min;
        }

        #region Public Properties

        public GEMSSingle Source
        {
           get { return source; }
        }
      

        public float DistanceToCamera
        {
            get { return distanceToCamera; }
        }

        #endregion

    }
}
