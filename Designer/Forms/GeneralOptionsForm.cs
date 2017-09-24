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

using GEMS.Designer;
using GEMS.Designer.Models.GeometryOperations;

namespace GEMS.Designer.Forms
{
    public partial class GeneralOptionsForm : Form
    {

        private Length.LengthUnit defaultSystemLengthUnit;

        public GeneralOptionsForm()
        {
            InitializeComponent();

            defaultSystemLengthUnit = (Length.LengthUnit)GEMS.Designer.Properties.Settings.Default.DefaultLengthUnit;
        }

        private void GeneralOptionsForm_Load(object sender, EventArgs e)
        {
            this.cboLengthUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));

            this.cboLengthUnit.SelectedItem = defaultSystemLengthUnit;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Save the setting
            defaultSystemLengthUnit = (Length.LengthUnit)this.cboLengthUnit.SelectedItem;

            GEMS.Designer.Properties.Settings.Default.DefaultLengthUnit = (int)defaultSystemLengthUnit;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}