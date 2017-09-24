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
    public partial class FieldOutputForm : Form
    {
        private PointOutput po = null;

        public FieldOutputForm(PointOutput po)
        {
            InitializeComponent();

            this.po = po;
        }

        private void FieldOutputForm_Load(object sender, EventArgs e)
        {
            this.cboEFieldX.SelectedIndex = (int)po.EFieldX;
            this.cboHFieldX.SelectedIndex = (int)po.HFieldX;
            this.cboEFieldY.SelectedIndex = (int)po.EFieldY;
            this.cboHFieldY.SelectedIndex = (int)po.HFieldY;
            this.cboEFieldZ.SelectedIndex = (int)po.EFieldZ;
            this.cboHFieldZ.SelectedIndex = (int)po.HFieldZ;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            po.EFieldX = (uint)this.cboEFieldX.SelectedIndex;
            po.HFieldX = (uint)this.cboHFieldX.SelectedIndex;
            po.EFieldY = (uint)this.cboEFieldY.SelectedIndex;
            po.HFieldY = (uint)this.cboHFieldY.SelectedIndex;
            po.EFieldZ = (uint)this.cboEFieldZ.SelectedIndex;
            po.HFieldZ = (uint)this.cboHFieldZ.SelectedIndex;
        }


    }
}