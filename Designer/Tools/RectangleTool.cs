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
    public class RectangleTool : Tool
    {
        ModelPane modelSpace = null;

        public RectangleTool(MainForm workSpace)
            : base(workSpace)
        {
            this.modelSpace = workSpace.ModelWorkSpace;
        }


        #region Mouse Event Handler

        private void OnMouseDown(object sender,MouseEventArgs e)
        {
            CreateRectangle newRectangle = new CreateRectangle(modelSpace.Project.CreateNewOperationId());
            
            //The width,height,depth of cuboid is setted with default value now
            //We'll improve it next version by using mouse draging
            Length.LengthUnit unit = modelSpace.Project.Environment.DefaultLengthUnit;
            float value1 = modelSpace.Project.Environment.GridSize.Value * 2;
            float value2 = modelSpace.Project.Environment.GridSize.Value;

            newRectangle.Width = new Length(value1, unit);
            newRectangle.Height = new Length(value2, unit);

            //Set the refpoint
            Vector3 refPoint = modelSpace.SceneRender.DisplayedGridPoint;
            newRectangle.RefPoint = new Vector3WithUnit(refPoint, modelSpace.Project.Environment.DefaultLengthUnit);

            //Set the aline axis
            switch (modelSpace.Project.Environment.GridPlane)
            {
                case GridPlane.XY:
                    newRectangle.AlineAxis = Axis.Z;
                    break;
                case GridPlane.XZ:
                    newRectangle.AlineAxis = Axis.Y;
                    break;
                case GridPlane.YZ:
                    newRectangle.AlineAxis = Axis.X;
                    break;
                default:
                    newRectangle.AlineAxis = Axis.Z;
                    break;
            }

            //Create the new single
            modelSpace.Project.CreateNewSingle(newRectangle);

            workspace.SelectTool();

        }       

        private void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            modelSpace.SceneRender.UpdateGridPoint ( (short)e.X , (short)e.Y );
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
            this.workspace.EnableCreateRectangleTool = true;

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
            this.workspace.EnableCreateRectangleTool = false;

            modelSpace.D3d.MouseLeave -= new EventHandler(OnMouseLeave);
            modelSpace.D3d.MouseEnter -= new EventHandler(OnMouseEnter);
            modelSpace.D3d.MouseDown -= new MouseEventHandler(OnMouseDown);
            modelSpace.D3d.MouseMove -= new MouseEventHandler(OnMouseMove);
            modelSpace.D3d.MouseUp -= new MouseEventHandler(OnMouseUp);

            base.OnDeactivate();
        }
    }
}
