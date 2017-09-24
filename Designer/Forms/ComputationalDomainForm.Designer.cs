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
    partial class ComputationalDomainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( ComputationalDomainForm ) );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.txtZmax = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtZmin = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtYmax = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtYmin = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtXmax = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtXmin = new GEMS.Designer.Controls.NumberTextBox ( );
            this.cboZmaxUnit = new System.Windows.Forms.ComboBox ( );
            this.cboZminUnit = new System.Windows.Forms.ComboBox ( );
            this.cboYmaxUnit = new System.Windows.Forms.ComboBox ( );
            this.cboYminUnit = new System.Windows.Forms.ComboBox ( );
            this.cboXmaxUnit = new System.Windows.Forms.ComboBox ( );
            this.cboXminUnit = new System.Windows.Forms.ComboBox ( );
            this.label6 = new System.Windows.Forms.Label ( );
            this.label5 = new System.Windows.Forms.Label ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.label4 = new System.Windows.Forms.Label ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.btnAutoRange = new System.Windows.Forms.Button ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.btnOK = new System.Windows.Forms.Button ( );
            this.groupBox2 = new System.Windows.Forms.GroupBox ( );
            this.cboZmaxCondUnit = new System.Windows.Forms.ComboBox ( );
            this.cboZminCondUnit = new System.Windows.Forms.ComboBox ( );
            this.cboYmaxCondUnit = new System.Windows.Forms.ComboBox ( );
            this.cboYminCondUnit = new System.Windows.Forms.ComboBox ( );
            this.cboXmaxCondUnit = new System.Windows.Forms.ComboBox ( );
            this.cboXminCondUnit = new System.Windows.Forms.ComboBox ( );
            this.label7 = new System.Windows.Forms.Label ( );
            this.label8 = new System.Windows.Forms.Label ( );
            this.label9 = new System.Windows.Forms.Label ( );
            this.label10 = new System.Windows.Forms.Label ( );
            this.label11 = new System.Windows.Forms.Label ( );
            this.label12 = new System.Windows.Forms.Label ( );
            this.groupBox1.SuspendLayout ( );
            this.groupBox2.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.txtZmax );
            this.groupBox1.Controls.Add ( this.txtZmin );
            this.groupBox1.Controls.Add ( this.txtYmax );
            this.groupBox1.Controls.Add ( this.txtYmin );
            this.groupBox1.Controls.Add ( this.txtXmax );
            this.groupBox1.Controls.Add ( this.txtXmin );
            this.groupBox1.Controls.Add ( this.cboZmaxUnit );
            this.groupBox1.Controls.Add ( this.cboZminUnit );
            this.groupBox1.Controls.Add ( this.cboYmaxUnit );
            this.groupBox1.Controls.Add ( this.cboYminUnit );
            this.groupBox1.Controls.Add ( this.cboXmaxUnit );
            this.groupBox1.Controls.Add ( this.cboXminUnit );
            this.groupBox1.Controls.Add ( this.label6 );
            this.groupBox1.Controls.Add ( this.label5 );
            this.groupBox1.Controls.Add ( this.label3 );
            this.groupBox1.Controls.Add ( this.label4 );
            this.groupBox1.Controls.Add ( this.label2 );
            this.groupBox1.Controls.Add ( this.label1 );
            this.groupBox1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.groupBox1.Location = new System.Drawing.Point ( 14 , 15 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Size = new System.Drawing.Size ( 569 , 71 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulation Range";
            // 
            // txtZmax
            // 
            this.txtZmax.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtZmax.Location = new System.Drawing.Point ( 427 , 45 );
            this.txtZmax.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtZmax.Name = "txtZmax";
            this.txtZmax.Size = new System.Drawing.Size ( 71 , 21 );
            this.txtZmax.TabIndex = 7;
            this.txtZmax.Text = "0";
            this.txtZmax.Value = 0F;
            // 
            // txtZmin
            // 
            this.txtZmin.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtZmin.Location = new System.Drawing.Point ( 427 , 22 );
            this.txtZmin.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtZmin.Name = "txtZmin";
            this.txtZmin.Size = new System.Drawing.Size ( 71 , 21 );
            this.txtZmin.TabIndex = 6;
            this.txtZmin.Text = "0";
            this.txtZmin.Value = 0F;
            // 
            // txtYmax
            // 
            this.txtYmax.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtYmax.Location = new System.Drawing.Point ( 234 , 45 );
            this.txtYmax.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtYmax.Name = "txtYmax";
            this.txtYmax.Size = new System.Drawing.Size ( 71 , 21 );
            this.txtYmax.TabIndex = 5;
            this.txtYmax.Text = "0";
            this.txtYmax.Value = 0F;
            // 
            // txtYmin
            // 
            this.txtYmin.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtYmin.Location = new System.Drawing.Point ( 234 , 22 );
            this.txtYmin.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtYmin.Name = "txtYmin";
            this.txtYmin.Size = new System.Drawing.Size ( 71 , 21 );
            this.txtYmin.TabIndex = 4;
            this.txtYmin.Text = "0";
            this.txtYmin.Value = 0F;
            // 
            // txtXmax
            // 
            this.txtXmax.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtXmax.Location = new System.Drawing.Point ( 50 , 45 );
            this.txtXmax.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtXmax.Name = "txtXmax";
            this.txtXmax.Size = new System.Drawing.Size ( 71 , 21 );
            this.txtXmax.TabIndex = 3;
            this.txtXmax.Text = "0";
            this.txtXmax.Value = 0F;
            // 
            // txtXmin
            // 
            this.txtXmin.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtXmin.Location = new System.Drawing.Point ( 50 , 22 );
            this.txtXmin.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtXmin.Name = "txtXmin";
            this.txtXmin.Size = new System.Drawing.Size ( 71 , 21 );
            this.txtXmin.TabIndex = 3;
            this.txtXmin.Text = "0";
            this.txtXmin.Value = 0F;
            // 
            // cboZmaxUnit
            // 
            this.cboZmaxUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboZmaxUnit.FormattingEnabled = true;
            this.cboZmaxUnit.Location = new System.Drawing.Point ( 500 , 45 );
            this.cboZmaxUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboZmaxUnit.Name = "cboZmaxUnit";
            this.cboZmaxUnit.Size = new System.Drawing.Size ( 52 , 23 );
            this.cboZmaxUnit.TabIndex = 2;
            // 
            // cboZminUnit
            // 
            this.cboZminUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboZminUnit.FormattingEnabled = true;
            this.cboZminUnit.Location = new System.Drawing.Point ( 500 , 22 );
            this.cboZminUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboZminUnit.Name = "cboZminUnit";
            this.cboZminUnit.Size = new System.Drawing.Size ( 52 , 23 );
            this.cboZminUnit.TabIndex = 2;
            // 
            // cboYmaxUnit
            // 
            this.cboYmaxUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboYmaxUnit.FormattingEnabled = true;
            this.cboYmaxUnit.Location = new System.Drawing.Point ( 307 , 45 );
            this.cboYmaxUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboYmaxUnit.Name = "cboYmaxUnit";
            this.cboYmaxUnit.Size = new System.Drawing.Size ( 52 , 23 );
            this.cboYmaxUnit.TabIndex = 2;
            // 
            // cboYminUnit
            // 
            this.cboYminUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboYminUnit.FormattingEnabled = true;
            this.cboYminUnit.Location = new System.Drawing.Point ( 307 , 22 );
            this.cboYminUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboYminUnit.Name = "cboYminUnit";
            this.cboYminUnit.Size = new System.Drawing.Size ( 52 , 23 );
            this.cboYminUnit.TabIndex = 2;
            // 
            // cboXmaxUnit
            // 
            this.cboXmaxUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboXmaxUnit.FormattingEnabled = true;
            this.cboXmaxUnit.Location = new System.Drawing.Point ( 123 , 45 );
            this.cboXmaxUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboXmaxUnit.Name = "cboXmaxUnit";
            this.cboXmaxUnit.Size = new System.Drawing.Size ( 52 , 23 );
            this.cboXmaxUnit.TabIndex = 2;
            // 
            // cboXminUnit
            // 
            this.cboXminUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboXminUnit.FormattingEnabled = true;
            this.cboXminUnit.Location = new System.Drawing.Point ( 123 , 22 );
            this.cboXminUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboXminUnit.Name = "cboXminUnit";
            this.cboXminUnit.Size = new System.Drawing.Size ( 52 , 23 );
            this.cboXminUnit.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label6.Location = new System.Drawing.Point ( 390 , 49 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size ( 38 , 15 );
            this.label6.TabIndex = 0;
            this.label6.Text = "ZMax";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label5.Location = new System.Drawing.Point ( 199 , 49 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size ( 38 , 15 );
            this.label5.TabIndex = 0;
            this.label5.Text = "YMax";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label3.Location = new System.Drawing.Point ( 390 , 27 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 35 , 15 );
            this.label3.TabIndex = 0;
            this.label3.Text = "ZMin";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label4.Location = new System.Drawing.Point ( 13 , 49 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size ( 39 , 15 );
            this.label4.TabIndex = 0;
            this.label4.Text = "XMax";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label2.Location = new System.Drawing.Point ( 199 , 27 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 35 , 15 );
            this.label2.TabIndex = 0;
            this.label2.Text = "YMin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label1.Location = new System.Drawing.Point ( 13 , 27 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 36 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "XMin";
            // 
            // btnAutoRange
            // 
            this.btnAutoRange.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnAutoRange.Location = new System.Drawing.Point ( 14 , 174 );
            this.btnAutoRange.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnAutoRange.Name = "btnAutoRange";
            this.btnAutoRange.Size = new System.Drawing.Size ( 87 , 25 );
            this.btnAutoRange.TabIndex = 3;
            this.btnAutoRange.Text = "Auto Range";
            this.btnAutoRange.UseVisualStyleBackColor = true;
            this.btnAutoRange.Click += new System.EventHandler ( this.btnAutoRange_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnCancel.Location = new System.Drawing.Point ( 496 , 174 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 87 , 25 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler ( this.btnCancel_Click );
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnOK.Location = new System.Drawing.Point ( 403 , 174 );
            this.btnOK.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size ( 87 , 25 );
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler ( this.btnOK_Click );
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add ( this.cboZmaxCondUnit );
            this.groupBox2.Controls.Add ( this.cboZminCondUnit );
            this.groupBox2.Controls.Add ( this.cboYmaxCondUnit );
            this.groupBox2.Controls.Add ( this.cboYminCondUnit );
            this.groupBox2.Controls.Add ( this.cboXmaxCondUnit );
            this.groupBox2.Controls.Add ( this.cboXminCondUnit );
            this.groupBox2.Controls.Add ( this.label7 );
            this.groupBox2.Controls.Add ( this.label8 );
            this.groupBox2.Controls.Add ( this.label9 );
            this.groupBox2.Controls.Add ( this.label10 );
            this.groupBox2.Controls.Add ( this.label11 );
            this.groupBox2.Controls.Add ( this.label12 );
            this.groupBox2.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.groupBox2.Location = new System.Drawing.Point ( 14 , 94 );
            this.groupBox2.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Size = new System.Drawing.Size ( 569 , 73 );
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Computational Domain Boundary Condition";
            // 
            // cboZmaxCondUnit
            // 
            this.cboZmaxCondUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboZmaxCondUnit.FormattingEnabled = true;
            this.cboZmaxCondUnit.Location = new System.Drawing.Point ( 427 , 43 );
            this.cboZmaxCondUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboZmaxCondUnit.Name = "cboZmaxCondUnit";
            this.cboZmaxCondUnit.Size = new System.Drawing.Size ( 125 , 23 );
            this.cboZmaxCondUnit.TabIndex = 2;
            // 
            // cboZminCondUnit
            // 
            this.cboZminCondUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboZminCondUnit.FormattingEnabled = true;
            this.cboZminCondUnit.Location = new System.Drawing.Point ( 427 , 22 );
            this.cboZminCondUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboZminCondUnit.Name = "cboZminCondUnit";
            this.cboZminCondUnit.Size = new System.Drawing.Size ( 125 , 23 );
            this.cboZminCondUnit.TabIndex = 2;
            // 
            // cboYmaxCondUnit
            // 
            this.cboYmaxCondUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboYmaxCondUnit.FormattingEnabled = true;
            this.cboYmaxCondUnit.Location = new System.Drawing.Point ( 233 , 43 );
            this.cboYmaxCondUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboYmaxCondUnit.Name = "cboYmaxCondUnit";
            this.cboYmaxCondUnit.Size = new System.Drawing.Size ( 125 , 23 );
            this.cboYmaxCondUnit.TabIndex = 2;
            // 
            // cboYminCondUnit
            // 
            this.cboYminCondUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboYminCondUnit.FormattingEnabled = true;
            this.cboYminCondUnit.Location = new System.Drawing.Point ( 233 , 22 );
            this.cboYminCondUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboYminCondUnit.Name = "cboYminCondUnit";
            this.cboYminCondUnit.Size = new System.Drawing.Size ( 125 , 23 );
            this.cboYminCondUnit.TabIndex = 2;
            // 
            // cboXmaxCondUnit
            // 
            this.cboXmaxCondUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboXmaxCondUnit.FormattingEnabled = true;
            this.cboXmaxCondUnit.Location = new System.Drawing.Point ( 50 , 43 );
            this.cboXmaxCondUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboXmaxCondUnit.Name = "cboXmaxCondUnit";
            this.cboXmaxCondUnit.Size = new System.Drawing.Size ( 125 , 23 );
            this.cboXmaxCondUnit.TabIndex = 2;
            // 
            // cboXminCondUnit
            // 
            this.cboXminCondUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboXminCondUnit.FormattingEnabled = true;
            this.cboXminCondUnit.Location = new System.Drawing.Point ( 50 , 22 );
            this.cboXminCondUnit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboXminCondUnit.Name = "cboXminCondUnit";
            this.cboXminCondUnit.Size = new System.Drawing.Size ( 125 , 23 );
            this.cboXminCondUnit.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label7.Location = new System.Drawing.Point ( 390 , 48 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size ( 38 , 15 );
            this.label7.TabIndex = 0;
            this.label7.Text = "ZMax";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label8.Location = new System.Drawing.Point ( 198 , 48 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size ( 38 , 15 );
            this.label8.TabIndex = 0;
            this.label8.Text = "YMax";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label9.Location = new System.Drawing.Point ( 390 , 27 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size ( 35 , 15 );
            this.label9.TabIndex = 0;
            this.label9.Text = "ZMin";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label10.Location = new System.Drawing.Point ( 13 , 48 );
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size ( 39 , 15 );
            this.label10.TabIndex = 0;
            this.label10.Text = "XMax";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label11.Location = new System.Drawing.Point ( 198 , 27 );
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size ( 35 , 15 );
            this.label11.TabIndex = 0;
            this.label11.Text = "YMin";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label12.Location = new System.Drawing.Point ( 13 , 27 );
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size ( 36 , 15 );
            this.label12.TabIndex = 0;
            this.label12.Text = "XMin";
            // 
            // ComputationalDomainForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size ( 598 , 210 );
            this.Controls.Add ( this.btnOK );
            this.Controls.Add ( this.btnCancel );
            this.Controls.Add ( this.btnAutoRange );
            this.Controls.Add ( this.groupBox2 );
            this.Controls.Add ( this.groupBox1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputationalDomainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Computational Domain";
            this.Load += new System.EventHandler ( this.ComputationalDomainForm_Load );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            this.groupBox2.ResumeLayout ( false );
            this.groupBox2.PerformLayout ( );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboXminUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboZminUnit;
        private System.Windows.Forms.ComboBox cboYminUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAutoRange;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboZmaxUnit;
        private System.Windows.Forms.ComboBox cboYmaxUnit;
        private System.Windows.Forms.ComboBox cboXmaxUnit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboZmaxCondUnit;
        private System.Windows.Forms.ComboBox cboZminCondUnit;
        private System.Windows.Forms.ComboBox cboYmaxCondUnit;
        private System.Windows.Forms.ComboBox cboYminCondUnit;
        private System.Windows.Forms.ComboBox cboXmaxCondUnit;
        private System.Windows.Forms.ComboBox cboXminCondUnit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private GEMS.Designer.Controls.NumberTextBox txtZmax;
        private GEMS.Designer.Controls.NumberTextBox txtZmin;
        private GEMS.Designer.Controls.NumberTextBox txtYmax;
        private GEMS.Designer.Controls.NumberTextBox txtYmin;
        private GEMS.Designer.Controls.NumberTextBox txtXmax;
        private GEMS.Designer.Controls.NumberTextBox txtXmin;
    }
}