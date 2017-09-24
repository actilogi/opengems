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
    /// <summary>
    /// A class rendering something over the Direct3D 
    /// </summary>
    public abstract class Direct3dRender : IDisposable
    {
        #region Protected Members

        protected Direct3d d3d;
        protected bool isDisplayed = true;
        protected bool isDxLost = false;
        protected Vector3 minVector3 = Vector3.Empty;
        protected Vector3 maxVector3 = Vector3.Empty;

        // Track whether Dispose has been called.
        protected bool disposed = false;

        #endregion

        #region Protected Const members

        internal static Color DefaultLineColor = Color.Red;
        internal static Color DefaultXAxisColor = Color.Blue;
        internal static Color DefaultYAxisColor = Color.Green;
        internal static Color DefaultZAxisColor = Color.Red;
        internal static int DefaultLineWidth = 2;
        internal static int DefaultSlices = 36;
        internal static int DefaultStacks = 36;
        internal static int DefaultPointWidth = 8;
        internal static Color DefaultPointColor = Color.Black;
        internal static Vector3 DefaultVertexNormal = new Vector3(0, 0, 1);

        internal static float DefaultMiniAxisWidthRatio = 0.05f;
        internal static float DefaultMiniAxisHeightRatio = 0.95f;

        #endregion

        #region Public Properties

        public bool IsDisplayed
        {
            get { return isDisplayed; }
            set { isDisplayed = value; }
        }

        public Vector3 MaxVector3
        {
            get { return maxVector3; }
            set { maxVector3 = value; }
        }

        public Vector3 MinVector3
        {
            get { return minVector3; }
            set { minVector3 = value; }
        }

        #endregion

        #region Abstract methods

        public abstract void Initialize();
        protected abstract void PerformRender();
        protected abstract void PerformDispose ( );

        #endregion

        #region Public methods

        public Direct3dRender ( Direct3d d3d )
        {
            this.d3d    = d3d;
            this.d3d.DxLost +=new Direct3d.DxDirect3dDelegate ( d3d_DxLost );
            this.d3d.DxRestore +=new Direct3d.DxDirect3dDelegate ( d3d_DxRestore );
        }

        void d3d_DxRestore ( Direct3d d3d , Device dx )
        {
            isDxLost = false;
        }

        void d3d_DxLost ( Direct3d d3d , Device dx )
        {
            isDxLost = true;
        }

        /// <summary>
        /// Render itself
        /// </summary>
        public void Render()
        {
            if (isDisplayed && !isDxLost)
                PerformRender();
        }

        /// <summary>
        /// Dispose all the resource
        /// </summary>
        public void Dispose ( )
        {
            Dispose ( true );

            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize ( this );
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose ( bool disposing )
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    PerformDispose ( );
                }
            }
            disposed = true;
        }

        // Use C# destructor syntax for finalization code.
       // This destructor will run only if the Dispose method 
       // does not get called.
       // It gives your base class the opportunity to finalize.
       // Do not provide destructors in types derived from this class.
        ~Direct3dRender ( )      
       {
          // Do not re-create Dispose clean-up code here.
          // Calling Dispose(false) is optimal in terms of
          // readability and maintainability.
          Dispose(false);
       }

        #endregion

    }
}
