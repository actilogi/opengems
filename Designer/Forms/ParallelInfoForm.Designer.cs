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
    partial class ParallelInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( ParallelInfoForm ) );
            this.splitContainer1 = new System.Windows.Forms.SplitContainer ( );
            this.d3d = new GEMS.Designer.Direct3D.Direct3d ( );
            this.btnReset = new System.Windows.Forms.Button ( );
            this.groupBox4 = new System.Windows.Forms.GroupBox ( );
            this.txtBalance = new System.Windows.Forms.TextBox ( );
            this.txtCPUNumber = new System.Windows.Forms.TextBox ( );
            this.txtZMeshCount = new System.Windows.Forms.TextBox ( );
            this.txtYMeshCount = new System.Windows.Forms.TextBox ( );
            this.txtXMeshCount = new System.Windows.Forms.TextBox ( );
            this.label8 = new System.Windows.Forms.Label ( );
            this.label7 = new System.Windows.Forms.Label ( );
            this.label6 = new System.Windows.Forms.Label ( );
            this.label5 = new System.Windows.Forms.Label ( );
            this.label4 = new System.Windows.Forms.Label ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.btnOk = new System.Windows.Forms.Button ( );
            this.groupBox3 = new System.Windows.Forms.GroupBox ( );
            this.btnCheckZ = new System.Windows.Forms.Button ( );
            this.nudZDivision = new System.Windows.Forms.NumericUpDown ( );
            this.label3 = new System.Windows.Forms.Label ( );
            this.groupBox2 = new System.Windows.Forms.GroupBox ( );
            this.btnCheckY = new System.Windows.Forms.Button ( );
            this.nudYDivision = new System.Windows.Forms.NumericUpDown ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.btnCheckX = new System.Windows.Forms.Button ( );
            this.nudXDivision = new System.Windows.Forms.NumericUpDown ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.splitContainer1.Panel1.SuspendLayout ( );
            this.splitContainer1.Panel2.SuspendLayout ( );
            this.splitContainer1.SuspendLayout ( );
            this.groupBox4.SuspendLayout ( );
            this.groupBox3.SuspendLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudZDivision ) ).BeginInit ( );
            this.groupBox2.SuspendLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudYDivision ) ).BeginInit ( );
            this.groupBox1.SuspendLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudXDivision ) ).BeginInit ( );
            this.SuspendLayout ( );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point ( 0 , 0 );
            this.splitContainer1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add ( this.d3d );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add ( this.btnReset );
            this.splitContainer1.Panel2.Controls.Add ( this.groupBox4 );
            this.splitContainer1.Panel2.Controls.Add ( this.btnCancel );
            this.splitContainer1.Panel2.Controls.Add ( this.btnOk );
            this.splitContainer1.Panel2.Controls.Add ( this.groupBox3 );
            this.splitContainer1.Panel2.Controls.Add ( this.groupBox2 );
            this.splitContainer1.Panel2.Controls.Add ( this.groupBox1 );
            this.splitContainer1.Size = new System.Drawing.Size ( 826 , 634 );
            this.splitContainer1.SplitterDistance = 649;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // d3d
            // 
            this.d3d.BackColor = System.Drawing.Color.White;
            this.d3d.Dock = System.Windows.Forms.DockStyle.Fill;
            this.d3d.DxAutoResize = false;
            this.d3d.DxBackBufferCount = 2;
            this.d3d.DxFullScreen = false;
            this.d3d.DxSimulateFullScreen = false;
            this.d3d.Location = new System.Drawing.Point ( 0 , 0 );
            this.d3d.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.d3d.Name = "d3d";
            this.d3d.Projection = ( (Microsoft.DirectX.Matrix)( resources.GetObject ( "d3d.Projection" ) ) );
            this.d3d.Size = new System.Drawing.Size ( 649 , 634 );
            this.d3d.TabIndex = 0;
            this.d3d.View = ( (Microsoft.DirectX.Matrix)( resources.GetObject ( "d3d.View" ) ) );
            this.d3d.MouseWheel += new System.Windows.Forms.MouseEventHandler ( this.d3d_MouseWheel );
            this.d3d.MouseUp += new System.Windows.Forms.MouseEventHandler ( this.d3d_MouseUp );
            this.d3d.MouseMove += new System.Windows.Forms.MouseEventHandler ( this.d3d_MouseMove );
            this.d3d.MouseDown += new System.Windows.Forms.MouseEventHandler ( this.d3d_MouseDown );
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point ( 14 , 295 );
            this.btnReset.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Grobal";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler ( this.btnReset_Click );
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add ( this.txtBalance );
            this.groupBox4.Controls.Add ( this.txtCPUNumber );
            this.groupBox4.Controls.Add ( this.txtZMeshCount );
            this.groupBox4.Controls.Add ( this.txtYMeshCount );
            this.groupBox4.Controls.Add ( this.txtXMeshCount );
            this.groupBox4.Controls.Add ( this.label8 );
            this.groupBox4.Controls.Add ( this.label7 );
            this.groupBox4.Controls.Add ( this.label6 );
            this.groupBox4.Controls.Add ( this.label5 );
            this.groupBox4.Controls.Add ( this.label4 );
            this.groupBox4.Location = new System.Drawing.Point ( 7 , 332 );
            this.groupBox4.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox4.Size = new System.Drawing.Size ( 156 , 190 );
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Summary";
            // 
            // txtBalance
            // 
            this.txtBalance.Location = new System.Drawing.Point ( 94 , 148 );
            this.txtBalance.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size ( 58 , 21 );
            this.txtBalance.TabIndex = 1;
            // 
            // txtCPUNumber
            // 
            this.txtCPUNumber.Location = new System.Drawing.Point ( 94 , 118 );
            this.txtCPUNumber.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtCPUNumber.Name = "txtCPUNumber";
            this.txtCPUNumber.ReadOnly = true;
            this.txtCPUNumber.Size = new System.Drawing.Size ( 58 , 21 );
            this.txtCPUNumber.TabIndex = 1;
            // 
            // txtZMeshCount
            // 
            this.txtZMeshCount.Location = new System.Drawing.Point ( 94 , 88 );
            this.txtZMeshCount.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtZMeshCount.Name = "txtZMeshCount";
            this.txtZMeshCount.ReadOnly = true;
            this.txtZMeshCount.Size = new System.Drawing.Size ( 58 , 21 );
            this.txtZMeshCount.TabIndex = 1;
            // 
            // txtYMeshCount
            // 
            this.txtYMeshCount.Location = new System.Drawing.Point ( 94 , 58 );
            this.txtYMeshCount.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtYMeshCount.Name = "txtYMeshCount";
            this.txtYMeshCount.ReadOnly = true;
            this.txtYMeshCount.Size = new System.Drawing.Size ( 58 , 21 );
            this.txtYMeshCount.TabIndex = 1;
            // 
            // txtXMeshCount
            // 
            this.txtXMeshCount.Location = new System.Drawing.Point ( 94 , 28 );
            this.txtXMeshCount.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtXMeshCount.Name = "txtXMeshCount";
            this.txtXMeshCount.ReadOnly = true;
            this.txtXMeshCount.Size = new System.Drawing.Size ( 58 , 21 );
            this.txtXMeshCount.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point ( 36 , 152 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size ( 55 , 15 );
            this.label8.TabIndex = 0;
            this.label8.Text = "Balance:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point ( 8 , 122 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size ( 84 , 15 );
            this.label7.TabIndex = 0;
            this.label7.Text = "Num of CPUs:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point ( 15 , 92 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size ( 70 , 15 );
            this.label6.TabIndex = 0;
            this.label6.Text = "Meshs of Z:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point ( 15 , 62 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size ( 70 , 15 );
            this.label5.TabIndex = 0;
            this.label5.Text = "Meshs of Y:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point ( 15 , 32 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size ( 71 , 15 );
            this.label4.TabIndex = 0;
            this.label4.Text = "Meshs of X:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCancel.Location = new System.Drawing.Point ( 0 , 576 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 172 , 29 );
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler ( this.btnCancel_Click );
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.Location = new System.Drawing.Point ( 0 , 605 );
            this.btnOk.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size ( 172 , 29 );
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler ( this.btnOk_Click );
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add ( this.btnCheckZ );
            this.groupBox3.Controls.Add ( this.nudZDivision );
            this.groupBox3.Controls.Add ( this.label3 );
            this.groupBox3.Location = new System.Drawing.Point ( 7 , 201 );
            this.groupBox3.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox3.Size = new System.Drawing.Size ( 156 , 86 );
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Z direction";
            // 
            // btnCheckZ
            // 
            this.btnCheckZ.Location = new System.Drawing.Point ( 7 , 55 );
            this.btnCheckZ.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCheckZ.Name = "btnCheckZ";
            this.btnCheckZ.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnCheckZ.TabIndex = 2;
            this.btnCheckZ.Text = "Check...";
            this.btnCheckZ.UseVisualStyleBackColor = true;
            this.btnCheckZ.Click += new System.EventHandler ( this.btnCheckZ_Click );
            // 
            // nudZDivision
            // 
            this.nudZDivision.Location = new System.Drawing.Point ( 64 , 23 );
            this.nudZDivision.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.nudZDivision.Minimum = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
            this.nudZDivision.Name = "nudZDivision";
            this.nudZDivision.Size = new System.Drawing.Size ( 72 , 21 );
            this.nudZDivision.TabIndex = 1;
            this.nudZDivision.Value = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
            this.nudZDivision.ValueChanged += new System.EventHandler ( this.nudZDivision_ValueChanged );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point ( 8 , 26 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size ( 50 , 15 );
            this.label3.TabIndex = 0;
            this.label3.Text = "Division";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add ( this.btnCheckY );
            this.groupBox2.Controls.Add ( this.nudYDivision );
            this.groupBox2.Controls.Add ( this.label2 );
            this.groupBox2.Location = new System.Drawing.Point ( 3 , 107 );
            this.groupBox2.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Size = new System.Drawing.Size ( 156 , 86 );
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y direction";
            // 
            // btnCheckY
            // 
            this.btnCheckY.Location = new System.Drawing.Point ( 7 , 52 );
            this.btnCheckY.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCheckY.Name = "btnCheckY";
            this.btnCheckY.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnCheckY.TabIndex = 2;
            this.btnCheckY.Text = "Check...";
            this.btnCheckY.UseVisualStyleBackColor = true;
            this.btnCheckY.Click += new System.EventHandler ( this.btnCheckY_Click );
            // 
            // nudYDivision
            // 
            this.nudYDivision.Location = new System.Drawing.Point ( 64 , 23 );
            this.nudYDivision.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.nudYDivision.Minimum = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
            this.nudYDivision.Name = "nudYDivision";
            this.nudYDivision.Size = new System.Drawing.Size ( 72 , 21 );
            this.nudYDivision.TabIndex = 1;
            this.nudYDivision.Value = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
            this.nudYDivision.ValueChanged += new System.EventHandler ( this.nudYDivision_ValueChanged );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point ( 8 , 26 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 50 , 15 );
            this.label2.TabIndex = 0;
            this.label2.Text = "Division";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.btnCheckX );
            this.groupBox1.Controls.Add ( this.nudXDivision );
            this.groupBox1.Controls.Add ( this.label1 );
            this.groupBox1.Location = new System.Drawing.Point ( 3 , 13 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Size = new System.Drawing.Size ( 156 , 86 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X direction";
            // 
            // btnCheckX
            // 
            this.btnCheckX.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCheckX.Location = new System.Drawing.Point ( 11 , 51 );
            this.btnCheckX.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCheckX.Name = "btnCheckX";
            this.btnCheckX.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnCheckX.TabIndex = 2;
            this.btnCheckX.Text = "Check...";
            this.btnCheckX.UseCompatibleTextRendering = true;
            this.btnCheckX.UseVisualStyleBackColor = true;
            this.btnCheckX.Click += new System.EventHandler ( this.btnCheckX_Click );
            // 
            // nudXDivision
            // 
            this.nudXDivision.Location = new System.Drawing.Point ( 64 , 22 );
            this.nudXDivision.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.nudXDivision.Minimum = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
            this.nudXDivision.Name = "nudXDivision";
            this.nudXDivision.Size = new System.Drawing.Size ( 72 , 21 );
            this.nudXDivision.TabIndex = 1;
            this.nudXDivision.Value = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
            this.nudXDivision.ValueChanged += new System.EventHandler ( this.nudXDivision_ValueChanged );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point ( 8 , 25 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 50 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Division";
            // 
            // ParallelInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 826 , 634 );
            this.Controls.Add ( this.splitContainer1 );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.Name = "ParallelInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ParallelInfoForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler ( this.ParallelInfoForm_FormClosing );
            this.Load += new System.EventHandler ( this.ParallelInfoForm_Load );
            this.splitContainer1.Panel1.ResumeLayout ( false );
            this.splitContainer1.Panel2.ResumeLayout ( false );
            this.splitContainer1.ResumeLayout ( false );
            this.groupBox4.ResumeLayout ( false );
            this.groupBox4.PerformLayout ( );
            this.groupBox3.ResumeLayout ( false );
            this.groupBox3.PerformLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudZDivision ) ).EndInit ( );
            this.groupBox2.ResumeLayout ( false );
            this.groupBox2.PerformLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudYDivision ) ).EndInit ( );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudXDivision ) ).EndInit ( );
            this.ResumeLayout ( false );

        }        

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GEMS.Designer.Direct3D.Direct3d d3d;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudZDivision;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudYDivision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudXDivision;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCPUNumber;
        private System.Windows.Forms.TextBox txtZMeshCount;
        private System.Windows.Forms.TextBox txtYMeshCount;
        private System.Windows.Forms.TextBox txtXMeshCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBalance;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnCheckZ;
        private System.Windows.Forms.Button btnCheckY;
        private System.Windows.Forms.Button btnCheckX;
        private System.Windows.Forms.Button btnReset;
    }
}