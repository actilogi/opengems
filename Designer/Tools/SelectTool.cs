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
using System.Windows.Forms;

using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Panes;
using GEMS.Designer.Direct3D;

namespace GEMS.Designer.Tools
{    
    /// <summary>
    /// This tool can select a geometry on the scene, and zoom the scene,
    /// perform some key actions
    /// </summary>
    public class SelectTool : Tool
    {
        ModelPane modelSpace = null;

        public SelectTool(MainForm workSpace)
            : base(workSpace)
        {
            this.modelSpace = workSpace.ModelWorkSpace;
        }       

        private void OnMouseDown(object sender,MouseEventArgs e)
        {
            modelSpace.SceneRender.PerformPicking(e);          
            
        }
       
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            workspace.currentProject.PerformKeyDown ( e );
        }

        void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            modelSpace.SceneRender.PerformMouseWheel ( e );
        }      

        protected override void OnActivate()
        {
            this.workspace.EnableSelectedTool = true;

            modelSpace.D3d.MouseDown += new MouseEventHandler(OnMouseDown);
            //modelSpace.D3d.MouseUp += new MouseEventHandler ( OnMouseUp );
           // modelSpace.D3d.MouseMove += new MouseEventHandler ( OnMouseMove );
            modelSpace.D3d.MouseWheel += new MouseEventHandler ( OnMouseWheel );
            modelSpace.D3d.KeyDown += new KeyEventHandler(OnKeyDown);


            base.OnActivate();
        }

        protected override void OnDeactivate()
        {
            this.workspace.EnableSelectedTool = false;

            modelSpace.D3d.MouseDown -= new MouseEventHandler(OnMouseDown);
            //modelSpace.D3d.MouseUp -= new MouseEventHandler ( OnMouseUp );
            //modelSpace.D3d.MouseMove -= new MouseEventHandler ( OnMouseMove );
            modelSpace.D3d.MouseWheel -= new MouseEventHandler(OnMouseWheel);
            modelSpace.D3d.KeyDown -= new KeyEventHandler(OnKeyDown);

            base.OnDeactivate();
        }
    }
}
