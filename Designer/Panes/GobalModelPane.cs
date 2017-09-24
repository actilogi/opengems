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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Direct3D;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;
using GEMS.Designer;

namespace GEMS.Designer.Panes
{
    public partial class GobalModelPane : UserControl, IObserver
    {
        public GobalModelPane ( )
        {
            InitializeComponent();
        }

        #region Internal Members

        //Control Members
        private GEMSProject m_project;
        private GEMSProjectRender sceneRender = null;


        #endregion

        #region Public Properties

        // IObserver.Proejct
        public GEMSProject Project
        {
            set
            {
                if (value == null)
                    return;

                m_project = (GEMSProject)value;
                sceneRender.Initialize(m_project, GEMSProjectRender.SceneMode.Preview);
            }

            get { return m_project; }
        }

        public GEMSProjectRender SceneRender
        {
            get { return sceneRender; }
        }
  
       
        public Direct3d D3d
        {
            get { return d3d; }
        }
        
        #endregion

        private void ModelPane_Load ( object sender , EventArgs e )
        {

        }

        private void d3d_DxLoaded ( Direct3d d3d , Device dx )
        {
            sceneRender = new GEMSProjectRender (d3d);
        }

        #region Mouse Messhage Handlers

        private void d3d_MouseDown ( object sender , MouseEventArgs e )
        {
            sceneRender.PerformMouseDown ( e );
        }

        void d3d_MouseMove ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
            sceneRender.PerformMouseMove ( e );
        }

        private void d3d_MouseUp ( object sender , MouseEventArgs e )
        {
            sceneRender.PerformMouseUp ( e );
        }

        void d3d_MouseWheel ( object sender , System.Windows.Forms.MouseEventArgs e )
        {
            sceneRender.PerformMouseWheel ( e );
        }

        #endregion
               
    }
}