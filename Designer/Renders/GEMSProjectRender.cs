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
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Diagnostics;

using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryOperations;

namespace GEMS.Designer.Direct3D
{
    public class GEMSProjectRender : Direct3dRender
    {
        public enum SceneMode
        {
            Modeling ,
            Parallel,
            Preview
        }

        public enum RenderMode
        {
            //Under this mode, all the singles is opaque
            Viewing,

            //Under this mode, transparency of each single depend on : 
            //1. transparency of each single 
            //2. whether single is being selected
            //3. current mode of scene
            Modeling,  
        }

        //Data source
        private GEMSProject m_project;

        //Control member
        private bool isAxisRenderCreated;           //Whether create an axis system render
        private bool isParallelCreated;             //Whether create an parallel information render
        private bool isGridRenderCreated;           //Whether create an grid render 

        private SceneMode currentMode = SceneMode.Modeling;   //Scene mode

        private RenderMode renderMode = RenderMode.Modeling;  //Render single mode

        //Render member
        private AxisRender axisRender;              //Render axices
        private GridRender gridRender;              //Render a grid 
        private PointRender gridPointRender;        //Render a point on the grid
        private DomainRender domainRender;          //Render a bounding box indicating the simualtion domain
        private ParallelRender parallelRender;      //Render some bounding boxes indicating the parallel division information
        private List<GEMSSingleRender> singleRenders = new List<GEMSSingleRender> ( );

        //A Test painter to display all the mesh points with material distribution information
        private PointsRender meshRender;
        private Vector3 displayedGridPoint = Vector3.Empty;     //Current displayed point on the grid

        //Camera Members
        private static Vector3 DefaultCameraPosition = new Vector3 ( 1.0f , 1.0f , 1.0f );
        private static Vector3 DefaultCameraUpVector = new Vector3 ( .0f , .0f , 1.0f );//new Vector3(-1.0f, -1.0f, 2.0f);
        private static Vector3 Orgin = new Vector3 ( 0.0f , 0.0f , 0.0f );
        private Camera camera;

        //ViewPort Memebers
        bool isFit = false;

        #region Construction Methods

        public GEMSProjectRender ( Direct3d d3d )
            : base ( d3d )
        {
            d3d.DxRender3d +=new Direct3d.DxDirect3dDelegate ( d3d_DxRender3d );

            //Create a camera
            camera = new Camera ( d3d );
            camera.ViewChanged += new Camera.ViewChangedEventHandler ( PerformViewChanged );
        }

        public void Initialize ( GEMSProject project , SceneMode mode )
        {
            Reset ( );

            //Data source
            m_project = project;
            m_project.DataChanged += new GEMSProject.DataChangedEventHandler ( OnGEMSProjectDataChanged );

            currentMode = mode;

            //Create the renders
            Initialize ( );

        }

        /// <summary>
        /// Create all the renders and set the default render information
        /// </summary>
        public override void Initialize ( )
        {
            //Set the camera parameters
            camera.SetButtonMasks ( (int)MouseButtonMask.Left , (int)MouseButtonMask.Right );
            camera.SetViewParameters ( DefaultCameraPosition , Orgin , DefaultCameraUpVector );
            camera.SetProjectionParameters ( d3d.ClientSize.Width/ 250.0f , d3d.ClientSize.Height / 250.0f );

            //Set the mode
            switch (currentMode)
            {
                case SceneMode.Modeling:
                    this.isAxisRenderCreated = true;
                    this.isGridRenderCreated = true;
                    this.isParallelCreated = false;
                    break;
                case SceneMode.Parallel:
                    this.isAxisRenderCreated = false;
                    this.isGridRenderCreated = false;
                    this.isParallelCreated = true;
                    break;
                case SceneMode.Preview:
                    this.isAxisRenderCreated = true;
                    this.isGridRenderCreated = false;
                    this.isParallelCreated = false;
                    break;
                default:
                    break;
            }

            //Create all the renders
            CreateSingleRenders ( );
            CreateAxisRender ( );
            CreateGridRender ( );
            CreateParallelRender ( );
            CreateGridPointRender ( );

            //Let the render to compuate the suitably viewport 
            isFit = true;
        }

        private void Reset ( )
        {
            domainRender = null;
        }

        #endregion

        #region Render Create Methods

        /// <summary>
        /// Create single painters for each single object
        /// </summary>
        private void CreateSingleRenders ( )
        {

            foreach (GEMSSingleRender singleRender in singleRenders)
            {
                singleRender.Dispose ( );
            }

            singleRenders.Clear ( );

            if (m_project != null && m_project.Singles != null)
            {
                foreach (GEMSSingle single in m_project.Singles)
                {
                    GEMSSingleRender render = new GEMSSingleRender ( d3d , single ,this);
                    render.Initialize ( );
                    singleRenders.Add ( render );
                }

                GenerateBoundingBox ( );
            }
        }

        /// <summary>
        /// Create an axis painter
        /// </summary>
        private void CreateAxisRender ( )
        {
            if (axisRender != null)
                axisRender.Dispose ( );

            //Create the axis painter
            if (isAxisRenderCreated)
            {
                axisRender = new AxisRender ( d3d , camera);
                axisRender.Initialize ( );
            }


        }

        /// <summary>
        /// Create grid painter
        /// </summary>
        private void CreateGridRender ( )
        {
            if (gridRender != null)
                gridRender.Dispose ( );

            if (m_project != null && m_project.Environment != null)
            {
                //Create the grid painter
                if (isGridRenderCreated)
                {
                    gridRender = new GridRender ( d3d , camera , m_project.Environment );
                    gridRender.Initialize ( );
                }

            }
        }

        /// <summary>
        /// Create grid point painter
        /// </summary>
        private void CreateGridPointRender ( )
        {
            if (gridPointRender != null)
                gridPointRender.Dispose ( );

            gridPointRender = new PointRender ( d3d , displayedGridPoint , Color.Red );
            gridPointRender.Initialize ( );
            gridPointRender.IsDisplayed = false;
        }

        /// <summary>
        /// Create a parallel painter
        /// </summary>
        private void CreateParallelRender ( )
        {
            if (m_project != null
                && m_project.Parallel != null
                && m_project.Mesh != null)
            {
                //Create the parallel information painter
                if (isParallelCreated)
                {
                    parallelRender = new ParallelRender ( d3d , m_project.Parallel.Clone ( ) , m_project.Mesh );
                    parallelRender.Initialize ( );
                }
            }
        }

        #endregion

        #region 3D Scene Event Handler and Methods

        /// <summary>
        /// Draw the secne
        /// </summary>
        protected override void PerformRender ( )
        {
            if (axisRender != null)
            {
                axisRender.Render ( );
            }

            if (gridRender != null)
            {
                gridRender.Render ( );
            }

            if (parallelRender != null)
            {
                parallelRender.Render ( );
            }

            if (domainRender != null)
            {
                domainRender.Render ( );
            }

            if (meshRender != null)
            {
                meshRender.Render ( );
            }

            if (gridPointRender != null)
            {
                gridPointRender.Render ( );
            }


            d3d.Dx.RenderState.AlphaBlendEnable = true;
            d3d.Dx.RenderState.CullMode = Cull.None;

            d3d.Dx.RenderState.SourceBlend = Blend.SourceAlpha;
            d3d.Dx.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            //Render the single geometry with its ditance to the near plane of view from big to small 
            for (int i = singleRenders.Count - 1 ; i >= 0 ; i--)
            {
                singleRenders[i].Render ( );
            }

            d3d.Dx.RenderState.CullMode = Cull.CounterClockwise;
            d3d.Dx.RenderState.AlphaBlendEnable = false;
            

        }

        /// <summary>
        /// Occurs when it is time to render 3d objects.  
        /// </summary>
        private void d3d_DxRender3d ( Direct3d d3d , Device dx )
        {
            //Console.WriteLine ( "viewPortWidth = {0},viewPortHeight = {1}" , viewPortWidth , viewPortHeight );

            SetupLight ( );
            d3d.View        = camera.ViewMatrix;
            d3d.Projection  = camera.ProjectionMatrix;

            if (isFit )
            {
                ComputeFitViewPort ( );
                isFit = false;
            }

            if (m_project != null)
            {
                Render ( );
            }
        }

        /// <summary>
        /// Clean up any resources being used. 
        /// </summary>
        protected override void PerformDispose ( )
        {
            d3d.DxRender3d -=new Direct3d.DxDirect3dDelegate ( d3d_DxRender3d );
            camera.ViewChanged -= new Camera.ViewChangedEventHandler ( PerformViewChanged );
            m_project.DataChanged -= new GEMSProject.DataChangedEventHandler ( OnGEMSProjectDataChanged );

            if (axisRender != null)
            {
                axisRender.Dispose ( );
                axisRender = null;
            }

            if (gridRender != null)
            {
                gridRender.Dispose ( );
                gridRender = null;
            }

            if (parallelRender != null)
            {
                parallelRender.Dispose ( );
                parallelRender = null;
            }

            if (domainRender != null)
            {
                domainRender.Dispose ( );
                domainRender = null;
            }

            if (meshRender != null)
            {
                meshRender.Dispose ( );
                meshRender = null;
            }

            if (gridPointRender != null)
            {
                gridPointRender.Dispose ( );
                gridPointRender = null;
            }

            foreach (GEMSSingleRender singleRender in singleRenders)
            {
                singleRender.Dispose ( );
            }

            singleRenders.Clear ( );
            singleRenders = null;
        }

        #endregion

        #region Scene Management Methods

        /// <summary>
        /// Setup the light in the scene
        /// </summary>
        private void SetupLight ( )
        {
            d3d.Dx.Lights[0].Enabled = true;
            d3d.Dx.Lights[0].Type = LightType.Directional;
            d3d.Dx.Lights[0].Diffuse =  Color.White;
            d3d.Dx.Lights[0].Direction = Vector3.Normalize ( camera.Look + new Vector3 ( 0.1f , 0.1f , 0.1f ) );
            d3d.Dx.Lights[0].Update ( );

            d3d.Dx.Lights[1].Enabled = true;
            d3d.Dx.Lights[1].Type = LightType.Directional;
            d3d.Dx.Lights[1].Diffuse =  Color.White;
            d3d.Dx.Lights[1].Direction = Vector3.Normalize ( -camera.Look - new Vector3 ( 0.1f , 0.1f , 0.1f ) );
            d3d.Dx.Lights[1].Update ( );

            d3d.Dx.RenderState.Lighting = true;

        }

        /// <summary>
        /// Compute a bounding box which bounded all the GEMSSingle geometries
        /// </summary>
        private void GenerateBoundingBox ( )
        {
            int index = 0;
            foreach (GEMSSingleRender singleRender in singleRenders)
            {
                Vector3 singleMinVector3 = singleRender.Source.PrimaryModel.MinVector3;
                Vector3 singleMaxVector3 = singleRender.Source.PrimaryModel.MaxVector3;

                //Update the boundingbox of the single geometries
                if (index == 0)
                {
                    minVector3 = singleMinVector3;
                    maxVector3 = singleMaxVector3;
                }
                else
                {
                    if (minVector3.X > singleMinVector3.X)
                        minVector3.X = singleMinVector3.X;

                    if (minVector3.Y > singleMinVector3.Y)
                        minVector3.Y = singleMinVector3.Y;

                    if (minVector3.Z > singleMinVector3.Z)
                        minVector3.Z = singleMinVector3.Z;

                    if (maxVector3.X < singleMaxVector3.X)
                        maxVector3.X = singleMaxVector3.X;

                    if (maxVector3.Y < singleMaxVector3.Y)
                        maxVector3.Y = singleMaxVector3.Y;

                    if (maxVector3.Z < singleMaxVector3.Z)
                        maxVector3.Z = singleMaxVector3.Z;
                }

                index++;
            }
        }

        /// <summary>
        /// Compute the new width and height of viewport,
        /// with which all the geometries in the scene can be displayed suitably
        /// </summary>
        public void ComputeFitViewPort ( )
        {
            if (singleRenders.Count == 0)
                return;

            Matrix viewMatrix = d3d.View;
            Matrix projMatrix = Matrix.OrthoLH ( d3d.ClientSize.Width , d3d.ClientSize.Height
                , Camera.DefaultViewPortNearPlane , Camera.DefaultViewPortFarPlane );

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

            //Project them to the viewport 
            for (int i = 0 ; i < vectors.Count ; i++)
            {
                vectors[i] = Vector3.Project ( vectors[i] , d3d.Dx.Viewport , projMatrix , viewMatrix , Matrix.Identity );
            }

            //Compute the rectangle bounding these eight points
            float minX = vectors[0].X;
            float minY = vectors[0].Y;
            float maxX = vectors[0].X;
            float maxY = vectors[0].Y;
            foreach (Vector3 vector in vectors)
            {
                if (minX > vector.X) minX = vector.X;
                if (minY > vector.Y) minY = vector.Y;
                if (maxX < vector.X) maxX = vector.X;
                if (maxY < vector.Y) maxY = vector.Y;
            }

            //Compute the ratioes of new viewport size and old viewport size
            float ratio1 =  d3d.ClientSize.Height / (( maxY - minY ) * 1.1f);
            float ratio2 =  d3d.ClientSize.Width  / (( maxX - minX ) * 1.1f);

            //Select the better ratio
            float ratio = ratio1 > ratio2 ? ratio2 : ratio1;

            //Update the size of current viewport
            float newViewPortHeight = d3d.ClientSize.Height / ratio;
            float newViewPortWidth  =  d3d.ClientSize.Width / ratio;

            //Get the center of the rectangle
            float centerX = ( maxX - minX ) / 2 + minX;
            float centerY = ( maxY - minY ) / 2 + minY;

            //Generate a line in the object space 
            Vector3 near;
            Vector3 far;
            GenerateLine ( centerX , centerY , viewMatrix , projMatrix , out near , out far );

            //Move the camera to the point which is the intersect between the line and camera plane 
            camera.MoveCamera ( near , far );
            camera.SetProjectionParameters ( newViewPortWidth , newViewPortHeight );
        }

        /// <summary>
        /// Display the scene in the ISO View
        /// </summary>
        public void ResetView ( )
        {
            camera.SetViewParameters ( DefaultCameraPosition , Orgin , DefaultCameraUpVector );
            if (currentMode == SceneMode.Parallel)
                isFit = true;
        }

        /// <summary>
        /// Place the camera on the Z axis and display the scene one the XY plane
        /// </summary>
        public void SetXYView ( )
        {
            camera.SetViewParameters ( new Vector3 ( 0 , 0 , 1.0f ) , Orgin , new Vector3 ( 0.0f , 1.0f , 0.0f ) );
            if (currentMode == SceneMode.Parallel)
                isFit = true;
        }

        /// <summary>
        /// Place the camera on the X axis and display the scene one the YZ plane
        /// </summary>
        public void SetYZView ( )
        {
            camera.SetViewParameters ( new Vector3 ( 1.0f , 0 , 0 ) , Orgin , new Vector3 ( 0.0f , 0.0f , 1.0f ) );
            if (currentMode == SceneMode.Parallel)
                isFit = true;
        }

        /// <summary>
        /// Place the camera on the Y axis and display the scene one the ZX plane
        /// </summary>
        public void SetZXView ( )
        {
            camera.SetViewParameters ( new Vector3 ( 0 , 1.0f , 0 ) , Orgin , new Vector3 ( 1.0f , 0.0f , 0.0f ) );
            if (currentMode == SceneMode.Parallel)
                isFit = true;
        }

        /// <summary>
        /// Tell the render to compute the fit viewport for the scene
        /// </summary>
        public void SetFitView ( )
        {
            isFit = true;
        }

        /// <summary>
        /// Display the domain
        /// </summary>
        public void DisplayDomainRender ( )
        {
            if (domainRender == null)
            {
                this.domainRender = new DomainRender ( d3d , this.m_project.ComputationalDomain );
                this.domainRender.Initialize ( );
            }
            else
                this.domainRender.IsDisplayed = true;
        }

        /// <summary>
        /// Hide the domain
        /// </summary>
        public void UnDisplayDomainRender ( )
        {
            if (domainRender != null)
                this.domainRender.IsDisplayed = false;
        }

        public void UpdateGridPoint ( short mouseX , short mouseY )
        {
            Vector3 near , far;
            GenerateLine ( mouseX , mouseY , out near , out far );

            displayedGridPoint = gridRender.ComputeNearestGridPoint ( near , far );
            gridPointRender.UpdateDisplayedPoint ( displayedGridPoint );

           // Console.WriteLine ( "gridpt  : X = {0},Y = {1}, Z = {2}" , displayedGridPoint.X , displayedGridPoint.Y , displayedGridPoint.Z );

        }

        public void RenderMeshPoint ( List<CustomVertex.PositionColored> meshPoints )
        {
            meshRender = new PointsRender ( d3d , meshPoints );
            meshRender.Initialize ( );
        }

        #endregion

        #region Mouse and Key Event hanlder and Methods

        /// <summary>
        /// Picking a geometry in the scene with the near and far vector 
        /// </summary>
        public void PerformPicking ( MouseEventArgs e )
        {
            //Get the line
            Vector3 near , far;
            GenerateLine ( e.X , e.Y , out near , out far );

            //Do picking
            float distance = 0.0f;
            int index = 0;

            GEMSSingleRender pickedSingle = null;
            foreach (GEMSSingleRender render in singleRenders)
            {
                // Retrieve intersection information
                IntersectInformation closestIntersection;

                bool intersects = render.Intersect ( near , far , out closestIntersection );
                if (intersects)
                {
                    if (index == 0)
                    {
                        distance = closestIntersection.Dist;
                        pickedSingle = render;
                    }
                    else
                    {
                        if (distance > closestIntersection.Dist)
                        {
                            distance = closestIntersection.Dist;
                            pickedSingle = render;
                        }
                    }
                    index++;
                }
            }

            if (pickedSingle != null)
                m_project.CurrentSelectedObject = pickedSingle.Source;
            else
                m_project.CurrentSelectedObject = null;
        }

        /// <summary>
        /// Performing when user rolled the mouse wheel
        /// </summary>
        public void PerformMouseWheel ( MouseEventArgs e )
        {
            camera.OnMouseWheel ( e );
        }

        /// <summary>
        /// Performing when user pushed one mouse button
        /// </summary>
        public void PerformMouseDown ( MouseEventArgs e )
        {
            camera.OnMouseDown ( e );
        }

        /// <summary>
        /// Performing when user poped one mouse button
        /// </summary>
        public void PerformMouseUp ( MouseEventArgs e )
        {
            camera.OnMouseUp ( e );

        }

        /// <summary>
        /// Performing when user move the mouse
        /// </summary>
        public void PerformMouseMove ( MouseEventArgs e )
        {
            camera.OnMouseMove ( e );
        }      

        /// <summary>
        /// Performing when the view matrix is changed
        /// </summary>
        private void PerformViewChanged ( object sender , EventArgs e )
        {
            SortSingelRender ( );
        }

        /// <summary>
        /// Sort the single render with the distance from the single geometry to camera's eye
        /// </summary>
        private void SortSingelRender ( )
        {
            //Just compute the bounding box of each single geometry
            foreach (GEMSSingleRender singleRender in singleRenders)
            {
                singleRender.ComputeDistanceToCamera ( camera.Eye );
            }

            GEMSSingleRenderComparer singleComparer = new GEMSSingleRenderComparer ( );
            singleRenders.Sort ( singleComparer );
        }

        /// <summary>
        /// Generate a line whose start point is on the near plane of the viewing frustum,
        /// and the end point is on the far plane of the viewing frustum.
        /// </summary>
        private void GenerateLine ( float mouseX , float mouseY , out Vector3 near , out Vector3 far )
        {
            GenerateLine ( mouseX , mouseY , d3d.View , d3d.Projection , out near , out far );
        }

        /// <summary>
        /// Generate a line whose start point is on the near plane of the viewing frustum,
        /// and the end point is on the far plane of the viewing frustum.
        /// </summary>
        private void GenerateLine ( float mouseX , float mouseY , Matrix viewMatrix , Matrix projMatrix , out Vector3 near , out Vector3 far )
        {
            // Clamp mouse coordinates to viewport
            if (mouseX < 0) mouseX = 0;
            if (mouseY < 0) mouseY = 0;
            if (mouseX > d3d.Dx.Viewport.Width) mouseX = (short)d3d.Dx.Viewport.Width;
            if (mouseY > d3d.Dx.Viewport.Height) mouseY = (short)d3d.Dx.Viewport.Height;

            // Put mouse coordinates in screenspace Vector3's. These are the points
            // defining our ray for picking, which we'll transform back to world space
            near = new Vector3 ( mouseX , mouseY , 0 );
            far = new Vector3 ( mouseX , mouseY , 1 );

            // Transform points to world space
            near.Unproject ( d3d.Dx.Viewport , projMatrix , viewMatrix , Matrix.Identity );
            far.Unproject ( d3d.Dx.Viewport , projMatrix , viewMatrix , Matrix.Identity );
        }
       

        #endregion

        #region GEMSProject Event Handler

        void OnGEMSProjectDataChanged ( object sender , GEMSProject.DataChangedEventArgs e )
        {
            switch (e.changedType)
            {
                case GEMSProject.DataChangedEventArgs.DataChangeType.Initialized:
                    CreateSingleRenders ( );
                    CreateGridRender ( );
                    CreateParallelRender ( );
                    isFit = true;

                    break;
                default:
                    UpdateSingleRenders ( e );
                    break;
            }
        }

        private void UpdateSingleRenders ( GEMSProject.DataChangedEventArgs e )
        {
            if (e.changedSingle is GEMSSingle)
            {
                GEMSSingle changedSingle = e.changedSingle as GEMSSingle;

                switch (e.changedType)
                {
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SingleCutted:
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SingleDeleted:
                        {
                            //Delete one single painter
                            GEMSSingleRender targetSingleRender = null;
                            foreach (GEMSSingleRender singleRender in singleRenders)
                            {
                                if (singleRender.Source == changedSingle)
                                    targetSingleRender = singleRender;
                            }
                            this.singleRenders.Remove ( targetSingleRender );
                            targetSingleRender.Dispose ( );
                        }
                        break;
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SinglePasted:
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SingleCreated:
                        //Create one single painter
                        {
                            GEMSSingleRender newSingleRender = new GEMSSingleRender ( d3d , changedSingle,this );
                            newSingleRender.Initialize ( );
                            this.singleRenders.Add ( newSingleRender );

                        }
                        break;
                }
            }

            //Re-generate bounding box
            GenerateBoundingBox ( );

            if (currentMode == SceneMode.Preview)
                isFit = true;

        }

        #endregion

        #region Public Properties

        public SceneMode CurrentMode
        {
            get { return currentMode; }
        }

        public RenderMode SingleRenderMode
        {
            get { return renderMode; }
            set { renderMode = value; }
        }

        public ParallelRender ParallelRender
        {
            get { return parallelRender; }
        }

        public List<GEMSSingleRender> SingleRenders
        {
            get { return singleRenders; }
        }

        public GEMSProject Source
        {
            get { return m_project; }
        }

        public bool IsDisplayGridPoint
        {
            set { gridPointRender.IsDisplayed = value; }
        }

        public Vector3 DisplayedGridPoint
        {
            get
            {
                return displayedGridPoint;
            }
        }

        #endregion
    }
}

