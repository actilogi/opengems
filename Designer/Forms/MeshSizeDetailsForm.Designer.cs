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
    partial class MeshSizeDetailsForm
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
            this.btnClose = new System.Windows.Forms.Button ( );
            this.ertxtMeshDetails = new GEMS.Designer.Controls.ExRichTextBox ( );
            this.SuspendLayout ( );
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point ( 377 , 305 );
            this.btnClose.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ertxtMeshDetails
            // 
            this.ertxtMeshDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ertxtMeshDetails.HiglightColor = GEMS.Designer.Controls.RtfColor.White;
            this.ertxtMeshDetails.Location = new System.Drawing.Point ( 3 , 3 );
            this.ertxtMeshDetails.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.ertxtMeshDetails.Name = "ertxtMeshDetails";
            this.ertxtMeshDetails.Size = new System.Drawing.Size ( 461 , 294 );
            this.ertxtMeshDetails.TabIndex = 0;
            this.ertxtMeshDetails.Text = "";
            this.ertxtMeshDetails.TextColor = GEMS.Designer.Controls.RtfColor.Black;
            // 
            // MeshSizeDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 467 , 342 );
            this.Controls.Add ( this.btnClose );
            this.Controls.Add ( this.ertxtMeshDetails );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeshSizeDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mesh Details";
            this.Load += new System.EventHandler ( this.MeshSizeDetailsForm_Load );
            this.ResumeLayout ( false );

        }

        #endregion

        private GEMS.Designer.Controls.ExRichTextBox ertxtMeshDetails;
        private System.Windows.Forms.Button btnClose;
    }
}