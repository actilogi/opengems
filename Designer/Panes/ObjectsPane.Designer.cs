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
    partial class ObjectsPane
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
            this.tvObjects = new System.Windows.Forms.TreeView ( );
            this.paneCaption1 = new GEMS.Panes.PaneCaption ( );
            this.SuspendLayout ( );
            // 
            // tvObjects
            // 
            this.tvObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvObjects.Font = new System.Drawing.Font ( "Tahoma" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.tvObjects.HideSelection = false;
            this.tvObjects.Location = new System.Drawing.Point ( 0 , 27 );
            this.tvObjects.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.Size = new System.Drawing.Size ( 253 , 280 );
            this.tvObjects.TabIndex = 2;
            this.tvObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler ( this.tvObjects_AfterSelect );
            this.tvObjects.KeyDown += new System.Windows.Forms.KeyEventHandler ( this.tvObjects_KeyDown );
            // 
            // paneCaption1
            // 
            this.paneCaption1.AllowActive = false;
            this.paneCaption1.AntiAlias = false;
            this.paneCaption1.Caption = "Objects Browser";
            this.paneCaption1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneCaption1.Font = new System.Drawing.Font ( "Tahoma" , 10.5F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.paneCaption1.Location = new System.Drawing.Point ( 0 , 0 );
            this.paneCaption1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.paneCaption1.Name = "paneCaption1";
            this.paneCaption1.Size = new System.Drawing.Size ( 253 , 27 );
            this.paneCaption1.TabIndex = 1;
            // 
            // ObjectsPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add ( this.tvObjects );
            this.Controls.Add ( this.paneCaption1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.Name = "ObjectsPane";
            this.Size = new System.Drawing.Size ( 253 , 307 );
            this.Load += new System.EventHandler ( this.ObjectsPane_Load );
            this.ResumeLayout ( false );

        }

        #endregion

        private GEMS.Panes.PaneCaption paneCaption1;
        private System.Windows.Forms.TreeView tvObjects;
    }
}
