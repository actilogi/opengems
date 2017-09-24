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
    public partial class ModelPane : UserControl, IObserver
    {     
 
        public ModelPane()
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

                sceneRender.Initialize(m_project, GEMSProjectRender.SceneMode.Modeling);

                //sceneRender.SetFitView ( );
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

        public void SaveScreenshot ( string fileName )
        {
            Surface backbuffer = d3d.Dx.GetBackBuffer(0, 0, BackBufferType.Mono);
            SurfaceLoader.Save ( fileName , ImageFileFormat.Jpg , backbuffer );
            backbuffer.Dispose();
        }
       
    }
}