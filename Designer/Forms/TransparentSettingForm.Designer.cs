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
    partial class TransparentSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( TransparentSettingForm ) );
            this.tbTranparency = new System.Windows.Forms.TrackBar ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.pbTransparencyDisplay = new System.Windows.Forms.PictureBox ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.btnOk = new System.Windows.Forms.Button ( );
            ( (System.ComponentModel.ISupportInitialize)( this.tbTranparency ) ).BeginInit ( );
            ( (System.ComponentModel.ISupportInitialize)( this.pbTransparencyDisplay ) ).BeginInit ( );
            this.SuspendLayout ( );
            // 
            // tbTranparency
            // 
            this.tbTranparency.Location = new System.Drawing.Point ( 100 , 19 );
            this.tbTranparency.Name = "tbTranparency";
            this.tbTranparency.Size = new System.Drawing.Size ( 286 , 45 );
            this.tbTranparency.TabIndex = 0;
            this.tbTranparency.Scroll += new System.EventHandler ( this.tbTranparency_Scroll );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point ( 13 , 19 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 81 , 15 );
            this.label1.TabIndex = 1;
            this.label1.Text = "Transparency";
            // 
            // pbTransparencyDisplay
            // 
            this.pbTransparencyDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTransparencyDisplay.Location = new System.Drawing.Point ( 392 , 19 );
            this.pbTransparencyDisplay.Name = "pbTransparencyDisplay";
            this.pbTransparencyDisplay.Size = new System.Drawing.Size ( 90 , 28 );
            this.pbTransparencyDisplay.TabIndex = 2;
            this.pbTransparencyDisplay.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point ( 392 , 63 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 88 , 27 );
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point ( 298 , 63 );
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size ( 88 , 27 );
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // TransparentSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 492 , 103 );
            this.Controls.Add ( this.btnOk );
            this.Controls.Add ( this.btnCancel );
            this.Controls.Add ( this.pbTransparencyDisplay );
            this.Controls.Add ( this.label1 );
            this.Controls.Add ( this.tbTranparency );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransparentSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transparent";
            this.Load += new System.EventHandler ( this.TransparentSettingForm_Load );
            ( (System.ComponentModel.ISupportInitialize)( this.tbTranparency ) ).EndInit ( );
            ( (System.ComponentModel.ISupportInitialize)( this.pbTransparencyDisplay ) ).EndInit ( );
            this.ResumeLayout ( false );
            this.PerformLayout ( );

        }

        #endregion

        private System.Windows.Forms.TrackBar tbTranparency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbTransparencyDisplay;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}