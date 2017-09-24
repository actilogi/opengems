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

using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Utility;
using GEMS.Designer.Models;
using ZedGraph;

namespace GEMS.Designer.Forms
{
    public partial class ExcitationSourceForm : Form
    {
        private GEMSExcitationSource source;

        private PluseType pluseType;
        private int lossness;
        private Frequency maxFrequency;
        private Frequency minFrequency = new Frequency(0.0f, Frequency.FrequencyUnit.GHz);

        private const int serialsLength = 100;
        private const int fftSign = 1;
       
        public ExcitationSourceForm(GEMSExcitationSource source)
        {
            InitializeComponent();

            this.source = source;

            //Copy the data
            this.pluseType = source.SourcePluseType;
            this.lossness = source.Lossness == 0 ? 20 : source.Lossness;
            this.maxFrequency = source.MaxFrequency;
        } 

        private void ExcitationSourceForm_Load(object sender, EventArgs e)
        {
            InitializeContorls();
            BindControls();

            this.cboFrequencyUnit.SelectedIndexChanged += new System.EventHandler(this.cboFrequencyUnit_SelectedIndexChanged);
            this.txtFrequncy.TextChanged += new System.EventHandler(this.txtFrequncy_TextChanged);
            this.tbBand.Scroll += new System.EventHandler(this.tbBand_Scroll);
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtFrequncy.Value <= 0.0)
            {
                MessageBox.Show("Error frequency data!!!");
                return;
            }
            //Save the new value
            source.Lossness = this.tbBand.Value;

            source.MaxFrequency = this.maxFrequency;

            source.SourcePluseType = this.pluseType;

            source.Parent.IsUpdated = true;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  

        #region Control Initialize Methods

        /// <summary>
        /// Initialize the controls
        /// </summary>
        private void InitializeContorls()
        {
            this.cboFrequencyUnit.DataSource = Enum.GetValues(typeof(Frequency.FrequencyUnit));

            this.InitiliazeTimeDomainPane();
            this.InitializeFrequencyDomainPane();
        }

        /// <summary>
        /// Update the available status of the controls
        /// </summary>
        /// <param name="enable">New status</param>
        private void UpdateControlsStatus(bool enable)
        {
            this.txtFrequncy.Enabled = enable;
            this.cboFrequencyUnit.Enabled = enable;
            this.tbBand.Enabled = enable;
            this.zedGraphFrequencyDomain.Enabled = enable;
            this.zedGraphTimeDomain.Enabled = enable;
        }

        /// <summary>
        /// Bind the data of excitation source to controls
        /// </summary>
        private void BindControls()
        {
            //Set the data
            this.txtFrequncy.Value = this.maxFrequency.Value;
            this.cboFrequencyUnit.SelectedItem = this.maxFrequency.Unit;
            this.tbBand.Value = this.lossness;
            gbBand.Text = string.Format ( "Working Frequency Bandwidth(3~26dB:{0}dB)" , tbBand.Value );

            if (pluseType == PluseType.Gaussian)
            {
                this.cbGaussian.Checked = true;
            }
            else if (pluseType == PluseType.Differentiated_Gaussian)
            {
                this.cbDGaussian.Checked = true;
            }
            else
            {
                this.cbGaussian.Checked = false;
                this.cbDGaussian.Checked = false;
                UpdateControlsStatus(false);
                return;
            }

            //Render the wave
            RenderWave();
        }

        #endregion

        #region Wave Pane Initialize and Render Methods

        /// <summary>
        ///Initialize the Graphics control to display time domain wave of excitation source
        /// </summary>
        private void InitiliazeTimeDomainPane()
        {
            //Get the control of plot graph control
            GraphPane paneTimeDomain = this.zedGraphTimeDomain.GraphPane;

            //Configure the pane
            paneTimeDomain.Title.Text = string.Empty;
            paneTimeDomain.IsFontsScaled = false;

            //Configure the X axis
            paneTimeDomain.XAxis.Title.Text = string.Empty;
            paneTimeDomain.XAxis.MajorGrid.IsVisible = true;
            paneTimeDomain.XAxis.MinorGrid.IsVisible = true;
            paneTimeDomain.XAxis.MajorTic.Size = paneTimeDomain.XAxis.MinorTic.Size;
            paneTimeDomain.XAxis.MinorTic.IsInside = false;
            paneTimeDomain.XAxis.MinorTic.IsOutside = false;
            paneTimeDomain.XAxis.MajorTic.IsInside = false;
            paneTimeDomain.XAxis.MajorTic.IsOutside = false;
            paneTimeDomain.XAxis.Scale.FontSpec.Size = 10;
            paneTimeDomain.XAxis.MinorGrid.DashOn = 3;
            paneTimeDomain.XAxis.MinorGrid.DashOff = 3;
            paneTimeDomain.XAxis.ScaleFormatEvent += new ZedGraph.Axis.ScaleFormatHandler(XAxis_ScaleFormatEvent);
            paneTimeDomain.XAxis.ScaleTitleEvent+=new ZedGraph.Axis.ScaleTitleEventHandler(XAxis_ScaleTitleEvent);

            //Configure the Y axis
            paneTimeDomain.YAxis.Title.Text = string.Empty;
            paneTimeDomain.YAxis.MajorGrid.IsVisible = true;
            paneTimeDomain.YAxis.MinorGrid.IsVisible = true;
            paneTimeDomain.YAxis.MajorTic.Size = paneTimeDomain.YAxis.MinorTic.Size;
            paneTimeDomain.YAxis.MinorGrid.DashOn = 3;
            paneTimeDomain.YAxis.MinorGrid.DashOff = 3;
            paneTimeDomain.YAxis.MinorTic.IsInside = false;
            paneTimeDomain.YAxis.MinorTic.IsOutside = false;
            paneTimeDomain.YAxis.MajorTic.IsInside = false;
            paneTimeDomain.YAxis.MajorTic.IsOutside = false;
            paneTimeDomain.YAxis.Scale.FontSpec.Size = 10;            

        }

        string XAxis_ScaleTitleEvent(ZedGraph.Axis axis)
        {
            //When the scale is very large , the title will display the scale information
            //So we need hide it!
            return "";    
        }

        string XAxis_ScaleFormatEvent(GraphPane pane, ZedGraph.Axis axis, double val, int index)
        {
            val = Time.ChangeUnit ( (float)val , Time.TimeUnit.sec , Time.TimeUnit.ns );
            return val.ToString ( "e3" ) + "ns";
        }

        /// <summary>
        /// Initialize the Graphics control to display frequency domain wave of excitation source
        /// </summary>
        private void InitializeFrequencyDomainPane()
        {
            GraphPane myPane = this.zedGraphFrequencyDomain.GraphPane;

            myPane.IsFontsScaled = false;
            myPane.Title.Text = string.Empty;

            myPane.XAxis.Title.Text = string.Empty;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.XAxis.Scale.IsLabelsInside = false;
            myPane.XAxis.MajorTic.Size = myPane.XAxis.MinorTic.Size;
            myPane.XAxis.MinorTic.IsInside = false;
            myPane.XAxis.MinorTic.IsOutside = false;
            myPane.XAxis.MajorTic.IsInside = false;
            myPane.XAxis.MajorTic.IsOutside = false;
            myPane.XAxis.Scale.FontSpec.Size = 10;
            myPane.XAxis.MinorGrid.DashOn = 3;
            myPane.XAxis.MinorGrid.DashOff = 3;
            myPane.XAxis.ScaleFormatEvent += new ZedGraph.Axis.ScaleFormatHandler(XAxis_ScaleFormatEvent1);
            myPane.XAxis.ScaleTitleEvent += new ZedGraph.Axis.ScaleTitleEventHandler(XAxis_ScaleTitleEvent);

            myPane.YAxis.Title.Text = string.Empty;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;
            myPane.YAxis.MajorTic.Size = myPane.YAxis.MinorTic.Size;
            myPane.YAxis.MinorGrid.DashOn = 3;
            myPane.YAxis.MinorGrid.DashOff = 3;
            myPane.YAxis.MinorTic.IsInside = false;
            myPane.YAxis.MinorTic.IsOutside = false;
            myPane.YAxis.MajorTic.IsInside = false;
            myPane.YAxis.MajorTic.IsOutside = false;
            myPane.YAxis.Scale.FontSpec.Size = 10;            
        }

        string XAxis_ScaleFormatEvent1(GraphPane pane, ZedGraph.Axis axis, double val, int index)
        {
            return val.ToString("e3") + "G";
        }
                
  
        private void RenderWave()
        {
            if (this.maxFrequency.Value > 0.0 && this.pluseType != PluseType.None)
            {
                double tao = 0.0;
                //Compute the time domain data
                double[] tSteps;
                double[] pluses = MathUtility.GetPulseSerials(minFrequency.Value, maxFrequency.ChangeUnit(Frequency.FrequencyUnit.Hz), lossness, pluseType, serialsLength, ref tao, out tSteps);

                //Compute the frequency data
                double[] fSteps;
                double[] ffts = MathUtility.ComputeFrequencyData(pluseType, tao, maxFrequency.ChangeUnit(Frequency.FrequencyUnit.Hz), serialsLength, out fSteps);

                double[] lossNew = new double[serialsLength];
                double value = 1.0 / Math.Pow(10.0, lossness / 20.0);
                for (int i = 0; i < serialsLength; i++)
                {
                    lossNew[i] = value;
                }
                
                //Render the wave
                RenderTimeDomainWave(tSteps, pluses);
                RenderFrequencyDomainWave(fSteps, ffts, lossNew);
            }
            else
            {
                RenderTimeDomainWave(null, null);
                RenderFrequencyDomainWave(null, null,null);
            }           
        }

        /// <summary>
        /// Render the time domain wave
        /// </summary>
        private void RenderTimeDomainWave(double[] x, double[] y)
        {
            //Get the control of plot graph control
            GraphPane paneTimeDomain = this.zedGraphTimeDomain.GraphPane;

            if(x == null)
            {
                paneTimeDomain.CurveList.Clear();
                paneTimeDomain.XAxis.Scale.IsVisible = false;    
            }
            else
            {
                //Delete the old curve
                paneTimeDomain.CurveList.Clear();

                //Add new one base on new data
                LineItem curve = paneTimeDomain.AddCurve(string.Empty, x, y, Color.Blue, SymbolType.None);
                curve.Line.IsSmooth = true;
                curve.Line.Width = 2.0f;

               
                //Configure the pan's parameters which is based on wave data
                paneTimeDomain.XAxis.Scale.IsVisible = true;
                paneTimeDomain.XAxis.Scale.Max = x[1] + x[x.Length - 1];
                paneTimeDomain.XAxis.Scale.Min = 0;
                paneTimeDomain.XAxis.Scale.MajorStep = paneTimeDomain.XAxis.Scale.Max;
                paneTimeDomain.XAxis.Scale.MinorStep = paneTimeDomain.XAxis.Scale.Max / 10;

                paneTimeDomain.YAxis.Scale.Max = 1.0;                
                paneTimeDomain.YAxis.Scale.Min = pluseType == PluseType.Gaussian ? 0 : -1.0;
                paneTimeDomain.YAxis.Scale.MajorStep = paneTimeDomain.YAxis.Scale.Max;
                paneTimeDomain.YAxis.Scale.MinorStep = paneTimeDomain.YAxis.Scale.Max / 10;

                 // Make sure the X axis is rescaled to accommodate actual data
                zedGraphTimeDomain.AxisChange();
            }

            // Force a redraw
            zedGraphTimeDomain.Invalidate();
        }

        /// <summary>
        /// Render the frequency domain wave
        /// </summary>
        private void RenderFrequencyDomainWave(double[] x, double[] y, double[] z)
        {
            //Get the control of plot graph control
            GraphPane paneFrequencyDomain = this.zedGraphFrequencyDomain.GraphPane;

            if (x == null)
            {
                paneFrequencyDomain.CurveList.Clear();
                paneFrequencyDomain.XAxis.Scale.IsVisible = false;
            }
            else
            {
                //Delete the old curve
                paneFrequencyDomain.CurveList.Clear();

                //Add new one base on new data
                LineItem curve = paneFrequencyDomain.AddCurve(string.Empty, x, y, Color.Blue, SymbolType.None);
                curve.Line.IsSmooth = true;
                curve.Line.Width = 2.0f;

                //Add new one base on new data
                LineItem line = paneFrequencyDomain.AddCurve(string.Empty, x, z, Color.Red, SymbolType.None);
                line.Line.IsSmooth = true;
                line.Line.Width = 2.0f;
                line.Label.Text = this.lossness + "dB";

                //Configure the pan's parameters which is based on wave data
                paneFrequencyDomain.XAxis.Scale.IsVisible = true;
                paneFrequencyDomain.XAxis.Scale.Max = x[1] + x[x.Length - 1];
                paneFrequencyDomain.XAxis.Scale.Min = 0;
                paneFrequencyDomain.XAxis.Scale.MajorStep = paneFrequencyDomain.XAxis.Scale.Max;
                paneFrequencyDomain.XAxis.Scale.MinorStep = paneFrequencyDomain.XAxis.Scale.Max / 10;


                paneFrequencyDomain.YAxis.Scale.Max = 1.0;
                paneFrequencyDomain.YAxis.Scale.MajorStep = paneFrequencyDomain.YAxis.Scale.Max;
                paneFrequencyDomain.YAxis.Scale.MinorStep = paneFrequencyDomain.YAxis.Scale.Max / 10;

                // Make sure the X axis is rescaled to accommodate actual data
                zedGraphFrequencyDomain.AxisChange();
            }

            // Force a redraw
            zedGraphFrequencyDomain.Invalidate();

        }

        #endregion

        #region Data Changed Handler

        private void txtFrequncy_TextChanged(object sender, EventArgs e)
        {
            if (txtFrequncy.Value > 0.0)
            {
                this.maxFrequency.Value = txtFrequncy.Value;

                RenderWave();
            }

        }

        private void cboFrequencyUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.maxFrequency.Unit = (Frequency.FrequencyUnit)this.cboFrequencyUnit.SelectedItem;

            RenderWave();

        }

        private void tbBand_Scroll(object sender, EventArgs e)
        {
            gbBand.Text = string.Format("Working Frequency Bandwidth(3~26dB:{0}dB)", tbBand.Value);

            if (pluseType != PluseType.None)
            {
                this.lossness = tbBand.Value;
                RenderWave();

            }
        }

        private void cbGaussian_Click(object sender, EventArgs e)
        {
            if (cbDGaussian.Checked)
                cbDGaussian.Checked = false;

            UpdateControlsStatus(cbGaussian.Checked);

            if (cbGaussian.Checked)
            {
                this.pluseType = PluseType.Gaussian;
            }
            else
            {
                this.pluseType = PluseType.None;
            }

            RenderWave();
        }

        private void cbDGaussian_Click(object sender, EventArgs e)
        {
            if (cbGaussian.Checked)
                cbGaussian.Checked = false;

            UpdateControlsStatus(cbDGaussian.Checked);

            if (cbDGaussian.Checked)
            {
                this.pluseType = PluseType.Differentiated_Gaussian;
            }
            else
            {
                this.pluseType = PluseType.None;
            }

            RenderWave();

        }

        #endregion    
    }
}