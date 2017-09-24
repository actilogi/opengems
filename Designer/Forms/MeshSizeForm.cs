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

using GEMS.Designer.Models;

namespace GEMS.Designer.Forms
{
    public partial class MeshSizeForm : Form
    {
        private GEMSMesh mesh;

        public MeshSizeForm(GEMSMesh mesh)
        {
            InitializeComponent();

            this.mesh = mesh;
        }

        private void MeshSizeForm_Load(object sender, EventArgs e)
        {
            this.txtXSize.Value = mesh.MinMeshCell.X;
            this.txtYSize.Value = mesh.MinMeshCell.Y;
            this.txtZSize.Value = mesh.MinMeshCell.Z;

            this.lblXSizeUnit.Text = mesh.Parent.Environment.DefaultLengthUnit.ToString();
            this.lblYSizeUnit.Text = mesh.Parent.Environment.DefaultLengthUnit.ToString();
            this.lblZSizeUnit.Text = mesh.Parent.Environment.DefaultLengthUnit.ToString();
        }
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMesh_Click(object sender, EventArgs e)
        {            
            if (txtXSize.Value > 0.0 && txtYSize.Value > 0.0 && txtZSize.Value > 0.0)
            {
                //Ensure the cell size is available
                StringBuilder checkMessageBuilder = new StringBuilder();
                bool isCellSizeAvailable = true;

                if ((mesh.Parent.ComputationalDomain.MaxX - mesh.Parent.ComputationalDomain.MinX) / txtXSize.Value < 2)
                {
                    checkMessageBuilder.AppendLine("Cell size of X direction is too large!");
                    isCellSizeAvailable = false;
                }

                if ((mesh.Parent.ComputationalDomain.MaxY - mesh.Parent.ComputationalDomain.MinY) / txtYSize.Value < 2)
                {
                    checkMessageBuilder.AppendLine ( "Cell size of Y direction is too large!" );
                    isCellSizeAvailable = false;
                } 
                
                if ((mesh.Parent.ComputationalDomain.MaxZ - mesh.Parent.ComputationalDomain.MinZ) / txtZSize.Value < 2)
                {
                    checkMessageBuilder.AppendLine ( "Cell size of Z direction is too large!" );
                    isCellSizeAvailable = false;
                }

                if (isCellSizeAvailable)
                {
                    mesh.GenerateMeshPoints(txtXSize.Value, txtYSize.Value, txtZSize.Value);
                    mesh.Parent.Parallel.InitializeDivision();
                    mesh.Parent.HuygensBox.ResetRange();
                }
                else
                {
                    MessageBox.Show(checkMessageBuilder.ToString());
                }

                return;
            }
            else
            {
                MessageBox.Show("Minimum cell size is invalid");
                return;
            }
        }

        private void btnDetails_Click ( object sender , EventArgs e )
        {
            if (mesh.MeshCountInX > 0 && mesh.MeshCountInY > 0 && mesh.MeshCountInZ > 0)
            {
                MeshSizeDetailsForm form = new MeshSizeDetailsForm ( mesh );
                form.ShowDialog ( );
            }
        }        
    }
}