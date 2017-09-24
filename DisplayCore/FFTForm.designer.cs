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

namespace GEMS.Display.Core
{
    partial class FFTForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if (disposing && ( components != null ))
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( FFTForm ) );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.label4 = new System.Windows.Forms.Label ( );
            this.label6 = new System.Windows.Forms.Label ( );
            this.label5 = new System.Windows.Forms.Label ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.cboFreqUnits = new System.Windows.Forms.ComboBox ( );
            this.txtEndFreq = new System.Windows.Forms.TextBox ( );
            this.txtPowof2 = new System.Windows.Forms.TextBox ( );
            this.txtDataLength = new System.Windows.Forms.TextBox ( );
            this.txtSourceDataLength = new System.Windows.Forms.TextBox ( );
            this.txtFreqStep = new System.Windows.Forms.TextBox ( );
            this.txtStartFreq = new System.Windows.Forms.TextBox ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.groupBox3 = new System.Windows.Forms.GroupBox ( );
            this.rbRi = new System.Windows.Forms.RadioButton ( );
            this.rbAp = new System.Windows.Forms.RadioButton ( );
            this.btnCalculate = new System.Windows.Forms.Button ( );
            this.btnPreview = new System.Windows.Forms.Button ( );
            this.btnClose = new System.Windows.Forms.Button ( );
            this.groupBox1.SuspendLayout ( );
            this.groupBox3.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.label4 );
            this.groupBox1.Controls.Add ( this.label6 );
            this.groupBox1.Controls.Add ( this.label5 );
            this.groupBox1.Controls.Add ( this.label3 );
            this.groupBox1.Controls.Add ( this.cboFreqUnits );
            this.groupBox1.Controls.Add ( this.txtEndFreq );
            this.groupBox1.Controls.Add ( this.txtPowof2 );
            this.groupBox1.Controls.Add ( this.txtDataLength );
            this.groupBox1.Controls.Add ( this.txtSourceDataLength );
            this.groupBox1.Controls.Add ( this.txtFreqStep );
            this.groupBox1.Controls.Add ( this.txtStartFreq );
            this.groupBox1.Controls.Add ( this.label2 );
            this.groupBox1.Controls.Add ( this.label1 );
            this.groupBox1.Location = new System.Drawing.Point ( 13 , 13 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size ( 519 , 82 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FFT Setting";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point ( 309 , 22 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size ( 32 , 15 );
            this.label4.TabIndex = 3;
            this.label4.Text = "Step";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point ( 326 , 53 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size ( 14 , 15 );
            this.label6.TabIndex = 3;
            this.label6.Text = "=";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point ( 181 , 53 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size ( 119 , 15 );
            this.label5.TabIndex = 3;
            this.label5.Text = "Expand Length of 2^";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point ( 6 , 53 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 74 , 15 );
            this.label3.TabIndex = 3;
            this.label3.Text = "Data Length";
            // 
            // cboFreqUnits
            // 
            this.cboFreqUnits.FormattingEnabled = true;
            this.cboFreqUnits.Location = new System.Drawing.Point ( 442 , 18 );
            this.cboFreqUnits.Name = "cboFreqUnits";
            this.cboFreqUnits.Size = new System.Drawing.Size ( 56 , 23 );
            this.cboFreqUnits.TabIndex = 2;
            this.cboFreqUnits.SelectedIndexChanged += new System.EventHandler ( this.cboFreqUnits_SelectedIndexChanged );
            // 
            // txtEndFreq
            // 
            this.txtEndFreq.Location = new System.Drawing.Point ( 209 , 19 );
            this.txtEndFreq.Name = "txtEndFreq";
            this.txtEndFreq.Size = new System.Drawing.Size ( 94 , 21 );
            this.txtEndFreq.TabIndex = 1;
            // 
            // txtPowof2
            // 
            this.txtPowof2.Location = new System.Drawing.Point ( 301 , 50 );
            this.txtPowof2.Name = "txtPowof2";
            this.txtPowof2.ReadOnly = true;
            this.txtPowof2.Size = new System.Drawing.Size ( 22 , 21 );
            this.txtPowof2.TabIndex = 1;
            // 
            // txtDataLength
            // 
            this.txtDataLength.Location = new System.Drawing.Point ( 342 , 50 );
            this.txtDataLength.Name = "txtDataLength";
            this.txtDataLength.ReadOnly = true;
            this.txtDataLength.Size = new System.Drawing.Size ( 94 , 21 );
            this.txtDataLength.TabIndex = 1;
            // 
            // txtSourceDataLength
            // 
            this.txtSourceDataLength.Location = new System.Drawing.Point ( 81 , 50 );
            this.txtSourceDataLength.Name = "txtSourceDataLength";
            this.txtSourceDataLength.ReadOnly = true;
            this.txtSourceDataLength.Size = new System.Drawing.Size ( 93 , 21 );
            this.txtSourceDataLength.TabIndex = 1;
            // 
            // txtFreqStep
            // 
            this.txtFreqStep.Location = new System.Drawing.Point ( 342 , 19 );
            this.txtFreqStep.Name = "txtFreqStep";
            this.txtFreqStep.ReadOnly = true;
            this.txtFreqStep.Size = new System.Drawing.Size ( 94 , 21 );
            this.txtFreqStep.TabIndex = 1;
            // 
            // txtStartFreq
            // 
            this.txtStartFreq.Location = new System.Drawing.Point ( 80 , 19 );
            this.txtStartFreq.Name = "txtStartFreq";
            this.txtStartFreq.Size = new System.Drawing.Size ( 94 , 21 );
            this.txtStartFreq.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point ( 181 , 22 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 29 , 15 );
            this.label2.TabIndex = 0;
            this.label2.Text = "End";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point ( 48 , 22 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 32 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Start";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add ( this.rbRi );
            this.groupBox3.Controls.Add ( this.rbAp );
            this.groupBox3.Location = new System.Drawing.Point ( 13 , 101 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size ( 519 , 54 );
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Preview Setting";
            // 
            // rbRi
            // 
            this.rbRi.AutoSize = true;
            this.rbRi.Location = new System.Drawing.Point ( 114 , 25 );
            this.rbRi.Name = "rbRi";
            this.rbRi.Size = new System.Drawing.Size ( 82 , 19 );
            this.rbRi.TabIndex = 0;
            this.rbRi.TabStop = true;
            this.rbRi.Text = "Real/Imag";
            this.rbRi.UseVisualStyleBackColor = true;
            // 
            // rbAp
            // 
            this.rbAp.AutoSize = true;
            this.rbAp.Location = new System.Drawing.Point ( 9 , 25 );
            this.rbAp.Name = "rbAp";
            this.rbAp.Size = new System.Drawing.Size ( 88 , 19 );
            this.rbAp.TabIndex = 0;
            this.rbAp.TabStop = true;
            this.rbAp.Text = "Amp/Phase";
            this.rbAp.UseVisualStyleBackColor = true;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point ( 246 , 162 );
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size ( 90 , 27 );
            this.btnCalculate.TabIndex = 3;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler ( this.btnCalculate_Click );
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point ( 346 , 162 );
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size ( 90 , 27 );
            this.btnPreview.TabIndex = 3;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler ( this.btnPreview_Click );
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point ( 442 , 162 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size ( 90 , 27 );
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler ( this.btnClose_Click );
            // 
            // FFTForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 544 , 198 );
            this.Controls.Add ( this.btnClose );
            this.Controls.Add ( this.btnPreview );
            this.Controls.Add ( this.btnCalculate );
            this.Controls.Add ( this.groupBox3 );
            this.Controls.Add ( this.groupBox1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FFTForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FFT ";
            this.Load += new System.EventHandler ( this.FFTForm_Load );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            this.groupBox3.ResumeLayout ( false );
            this.groupBox3.PerformLayout ( );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEndFreq;
        private System.Windows.Forms.TextBox txtStartFreq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboFreqUnits;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbRi;
        private System.Windows.Forms.RadioButton rbAp;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSourceDataLength;
        private System.Windows.Forms.TextBox txtFreqStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPowof2;
        private System.Windows.Forms.TextBox txtDataLength;
    }
}