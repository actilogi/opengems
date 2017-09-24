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
    partial class FieldOutputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( FieldOutputForm ) );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.btnOk = new System.Windows.Forms.Button ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.cboHFieldZ = new System.Windows.Forms.ComboBox ( );
            this.cboHFieldY = new System.Windows.Forms.ComboBox ( );
            this.cboHFieldX = new System.Windows.Forms.ComboBox ( );
            this.cboEFieldZ = new System.Windows.Forms.ComboBox ( );
            this.cboEFieldY = new System.Windows.Forms.ComboBox ( );
            this.cboEFieldX = new System.Windows.Forms.ComboBox ( );
            this.label9 = new System.Windows.Forms.Label ( );
            this.label6 = new System.Windows.Forms.Label ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.label8 = new System.Windows.Forms.Label ( );
            this.label7 = new System.Windows.Forms.Label ( );
            this.label5 = new System.Windows.Forms.Label ( );
            this.label4 = new System.Windows.Forms.Label ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.groupBox1.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnCancel.Location = new System.Drawing.Point ( 312 , 149 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnOk.Location = new System.Drawing.Point ( 219 , 149 );
            this.btnOk.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler ( this.btnOk_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.cboHFieldZ );
            this.groupBox1.Controls.Add ( this.cboHFieldY );
            this.groupBox1.Controls.Add ( this.cboHFieldX );
            this.groupBox1.Controls.Add ( this.cboEFieldZ );
            this.groupBox1.Controls.Add ( this.cboEFieldY );
            this.groupBox1.Controls.Add ( this.cboEFieldX );
            this.groupBox1.Controls.Add ( this.label9 );
            this.groupBox1.Controls.Add ( this.label6 );
            this.groupBox1.Controls.Add ( this.label3 );
            this.groupBox1.Controls.Add ( this.label8 );
            this.groupBox1.Controls.Add ( this.label7 );
            this.groupBox1.Controls.Add ( this.label5 );
            this.groupBox1.Controls.Add ( this.label4 );
            this.groupBox1.Controls.Add ( this.label2 );
            this.groupBox1.Controls.Add ( this.label1 );
            this.groupBox1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.groupBox1.Location = new System.Drawing.Point ( 12 , 13 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Size = new System.Drawing.Size ( 385 , 126 );
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time Domain";
            // 
            // cboHFieldZ
            // 
            this.cboHFieldZ.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboHFieldZ.FormattingEnabled = true;
            this.cboHFieldZ.Items.AddRange ( new object[] {
            "none",
            "+",
            "-"} );
            this.cboHFieldZ.Location = new System.Drawing.Point ( 284 , 81 );
            this.cboHFieldZ.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboHFieldZ.Name = "cboHFieldZ";
            this.cboHFieldZ.Size = new System.Drawing.Size ( 75 , 23 );
            this.cboHFieldZ.TabIndex = 2;
            // 
            // cboHFieldY
            // 
            this.cboHFieldY.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboHFieldY.FormattingEnabled = true;
            this.cboHFieldY.Items.AddRange ( new object[] {
            "none",
            "+",
            "-"} );
            this.cboHFieldY.Location = new System.Drawing.Point ( 284 , 56 );
            this.cboHFieldY.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboHFieldY.Name = "cboHFieldY";
            this.cboHFieldY.Size = new System.Drawing.Size ( 75 , 23 );
            this.cboHFieldY.TabIndex = 2;
            // 
            // cboHFieldX
            // 
            this.cboHFieldX.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboHFieldX.FormattingEnabled = true;
            this.cboHFieldX.Items.AddRange ( new object[] {
            "none",
            "+",
            "-"} );
            this.cboHFieldX.Location = new System.Drawing.Point ( 284 , 31 );
            this.cboHFieldX.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboHFieldX.Name = "cboHFieldX";
            this.cboHFieldX.Size = new System.Drawing.Size ( 75 , 23 );
            this.cboHFieldX.TabIndex = 2;
            // 
            // cboEFieldZ
            // 
            this.cboEFieldZ.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboEFieldZ.FormattingEnabled = true;
            this.cboEFieldZ.Items.AddRange ( new object[] {
            "none",
            "+",
            "-"} );
            this.cboEFieldZ.Location = new System.Drawing.Point ( 143 , 81 );
            this.cboEFieldZ.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboEFieldZ.Name = "cboEFieldZ";
            this.cboEFieldZ.Size = new System.Drawing.Size ( 75 , 23 );
            this.cboEFieldZ.TabIndex = 2;
            // 
            // cboEFieldY
            // 
            this.cboEFieldY.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboEFieldY.FormattingEnabled = true;
            this.cboEFieldY.Items.AddRange ( new object[] {
            "none",
            "+",
            "-"} );
            this.cboEFieldY.Location = new System.Drawing.Point ( 143 , 56 );
            this.cboEFieldY.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboEFieldY.Name = "cboEFieldY";
            this.cboEFieldY.Size = new System.Drawing.Size ( 75 , 23 );
            this.cboEFieldY.TabIndex = 2;
            // 
            // cboEFieldX
            // 
            this.cboEFieldX.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboEFieldX.FormattingEnabled = true;
            this.cboEFieldX.Items.AddRange ( new object[] {
            "none",
            "+",
            "-"} );
            this.cboEFieldX.Location = new System.Drawing.Point ( 143 , 31 );
            this.cboEFieldX.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.cboEFieldX.Name = "cboEFieldX";
            this.cboEFieldX.Size = new System.Drawing.Size ( 75 , 23 );
            this.cboEFieldX.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label9.Location = new System.Drawing.Point ( 237 , 85 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size ( 43 , 15 );
            this.label9.TabIndex = 1;
            this.label9.Text = "H-field";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label6.Location = new System.Drawing.Point ( 237 , 60 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size ( 43 , 15 );
            this.label6.TabIndex = 1;
            this.label6.Text = "H-field";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label3.Location = new System.Drawing.Point ( 237 , 35 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 43 , 15 );
            this.label3.TabIndex = 1;
            this.label3.Text = "H-field";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label8.Location = new System.Drawing.Point ( 98 , 85 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size ( 42 , 15 );
            this.label8.TabIndex = 1;
            this.label8.Text = "E-field";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label7.Location = new System.Drawing.Point ( 17 , 85 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size ( 71 , 15 );
            this.label7.TabIndex = 0;
            this.label7.Text = "Z-direction :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label5.Location = new System.Drawing.Point ( 98 , 60 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size ( 42 , 15 );
            this.label5.TabIndex = 1;
            this.label5.Text = "E-field";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label4.Location = new System.Drawing.Point ( 17 , 60 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size ( 71 , 15 );
            this.label4.TabIndex = 0;
            this.label4.Text = "Y-direction :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label2.Location = new System.Drawing.Point ( 98 , 35 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 42 , 15 );
            this.label2.TabIndex = 1;
            this.label2.Text = "E-field";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label1.Location = new System.Drawing.Point ( 17 , 35 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 72 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "X-direction :";
            // 
            // FieldOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 410 , 188 );
            this.Controls.Add ( this.groupBox1 );
            this.Controls.Add ( this.btnOk );
            this.Controls.Add ( this.btnCancel );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FieldOutputForm";
            this.Text = "Field Output";
            this.Load += new System.EventHandler ( this.FieldOutputForm_Load );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboHFieldX;
        private System.Windows.Forms.ComboBox cboEFieldX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboHFieldZ;
        private System.Windows.Forms.ComboBox cboHFieldY;
        private System.Windows.Forms.ComboBox cboEFieldZ;
        private System.Windows.Forms.ComboBox cboEFieldY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}