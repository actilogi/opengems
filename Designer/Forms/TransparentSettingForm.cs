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
    public partial class TransparentSettingForm : Form
    {
        Color singleColor;

        public TransparentSettingForm (Color singleColor )
        {
            InitializeComponent ( );

            this.singleColor = singleColor;
        }

        private void TransparentSettingForm_Load ( object sender , EventArgs e )
        {
            this.pbTransparencyDisplay.BackColor = singleColor;

            this.tbTranparency.Maximum = GEMSSingle.MaxTransparency;
            this.tbTranparency.Minimum = GEMSSingle.MinTransparency;

            this.tbTranparency.Value = singleColor.A;
        }

        private void tbTranparency_Scroll ( object sender , EventArgs e )
        {
            singleColor = Color.FromArgb ( tbTranparency.Value , singleColor );

            this.pbTransparencyDisplay.BackColor = singleColor;
        }

        public int Transparency
        {
            get { return singleColor.A; }
        }
    }
}