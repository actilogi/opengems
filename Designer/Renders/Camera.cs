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
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace GEMS.Designer.Direct3D
{
    #region Arcball
    /// <summary>
    /// Class holds arcball data
    /// </summary>
    public class ArcBall
    {
        #region Instance Data
        protected Matrix rotation; // Matrix for arc ball's orientation
        protected Matrix translation; // Matrix for arc ball's position
        protected Matrix translationDelta; // Matrix for arc ball's position

        protected int width; // arc ball's window width
        protected int height; // arc ball's window height
        protected Vector2 center;  // center of arc ball 
        protected float radius; // arc ball's radius in screen coords
        protected float radiusTranslation; // arc ball's radius for translating the target

        protected Quaternion downQuat; // Quaternion before button down
        protected Quaternion nowQuat; // Composite quaternion for current drag
        protected bool isDragging; // Whether user is dragging arc ball

        protected System.Drawing.Point lastMousePosition; // position of last mouse point
        protected Vector3 downPt; // starting point of rotation arc
        protected Vector3 currentPt; // current point of rotation arc
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the rotation matrix
        /// </summary>
        public Matrix RotationMatrix 
        { 
            get 
            { 
                return rotation = Matrix.RotationQuaternion ( nowQuat ); 
            }
        }

        /// <summary>
        /// Gets the translation matrix
        /// </summary>
        public Matrix TranslationMatrix 
        { 
            get 
            { 
                return translation; 
            } 
        }

        /// <summary>
        /// Gets the translation delta matrix
        /// </summary>
        public Matrix TranslationDeltaMatrix 
        { 
            get 
            { 
                return translationDelta; 
            }
        }

        /// <summary>
        /// Gets the dragging state
        /// </summary>
        public bool IsBeingDragged 
        { 
            get 
            { 
                return isDragging; 
            } 
        }

        /// <summary>Gets or sets the current quaternion</summary>
        public Quaternion CurrentQuaternion 
        { 
            get 
            { 
                return nowQuat; 
            } 
            set 
            { 
                nowQuat = value; 
            } 
        }

        #endregion      

        /// <summary>
        /// Create new instance of the arcball class
        /// </summary>
        public ArcBall ( )
        {
            Reset ( );
            downPt = Vector3.Empty;
            currentPt = Vector3.Empty;

            System.Windows.Forms.Form active = System.Windows.Forms.Form.ActiveForm;
            if (active != null)
            {
                System.Drawing.Rectangle rect = active.ClientRectangle;
                SetWindow ( rect.Width , rect.Height );
            }
        }

        /// <summary>
        /// Resets the arcball
        /// </summary>
        public void Reset ( )
        {
            downQuat = Quaternion.Identity;
            nowQuat = Quaternion.Identity;
            rotation = Matrix.Identity;
            translation = Matrix.Identity;
            translationDelta = Matrix.Identity;
            isDragging = false;
            radius = 1.0f;
            radiusTranslation = 1.0f;
        }

        /// <summary>
        /// Convert a screen point to a vector
        /// </summary>
        public Vector3 ScreenToVector ( float screenPointX , float screenPointY )
        {
            float x = -( screenPointX - width / 2.0f ) / ( radius * width / 2.0f );
            float y = ( screenPointY - height / 2.0f ) / ( radius * height / 2.0f );
            float z = 0.0f;
            float mag = ( x * x ) + ( y * y );

            if (mag > 1.0f)
            {
                float scale = 1.0f / (float)Math.Sqrt ( mag );
                x *= scale;
                y *= scale;
            }
            else
                z = (float)Math.Sqrt ( 1.0f - mag );

            return new Vector3 ( x , y , z );
        }

        /// <summary>
        /// Set window paramters
        /// </summary>
        public void SetWindow ( int w , int h , float r )
        {
            width = w; height = h; radius = r;
            center = new Vector2 ( w / 2.0f , h / 2.0f );
        }
        public void SetWindow ( int w , int h )
        {
            SetWindow ( w , h , 1.0f ); // default radius
        }

        /// <summary>
        /// Computes a quaternion from ball points
        /// </summary>
        public static Quaternion QuaternionFromBallPoints ( Vector3 from , Vector3 to)
        {
            float dot = Vector3.Dot ( from , to );
            Vector3 part = Vector3.Cross ( from , to );
            return new Quaternion ( part.X , part.Y , part.Z , dot );
        }

        /// <summary>
        /// Begin the arcball 'dragging'
        /// </summary>
        public void OnBegin ( int x , int y )
        {
            isDragging = true;
            downQuat = nowQuat;
            downPt = ScreenToVector ( (float)x , (float)y );
        }
        /// <summary>
        /// The arcball is 'moving'
        /// </summary>
        public void OnMove ( int x , int y )
        {
            if (isDragging)
            {
                currentPt = ScreenToVector ( (float)x , (float)y );
                nowQuat = downQuat * QuaternionFromBallPoints ( downPt , currentPt );
            }
        }
        /// <summary>
        /// Done dragging the arcball
        /// </summary>
        public void OnEnd ( )
        {
            isDragging = false;
        }

    }
    #endregion

    /// <summary>
    /// Mouse button mask values
    /// </summary>
    [Flags]
    public enum MouseButtonMask : byte
    {
        None=0 ,
        Left=0x01 ,
        Middle=0x02 ,
        Right=0x04 ,
        Wheel=0x08 ,
    }

    public class Camera
    {

        private Direct3d d3d;
        private bool dxLost = false;
        private Matrix viewMatrix; // View Matrix
        private Matrix projMatrix; // Projection matrix

        private Vector3 eye;                 // Camera position
        private Vector3 lookAt;              // Camera LookAt position
        private Vector3 up;                  // world Up direction
        private Vector3 look;                // Camera Look direction        

        //ViewPort Memebers
        private float viewportHeight;
        private float viewportWidth;
        public const float DefaultViewPortNearPlane = -1000.0f;
        public const float DefaultViewPortFarPlane = 1000.0f;

        private int rotateCameraButtonMask;
        private int moveCameraButtonMask;

        private bool isDragging = false;       //Whether user started dragging the mouse with one mouse button down
        private Vector3 lastMousePoint;        //The point on the camera plane of the last mouse point on the screen
        protected ArcBall viewArcball = new ArcBall ( );

        private static readonly Vector3 DefaultEye = new Vector3 ( 1.0f , 1.0f , 1.0f );
        private static readonly Vector3 DefaultLookAt = Vector3.Empty;
        private static readonly Vector3 AxisX = new Vector3 ( 1.0f , 0.0f , 0.0f );
        private static readonly Vector3 AxisY = new Vector3 ( 0.0f , 1.0f , 0.0f );
        private static readonly Vector3 AxisZ = new Vector3 ( 0.0f , 0.0f , 1.0f );

        #region Public Properties

        /// <summary>
        /// Returns the view transformation matrix
        /// </summary>
        public Matrix ViewMatrix 
        {
            get 
            { 
                return viewMatrix; 
            } 
        }

        /// <summary>
        /// Returns the projection transformation matrix
        /// </summary>
        public Matrix ProjectionMatrix 
        { 
            get 
            { 
                return projMatrix; 
            } 
        }

        /// <summary>
        /// Get the camera's look direction vector
        /// </summary>
        public Vector3 Look
        {
            get 
            {
                return look;
            }
        }

        /// <summary>
        /// Get the camera's look target
        /// </summary>
        public Vector3 LookAt
        {
            get
            {
                return lookAt;
            }
        }

        /// <summary>
        /// Get the camera's position
        /// </summary>
        public Vector3 Eye
        {
            get
            {
                return eye;
            }
        }

        /// <summary>
        /// Get the camera's up direction
        /// </summary>
        public Vector3 Up
        {
            get
            {
                return up;
            }
        }

        /// <summary>
        /// Get the camera's right direction
        /// </summary>
        public Vector3 Right
        {
            get
            {
                Vector3 right =  Vector3.Cross ( up , look );
                right.Normalize ( );

                return right;
            }
        }

        /// <summary>
        /// Get the view port height
        /// </summary>
        public float ViewportHeight
        {
            get
            {
                return viewportHeight;
            }
        }

        /// <summary>
        /// Get the view port width
        /// </summary>
        public float ViewportWidth
        {
            get
            {
                return viewportWidth;
            }
        }

        #endregion

        #region Delagate and Event Declarations        

        public delegate void ViewChangedEventHandler ( object sender , EventArgs e );
        public delegate void ProjectionChangedEventHandler ( object sender , EventArgs e );

        //Raised when view matrix is changed ,such as camera'eye is changed
        public virtual event ViewChangedEventHandler ViewChanged;

        //Raised when projection matrix is changed ,such as windows resized or zoomed
        public virtual event ProjectionChangedEventHandler ProjectionChanged;

        public void ViewChangedAlarm ()
        {
            if (ViewChanged != null)
            {
                ViewChanged ( this , EventArgs.Empty );
            }
        }

        public void ProjectionChangedAlarm ( )
        {
            if (ProjectionChanged != null)
            {
                ProjectionChanged ( this , EventArgs.Empty );
            }
        }

        #endregion

    
        public Camera ( Direct3d d3d )
        {
            this.d3d = d3d;
            this.d3d.Resize +=new EventHandler ( OnD3dResize );
            this.d3d.DxLost +=new Direct3d.DxDirect3dDelegate ( d3d_DxLost );
            this.d3d.DxRestore +=new Direct3d.DxDirect3dDelegate ( d3d_DxRestore );

            viewArcball.SetWindow ( d3d.ClientSize.Width , d3d.ClientSize.Height );
        }

        void d3d_DxRestore ( Direct3d d3d , Device dx )
        {
            dxLost = false;
        }

        void d3d_DxLost ( Direct3d d3d , Device dx )
        {
            dxLost = true;
        }

        private void OnD3dResize ( object sender , EventArgs e )
        {
            if (dxLost)
                return;

            //Set the window size
            viewArcball.SetWindow ( d3d.ClientSize.Width , d3d.ClientSize.Height );

            //Reset the projection
            float currentRatio = d3d.ClientSize.Height / viewportHeight;

            viewportWidth = d3d.ClientSize.Width / currentRatio;
            viewportHeight = d3d.ClientSize.Height / currentRatio;
            SetProjectionParameters ( viewportWidth , viewportHeight );
        }

        public void SetButtonMasks ( int rotateCamera , int moveCamera )
        {
            rotateCameraButtonMask = rotateCamera;
            moveCameraButtonMask = moveCamera;
        }

        /// <summary>
        /// Client can call this to change the position and direction of camera
        /// </summary>
        public void SetViewParameters ( Vector3 eyePt , Vector3 lookAtPt , Vector3 upPt )
        {
            // Store the data
            eye     = eyePt;
            lookAt  = lookAtPt;
            up      = upPt;

            // Calculate the view matrix
            viewMatrix = Matrix.LookAtLH ( eye , lookAt , up );
            look  = new Vector3 ( viewMatrix.M13 , viewMatrix.M23 , viewMatrix.M33 );

            // Propogate changes to the member arcball
            viewArcball.CurrentQuaternion = Quaternion.RotationMatrix ( viewMatrix );
           
            //Re-calculate the view matrix
            CalculateViewMatrix ( );
        }

        /// <summary>
        /// Calculates the projection matrix based on input params
        /// </summary>
        public void SetProjectionParameters ( float width , float height)
        {
            projMatrix = Matrix.OrthoLH ( width , height , DefaultViewPortNearPlane , DefaultViewPortFarPlane );

            //projMatrix = Matrix.OrthoRH ( width , height , DefaultViewPortNearPlane , DefaultViewPortFarPlane );

            viewportWidth = width;
            viewportHeight = height;

            ProjectionChangedAlarm ( );
        }

        /// <summary>
        /// Reset the camera's position back to the default
        /// </summary>
        public void Reset ( )
        {
            SetViewParameters ( DefaultEye , DefaultLookAt , AxisZ );

            viewArcball.Reset ( );
        }        

        /// <summary>
        /// Compute the view matrix
        /// </summary>
        public void CalculateViewMatrix ( )
        {
            // Get the inverse of the arcball's rotation matrix
            Matrix cameraRotation = Matrix.Invert ( viewArcball.RotationMatrix );

            // Transform vectors based on camera's rotation matrix
            Vector3 localUp = new Vector3 ( 0 , 1 , 0 );
            Vector3 localAhead = new Vector3 ( 0 , 0 , 1 );
            Vector3 worldUp = Vector3.TransformCoordinate ( localUp , cameraRotation );
            Vector3 worldAhead = Vector3.TransformCoordinate ( localAhead , cameraRotation );

            eye = lookAt - worldAhead;
            viewMatrix = Matrix.LookAtLH ( eye , lookAt , worldUp );

            //Console.WriteLine ( "eye:x={0},y={1},z={2}",eye.X,eye.Y,eye.Z );

            //Update the vector
            up = new Vector3 ( viewMatrix.M12 , viewMatrix.M22 , viewMatrix.M32 );
            look = new Vector3 ( viewMatrix.M13 , viewMatrix.M23 , viewMatrix.M33 );

            //View changed!
            ViewChangedAlarm ( );
        }
     

        /// <summary>
        /// Performing when user move mouse
        /// </summary>
        public void OnMouseMove ( MouseEventArgs e)
        {
            if (( e.Button == MouseButtons.Left && ( rotateCameraButtonMask & (int)MouseButtonMask.Left ) != 0 )
                ||( e.Button == MouseButtons.Right && ( rotateCameraButtonMask & (int)MouseButtonMask.Right ) != 0 ))
            {
                if (viewArcball.IsBeingDragged)
                {
                    viewArcball.OnMove ( e.X , e.Y);
                    CalculateViewMatrix ( );
                }
            }

            if (( e.Button == MouseButtons.Left && ( moveCameraButtonMask & (int)MouseButtonMask.Left ) != 0 )
                ||( e.Button == MouseButtons.Right && ( moveCameraButtonMask & (int)MouseButtonMask.Right ) != 0 ))
            {
                if (isDragging)
                {
                    Vector3 currentMousePoint = DxViewToCameraPlane ( e.X , e.Y );

                    MoveCamera ( currentMousePoint - lastMousePoint );
                    //View matrix has changed , so 
                    //recompute the currentPoint with new view matrix                
                    lastMousePoint = DxViewToCameraPlane ( e.X , e.Y );
                }
            }
        }

        /// <summary>
        /// Performing when user poped one mouse button
        /// </summary>
        public void OnMouseUp ( MouseEventArgs e )
        {
            if (( e.Button == MouseButtons.Left && ( rotateCameraButtonMask & (int)MouseButtonMask.Left ) != 0 )
                    ||( e.Button == MouseButtons.Right && ( rotateCameraButtonMask & (int)MouseButtonMask.Right ) != 0 ))
            {
                viewArcball.OnEnd ( );

            }

            if (( e.Button == MouseButtons.Left && ( moveCameraButtonMask & (int)MouseButtonMask.Left ) != 0 )
                    ||( e.Button == MouseButtons.Right && ( moveCameraButtonMask & (int)MouseButtonMask.Right ) != 0 ))
            {
                isDragging = false;
            }
        }

        /// <summary>
        /// Performing when user pushed one mouse button
        /// </summary>
        public void OnMouseDown ( MouseEventArgs e )
        {
            if (( e.Button == MouseButtons.Left && ( rotateCameraButtonMask & (int)MouseButtonMask.Left ) != 0 )
                    ||( e.Button == MouseButtons.Right && ( rotateCameraButtonMask & (int)MouseButtonMask.Right ) != 0 ))
            {
                viewArcball.OnBegin ( e.X , e.Y );

            }

            if (( e.Button == MouseButtons.Left && ( moveCameraButtonMask & (int)MouseButtonMask.Left ) != 0 )
                    ||( e.Button == MouseButtons.Right && ( moveCameraButtonMask & (int)MouseButtonMask.Right ) != 0 ))
            {
                isDragging = true;
                lastMousePoint = DxViewToCameraPlane ( e.X , e.Y );
            }
        }       

        /// <summary>
        /// Performing when user rolled the mouse wheel
        /// </summary>
        public void OnMouseWheel ( MouseEventArgs e )
        {            
            float currentRatio = d3d.ClientSize.Height / viewportHeight;

            currentRatio += e.Delta /20;

            if (currentRatio <= 0.0f)
                currentRatio = 1.0f;

            viewportWidth = d3d.ClientSize.Width / currentRatio;
            viewportHeight = d3d.ClientSize.Height / currentRatio;
            SetProjectionParameters ( viewportWidth , viewportHeight);
        }

        /// <summary>
        /// Move the camera
        /// </summary>
        private void MoveCamera ( Vector3 amount )
        {
            eye.Add ( -amount );
            lookAt.Add ( -amount );

            SetViewParameters ( eye , lookAt , up );
        }

        /// <summary>
        /// Move the camera
        /// </summary>
        public void MoveCamera ( Vector3 near , Vector3 far )
        {
            Vector3 target = DxViewToCameraPlane ( near , far );

            MoveCamera( eye - target );
        }

        /// <summary>
        ///Convert view port coordinates (ie. mouse) to the object space, 
        /// and then get the point on the camera plane which depended by up and right vector
        /// </summary>
        private Vector3 DxViewToCameraPlane ( int mouseX , int mouseY )
        {
            Vector3 near , far;
            GenerateLine ( mouseX , mouseY , out near , out far );

            Plane cameraPlane = Plane.FromPointNormal ( eye , look );
            //Console.WriteLine ( "CameraPlane : A = {0},B = {1},C = {2},D = {3}" , cameraPlane.A , cameraPlane.B , cameraPlane.C , cameraPlane .D);

            return Plane.IntersectLine ( cameraPlane , near , far );
        }

        /// <summary>
        ///Convert view port coordinates (ie. mouse) to the object space, 
        /// and then get the point on the camera plane which depended by up and right vector
        /// </summary>
        private Vector3 DxViewToCameraPlane ( Vector3 near , Vector3 far )
        {
            Plane cameraPlane = Plane.FromPointNormal ( eye , look );
            //Console.WriteLine ( "CameraPlane : A = {0},B = {1},C = {2},D = {3}" , cameraPlane.A , cameraPlane.B , cameraPlane.C , cameraPlane .D);

            return Plane.IntersectLine ( cameraPlane , near , far );
        }

        /// <summary>
        /// Generate a line whose start point is on the near plane of the viewing frustum,
        /// and the end point is on the far plane of the viewing frustum.
        /// </summary>
        private void GenerateLine ( int mouseX , int mouseY , out Vector3 near , out Vector3 far )
        {
            // Clamp mouse coordinates to viewport
            if (mouseX < 0) mouseX = 0;
            if (mouseY < 0) mouseY = 0;
            if (mouseX > d3d.Dx.Viewport.Width) mouseX = d3d.Dx.Viewport.Width;
            if (mouseY > d3d.Dx.Viewport.Height) mouseY = d3d.Dx.Viewport.Height;

            // Put mouse coordinates in screenspace Vector3's. These are the points
            // defining our ray for picking, which we'll transform back to world space
            near = new Vector3 ( mouseX , mouseY , 0 );
            far = new Vector3 ( mouseX , mouseY , 1 );

            // Transform points to world space
            near.Unproject ( d3d.Dx.Viewport , projMatrix , viewMatrix , Matrix.Identity );
            far.Unproject ( d3d.Dx.Viewport , projMatrix , viewMatrix , Matrix.Identity );
        }

        /// <summary>
        /// Compute the four corners of grid
        /// </summary>
        public void ComputeGridRange ( Plane gridPlane , 
            out Vector3 leftTop , out Vector3 leftBottom , out Vector3 rightTop , out Vector3 rightBottom)
        {
            Vector3 near;
            Vector3 far;

            //Compute the four corner vectors of the grid
            GenerateLine ( 0 , 0 , out near , out far );
            leftTop = Plane.IntersectLine ( gridPlane , near , far );

            GenerateLine ( 0 , d3d.ClientSize.Height , out near , out far );
            leftBottom = Plane.IntersectLine ( gridPlane , near , far );

            GenerateLine ( d3d.ClientSize.Width , 0 , out near , out far );
            rightTop = Plane.IntersectLine ( gridPlane , near , far );

            GenerateLine ( d3d.ClientSize.Width , d3d.ClientSize.Height , out near , out far );
            rightBottom = Plane.IntersectLine ( gridPlane , near , far );
        }
    }
}
