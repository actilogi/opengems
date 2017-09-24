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
    partial class PropertiesPane
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid ( );
            this.paneCaption1 = new GEMS.Panes.PaneCaption ( );
            this.SuspendLayout ( );
            // 
            // propertyGrid
            // 
            this.propertyGrid.CommandsDisabledLinkColor = System.Drawing.Color.Silver;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Font = new System.Drawing.Font ( "Tahoma" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.propertyGrid.Location = new System.Drawing.Point ( 0 , 27 );
            this.propertyGrid.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size ( 276 , 344 );
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.SelectedObjectsChanged += new System.EventHandler ( this.propertyGrid_SelectedObjectsChanged );
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler ( this.propertyGrid_PropertyValueChanged );
            // 
            // paneCaption1
            // 
            this.paneCaption1.AllowActive = false;
            this.paneCaption1.AntiAlias = false;
            this.paneCaption1.Caption = "Properties";
            this.paneCaption1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneCaption1.Font = new System.Drawing.Font ( "Tahoma" , 10.5F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.paneCaption1.Location = new System.Drawing.Point ( 0 , 0 );
            this.paneCaption1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.paneCaption1.Name = "paneCaption1";
            this.paneCaption1.Size = new System.Drawing.Size ( 276 , 27 );
            this.paneCaption1.TabIndex = 0;
            // 
            // PropertiesPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add ( this.propertyGrid );
            this.Controls.Add ( this.paneCaption1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.Name = "PropertiesPane";
            this.Size = new System.Drawing.Size ( 276 , 371 );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private GEMS.Panes.PaneCaption paneCaption1;
    }
}
