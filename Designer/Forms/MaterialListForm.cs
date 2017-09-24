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
using GEMS.Designer;

namespace GEMS.Designer.Forms
{
    public partial class MaterialListForm : Form
    {
        private GEMSSingle single = null;
        private List<GEMSMaterial> currentMaterialList = null;
        

        public MaterialListForm(GEMSSingle single)
        {
            InitializeComponent();

            this.single = single;

            currentMaterialList = new List<GEMSMaterial> (single.Parent.Materials );

        }

        private void MaterialListForm_Load(object sender, EventArgs e)
        {
            //Refresh the DataGridView
            this.geometryMaterialListBindingSource.DataSource = currentMaterialList;

            //Set the current selected material
            if (currentMaterialList.Count > 0)
            {
               if (single.SingleMaterial != null) //it is an available material
                {
                    for (int index = 0; index < currentMaterialList.Count; index++)
                    {
                        if (currentMaterialList[index].Id == single.SingleMaterial.Id)
                        {
                            this.dgvMaterils.Rows[index].Selected = true;
                            return;
                        }
                    }
                }
                else
                    this.dgvMaterils.Rows[0].Selected = true;
            }
     
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            GEMSMaterial selectedMaterial = GetSelectedMaterial();

            if (selectedMaterial == null)
                return;

            //Show the edit form 
            MaterialDetailsForm mdf = new MaterialDetailsForm(selectedMaterial);

            if (mdf.ShowDialog() == DialogResult.OK)
            {
                //Refresh the DataGridView control
                this.geometryMaterialListBindingSource.ResetBindings(false);
            }
        }
       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Create a new material
            GEMSMaterial newMaterial = single.Parent.CreateNewMaterial();

            //Show the edit form 
            MaterialDetailsForm mdf = new MaterialDetailsForm(newMaterial);

            if(mdf.ShowDialog() == DialogResult.OK) 
            {
                //Add this new material into the list of current project
                currentMaterialList.Add(newMaterial);

                //Refresh the DataGridView control
                this.geometryMaterialListBindingSource.ResetBindings(false);
            }          
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GEMSMaterial selectedMaterial = GetSelectedMaterial();

            if (selectedMaterial == null)
                return;
            
            if (selectedMaterial.Status != GEMSMaterial.MaterialStatus.Unused && selectedMaterial != single.SingleMaterial)
            {
                MessageBox.Show("This materil is used by other singles!");
            }
            else
            {
                currentMaterialList.Remove(selectedMaterial);

                if (selectedMaterial == single.SingleMaterial)
                    single.SingleMaterial = null;
            }

            //Refresh the DataGridView control
            this.geometryMaterialListBindingSource.ResetBindings(false);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
             GEMSMaterial selectedMaterial = GetSelectedMaterial();

            if (selectedMaterial == null)
                return;

            //Save the current material list to project
            single.Parent.Materials = currentMaterialList;

            single.Parent.IsUpdated = true;

            ////Set the selected material to the currentMaterial
            single.SingleMaterial = selectedMaterial;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            //Save the current material list to project
            single.Parent.Materials = currentMaterialList;
            single.SingleMaterial = null;

            this.Close();
        }

        /// <summary>
        /// Get the current selected material on the datagridview
        /// </summary>
        /// <returns></returns>
        private GEMSMaterial GetSelectedMaterial()
        {
            if (this.dgvMaterils.SelectedRows.Count != 0)
            {
                int materialId = (int)this.dgvMaterils.SelectedRows[0].Cells["Id"].Value;

                //Find this material
                foreach (GEMSMaterial material in currentMaterialList)
                {
                    if (material.Id == materialId)
                        return material;
                }

            }

            return null;
        }
    }
}