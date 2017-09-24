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

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

#endregion

namespace GEMS.Display.Core
{

    public partial class DisplayGuidPane : UserControl
    {

        #region Delagate and Event Declarations

        public class GEMSResultLoadedEventArgs : EventArgs
        {
            private Result loadedResult;

            public GEMSResultLoadedEventArgs ( Result result )
            {
                this.loadedResult = result;
            }

            public Result LoadedResult
            {
                get { return loadedResult; }
            }
        }
        

        public delegate void GEMSResultLoadedEventHandler ( object sender , GEMSResultLoadedEventArgs e );


        // GEMSResultLoaded is raised when user double-clicked a node of data guid tree
        // the result attched on the node will be loaded from the result file.
        public virtual event GEMSResultLoadedEventHandler GEMSResultLoaded;


        public void ResultLoadedAlarm ( Result result )
        {
            GEMSResultLoadedEventArgs e = new GEMSResultLoadedEventArgs ( result );

            if (GEMSResultLoaded != null)
            {
                GEMSResultLoaded ( this , e );
            }
        }
        

        #endregion


        private string resultFileName = string.Empty;

        public string CurrentResultFile
        {
            get { return resultFileName; }
        }

        private TreeNode root = null;
        private TreeNode voltageOutputNode = null;
        private TreeNode currentOutputNode  = null;
        private TreeNode nearFieldOutputNode = null;
        private TreeNode farFieldOutputNode   = null;

        private static string[] fieldAtPointOutputElements = new string[]{"Ex","Ey","Ez","Hx","Hy","Hz" };
        private static string[] farField2DElements = new string[] { "Etheta" , "Ephi" , "Etotal"};
        private static string[] farField2DSubElementsInnerNames = new string[] { "Amp" , "Phase" , "Directivity" , "DirectivityDB" , "Gain" , "GainDB" };
        private static string[] farField2DSubElementsDisplayNames = new string[] { "tAmp/w" , "Phase" , "Directivity" , "Directivity(DB)" , "Gain" , "Gain(DB)" };

        public DisplayGuidPane ( )
        {
            InitializeComponent ( );
        }

        private void apcDisplayGuid_Load ( object sender , EventArgs e )
        {
            InitializeDataGuideTree ( );

            this.tvResultGuide.NodeMouseDoubleClick +=new TreeNodeMouseClickEventHandler ( tvResultGuide_NodeMouseDoubleClick );
        }

        void tvResultGuide_NodeMouseDoubleClick ( object sender , TreeNodeMouseClickEventArgs e )
        {
            if (e.Node.Tag != null)
            {
                OutputReader reader = e.Node.Tag as OutputReader;

                if (reader != null)
                {                  
                    //Display the result on the Excel
                   // Globals.ThisWorkbook.DisplayResult(reader.Load ( ));
                    ResultLoadedAlarm ( reader.Load ( ) );

                    ////Test the result
                    //Result1D result1D = result as Result1D;
                    //if (result1D != null)
                    //{
                    //    StringBuilder builder = new StringBuilder ( );
                    //    foreach (KeyValuePair<float , float> kvp in result1D.KeyValuePairs)
                    //    {
                    //        builder.AppendFormat ( "key = {0} , value = {1}\n" , kvp.Key , kvp.Value );
                    //    }

                    //    MessageBox.Show ( builder.ToString ( ) );
                    //}
                    
                }
            }
        }
        
        private void InitializeDataGuideTree ( )
        {
            root = new TreeNode ( "Outputs" );
            voltageOutputNode   = root.Nodes.Add ( "Voltage" );
            currentOutputNode   = root.Nodes.Add ( "Current" );
            nearFieldOutputNode = root.Nodes.Add ( "Near Field" );
            farFieldOutputNode   = root.Nodes.Add ( "Far Field" );

            tvResultGuide.Nodes.Add ( root );
            root.ExpandAll ( ); 
        }

        private void btnOpenFile_Click ( object sender , EventArgs e )
        {
            // Displays an OpenFileDialog so the user can select a gpj file.
            OpenFileDialog openFileDialog1 = new OpenFileDialog ( );
            openFileDialog1.Filter = "OpenGEMS Result File(*.xml)|*.xml";
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Select an OpenGEMS Result File";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .GPJ file was selected, open it.
            if (openFileDialog1.ShowDialog ( ) == DialogResult.OK)
            {
                LoadResult(openFileDialog1.FileName);
            }
        }

        public void LoadResult(string fileName)
        {
            resultFileName = fileName;
            lblResultFileName.Text = resultFileName;
            toolTip.SetToolTip ( lblResultFileName , resultFileName );

            //Update the UI
            this.Cursor = Cursors.WaitCursor;

            this.btnOpenFile.Enabled = false;

            //Build the data guide tree
            voltageOutputNode.Nodes.Clear();
            currentOutputNode.Nodes.Clear();
            nearFieldOutputNode.Nodes.Clear();
            farFieldOutputNode.Nodes.Clear();

            BuildDataGuide(resultFileName);
            root.Expand();

            //Update the UI
            this.Cursor = Cursors.Default;
            this.btnOpenFile.Enabled = true;
        }

        private void BuildDataGuide ( string fileName )
        {
            XPathDocument document = new XPathDocument ( fileName );
            XPathNavigator navigator = document.CreateNavigator ( );

            navigator.MoveToChild ( "Document" , string.Empty );

            if (navigator.HasChildren)
            {
                BuildVoltageOutputTree(navigator.Clone());

                BuildCurrentOutputTree(navigator.Clone());

                BuildNearFieldOutputTree(navigator.Clone());

                BuildFarFieldOutputTree ( navigator.Clone ( ));
            }            
        }

        private void BuildVoltageOutputTree(XPathNavigator navigator)
        {
            XPathNodeIterator voIterator = navigator.SelectChildren("VoltageOutput", string.Empty);

            while (voIterator.MoveNext())
            {
                XPathNavigator voNavigator = voIterator.Current.Clone();
                string name = voNavigator.GetAttribute("objectName", string.Empty);

                TreeNode voltageOutputsNode = voltageOutputNode.Nodes.Add(name);
                TreeNode voltageOutputDataNode = voltageOutputsNode.Nodes.Add("TimeDomain");

                voltageOutputDataNode.Tag = new VoltageOutputReader(this.resultFileName,
                    "value",
                    "deltaTime",
                    "/Document/VoltageOutput[@objectName='" + name + "']/Voltages/Voltage",
                    name);
            }            
        }

        private void BuildCurrentOutputTree(XPathNavigator navigator)
        {
            XPathNodeIterator voIterator = navigator.SelectChildren("CurrentOutput", string.Empty);

            while (voIterator.MoveNext())
            {
                XPathNavigator voNavigator = voIterator.Current.Clone();
                string name = voNavigator.GetAttribute("objectName", string.Empty);

                TreeNode currentOutputsNode = currentOutputNode.Nodes.Add(name);
                TreeNode currentOutputDataNode = currentOutputsNode.Nodes.Add("TimeDomain");

                currentOutputDataNode.Tag = new CurrentOutputReader(this.resultFileName,
                    "value",
                    "deltaTime",
                    "/Document/CurrentOutput[@objectName='" + name + "']/Currents/Current",
                    name);
            }
        }

        private void BuildNearFieldOutputTree(XPathNavigator navigator)
        {
            XPathNodeIterator nfoIterator = navigator.SelectChildren("FieldAtPointOutput", string.Empty);

            while (nfoIterator.MoveNext())
            {
                XPathNavigator nfoNavigator = nfoIterator.Current.Clone();
                string name = nfoNavigator.GetAttribute("objectName", string.Empty);

                TreeNode fieldAtPointOutputNode = nearFieldOutputNode.Nodes.Add(name);

                for (int i = 0; i < fieldAtPointOutputElements.Length; i++)
                {
                    TreeNode elementNode = fieldAtPointOutputNode.Nodes.Add(fieldAtPointOutputElements[i]);
                    TreeNode elementDataNode = elementNode.Nodes.Add("TimeDomain");

                    elementDataNode.Tag = new FieldAtPointOutputElementReader(this.resultFileName,
                        fieldAtPointOutputElements[i],
                        "deltaTime",
                        "/Document/FieldAtPointOutput[@objectName='" + name + "']/Fields/Field",
                        name + "/" + fieldAtPointOutputElements[i]);
                }
            }
        }

        private void BuildFarFieldOutputTree(XPathNavigator navigator)
        {
            if (navigator.MoveToChild("FarFieldOutput", string.Empty))
            {
                TreeNode freqDomainOutputNode = farFieldOutputNode.Nodes.Add("FreqDomain");
                TreeNode thetaCutNode = freqDomainOutputNode.Nodes.Add("Theta Cut");
                TreeNode phiCutNode = freqDomainOutputNode.Nodes.Add("Phi Cut");

                FrequencyFormator.FrequencyUnit frequencyUnit = (FrequencyFormator.FrequencyUnit)Enum.Parse(typeof(FrequencyFormator.FrequencyUnit),
                    navigator.GetAttribute("frequency_Unit", string.Empty));

                XPathNodeIterator nodes = navigator.SelectChildren("FarField", string.Empty);

                while (nodes.MoveNext())
                {
                    XPathNavigator frequencyNavigator = nodes.Current.Clone();
                    string frequency = frequencyNavigator.GetAttribute("frequency", string.Empty);

                    //Theta cut
                    TreeNode thetaCutFrequencyNode = thetaCutNode.Nodes.Add(FrequencyFormator.Format(float.Parse(frequency), frequencyUnit));
                    XPathNodeIterator thetaNodes = frequencyNavigator.Select("ThetaCut/ThetaCutFarField");
                    string displayPath = string.Format("Theta Cut/{0}/", thetaCutFrequencyNode.Text);
                    while (thetaNodes.MoveNext())
                    {
                        string theta = thetaNodes.Current.GetAttribute("theta", string.Empty);
                        string xpath = "/Document/FarFieldOutput/FarField[@frequency='" + frequency + "']/ThetaCut/ThetaCutFarField[@theta='" + theta + "']/Value";
                        string nodeName = "theta=" + float.Parse(theta).ToString();

                        BuildFarFieldOutputElementTree(thetaCutFrequencyNode.Nodes.Add(nodeName), xpath, "phi", displayPath + nodeName);
                    }

                    //Phi cut
                    TreeNode phiCutFrequencyNode = phiCutNode.Nodes.Add(FrequencyFormator.Format(float.Parse(frequency), frequencyUnit));
                    XPathNodeIterator phiNodes = frequencyNavigator.Select("PhiCut/PhiCutFarField");
                    displayPath = string.Format("Phi Cut/{0}/", phiCutFrequencyNode.Text);
                    while (phiNodes.MoveNext())
                    {
                        string phi = phiNodes.Current.GetAttribute("phi", string.Empty);
                        string xpath = "/Document/FarFieldOutput/FarField[@frequency='" + frequency + "']/PhiCut/PhiCutFarField[@phi='" + phi + "']/Value";
                        string nodeName = "phi=" + float.Parse(phi).ToString();

                        BuildFarFieldOutputElementTree(phiCutFrequencyNode.Nodes.Add(nodeName), xpath, "theta", displayPath + nodeName);
                    }
                }
            }
        }

        private void BuildFarFieldOutputElementTree (TreeNode root , string xpath , string parent,string displayPath)
        {
            for (int i = 0 ; i < farField2DElements.Length ; i++)
            {
                TreeNode elementNode = root.Nodes.Add ( farField2DElements[i] );

                displayPath += "/" + farField2DElements[i];
                for (int j = 0 ; j < farField2DSubElementsDisplayNames.Length ; j++)
                {
                    TreeNode subElementNode = elementNode.Nodes.Add (farField2DSubElementsDisplayNames[j] );
                    subElementNode.Tag = new FarFieldElementReader ( this.resultFileName , 
                        farField2DSubElementsInnerNames[j] , 
                        parent , 
                        xpath + "/" + farField2DElements[i],
                        displayPath + "/" + farField2DSubElementsDisplayNames [j]);
                }
            }
        }

        private void btnFFT_Click ( object sender , EventArgs e )
        {
            if (tvResultGuide.SelectedNode != null
                && tvResultGuide.SelectedNode.Tag != null)
            {
                OutputReader reader = tvResultGuide.SelectedNode.Tag as OutputReader;

                Result1D result1d = reader.Load() as Result1D;

                if (result1d != null && result1d.Domain == DataDomainType.TimeDomain)
                {
                    FFTForm fft = new FFTForm (result1d,this);
                    fft.Show ( );
                }
            }
        }       
        
    }
}
