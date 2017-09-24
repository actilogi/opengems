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
    public class GridRender : Direct3dRender
    {
        private AutoVertexBuffer gridVertexBuffer;
        private CustomVertex.PositionOnly[] verts;
        private int vertexCount = 0;
        private const int maxVertexCount = 2000;

        private GEMSEnvironment enviroment;

        private GridPlane gridPlane;
        private float gridOffset = 0.0f;
        private float gridSize = 0.2f;
        private bool gridDisplayed;
        private Camera camera;

        public GridRender ( Direct3d d3d , Camera camera , GEMSEnvironment enviroment )
            : base ( d3d )
        {
            this.enviroment = enviroment;
            this.gridDisplayed = enviroment.GridDisplayed;
            this.gridPlane = enviroment.GridPlane;
            this.gridOffset = enviroment.GidOffset;
            this.gridSize = enviroment.GridSize.Value;
            
            this.camera = camera;
            enviroment.GridOptionChanged += new GEMSEnvironment.GridOptionChangedEventHandler ( OnGridOptionChanged );
            camera.ViewChanged +=new Camera.ViewChangedEventHandler ( OnViewChanged );
            camera.ProjectionChanged += new Camera.ProjectionChangedEventHandler ( OnProjectionChanged );

            isDisplayed = IsDisplay (camera.Look);
        }

        void OnGridOptionChanged ( object sender , EventArgs e )
        {
            if (gridPlane != enviroment.GridPlane
                || gridOffset != enviroment.GidOffset
                || gridSize != enviroment.GridSize.ChangeUnit ( enviroment.DefaultLengthUnit ))
            {
                gridDisplayed = enviroment.GridDisplayed;
                gridPlane = enviroment.GridPlane;
                gridOffset = enviroment.GidOffset;
                gridSize = enviroment.GridSize.ChangeUnit ( enviroment.DefaultLengthUnit );

                //Re-generate  the grid
                Initialize ( );
            }
            else
            {
                gridDisplayed = enviroment.GridDisplayed;
                gridPlane = enviroment.GridPlane;
                gridOffset = enviroment.GidOffset;
                gridSize = enviroment.GridSize.ChangeUnit ( enviroment.DefaultLengthUnit );
            }

            isDisplayed = IsDisplay ( camera.Look );

        }

        public override void Initialize ( )
        {
            if (gridVertexBuffer != null)
            {
                gridVertexBuffer.Dispose ( );
                vertexCount = 0;
            }

            switch (gridPlane)
            {
                case GridPlane.XY:
                    GenerateXYGrid ( );
                    break;
                case GridPlane.XZ:
                    GenerateXZGrid ( );
                    break;
                case GridPlane.YZ:
                    GenerateYZGrid ( );
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Generate the grid along the XY plane
        /// </summary>
        private void GenerateXYGrid ( )
        {
            Vector3 leftTop;
            Vector3 leftBottom;
            Vector3 rightTop;
            Vector3 rightBottom;

            camera.ComputeGridRange (new Plane ( 0 , 0 , 1 , gridOffset ) 
                , out leftTop ,out leftBottom,out rightTop,out rightBottom);

            float maxY  = leftTop.Y;
            maxY = maxY > leftBottom.Y ? maxY : leftBottom.Y;
            maxY = maxY > rightTop.Y ? maxY : rightTop.Y;
            maxY = maxY > rightBottom.Y ? maxY : rightBottom.Y;

            float minY = leftTop.Y;
            minY = minY < leftBottom.Y ? minY : leftBottom.Y;
            minY = minY < rightTop.Y ? minY : rightTop.Y;
            minY = minY < rightBottom.Y ? minY : rightBottom.Y;

            float maxX  = leftTop.X;
            maxX = maxX > leftBottom.X ? maxX : leftBottom.X;
            maxX = maxX > rightTop.X ? maxX : rightTop.X;
            maxX = maxX > rightBottom.X ? maxX : rightBottom.X;

            float minX = leftTop.X;
            minX = minX < leftBottom.X ? minX : leftBottom.X;
            minX = minX < rightTop.X ? minX : rightTop.X;
            minX = minX < rightBottom.X ? minX : rightBottom.X;

            //Swap the four value to the interger multiple of gridSize
            maxX = gridSize * ( (int)( maxX / gridSize ) + 2 );
            minX = gridSize * ( (int)( minX / gridSize ) - 2 );
            maxY = gridSize * ( (int)( maxY / gridSize ) + 2 );
            minY = gridSize * ( (int)( minY / gridSize ) - 2 );            

            float width =  maxX - minX;
            float height = maxY - minY;

            if (width > 1e-6 && height > 1e-6)
            {
                //Compute the count of vertex
                int widthCount = (int)( width / gridSize ) + 2;
                int heightCount = (int)( height / gridSize ) + 2;        
                vertexCount = widthCount * 2 + heightCount * 2 ;

                if (vertexCount < maxVertexCount)
                {
                    //Create the vertex buffer
                    gridVertexBuffer = new AutoVertexBuffer ( d3d , typeof ( CustomVertex.PositionOnly ) , vertexCount ,
                         Usage.Dynamic , CustomVertex.PositionOnly.Format , Pool.Default );
                    verts = new CustomVertex.PositionOnly[vertexCount];

                    //Fill the data
                    int widthIndex = 0;
                    int index = 0;
                    for (index = 0 ; index < widthCount * 2 ; index++)
                    {
                        verts[index].Position = new Vector3 ( maxX - widthIndex * gridSize , maxY , gridOffset );
                        verts[++index].Position = new Vector3 ( maxX - widthIndex * gridSize , minY , gridOffset );

                        widthIndex++;
                    }
                    int heightIndex = 0;
                    for ( ; index < vertexCount ; index++)
                    {
                        verts[index].Position = new Vector3 ( maxX , maxY - heightIndex * gridSize , gridOffset );
                        verts[++index].Position = new Vector3 ( minX , maxY - heightIndex * gridSize , gridOffset );

                        heightIndex++;
                    }

                    gridVertexBuffer.VB.SetData ( verts , 0 , LockFlags.None );
                }
            }

        }

        /// <summary>
        /// Generate the grid along the XZ plane
        /// </summary>
        private void GenerateXZGrid ( )
        {
            Vector3 leftTop;
            Vector3 leftBottom;
            Vector3 rightTop;
            Vector3 rightBottom;

            camera.ComputeGridRange ( new Plane ( 0 , 1 , 0 , gridOffset )
                , out leftTop , out leftBottom , out rightTop , out rightBottom );

            float maxZ  = leftTop.Z;
            maxZ = maxZ > leftBottom.Z ? maxZ : leftBottom.Z;
            maxZ = maxZ > rightTop.Z ? maxZ : rightTop.Z;
            maxZ = maxZ > rightBottom.Z ? maxZ : rightBottom.Z;

            float minZ = leftTop.Z;
            minZ = minZ < leftBottom.Z ? minZ : leftBottom.Z;
            minZ = minZ < rightTop.Z ? minZ : rightTop.Z;
            minZ = minZ < rightBottom.Z ? minZ : rightBottom.Z;

            float maxX  = leftTop.X;
            maxX = maxX > leftBottom.X ? maxX : leftBottom.X;
            maxX = maxX > rightTop.X ? maxX : rightTop.X;
            maxX = maxX > rightBottom.X ? maxX : rightBottom.X;

            float minX = leftTop.X;
            minX = minX < leftBottom.X ? minX : leftBottom.X;
            minX = minX < rightTop.X ? minX : rightTop.X;
            minX = minX < rightBottom.X ? minX : rightBottom.X;

            //Swap the four value to the interger multiple of gridSize
            maxX = gridSize * ( (int)( maxX / gridSize ) + 2 );
            minX = gridSize * ( (int)( minX / gridSize ) - 2 );
            maxZ = gridSize * ( (int)( maxZ / gridSize ) + 2 );
            minZ = gridSize * ( (int)( minZ / gridSize ) - 2 );

            float width =  maxX - minX;
            float height = maxZ - minZ;

            if (width > 1e-6 && height > 1e-6)
            {
                //Compute the count of vertex
                int widthCount = (int)( width / gridSize ) + 2;
                int heightCount = (int)( height / gridSize ) + 2;
                vertexCount = widthCount * 2 + heightCount * 2;

                if (vertexCount < maxVertexCount)
                {
                    //Create the vertex buffer
                    gridVertexBuffer = new AutoVertexBuffer ( d3d , typeof ( CustomVertex.PositionOnly ) , vertexCount ,
                         Usage.Dynamic , CustomVertex.PositionOnly.Format , Pool.Default );
                    verts = new CustomVertex.PositionOnly[vertexCount];

                    int widthIndex = 0;
                    int index = 0;
                    for (index = 0 ; index < widthCount * 2 ; index++)
                    {
                        verts[index].Position = new Vector3 ( maxX - widthIndex * gridSize , gridOffset , maxZ );
                        verts[++index].Position = new Vector3 ( maxX - widthIndex * gridSize , gridOffset , minZ );

                        widthIndex++;
                    }

                    int heightIndex = 0;
                    for ( ; index < vertexCount ; index++)
                    {
                        verts[index].Position = new Vector3 ( maxX , gridOffset , maxZ - heightIndex * gridSize );
                        verts[++index].Position = new Vector3 ( minX , gridOffset , maxZ - heightIndex * gridSize );

                        heightIndex++;
                    }

                    gridVertexBuffer.VB.SetData ( verts , 0 , LockFlags.None );
                }
            }

        }

        /// <summary>
        /// Generate the grid along the YZ plane
        /// </summary>
        private void GenerateYZGrid ( )
        {
            Vector3 leftTop;
            Vector3 leftBottom;
            Vector3 rightTop;
            Vector3 rightBottom;

            camera.ComputeGridRange ( new Plane ( 1 , 0 , 0 , gridOffset )
                , out leftTop , out leftBottom , out rightTop , out rightBottom );

            float maxZ  = leftTop.Z;
            maxZ = maxZ > leftBottom.Z ? maxZ : leftBottom.Z;
            maxZ = maxZ > rightTop.Z ? maxZ : rightTop.Z;
            maxZ = maxZ > rightBottom.Z ? maxZ : rightBottom.Z;

            float minZ = leftTop.Z;
            minZ = minZ < leftBottom.Z ? minZ : leftBottom.Z;
            minZ = minZ < rightTop.Z ? minZ : rightTop.Z;
            minZ = minZ < rightBottom.Z ? minZ : rightBottom.Z;

            float maxY  = leftTop.Y;
            maxY = maxY > leftBottom.Y ? maxY : leftBottom.Y;
            maxY = maxY > rightTop.Y ? maxY : rightTop.Y;
            maxY = maxY > rightBottom.Y ? maxY : rightBottom.Y;

            float minY = leftTop.Y;
            minY = minY < leftBottom.Y ? minY : leftBottom.Y;
            minY = minY < rightTop.Y ? minY : rightTop.Y;
            minY = minY < rightBottom.Y ? minY : rightBottom.Y;

            //Swap the four value to the interger multiple of gridSize
            maxY = gridSize * ( (int)( maxY / gridSize ) + 2 );
            minY = gridSize * ( (int)( minY / gridSize ) - 2 );
            maxZ = gridSize * ( (int)( maxZ / gridSize ) + 2 );
            minZ = gridSize * ( (int)( minZ / gridSize ) - 2 );

            float width = maxY - minY;
            float height = maxZ - minZ;

            if (width > 1e-6 && height > 1e-6)
            {
                //Compute the count of vertex
                int widthCount = (int)( width / gridSize ) + 2;
                int heightCount = (int)( height / gridSize ) + 2;
                vertexCount = widthCount * 2 + heightCount * 2;

                if (vertexCount < maxVertexCount)
                {
                    //Create the vertex buffer
                    gridVertexBuffer = new AutoVertexBuffer ( d3d , typeof ( CustomVertex.PositionOnly ) , vertexCount ,
                         Usage.Dynamic , CustomVertex.PositionOnly.Format , Pool.Default );
                    verts = new CustomVertex.PositionOnly[vertexCount];

                    //Fill the data
                    int widthIndex = 0;
                    int index = 0;
                    for (index = 0 ; index < widthCount * 2 ; index++)
                    {
                        verts[index].Position = new Vector3 ( gridOffset , maxY - widthIndex * gridSize , maxZ );
                        verts[++index].Position = new Vector3 ( gridOffset , maxY -  widthIndex * gridSize , minZ );

                        widthIndex++;
                    }

                    int heightIndex = 0;
                    for ( ; index < vertexCount ; index++)
                    {
                        verts[index].Position = new Vector3 ( gridOffset , maxY , maxZ - heightIndex * gridSize );
                        verts[++index].Position = new Vector3 ( gridOffset , minY , maxZ - heightIndex * gridSize );

                        heightIndex++;
                    }

                    gridVertexBuffer.VB.SetData ( verts , 0 , LockFlags.None );
                }
            }
        }

        protected override void PerformRender ( )
        {
            if (gridVertexBuffer == null || vertexCount > maxVertexCount || vertexCount <= 0)
                return;

            d3d.Dx.Transform.World = Matrix.Identity;
            d3d.Dx.RenderState.AntiAliasedLineEnable = true;
            d3d.Dx.VertexFormat = gridVertexBuffer.VB.Description.VertexFormat;
            d3d.Dx.SetStreamSource ( 0 , gridVertexBuffer.VB , 0 );
            d3d.Dx.DrawPrimitives ( PrimitiveType.LineList , 0 , vertexCount / 2 );

        }
               

        public Vector3 ComputeNearestGridPoint ( Vector3 near , Vector3 far )
        {
            Vector3 gridPoint = Vector3.Empty;
            Vector3 intersectPoint = Vector3.Empty;
            Plane plane = Plane.Empty;

            switch (gridPlane)
            {
                case GridPlane.XY:
                    plane = new Plane ( 0 , 0 , 1 , gridOffset );
                    intersectPoint = Plane.IntersectLine ( plane , near , far );
                    gridPoint.X = ComputeNearestAxisValue ( intersectPoint.X );
                    gridPoint.Y = ComputeNearestAxisValue ( intersectPoint.Y );
                    gridPoint.Z = gridOffset;
                    break;
                case GridPlane.XZ:
                    plane = new Plane ( 0 , 1 , 0 , gridOffset );
                    intersectPoint = Plane.IntersectLine ( plane , near , far );
                    gridPoint.X = ComputeNearestAxisValue ( intersectPoint.X );
                    gridPoint.Y = gridOffset;
                    gridPoint.Z = ComputeNearestAxisValue ( intersectPoint.Z );
                    break;
                case GridPlane.YZ:
                    plane = new Plane ( 1 , 0 , 0 , gridOffset );
                    intersectPoint = Plane.IntersectLine ( plane , near , far );
                    gridPoint.X = gridOffset;
                    gridPoint.Y = ComputeNearestAxisValue ( intersectPoint.Y );
                    gridPoint.Z = ComputeNearestAxisValue ( intersectPoint.Z );
                    break;
                default:
                    break;
            }
            //Console.WriteLine ( "sectPt  : X = {0},Y = {1}, Z = {2}" , intersectPoint.X , intersectPoint.Y , intersectPoint.Z );

            return gridPoint;
        }

        private float ComputeNearestAxisValue ( float axisValue )
        {
            int gridCount = (int)Math.Abs ( axisValue / gridSize );

            float distance1 = Math.Abs ( gridCount * gridSize - Math.Abs ( axisValue ) );
            float distance2 = Math.Abs ( ( gridCount + 1 ) * gridSize - Math.Abs ( axisValue ) );

            if (distance1 > distance2)
            {
                return ( axisValue > 0 ? 1 : -1 ) * ( gridCount + 1 ) * gridSize;

            }
            else
            {
                return ( axisValue > 0 ? 1 : -1 ) * gridCount * gridSize;

            }
        }

        private void OnProjectionChanged ( object sender , EventArgs e )
        {
            Initialize ( );
        }

        private void OnViewChanged ( object sender , EventArgs e )
        {
            isDisplayed = IsDisplay ( camera.Look );

            if (isDisplayed)
                Initialize ( );
        }

        /// <summary>
        /// Ensure whether the grid is displayed based the angle 
        /// betweem camera's look vector and the normal of grid plane
        /// </summary>
        public bool IsDisplay ( Vector3 cameraLook )
        {
            cameraLook.Normalize ( );

            Vector3 intersectPoint = Vector3.Empty;
            float angle = 0.0f;

            switch (gridPlane)
            {
                case GridPlane.XY:
                    angle = (float)Math.Acos ( Math.Abs ( Vector3.Dot ( cameraLook , new Vector3 ( 0 , 0 , 1 ) ) ) );
                    break;
                case GridPlane.XZ:
                    angle = (float)Math.Acos ( Math.Abs ( Vector3.Dot ( cameraLook , new Vector3 ( 0 , 1 , 0 ) ) ) );
                    break;
                case GridPlane.YZ:
                    angle =(float)Math.Acos ( Math.Abs ( Vector3.Dot ( cameraLook , new Vector3 ( 1 , 0 , 0 ) ) ) );
                    break;
                default:
                    break;
            }

            return  gridDisplayed && /*angle > ( Math.PI / 2.0f *  0.1 ) &&*/ angle < ( Math.PI / 2.0f *  0.95 );
        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            enviroment.GridOptionChanged -= new GEMSEnvironment.GridOptionChangedEventHandler ( OnGridOptionChanged );
            camera.ViewChanged -=new Camera.ViewChangedEventHandler ( OnViewChanged );
            camera.ProjectionChanged -= new Camera.ProjectionChangedEventHandler ( OnProjectionChanged );

            gridVertexBuffer.Dispose ( );
            gridVertexBuffer = null;
        }
    }
}