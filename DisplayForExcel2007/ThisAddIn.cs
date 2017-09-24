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
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

using GEMS.Display.Core;

namespace GEMS.Display.Excel2007
{
    public partial class ThisAddIn
    {
        private DisplayGuidPane displayPane = null;
        private string currentResultFile = string.Empty;

        private Microsoft.Office.Tools.CustomTaskPane ctpDisplayDataGuide = null;
        private Excel.Style headStyle = null;
        private Excel.Style dataStyle = null;
        private Excel.Workbook currentWorkbook = null;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //this.AddDisplayDataGuidPane();
            //MessageBox.Show("The GEMS Display add-in has been deployed successfully.");

            #region VSTO generated code

            this.Application = (Excel.Application)Microsoft.Office.Tools.Excel.ExcelLocale1033Proxy.Wrap(typeof(Excel.Application), this.Application);

            #endregion

        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            //this.RemoveDisplayDataGuidPane();
        }

        public void AddDisplayDataGuidPane()
        {
            displayPane = new DisplayGuidPane();
            displayPane.GEMSResultLoaded += new DisplayGuidPane.GEMSResultLoadedEventHandler(OnGEMSResultLoaded);

            ctpDisplayDataGuide = this.CustomTaskPanes.Add(displayPane, "OpenGEMS");
            ctpDisplayDataGuide.DockPosition =
                Microsoft.Office.Core.
                MsoCTPDockPosition.msoCTPDockPositionLeft;
            ctpDisplayDataGuide.Visible = true;

            if (currentResultFile != string.Empty)
            {
                displayPane.LoadResult(currentResultFile);
            }
        }

        public void RemoveDisplayDataGuidPane()
        {
            this.currentResultFile = displayPane.CurrentResultFile;

            this.CustomTaskPanes.Remove(ctpDisplayDataGuide);

            displayPane.GEMSResultLoaded -= new DisplayGuidPane.GEMSResultLoadedEventHandler(OnGEMSResultLoaded);
            displayPane.Dispose();
        }

        void OnGEMSResultLoaded(object sender, DisplayGuidPane.GEMSResultLoadedEventArgs e)
        {
            DisplayResult(e.LoadedResult);
        }

        public void DisplayResult(Result result)
        {
            this.Application.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlWait;

            if (this.Application.ActiveWorkbook == null)
            {
                this.currentWorkbook = this.Application.Workbooks.Add(missing);
                InitializeStyle(currentWorkbook);
            }
            else
            {
                this.currentWorkbook = this.Application.ActiveWorkbook;
                if (headStyle == null || dataStyle == null)
                    InitializeStyle(currentWorkbook);
            }

            if (result is Result1D)
            {
                Display1DResult(result as Result1D);
            }

            if (result is Result2D)
            {
                DisplayFFTResult(result as Result2D);
            }

            this.Application.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
        }

        private void InitializeStyle(Excel.Workbook workbook)
        {
            //Style of head 
            headStyle = workbook.Styles.Add(Guid.NewGuid().ToString(), missing);
            headStyle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            headStyle.Font.Name = "Verdana";
            headStyle.Font.Size = 10;
            headStyle.Font.Bold = true;
            headStyle.Font.ColorIndex = 5;

            //Style of data
            dataStyle = workbook.Styles.Add(Guid.NewGuid().ToString(), missing);
            dataStyle.Font.Name = "Verdana";
            dataStyle.Font.Size = 10.0f;
            dataStyle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        }

        /// <summary>
        /// Display the result
        /// 1. Create a new sheet
        /// 2. Create two range to display the key and value
        /// 3. Create a chart based the range object
        /// </summary>        
        private void Display1DResult(Result1D result1d)
        {
            if (result1d.Keys.Count > 0)
            {
                Excel.Worksheet resultSheet = currentWorkbook.Sheets.Add(missing, missing, missing, missing) as Excel.Worksheet;

                Excel.Range KeyColumnHead = resultSheet.get_Range("A1", missing);
                KeyColumnHead.Value2 = result1d.KeyLabel;

                Excel.Range ValueColumnHead = resultSheet.get_Range("B1", missing);
                ValueColumnHead.Value2 = result1d.ValueLabel;

                Excel.Range headRange = resultSheet.get_Range("A1", "B1");
                headRange.Style = headStyle;
                headRange.EntireColumn.AutoFit();

                //Bind the data
                float[,] keyvalues = new float[result1d.Keys.Count, 2];
                for (int i = 0; i < result1d.Keys.Count; i++)
                {
                    keyvalues[i, 0] = result1d.Keys[i];
                    keyvalues[i, 1] = result1d.Values[i];
                }

                string rangeKey = string.Format("A2:A{0}", result1d.Keys.Count + 1);
                string rangeValue = string.Format("B2:B{0}", result1d.Values.Count + 1);
                Excel.Range dataRange = resultSheet.get_Range(rangeKey, rangeValue);
                dataRange.Value2 = keyvalues;
                dataRange.Style = dataStyle;
                dataRange.EntireColumn.AutoFit();

                //Add a Chart for the selected data.
                Excel._Chart oChart = (Excel._Chart)currentWorkbook.Charts.Add(Type.Missing, Type.Missing,
                   Type.Missing, Type.Missing);

                //Use the ChartWizard to create a new chart from the selected data.
                Excel.Range oResizeRange = resultSheet.get_Range(rangeValue, missing).get_Resize(
                    result1d.Keys.Count, 1);

                oChart.ChartWizard(oResizeRange, Excel.XlChartType.xlLine, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                oChart.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

                //Configure the format of chart
                ConfigureExcelChartFormat(resultSheet.Name, result1d.ValueLabel, oChart);

                //Position of the chart
                resultSheet.Shapes.Item(1).Top = (float)(double)oResizeRange.Top;
                resultSheet.Shapes.Item(1).Left = (float)(double)oResizeRange.Left + (float)(double)oResizeRange.Width;
            }
        }

        private void DisplayFFTResult(Result2D result2d)
        {
            if (result2d.Keys.Count > 0)
            {
                Excel.Worksheet resultSheet = currentWorkbook.Sheets.Add(missing, missing, missing, missing) as Excel.Worksheet;

                //Bind Head's data
                string[] heads = new string[3] { result2d.KeyLabel, result2d.ValueLabel1, result2d.ValueLabel2 };
                Excel.Range headRange = resultSheet.get_Range("A1:C1", missing);
                headRange.Style = headStyle;
                headRange.Value2 = heads;

                //Bind the data  : key, value1,value2
                float[,] keyvalues = new float[result2d.Keys.Count, 3];
                for (int i = 0; i < result2d.Keys.Count; i++)
                {
                    keyvalues[i, 0] = result2d.Keys[i];
                    keyvalues[i, 1] = result2d.Values1[i];
                    keyvalues[i, 2] = result2d.Values2[i];
                }

                string rangeKey = string.Format("A2:C{0}", result2d.Keys.Count + 1);
                Excel.Range dataRange = resultSheet.get_Range(rangeKey, missing);
                dataRange.Value2 = keyvalues;
                dataRange.Style = dataStyle;
                dataRange.EntireColumn.AutoFit();

                //Chart 1
                //Add a Chart for the selected data.
                Excel._Chart oChart1 = (Excel._Chart)currentWorkbook.Charts.Add(Type.Missing, Type.Missing,
                   Type.Missing, Type.Missing);

                //Use the ChartWizard to create a new chart from the selected data.
                string value1Range = string.Format("B2:B{0}", result2d.Values1.Count + 1);
                Excel.Range oResizeRange = resultSheet.get_Range(value1Range, missing).get_Resize(
                    result2d.Values1.Count, 1);

                oChart1.ChartWizard(oResizeRange, Excel.XlChartType.xlLine, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                oChart1.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

                //Configure the format of chart
                ConfigureExcelChartFormat(resultSheet.Name, result2d.ValueLabel1, oChart1);

                //Position of the chart
                resultSheet.Shapes.Item(1).Top = (float)(double)dataRange.Top;
                resultSheet.Shapes.Item(1).Left = (float)(double)dataRange.Left + (float)(double)dataRange.Width;

                //Chart 2
                //Add a Chart for the selected data.
                Excel._Chart oChart2 = (Excel._Chart)currentWorkbook.Charts.Add(Type.Missing, Type.Missing,
                   Type.Missing, Type.Missing);

                //Use the ChartWizard to create a new chart from the selected data.                
                string value2Range = string.Format("C2:C{0}", result2d.Values2.Count + 1);
                oResizeRange = resultSheet.get_Range(value2Range, missing).get_Resize(
                    result2d.Values2.Count, 1);

                oChart2.ChartWizard(oResizeRange, Excel.XlChartType.xlLine, Type.Missing,
                    Excel.XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                oChart2.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

                //Configure the format of chart
                ConfigureExcelChartFormat(resultSheet.Name, result2d.ValueLabel2, oChart2);

                //Position of the chart
                resultSheet.Shapes.Item(2).Top = (float)resultSheet.Shapes.Item(1).Top + (float)resultSheet.Shapes.Item(1).Height;
                resultSheet.Shapes.Item(2).Left = (float)(double)dataRange.Left + (float)(double)dataRange.Width;

            }
        }

        private void ConfigureExcelChartFormat(string sheetName, string seriesName, Excel._Chart oChart)
        {
            //Serie format
            Excel.Series oSeries = (Excel.Series)oChart.SeriesCollection(1);
            oSeries.Name = seriesName;
            oSeries.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;
            oSeries.Smooth = true;
            oSeries.Border.ColorIndex = 3;
            oSeries.Border.Weight = 3;

            //Plot area format
            oChart.PlotArea.Interior.ColorIndex = 0;
            oChart.PlotArea.Border.ColorIndex = 5;

            //X axis format
            Excel.Axis xAxis = (Excel.Axis)oChart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            xAxis.MajorGridlines.Border.ColorIndex = 5;
            xAxis.Border.ColorIndex = 5;
            xAxis.TickLabels.Font.Name = "Verdana";
            xAxis.TickLabels.Font.Size = 8.0f;
            xAxis.TickLabels.AutoScaleFont = false;

            //Y axis format
            Excel.Axis yAxis = (Excel.Axis)oChart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            yAxis.TickLabels.Font.Name = "Verdana";
            yAxis.TickLabels.Font.Size = 8.0f;
            yAxis.TickLabels.AutoScaleFont = false;
            yAxis.Border.ColorIndex = 5;
            yAxis.MajorTickMark = Microsoft.Office.Interop.Excel.XlTickMark.xlTickMarkNone;

            //Legend format
            oChart.Legend.Font.Name = "Verdana";
            oChart.Legend.Font.Size = 9.0f;
            oChart.Legend.Font.Bold = true;
            oChart.Legend.AutoScaleFont = false;
            oChart.Legend.Position = Microsoft.Office.Interop.Excel.XlLegendPosition.xlLegendPositionTop;

            //Chart format
            oChart.HasTitle = false;
            oChart.Location(Excel.XlChartLocation.xlLocationAsObject, sheetName);

        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
