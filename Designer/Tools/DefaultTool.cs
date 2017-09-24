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
    /// This tool can move the camera , zoom the scene
    /// </summary>
    public class DefaultTool : Tool
    {
        ModelPane modelSpace = null;

        public DefaultTool ( MainForm workSpace )
            : base(workSpace)
        {
            this.modelSpace = workSpace.ModelWorkSpace;
        }       

        private void OnMouseDown(object sender,MouseEventArgs e)
        {
            modelSpace.SceneRender.PerformMouseDown ( e );
        }       

        void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            modelSpace.SceneRender.PerformMouseMove ( e );
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            modelSpace.SceneRender.PerformMouseUp ( e );
        }       

        void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            modelSpace.SceneRender.PerformMouseWheel ( e );
        }

        protected override void OnActivate()
        {
            this.modelSpace.SceneRender.SingleRenderMode = GEMSProjectRender.RenderMode.Viewing;

            this.modelSpace.D3d.MouseDown += new MouseEventHandler(OnMouseDown);
            this.modelSpace.D3d.MouseUp += new MouseEventHandler(OnMouseUp);
            this.modelSpace.D3d.MouseMove += new MouseEventHandler(OnMouseMove);
            this.modelSpace.D3d.MouseWheel += new MouseEventHandler(OnMouseWheel);


            base.OnActivate();
        }

        protected override void OnDeactivate()
        {
            this.modelSpace.SceneRender.SingleRenderMode = GEMSProjectRender.RenderMode.Modeling;

            modelSpace.D3d.MouseDown -= new MouseEventHandler(OnMouseDown);
            modelSpace.D3d.MouseUp -= new MouseEventHandler(OnMouseUp);
            modelSpace.D3d.MouseMove -= new MouseEventHandler(OnMouseMove);
            modelSpace.D3d.MouseWheel -= new MouseEventHandler(OnMouseWheel);

            base.OnDeactivate();
        }
    }
}
