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

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryOperations;

namespace GEMS.Designer.Models.GeometryModels
{
    public abstract class GeometryModel
    {
        #region Internal Members

        //Render necessary members
        protected Matrix worldMatrix = Matrix.Identity;
        protected Vector3 minVector3 = Vector3.Empty;
        protected Vector3 maxVector3 = Vector3.Empty;

        protected Color modelColor;     

        //Control members
        protected List<GeometryModel> subModels = new List<GeometryModel>();

        #endregion

        #region Static members

        internal static int DefaultLineWidth = 3;
        internal static float DefaultEOLineLengthRatio = 0.1f;
        internal static float DefaultEOLineLength = 0.5f;
        internal static float DefaultDistanceDelta = 1e-6f;


        #endregion

        #region Pulic Members

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

        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
            set { worldMatrix = value; }
        }

        public List<GeometryModel> SubModels
        {
            get { return subModels; }
        }

        public Color ModelColor
        {
            get { return modelColor; }
            set { modelColor = value; }
        }  

        #endregion
   

        /// <summary>
        /// Make sure the given vertex is inside or outside of the geometry
        /// The vertex is in the world space
        /// </summary>
        /// <returns>True if inside,otherwise false</returns>
        public abstract bool PositionRelation(Vector3 vertex);
   
    }
}
