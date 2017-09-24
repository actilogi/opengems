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
    partial class PreCalculateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( PreCalculateForm ) );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            this.nudTimeSteps = new System.Windows.Forms.NumericUpDown ( );
            this.label1 = new System.Windows.Forms.Label ( );
            this.groupBox2 = new System.Windows.Forms.GroupBox ( );
            this.pbPreCalculateProgress = new System.Windows.Forms.ProgressBar ( );
            this.btnValidate = new System.Windows.Forms.Button ( );
            this.btnStart = new System.Windows.Forms.Button ( );
            this.rtxtMessages = new GEMS.Designer.Controls.ExRichTextBox ( );
            this.groupBox3 = new System.Windows.Forms.GroupBox ( );
            this.btnSelectFile = new System.Windows.Forms.Button ( );
            this.txtPreCalculateFileName = new System.Windows.Forms.TextBox ( );
            this.label2 = new System.Windows.Forms.Label ( );
            this.btnOk = new System.Windows.Forms.Button ( );
            this.groupBox1.SuspendLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudTimeSteps ) ).BeginInit ( );
            this.groupBox2.SuspendLayout ( );
            this.groupBox3.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point ( 517 , 532 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.nudTimeSteps );
            this.groupBox1.Controls.Add ( this.label1 );
            this.groupBox1.Location = new System.Drawing.Point ( 12 , 13 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox1.Size = new System.Drawing.Size ( 592 , 67 );
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Convergent Condition";
            // 
            // nudTimeSteps
            // 
            this.nudTimeSteps.Location = new System.Drawing.Point ( 99 , 30 );
            this.nudTimeSteps.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.nudTimeSteps.Maximum = new decimal ( new int[] {
            1000000000,
            0,
            0,
            0} );
            this.nudTimeSteps.Minimum = new decimal ( new int[] {
            1,
            0,
            0,
            0} );
            this.nudTimeSteps.Name = "nudTimeSteps";
            this.nudTimeSteps.Size = new System.Drawing.Size ( 140 , 21 );
            this.nudTimeSteps.TabIndex = 1;
            this.nudTimeSteps.Value = new decimal ( new int[] {
            5000,
            0,
            0,
            0} );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point ( 18 , 32 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size ( 69 , 15 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Steps";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add ( this.pbPreCalculateProgress );
            this.groupBox2.Controls.Add ( this.btnValidate );
            this.groupBox2.Controls.Add ( this.btnStart );
            this.groupBox2.Controls.Add ( this.rtxtMessages );
            this.groupBox2.Location = new System.Drawing.Point ( 12 , 172 );
            this.groupBox2.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox2.Size = new System.Drawing.Size ( 592 , 352 );
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pre Calucalte";
            // 
            // pbPreCalculateProgress
            // 
            this.pbPreCalculateProgress.Location = new System.Drawing.Point ( 12 , 312 );
            this.pbPreCalculateProgress.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.pbPreCalculateProgress.Name = "pbPreCalculateProgress";
            this.pbPreCalculateProgress.Size = new System.Drawing.Size ( 562 , 29 );
            this.pbPreCalculateProgress.TabIndex = 5;
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point ( 8 , 25 );
            this.btnValidate.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size ( 169 , 30 );
            this.btnValidate.TabIndex = 1;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler ( this.btnValidate_Click );
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point ( 184 , 25 );
            this.btnStart.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size ( 169 , 30 );
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler ( this.btnStart_Click );
            // 
            // rtxtMessages
            // 
            this.rtxtMessages.BackColor = System.Drawing.Color.White;
            this.rtxtMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxtMessages.HiglightColor = GEMS.Designer.Controls.RtfColor.White;
            this.rtxtMessages.Location = new System.Drawing.Point ( 8 , 62 );
            this.rtxtMessages.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.rtxtMessages.Name = "rtxtMessages";
            this.rtxtMessages.ReadOnly = true;
            this.rtxtMessages.Size = new System.Drawing.Size ( 566 , 242 );
            this.rtxtMessages.TabIndex = 0;
            this.rtxtMessages.Text = "";
            this.rtxtMessages.TextColor = GEMS.Designer.Controls.RtfColor.Black;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add ( this.btnSelectFile );
            this.groupBox3.Controls.Add ( this.txtPreCalculateFileName );
            this.groupBox3.Controls.Add ( this.label2 );
            this.groupBox3.Location = new System.Drawing.Point ( 12 , 88 );
            this.groupBox3.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.groupBox3.Size = new System.Drawing.Size ( 592 , 76 );
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pre-Calculate File";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point ( 466 , 31 );
            this.btnSelectFile.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size ( 48 , 21 );
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler ( this.btnSelectFile_Click );
            // 
            // txtPreCalculateFileName
            // 
            this.txtPreCalculateFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPreCalculateFileName.Location = new System.Drawing.Point ( 93 , 31 );
            this.txtPreCalculateFileName.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.txtPreCalculateFileName.Name = "txtPreCalculateFileName";
            this.txtPreCalculateFileName.Size = new System.Drawing.Size ( 367 , 21 );
            this.txtPreCalculateFileName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point ( 28 , 34 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size ( 59 , 15 );
            this.label2.TabIndex = 0;
            this.label2.Text = "Filename";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point ( 424 , 532 );
            this.btnOk.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler ( this.btnOk_Click );
            // 
            // PreCalculateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 616 , 572 );
            this.Controls.Add ( this.groupBox3 );
            this.Controls.Add ( this.groupBox2 );
            this.Controls.Add ( this.groupBox1 );
            this.Controls.Add ( this.btnOk );
            this.Controls.Add ( this.btnCancel );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreCalculateForm";
            this.Text = "Pre Calculate";
            this.Load += new System.EventHandler ( this.PreCalculateForm_Load );
            this.groupBox1.ResumeLayout ( false );
            this.groupBox1.PerformLayout ( );
            ( (System.ComponentModel.ISupportInitialize)( this.nudTimeSteps ) ).EndInit ( );
            this.groupBox2.ResumeLayout ( false );
            this.groupBox3.ResumeLayout ( false );
            this.groupBox3.PerformLayout ( );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudTimeSteps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnValidate;
        private GEMS.Designer.Controls.ExRichTextBox rtxtMessages;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtPreCalculateFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar pbPreCalculateProgress;
        private System.Windows.Forms.Button btnOk;
    }
}