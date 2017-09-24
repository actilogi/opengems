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

using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;

namespace GEMS.Designer.Forms
{
    public partial class MaterialDetailsForm : Form
    {
        private GEMSMaterial currentMaterial = null;

        public MaterialDetailsForm(GEMSMaterial currentMaterial)
        {
            InitializeComponent();

            this.currentMaterial = currentMaterial;
        }

        private void MaterialDetailsForm_Load(object sender, EventArgs e)
        {
            //Bind controls
            this.txtMaterialName.Text = currentMaterial.Name;
            this.txtDetails.Text = currentMaterial.Detail;
            this.txtEpsilonX.Value = currentMaterial.EpsilonX;
            this.txtSigmaX.Value = currentMaterial.SigmaX;
            this.txtMuX.Value = currentMaterial.MuX;
            this.txtMusigmaX.Value = currentMaterial.MusigmaX;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtMaterialName.Text == "N/A")
            {
                MessageBox.Show("The Name of this material is not available!");
                return;
            }

            if (!currentMaterial.Parent.ValidateMaterialName(txtMaterialName.Text, currentMaterial.Id))
            {
                MessageBox.Show("The Name of this material has been used!");
                return;
            }

            currentMaterial.Name = this.txtMaterialName.Text;
            currentMaterial.Detail = this.txtDetails.Text;
            currentMaterial.EpsilonX = this.txtEpsilonX.Value;
            currentMaterial.SigmaX = this.txtSigmaX.Value;
            currentMaterial.MuX = this.txtMuX.Value;
            currentMaterial.MusigmaX = this.txtMusigmaX.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}