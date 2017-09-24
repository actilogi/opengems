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


namespace GEMS.Display.Core
{
    public partial class FFTForm : Form
    {
        private FFTCalculator fft;
        private DisplayGuidPane parentForm;

        public FFTForm ( Result1D source,DisplayGuidPane parent )
        {
            InitializeComponent ( );

            fft = new FFTCalculator ( source );
            this.parentForm = parent;
        }

        private void FFTForm_Load ( object sender , EventArgs e )
        {
            this.cboFreqUnits.DataSource = Enum.GetValues ( typeof ( FrequencyFormator.FrequencyUnit ) );
            this.cboFreqUnits.SelectedIndex = (int)FrequencyFormator.FrequencyUnit.GHz;

            this.txtDataLength.Text = this.fft.FFTDataLength.ToString ( );
            this.txtPowof2.Text = this.fft.PowOf2.ToString();
            this.txtSourceDataLength.Text = this.fft.SourceDataLength.ToString();

            this.btnPreview.Enabled = false;
            this.rbAp.Checked = true;
        }      

        private void btnCalculate_Click ( object sender , EventArgs e )
        {
            fft.Calculate ( );

            this.btnPreview.Enabled = true;
        }

        private void btnPreview_Click ( object sender , EventArgs e )
        {
            float startFreq;
            float endFreq;

            if (!float.TryParse ( txtStartFreq.Text.Trim ( ) , out startFreq ))
            {
                MessageBox.Show ( "Invalidate start frequency!" );
                return;
            }

            if (!float.TryParse ( txtEndFreq.Text.Trim ( ) , out endFreq ))
            {
                MessageBox.Show ( "Invalidate end frequency!" );
                return;
            }

            if (startFreq >= endFreq)
            {
                MessageBox.Show ( "The start frequency must be smaller than the end frequency!" );
                return;
            }

            //Change the unit of frequency
            startFreq = FrequencyFormator.ChangedUnit ( startFreq , (FrequencyFormator.FrequencyUnit)cboFreqUnits.SelectedValue , FrequencyFormator.FrequencyUnit.Hz );
            endFreq = FrequencyFormator.ChangedUnit ( endFreq , (FrequencyFormator.FrequencyUnit)cboFreqUnits.SelectedValue , FrequencyFormator.FrequencyUnit.Hz );

            if (rbAp.Checked)
            {
                parentForm.ResultLoadedAlarm ( fft.GetAmpPhaseData ( startFreq , endFreq ) );
            }

            if (rbRi.Checked)
            {
                parentForm.ResultLoadedAlarm ( fft.GetRealImageData ( startFreq , endFreq ) );
            }
        }

        private void cboFreqUnits_SelectedIndexChanged ( object sender , EventArgs e )
        {
            float freq = FrequencyFormator.ChangedUnit ( fft.Step ,FrequencyFormator.FrequencyUnit.Hz, (FrequencyFormator.FrequencyUnit)cboFreqUnits.SelectedValue );
            this.txtFreqStep.Text = freq.ToString ( );
        }

        private void btnClose_Click ( object sender , EventArgs e )
        {
            this.Close ( );
        }



    }
}