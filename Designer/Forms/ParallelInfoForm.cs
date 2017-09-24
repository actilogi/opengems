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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Direct3D;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;
using GEMS.Designer;

namespace GEMS.Designer.Forms
{
    public partial class ParallelInfoForm : Form
    {
        private GEMSProject m_project = null;
        private GEMSParallel parallel = null;

        public ParallelInfoForm ( GEMSProject project )
        {
            InitializeComponent ( );

            m_project = project;
        }

        //Renders
        GEMSProjectRender sceneRender = null;

        #region Form Event Handler

        private void ParallelInfoForm_Load ( object sender , EventArgs e )
        {
            sceneRender = new GEMSProjectRender ( d3d );
            sceneRender.Initialize ( m_project , GEMSProjectRender.SceneMode.Parallel );            
            parallel = sceneRender.ParallelRender.Parallel;

            nudXDivision.Value = parallel.DivisionCountInX;
            nudYDivision.Value = parallel.DivisionCountInY;
            nudZDivision.Value = parallel.DivisionCountInZ;

            nudXDivision.Maximum = parallel.DivisionMaxCountInX;
            nudYDivision.Maximum = parallel.DivisionMaxCountInY;
            nudZDivision.Maximum = parallel.DivisionMaxCountInZ;

            txtXMeshCount.Text = m_project.Mesh.MeshCountInX.ToString ( );
            txtYMeshCount.Text = m_project.Mesh.MeshCountInY.ToString ( );
            txtZMeshCount.Text = m_project.Mesh.MeshCountInZ.ToString ( );

            txtCPUNumber.Text = parallel.CPUNumber.ToString ( );
            txtBalance.Text = parallel.Balance.ToString ( );

            parallel.GEMSParallel_DataChanged += new GEMSParallel.GEMSParallel_DataChangedEventHandler ( OnGEMSParallelDataChanged );
        }

        private void OnGEMSParallelDataChanged ( object sender , EventArgs e )
        {
            this.txtCPUNumber.Text = parallel.CPUNumber.ToString ( );
            this.txtBalance.Text = parallel.Balance.ToString ( );
        }

        private void btnPEC_Click ( object sender , EventArgs e )
        {

        }

        private void btnOk_Click ( object sender , EventArgs e )
        {
            this.m_project.Parallel = this.parallel;

            Close ( );
        }

        private void btnCancel_Click ( object sender , EventArgs e )
        {
            Close ( );
        }


        private void btnCheckX_Click ( object sender , EventArgs e )
        {
            sceneRender.SetXYView ( );
        }

        private void btnCheckY_Click ( object sender , EventArgs e )
        {
            sceneRender.SetYZView ( );
        }

        private void btnCheckZ_Click ( object sender , EventArgs e )
        {
            sceneRender.SetZXView ( );
        }

        private void btnReset_Click ( object sender , EventArgs e )
        {
            sceneRender.ResetView ( );
        }

        #endregion

        #region NumericeUpDwonControls Value Changed Hanlders

        private void nudXDivision_ValueChanged ( object sender , EventArgs e )
        {
            parallel.UpdateDivisionInX ( (int)this.nudXDivision.Value );
        }

        private void nudYDivision_ValueChanged ( object sender , EventArgs e )
        {
            parallel.UpdateDivisionInY ( (int)this.nudYDivision.Value );
        }

        private void nudZDivision_ValueChanged ( object sender , EventArgs e )
        {
            parallel.UpdateDivisionInZ ( (int)this.nudZDivision.Value );
        }

        #endregion


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

        private void ParallelInfoForm_FormClosing ( object sender , FormClosingEventArgs e )
        {
            if (sceneRender != null)
            {
                sceneRender.Dispose ( );
                sceneRender = null;
            }
        }



    }
}