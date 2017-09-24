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

namespace GEMS.Display.Core
{
    [System.ComponentModel.ToolboxItemAttribute ( false )]
    partial class DisplayGuidPane
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose ( bool disposing )
        {
            if (disposing && ( components != null ))
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.components = new System.ComponentModel.Container ( );
            this.splitContainer1 = new System.Windows.Forms.SplitContainer ( );
            this.tvResultGuide = new System.Windows.Forms.TreeView ( );
            this.btnOpenFile = new System.Windows.Forms.Button ( );
            this.lblResultFileName = new System.Windows.Forms.Label ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.btnFFT = new System.Windows.Forms.Button ( );
            this.paneCaption1 = new GEMS.Display.Core.PaneCaption ( );
            this.toolTip = new System.Windows.Forms.ToolTip ( this.components );
            this.splitContainer1.Panel1.SuspendLayout ( );
            this.splitContainer1.Panel2.SuspendLayout ( );
            this.splitContainer1.SuspendLayout ( );
            this.groupBox1.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point ( 0 , 20 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add ( this.tvResultGuide );
            this.splitContainer1.Panel1.Controls.Add ( this.btnOpenFile );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add ( this.lblResultFileName );
            this.splitContainer1.Panel2.Controls.Add ( this.groupBox1 );
            this.splitContainer1.Size = new System.Drawing.Size ( 189 , 495 );
            this.splitContainer1.SplitterDistance = 398;
            this.splitContainer1.TabIndex = 7;
            // 
            // tvResultGuide
            // 
            this.tvResultGuide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvResultGuide.Location = new System.Drawing.Point ( 0 , 0 );
            this.tvResultGuide.Name = "tvResultGuide";
            this.tvResultGuide.Size = new System.Drawing.Size ( 189 , 368 );
            this.tvResultGuide.TabIndex = 1;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOpenFile.Location = new System.Drawing.Point ( 0 , 368 );
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size ( 189 , 30 );
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Load...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler ( this.btnOpenFile_Click );
            // 
            // lblResultFileName
            // 
            this.lblResultFileName.AutoSize = true;
            this.lblResultFileName.Location = new System.Drawing.Point ( 6 , 67 );
            this.lblResultFileName.Name = "lblResultFileName";
            this.lblResultFileName.Size = new System.Drawing.Size ( 0 , 15 );
            this.lblResultFileName.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.btnFFT );
            this.groupBox1.Location = new System.Drawing.Point ( 0 , 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size ( 189 , 60 );
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Process";
            // 
            // btnFFT
            // 
            this.btnFFT.Location = new System.Drawing.Point ( 6 , 20 );
            this.btnFFT.Name = "btnFFT";
            this.btnFFT.Size = new System.Drawing.Size ( 87 , 26 );
            this.btnFFT.TabIndex = 0;
            this.btnFFT.Text = "FFT";
            this.btnFFT.UseVisualStyleBackColor = true;
            this.btnFFT.Click += new System.EventHandler ( this.btnFFT_Click );
            // 
            // paneCaption1
            // 
            this.paneCaption1.AllowActive = false;
            this.paneCaption1.AntiAlias = false;
            this.paneCaption1.Caption = "OpenGEMS Results";
            this.paneCaption1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneCaption1.Font = new System.Drawing.Font ( "Tahoma" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.paneCaption1.Location = new System.Drawing.Point ( 0 , 0 );
            this.paneCaption1.Name = "paneCaption1";
            this.paneCaption1.Size = new System.Drawing.Size ( 189 , 20 );
            this.paneCaption1.TabIndex = 5;
            // 
            // DisplayGuidPane
            // 
            this.Controls.Add ( this.splitContainer1 );
            this.Controls.Add ( this.paneCaption1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.Name = "DisplayGuidPane";
            this.Size = new System.Drawing.Size ( 189 , 515 );
            this.Load += new System.EventHandler ( this.apcDisplayGuid_Load );
            this.splitContainer1.Panel1.ResumeLayout ( false );
            this.splitContainer1.Panel2.ResumeLayout ( false );
            this.splitContainer1.Panel2.PerformLayout ( );
            this.splitContainer1.ResumeLayout ( false );
            this.groupBox1.ResumeLayout ( false );
            this.ResumeLayout ( false );

        }

        #endregion

        private PaneCaption paneCaption1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvResultGuide;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFFT;
        private System.Windows.Forms.Label lblResultFileName;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
