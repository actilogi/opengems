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

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Panes;
using GEMS.Designer.Direct3D;

namespace GEMS.Designer.Tools
{
    public class LineTool : Tool
    {
        ModelPane modelSpace = null;

        public LineTool(MainForm workSpace)
            : base(workSpace)
        {
            this.modelSpace = workSpace.ModelWorkSpace;
        }


        #region Mouse Event Handler

        private void OnMouseDown(object sender,MouseEventArgs e)
        {
            CreateLine newLine = new CreateLine(modelSpace.Project.CreateNewOperationId());

            float value = modelSpace.Project.Environment.GridSize.Value * 2;

            Vector3 point1 = modelSpace.SceneRender.DisplayedGridPoint;
            newLine.StartPoint = new Vector3WithUnit(point1, modelSpace.Project.Environment.DefaultLengthUnit);
            switch (modelSpace.Project.Environment.GridPlane)
            {
                case GridPlane.XY:
                    newLine.EndPoint = new Vector3WithUnit(point1 + new Vector3(value,0,0), modelSpace.Project.Environment.DefaultLengthUnit);
                    break;
                case GridPlane.XZ:
                    newLine.EndPoint = new Vector3WithUnit(point1 + new Vector3(0, 0, value), modelSpace.Project.Environment.DefaultLengthUnit);
                    break;
                case GridPlane.YZ:
                    newLine.EndPoint = new Vector3WithUnit(point1 + new Vector3(0, value, 0), modelSpace.Project.Environment.DefaultLengthUnit);
                    break;
                default:
                    newLine.EndPoint = new Vector3WithUnit(point1 + new Vector3(value, 0, 0), modelSpace.Project.Environment.DefaultLengthUnit);
                    break;
            }


            //Create the new single
            modelSpace.Project.CreateNewSingle(newLine);

            workspace.SelectTool();

        }       

        private void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            modelSpace.SceneRender.UpdateGridPoint((short)e.X, (short)e.Y);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {

        }
        
        private void OnMouseLeave(object sender, EventArgs e)
        {
            this.modelSpace.SceneRender.IsDisplayGridPoint = false;
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            this.modelSpace.SceneRender.IsDisplayGridPoint = true;
        }


        #endregion

        protected override void OnActivate()
        {
            this.workspace.EnableCreateLineTool = true;

            this.modelSpace.D3d.MouseLeave += new EventHandler(OnMouseLeave);
            this.modelSpace.D3d.MouseEnter += new EventHandler(OnMouseEnter);
            this.modelSpace.D3d.MouseDown += new MouseEventHandler(OnMouseDown);
            this.modelSpace.D3d.MouseMove += new MouseEventHandler(OnMouseMove);
            this.modelSpace.D3d.MouseUp += new MouseEventHandler(OnMouseUp);

            base.OnActivate();
        }

        
        protected override void OnDeactivate()
        {
            this.modelSpace.SceneRender.IsDisplayGridPoint = false;
            this.workspace.EnableCreateLineTool = false;

            modelSpace.D3d.MouseLeave -= new EventHandler(OnMouseLeave);
            modelSpace.D3d.MouseEnter -= new EventHandler(OnMouseEnter);
            modelSpace.D3d.MouseDown -= new MouseEventHandler(OnMouseDown);
            modelSpace.D3d.MouseMove -= new MouseEventHandler(OnMouseMove);
            modelSpace.D3d.MouseUp -= new MouseEventHandler(OnMouseUp);

            base.OnDeactivate();
        }
    }
}
