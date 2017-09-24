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
    public class AxisRender : Direct3dRender
    {
        protected Line lineRender;
        protected Camera camera;

        protected Vector3 origin = Vector3.Empty;

        protected List<Vector3> xAxis = new List<Vector3> ( );
        protected List<Vector3> yAxis = new List<Vector3> ( );
        protected List<Vector3> zAxis = new List<Vector3> ( );

        protected AutoMesh xAxisCone;
        protected Material xAxisConeMatrial;
        protected Matrix xAxisConeMatrix;

        protected AutoMesh yAxisCone;
        protected Material yAxisConeMatrial;
        protected Matrix yAxisConeMatrix;

        protected AutoMesh zAxisCone;
        protected Material zAxisConeMatrial;
        protected Matrix zAxisConeMatrix;

        private Vector3 xAxisText = Vector3.Empty;
        private Vector3 yAxisText = Vector3.Empty;
        private Vector3 zAxisText = Vector3.Empty;

        private Microsoft.DirectX.Direct3D.Font font = null;

        public Vector3 AxisOrigin
        {
            get { return origin; }
            set { origin = value; }
        }

        public AxisRender ( Direct3d d3d , Camera camera )
            : base ( d3d )
        {
            lineRender = new Line ( d3d.Dx );

            this.camera = camera;
            this.camera.ViewChanged += new Camera.ViewChangedEventHandler ( camera_ViewChanged );
            this.camera.ProjectionChanged += new Camera.ProjectionChangedEventHandler ( camera_ProjectionChanged );

            System.Drawing.Font localFont = new System.Drawing.Font ( "Verdana" , 10.0f , FontStyle.Bold );
            font = new Microsoft.DirectX.Direct3D.Font ( d3d.Dx , localFont );

            xAxisConeMatrial =  GraphicsUtility.InitMaterial ( Direct3dRender.DefaultXAxisColor );
            yAxisConeMatrial =  GraphicsUtility.InitMaterial ( Direct3dRender.DefaultYAxisColor );
            zAxisConeMatrial =  GraphicsUtility.InitMaterial ( Direct3dRender.DefaultZAxisColor );

        }

        void camera_ProjectionChanged ( object sender , EventArgs e )
        {
            Initialize ( );
        }

        void camera_ViewChanged ( object sender , EventArgs e )
        {
            Initialize ( );
        }


        public override void Initialize ( )
        {
            //Create a plane,on which camera 
            Plane cameraPlane = Plane.FromPointNormal ( camera.Eye * 2.0f , -camera.Look );

            //top left corner
            Vector3 topleftNear = new Vector3 ( 0 , 0 , 0 );
            Vector3 topleftFar = new Vector3 ( 0 , 0 , 1 );
            topleftNear.Unproject ( d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );
            topleftFar.Unproject ( d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );
            Vector3 topleft_cameraPlane_Intersect = Plane.IntersectLine ( cameraPlane , topleftNear , topleftFar );

            //bottom right
            Vector3 bottomrightNear = new Vector3 ( d3d.ClientSize.Width , d3d.ClientSize.Height , 0 );
            Vector3 bottomrightFar = new Vector3 ( d3d.ClientSize.Width , d3d.ClientSize.Height , 1 );
            bottomrightNear.Unproject ( d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );
            bottomrightFar.Unproject ( d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );
            Vector3 bottomright_cameraPlane_Intersect = Plane.IntersectLine ( cameraPlane , bottomrightNear , bottomrightFar );


            //Use two points to make four plane            
            List<Plane> planes = new List<Plane> ( );
            planes.Add ( cameraPlane );
            planes.Add ( Plane.FromPointNormal ( topleft_cameraPlane_Intersect , -camera.Right ) );
            planes.Add ( Plane.FromPointNormal ( topleft_cameraPlane_Intersect , camera.Up ) );
            planes.Add ( Plane.FromPointNormal ( bottomright_cameraPlane_Intersect , -camera.Up ) );
            planes.Add ( Plane.FromPointNormal ( bottomright_cameraPlane_Intersect , camera.Right ) );

            //Process the x axis
            Vector3 xAxisDirection = new Vector3 ( 1 , 0 , 0 );
            List<Vector3> xAxisIntersects = new List<Vector3> ( );

            // Console.WriteLine ( "NEW INTIALIZE!" );

            for (int i = 0 ; i < planes.Count ; i++)
            {
                if (Math.Abs ( Plane.DotNormal ( planes[i] , xAxisDirection ) ) <= 1e-6) //parallel 
                {
                    //Console.WriteLine ( "PLANE = {0} Parallel" , i );
                    continue;
                }

                Vector3 inter = Plane.IntersectLine ( planes[i] , origin , origin + new Vector3 ( 10000f , 0 , 0 ) );

                if (inter.X < 0)
                    continue;

                //Vector3 interProj = Vector3.Project ( inter , d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );

                //interProj must be in the range of the viewport
                // if (Math.Floor ( interProj.X ) + 1 >= 0 && Math.Floor ( interProj.X ) + 1 <= d3d.Dx.Viewport.Width 
                // && Math.Floor ( interProj.Y ) + 1 >= 0 && Math.Floor ( interProj.Y ) + 1 <= d3d.Dx.Viewport.Height)
                {
                    xAxisIntersects.Add ( inter );

                    //Console.WriteLine ( "PLANE = {3} , X = {0},Y = {1},Z = {2} , dot = {4}" , inter.X , inter.Y , inter.Z , i , Math.Abs ( Plane.DotNormal ( planes[i] , xAxisDirection ) ) );
                }
            }

            xAxisIntersects.Sort ( CompareVector3ByLength );

            //Process the y axis
            Vector3 yAxisDirection = new Vector3 ( 0 , 1 , 0 );
            List<Vector3> yAxisIntersects = new List<Vector3> ( );
            for (int i = 0 ; i < planes.Count ; i++)
            {
                if (Math.Abs ( Plane.DotNormal ( planes[i] , yAxisDirection ) ) <= 1e-6) //parallel 
                    continue;

                Vector3 inter = Plane.IntersectLine ( planes[i] , origin , origin + new Vector3 ( 0 , 10000f , 0 ) );

                if (inter.Y < 0)
                    continue;

                //Vector3 interProj = Vector3.Project ( inter , d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );

                //interProj must be in the range of the viewport
                // if (Math.Floor ( interProj.X ) + 1 >= 0 && Math.Floor ( interProj.X ) + 1 <= d3d.Dx.Viewport.Width 
                // && Math.Floor ( interProj.Y ) + 1 >= 0 && Math.Floor ( interProj.Y ) + 1 <= d3d.Dx.Viewport.Height)
                {
                    yAxisIntersects.Add ( inter );
                }
            }
            yAxisIntersects.Sort ( CompareVector3ByLength );

            //Process the z axis1
            Vector3 zAxisDirection = new Vector3 ( 0 , 0 , 1 );
            List<Vector3> zAxisIntersects = new List<Vector3> ( );
            for (int i = 0 ; i < planes.Count ; i++)
            {
                if (Math.Abs ( Plane.DotNormal ( planes[i] , zAxisDirection ) ) <= 1e-6) //parallel 
                    continue;

                Vector3 inter = Plane.IntersectLine ( planes[i] , origin , origin + new Vector3 ( 0 , 0 , 10000f ) );

                if (inter.Z < 0)
                    continue;

                //Vector3 interProj = Vector3.Project ( inter , d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );

                //interProj must be in the range of the viewport
                // if (Math.Floor ( interProj.X ) + 1 >= 0 && Math.Floor ( interProj.X ) + 1 <= d3d.Dx.Viewport.Width 
                // && Math.Floor ( interProj.Y ) + 1 >= 0 && Math.Floor ( interProj.Y ) + 1 <= d3d.Dx.Viewport.Height)
                {
                    zAxisIntersects.Add ( inter );
                }
            }
            zAxisIntersects.Sort ( CompareVector3ByLength );

            //Process the origin
            Vector3 originProj = Vector3.Project ( origin , d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );

            xAxis.Clear ( );
            yAxis.Clear ( );
            zAxis.Clear ( );

            if (xAxisCone != null)
            {
                xAxisCone.Dispose ( );
                xAxisCone = null;
            }

            if (yAxisCone != null)
            {
                yAxisCone.Dispose ( );
                yAxisCone = null;
            }

            if (zAxisCone != null)
            {
                zAxisCone.Dispose ( );
                zAxisCone = null;
            }

            if (originProj.X >= 0 && originProj.X <= d3d.Dx.Viewport.Width 
                && originProj.Y >= 0 && originProj.Y <= d3d.Dx.Viewport.Height)
            {
                //Origin is in the range of viewport
                //X Axis
                if (xAxisIntersects.Count >= 1)
                {
                    float xAxisLength = Vector3.Subtract ( xAxisIntersects[0] , origin ).Length ( );

                    xAxis.Add ( origin );
                    xAxis.Add ( origin + new Vector3 ( xAxisLength * 0.9f , 0 , 0 ) );

                }

                //Y Axis
                if (yAxisIntersects.Count >= 1)
                {
                    float yAxisLength = Vector3.Subtract ( yAxisIntersects[0] , origin ).Length ( );

                    yAxis.Add ( origin );
                    yAxis.Add ( origin + new Vector3 ( 0 , yAxisLength * 0.9f , 0 ) );

                }

                //Z Axis
                if (zAxisIntersects.Count >= 1)
                {
                    float zAxisLength = Vector3.Subtract ( zAxisIntersects[0] , origin ).Length ( );
                    zAxis.Add ( origin );
                    zAxis.Add ( origin + new Vector3 ( 0 , 0 , zAxisLength * 0.9f ) );
                }

            }
            else
            {
                //origin is out of the range of viewport
                //X Axis                
                if (xAxisIntersects.Count >= 2)
                {
                    float xAxisLength = Vector3.Subtract ( xAxisIntersects[1] , xAxisIntersects[0] ).Length ( );

                    xAxis.Add ( xAxisIntersects[0] );
                    xAxis.Add ( xAxisIntersects[0]  + new Vector3 ( xAxisLength * 0.9f , 0 , 0 ) );
                }

                //Y Axis                
                if (yAxisIntersects.Count >= 2)
                {
                    float yAxisLength = Vector3.Subtract ( yAxisIntersects[1] , yAxisIntersects[0] ).Length ( );

                    yAxis.Add ( yAxisIntersects[0] );
                    yAxis.Add ( yAxisIntersects[0] + new Vector3 ( 0 , yAxisLength * 0.9f , 0 ) );
                }

                //Z Axis                
                if (zAxisIntersects.Count >= 2)
                {
                    float zAxisLength = Vector3.Subtract ( zAxisIntersects[1] , zAxisIntersects[0] ).Length ( );

                    zAxis.Add ( zAxisIntersects[0] );
                    zAxis.Add ( zAxisIntersects[0] + new Vector3 ( 0 , 0 , zAxisLength * 0.9f ) );
                }
            }

            if (xAxis.Count == 2)
            {
                float radius = camera.ViewportWidth * 0.008f;
                xAxisCone = new AutoMesh ( d3d , Mesh.Cylinder ( d3d.Dx , radius , 0 , radius * 3 , Direct3dRender.DefaultSlices , Direct3dRender.DefaultStacks ) );
                xAxisConeMatrix = Matrix.RotationY ( (float)Math.PI / 2 ) * Matrix.Translation ( xAxis[1] + new Vector3 ( radius * 1.5f , 0 , 0 ) );

                xAxisText = xAxis[1] + new Vector3 ( 0 , radius * 2.0f , 0 );
                xAxisText.Project ( d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );
            }

            if (yAxis.Count == 2)
            {
                float radius = camera.ViewportWidth * 0.008f;
                yAxisCone = new AutoMesh ( d3d , Mesh.Cylinder ( d3d.Dx , radius , 0 , radius * 3 , Direct3dRender.DefaultSlices , Direct3dRender.DefaultStacks ) );
                yAxisConeMatrix = Matrix.RotationX ( -(float)Math.PI / 2 ) * Matrix.Translation ( yAxis[1] + new Vector3 ( 0 , radius * 1.5f , 0 ) );

                yAxisText = yAxis[1] + new Vector3 ( radius * 2.0f , 0 , 0 );
                yAxisText.Project ( d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );

            }

            if (zAxis.Count == 2)
            {
                float radius = camera.ViewportWidth * 0.008f;
                zAxisCone = new AutoMesh ( d3d , Mesh.Cylinder ( d3d.Dx , radius , 0 , radius * 3 , Direct3dRender.DefaultSlices , Direct3dRender.DefaultStacks ) );
                zAxisConeMatrix = Matrix.Translation ( zAxis[1] + new Vector3 ( 0 , 0 , radius * 1.5f ) );

                zAxisText = zAxis[1] + new Vector3 ( radius * 2.0f , 0 , 0 );
                zAxisText.Project ( d3d.Dx.Viewport , camera.ProjectionMatrix , camera.ViewMatrix , Matrix.Identity );

            }
        }

        protected override void PerformRender ( )
        {
            Matrix transformMatrix =  d3d.View * d3d.Projection;

            d3d.Dx.Transform.World = Matrix.Translation ( origin );

            lineRender.Antialias = true;
            lineRender.Width = Direct3dRender.DefaultLineWidth;

            if (xAxis.Count == 2)
            {
                lineRender.DrawTransform ( xAxis.ToArray ( ) , transformMatrix , Direct3dRender.DefaultXAxisColor );
                font.DrawText ( null , "X" , (int)xAxisText.X , (int)xAxisText.Y , Direct3dRender.DefaultXAxisColor );


                if (xAxisCone != null)
                {
                    d3d.Dx.Transform.World = xAxisConeMatrix;
                    d3d.Dx.Material = xAxisConeMatrial;

                    xAxisCone.M.DrawSubset ( 0 );

                    d3d.Dx.Transform.World = Matrix.Identity;
                }

            }

            if (yAxis.Count == 2)
            {
                lineRender.DrawTransform ( yAxis.ToArray ( ) , transformMatrix , Direct3dRender.DefaultYAxisColor );
                font.DrawText ( null , "Y" , (int)yAxisText.X , (int)yAxisText.Y , Direct3dRender.DefaultYAxisColor );

                if (yAxisCone != null)
                {
                    d3d.Dx.Transform.World = yAxisConeMatrix;
                    d3d.Dx.Material = yAxisConeMatrial;

                    yAxisCone.M.DrawSubset ( 0 );

                    d3d.Dx.Transform.World = Matrix.Identity;
                }
            }

            if (zAxis.Count == 2)
            {
                lineRender.DrawTransform ( zAxis.ToArray ( ) , transformMatrix , Direct3dRender.DefaultZAxisColor );
                font.DrawText ( null , "Z" , (int)zAxisText.X , (int)zAxisText.Y , Direct3dRender.DefaultZAxisColor );

                if (zAxisCone != null)
                {
                    d3d.Dx.Transform.World = zAxisConeMatrix;
                    d3d.Dx.Material = zAxisConeMatrial;

                    zAxisCone.M.DrawSubset ( 0 );

                    d3d.Dx.Transform.World = Matrix.Identity;
                }
            }

        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            if (lineRender != null)
                lineRender.Dispose ( );

            if (xAxisCone != null)
            {
                xAxisCone.Dispose ( );               
                xAxisCone = null;
            }

            if (yAxisCone != null)
            {
                yAxisCone.Dispose ( );
                yAxisCone = null;
            }

            if (zAxisCone != null)
            {
                zAxisCone.Dispose ( );
                zAxisCone = null;
            }
        }


        private static int CompareVector3ByLength ( Vector3 x , Vector3 y )
        {

            return x.Length ( ).CompareTo ( y.Length ( ) );

        }


    }
}
