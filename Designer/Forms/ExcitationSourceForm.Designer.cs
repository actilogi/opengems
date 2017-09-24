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

namespace GEMS.Designer.Forms
{
    partial class ExcitationSourceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container ( );
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( ExcitationSourceForm ) );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.cbDGaussian = new System.Windows.Forms.CheckBox ( );
            this.cbGaussian = new System.Windows.Forms.CheckBox ( );
            this.groupBox2 = new System.Windows.Forms.GroupBox ( );
            this.txtFrequncy = new GEMS.Designer.Controls.NumberTextBox ( );
            this.cboFrequencyUnit = new System.Windows.Forms.ComboBox ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.gbBand = new System.Windows.Forms.GroupBox ( );
            this.tbBand = new System.Windows.Forms.TrackBar ( );
            this.groupBox4 = new System.Windows.Forms.GroupBox ( );
            this.zedGraphFrequencyDomain = new ZedGraph.ZedGraphControl ( );
            this.groupBox5 = new System.Windows.Forms.GroupBox ( );
            this.zedGraphTimeDomain = new ZedGraph.ZedGraphControl ( );
            this.btnOK = new System.Windows.Forms.Button ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.groupBox1.SuspendLayout ( );
            this.groupBox2.SuspendLayout ( );
            this.gbBand.SuspendLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.tbBand ) ).BeginInit ( );
            this.groupBox4.SuspendLayout ( );
            this.groupBox5.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.cbDGaussian );
            this.groupBox1.Controls.Add ( this.cbGaussian );
            this.groupBox1.Location = new System.Drawing.Point ( 14 , 15 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Size = new System.Drawing.Size ( 351 , 65 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pulse Type";
            // 
            // cbDGaussian
            // 
            this.cbDGaussian.AutoSize = true;
            this.cbDGaussian.Location = new System.Drawing.Point ( 139 , 29 );
            this.cbDGaussian.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cbDGaussian.Name = "cbDGaussian";
            this.cbDGaussian.Size = new System.Drawing.Size ( 154 , 19 );
            this.cbDGaussian.TabIndex = 0;
            this.cbDGaussian.Text = "Differentiated Gaussian";
            this.cbDGaussian.UseVisualStyleBackColor = true;
            this.cbDGaussian.Click += new System.EventHandler ( this.cbDGaussian_Click );
            // 
            // cbGaussian
            // 
            this.cbGaussian.AutoSize = true;
            this.cbGaussian.Location = new System.Drawing.Point ( 17 , 29 );
            this.cbGaussian.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cbGaussian.Name = "cbGaussian";
            this.cbGaussian.Size = new System.Drawing.Size ( 78 , 19 );
            this.cbGaussian.TabIndex = 0;
            this.cbGaussian.Text = "Gaussian";
            this.cbGaussian.UseVisualStyleBackColor = true;
            this.cbGaussian.Click += new System.EventHandler ( this.cbGaussian_Click );
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add ( this.txtFrequncy );
            this.groupBox2.Controls.Add ( this.cboFrequencyUnit );
            this.groupBox2.Controls.Add ( this.label1 );
            this.groupBox2.Location = new System.Drawing.Point ( 380 , 15 );
            this.groupBox2.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Size = new System.Drawing.Size ( 356 , 65 );
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Maximum Working Frequency";
            // 
            // txtFrequncy
            // 
            this.txtFrequncy.Location = new System.Drawing.Point ( 101 , 25 );
            this.txtFrequncy.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtFrequncy.Name = "txtFrequncy";
            this.txtFrequncy.Size = new System.Drawing.Size ( 116 , 21 );
            this.txtFrequncy.TabIndex = 3;
            this.txtFrequncy.Text = "0";
            this.txtFrequncy.Value = 0F;
            // 
            // cboFrequencyUnit
            // 
            this.cboFrequencyUnit.FormattingEnabled = true;
            this.cboFrequencyUnit.Location = new System.Drawing.Point ( 219 , 25 );
            this.cboFrequencyUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboFrequencyUnit.Name = "cboFrequencyUnit";
            this.cboFrequencyUnit.Size = new System.Drawing.Size ( 67 , 23 );
            this.cboFrequencyUnit.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point ( 23 , 29 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 64 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Frequency";
            // 
            // gbBand
            // 
            this.gbBand.Controls.Add ( this.tbBand );
            this.gbBand.Location = new System.Drawing.Point ( 15 , 89 );
            this.gbBand.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.gbBand.Name = "gbBand";
            this.gbBand.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.gbBand.Size = new System.Drawing.Size ( 721 , 61 );
            this.gbBand.TabIndex = 1;
            this.gbBand.TabStop = false;
            this.gbBand.Text = "Working Frequency Bandwidth(3~26dB:20dB)";
            // 
            // tbBand
            // 
            this.tbBand.Location = new System.Drawing.Point ( 16 , 16 );
            this.tbBand.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.tbBand.Maximum = 26;
            this.tbBand.Minimum = 3;
            this.tbBand.Name = "tbBand";
            this.tbBand.Size = new System.Drawing.Size ( 698 , 45 );
            this.tbBand.TabIndex = 0;
            this.tbBand.Value = 20;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add ( this.zedGraphFrequencyDomain );
            this.groupBox4.Location = new System.Drawing.Point ( 17 , 155 );
            this.groupBox4.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox4.Size = new System.Drawing.Size ( 356 , 342 );
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Frequency Domain";
            // 
            // zedGraphFrequencyDomain
            // 
            this.zedGraphFrequencyDomain.EditButtons = System.Windows.Forms.MouseButtons.None;
            this.zedGraphFrequencyDomain.IsShowContextMenu = false;
            this.zedGraphFrequencyDomain.IsShowCopyMessage = false;
            this.zedGraphFrequencyDomain.Location = new System.Drawing.Point ( 7 , 25 );
            this.zedGraphFrequencyDomain.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.zedGraphFrequencyDomain.Name = "zedGraphFrequencyDomain";
            this.zedGraphFrequencyDomain.ScrollGrace = 0;
            this.zedGraphFrequencyDomain.ScrollMaxX = 0;
            this.zedGraphFrequencyDomain.ScrollMaxY = 0;
            this.zedGraphFrequencyDomain.ScrollMaxY2 = 0;
            this.zedGraphFrequencyDomain.ScrollMinX = 0;
            this.zedGraphFrequencyDomain.ScrollMinY = 0;
            this.zedGraphFrequencyDomain.ScrollMinY2 = 0;
            this.zedGraphFrequencyDomain.Size = new System.Drawing.Size ( 341 , 301 );
            this.zedGraphFrequencyDomain.TabIndex = 0;
            this.zedGraphFrequencyDomain.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add ( this.zedGraphTimeDomain );
            this.groupBox5.Location = new System.Drawing.Point ( 380 , 156 );
            this.groupBox5.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox5.Size = new System.Drawing.Size ( 356 , 341 );
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Time Domain";
            // 
            // zedGraphTimeDomain
            // 
            this.zedGraphTimeDomain.EditButtons = System.Windows.Forms.MouseButtons.None;
            this.zedGraphTimeDomain.IsShowContextMenu = false;
            this.zedGraphTimeDomain.IsShowCopyMessage = false;
            this.zedGraphTimeDomain.Location = new System.Drawing.Point ( 8 , 25 );
            this.zedGraphTimeDomain.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.zedGraphTimeDomain.Name = "zedGraphTimeDomain";
            this.zedGraphTimeDomain.ScrollGrace = 0;
            this.zedGraphTimeDomain.ScrollMaxX = 0;
            this.zedGraphTimeDomain.ScrollMaxY = 0;
            this.zedGraphTimeDomain.ScrollMaxY2 = 0;
            this.zedGraphTimeDomain.ScrollMinX = 0;
            this.zedGraphTimeDomain.ScrollMinY = 0;
            this.zedGraphTimeDomain.ScrollMinY2 = 0;
            this.zedGraphTimeDomain.Size = new System.Drawing.Size ( 341 , 301 );
            this.zedGraphTimeDomain.TabIndex = 0;
            this.zedGraphTimeDomain.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point ( 478 , 505 );
            this.btnOK.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size ( 119 , 29 );
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler ( this.btnOK_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point ( 617 , 505 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 119 , 29 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler ( this.btnCancel_Click );
            // 
            // ExcitationSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 751 , 546 );
            this.Controls.Add ( this.groupBox5 );
            this.Controls.Add ( this.btnCancel );
            this.Controls.Add ( this.btnOK );
            this.Controls.Add ( this.groupBox4 );
            this.Controls.Add ( this.gbBand );
            this.Controls.Add ( this.groupBox2 );
            this.Controls.Add ( this.groupBox1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExcitationSourceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Excitation Source";
            this.Load += new System.EventHandler ( this.ExcitationSourceForm_Load );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            this.groupBox2.ResumeLayout ( false );
            this.groupBox2.PerformLayout ( );
            this.gbBand.ResumeLayout ( false );
            this.gbBand.PerformLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.tbBand ) ).EndInit ( );
            this.groupBox4.ResumeLayout ( false );
            this.groupBox5.ResumeLayout ( false );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox gbBand;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private GEMS.Designer.Controls.NumberTextBox txtFrequncy;
        private System.Windows.Forms.ComboBox cboFrequencyUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbBand;
        private System.Windows.Forms.CheckBox cbDGaussian;
        private System.Windows.Forms.CheckBox cbGaussian;
        private ZedGraph.ZedGraphControl zedGraphFrequencyDomain;
        private ZedGraph.ZedGraphControl zedGraphTimeDomain;
    }
}