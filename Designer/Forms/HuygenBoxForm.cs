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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GEMS.Designer.Models;

namespace GEMS.Designer.Forms
{
    public partial class HuygenBoxForm : Form
    {       

        private GEMSProject m_project = null;
        private GEMSHuygensBox box = null;
        private BindingSource frequencySource = new BindingSource();

        public HuygenBoxForm(GEMSProject m_project)
        {
            InitializeComponent();

            this.m_project = m_project;
            this.box = m_project.HuygensBox.Clone();

            this.dgvFrequencyList.CellFormatting += new DataGridViewCellFormattingEventHandler(OnCellFormatting);
            this.dgvFrequencyList.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.OnRowPostPaint);
            this.dgvFrequencyList.CellValidating += new DataGridViewCellValidatingEventHandler(OnCellValidating);
        }       


        private void InitializeControls()
        {
            this.cboFrequencyUnit.DataSource = Enum.GetValues(typeof(Frequency.FrequencyUnit));
            this.cboXUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboZUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
            this.cboYUnit.DataSource = Enum.GetValues(typeof(Length.LengthUnit));
        }

        private void BindControls()
        {
            this.cbHuygenBoxEnable.Checked = box.IsEnable;
            cbHuygenBoxEnable_CheckedChanged(null, null);

            this.nudXmin.Value = box.MinX;
            this.nudXmax.Value = box.MaxX;
            this.nudYmin.Value = box.MinY;
            this.nudYmax.Value = box.MaxY;
            this.nudZmin.Value = box.MinZ;
            this.nudZmax.Value = box.MaxZ;

            this.txtPhi.Text = box.PhiString;
            this.txtThetaStart.Value = box.ThetaStart;
            this.txtThetaEnd.Value = box.ThetaEnd;
            this.txtThetaStep.Value = box.ThetaStep;
            this.cbPhiEnable.Checked = box.IsPhiEnable;

            this.txtTheta.Text = box.ThetaString;
            this.txtPhiStart.Value = box.PhiStart;
            this.txtPhiEnd.Value = box.PhiEnd;
            this.txtPhiStep.Value = box.PhiStep;
            this.cbThetaEnable.Checked = box.IsThetaEnable;

            this.txtX.Value = box.ReferencePoint.X.Value;
            this.txtY.Value = box.ReferencePoint.Y.Value;
            this.txtZ.Value = box.ReferencePoint.Z.Value;
            this.cboXUnit.SelectedItem = box.ReferencePoint.X.Unit;
            this.cboYUnit.SelectedItem = box.ReferencePoint.Y.Unit;
            this.cboZUnit.SelectedItem = box.ReferencePoint.Z.Unit;

            this.cbXMin.Checked = (box.ApertureField & 0x01) != 0;
            this.cbYMin.Checked = (box.ApertureField & 0x02) != 0;
            this.cbZMin.Checked = (box.ApertureField & 0x04) != 0;
            this.cbXMax.Checked = (box.ApertureField & 0x08) != 0;
            this.cbYMax.Checked = (box.ApertureField & 0x10) != 0;
            this.cbZMax.Checked = (box.ApertureField & 0x20) != 0;

            //this.rbCurrents.Checked = !box.IsMegneticCurrentOnly;
            //this.rbUsemc.Checked = box.IsMegneticCurrentOnly;

            this.cboFrequencyUnit.SelectedItem = box.FrequencyUnit;

            frequencySource.DataSource = box.FrequencyList;
            this.dgvFrequencyList.DataSource = frequencySource;
        }        
        
        private void HuygenBoxForm_Load(object sender, EventArgs e)
        {
            InitializeControls();
            BindControls();
        }

        private void cbHuygenBoxEnable_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control.Name == "cbHuygenBoxEnable" || control.Name == "btnOK" || control.Name == "btnCancel" )
                    continue;

                control.Enabled = cbHuygenBoxEnable.Checked;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cbHuygenBoxEnable.Checked && box.FrequencyList.Count == 0)
            {
                MessageBox.Show("Please input at least one frequency to run farfield calculation in frequency domain!");
                return;
            }

            if (!(box.Parent.Mesh.MeshCountInX - ( this.nudXmax.Value + this.nudXmin.Value ) >= 1
                && this.nudXmax.Value >= 1 
                && this.nudXmin.Value >= 1))
            {
                MessageBox.Show ( "Range in X direction must be larger than 1 ,and Xmin,Xmax must be larger than 1!" );
                return;
            }

            if (!(box.Parent.Mesh.MeshCountInY - ( this.nudYmax.Value + this.nudYmin.Value ) >= 1
                && this.nudYmax.Value >= 1 
                && this.nudYmin.Value >= 1))
            {
                MessageBox.Show ( "Range in Y direction must be larger than 1 ,and Ymin,Ymax must be larger than 1!" );
                return;
            }

            if (!(box.Parent.Mesh.MeshCountInZ - ( this.nudZmax.Value + this.nudZmin.Value ) >= 1
                && this.nudZmax.Value >= 1 
                && this.nudZmin.Value >= 1))
            {
                MessageBox.Show ( "Range in Z direction must be larger than 1 ,and Zmin,Zmax must be larger than 1!" );
                return;
            }

            box.IsEnable = this.cbHuygenBoxEnable.Checked;
            box.MinX = (int)this.nudXmin.Value;
            box.MaxX = (int)this.nudXmax.Value;
            box.MinY = (int)this.nudYmin.Value;
            box.MaxY = (int)this.nudYmax.Value;
            box.MinZ = (int)this.nudZmin.Value;
            box.MaxZ = (int)this.nudZmax.Value;

            box.PhiList = this.GetDegreeList(this.txtPhi.Text);
            box.ThetaStart  =  this.txtThetaStart.Value;
            box.ThetaEnd  =  this.txtThetaEnd.Value;
            box.ThetaStep =  this.txtThetaStep.Value;
            box.IsPhiEnable = this.cbPhiEnable.Checked;

            box.ThetaList = this.GetDegreeList(this.txtTheta.Text);
            box.PhiStart = this.txtPhiStart.Value;
            box.PhiEnd = this.txtPhiEnd.Value;
            box.PhiStep = this.txtPhiStep.Value;
            box.IsThetaEnable = this.cbThetaEnable.Checked;

            box.ReferencePoint.X.Value = this.txtX.Value;
            box.ReferencePoint.Y.Value = this.txtY.Value;
            box.ReferencePoint.Z.Value = this.txtZ.Value;
            box.ReferencePoint.X.Unit = (Length.LengthUnit)this.cboXUnit.SelectedItem;
            box.ReferencePoint.Y.Unit = (Length.LengthUnit)this.cboYUnit.SelectedItem;
            box.ReferencePoint.Z.Unit = (Length.LengthUnit)this.cboZUnit.SelectedItem;

            box.ApertureField = 0;
            box.ApertureField |= (byte)(this.cbXMin.Checked ? 0x01: 0x00);
            box.ApertureField |= (byte)(this.cbYMin.Checked ? 0x02 : 0x00);
            box.ApertureField |= (byte)(this.cbZMin.Checked ? 0x04 : 0x00);
            box.ApertureField |= (byte)(this.cbXMax.Checked ? 0x08 : 0x00);
            box.ApertureField |= (byte)(this.cbYMax.Checked ? 0x10 : 0x00);
            box.ApertureField |= (byte)(this.cbZMax.Checked ? 0x20 : 0x00);

            //box.IsMegneticCurrentOnly = !this.rbCurrents.Checked;
            box.FrequencyUnit = (Frequency.FrequencyUnit)this.cboFrequencyUnit.SelectedItem;
            
            GetDegreeList(this.txtPhi.Text);

            m_project.HuygensBox = box;
            m_project.IsUpdated = true;

            this.Close();
        }

        private List<float> GetDegreeList(string degreeString)
        {
            degreeString = degreeString.Trim();
            List<float> degrees = new List<float>();

            if (degreeString != string.Empty)
            {
                string[] degreeStrings = degreeString.Split(',');

                for (int i = 0; i < degreeStrings.Length; i++)
                {
                    float degree;

                    if (degreeStrings[i].Trim() == string.Empty)
                        continue;

                    if (float.TryParse(degreeStrings[i], out degree))
                    {
                        degrees.Add(degree);
                    }
                }
            }

            return degrees;            
        }
      

        #region Frequency List Methods

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (box.FrequencyList.Count > 0)
            {
                int index = dgvFrequencyList.CurrentCell.RowIndex;

                frequencySource.RemoveAt(index);
            }
        }       

        private void btnAppend_Click(object sender, EventArgs e)
        {
            frequencySource.Add(0.0f);

            dgvFrequencyList.CurrentCell = dgvFrequencyList[0, dgvFrequencyList.Rows.Count - 1];
            dgvFrequencyList.BeginEdit(false);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            frequencySource.Clear();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int index = dgvFrequencyList.CurrentCell.RowIndex;

            frequencySource.Insert(index, 0.0f);

            dgvFrequencyList.CurrentCell = dgvFrequencyList[0, index];
            dgvFrequencyList.BeginEdit(false);

        }        

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            float start = txtStart.Value;
            float end = txtEnd.Value;
            float step = txtStep.Value;

            if (step > 0.0)
            {
                frequencySource.Clear();
                while (end > start)
                {
                    frequencySource.Add(start);
                    start += step;
                }
                frequencySource.Add(end);
            }
        }

        private void OnRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Paint the row number on the row header.
            // The using statement automatically disposes the brush.
            using (SolidBrush b = new SolidBrush(this.dgvFrequencyList.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            }
        }

        void OnCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvFrequencyList.Columns[e.ColumnIndex].Name.Equals("colFrequency"))
            {
                e.Value = frequencySource[e.RowIndex].ToString();
            }
        }

        void OnCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvFrequencyList.Columns[e.ColumnIndex].Name.Equals("colFrequency"))
            {
                if (dgvFrequencyList[e.ColumnIndex, e.RowIndex] != null 
                    && dgvFrequencyList[e.ColumnIndex, e.RowIndex].IsInEditMode)
                {
                    dgvFrequencyList.Rows[e.RowIndex].ErrorText = string.Empty;
                    float newFrequencyValue;

                    if (!float.TryParse(e.FormattedValue.ToString(), out newFrequencyValue))
                    {
                        e.Cancel = true;
                        dgvFrequencyList.Rows[e.RowIndex].ErrorText = "The value must be a float number";
                    }

                    if (!(newFrequencyValue > 0.0f))
                    {
                        e.Cancel = true;
                        dgvFrequencyList.Rows[e.RowIndex].ErrorText = "The float number must be larger than 0.0";
                    }
                    else
                    {
                        bool isRepeated = false;

                        for (int i = 0; i < frequencySource.Count; i++)
                        {
                            if (i != e.RowIndex && (float)frequencySource[i] == newFrequencyValue)
                            {
                                isRepeated = true;
                                e.Cancel = true;
                                dgvFrequencyList.Rows[e.RowIndex].ErrorText = "This float number does already exists";
                                break;
                            }
                        }
                        if (!isRepeated)
                            frequencySource[e.RowIndex] = newFrequencyValue;
                    }
                }
            }
        }

        #endregion

        #region 2D Methods

        private void cbThetaEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.txtTheta.Enabled = cbThetaEnable.Checked;      
            this.txtPhiStart.Enabled = cbThetaEnable.Checked;      
            this.txtPhiEnd.Enabled = cbThetaEnable.Checked;
            this.txtPhiStep.Enabled = cbThetaEnable.Checked;
        }

        private void cbPhiEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPhi.Enabled  = cbPhiEnable.Checked;       
            this.txtThetaStart.Enabled  = cbPhiEnable.Checked;       
            this.txtThetaEnd.Enabled  = cbPhiEnable.Checked;          
            this.txtThetaStep.Enabled  = cbPhiEnable.Checked;
        }

        #endregion

      
       
       
    }
}