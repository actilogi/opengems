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
using GEMS.Designer;
using GEMS.Designer.Direct3D;
using GEMS.Designer.Models;

namespace GEMS.Designer.Forms
{
    public partial class ComputationalDomainForm : Form
    {
        private GEMSComputationalDomain domain = null;
        private GEMSProjectRender sceneRender = null;

        public ComputationalDomainForm(GEMSComputationalDomain domain, GEMSProjectRender sceneRender)
        {
            InitializeComponent();

            this.domain = domain;
            this.sceneRender = sceneRender;
        }

        /// <summary>
        /// Initialize the combobox's list
        /// </summary>
        private void InitializeCombobox()
        {
            //Initialize the Combobox
            this.cboXminUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboYminUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboZminUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboXmaxUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboYmaxUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboZmaxUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboXminCondUnit.DataSource = Enum.GetValues(typeof(BoundaryCondition));
            this.cboYminCondUnit.DataSource = Enum.GetValues(typeof(BoundaryCondition));
            this.cboZminCondUnit.DataSource = Enum.GetValues(typeof(BoundaryCondition));
            this.cboXmaxCondUnit.DataSource = Enum.GetValues(typeof(BoundaryCondition));
            this.cboYmaxCondUnit.DataSource = Enum.GetValues(typeof(BoundaryCondition));
            this.cboZmaxCondUnit.DataSource = Enum.GetValues(typeof(BoundaryCondition));
        }

        /// <summary>
        /// Bound the data of current domain to the controls
        /// </summary>
        private void BoundData()
        {
            cboXminUnit.SelectedItem = domain.MinVector3.X.Unit;
            txtXmin.Value = domain.MinVector3.X.Value;
            cboXmaxUnit.SelectedItem = domain.MaxVector3.X.Unit;
            txtXmax.Value = domain.MaxVector3.X.Value;

            cboYminUnit.SelectedItem = domain.MinVector3.Y.Unit;
            txtYmin.Value = domain.MinVector3.Y.Value;
            cboYmaxUnit.SelectedItem = domain.MaxVector3.Y.Unit;
            txtYmax.Value = domain.MaxVector3.Y.Value;

            cboZminUnit.SelectedItem = domain.MinVector3.Z.Unit;
            txtZmin.Value = domain.MinVector3.Z.Value;
            cboZmaxUnit.SelectedItem = domain.MaxVector3.Z.Unit;
            txtZmax.Value = domain.MaxVector3.Z.Value;

            cboXminCondUnit.SelectedItem = domain.ConditionXmin;
            cboXmaxCondUnit.SelectedItem = domain.ConditionXmax;

            cboYminCondUnit.SelectedItem = domain.ConditionYmin;
            cboYmaxCondUnit.SelectedItem = domain.ConditionYmax;

            cboZminCondUnit.SelectedItem = domain.ConditionZmin;
            cboZmaxCondUnit.SelectedItem = domain.ConditionZmax;
        }

        private void ComputationalDomainForm_Load(object sender, EventArgs e)
        {
            InitializeCombobox();
            BoundData();
        }

        /// <summary>
        /// Auto fill the simulation range data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoRange_Click(object sender, EventArgs e)
        {
            cboXminUnit.SelectedItem = domain.Parent.Environment.DefaultLengthUnit;
            txtXmin.Value = sceneRender.MinVector3.X;
            cboXmaxUnit.SelectedItem = domain.Parent.Environment.DefaultLengthUnit;
            txtXmax.Value = sceneRender.MaxVector3.X;

            cboYminUnit.SelectedItem = domain.Parent.Environment.DefaultLengthUnit;
            txtYmin.Value = sceneRender.MinVector3.Y;
            cboYmaxUnit.SelectedItem = domain.Parent.Environment.DefaultLengthUnit;
            txtYmax.Value = sceneRender.MaxVector3.Y;

            cboZminUnit.SelectedItem = domain.Parent.Environment.DefaultLengthUnit;
            txtZmin.Value = sceneRender.MinVector3.Z;
            cboZmaxUnit.SelectedItem = domain.Parent.Environment.DefaultLengthUnit;
            txtZmax.Value = sceneRender.MaxVector3.Z;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Ensure the domain is available
            Length xmin = new Length(txtXmin.Value, (Length.LengthUnit)this.cboXminUnit.SelectedItem);
            Length xmax = new Length(txtXmax.Value, (Length.LengthUnit)this.cboXmaxUnit.SelectedItem);

            Length ymin = new Length(txtYmin.Value, (Length.LengthUnit)this.cboYminUnit.SelectedItem);
            Length ymax = new Length(txtYmax.Value, (Length.LengthUnit)this.cboYmaxUnit.SelectedItem);

            Length zmin = new Length(txtZmin.Value, (Length.LengthUnit)this.cboZminUnit.SelectedItem);
            Length zmax = new Length(txtZmax.Value, (Length.LengthUnit)this.cboZmaxUnit.SelectedItem);

            if (xmin.Equals(domain.MinVector3.X)
                && ymin.Equals(domain.MinVector3.Y)
                && zmin.Equals(domain.MinVector3.Z)
                && xmax.Equals(domain.MaxVector3.X)
                && ymax.Equals(domain.MaxVector3.Y)
                && zmax.Equals(domain.MaxVector3.Z))
            {
                //No change in the domain 
                //Boundary condition will not effect the domain
                domain.ConditionXmin = (BoundaryCondition)this.cboXminCondUnit.SelectedItem;
                domain.ConditionXmax = (BoundaryCondition)this.cboXmaxCondUnit.SelectedItem;

                domain.ConditionYmin = (BoundaryCondition)this.cboYminCondUnit.SelectedItem;
                domain.ConditionYmax = (BoundaryCondition)this.cboYmaxCondUnit.SelectedItem;

                domain.ConditionZmin = (BoundaryCondition)this.cboZminCondUnit.SelectedItem;
                domain.ConditionZmax = (BoundaryCondition)this.cboZmaxCondUnit.SelectedItem;

                this.Close ( );
                
                return;
            }

            bool isDomainAvailable = true;
            StringBuilder checkMessageBuilder = new StringBuilder();

            if (xmax < xmin)
            {
                isDomainAvailable = false;
                checkMessageBuilder.AppendLine("Domain of X direction must larger than zero!");
            }

            if (ymax < ymin)
            {
                isDomainAvailable = false;
                checkMessageBuilder.AppendLine("Domain of Y direction must larger than zero!");
            }

            if (zmax < zmin)
            {
                isDomainAvailable = false;
                checkMessageBuilder.AppendLine("Domain of Z direction must larger than zero!");
            }

            if (isDomainAvailable)
            {
                //Set the value
                domain.MinVector3.X = xmin;
                domain.MaxVector3.X = xmax;
                domain.MinVector3.Y = ymin;
                domain.MaxVector3.Y = ymax;
                domain.MinVector3.Z = zmin;
                domain.MaxVector3.Z = zmax;

                domain.ConditionXmin = (BoundaryCondition)this.cboXminCondUnit.SelectedItem;
                domain.ConditionXmax = (BoundaryCondition)this.cboXmaxCondUnit.SelectedItem;

                domain.ConditionYmin = (BoundaryCondition)this.cboYminCondUnit.SelectedItem;
                domain.ConditionYmax = (BoundaryCondition)this.cboYmaxCondUnit.SelectedItem;

                domain.ConditionZmin = (BoundaryCondition)this.cboZminCondUnit.SelectedItem;
                domain.ConditionZmax = (BoundaryCondition)this.cboZmaxCondUnit.SelectedItem;

                domain.DomainDataChangedAlarm();

                domain.Parent.Mesh.Reset();

                this.Close();
            }
            else
            {
                MessageBox.Show(checkMessageBuilder.ToString());
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}