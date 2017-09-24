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

namespace GEMS.Designer.Panes
{
    using GEMS.Designer.Direct3D;
    partial class ModelPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( ModelPane ) );
            this.d3d = new GEMS.Designer.Direct3D.Direct3d ( );
            this.SuspendLayout ( );
            // 
            // d3d
            // 
            this.d3d.BackColor = System.Drawing.SystemColors.Window;
            this.d3d.Dock = System.Windows.Forms.DockStyle.Fill;
            this.d3d.DxAutoResize = false;
            this.d3d.DxBackBufferCount = 2;
            this.d3d.DxFullScreen = false;
            this.d3d.DxSimulateFullScreen = false;
            this.d3d.Location = new System.Drawing.Point ( 0 , 0 );
            this.d3d.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.d3d.Name = "d3d";
            this.d3d.Projection = ( (Microsoft.DirectX.Matrix)( resources.GetObject ( "d3d.Projection" ) ) );
            this.d3d.Size = new System.Drawing.Size ( 853 , 835 );
            this.d3d.TabIndex = 0;
            this.d3d.View = ( (Microsoft.DirectX.Matrix)( resources.GetObject ( "d3d.View" ) ) );
            this.d3d.DxLoaded += new GEMS.Designer.Direct3D.Direct3d.DxDirect3dDelegate ( this.d3d_DxLoaded );
            // 
            // ModelPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add ( this.d3d );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.Name = "ModelPane";
            this.Size = new System.Drawing.Size ( 853 , 835 );
            this.Load += new System.EventHandler ( this.ModelPane_Load );
            this.ResumeLayout ( false );

        }

        #endregion

        private GEMS.Designer.Direct3D.Direct3d d3d;

    }
}
