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
    partial class MaterialDetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( MaterialDetailsForm ) );
            this.groupBox2 = new System.Windows.Forms.GroupBox ( );
            this.groupBox4 = new System.Windows.Forms.GroupBox ( );
            this.txtSigmaX = new GEMS.Designer.Controls.NumberTextBox ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.txtEpsilonX = new GEMS.Designer.Controls.NumberTextBox ( );
            this.label6 = new System.Windows.Forms.Label ( );
            this.label5 = new System.Windows.Forms.Label ( );
            this.txtMusigmaX = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtMuX = new GEMS.Designer.Controls.NumberTextBox ( );
            this.label4 = new System.Windows.Forms.Label ( );
            this.groupBox3 = new System.Windows.Forms.GroupBox ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.txtDetails = new System.Windows.Forms.TextBox ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.txtMaterialName = new System.Windows.Forms.TextBox ( );
            this.btnOK = new System.Windows.Forms.Button ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.groupBox2.SuspendLayout ( );
            this.groupBox4.SuspendLayout ( );
            this.groupBox3.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add ( this.groupBox4 );
            this.groupBox2.Controls.Add ( this.groupBox3 );
            this.groupBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox2.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.groupBox2.Location = new System.Drawing.Point ( 12 , 13 );
            this.groupBox2.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Size = new System.Drawing.Size ( 432 , 334 );
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Material Details";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add ( this.txtSigmaX );
            this.groupBox4.Controls.Add ( this.label3 );
            this.groupBox4.Controls.Add ( this.txtEpsilonX );
            this.groupBox4.Controls.Add ( this.label6 );
            this.groupBox4.Controls.Add ( this.label5 );
            this.groupBox4.Controls.Add ( this.txtMusigmaX );
            this.groupBox4.Controls.Add ( this.txtMuX );
            this.groupBox4.Controls.Add ( this.label4 );
            this.groupBox4.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox4.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.groupBox4.Location = new System.Drawing.Point ( 6 , 179 );
            this.groupBox4.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox4.Size = new System.Drawing.Size ( 412 , 140 );
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "All-direction (Note : Every materil is isotropic)";
            // 
            // txtSigmaX
            // 
            this.txtSigmaX.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtSigmaX.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtSigmaX.Location = new System.Drawing.Point ( 173 , 52 );
            this.txtSigmaX.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtSigmaX.Name = "txtSigmaX";
            this.txtSigmaX.Size = new System.Drawing.Size ( 224 , 21 );
            this.txtSigmaX.TabIndex = 3;
            this.txtSigmaX.Text = "0";
            this.txtSigmaX.Value = 0F;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label3.Location = new System.Drawing.Point ( 56 , 30 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 111 , 15 );
            this.label3.TabIndex = 2;
            this.label3.Text = "Relative permittivity";
            // 
            // txtEpsilonX
            // 
            this.txtEpsilonX.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtEpsilonX.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtEpsilonX.Location = new System.Drawing.Point ( 173 , 28 );
            this.txtEpsilonX.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtEpsilonX.Name = "txtEpsilonX";
            this.txtEpsilonX.Size = new System.Drawing.Size ( 224 , 21 );
            this.txtEpsilonX.TabIndex = 3;
            this.txtEpsilonX.Text = "0";
            this.txtEpsilonX.Value = 0F;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label6.Location = new System.Drawing.Point ( 13 , 106 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size ( 154 , 15 );
            this.label6.TabIndex = 2;
            this.label6.Text = "Magnetic conductivity(H/m)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label5.Location = new System.Drawing.Point ( 46 , 80 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size ( 121 , 15 );
            this.label5.TabIndex = 2;
            this.label5.Text = "Relative permeability";
            // 
            // txtMusigmaX
            // 
            this.txtMusigmaX.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtMusigmaX.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtMusigmaX.Location = new System.Drawing.Point ( 173 , 103 );
            this.txtMusigmaX.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtMusigmaX.Name = "txtMusigmaX";
            this.txtMusigmaX.Size = new System.Drawing.Size ( 224 , 21 );
            this.txtMusigmaX.TabIndex = 3;
            this.txtMusigmaX.Text = "0";
            this.txtMusigmaX.Value = 0F;
            // 
            // txtMuX
            // 
            this.txtMuX.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtMuX.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtMuX.Location = new System.Drawing.Point ( 173 , 77 );
            this.txtMuX.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtMuX.Name = "txtMuX";
            this.txtMuX.Size = new System.Drawing.Size ( 224 , 21 );
            this.txtMuX.TabIndex = 3;
            this.txtMuX.Text = "0";
            this.txtMuX.Value = 0F;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.label4.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label4.Location = new System.Drawing.Point ( 66 , 55 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size ( 101 , 15 );
            this.label4.TabIndex = 2;
            this.label4.Text = "Conductivity(S/m)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add ( this.label1 );
            this.groupBox3.Controls.Add ( this.txtDetails );
            this.groupBox3.Controls.Add ( this.label2 );
            this.groupBox3.Controls.Add ( this.txtMaterialName );
            this.groupBox3.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox3.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.groupBox3.Location = new System.Drawing.Point ( 6 , 22 );
            this.groupBox3.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox3.Size = new System.Drawing.Size ( 412 , 149 );
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label1.Location = new System.Drawing.Point ( 43 , 23 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 41 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // txtDetails
            // 
            this.txtDetails.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtDetails.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtDetails.Location = new System.Drawing.Point ( 87 , 45 );
            this.txtDetails.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.Size = new System.Drawing.Size ( 312 , 88 );
            this.txtDetails.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label2.Location = new System.Drawing.Point ( 15 , 45 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 69 , 15 );
            this.label2.TabIndex = 0;
            this.label2.Text = "Description";
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtMaterialName.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtMaterialName.Location = new System.Drawing.Point ( 87 , 20 );
            this.txtMaterialName.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new System.Drawing.Size ( 312 , 21 );
            this.txtMaterialName.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnOK.Location = new System.Drawing.Point ( 236 , 356 );
            this.btnOK.Margin = new System.Windows.Forms.Padding ( 3 , 5 , 3 , 5 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size ( 101 , 30 );
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler ( this.btnOK_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnCancel.Location = new System.Drawing.Point ( 343 , 356 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 5 , 3 , 5 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 101 , 30 );
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler ( this.btnCancel_Click );
            // 
            // MaterialDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 455 , 395 );
            this.Controls.Add ( this.btnOK );
            this.Controls.Add ( this.btnCancel );
            this.Controls.Add ( this.groupBox2 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Regular Material";
            this.Load += new System.EventHandler ( this.MaterialDetailsForm_Load );
            this.groupBox2.ResumeLayout ( false );
            this.groupBox4.ResumeLayout ( false );
            this.groupBox4.PerformLayout ( );
            this.groupBox3.ResumeLayout ( false );
            this.groupBox3.PerformLayout ( );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private GEMS.Designer.Controls.NumberTextBox txtSigmaX;
        private System.Windows.Forms.Label label3;
        private GEMS.Designer.Controls.NumberTextBox txtEpsilonX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private GEMS.Designer.Controls.NumberTextBox txtMusigmaX;
        private GEMS.Designer.Controls.NumberTextBox txtMuX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMaterialName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}