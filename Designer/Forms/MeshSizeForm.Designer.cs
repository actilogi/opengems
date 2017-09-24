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
    partial class MeshSizeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( MeshSizeForm ) );
            this.txtZSize = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtYSize = new GEMS.Designer.Controls.NumberTextBox ( );
            this.txtXSize = new GEMS.Designer.Controls.NumberTextBox ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.btnMesh = new System.Windows.Forms.Button ( );
            this.btnClose = new System.Windows.Forms.Button ( );
            this.lblXSizeUnit = new System.Windows.Forms.Label ( );
            this.lblYSizeUnit = new System.Windows.Forms.Label ( );
            this.lblZSizeUnit = new System.Windows.Forms.Label ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.btnDetails = new System.Windows.Forms.Button ( );
            this.groupBox1.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // txtZSize
            // 
            this.txtZSize.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtZSize.Location = new System.Drawing.Point ( 89 , 79 );
            this.txtZSize.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtZSize.Name = "txtZSize";
            this.txtZSize.Size = new System.Drawing.Size ( 149 , 21 );
            this.txtZSize.TabIndex = 6;
            this.txtZSize.Text = "0";
            this.txtZSize.Value = 0F;
            // 
            // txtYSize
            // 
            this.txtYSize.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtYSize.Location = new System.Drawing.Point ( 89 , 53 );
            this.txtYSize.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtYSize.Name = "txtYSize";
            this.txtYSize.Size = new System.Drawing.Size ( 149 , 21 );
            this.txtYSize.TabIndex = 4;
            this.txtYSize.Text = "0";
            this.txtYSize.Value = 0F;
            // 
            // txtXSize
            // 
            this.txtXSize.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.txtXSize.Location = new System.Drawing.Point ( 89 , 26 );
            this.txtXSize.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtXSize.Name = "txtXSize";
            this.txtXSize.Size = new System.Drawing.Size ( 149 , 21 );
            this.txtXSize.TabIndex = 3;
            this.txtXSize.Text = "0";
            this.txtXSize.Value = 0F;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label3.Location = new System.Drawing.Point ( 17 , 82 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 65 , 15 );
            this.label3.TabIndex = 0;
            this.label3.Text = "Z-direction";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label2.Location = new System.Drawing.Point ( 17 , 56 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 65 , 15 );
            this.label2.TabIndex = 0;
            this.label2.Text = "Y-direction";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.label1.Location = new System.Drawing.Point ( 17 , 29 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 66 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "X-direction";
            // 
            // btnMesh
            // 
            this.btnMesh.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnMesh.Location = new System.Drawing.Point ( 3 , 131 );
            this.btnMesh.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnMesh.Name = "btnMesh";
            this.btnMesh.Size = new System.Drawing.Size ( 83 , 27 );
            this.btnMesh.TabIndex = 7;
            this.btnMesh.Text = "Mesh";
            this.btnMesh.UseVisualStyleBackColor = true;
            this.btnMesh.Click += new System.EventHandler ( this.btnMesh_Click );
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnClose.Location = new System.Drawing.Point ( 242 , 131 );
            this.btnClose.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size ( 83 , 27 );
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler ( this.btnClose_Click );
            // 
            // lblXSizeUnit
            // 
            this.lblXSizeUnit.AutoSize = true;
            this.lblXSizeUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.lblXSizeUnit.Location = new System.Drawing.Point ( 239 , 29 );
            this.lblXSizeUnit.Name = "lblXSizeUnit";
            this.lblXSizeUnit.Size = new System.Drawing.Size ( 41 , 15 );
            this.lblXSizeUnit.TabIndex = 8;
            this.lblXSizeUnit.Text = "label4";
            // 
            // lblYSizeUnit
            // 
            this.lblYSizeUnit.AutoSize = true;
            this.lblYSizeUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.lblYSizeUnit.Location = new System.Drawing.Point ( 239 , 56 );
            this.lblYSizeUnit.Name = "lblYSizeUnit";
            this.lblYSizeUnit.Size = new System.Drawing.Size ( 41 , 15 );
            this.lblYSizeUnit.TabIndex = 8;
            this.lblYSizeUnit.Text = "label4";
            // 
            // lblZSizeUnit
            // 
            this.lblZSizeUnit.AutoSize = true;
            this.lblZSizeUnit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.lblZSizeUnit.Location = new System.Drawing.Point ( 239 , 82 );
            this.lblZSizeUnit.Name = "lblZSizeUnit";
            this.lblZSizeUnit.Size = new System.Drawing.Size ( 41 , 15 );
            this.lblZSizeUnit.TabIndex = 8;
            this.lblZSizeUnit.Text = "label4";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.label1 );
            this.groupBox1.Controls.Add ( this.lblZSizeUnit );
            this.groupBox1.Controls.Add ( this.txtXSize );
            this.groupBox1.Controls.Add ( this.lblYSizeUnit );
            this.groupBox1.Controls.Add ( this.label3 );
            this.groupBox1.Controls.Add ( this.lblXSizeUnit );
            this.groupBox1.Controls.Add ( this.label2 );
            this.groupBox1.Controls.Add ( this.txtYSize );
            this.groupBox1.Controls.Add ( this.txtZSize );
            this.groupBox1.Location = new System.Drawing.Point ( 3 , 7 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size ( 322 , 114 );
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mininum Cell Size";
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point ( 100 , 131 );
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size ( 83 , 27 );
            this.btnDetails.TabIndex = 10;
            this.btnDetails.Text = "Details...";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler ( this.btnDetails_Click );
            // 
            // MeshSizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 328 , 164 );
            this.Controls.Add ( this.btnDetails );
            this.Controls.Add ( this.groupBox1 );
            this.Controls.Add ( this.btnClose );
            this.Controls.Add ( this.btnMesh );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeshSizeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Mesh";
            this.Load += new System.EventHandler ( this.MeshSizeForm_Load );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            this.ResumeLayout ( false );

        }

        #endregion

        private GEMS.Designer.Controls.NumberTextBox txtZSize;
        private GEMS.Designer.Controls.NumberTextBox txtYSize;
        private GEMS.Designer.Controls.NumberTextBox txtXSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMesh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblXSizeUnit;
        private System.Windows.Forms.Label lblYSizeUnit;
        private System.Windows.Forms.Label lblZSizeUnit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDetails;

    }
}