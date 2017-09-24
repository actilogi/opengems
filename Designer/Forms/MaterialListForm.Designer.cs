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
    partial class MaterialListForm
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
            this.components = new System.ComponentModel.Container ( );
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle ( );
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle ( );
            this.dgvMaterils = new System.Windows.Forms.DataGridView ( );
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn ( );
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn ( );
            this.geometryMaterialListBindingSource = new System.Windows.Forms.BindingSource ( this.components );
            this.gEMSProjectBindingSource = new System.Windows.Forms.BindingSource ( this.components );
            this.btnOk = new System.Windows.Forms.Button ( );
            this.btnCancel = new System.Windows.Forms.Button ( );
            this.btnEdit = new System.Windows.Forms.Button ( );
            this.btnAdd = new System.Windows.Forms.Button ( );
            this.btnDelete = new System.Windows.Forms.Button ( );
            this.btnVoid = new System.Windows.Forms.Button ( );
            this.groupBox1 = new System.Windows.Forms.GroupBox ( );
            ( (System.ComponentModel.ISupportInitialize)( this.dgvMaterils ) ).BeginInit ( );
            ( (System.ComponentModel.ISupportInitialize)( this.geometryMaterialListBindingSource ) ).BeginInit ( );
            ( (System.ComponentModel.ISupportInitialize)( this.gEMSProjectBindingSource ) ).BeginInit ( );
            this.groupBox1.SuspendLayout ( );
            this.SuspendLayout ( );
            // 
            // dgvMaterils
            // 
            this.dgvMaterils.AllowUserToAddRows = false;
            this.dgvMaterils.AllowUserToDeleteRows = false;
            this.dgvMaterils.AllowUserToResizeRows = false;
            this.dgvMaterils.AutoGenerateColumns = false;
            this.dgvMaterils.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvMaterils.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaterils.Columns.AddRange ( new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.nameDataGridViewTextBoxColumn} );
            this.dgvMaterils.DataSource = this.geometryMaterialListBindingSource;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMaterils.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMaterils.GridColor = System.Drawing.Color.FromArgb ( ( (int)( ( (byte)( 224 ) ) ) ) , ( (int)( ( (byte)( 224 ) ) ) ) , ( (int)( ( (byte)( 224 ) ) ) ) );
            this.dgvMaterils.Location = new System.Drawing.Point ( 6 , 22 );
            this.dgvMaterils.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.dgvMaterils.MultiSelect = false;
            this.dgvMaterils.Name = "dgvMaterils";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb ( ( (int)( ( (byte)( 255 ) ) ) ) , ( (int)( ( (byte)( 128 ) ) ) ) , ( (int)( ( (byte)( 128 ) ) ) ) );
            dataGridViewCellStyle2.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMaterils.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMaterils.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvMaterils.RowTemplate.Height = 23;
            this.dgvMaterils.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMaterils.ShowEditingIcon = false;
            this.dgvMaterils.Size = new System.Drawing.Size ( 604 , 283 );
            this.dgvMaterils.TabIndex = 0;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.Frozen = true;
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Id.Width = 43;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // geometryMaterialListBindingSource
            // 
            this.geometryMaterialListBindingSource.DataMember = "Materials";
            this.geometryMaterialListBindingSource.DataSource = this.gEMSProjectBindingSource;
            // 
            // gEMSProjectBindingSource
            // 
            this.gEMSProjectBindingSource.DataSource = typeof ( GEMS.Designer.Models.GEMSProject );
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnOk.Location = new System.Drawing.Point ( 447 , 332 );
            this.btnOk.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler ( this.btnOk_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnCancel.Location = new System.Drawing.Point ( 540 , 332 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler ( this.btnCancel_Click );
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnEdit.Location = new System.Drawing.Point ( 197 , 332 );
            this.btnEdit.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler ( this.btnEdit_Click );
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnAdd.Location = new System.Drawing.Point ( 11 , 332 );
            this.btnAdd.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler ( this.btnAdd_Click );
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnDelete.Location = new System.Drawing.Point ( 104 , 332 );
            this.btnDelete.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler ( this.btnDelete_Click );
            // 
            // btnVoid
            // 
            this.btnVoid.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnVoid.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.btnVoid.Location = new System.Drawing.Point ( 354 , 332 );
            this.btnVoid.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.btnVoid.Name = "btnVoid";
            this.btnVoid.Size = new System.Drawing.Size ( 87 , 29 );
            this.btnVoid.TabIndex = 1;
            this.btnVoid.Text = "Void";
            this.btnVoid.UseVisualStyleBackColor = true;
            this.btnVoid.Click += new System.EventHandler ( this.btnVoid_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add ( this.dgvMaterils );
            this.groupBox1.Location = new System.Drawing.Point ( 11 , 7 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size ( 616 , 317 );
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Material List";
            // 
            // MaterialListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 7F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 639 , 374 );
            this.Controls.Add ( this.groupBox1 );
            this.Controls.Add ( this.btnEdit );
            this.Controls.Add ( this.btnDelete );
            this.Controls.Add ( this.btnAdd );
            this.Controls.Add ( this.btnCancel );
            this.Controls.Add ( this.btnVoid );
            this.Controls.Add ( this.btnOk );
            this.Font = new System.Drawing.Font ( "Microsoft Sans Serif" , 9F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ( (byte)( 0 ) ) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding ( 3 , 4 , 3 , 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Current Materials";
            this.Load += new System.EventHandler ( this.MaterialListForm_Load );
            ( (System.ComponentModel.ISupportInitialize)( this.dgvMaterils ) ).EndInit ( );
            ( (System.ComponentModel.ISupportInitialize)( this.geometryMaterialListBindingSource ) ).EndInit ( );
            ( (System.ComponentModel.ISupportInitialize)( this.gEMSProjectBindingSource ) ).EndInit ( );
            this.groupBox1.ResumeLayout ( false );
            this.ResumeLayout ( false );

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMaterils;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.BindingSource geometryMaterialListBindingSource;
        private System.Windows.Forms.BindingSource gEMSProjectBindingSource;
        private System.Windows.Forms.Button btnVoid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}