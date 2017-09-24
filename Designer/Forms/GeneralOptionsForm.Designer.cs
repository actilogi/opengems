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
    partial class GeneralOptionsForm
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
            this.cboLengthUnit = new System.Windows.Forms.ComboBox ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.btnOK = new System.Windows.Forms.Button ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.groupBox1.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // cboLengthUnit
            // 
            this.cboLengthUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.cboLengthUnit.FormattingEnabled = true;
            this.cboLengthUnit.Location = new System.Drawing.Point ( 110 , 25 );
            this.cboLengthUnit.Margin = new System.Windows.Forms.Padding ( 3 , 5 , 3 , 5 );
            this.cboLengthUnit.Name = "cboLengthUnit";
            this.cboLengthUnit.Size = new System.Drawing.Size ( 154 , 23 );
            this.cboLengthUnit.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label1.Location = new System.Drawing.Point ( 17 , 30 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 70 , 15 );
            this.label1.TabIndex = 3;
            this.label1.Text = "Length Unit";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.label1 );
            this.groupBox1.Controls.Add ( this.cboLengthUnit );
            this.groupBox1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.groupBox1.Location = new System.Drawing.Point ( 12 , 11 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Size = new System.Drawing.Size ( 293 , 69 );
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unit";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnOK.Location = new System.Drawing.Point ( 128 , 96 );
            this.btnOK.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size ( 85 , 25 );
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler ( this.btnOK_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnCancel.Location = new System.Drawing.Point ( 220 , 96 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 85 , 25 );
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler ( this.btnCancel_Click );
            // 
            // GeneralOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 316 , 137 );
            this.Controls.Add ( this.btnCancel );
            this.Controls.Add ( this.btnOK );
            this.Controls.Add ( this.groupBox1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeneralOptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "General Options";
            this.Load += new System.EventHandler ( this.GeneralOptionsForm_Load );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.ComboBox cboLengthUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}