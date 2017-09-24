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
    public class SphereTool : Tool
    {
        ModelPane modelSpace = null;

        public SphereTool(MainForm workSpace)
            : base(workSpace)
        {
            this.modelSpace = workSpace.ModelWorkSpace;
        }


        #region Mouse Event Handler

        private void OnMouseDown(object sender,MouseEventArgs e)
        {
            CreateSphere newSphere = new CreateSphere(modelSpace.Project.CreateNewOperationId());
            
            //The width,height,depth of cuboid is setted with default value now
            //We'll improve it next version by using mouse draging
            Length.LengthUnit unit = modelSpace.Project.Environment.DefaultLengthUnit;
            float value = modelSpace.Project.Environment.GridSize.Value;

            newSphere.Radius = new Length(value, unit);
 
            //Get the refPoint
            Vector3 center = modelSpace.SceneRender.DisplayedGridPoint;
            newSphere.Center = new Vector3WithUnit(center, modelSpace.Project.Environment.DefaultLengthUnit);

            //Create the new single
            modelSpace.Project.CreateNewSingle(newSphere);

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
            this.workspace.EnableCreateSphereTool = true;

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
            this.workspace.EnableCreateSphereTool = false;

            modelSpace.D3d.MouseLeave -= new EventHandler(OnMouseLeave);
            modelSpace.D3d.MouseEnter -= new EventHandler(OnMouseEnter);
            modelSpace.D3d.MouseDown -= new MouseEventHandler(OnMouseDown);
            modelSpace.D3d.MouseMove -= new MouseEventHandler(OnMouseMove);
            modelSpace.D3d.MouseUp -= new MouseEventHandler(OnMouseUp);

            base.OnDeactivate();
        }
    }
}
