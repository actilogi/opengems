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
    partial class ExcitationOutputForm
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
            this.gbPhaseDelay = new System.Windows.Forms.GroupBox ( );
            this.cboFreqUnit = new System.Windows.Forms.ComboBox ( );
            this.label5 = new System.Windows.Forms.Label ( );
            this.txtFreq = new GEMS.Designer.Controls.NumberTextBox ( );
            this.label4 = new System.Windows.Forms.Label ( );
            this.txtPhase = new GEMS.Designer.Controls.NumberTextBox ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.txtE0 = new System.Windows.Forms.TextBox ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.cboTimeDelayUnit = new System.Windows.Forms.ComboBox ( );
            this.btnOK = new System.Windows.Forms.Button ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.gbTimeDelay = new System.Windows.Forms.GroupBox ( );
            this.txtTimeDelay = new GEMS.Designer.Controls.NumberTextBox ( );
            this.cboExcitationUnit = new System.Windows.Forms.ComboBox ( );
            this.txtExcitationValue = new GEMS.Designer.Controls.NumberTextBox ( );
            this.lbExcitationType = new System.Windows.Forms.Label ( );
            this.cboSignValue = new System.Windows.Forms.ComboBox ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.txtIOType = new System.Windows.Forms.TextBox ( );
            this.gbPhaseDelay.SuspendLayout ( );
            this.groupBox1.SuspendLayout ( );
            this.gbTimeDelay.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // gbPhaseDelay
            // 
            this.gbPhaseDelay.Controls.Add ( this.cboFreqUnit );
            this.gbPhaseDelay.Controls.Add ( this.label5 );
            this.gbPhaseDelay.Controls.Add ( this.txtFreq );
            this.gbPhaseDelay.Controls.Add ( this.label4 );
            this.gbPhaseDelay.Controls.Add ( this.txtPhase );
            this.gbPhaseDelay.Location = new System.Drawing.Point ( 14 , 147 );
            this.gbPhaseDelay.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.gbPhaseDelay.Name = "gbPhaseDelay";
            this.gbPhaseDelay.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.gbPhaseDelay.Size = new System.Drawing.Size ( 374 , 61 );
            this.gbPhaseDelay.TabIndex = 5;
            this.gbPhaseDelay.TabStop = false;
            this.gbPhaseDelay.Text = "Phase Delay ( degree )";
            // 
            // cboFreqUnit
            // 
            this.cboFreqUnit.FormattingEnabled = true;
            this.cboFreqUnit.Location = new System.Drawing.Point ( 311 , 26 );
            this.cboFreqUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboFreqUnit.Name = "cboFreqUnit";
            this.cboFreqUnit.Size = new System.Drawing.Size ( 53 , 23 );
            this.cboFreqUnit.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point ( 171 , 31 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size ( 35 , 15 );
            this.label5.TabIndex = 2;
            this.label5.Text = "Freq.";
            // 
            // txtFreq
            // 
            this.txtFreq.Location = new System.Drawing.Point ( 223 , 26 );
            this.txtFreq.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtFreq.Name = "txtFreq";
            this.txtFreq.Size = new System.Drawing.Size ( 83 , 21 );
            this.txtFreq.TabIndex = 3;
            this.txtFreq.Text = "0";
            this.txtFreq.Value = 0F;
            this.txtFreq.TextChanged += new System.EventHandler ( this.txtFreq_TextChanged );
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point ( 15 , 31 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size ( 42 , 15 );
            this.label4.TabIndex = 2;
            this.label4.Text = "Phase";
            // 
            // txtPhase
            // 
            this.txtPhase.Location = new System.Drawing.Point ( 61 , 26 );
            this.txtPhase.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtPhase.Name = "txtPhase";
            this.txtPhase.Size = new System.Drawing.Size ( 83 , 21 );
            this.txtPhase.TabIndex = 3;
            this.txtPhase.Text = "0";
            this.txtPhase.Value = 0F;
            this.txtPhase.TextChanged += new System.EventHandler ( this.txtPhase_TextChanged );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point ( 330 , 280 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler ( this.btnCancel_Click );
            // 
            // txtE0
            // 
            this.txtE0.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtE0.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtE0.Location = new System.Drawing.Point ( 48 , 19 );
            this.txtE0.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtE0.Name = "txtE0";
            this.txtE0.ReadOnly = true;
            this.txtE0.Size = new System.Drawing.Size ( 122 , 21 );
            this.txtE0.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point ( 188 , 24 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 33 , 15 );
            this.label2.TabIndex = 9;
            this.label2.Text = "Type";
            // 
            // cboTimeDelayUnit
            // 
            this.cboTimeDelayUnit.FormattingEnabled = true;
            this.cboTimeDelayUnit.Location = new System.Drawing.Point ( 152 , 19 );
            this.cboTimeDelayUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboTimeDelayUnit.Name = "cboTimeDelayUnit";
            this.cboTimeDelayUnit.Size = new System.Drawing.Size ( 53 , 23 );
            this.cboTimeDelayUnit.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point ( 236 , 280 );
            this.btnOK.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler ( this.btnOK_Click );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point ( 14 , 24 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 27 , 15 );
            this.label1.TabIndex = 10;
            this.label1.Text = "E/O";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.gbPhaseDelay );
            this.groupBox1.Controls.Add ( this.gbTimeDelay );
            this.groupBox1.Controls.Add ( this.cboExcitationUnit );
            this.groupBox1.Controls.Add ( this.txtExcitationValue );
            this.groupBox1.Controls.Add ( this.lbExcitationType );
            this.groupBox1.Controls.Add ( this.cboSignValue );
            this.groupBox1.Controls.Add ( this.label3 );
            this.groupBox1.Location = new System.Drawing.Point ( 16 , 54 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Size = new System.Drawing.Size ( 401 , 219 );
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // gbTimeDelay
            // 
            this.gbTimeDelay.Controls.Add ( this.cboTimeDelayUnit );
            this.gbTimeDelay.Controls.Add ( this.txtTimeDelay );
            this.gbTimeDelay.Location = new System.Drawing.Point ( 14 , 75 );
            this.gbTimeDelay.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.gbTimeDelay.Name = "gbTimeDelay";
            this.gbTimeDelay.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.gbTimeDelay.Size = new System.Drawing.Size ( 374 , 54 );
            this.gbTimeDelay.TabIndex = 5;
            this.gbTimeDelay.TabStop = false;
            this.gbTimeDelay.Text = "Time Delay";
            // 
            // txtTimeDelay
            // 
            this.txtTimeDelay.Location = new System.Drawing.Point ( 17 , 20 );
            this.txtTimeDelay.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtTimeDelay.Name = "txtTimeDelay";
            this.txtTimeDelay.Size = new System.Drawing.Size ( 126 , 21 );
            this.txtTimeDelay.TabIndex = 5;
            this.txtTimeDelay.Text = "0";
            this.txtTimeDelay.Value = 0F;
            // 
            // cboExcitationUnit
            // 
            this.cboExcitationUnit.FormattingEnabled = true;
            this.cboExcitationUnit.Location = new System.Drawing.Point ( 325 , 25 );
            this.cboExcitationUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboExcitationUnit.Name = "cboExcitationUnit";
            this.cboExcitationUnit.Size = new System.Drawing.Size ( 53 , 23 );
            this.cboExcitationUnit.TabIndex = 4;
            // 
            // txtExcitationValue
            // 
            this.txtExcitationValue.Location = new System.Drawing.Point ( 237 , 26 );
            this.txtExcitationValue.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtExcitationValue.Name = "txtExcitationValue";
            this.txtExcitationValue.Size = new System.Drawing.Size ( 83 , 21 );
            this.txtExcitationValue.TabIndex = 3;
            this.txtExcitationValue.Text = "0";
            this.txtExcitationValue.Value = 0F;
            // 
            // lbExcitationType
            // 
            this.lbExcitationType.AutoSize = true;
            this.lbExcitationType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbExcitationType.Location = new System.Drawing.Point ( 171 , 30 );
            this.lbExcitationType.Name = "lbExcitationType";
            this.lbExcitationType.Size = new System.Drawing.Size ( 48 , 15 );
            this.lbExcitationType.TabIndex = 2;
            this.lbExcitationType.Text = "Voltage";
            // 
            // cboSignValue
            // 
            this.cboSignValue.FormattingEnabled = true;
            this.cboSignValue.Items.AddRange ( new object[] {
            "_",
            "+"} );
            this.cboSignValue.Location = new System.Drawing.Point ( 48 , 25 );
            this.cboSignValue.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboSignValue.Name = "cboSignValue";
            this.cboSignValue.Size = new System.Drawing.Size ( 105 , 23 );
            this.cboSignValue.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point ( 7 , 30 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 32 , 15 );
            this.label3.TabIndex = 0;
            this.label3.Text = "Sign";
            // 
            // txtIOType
            // 
            this.txtIOType.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtIOType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtIOType.Location = new System.Drawing.Point ( 235 , 19 );
            this.txtIOType.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtIOType.Name = "txtIOType";
            this.txtIOType.ReadOnly = true;
            this.txtIOType.Size = new System.Drawing.Size ( 175 , 21 );
            this.txtIOType.TabIndex = 12;
            // 
            // ExcitationOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 434 , 319 );
            this.Controls.Add ( this.btnCancel );
            this.Controls.Add ( this.txtE0 );
            this.Controls.Add ( this.label2 );
            this.Controls.Add ( this.btnOK );
            this.Controls.Add ( this.label1 );
            this.Controls.Add ( this.groupBox1 );
            this.Controls.Add ( this.txtIOType );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExcitationOutputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Excitation/Output";
            this.Load += new System.EventHandler ( this.ExcitationOutputForm_Load );
            this.gbPhaseDelay.ResumeLayout ( false );
            this.gbPhaseDelay.PerformLayout ( );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            this.gbTimeDelay.ResumeLayout ( false );
            this.gbTimeDelay.PerformLayout ( );
            this.ResumeLayout ( false );
            this.PerformLayout ( );

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPhaseDelay;
        private System.Windows.Forms.ComboBox cboFreqUnit;
        private System.Windows.Forms.Label label5;
        private GEMS.Designer.Controls.NumberTextBox txtFreq;
        private System.Windows.Forms.Label label4;
        private GEMS.Designer.Controls.NumberTextBox txtPhase;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtE0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTimeDelayUnit;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbTimeDelay;
        private GEMS.Designer.Controls.NumberTextBox txtTimeDelay;
        private System.Windows.Forms.ComboBox cboExcitationUnit;
        private System.Windows.Forms.Label lbExcitationType;
        private System.Windows.Forms.ComboBox cboSignValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIOType;
        private GEMS.Designer.Controls.NumberTextBox txtExcitationValue;
    }
}