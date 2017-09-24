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
using GEMS.Designer.Utility;

namespace GEMS.Designer.Forms
{
    public partial class ExcitationOutputForm : Form
    {
        private SingleEO io = null;
       
        public ExcitationOutputForm(SingleEO io)
        {
            InitializeComponent();

            this.io = io;
        }

        private void ExcitationOutputForm_Load(object sender, EventArgs e)
        {
            if (io == null)
                this.Close();

            switch (io.GetType().Name)
            {
                case "VoltageExcitation":

                    //Initialize the status of the controls
                   this.txtE0.Text = "Excitation";
                   this.txtIOType.Text = "Voltage";
                   this.lbExcitationType.Text = "Voltage";
                   this.cboExcitationUnit.DataSource = Enum.GetValues(typeof(Voltage.VoltageUnit));
                   this.cboFreqUnit.DataSource = Enum.GetValues(typeof(Frequency.FrequencyUnit));
                   this.cboTimeDelayUnit.DataSource = Enum.GetValues(typeof(Time.TimeUnit));
                   UpdateConstrolsVisibleStatus(true);

                    //Bind controls
                   VoltageExcitation ve = (VoltageExcitation)io;
                   this.txtExcitationValue.Value        = ve.ExcitationVoltage.Value;
                   this.cboExcitationUnit.SelectedItem  = ve.ExcitationVoltage.Unit;
                   this.txtFreq.Value                   = ve.PhaseFrequency.Value;
                   this.cboFreqUnit.SelectedItem        = ve.PhaseFrequency.Unit;
                   this.txtPhase.Value                  = ve.PhaseDelay;
                   this.txtTimeDelay.Value              = ve.TimeDelay.Value;
                   this.cboTimeDelayUnit.SelectedItem   = ve.TimeDelay.Unit;
                   this.cboSignValue.SelectedIndex      = ve.Positive;

                   this.cboFreqUnit.SelectedIndexChanged += new System.EventHandler(this.cboFreqUnit_SelectedIndexChanged);
                   this.cboTimeDelayUnit.SelectedIndexChanged += new System.EventHandler(this.cboTimeDelayUnit_SelectedIndexChanged);
                   this.txtTimeDelay.TextChanged += new System.EventHandler(this.txtTimeDelay_TextChanged);

                   break;
               case "VoltageOutput":

                   //Initialize the status of the controls
                   this.txtE0.Text = "Output";
                   this.txtIOType.Text = "Voltage";
                   UpdateConstrolsVisibleStatus(false);

                   //Bind controls
                   VoltageOutput vo = (VoltageOutput)io;
                   this.cboSignValue.SelectedIndex = vo.Positive;
   
                   break;
               case "CurrentExcitation":

                   //Initialize the status of the controls
                   this.txtE0.Text = "Excitation";
                   this.txtIOType.Text = "Current";
                   this.cboExcitationUnit.DataSource = Enum.GetValues(typeof(Current.CurrentUnit));
                   this.cboFreqUnit.DataSource = Enum.GetValues(typeof(Frequency.FrequencyUnit));
                   this.cboTimeDelayUnit.DataSource = Enum.GetValues(typeof(Time.TimeUnit));
                   this.lbExcitationType.Text = "Current";
                   UpdateConstrolsVisibleStatus(true);

                   //Bind controls
                   CurrentExcitation ce = (CurrentExcitation)io;
                   this.txtExcitationValue.Value        = ce.ExcitationCurrent.Value;
                   this.cboExcitationUnit.SelectedItem  = ce.ExcitationCurrent.Unit;
                   this.txtFreq.Value                   = ce.PhaseFrequency.Value;
                   this.cboFreqUnit.SelectedItem        = ce.PhaseFrequency.Unit;
                   this.txtPhase.Value                  = ce.PhaseDelay;
                   this.txtTimeDelay.Value              = ce.TimeDelay.Value;
                   this.cboTimeDelayUnit.SelectedItem   = ce.TimeDelay.Unit;
                   this.cboSignValue.SelectedIndex      = ce.Positive;

                   this.cboFreqUnit.SelectedIndexChanged += new System.EventHandler(this.cboFreqUnit_SelectedIndexChanged);
                   this.cboTimeDelayUnit.SelectedIndexChanged += new System.EventHandler(this.cboTimeDelayUnit_SelectedIndexChanged);
                   this.txtTimeDelay.TextChanged += new System.EventHandler(this.txtTimeDelay_TextChanged);
                   break;
               case "CurrentOutput":

                   //Initialize the status of the controls
                   this.txtE0.Text = "Output";
                   this.txtIOType.Text = "Current";
                   UpdateConstrolsVisibleStatus(false);

                   //Bind controls
                   CurrentOutput co = (CurrentOutput)io;
                   this.cboSignValue.SelectedIndex = co.Positive;             

                   break;
               default:
                  this.Close();
                  break;
            }
        }

        private void UpdateConstrolsVisibleStatus(bool isVisible)
        {
            this.lbExcitationType.Visible = isVisible;
            this.cboExcitationUnit.Visible = isVisible;
            this.txtExcitationValue.Visible = isVisible;
            this.gbPhaseDelay.Visible = isVisible;
            this.gbTimeDelay.Visible = isVisible;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            switch (io.GetType().Name)
            {
                case "VoltageExcitation":

                    //Get the value of controls
                    VoltageExcitation ve = (VoltageExcitation)io;

                    ve.ExcitationVoltage.Value    =  this.txtExcitationValue.Value ;     
                    ve.ExcitationVoltage.Unit     =  (Voltage.VoltageUnit)this.cboExcitationUnit.SelectedItem;
                    ve.PhaseFrequency.Value       =  this.txtFreq.Value  ;               
                    ve.PhaseFrequency.Unit        =  (Frequency.FrequencyUnit)this.cboFreqUnit.SelectedItem ;     
                    ve.PhaseDelay                 =  this.txtPhase.Value;                
                    ve.TimeDelay.Value            =  this.txtTimeDelay.Value;            
                    ve.TimeDelay.Unit             =  (Time.TimeUnit)this.cboTimeDelayUnit.SelectedItem;
                    ve.Positive                   =  this.cboSignValue.SelectedIndex;    

                    break;
                case "VoltageOutput":

                    //Get the value of controls
                    VoltageOutput vo = (VoltageOutput)io;
                    vo.Positive = this.cboSignValue.SelectedIndex;

                    break;
                case "CurrentExcitation":

                    //Get the value of controls
                    CurrentExcitation ce = (CurrentExcitation)io;

                    ce.ExcitationCurrent.Value  = this.txtExcitationValue.Value;
                    ce.ExcitationCurrent.Unit   = (Current.CurrentUnit)this.cboExcitationUnit.SelectedItem;
                    ce.PhaseFrequency.Value     = this.txtFreq.Value;
                    ce.PhaseFrequency.Unit      = (Frequency.FrequencyUnit)this.cboFreqUnit.SelectedItem;
                    ce.PhaseDelay               = this.txtPhase.Value;
                    ce.TimeDelay.Value          = this.txtTimeDelay.Value;
                    ce.TimeDelay.Unit           = (Time.TimeUnit)this.cboTimeDelayUnit.SelectedItem;
                    ce.Positive                 = this.cboSignValue.SelectedIndex;    

                    break;
                case "CurrentOutput":

                    //Get the value of controls
                    CurrentOutput co = (CurrentOutput)io;
                    co.Positive = this.cboSignValue.SelectedIndex;

                    break;
                default:
                    this.Close();
                    break;
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            

            this.Close();
        }

        private void txtPhase_TextChanged(object sender, EventArgs e)
        {
            ResetTimeDelay();
        }

        private void txtFreq_TextChanged(object sender, EventArgs e)
        {
            ResetTimeDelay();
        }

        private void cboFreqUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetTimeDelay();
        }

        private void cboTimeDelayUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetPhase();
        }

        private void txtTimeDelay_TextChanged(object sender, EventArgs e)
        {
            ResetPhase();
         }   

        private void ResetTimeDelay()
        {
            float phase = this.txtPhase.Value;
            Frequency frequency = new Frequency(this.txtFreq.Value, (Frequency.FrequencyUnit)this.cboFreqUnit.SelectedItem);

            if (frequency.Value > 0.0)
            {
                float timeDelay = (float)MathUtility.ComputeTimeDelay(phase, frequency.ChangeUnit(Frequency.FrequencyUnit.Hz));

                //Set this value to txtTimeDelay
                txtTimeDelay.Value = new Time(timeDelay, Time.TimeUnit.sec).ChangeUnit((Time.TimeUnit)cboTimeDelayUnit.SelectedItem);
            }
        }

        private void ResetPhase()
        {
            Time timeDelay = new Time(this.txtTimeDelay.Value, (Time.TimeUnit)cboTimeDelayUnit.SelectedItem);
            Frequency frequency = new Frequency(this.txtFreq.Value, (Frequency.FrequencyUnit)this.cboFreqUnit.SelectedItem);

            txtPhase.Value = (float)MathUtility.ComputePhase(timeDelay.ChangeUnit(Time.TimeUnit.sec), frequency.ChangeUnit(Frequency.FrequencyUnit.Hz));
       }
       
    }
}