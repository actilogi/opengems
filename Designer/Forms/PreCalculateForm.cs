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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GEMS.Designer.Models;

namespace GEMS.Designer.Forms
{
    public partial class PreCalculateForm : Form
    {
        private GEMSProject m_project = null;

        private readonly FontFamily displayFontFamily = new FontFamily("Times New Roman");

        private readonly Font  normalFont;
        private readonly Font blodFont;

        private bool validateResult = false;

        public PreCalculateForm(GEMSProject m_project)
        {
            InitializeComponent();

            this.m_project = m_project;
            this.m_project.PreCalculateGoOn += new GEMSProject.PreCalculateGoOnEventHandler(OnPreCalculateGoOn);

            normalFont = new Font(displayFontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
            blodFont = new Font(displayFontFamily, 10, FontStyle.Bold, GraphicsUnit.Point);

        }
       

        private void PreCalculateForm_Load(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;

            this.nudTimeSteps.Value = m_project.TimeStep;
            this.txtPreCalculateFileName.Text = m_project.PreCalculationFileName;

            this.txtPreCalculateFileName.TextChanged += new EventHandler(OnPreCalculateFileNameTextChanged);
            
        }

        void OnPreCalculateFileNameTextChanged(object sender, EventArgs e)
        {
            this.btnStart.Enabled = validateResult && this.txtPreCalculateFileName.Text.Trim() != string.Empty;
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            rtxtMessages.Clear();

            validateResult = m_project.Validate((int)nudTimeSteps.Value);

            this.btnStart.Enabled = validateResult && this.txtPreCalculateFileName.Text.Trim() != string.Empty;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            //Displays an SaveFileDialog,so user can select where to save
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "GEMS Project(*.pcf)|*.pcf";
            saveFileDialog1.Title = "Save File As";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPreCalculateFileName.Text = saveFileDialog1.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            string directoryName = Path.GetDirectoryName ( txtPreCalculateFileName.Text.Trim());
            if (!Directory.Exists ( directoryName ) )
            {
                if (MessageBox.Show ( "Directory \"" + directoryName + "\"" + "does not existed , Would you like to create it?" , "Warning" , MessageBoxButtons.YesNo ) == DialogResult.Yes)
                {
                    Directory.CreateDirectory ( directoryName );
                }
                else
                    return;
            }

            this.Cursor = Cursors.WaitCursor;
            this.btnOk.Enabled = false;
            this.btnCancel.Enabled = false;

            this.pbPreCalculateProgress.Value = 0;
            this.pbPreCalculateProgress.Minimum = 0;
            this.pbPreCalculateProgress.Maximum = m_project.GetPrecalculateSteps();
            this.pbPreCalculateProgress.Step = 1;
            
            m_project.PreCalculate(this.txtPreCalculateFileName.Text, (int)this.nudTimeSteps.Value);

            this.btnOk.Enabled = true;
            this.btnCancel.Enabled = true;
            this.Cursor = Cursors.Arrow;
        }

        string oldMesg = string.Empty;
        private void OnPreCalculateGoOn(object sender, GEMSProject.PreCalculateGoOnEventArgs e)
        {          
            switch (e.MsgType)
            {
                case GEMSProject.PreCalculateGoOnEventArgs.MessageType.Content:
                    rtxtMessages.AppendTextAsRtf(e.Msg, this.normalFont, GEMS.Designer.Controls.RtfColor.Black);
                    break;
                case GEMSProject.PreCalculateGoOnEventArgs.MessageType.Error:
                    rtxtMessages.AppendTextAsRtf(e.Msg, this.blodFont, GEMS.Designer.Controls.RtfColor.Red);
                    break;
                case GEMSProject.PreCalculateGoOnEventArgs.MessageType.Important:
                    rtxtMessages.AppendTextAsRtf(e.Msg, this.blodFont, GEMS.Designer.Controls.RtfColor.Green);
                    break;
                case GEMSProject.PreCalculateGoOnEventArgs.MessageType.SubTitle:
                    rtxtMessages.AppendTextAsRtf(e.Msg, this.blodFont, GEMS.Designer.Controls.RtfColor.Black);
                    break;
                case GEMSProject.PreCalculateGoOnEventArgs.MessageType.Title:
                    rtxtMessages.AppendTextAsRtf(e.Msg, this.blodFont, GEMS.Designer.Controls.RtfColor.Blue);
                    break;
                case GEMSProject.PreCalculateGoOnEventArgs.MessageType.Warning:
                    rtxtMessages.AppendTextAsRtf(e.Msg, this.normalFont, GEMS.Designer.Controls.RtfColor.Red);
                    break;
                case GEMSProject.PreCalculateGoOnEventArgs.MessageType.Step:
                    if(e.Msg != string.Empty)
                        rtxtMessages.AppendTextAsRtf(e.Msg, this.normalFont, GEMS.Designer.Controls.RtfColor.Blue);
                    this.pbPreCalculateProgress.PerformStep();
                    break;
            }

            if(e.Msg != string.Empty && e.MsgType != GEMSProject.PreCalculateGoOnEventArgs.MessageType.SubStep)
                rtxtMessages.ScrollToCaret();

        }

        private void btnOk_Click ( object sender , EventArgs e )
        {
            m_project.PreCalculationFileName = this.txtPreCalculateFileName.Text;
            m_project.TimeStep = (int)this.nudTimeSteps.Value;
        }
        

    }
}