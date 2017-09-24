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
    partial class GobalModelPane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( GobalModelPane ) );
            this.paneCaption1 = new GEMS.Panes.PaneCaption ( );
            this.d3d = new GEMS.Designer.Direct3D.Direct3d ( );
            this.SuspendLayout ( );
            // 
            // paneCaption1
            // 
            this.paneCaption1.AllowActive = false;
            this.paneCaption1.AntiAlias = false;
            this.paneCaption1.Caption = "Model Preview";
            this.paneCaption1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneCaption1.Font = new System.Drawing.Font ( "Tahoma" , 10.5F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.paneCaption1.Location = new System.Drawing.Point ( 0 , 0 );
            this.paneCaption1.Name = "paneCaption1";
            this.paneCaption1.Size = new System.Drawing.Size ( 335 , 26 );
            this.paneCaption1.TabIndex = 1;
            // 
            // d3d
            // 
            this.d3d.BackColor = System.Drawing.Color.White;
            this.d3d.Dock = System.Windows.Forms.DockStyle.Fill;
            this.d3d.DxAutoResize = false;
            this.d3d.DxBackBufferCount = 2;
            this.d3d.DxFullScreen = false;
            this.d3d.DxSimulateFullScreen = false;
            this.d3d.Location = new System.Drawing.Point ( 0 , 26 );
            this.d3d.Name = "d3d";
            this.d3d.Projection = ( (Microsoft.DirectX.Matrix)( resources.GetObject ( "d3d.Projection" ) ) );
            this.d3d.Size = new System.Drawing.Size ( 335 , 275 );
            this.d3d.TabIndex = 2;
            this.d3d.View = ( (Microsoft.DirectX.Matrix)( resources.GetObject ( "d3d.View" ) ) );
            this.d3d.MouseUp += new System.Windows.Forms.MouseEventHandler ( this.d3d_MouseUp );
            this.d3d.MouseMove += new System.Windows.Forms.MouseEventHandler ( this.d3d_MouseMove );
            this.d3d.MouseDown += new System.Windows.Forms.MouseEventHandler ( this.d3d_MouseDown );
            this.d3d.MouseWheel +=new System.Windows.Forms.MouseEventHandler(this.d3d_MouseWheel);
            this.d3d.DxLoaded += new GEMS.Designer.Direct3D.Direct3d.DxDirect3dDelegate ( this.d3d_DxLoaded );
            // 
            // GobalModelPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add ( this.d3d );
            this.Controls.Add ( this.paneCaption1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.Name = "GobalModelPane";
            this.Size = new System.Drawing.Size ( 335 , 301 );
            this.Load += new System.EventHandler ( this.ModelPane_Load );
            this.ResumeLayout ( false );

        }

        #endregion

        private GEMS.Panes.PaneCaption paneCaption1;
        private Direct3d d3d;

    }
}
