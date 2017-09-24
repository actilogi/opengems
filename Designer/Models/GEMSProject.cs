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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models.GeometryModels;
using GEMS.Designer.Utility;

namespace GEMS.Designer.Models
{
    public partial class GEMSProject
    {
        public GEMSProject ( )
        {
            currentFileName = string.Empty;
        }

        #region Delagate and Event Declarations

        public class DataChangedEventArgs : EventArgs
        {
            public enum DataChangeType
            {
                Initialized ,
                SingleEdited ,
                SingleDeleted ,
                SingleCutted ,
                SinglePasted ,
                SingleCreated
            }

            public DataChangeType changedType;

            //This will be null when the changedType is Initialized
            public GEMSSingle changedSingle;

            public DataChangedEventArgs ( DataChangeType changedType , GEMSSingle changedSingle )
            {
                this.changedType = changedType;
                this.changedSingle = changedSingle;
            }
        }

        public class PreCalculateGoOnEventArgs : EventArgs
        {
            public enum MessageType
            {
                Title ,
                SubTitle ,
                Content ,
                Important ,
                Warning ,
                Error ,
                Step ,
                SubStep ,
            }

            public string Msg;
            public MessageType MsgType;

            public PreCalculateGoOnEventArgs ( string msg , MessageType msgType )
            {
                this.Msg = msg;
                this.MsgType = msgType;
            }
        }

        public delegate void CurrentObjectChangedEventHandler ( object sender , EventArgs e );
        public delegate void DataChangedEventHandler ( object sender , DataChangedEventArgs e );
        public delegate void PreCalculateGoOnEventHandler ( object sender , PreCalculateGoOnEventArgs e );

        // CurrentObjectChanged is raised when a different object is selected from the tree or the scene
        public virtual event CurrentObjectChangedEventHandler CurrentObjectChanged;

        // DataChanged is raised when the data changes, 
        // either because the user edited the single and its operations
        public virtual event DataChangedEventHandler DataChanged;

        // PreCalculateGoOn is raised when project need broadcast some messages 
        // because one or some steps of pre-calculate has been finished
        public virtual event PreCalculateGoOnEventHandler PreCalculateGoOn;


        public void DataChangedAlarm ( DataChangedEventArgs.DataChangeType changedType , GEMSSingle changedSingle )
        {
            if (changedSingle != null || changedType != DataChangedEventArgs.DataChangeType.Initialized)
                this.isUpdated = true;

            DataChangedEventArgs e = new DataChangedEventArgs ( changedType , changedSingle );

            if (DataChanged != null)
            {
                DataChanged ( this , e );
            }
        }

        public void CurrentObjectChangedAlarm ( )
        {
            if (CurrentObjectChanged != null)
            {
                CurrentObjectChanged ( this , EventArgs.Empty );
            }
        }

        public void PreCalculateGoOnAlarm ( string message , PreCalculateGoOnEventArgs.MessageType messageType )
        {
            if (PreCalculateGoOn != null)
            {
                PreCalculateGoOn ( this , new PreCalculateGoOnEventArgs ( message , messageType ) );
            }
        }

        #endregion

        #region Internal Members

        //Members for controling
        private string currentFileName = string.Empty;
        private Guid projectId = Guid.Empty;
        private object currentSelectedObject;
        private GEMSSingle singleClipboard;
        private bool isUpdated = false;
        private int maxSingleId = -1;
        private int maxOperationId = -1;
        private int maxMaterialId = -1;

        //Members for simulation information
        private GEMSEnvironment environment;
        private GEMSComputationalDomain computationalDomain;
        private GEMSExcitationSource excitationSource;
        private GEMSParallel parallel;
        private GEMSMesh mesh;
        private List<GEMSMaterial> materials;
        private List<GEMSSingle> singles;
        private List<GeometryOperation> operations;
        private GEMSHuygensBox huygensBox;

        //Memeber for pre calculate information
        private int timeStep = 5000;
        private string preCalculationFileName = string.Empty;

        #endregion

        #region Public Members

        public bool IsUpdated
        {
            get { return isUpdated; }
            set { isUpdated = value; }
        }

        public Guid ProjectId
        {
            get { return projectId; }
        }

        public string CurrentFileName
        {
            get { return currentFileName; }
        }

        public Object CurrentSelectedObject
        {
            get { return currentSelectedObject; }
            set
            {
                currentSelectedObject = value;

                CurrentObjectChangedAlarm ( );
            }
        }

        public GEMSEnvironment Environment
        {
            get { return environment; }
        }

        public GEMSComputationalDomain ComputationalDomain
        {
            get { return computationalDomain; }
        }

        public GEMSExcitationSource ExcitationSource
        {
            get { return excitationSource; }
        }

        public List<GEMSSingle> Singles
        {
            get { return singles; }
        }

        public List<GeometryOperation> Operations
        {
            get { return operations; }
        }


        public List<GEMSMaterial> Materials
        {
            get { return this.materials; }
            set { this.materials = value; }
        }

        public GEMSHuygensBox HuygensBox
        {
            get { return huygensBox; }
            set { huygensBox = value; }
        }

        public GEMSParallel Parallel
        {
            get { return parallel; }
            set { parallel = value; }
        }

        public GEMSMesh Mesh
        {
            get { return mesh; }
        }

        public string PreCalculationFileName
        {
            get { return preCalculationFileName; }
            set { preCalculationFileName = value; }
        }

        public int TimeStep
        {
            get { return timeStep; }
            set { timeStep = value; }
        }

        #endregion

        #region  PreCalculate Const Memeber

        private const float C0 = 3E8f;


        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialize a new GEMS project 
        /// </summary>
        public void Initialize ( )
        {
            //Create the objects needed
            this.projectId = Guid.NewGuid ( );
            this.environment = new GEMSEnvironment ( );
            this.excitationSource = new GEMSExcitationSource ( this );
            this.computationalDomain = new GEMSComputationalDomain ( this );
            this.mesh = new GEMSMesh ( this );
            this.parallel = new GEMSParallel ( this );
            this.huygensBox = new GEMSHuygensBox ( this );

            this.materials = new List<GEMSMaterial> ( );
            this.singles = new List<GEMSSingle> ( );
            this.operations = new List<GeometryOperation> ( );

            this.timeStep = 5000;
            this.preCalculationFileName = string.Empty;

            //Update all the panes
            DataChangedAlarm ( DataChangedEventArgs.DataChangeType.Initialized , null );
        }

        /// <summary>
        /// Load the information of GEMS project from specified file
        /// </summary>
        public void Load ( string projectName )
        {
            this.currentFileName = projectName;

            //Open the xml document
            XPathDocument document = new XPathDocument ( currentFileName );
            XPathNavigator navigator = document.CreateNavigator ( );

            //Read the id of the project
            try
            {
                navigator.MoveToChild ( "Document" , string.Empty );
                projectId = new Guid ( navigator.GetAttribute ( "id" , string.Empty ) );
            }
            catch
            {
                projectId = Guid.NewGuid ( );
            }
            navigator.MoveToParent ( );


            //Load the environment
            try
            {
                environment = new GEMSEnvironment ( navigator.SelectSingleNode ( "/Document/Environment" ).Clone ( ) );
            }
            catch (XPathException)
            {
                environment = new GEMSEnvironment ( );
            }

            //Load the excitation source
            //There's only one excitation source in a project
            try
            {
                excitationSource = new GEMSExcitationSource ( navigator.SelectSingleNode ( "/Document/Frequency" ).Clone ( ) , this );
            }
            catch (XPathException)
            {
                excitationSource = new GEMSExcitationSource ( this );
            }

            //Load the computation domain
            try
            {
                computationalDomain = new GEMSComputationalDomain ( navigator.SelectSingleNode ( "/Document/Domain" ).Clone ( ) , this );
            }
            catch (XPathException)
            {
                computationalDomain = new GEMSComputationalDomain ( this );
            }

            //Load the materials 
            try
            {
                LoadMaterials ( navigator.Clone ( ) );
            }
            catch (XPathException)
            {
                materials = new List<GEMSMaterial> ( );
            }

            //Load the mesh
            try
            {
                mesh = new GEMSMesh ( navigator.Clone ( ) , this );
            }
            catch (XPathException)
            {
                mesh = new GEMSMesh ( this );
            }

            //Load the parallel information 
            try
            {
                parallel = new GEMSParallel ( navigator.SelectSingleNode ( "/Document/Parallel" ).Clone ( ) , this );
            }
            catch (XPathException)
            {
                parallel = new GEMSParallel ( this );
            }

            //Load the huygen's box
            try
            {
                huygensBox = new GEMSHuygensBox ( navigator.SelectSingleNode ( "/Document/HuygensBox" ).Clone ( ) , this );
            }
            catch (XPathException)
            {
                huygensBox = new GEMSHuygensBox ( this );
            }


            //Load the singles
            try
            {
                LoadSingles ( navigator );
            }
            catch (XPathException)
            {
                this.singles = new List<GEMSSingle> ( );
            }

            try
            {
                LoadPrecalculateInfo ( navigator.SelectSingleNode ( "/Document/PCFSetting" ).Clone ( ) );
            }
            catch (XPathException)
            {
                this.timeStep = 5000;
                this.preCalculationFileName = string.Empty;
            }
            catch (NullReferenceException)
            {
                this.timeStep = 5000;
                this.preCalculationFileName = string.Empty;
            }

            //Update all the panes
            DataChangedAlarm ( DataChangedEventArgs.DataChangeType.Initialized , null );
        }

        /// <summary>
        /// Load the material list of project
        /// </summary>
        private void LoadMaterials ( XPathNavigator navigator )
        {
            XPathExpression expr;
            expr = navigator.Compile ( "/Document/Materials/Material" );
            //Sort the selected matrials by id.
            expr.AddSort ( "@id" , XmlSortOrder.Ascending , XmlCaseOrder.None , "" , XmlDataType.Number );

            XPathNodeIterator nodes = navigator.Select ( expr );

            materials = new List<GEMSMaterial> ( );

            //Add the user customized material
            while (nodes.MoveNext ( ))
            {
                //New material
                GEMSMaterial material = new GEMSMaterial ( nodes.Current.Clone ( ) , this );

                if (material.Id > maxMaterialId)
                    maxMaterialId = material.Id;

                //Add to the list
                materials.Add ( material );
            }
        }

        /// <summary>
        /// Load the singles
        /// </summary>
        private void LoadSingles ( XPathNavigator navigator )
        {
            XPathExpression expr;
            expr = navigator.Compile ( "/Document/Models/Single" );
            //Sort the singles by id.
            expr.AddSort ( "@id" , XmlSortOrder.Ascending , XmlCaseOrder.None , "" , XmlDataType.Number );

            XPathNodeIterator nodes = navigator.Select ( expr );

            singles = new List<GEMSSingle> ( );
            operations = new List<GeometryOperation> ( );

            //Add the singleNodes
            while (nodes.MoveNext ( ))
            {
                //New single
                GEMSSingle single = new GEMSSingle ( nodes.Current.Clone ( ) , this );

                if (single.Id > maxSingleId)
                    maxSingleId = single.Id;

                //Add to the list
                singles.Add ( single );
            }
        }

        /// <summary>
        /// Load the information of precalculation
        /// </summary>
        private void LoadPrecalculateInfo ( XPathNavigator navigator )
        {
            navigator.MoveToChild ( "PCFName" , string.Empty );
            this.preCalculationFileName = navigator.GetAttribute ( "value" , string.Empty );
            navigator.MoveToParent ( );

            navigator.MoveToChild ( "TimeStep" , string.Empty );
            this.timeStep = int.Parse ( navigator.GetAttribute ( "value" , string.Empty ) );
            navigator.MoveToParent ( );
        }

        /// <summary>
        /// Save the project
        /// </summary>
        public void Save ( )
        {
            if (currentFileName == string.Empty)
                return;

            XmlDocument doc = new XmlDocument ( );
            doc.LoadXml ( this.BuildOuterXmlString ( ) );
            doc.Save ( currentFileName );
            isUpdated = false;

            if (this.preCalculationFileName == string.Empty)
                this.preCalculationFileName = Path.ChangeExtension ( currentFileName , "pcf" );
        }


        /// <summary>
        /// Save the project to the file with specified filename
        /// </summary>
        public void Save ( string fileName )
        {
            this.currentFileName = fileName;

            this.Save ( );
        }

        #endregion

        #region Outer Xml String Build
        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns></returns>
        private string BuildOuterXmlString ( )
        {
            StringBuilder projectStringBuilder = new StringBuilder ( );

            //The document header
            projectStringBuilder.Append ( "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>" );
            projectStringBuilder.AppendFormat ( "<Document version=\"1.00.00\" type=\"scene\" id=\"{0}\">" , projectId );

            //Enviornment
            projectStringBuilder.Append ( environment.BuildOuterXmlString ( ) );

            //PCF setting
            projectStringBuilder.Append ( BuildPrecalculateInfoOuterXmlString ( ) );

            //Computational Domain
            projectStringBuilder.Append ( computationalDomain.BuildOuterXmlString ( ) );

            //ExcitationSource
            projectStringBuilder.Append ( excitationSource.BuildOuterXmlString ( ) );

            //Huygen's Box
            projectStringBuilder.Append ( huygensBox.BuildOuterXmlString ( ) );

            //Materials
            projectStringBuilder.Append ( BuildMaterialsOuterXmlString ( ) );

            //Coordinates
            projectStringBuilder.Append ( BuildCoordinateOuterXmlString ( ) );

            //Signals
            projectStringBuilder.Append ( BuildSignalsOuterXmlString ( ) );

            //Opertions
            projectStringBuilder.Append ( BuildOperationsOuterXmlString ( ) );

            //Mesh and Key Points
            projectStringBuilder.Append ( mesh.BuildOuterXmlString ( ) );

            //Parallel information
            projectStringBuilder.Append ( parallel.BuildOuterXmlString ( ) );

            //Tail
            projectStringBuilder.Append ( "</Document>" );

            return projectStringBuilder.ToString ( );
        }

        /// <summary>
        /// Build a xml string containing the information of all materils
        /// </summary>
        private string BuildMaterialsOuterXmlString ( )
        {
            StringBuilder materilsNodeBuilder = new StringBuilder ( );

            //Header
            materilsNodeBuilder.Append ( "<Materials>" );

            //Content
            foreach (GEMSMaterial material in materials)
            {
                materilsNodeBuilder.Append ( material.BuildOuterXmlString ( ) );
            }

            //Tail
            materilsNodeBuilder.Append ( "</Materials>" );

            return materilsNodeBuilder.ToString ( );
        }

        /// <summary>
        /// Build a xml string containing the information of all signals
        /// </summary>
        private string BuildSignalsOuterXmlString ( )
        {
            StringBuilder signalsNodeBuilder = new StringBuilder ( );

            //Header
            signalsNodeBuilder.Append ( "<Models>" );

            //Content
            foreach (GEMSSingle signal in this.singles)
            {
                signalsNodeBuilder.Append ( signal.BuildOuterXmlString ( ) );
            }

            //Tail
            signalsNodeBuilder.Append ( "</Models>" );

            return signalsNodeBuilder.ToString ( );
        }

        /// <summary>
        /// Build a xml string containing the information of all signals
        /// </summary>
        private string BuildOperationsOuterXmlString ( )
        {
            StringBuilder operationsNodeBuilder = new StringBuilder ( );

            //Header
            operationsNodeBuilder.Append ( "<Operations>" );

            //Content
            foreach (GeometryOperation operation in operations)
            {
                operationsNodeBuilder.Append ( operation.BuildOuterXmlString ( ) );
            }

            //Tail
            operationsNodeBuilder.Append ( "</Operations>" );

            return operationsNodeBuilder.ToString ( );
        }

        /// <summary>
        /// Build a xml string containing the information of coordinate,
        /// and this method just used to keep the compatibility  with GEMS business version
        /// </summary>
        private string BuildCoordinateOuterXmlString ( )
        {
            StringBuilder coordinateNodeBuilder = new StringBuilder ( );

            coordinateNodeBuilder.Append ( "<Coordinates>" );
            coordinateNodeBuilder.Append ( "<Coordinate reference=\"-1\" id=\"0\" name=\"Global CS\" >" );
            coordinateNodeBuilder.Append ( "<Origin x=\"0\" y=\"0\" z=\"0\" ux=\"3\" uy=\"3\" uz=\"3\" />" );
            coordinateNodeBuilder.Append ( "<AxisX x=\"1\" y=\"0\" z=\"0\" ux=\"3\" uy=\"3\" uz=\"3\" />" );
            coordinateNodeBuilder.Append ( "<AxisY x=\"0\" y=\"1\" z=\"0\" ux=\"3\" uy=\"3\" uz=\"3\" />" );
            coordinateNodeBuilder.Append ( "</Coordinate>" );
            coordinateNodeBuilder.Append ( "</Coordinates>" );

            return coordinateNodeBuilder.ToString ( );
        }

        /// <summary>
        /// Build a xml string containing the information of pcf setting,
        /// </summary>
        private string BuildPrecalculateInfoOuterXmlString ( )
        {
            StringBuilder pcfNodeBuilder = new StringBuilder ( );

            pcfNodeBuilder.Append ( "<PCFSetting>" );
            pcfNodeBuilder.AppendFormat ( "<PCFName value=\"{0}\" />" , this.preCalculationFileName );
            pcfNodeBuilder.AppendFormat ( "<TimeStep value=\"{0}\" enabled=\"1\" />" , this.timeStep );
            pcfNodeBuilder.Append ( "</PCFSetting>" );

            return pcfNodeBuilder.ToString ( );
        }

        #endregion

        #region Material Methods

        /// <summary>
        /// Get one Material object with specified id,
        /// if not founded, return unavailable material
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GEMSMaterial SearchMaterial ( int id )
        {
            foreach (GEMSMaterial material in materials)
            {
                if (material.Id == id)
                    return material;
            }

            //Nothing founded
            return null;
        }

        /// <summary>
        /// Get one Material object with specified name,
        /// if not founded, return unavailable material
        /// </summary>
        public GEMSMaterial SearchMaterial ( string name )
        {
            foreach (GEMSMaterial material in materials)
            {
                if (material.Name == name)
                    return material;
            }

            //Nothing founded
            return null;
        }

        /// <summary>
        /// Create a new material object ,and assign it a new id
        /// This method will not add this new object into the existed material list
        /// </summary>
        /// <returns></returns>
        public GEMSMaterial CreateNewMaterial ( )
        {
            maxMaterialId++;
            return new GEMSMaterial ( maxMaterialId , this );
        }

        /// <summary>
        /// Ensure the name of material has not been used
        /// </summary>
        public bool ValidateMaterialName ( string name , int id )
        {
            foreach (GEMSMaterial material in materials)
            {
                if (material.Name == name && material.Id != id)
                    return false;
            }

            return true;
        }


        #endregion

        #region Single Methods

        /// <summary>
        /// Performing when user press some keys of keyboard
        /// </summary>
        public void PerformKeyDown ( KeyEventArgs e )
        {
            //Capture the message when user press "ctrl + x","ctrl+c(C)","ctrl + v(V)","d(D)"
            //on the single
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                //Paste a new single to the list
                PasteNewSingle ( );

                return;
            }

            GEMSSingle targetSingle = GetCurrentSelectedSingle ( );
            if (targetSingle != null)
            {
                if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                {
                    //Add the selected single to clipboard
                    CopySingle ( targetSingle );

                }
                else if (e.KeyCode == Keys.X && e.Modifiers == Keys.Control)
                {
                    CutSingle ( targetSingle );
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    //Delete the selected single
                    if (MessageBox.Show ( "Really want to delete this single?" , "Warning" ,
                        MessageBoxButtons.OKCancel , MessageBoxIcon.Question ) == DialogResult.OK)
                    {
                        RemoveSingle ( targetSingle );
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private GEMSSingle GetCurrentSelectedSingle ( )
        {
            if (currentSelectedObject is GEMSSingle)
            {
                return currentSelectedObject as GEMSSingle;
            }
            else if (currentSelectedObject is GeometryOperation)
            {
                return ( currentSelectedObject as GeometryOperation ).Parent;
            }
            else
                return null;
        }

        /// <summary>
        /// Put the selected single into clipboard
        /// </summary>
        public void CopySingle ( GEMSSingle selectedSingle )
        {
            this.singleClipboard = selectedSingle;
        }

        /// <summary>
        /// Ensure which single is selected and then put it to the clipboard
        /// </summary>
        public void CopySingle ( )
        {
            CopySingle ( GetCurrentSelectedSingle ( ) );
        }

        /// <summary>
        /// Put the selected single into clipboard and remove it from single list
        /// </summary>
        public void CutSingle ( GEMSSingle selectedSingle )
        {
            this.singleClipboard = selectedSingle;

            RemoveSingle ( selectedSingle );
        }

        /// <summary>
        /// Put the selected single into clipboard and remove it from single list
        /// </summary>
        public void CutSingle ( )
        {
            CutSingle ( GetCurrentSelectedSingle ( ) );
        }

        /// <summary>
        /// Remove the selected single from single list
        /// </summary>
        public void RemoveSingle ( GEMSSingle removedSingle )
        {
            this.singles.Remove ( removedSingle );

            //Update all the panes
            DataChangedAlarm ( DataChangedEventArgs.DataChangeType.SingleDeleted , removedSingle );
        }

        /// <summary>
        /// Add a new single to current list, 
        /// and the information of the new single is based on the single in the clipboard
        /// </summary>
        public void PasteNewSingle ( )
        {
            if (this.singleClipboard != null)
            {
                GEMSSingle newSingle = null;

                //Ensure whether the single being pasted is existed in the single list or just in the clipboard
                bool isExisted = false;
                foreach (GEMSSingle single in singles)
                {
                    if (single.Id == singleClipboard.Id)
                    {
                        isExisted = true;
                        break;
                    }
                }

                if (isExisted)
                {
                    maxSingleId++;

                    //Create a new single
                    newSingle = this.singleClipboard.Clone ( maxSingleId );
                }
                else
                {
                    newSingle = this.singleClipboard;
                }

                //Add to the single list
                this.singles.Add ( newSingle );

                //Update all the panes
                DataChangedAlarm ( DataChangedEventArgs.DataChangeType.SinglePasted , newSingle );

                this.CurrentSelectedObject = newSingle;
            }
        }

        /// <summary>
        /// Create a new single
        /// </summary>
        public GEMSSingle CreateNewSingle ( GeometryCreateOperation createOperation )
        {
            //Create a new single
            maxSingleId++;
            GEMSSingle newSingle = new GEMSSingle ( maxSingleId , this );
            newSingle.InitilizeSingle ( createOperation );

            //Add to the single list
            this.singles.Add ( newSingle );

            //Create the operation
            this.operations.Add ( createOperation );

            //Update all the panes
            DataChangedAlarm ( DataChangedEventArgs.DataChangeType.SingleCreated , newSingle );

            this.CurrentSelectedObject = newSingle.CreateOperation;

            return newSingle;
        }

        #endregion

        #region Operation Methods

        public int CreateNewOperationId ( )
        {
            maxOperationId++;
            return maxOperationId;
        }

        public void AddOperation ( GeometryOperation operation )
        {
            this.operations.Add ( operation );

            if (maxOperationId < operation.Id)
                maxOperationId = operation.Id;

        }

        #endregion

        #region Simulation Methods

        /// <summary>
        /// The main method
        /// </summary>
        public void PreCalculate ( string fileName , int steps )
        {
            if (fileName == string.Empty)
                fileName = string.Format ( "{0}.pcf" , projectId.ToString ( ) );

            using (BinaryWriter bw = new BinaryWriter ( File.Open ( fileName , FileMode.Create ) ))
            {
                string message = string.Empty;

                message = "\n\nPre calcuate started!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );

                message = "Processing file header information...\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Time step
                bw.Write ( GEMSPreCalculateFileTags.TAG_TIMESTEP );
                bw.Write ( steps );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                //stamp
                bw.Write ( GEMSPreCalculateFileTags.TAG_STAMP );
                bw.Write ( projectId.ToString ( ).ToCharArray ( ) );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                //Ratio
                bw.Write ( GEMSPreCalculateFileTags.TAG_LENUNITRATIO );
                bw.Write ( Length.RatioValue ( environment.DefaultLengthUnit ) );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                //Cell count
                bw.Write ( GEMSPreCalculateFileTags.TAG_CELLCOUNT );
                bw.Write ( mesh.MeshCountInX );
                bw.Write ( mesh.MeshCountInY );
                bw.Write ( mesh.MeshCountInZ );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                message = "Completed!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );


                message = "Processing parallel information...\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Parallel information
                SaveParallel ( bw );

                message = "Completed!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );


                message = "Processing computational domain information....\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Domain
                SaveDomain ( bw );

                message = "Completed!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );


                message = "Processing mesh information...\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Mesh
                SaveMesh ( bw );

                message = "Processing huygensbox information...\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Huygens Box
                SaveHuygensBox ( bw );

                message = "Completed!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );


                message = "Processing pluse information...\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Pluse
                SavePulse ( bw , steps );

                message = "Completed!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );


                message = "Processing material distribution information...\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Material
                SaveMaterialDistribute ( bw );

                message = "Completed!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );


                message = "Processing excitation and output information...\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Step );

                //Exciation and Outputs
                SaveEO ( bw );

                message = "Completed!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );


                message = "Pre calculate ended!\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );

                //End
                bw.Write ( GEMSPreCalculateFileTags.TAG_END_FILE );
            }
        }

        #region Simulation Condition Methods

        public bool CanSetupMesh ( )
        {
            return computationalDomain.IsSimulationAreaNotZero ( );
        }

        public bool CanSetupParallel ( )
        {
            return mesh.IsParallelable ( );
        }

        public bool CanSetupComputationalDomain ( )
        {
            return excitationSource.SourcePluseType != PluseType.None;
        }

        public bool CanSetupHuygensBox ( )
        {
            return mesh.IsParallelable ( );
        }

        public bool CanPreCalculate ( )
        {
            return mesh.IsParallelable ( );
        }

        public bool Validate ( int steps )
        {
            bool isPassed = true;

            string message = string.Empty;
            StringBuilder messageBuilder = new StringBuilder ( );

            #region Excitation Source Validate Code

            message = "Excitation Source :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );

            message = string.Format ( "Type : {0}\n" , this.excitationSource.SourcePluseType.ToString ( ) );
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Content );

            message = string.Format ( "Maximum working frequency : {0}\n" , this.excitationSource.MaxFrequency.ToString ( ) );
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Content );

            message = string.Format ( "Working frequency bandwidth : {0}dB\n" , this.excitationSource.Lossness );
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Content );

            Vector3 minMeshCell = mesh.MinMeshCell;

            //Compute deltaT
            float factor = 0.995f;
            float deltaT = (float)( factor / Math.Sqrt ( 1.0 / ( minMeshCell.X * minMeshCell.X )
                                                    + 1.0 / ( minMeshCell.Y * minMeshCell.Y )
                                                    + 1.0 / ( minMeshCell.Z * minMeshCell.Z ) ) / C0
                                                    * Length.RatioValue ( environment.DefaultLengthUnit ) );

            message = string.Format ( "DeltaT : {0:e}sec \n" , deltaT );
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Content );

            message = string.Format ( "Time Step : {0} \n" , steps );
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Content );

            double[] tsteps;
            double tao = 0.0;
            double[] serials = MathUtility.GetPulseSerials ( 0.0 , excitationSource.MaxFrequency.ChangeUnit ( Frequency.FrequencyUnit.Hz ) , excitationSource.Lossness , excitationSource.SourcePluseType , steps , ref tao , out tsteps );

            if (( steps == 1 ) || ( serials[serials.Length - 1] > serials[serials.Length - 2] && serials[serials.Length - 1] > 0 ))
            {
                message = "Pulse will be truncated\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );
            }
            else
            {
                if (serials[serials.Length - 1] / 1.0 > 0.01)
                {
                    message = "Pulse will be truncated\n";
                    this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Warning );
                }
            }

            #endregion

            #region Domain Validate Code

            message = "\nDomain :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );

            message = "Simulation Range : \n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.SubTitle );
            messageBuilder.AppendFormat ( "X Min : {0} \n" , computationalDomain.MinX );
            messageBuilder.AppendFormat ( "X Max : {0} \n" , computationalDomain.MaxX );
            messageBuilder.AppendFormat ( "Y Min : {0} \n" , computationalDomain.MinY );
            messageBuilder.AppendFormat ( "Y Max : {0} \n" , computationalDomain.MaxY );
            messageBuilder.AppendFormat ( "Z Min : {0} \n" , computationalDomain.MinZ );
            messageBuilder.AppendFormat ( "Z Max : {0} \n" , computationalDomain.MaxZ );
            this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

            message = "Boundary Condition : \n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.SubTitle );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "X Min : {0}, 0 cell(s) \n" , computationalDomain.ConditionXmin.ToString ( ) );
            messageBuilder.AppendFormat ( "X Max : {0}, 0 cell(s) \n" , computationalDomain.ConditionXmax.ToString ( ) );
            messageBuilder.AppendFormat ( "Y Min : {0}, 0 cell(s) \n" , computationalDomain.ConditionYmin.ToString ( ) );
            messageBuilder.AppendFormat ( "Y Max : {0}, 0 cell(s) \n" , computationalDomain.ConditionYmax.ToString ( ) );
            messageBuilder.AppendFormat ( "Z Min : {0}, 0 cell(s) \n" , computationalDomain.ConditionZmin.ToString ( ) );
            messageBuilder.AppendFormat ( "Z Max : {0}, 0 cell(s) \n" , computationalDomain.ConditionZmax.ToString ( ) );
            this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

            #endregion

            #region Cell Validate Code

            message = "\nCell :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );

            message = "X-direction :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.SubTitle );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "Number of Cells : {0}\n" , mesh.MeshCountInX );
            messageBuilder.AppendFormat ( "Max Cell Size : {0}\n" , mesh.MaxMeshCell.X );
            messageBuilder.AppendFormat ( "Min Cell Size : {0}\n" , mesh.MinMeshCell.X );
            messageBuilder.AppendFormat ( "Max Ratio : {0}\n" , mesh.MaxMeshCell.X / mesh.MinMeshCell.X );
            this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

            message = "Y-direction :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.SubTitle );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "Number of Cells : {0}\n" , mesh.MeshCountInY );
            messageBuilder.AppendFormat ( "Max Cell Size : {0}\n" , mesh.MaxMeshCell.Y );
            messageBuilder.AppendFormat ( "Min Cell Size : {0}\n" , mesh.MinMeshCell.Y );
            messageBuilder.AppendFormat ( "Max Ratio : {0}\n" , mesh.MaxMeshCell.Y / mesh.MinMeshCell.Y );
            this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

            message = "Z-direction :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.SubTitle );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "Number of Cells : {0}\n" , mesh.MeshCountInZ );
            messageBuilder.AppendFormat ( "Max Cell Size : {0}\n" , mesh.MaxMeshCell.Z );
            messageBuilder.AppendFormat ( "Min Cell Size : {0}\n" , mesh.MinMeshCell.Z );
            messageBuilder.AppendFormat ( "Max Ratio : {0}\n" , mesh.MaxMeshCell.Z / mesh.MinMeshCell.Z );
            this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

            #endregion

            #region Parallel Validate Code

            message = "\nParallel :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "Number of processors : {0}\n" , parallel.CPUNumber );
            messageBuilder.AppendFormat ( "Balance of parallel : {0}\n" , parallel.Balance );
            this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

            #endregion

            #region Source and Output Validate Code

            StringBuilder sourceMessage = new StringBuilder ( );
            StringBuilder outputMessage = new StringBuilder ( );
            bool isExcitationExisted = false;
            bool isOutputExisted = false;

            //Check the exciatation and output in single
            foreach (GEMSSingle single in singles)
            {
                if (single.CurrentEO is SingleExcitation)
                {
                    sourceMessage.AppendFormat ( "{0} : Enabled\n" , single.Name );
                    isExcitationExisted = true;
                }
                else if (single.CurrentEO is SingleOutput)
                {
                    outputMessage.AppendFormat ( "{0} : Enabled\n" , single.Name );
                    isOutputExisted = true;
                }
            }

            //Check whether existed huygen's box
            bool isHuygensBoxRangeAvaliable = ( mesh.MeshCountInX - huygensBox.MaxX > huygensBox.MinX )
                                    && ( mesh.MeshCountInY - huygensBox.MaxY > huygensBox.MinY )
                                    && ( mesh.MeshCountInZ - huygensBox.MaxZ > huygensBox.MinZ );

            if (huygensBox.IsEnable && isHuygensBoxRangeAvaliable)
            {

                isOutputExisted = true;
            }


            message = "\nSource :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );
            if (isExcitationExisted)
                this.PreCalculateGoOnAlarm ( sourceMessage.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );
            else
            {
                isPassed = false;
                message = "No source available\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Error );
            }

            message = "\nOutput :\n";
            this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Title );
            if (isOutputExisted)
                this.PreCalculateGoOnAlarm ( outputMessage.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );
            else
            {
                isPassed = false;
                message = "No output available\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Error );
            }

            if (huygensBox.IsEnable)
            {
                message = "HuygensBox : ";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Important );

                message = string.Format ( "{0}\n" , isHuygensBoxRangeAvaliable ? "Enabled":"Disabled" );
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.Content );

                message = "Geometry :\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.SubTitle );
                messageBuilder.Remove ( 0 , messageBuilder.Length );
                messageBuilder.AppendFormat ( "X Min : {0}, X Max : {1}\n" , huygensBox.MinX , mesh.MeshCountInX - huygensBox.MaxX );
                messageBuilder.AppendFormat ( "Y Min : {0}, Y Max : {1}\n" , huygensBox.MinY , mesh.MeshCountInY - huygensBox.MaxY );
                messageBuilder.AppendFormat ( "Z Min : {0}, Z Max : {1}\n" , huygensBox.MinZ , mesh.MeshCountInZ - huygensBox.MaxZ );
                this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

                message = "Frequency Domain :\n";
                this.PreCalculateGoOnAlarm ( message , PreCalculateGoOnEventArgs.MessageType.SubTitle );
                messageBuilder.Remove ( 0 , messageBuilder.Length );
                messageBuilder.AppendFormat ( "Num of Frequency : {0}\n" , huygensBox.FrequencyList.Count );
                messageBuilder.AppendFormat ( "Phi-cut : {0} , Num of Cut : {1}\n" ,
                    huygensBox.IsPhiEnable ? "Enable" : "Disable" , huygensBox.PhiList.Count );
                messageBuilder.AppendFormat ( "Theta-cut : {0} , Num of Cut : {1}\n" ,
                    huygensBox.IsThetaEnable ? "Enable" : "Disable" , huygensBox.ThetaList.Count );
                this.PreCalculateGoOnAlarm ( messageBuilder.ToString ( ) , PreCalculateGoOnEventArgs.MessageType.Content );

            }

            #endregion

            return isPassed;
        }

        #endregion

        #region Precalculate Methods

        /// <summary>
        /// Estimate the total steps of the full pre-calculate process
        /// The method used is so stupid!:( , but i didn't know a more better method
        /// </summary>
        public int GetPrecalculateSteps ( )
        {
            int steps = 0;
            steps += 1; //file header
            steps += 1; //parallel
            steps += 1; //domain
            steps += 1; //Mesh
            steps += 1; //huygens box
            steps += 1; //pluse
            steps += 1 + mesh.MeshPointsInX.Count * mesh.MeshPointsInY.Count * mesh.MeshPointsInZ.Count; //material distribution
            steps += 1 + this.singles.Count; // excitation and output

            return steps;
        }

        private void SavePulse ( BinaryWriter bw , int steps )
        {
            Vector3 minMeshCell = mesh.MinMeshCell;

            //Compute deltaT
            float factor = 0.995f;
            float deltaT = (float)( factor / Math.Sqrt ( 1.0 / ( minMeshCell.X * minMeshCell.X )
                                                    + 1.0 / ( minMeshCell.Y * minMeshCell.Y )
                                                    + 1.0 / ( minMeshCell.Z * minMeshCell.Z ) ) / C0
                                                    * Length.RatioValue ( environment.DefaultLengthUnit ) );

            bw.Write ( GEMSPreCalculateFileTags.TAG_PULSE );
            bw.Write ( deltaT );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            double[] tsteps;
            double tao = 0.0;
            double[] serials = MathUtility.GetPulseSerials ( 0.0 ,
                excitationSource.MaxFrequency.ChangeUnit ( Frequency.FrequencyUnit.Hz )
                , excitationSource.Lossness , excitationSource.SourcePluseType , deltaT , steps , ref tao , out tsteps );

            bw.Write ( serials.Length );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            for (int i = 0 ; i < serials.Length ; i++)
            {
                bw.Write ( (float)serials[i] );
            }

            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
        }

        private void SaveMaterialDistribute ( BinaryWriter bw )
        {
            bw.Write ( GEMSPreCalculateFileTags.TAG_EMATERIAL_START );

            //Dictionary<MeshPointKey , MeshPointMaterial> meshPontMaterials = GenerateMaterialDistribution ( );

            //foreach (KeyValuePair<MeshPointKey , MeshPointMaterial> kvp in meshPontMaterials)
            //{
            //    kvp.Key.Write ( bw );

            //    bw.Write ( kvp.Value.MaterialSigma.X );
            //    bw.Write ( kvp.Value.MaterialSigma.Y );
            //    bw.Write ( kvp.Value.MaterialSigma.Z );

            //    bw.Write ( kvp.Value.MaterialEpsilon.X );
            //    bw.Write ( kvp.Value.MaterialEpsilon.Y );
            //    bw.Write ( kvp.Value.MaterialEpsilon.Z );
            //}

            GenerateMaterialDistribution ( bw );

            bw.Write ( GEMSPreCalculateFileTags.TAG_EMATERIAL_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

        }

        private void SaveMesh ( BinaryWriter bw )
        {
            bw.Write ( GEMSPreCalculateFileTags.TAG_CELLSIZE_X );
            bw.Write ( mesh.MeshPointsInX[0] );
            for (int i = 1 ; i < mesh.MeshPointsInX.Count ; i++)
            {
                bw.Write ( mesh.MeshPointsInX[i] - mesh.MeshPointsInX[i - 1] );
            }
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            bw.Write ( GEMSPreCalculateFileTags.TAG_CELLSIZE_Y );
            bw.Write ( mesh.MeshPointsInY[0] );
            for (int i = 1 ; i < mesh.MeshPointsInY.Count ; i++)
            {
                bw.Write ( mesh.MeshPointsInY[i] - mesh.MeshPointsInY[i - 1] );
            }
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            bw.Write ( GEMSPreCalculateFileTags.TAG_CELLSIZE_Z );
            bw.Write ( mesh.MeshPointsInZ[0] );
            for (int i = 1 ; i < mesh.MeshPointsInZ.Count ; i++)
            {
                bw.Write ( mesh.MeshPointsInZ[i] - mesh.MeshPointsInZ[i - 1] );
            }
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

        }

        private void SaveParallel ( BinaryWriter bw )
        {
            bw.Write ( GEMSPreCalculateFileTags.TAG_PARALLEL );
            bw.Write ( parallel.AreaList.Count );
            foreach (GEMSParallelArea area in parallel.AreaList)
            {
                if (!area.IsPEC)
                {
                    bw.Write ( area.Start.X );
                    bw.Write ( area.End.X );
                    bw.Write ( area.Start.Y );
                    bw.Write ( area.End.Y );
                    bw.Write ( area.Start.Z );
                    bw.Write ( area.End.Z );
                }
            }

            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
        }

        private void SaveDomain ( BinaryWriter bw )
        {
            bw.Write ( GEMSPreCalculateFileTags.TAG_DOMAIN );

            bw.Write ( (int)computationalDomain.ConditionXmin );
            bw.Write ( (int)computationalDomain.ConditionXmax );
            bw.Write ( (int)computationalDomain.ConditionYmin );
            bw.Write ( (int)computationalDomain.ConditionYmax );
            bw.Write ( (int)computationalDomain.ConditionZmin );
            bw.Write ( (int)computationalDomain.ConditionZmax );


            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
        }

        private void SaveHuygensBox ( BinaryWriter bw )
        {
            bw.Write ( GEMSPreCalculateFileTags.TAG_HUYGENSBOX );

            //Genernal Information
            bw.Write ( huygensBox.IsEnable );
            bw.Write ( huygensBox.IsMegneticCurrentOnly );
            bw.Write ( huygensBox.ApertureField );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            //Geometry
            bw.Write ( huygensBox.MinX );
            bw.Write ( huygensBox.MinY );
            bw.Write ( huygensBox.MinZ );
            bw.Write ( mesh.MeshCountInX - huygensBox.MaxX );
            bw.Write ( mesh.MeshCountInY - huygensBox.MaxY );
            bw.Write ( mesh.MeshCountInZ - huygensBox.MaxZ );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            //ReferencePoint
            Vector3 refpoint = huygensBox.ReferencePoint.GetDirectXVector ( Length.LengthUnit.meter );
            bw.Write ( refpoint.X );
            bw.Write ( refpoint.Y );
            bw.Write ( refpoint.Z );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            //2D Phi Plane
            bw.Write ( huygensBox.IsPhiEnable );
            bw.Write ( huygensBox.PhiList.Count );
            foreach (float phis in huygensBox.PhiList)
            {
                bw.Write ( phis );
            }
            bw.Write ( huygensBox.ThetaStart );
            bw.Write ( huygensBox.ThetaEnd );
            bw.Write ( huygensBox.ThetaStep );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            //2D Theta Plane
            bw.Write ( huygensBox.IsThetaEnable );
            bw.Write ( huygensBox.ThetaList.Count );
            foreach (float theta in huygensBox.ThetaList)
            {
                bw.Write ( theta );
            }
            bw.Write ( huygensBox.PhiStart );
            bw.Write ( huygensBox.PhiEnd );
            bw.Write ( huygensBox.PhiStep );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

            bw.Write ( (int)huygensBox.FrequencyUnit );
            bw.Write ( huygensBox.FrequencyList.Count );
            foreach (float frequency in huygensBox.FrequencyList)
            {
                bw.Write ( Frequency.ChangeUnit ( frequency , huygensBox.FrequencyUnit , Frequency.FrequencyUnit.Hz ) );
            }

            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
            bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
        }

        private void SaveEO ( BinaryWriter bw )
        {
            foreach (GEMSSingle single in singles)
            {
                this.PreCalculateGoOnAlarm ( string.Empty , PreCalculateGoOnEventArgs.MessageType.Step );

                #region Voltage Excitation Save Code

                if (single.CurrentEO is VoltageExcitation && single.PrimaryModel is LineModel)
                {
                    LineModel line = single.PrimaryModel as LineModel;
                    VoltageExcitation ve = single.CurrentEO as VoltageExcitation;

                    List<MeshPointKey> keys = line.MoveToMeshPoint ( mesh );

                    //prefix
                    bw.Write ( GEMSPreCalculateFileTags.TAG_EXCITATION_VOLTAGE );

                    //Name     
                    string vename = single.Name.PadRight ( GEMSPreCalculateFileTags.MAX_OUTPUTNAME_LENGTH , '\x0' );
                    bw.Write ( vename.ToCharArray ( ) );

                    //Meshpoint
                    if (single.CurrentEO.Positive == 1)
                    {
                        for (int i = 0 ; i < keys.Count ; i++)
                        {
                            keys[i].Write ( bw );
                        }
                    }
                    else
                    {
                        for (int i = keys.Count - 1 ; i < 0 ; i--)
                        {
                            keys[i].Write ( bw );
                        }
                    }

                    //Data of excitation
                    bw.Write ( ve.ExcitationVoltage.ChangeUnit ( Voltage.VoltageUnit.Volt ) );
                    bw.Write ( ve.TimeDelay.ChangeUnit ( Time.TimeUnit.sec ) );

                    //End
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                    continue;
                }
                #endregion

                #region Voltage Output Save Code

                if (single.CurrentEO is VoltageOutput && single.PrimaryModel is LineModel)
                {
                    LineModel line = single.PrimaryModel as LineModel;
                    VoltageOutput vo = single.CurrentEO as VoltageOutput;

                    List<MeshPointKey> keys = line.MoveToMeshPoint ( mesh );

                    //prefix
                    bw.Write ( GEMSPreCalculateFileTags.TAG_OUTPUT_VOLTAGE );

                    //Name                        
                    string voname = single.Name.PadRight ( GEMSPreCalculateFileTags.MAX_OUTPUTNAME_LENGTH , '\x0' );
                    bw.Write ( voname.ToCharArray ( ) );

                    //Meshpoint
                    if (single.CurrentEO.Positive == 1)
                    {
                        for (int i = 0 ; i < keys.Count ; i++)
                        {
                            keys[i].Write ( bw );
                        }
                    }
                    else
                    {
                        for (int i = keys.Count - 1 ; i < 0 ; i--)
                        {
                            keys[i].Write ( bw );
                        }
                    }

                    //End
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                    continue;
                }
                #endregion

                #region Current Excitation Save Code

                if (single.CurrentEO is CurrentExcitation &&
                    ( single.PrimaryModel is RectangleModel || single.PrimaryModel is RoundModel ))
                {
                    CurrentExcitation ce = single.CurrentEO as CurrentExcitation;
                    ITwoDimensionEO planeExcitation = single.PrimaryModel as ITwoDimensionEO;

                    //prefix
                    bw.Write ( GEMSPreCalculateFileTags.TAG_EXCITATION_CURRENT );

                    //Name                        
                    string voname = single.Name.PadRight ( GEMSPreCalculateFileTags.MAX_OUTPUTNAME_LENGTH , '\x0' );
                    bw.Write ( voname.ToCharArray ( ) );

                    //Mesh points which round the plane
                    int startIndexOnAlineAxis;
                    bool flag;
                    List<MeshPointKey> meshPointKeys = GetBoundary ( planeExcitation , out startIndexOnAlineAxis , out flag );

                    //Output
                    switch (planeExcitation.AlineAxis)
                    {
                        case Axis.X:

                            if (ce.Positive == 1)
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_X );
                            else
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_X_MINUS );

                            bw.Write ( startIndexOnAlineAxis );

                            //Index of the mesh points which rounded the plane
                            bw.Write ( meshPointKeys.Count );
                            if (flag)
                            {
                                for (int i = 0 ; i < meshPointKeys.Count ; i++)
                                {
                                    bw.Write ( meshPointKeys[i].Y );
                                    bw.Write ( meshPointKeys[i].Z );
                                }
                            }
                            else
                            {
                                for (int i = meshPointKeys.Count - 1 ; i >= 0 ; i--)
                                {
                                    bw.Write ( meshPointKeys[i].Y );
                                    bw.Write ( meshPointKeys[i].Z );
                                }
                            }

                            break;
                        case Axis.Y:
                            if (ce.Positive == 1)
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Y );
                            else
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Y_MINUS );

                            bw.Write ( startIndexOnAlineAxis );

                            //Index of the mesh points which rounded the plane
                            bw.Write ( meshPointKeys.Count );
                            if (flag)
                            {
                                for (int i = 0 ; i < meshPointKeys.Count ; i++)
                                {
                                    bw.Write ( meshPointKeys[i].Z );
                                    bw.Write ( meshPointKeys[i].X );
                                }
                            }
                            else
                            {
                                for (int i = meshPointKeys.Count - 1 ; i >= 0 ; i--)
                                {
                                    bw.Write ( meshPointKeys[i].Z );
                                    bw.Write ( meshPointKeys[i].X );
                                }
                            }

                            break;
                        case Axis.Z:
                            if (ce.Positive == 1)
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Z );
                            else
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Z_MINUS );

                            bw.Write ( startIndexOnAlineAxis );

                            //Index of the mesh points which rounded the plane
                            bw.Write ( meshPointKeys.Count );
                            if (flag)
                            {
                                for (int i = 0 ; i < meshPointKeys.Count ; i++)
                                {

                                    bw.Write ( meshPointKeys[i].X );
                                    bw.Write ( meshPointKeys[i].Y );
                                }
                            }
                            else
                            {
                                for (int i = meshPointKeys.Count - 1 ; i >= 0 ; i--)
                                {
                                    bw.Write ( meshPointKeys[i].X );
                                    bw.Write ( meshPointKeys[i].Y );
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    //Data of excitation
                    bw.Write ( ce.ExcitationCurrent.ChangeUnit ( Current.CurrentUnit.amp ) );
                    bw.Write ( ce.TimeDelay.ChangeUnit ( Time.TimeUnit.sec ) );

                    //End
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                    continue;
                }
                #endregion

                #region Current Output Save Code

                if (single.CurrentEO is CurrentOutput &&
                    ( single.PrimaryModel is RectangleModel || single.PrimaryModel is RoundModel ))
                {
                    CurrentOutput co = single.CurrentEO as CurrentOutput;
                    ITwoDimensionEO planeOutput = single.PrimaryModel as ITwoDimensionEO;

                    //prefix
                    bw.Write ( GEMSPreCalculateFileTags.TAG_OUTPUT_CURRENT );

                    //Name                        
                    string voname = single.Name.PadRight ( GEMSPreCalculateFileTags.MAX_OUTPUTNAME_LENGTH , '\x0' );
                    bw.Write ( voname.ToCharArray ( ) );

                    //Mesh points which round the plane
                    int startIndexOnAlineAxis;
                    bool flag;
                    List<MeshPointKey> meshPointKeys = GetBoundary ( planeOutput , out startIndexOnAlineAxis , out flag );

                    //Output
                    switch (planeOutput.AlineAxis)
                    {
                        case Axis.X:

                            if (co.Positive == 1)
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_X );
                            else
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_X_MINUS );

                            bw.Write ( startIndexOnAlineAxis );

                            //Index of the mesh points which rounded the plane
                            bw.Write ( meshPointKeys.Count );
                            if (flag)
                            {
                                for (int i = 0 ; i < meshPointKeys.Count ; i++)
                                {
                                    bw.Write ( meshPointKeys[i].Y );
                                    bw.Write ( meshPointKeys[i].Z );
                                }
                            }
                            else
                            {
                                for (int i = meshPointKeys.Count - 1 ; i >= 0 ; i--)
                                {
                                    bw.Write ( meshPointKeys[i].Y );
                                    bw.Write ( meshPointKeys[i].Z );
                                }
                            }
                            break;
                        case Axis.Y:
                            if (co.Positive == 1)
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Y );
                            else
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Y_MINUS );

                            bw.Write ( startIndexOnAlineAxis );

                            //Index of the mesh points which rounded the plane
                            bw.Write ( meshPointKeys.Count );
                            if (flag)
                            {
                                for (int i = 0 ; i < meshPointKeys.Count ; i++)
                                {
                                    bw.Write ( meshPointKeys[i].Z );
                                    bw.Write ( meshPointKeys[i].X );
                                }
                            }
                            else
                            {
                                for (int i = meshPointKeys.Count - 1 ; i >= 0 ; i--)
                                {
                                    bw.Write ( meshPointKeys[i].Z );
                                    bw.Write ( meshPointKeys[i].X );
                                }
                            }

                            break;
                        case Axis.Z:
                            if (co.Positive == 1)
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Z );
                            else
                                bw.Write ( GEMSPreCalculateFileTags.TAG_DIRECTION_Z_MINUS );

                            bw.Write ( startIndexOnAlineAxis );

                            //Index of the mesh points which rounded the plane
                            bw.Write ( meshPointKeys.Count );
                            if (flag)
                            {
                                for (int i = 0 ; i < meshPointKeys.Count ; i++)
                                {

                                    bw.Write ( meshPointKeys[i].X );
                                    bw.Write ( meshPointKeys[i].Y );
                                }
                            }
                            else
                            {
                                for (int i = meshPointKeys.Count - 1 ; i >= 0 ; i--)
                                {
                                    bw.Write ( meshPointKeys[i].X );
                                    bw.Write ( meshPointKeys[i].Y );
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    //End
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                    continue;
                }
                #endregion

                #region Point Output Save Code

                if (single.CurrentEO is PointOutput && single.PrimaryModel is PointModel)
                {
                    PointModel point = single.PrimaryModel as PointModel;
                    PointOutput po = single.CurrentEO as PointOutput;

                    //prefix
                    bw.Write ( GEMSPreCalculateFileTags.TAG_OUTPUT_FIELDONPOINT );

                    //Name                        
                    string voname = single.Name.PadRight ( GEMSPreCalculateFileTags.MAX_OUTPUTNAME_LENGTH , '\x0' );
                    bw.Write ( voname.ToCharArray ( ) );

                    //Flag
                    bw.Write ( po.GetFieldEnableFlag ( ) );

                    //Position
                    List<MeshPointKey> keys = point.MoveToMeshPoint ( mesh );
                    keys[0].Write ( bw );

                    //End
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );
                    bw.Write ( GEMSPreCalculateFileTags.TAG_ELEMENT_END );

                    continue;
                }

                #endregion
            }
        }


        #endregion

        #region Material Distribution Generate Methods

        #region MethPoint Material Distribution Inter result

        public class MeshPointMaterialInOneDirection
        {
            private Vector3 meshPoint;
            private bool isInSingle  = false;
            private float mtrEpsilon;
            private float mtrSigma;

            public MeshPointMaterialInOneDirection ( Vector3 meshPoint )
            {
                this.meshPoint = meshPoint;
            }

            public Vector3 MeshPoint
            {
                get { return meshPoint; }
                set { meshPoint = value; }
            }

            public float MaterialSigma
            {
                get { return mtrSigma; }
                set { mtrSigma = value; }
            }

            public float MaterialEpsilon
            {
                get { return mtrEpsilon; }
                set { mtrEpsilon = value; }
            }

            public bool IsInSingle
            {
                get { return isInSingle; }
                set { isInSingle = value; }
            }
        }

        #endregion

        //List<Vector3> meshPoints = new List<Vector3>();
        public List<CustomVertex.PositionColored> GetMeshPoint ( )
        {
            List<CustomVertex.PositionColored> meshPoints = new List<CustomVertex.PositionColored> ( );

            Dictionary<MeshPointKey , MeshPointMaterial> meshPontMaterials = GenerateMaterialDistribution ( );

            foreach (KeyValuePair<MeshPointKey , MeshPointMaterial> kvp in meshPontMaterials)
            {
                CustomVertex.PositionColored vertex = new CustomVertex.PositionColored ( );

                vertex.Position = kvp.Value.MeshPoint;

                if (kvp.Value.MaterialSigma.X == GEMSMaterial.FreeSpaceSigma
                    && kvp.Value.MaterialSigma.Y == GEMSMaterial.FreeSpaceSigma
                    && kvp.Value.MaterialSigma.Z == GEMSMaterial.FreeSpaceSigma)
                {
                    //No material
                    vertex.Color = Color.White.ToArgb ( );
                }
                else if (( kvp.Value.MaterialSigma.X == GEMSMaterial.FreeSpaceSigma
                    && kvp.Value.MaterialSigma.Y == GEMSMaterial.FreeSpaceSigma ) 
                    || ( kvp.Value.MaterialSigma.X == GEMSMaterial.FreeSpaceSigma
                    && kvp.Value.MaterialSigma.Z == GEMSMaterial.FreeSpaceSigma )
                    || ( kvp.Value.MaterialSigma.Z == GEMSMaterial.FreeSpaceSigma
                    && kvp.Value.MaterialSigma.Y == GEMSMaterial.FreeSpaceSigma ))
                {
                    //Material only in one direction
                    vertex.Color = Color.Blue.ToArgb ( );
                }
                else if (kvp.Value.MaterialSigma.X == GEMSMaterial.FreeSpaceSigma
                    || kvp.Value.MaterialSigma.Y == GEMSMaterial.FreeSpaceSigma
                    || kvp.Value.MaterialSigma.Z == GEMSMaterial.FreeSpaceSigma)
                {
                    //Material only in two directions
                    vertex.Color = Color.Yellow.ToArgb ( );
                }
                else
                    vertex.Color = Color.Red.ToArgb ( );

                meshPoints.Add ( vertex );
            }

            //meshPoints.RemoveAll ( PredicateMeshPointMaterialMesh );

            return meshPoints;
        }

        public static bool PredicateMeshPointMaterialMesh ( CustomVertex.PositionColored meshPoint )
        {
            return meshPoint.Color == Color.White.ToArgb ( );
        }

        /// <summary>
        /// Generate the matrial distribute information with:
        /// geomeotry information,
        /// computational domain,
        /// matrial of every single
        /// </summary>
        private Dictionary<MeshPointKey , MeshPointMaterial> GenerateMaterialDistribution ( )
        {
            #region Pre process
            //Find the singles which meet following conditions:
            //1. whose geometry is a plane( Rectangle,Round )  
            //2. which is not either an excitation or output
            //Then calculating the neareast mesh points to the plane, and getting the start index in the axis which alines
            //Record the index of single and the start index to a dictionary for following calculating
            Dictionary<int , MaterialDistributionPreprocess2DInfo> _2DSingleMeshInfo = new Dictionary<int , MaterialDistributionPreprocess2DInfo> ( );
            Dictionary<int , MaterialDistributionPreprocess1DInfo> _1DSingleMeshInfo = new Dictionary<int , MaterialDistributionPreprocess1DInfo> ( );

            foreach (GEMSSingle single in singles)
            {
                if (single.CurrentEO == null && single.PEC)
                {
                    if (single.PrimaryModel is RectangleModel || single.PrimaryModel is RoundModel)
                    {
                        ITwoDimensionEO plane = (ITwoDimensionEO)single.PrimaryModel;
                        MaterialDistributionPreprocess2DInfo mdpPlane = new MaterialDistributionPreprocess2DInfo ( );

                        int startX , startY , startZ;
                        mesh.GetNearestMeshPoint ( plane.CenterVector3 , out startX , out startY , out startZ );

                        mdpPlane.StartIndex = MeshPointKey.Key ( startX , startY , startZ );

                        _2DSingleMeshInfo.Add ( single.Id , mdpPlane );

                        continue;
                    }

                    if (single.PrimaryModel is LineModel)
                    {
                        LineModel line = (LineModel)single.PrimaryModel;
                        MaterialDistributionPreprocess1DInfo mdpLine = new MaterialDistributionPreprocess1DInfo ( );

                        //Line must be along one axis
                        Vector3 lineDirection = Vector3.Normalize ( line.Node2 - line.Node1 );

                        float xAxisDotValue = Math.Abs ( Vector3.Dot ( lineDirection , new Vector3 ( 1 , 0 , 0 ) ) );
                        float yAxisDotValue = Math.Abs ( Vector3.Dot ( lineDirection , new Vector3 ( 0 , 1 , 0 ) ) );
                        float zAxisDotValue = Math.Abs ( Vector3.Dot ( lineDirection , new Vector3 ( 0 , 0 , 1 ) ) );

                        if (xAxisDotValue > 0)
                        {
                            mdpLine.AlineAxis = Axis.X;
                        }
                        else if (yAxisDotValue > 0)
                        {
                            mdpLine.AlineAxis = Axis.Y;
                        }
                        else if (zAxisDotValue > 0)
                        {
                            mdpLine.AlineAxis = Axis.Z;
                        }
                        else
                            continue;

                        List<MeshPointKey> keys = line.MoveToMeshPoint ( mesh );

                        mdpLine.MeshPoint1 = new Vector3 ( mesh.MeshPointsInX[keys[0].X] , mesh.MeshPointsInY[keys[0].Y] , mesh.MeshPointsInZ[keys[0].Z] );
                        mdpLine.MeshPoint2 = new Vector3 ( mesh.MeshPointsInX[keys[1].X] , mesh.MeshPointsInY[keys[1].Y] , mesh.MeshPointsInZ[keys[1].Z] );
                        mdpLine.MeshPoint1Key = keys[0];
                        mdpLine.MeshPoint2Key = keys[1];

                        _1DSingleMeshInfo.Add ( single.Id , mdpLine );

                        continue;
                    }
                }
            }

            #endregion

            MeshPointKey keyComparer = new MeshPointKey ( );

            Dictionary<MeshPointKey , MeshPointMaterial> materialDistribution = new Dictionary<MeshPointKey , MeshPointMaterial> ( (IEqualityComparer<MeshPointKey>)keyComparer );

            List<float> meshPointsInX = mesh.MeshPointsInX;
            List<float> meshPointsInY = mesh.MeshPointsInY;
            List<float> meshPointsInZ = mesh.MeshPointsInZ;

            //Calculate the matrial of each mesh point
            for (int i = 0 ; i < meshPointsInX.Count ; i++)
            {
                for (int j = 0 ; j < meshPointsInY.Count ; j++)
                {
                    for (int k = 0 ; k < meshPointsInZ.Count ; k++)
                    {
                        this.PreCalculateGoOnAlarm ( string.Empty , PreCalculateGoOnEventArgs.MessageType.Step );

                        //Create a new GEMSMaterialDistributionItem object to record the mesh point and materils
                        Vector3 meshPoint = new Vector3 ( meshPointsInX[i] , meshPointsInY[j] , meshPointsInZ[k] );
                        MeshPointMaterial di = new MeshPointMaterial ( meshPoint );
                        MeshPointKey key = MeshPointKey.Key ( i , j , k );
                        materialDistribution.Add ( key , di );

                        //Create two vector to store the materil of tree axicies
                        Vector3 sigma = Vector3.Empty;
                        Vector3 epsilon = Vector3.Empty;

                        #region Get the vertecies which we need test the relation with each geometry

                        Vector3 meshPointX = Vector3.Empty;  //Point used to locate the points used to compute the material of X direction of meshPoint
                        List<MeshPointMaterialInOneDirection> meshPointXList = new List<MeshPointMaterialInOneDirection> ( ); //Points used to compute the material of X direction

                        Vector3 meshPointY = Vector3.Empty;  //Used to locate the points used to compute the material of Y direction of meshPoint
                        List<MeshPointMaterialInOneDirection> meshPointYList = new List<MeshPointMaterialInOneDirection> ( ); //Points used to compute the material of Y direction

                        Vector3 meshPointZ = Vector3.Empty;  //Used to locate the points used to compute the material of Z direction of meshPoint
                        List<MeshPointMaterialInOneDirection> meshPointZList = new List<MeshPointMaterialInOneDirection> ( ); //Points used to compute the material of Z direction

                        bool computeX = true;
                        bool computeY = true;
                        bool computeZ = true;

                        bool isMeshPointXOnNoThicknessSingle = false;
                        bool isMeshPointYOnNoThicknessSingle = false;
                        bool isMeshPointZOnNoThicknessSingle = false;

                        if (k + 1 == meshPointsInZ.Count)
                        {
                            MeshPointMaterial mpm = materialDistribution[MeshPointKey.Key ( i , j , k - 1 )];
                            sigma.Z = mpm.MaterialSigma.Z;
                            epsilon.Z = mpm.MaterialEpsilon.Z;
                            computeZ = false;
                        }
                        else
                        {
                            meshPointZ = meshPoint;
                            meshPointZ.Z += ( meshPointsInZ[k + 1] - meshPointZ.Z ) / 2;

                            //get the four points round the meshPointZ within the computational domain
                            if (i + 1 < meshPointsInX.Count)
                            {
                                float offsetX = ( meshPointsInX[i + 1] - meshPointZ.X ) / 20;
                                if (j + 1 < meshPointsInY.Count)
                                {
                                    float offsetY = ( meshPointsInY[j + 1] - meshPointZ.Y ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( offsetX , offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }

                                if (j - 1 >= 0)
                                {
                                    float offsetY = ( meshPointZ.Y - meshPointsInY[j - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( offsetX , -offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }
                            }

                            if (i - 1 >= 0)
                            {
                                float offsetX = ( meshPointZ.X - meshPointsInX[i - 1] ) / 20;
                                if (j + 1 < meshPointsInY.Count)
                                {
                                    float offsetY = ( meshPointsInY[j + 1] - meshPointZ.Y ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( -offsetX , offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }

                                if (j - 1 >= 0)
                                {
                                    float offsetY = ( meshPointZ.Y - meshPointsInY[j - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( -offsetX , -offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }
                            }
                        }

                        if (j + 1 == meshPointsInY.Count)
                        {
                            MeshPointMaterial mpm = materialDistribution[MeshPointKey.Key ( i , j - 1 , k )];
                            sigma.Y = mpm.MaterialSigma.Y;
                            epsilon.Y = mpm.MaterialEpsilon.Y;
                            computeY = false;
                        }
                        else
                        {
                            meshPointY = meshPoint;
                            meshPointY.Y += ( meshPointsInY[j + 1] - meshPointY.Y ) / 2;

                            //get the four points round the meshPointY within the computational domain
                            if (i + 1 < meshPointsInX.Count)
                            {
                                float offsetX = ( meshPointsInX[i + 1] - meshPointY.X ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointY.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY + new Vector3 ( offsetX , 0 , offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointY.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY +  new Vector3 ( offsetX , 0 , -offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }
                            }

                            if (i - 1 >= 0)
                            {
                                float offsetX = ( meshPointY.X - meshPointsInX[i - 1] ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointY.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY + new Vector3 ( -offsetX , 0 , offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointY.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY +  new Vector3 ( -offsetX , 0 , -offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }
                            }
                        }

                        if (i + 1 == meshPointsInX.Count)
                        {
                            MeshPointMaterial mpm = materialDistribution[MeshPointKey.Key ( i - 1 , j , k )];
                            sigma.X = mpm.MaterialSigma.X;
                            epsilon.X = mpm.MaterialEpsilon.X;
                            computeX = false;
                        }
                        else
                        {
                            meshPointX = meshPoint;
                            meshPointX.X += ( meshPointsInX[i + 1] - meshPointX.X ) / 2;

                            //get the four points round the meshPointX within the computational domain
                            if (j + 1 < meshPointsInY.Count)
                            {
                                float offsetY = ( meshPointsInY[j + 1] - meshPointX.Y ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointX.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , offsetY , offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointX.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , offsetY , -offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }
                            }

                            if (j - 1 >= 0)
                            {
                                float offsetY = ( meshPointX.Y - meshPointsInY[j - 1] ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointX.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , -offsetY , offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointX.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , -offsetY , -offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }
                            }
                        }

                        #endregion

                        //Compute the matrial of each direction meshpoint
                        foreach (GEMSSingle single in singles)
                        {
                            //If the corresponding GEMSSingle is an exciation or output
                            //we will not calculate it
                            //Also here , we do'nt calculate a plane or a line
                            if (single.CurrentEO != null
                                || ( single.PrimaryModel is PointModel )
                                )
                                continue;

                            #region Compute 1D geometry's material distribution

                            if (single.PrimaryModel is LineModel)
                            {
                                if (!single.PEC)
                                    continue;

                                MaterialDistributionPreprocess1DInfo mdpLine = null;
                                if (!_1DSingleMeshInfo.TryGetValue ( single.Id , out mdpLine ))
                                    continue;

                                LineModel line = new LineModel ( );
                                switch (mdpLine.AlineAxis)
                                {
                                    case Axis.X:
                                        if (computeX)
                                        {
                                            if (j == mdpLine.MeshPoint1Key.Y && k == mdpLine.MeshPoint1Key.Z)
                                            {
                                                bool isInside = line.PositionRelation ( meshPointX , mdpLine.MeshPoint1 , mdpLine.MeshPoint2 );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    sigma.X = GEMSMaterial.PECSigma;
                                                    epsilon.X = GEMSMaterial.PECEplison;
                                                    isMeshPointXOnNoThicknessSingle = true;
                                                }

                                                if (!isMeshPointXOnNoThicknessSingle)
                                                {
                                                    sigma.X = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.X = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }
                                        }
                                        break;
                                    case Axis.Y:
                                        if (computeY)
                                        {
                                            if (i == mdpLine.MeshPoint1Key.X && k == mdpLine.MeshPoint1Key.Z)
                                            {
                                                bool isInside = line.PositionRelation ( meshPointY , mdpLine.MeshPoint1 , mdpLine.MeshPoint2 );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    sigma.Y = GEMSMaterial.PECSigma;
                                                    epsilon.Y =  GEMSMaterial.PECEplison;
                                                    isMeshPointYOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointYOnNoThicknessSingle)
                                                {
                                                    sigma.Y = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Y = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }
                                        }
                                        break;
                                    case Axis.Z:
                                        if (computeZ)
                                        {
                                            if (i == mdpLine.MeshPoint1Key.X && j == mdpLine.MeshPoint1Key.Y)
                                            {
                                                bool isInside = line.PositionRelation ( meshPointZ , mdpLine.MeshPoint1 , mdpLine.MeshPoint2 );
                                                if (isInside)
                                                {
                                                    sigma.Z = GEMSMaterial.PECSigma;
                                                    epsilon.Z =  GEMSMaterial.PECEplison;

                                                    isMeshPointZOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointZOnNoThicknessSingle)
                                                {
                                                    sigma.Z = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Z = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }
                                        }
                                        break;
                                }

                                continue;
                            }

                            #endregion

                            #region Compute 2D geometry's material distribution

                            if (single.PrimaryModel is RectangleModel || single.PrimaryModel is RoundModel)
                            {
                                if (!single.PEC)
                                    continue;

                                MaterialDistributionPreprocess2DInfo mdpPlane = null;
                                if (!_2DSingleMeshInfo.TryGetValue ( single.Id , out mdpPlane ))
                                    continue;

                                ITwoDimensionEO plane = (ITwoDimensionEO)single.PrimaryModel;

                                switch (plane.AlineAxis)
                                {
                                    case Axis.X:
                                        if (i == mdpPlane.StartIndex.X)
                                        {
                                            if (computeZ)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointZ );
                                                if (isInside)
                                                {
                                                    sigma.Z = GEMSMaterial.PECSigma;
                                                    epsilon.Z =  GEMSMaterial.PECEplison;

                                                    isMeshPointZOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointZOnNoThicknessSingle)
                                                {
                                                    sigma.Z = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Z = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            if (computeY)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointY );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    sigma.Y = GEMSMaterial.PECSigma;
                                                    epsilon.Y =  GEMSMaterial.PECEplison;
                                                    isMeshPointYOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointYOnNoThicknessSingle)
                                                {
                                                    sigma.Y = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Y = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            break;
                                        }
                                        else
                                            break;
                                    case Axis.Y:
                                        if (j == mdpPlane.StartIndex.Y)
                                        {
                                            if (computeZ)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointZ );
                                                if (isInside)
                                                {
                                                    sigma.Z = GEMSMaterial.PECSigma;
                                                    epsilon.Z =  GEMSMaterial.PECEplison;
                                                    isMeshPointZOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointZOnNoThicknessSingle)
                                                {
                                                    sigma.Z = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Z = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            if (computeX)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointX );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    sigma.X = GEMSMaterial.PECSigma;
                                                    epsilon.X =  GEMSMaterial.PECEplison;
                                                    isMeshPointXOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointXOnNoThicknessSingle)
                                                {
                                                    sigma.X = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.X = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            break;
                                        }
                                        else
                                            break;
                                    case Axis.Z:
                                        if (k == mdpPlane.StartIndex.Z)
                                        {
                                            if (computeY)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointY );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    sigma.Y = GEMSMaterial.PECSigma;
                                                    epsilon.Y =  GEMSMaterial.PECEplison;
                                                    isMeshPointYOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointYOnNoThicknessSingle)
                                                {
                                                    sigma.Y = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Y = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            if (computeX)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointX );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    sigma.X = GEMSMaterial.PECSigma;
                                                    epsilon.X =  GEMSMaterial.PECEplison;
                                                    isMeshPointXOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointXOnNoThicknessSingle)
                                                {
                                                    sigma.X = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.X = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            break;
                                        }
                                        else
                                            break;
                                }

                                continue;
                            }

                            #endregion

                            #region Compute 3D geometry's material distribution

                            //Retrieve the matrial of z axis
                            if (computeZ)
                            {
                                foreach (MeshPointMaterialInOneDirection mpmInZ in meshPointZList)
                                {
                                    bool isInside = single.PrimaryModel.PositionRelation ( mpmInZ.MeshPoint );

                                    if (isInside) //Give the material of the geometry
                                    {
                                        isMeshPointZOnNoThicknessSingle = false;

                                        if (single.PEC)
                                        {
                                            mpmInZ.MaterialSigma    = GEMSMaterial.PECSigma;
                                            mpmInZ.MaterialEpsilon  =  GEMSMaterial.PECEplison;
                                        }
                                        else
                                        {
                                            if (single.SingleMaterial != null)
                                            {
                                                mpmInZ.MaterialSigma = single.SingleMaterial.SigmaX;
                                                mpmInZ.MaterialEpsilon = single.SingleMaterial.EpsilonX;
                                            }
                                            else
                                            {
                                                mpmInZ.MaterialSigma = GEMSMaterial.PECSigma;
                                                mpmInZ.MaterialEpsilon = GEMSMaterial.PECEplison;
                                            }
                                        }

                                        mpmInZ.IsInSingle = true;
                                    }

                                    if (!mpmInZ.IsInSingle)
                                    {
                                        mpmInZ.MaterialSigma = GEMSMaterial.FreeSpaceSigma;
                                        mpmInZ.MaterialEpsilon = GEMSMaterial.FreeSpaceEplison;
                                    }
                                }
                            }

                            //Retrieve the matrial of y axis
                            if (computeY)
                            {
                                foreach (MeshPointMaterialInOneDirection mpmInY in meshPointYList)
                                {
                                    bool isInside = single.PrimaryModel.PositionRelation ( mpmInY.MeshPoint );
                                    if (isInside) //Give the material of the geometry
                                    {
                                        isMeshPointYOnNoThicknessSingle = false;

                                        if (single.PEC)
                                        {
                                            mpmInY.MaterialSigma = GEMSMaterial.PECSigma;
                                            mpmInY.MaterialEpsilon  = GEMSMaterial.PECEplison;
                                        }
                                        else
                                        {
                                            if (single.SingleMaterial != null)
                                            {
                                                mpmInY.MaterialSigma = single.SingleMaterial.SigmaX;
                                                mpmInY.MaterialEpsilon = single.SingleMaterial.EpsilonX;
                                            }
                                            else
                                            {
                                                mpmInY.MaterialSigma = GEMSMaterial.PECSigma;
                                                mpmInY.MaterialEpsilon = GEMSMaterial.PECEplison;
                                            }
                                        }

                                        mpmInY.IsInSingle = true;
                                    }

                                    if (!mpmInY.IsInSingle)
                                    {
                                        mpmInY.MaterialSigma = GEMSMaterial.FreeSpaceSigma;
                                        mpmInY.MaterialEpsilon = GEMSMaterial.FreeSpaceEplison;
                                    }
                                }
                            }

                            //Retrieve the matrial of x axis
                            if (computeX)
                            {
                                foreach (MeshPointMaterialInOneDirection mpmInX in meshPointXList)
                                {
                                    bool isInside = single.PrimaryModel.PositionRelation ( mpmInX.MeshPoint );
                                    if (isInside) //Give the material of the geometry
                                    {
                                        isMeshPointXOnNoThicknessSingle = false;

                                        if (single.PEC)
                                        {
                                            mpmInX.MaterialSigma = GEMSMaterial.PECSigma;
                                            mpmInX.MaterialEpsilon = GEMSMaterial.PECEplison;
                                        }
                                        else
                                        {
                                            if (single.SingleMaterial != null)
                                            {
                                                mpmInX.MaterialSigma = single.SingleMaterial.SigmaX;
                                                mpmInX.MaterialEpsilon = single.SingleMaterial.EpsilonX;
                                            }
                                            else
                                            {
                                                mpmInX.MaterialSigma = GEMSMaterial.PECSigma;
                                                mpmInX.MaterialEpsilon = GEMSMaterial.PECEplison;
                                            }
                                        }
                                        mpmInX.IsInSingle = true;
                                    }

                                    if (!mpmInX.IsInSingle)
                                    {
                                        mpmInX.MaterialSigma = GEMSMaterial.FreeSpaceSigma;
                                        mpmInX.MaterialEpsilon = GEMSMaterial.FreeSpaceEplison;
                                    }
                                }
                            }
                            #endregion

                        }

                        #region Process the final result

                        if (!isMeshPointXOnNoThicknessSingle)
                        {
                            foreach (MeshPointMaterialInOneDirection mpmInX in meshPointXList)
                            {
                                if (mpmInX.MaterialSigma == GEMSMaterial.PECSigma)
                                {
                                    sigma.X = GEMSMaterial.PECSigma;
                                    epsilon.X = GEMSMaterial.PECEplison;
                                    break;
                                }
                                else
                                {
                                    sigma.X += mpmInX.MaterialSigma;
                                    epsilon.X += mpmInX.MaterialEpsilon;
                                }
                            }
                            if (meshPointXList.Count > 0)
                            {
                                sigma.X = sigma.X / meshPointXList.Count;
                                epsilon.X = epsilon.X / meshPointXList.Count;
                            }
                        }

                        if (!isMeshPointYOnNoThicknessSingle)
                        {
                            foreach (MeshPointMaterialInOneDirection mpmInY in meshPointYList)
                            {
                                if (mpmInY.MaterialSigma == GEMSMaterial.PECSigma)
                                {
                                    sigma.Y = GEMSMaterial.PECSigma;
                                    epsilon.Y = GEMSMaterial.PECEplison;
                                    break;
                                }
                                else
                                {
                                    sigma.Y += mpmInY.MaterialSigma;
                                    epsilon.Y += mpmInY.MaterialEpsilon;
                                }
                            }
                            if (meshPointYList.Count > 0)
                            {
                                sigma.Y = sigma.Y / meshPointYList.Count;
                                epsilon.Y = epsilon.Y / meshPointYList.Count;
                            }
                        }

                        if (!isMeshPointZOnNoThicknessSingle)
                        {
                            foreach (MeshPointMaterialInOneDirection mpmInZ in meshPointZList)
                            {
                                if (mpmInZ.MaterialSigma == GEMSMaterial.PECSigma)
                                {
                                    sigma.Z = GEMSMaterial.PECSigma;
                                    epsilon.Z = GEMSMaterial.PECEplison;
                                    break;
                                }
                                else
                                {
                                    sigma.Z += mpmInZ.MaterialSigma;
                                    epsilon.Z += mpmInZ.MaterialEpsilon;
                                }
                            }
                            if (meshPointZList.Count > 0)
                            {
                                sigma.Z = sigma.Z / meshPointZList.Count;
                                epsilon.Z = epsilon.Z / meshPointZList.Count;
                            }
                        }

                        //if (epsilon.X == 0 && epsilon.Y == 0 && epsilon.Z == 0)
                        //{
                        //    Console.WriteLine ("i = {0},j = {0},k = {0}",i,j,k);
                        //}

                        #endregion

                        //Store the matrial
                        di.MaterialEpsilon = epsilon;
                        di.MaterialSigma = sigma;

                    }
                }
            }

            return materialDistribution;
        }

        /// <summary>
        /// Generate the matrial distribute information with:
        /// geomeotry information,
        /// computational domain,
        /// matrial of every single
        /// </summary>
        private Dictionary<MeshPointKey , MeshPointMaterial> GenerateMaterialDistribution ( BinaryWriter bw )
        {
            #region Pre process
            //Find the singles which meet following conditions:
            //1. whose geometry is a plane( Rectangle,Round )  
            //2. which is not either an excitation or output
            //Then calculating the neareast mesh points to the plane, and getting the start index in the axis which alines
            //Record the index of single and the start index to a dictionary for following calculating
            Dictionary<int , MaterialDistributionPreprocess2DInfo> _2DSingleMeshInfo = new Dictionary<int , MaterialDistributionPreprocess2DInfo> ( );
            Dictionary<int , MaterialDistributionPreprocess1DInfo> _1DSingleMeshInfo = new Dictionary<int , MaterialDistributionPreprocess1DInfo> ( );

            foreach (GEMSSingle single in singles)
            {
                if (single.CurrentEO == null && single.PEC)
                {
                    if (single.PrimaryModel is RectangleModel || single.PrimaryModel is RoundModel)
                    {
                        ITwoDimensionEO plane = (ITwoDimensionEO)single.PrimaryModel;
                        MaterialDistributionPreprocess2DInfo mdpPlane = new MaterialDistributionPreprocess2DInfo ( );

                        int startX , startY , startZ;
                        mesh.GetNearestMeshPoint ( plane.CenterVector3 , out startX , out startY , out startZ );

                        mdpPlane.StartIndex = MeshPointKey.Key ( startX , startY , startZ );

                        _2DSingleMeshInfo.Add ( single.Id , mdpPlane );

                        continue;
                    }

                    if (single.PrimaryModel is LineModel)
                    {
                        LineModel line = (LineModel)single.PrimaryModel;
                        MaterialDistributionPreprocess1DInfo mdpLine = new MaterialDistributionPreprocess1DInfo ( );

                        //Line must be along one axis
                        Vector3 lineDirection = Vector3.Normalize ( line.Node2 - line.Node1 );

                        float xAxisDotValue = Math.Abs ( Vector3.Dot ( lineDirection , new Vector3 ( 1 , 0 , 0 ) ) );
                        float yAxisDotValue = Math.Abs ( Vector3.Dot ( lineDirection , new Vector3 ( 0 , 1 , 0 ) ) );
                        float zAxisDotValue = Math.Abs ( Vector3.Dot ( lineDirection , new Vector3 ( 0 , 0 , 1 ) ) );

                        if (xAxisDotValue > 0)
                        {
                            mdpLine.AlineAxis = Axis.X;
                        }
                        else if (yAxisDotValue > 0)
                        {
                            mdpLine.AlineAxis = Axis.Y;
                        }
                        else if (zAxisDotValue > 0)
                        {
                            mdpLine.AlineAxis = Axis.Z;
                        }
                        else
                            continue;

                        List<MeshPointKey> keys = line.MoveToMeshPoint ( mesh );

                        mdpLine.MeshPoint1 = new Vector3 ( mesh.MeshPointsInX[keys[0].X] , mesh.MeshPointsInY[keys[0].Y] , mesh.MeshPointsInZ[keys[0].Z] );
                        mdpLine.MeshPoint2 = new Vector3 ( mesh.MeshPointsInX[keys[1].X] , mesh.MeshPointsInY[keys[1].Y] , mesh.MeshPointsInZ[keys[1].Z] );
                        mdpLine.MeshPoint1Key = keys[0];
                        mdpLine.MeshPoint2Key = keys[1];

                        _1DSingleMeshInfo.Add ( single.Id , mdpLine );

                        continue;
                    }
                }
            }

            #endregion

            MeshPointKey keyComparer = new MeshPointKey ( );

            Dictionary<MeshPointKey , MeshPointMaterial> materialDistribution = new Dictionary<MeshPointKey , MeshPointMaterial> ( (IEqualityComparer<MeshPointKey>)keyComparer );

            List<float> meshPointsInX = mesh.MeshPointsInX;
            List<float> meshPointsInY = mesh.MeshPointsInY;
            List<float> meshPointsInZ = mesh.MeshPointsInZ;

            //Calculate the matrial of each mesh point
            //Main loop
            for (int i = 0 ; i < meshPointsInX.Count ; i++)
            {
                for (int j = 0 ; j < meshPointsInY.Count ; j++)
                {
                    for (int k = 0 ; k < meshPointsInZ.Count ; k++)
                    {
                        this.PreCalculateGoOnAlarm ( string.Empty , PreCalculateGoOnEventArgs.MessageType.Step );

                        //Create a new GEMSMaterialDistributionItem object to record the mesh point and materils
                        Vector3 meshPoint = new Vector3 ( meshPointsInX[i] , meshPointsInY[j] , meshPointsInZ[k] );
                        MeshPointMaterial di = new MeshPointMaterial ( meshPoint );
                        MeshPointKey key = MeshPointKey.Key ( i , j , k );

                        //Create two vector to store the materil of tree axicies
                        Vector3 sigma = Vector3.Empty;
                        Vector3 epsilon = Vector3.Empty;

                        #region Get the vertecies which we need test the relation with each geometry

                        Vector3 meshPointX = Vector3.Empty;  //Point used to locate the points used to compute the material of X direction of meshPoint
                        List<MeshPointMaterialInOneDirection> meshPointXList = new List<MeshPointMaterialInOneDirection> ( ); //Points used to compute the material of X direction

                        Vector3 meshPointY = Vector3.Empty;  //Used to locate the points used to compute the material of Y direction of meshPoint
                        List<MeshPointMaterialInOneDirection> meshPointYList = new List<MeshPointMaterialInOneDirection> ( ); //Points used to compute the material of Y direction

                        Vector3 meshPointZ = Vector3.Empty;  //Used to locate the points used to compute the material of Z direction of meshPoint
                        List<MeshPointMaterialInOneDirection> meshPointZList = new List<MeshPointMaterialInOneDirection> ( ); //Points used to compute the material of Z direction

                        bool computeX = true;
                        bool computeY = true;
                        bool computeZ = true;

                        bool isMeshPointXOnNoThicknessSingle = false;
                        bool isMeshPointYOnNoThicknessSingle = false;
                        bool isMeshPointZOnNoThicknessSingle = false;

                        if (k + 1 == meshPointsInZ.Count)
                        {
                            MeshPointMaterial mpm = materialDistribution[MeshPointKey.Key ( i , j , k - 1 )];
                            sigma.Z = mpm.MaterialSigma.Z;
                            epsilon.Z = mpm.MaterialEpsilon.Z;
                            computeZ = false;
                        }
                        else
                        {
                            meshPointZ = meshPoint;
                            meshPointZ.Z += ( meshPointsInZ[k + 1] - meshPointZ.Z ) / 2;

                            //get the four points round the meshPointZ within the computational domain
                            if (i + 1 < meshPointsInX.Count)
                            {
                                float offsetX = ( meshPointsInX[i + 1] - meshPointZ.X ) / 20;
                                if (j + 1 < meshPointsInY.Count)
                                {
                                    float offsetY = ( meshPointsInY[j + 1] - meshPointZ.Y ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( offsetX , offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }

                                if (j - 1 >= 0)
                                {
                                    float offsetY = ( meshPointZ.Y - meshPointsInY[j - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( offsetX , -offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }
                            }

                            if (i - 1 >= 0)
                            {
                                float offsetX = ( meshPointZ.X - meshPointsInX[i - 1] ) / 20;
                                if (j + 1 < meshPointsInY.Count)
                                {
                                    float offsetY = ( meshPointsInY[j + 1] - meshPointZ.Y ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( -offsetX , offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }

                                if (j - 1 >= 0)
                                {
                                    float offsetY = ( meshPointZ.Y - meshPointsInY[j - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInZ = new MeshPointMaterialInOneDirection ( meshPointZ + new Vector3 ( -offsetX , -offsetY , 0 ) );
                                    meshPointZList.Add ( mpmInZ );
                                }
                            }
                        }

                        if (j + 1 == meshPointsInY.Count)
                        {
                            MeshPointMaterial mpm = materialDistribution[MeshPointKey.Key ( i , j - 1 , k )];
                            sigma.Y = mpm.MaterialSigma.Y;
                            epsilon.Y = mpm.MaterialEpsilon.Y;
                            computeY = false;
                        }
                        else
                        {
                            meshPointY = meshPoint;
                            meshPointY.Y += ( meshPointsInY[j + 1] - meshPointY.Y ) / 2;

                            //get the four points round the meshPointY within the computational domain
                            if (i + 1 < meshPointsInX.Count)
                            {
                                float offsetX = ( meshPointsInX[i + 1] - meshPointY.X ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointY.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY + new Vector3 ( offsetX , 0 , offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointY.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY +  new Vector3 ( offsetX , 0 , -offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }
                            }

                            if (i - 1 >= 0)
                            {
                                float offsetX = ( meshPointY.X - meshPointsInX[i - 1] ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointY.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY + new Vector3 ( -offsetX , 0 , offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointY.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInY = new MeshPointMaterialInOneDirection ( meshPointY +  new Vector3 ( -offsetX , 0 , -offsetZ ) );
                                    meshPointYList.Add ( mpmInY );
                                }
                            }
                        }

                        if (i + 1 == meshPointsInX.Count)
                        {
                            MeshPointMaterial mpm = materialDistribution[MeshPointKey.Key ( i - 1 , j , k )];
                            sigma.X = mpm.MaterialSigma.X;
                            epsilon.X = mpm.MaterialEpsilon.X;
                            computeX = false;
                        }
                        else
                        {
                            meshPointX = meshPoint;
                            meshPointX.X += ( meshPointsInX[i + 1] - meshPointX.X ) / 2;

                            //get the four points round the meshPointX within the computational domain
                            if (j + 1 < meshPointsInY.Count)
                            {
                                float offsetY = ( meshPointsInY[j + 1] - meshPointX.Y ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointX.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , offsetY , offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointX.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , offsetY , -offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }
                            }

                            if (j - 1 >= 0)
                            {
                                float offsetY = ( meshPointX.Y - meshPointsInY[j - 1] ) / 20;
                                if (k + 1 < meshPointsInZ.Count)
                                {
                                    float offsetZ = ( meshPointsInZ[k + 1] - meshPointX.Z ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , -offsetY , offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }

                                if (k - 1 >= 0)
                                {
                                    float offsetZ = ( meshPointX.Z - meshPointsInZ[k - 1] ) / 20;
                                    MeshPointMaterialInOneDirection mpmInX = new MeshPointMaterialInOneDirection ( meshPointX + new Vector3 ( 0 , -offsetY , -offsetZ ) );
                                    meshPointXList.Add ( mpmInX );
                                }
                            }
                        }

                        #endregion

                        //Compute the matrial of each direction meshpoint
                        foreach (GEMSSingle single in singles)
                        {
                            //If the corresponding GEMSSingle is an exciation or output
                            //we will not calculate it
                            //Also here , we do'nt calculate a plane or a line
                            if (single.CurrentEO != null
                                || ( single.PrimaryModel is PointModel )
                                )
                                continue;

                            #region Compute 1D geometry's material distribution

                            if (single.PrimaryModel is LineModel)
                            {
                                if (!single.PEC)
                                    continue;

                                MaterialDistributionPreprocess1DInfo mdpLine = null;
                                if (!_1DSingleMeshInfo.TryGetValue ( single.Id , out mdpLine ))
                                    continue;

                                LineModel line = new LineModel ( );
                                switch (mdpLine.AlineAxis)
                                {
                                    case Axis.X:
                                        if (computeX)
                                        {
                                            if (j == mdpLine.MeshPoint1Key.Y && k == mdpLine.MeshPoint1Key.Z)
                                            {
                                                bool isInside = line.PositionRelation ( meshPointX , mdpLine.MeshPoint1 , mdpLine.MeshPoint2 );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                   // sigma.X = GEMSMaterial.PECSigma;
                                                   // epsilon.X = GEMSMaterial.PECEplison;

                                                    if (single.PEC)
                                                    {
                                                        sigma.X    = GEMSMaterial.PECSigma;
                                                        epsilon.X  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.X = single.SingleMaterial.SigmaX;
                                                            epsilon.X = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.X = GEMSMaterial.PECSigma;
                                                            epsilon.X = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointXOnNoThicknessSingle = true;
                                                }

                                                if (!isMeshPointXOnNoThicknessSingle)
                                                {
                                                    sigma.X = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.X = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }
                                        }
                                        break;
                                    case Axis.Y:
                                        if (computeY)
                                        {
                                            if (i == mdpLine.MeshPoint1Key.X && k == mdpLine.MeshPoint1Key.Z)
                                            {
                                                bool isInside = line.PositionRelation ( meshPointY , mdpLine.MeshPoint1 , mdpLine.MeshPoint2 );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.Y   = GEMSMaterial.PECSigma;
                                                        epsilon.Y  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.Y = single.SingleMaterial.SigmaX;
                                                            epsilon.Y = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.Y = GEMSMaterial.PECSigma;
                                                            epsilon.Y = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointYOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointYOnNoThicknessSingle)
                                                {
                                                    sigma.Y = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Y = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }
                                        }
                                        break;
                                    case Axis.Z:
                                        if (computeZ)
                                        {
                                            if (i == mdpLine.MeshPoint1Key.X && j == mdpLine.MeshPoint1Key.Y)
                                            {
                                                bool isInside = line.PositionRelation ( meshPointZ , mdpLine.MeshPoint1 , mdpLine.MeshPoint2 );
                                                if (isInside)
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.Z  = GEMSMaterial.PECSigma;
                                                        epsilon.Z  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.Z = single.SingleMaterial.SigmaX;
                                                            epsilon.Z = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.Z = GEMSMaterial.PECSigma;
                                                            epsilon.Z = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointZOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointZOnNoThicknessSingle)
                                                {
                                                    sigma.Z = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Z = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }
                                        }
                                        break;
                                }

                                continue;
                            }

                            #endregion

                            #region Compute 2D geometry's material distribution

                            if (single.PrimaryModel is RectangleModel || single.PrimaryModel is RoundModel)
                            {
                                if (!single.PEC)
                                    continue;

                                MaterialDistributionPreprocess2DInfo mdpPlane = null;
                                if (!_2DSingleMeshInfo.TryGetValue ( single.Id , out mdpPlane ))
                                    continue;

                                ITwoDimensionEO plane = (ITwoDimensionEO)single.PrimaryModel;

                                switch (plane.AlineAxis)
                                {
                                    case Axis.X:
                                        if (i == mdpPlane.StartIndex.X)
                                        {
                                            if (computeZ)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointZ );
                                                if (isInside)
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.Z  = GEMSMaterial.PECSigma;
                                                        epsilon.Z  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.Z = single.SingleMaterial.SigmaX;
                                                            epsilon.Z = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.Z = GEMSMaterial.PECSigma;
                                                            epsilon.Z = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointZOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointZOnNoThicknessSingle)
                                                {
                                                    sigma.Z = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Z = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            if (computeY)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointY );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.Y   = GEMSMaterial.PECSigma;
                                                        epsilon.Y  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.Y = single.SingleMaterial.SigmaX;
                                                            epsilon.Y = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.Y = GEMSMaterial.PECSigma;
                                                            epsilon.Y = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointYOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointYOnNoThicknessSingle)
                                                {
                                                    sigma.Y = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Y = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            break;
                                        }
                                        else
                                            break;
                                    case Axis.Y:
                                        if (j == mdpPlane.StartIndex.Y)
                                        {
                                            if (computeZ)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointZ );
                                                if (isInside)
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.Z  = GEMSMaterial.PECSigma;
                                                        epsilon.Z  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.Z = single.SingleMaterial.SigmaX;
                                                            epsilon.Z = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.Z = GEMSMaterial.PECSigma;
                                                            epsilon.Z = GEMSMaterial.PECEplison;
                                                        }
                                                    }
                                                    isMeshPointZOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointZOnNoThicknessSingle)
                                                {
                                                    sigma.Z = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Z = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            if (computeX)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointX );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.X    = GEMSMaterial.PECSigma;
                                                        epsilon.X  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.X = single.SingleMaterial.SigmaX;
                                                            epsilon.X = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.X = GEMSMaterial.PECSigma;
                                                            epsilon.X = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointXOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointXOnNoThicknessSingle)
                                                {
                                                    sigma.X = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.X = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            break;
                                        }
                                        else
                                            break;
                                    case Axis.Z:
                                        if (k == mdpPlane.StartIndex.Z)
                                        {
                                            if (computeY)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointY );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.Y   = GEMSMaterial.PECSigma;
                                                        epsilon.Y  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.Y = single.SingleMaterial.SigmaX;
                                                            epsilon.Y = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.Y = GEMSMaterial.PECSigma;
                                                            epsilon.Y = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointYOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointYOnNoThicknessSingle)
                                                {
                                                    sigma.Y = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.Y = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            if (computeX)
                                            {
                                                bool isInside = plane.PositionRelationOneXYPlane ( meshPointX );
                                                if (isInside) //Give the material of the geometry
                                                {
                                                    if (single.PEC)
                                                    {
                                                        sigma.X    = GEMSMaterial.PECSigma;
                                                        epsilon.X  =  GEMSMaterial.PECEplison;
                                                    }
                                                    else
                                                    {
                                                        if (single.SingleMaterial != null)
                                                        {
                                                            sigma.X = single.SingleMaterial.SigmaX;
                                                            epsilon.X = single.SingleMaterial.EpsilonX;
                                                        }
                                                        else
                                                        {
                                                            sigma.X = GEMSMaterial.PECSigma;
                                                            epsilon.X = GEMSMaterial.PECEplison;
                                                        }
                                                    }

                                                    isMeshPointXOnNoThicknessSingle = true;
                                                }
                                                if (!isMeshPointXOnNoThicknessSingle)
                                                {
                                                    sigma.X = GEMSMaterial.FreeSpaceSigma;
                                                    epsilon.X = GEMSMaterial.FreeSpaceEplison;
                                                }
                                            }

                                            break;
                                        }
                                        else
                                            break;
                                }

                                continue;
                            }

                            #endregion

                            #region Compute 3D geometry's material distribution

                            //Retrieve the matrial of z axis
                            if (computeZ)
                            {
                                foreach (MeshPointMaterialInOneDirection mpmInZ in meshPointZList)
                                {
                                    bool isInside = single.PrimaryModel.PositionRelation ( mpmInZ.MeshPoint );

                                    if (isInside) //Give the material of the geometry
                                    {
                                        isMeshPointZOnNoThicknessSingle = false;

                                        if (single.PEC)
                                        {
                                            mpmInZ.MaterialSigma    = GEMSMaterial.PECSigma;
                                            mpmInZ.MaterialEpsilon  =  GEMSMaterial.PECEplison;
                                        }
                                        else
                                        {
                                            if (single.SingleMaterial != null)
                                            {
                                                mpmInZ.MaterialSigma = single.SingleMaterial.SigmaX;
                                                mpmInZ.MaterialEpsilon = single.SingleMaterial.EpsilonX;
                                            }
                                            else
                                            {
                                                mpmInZ.MaterialSigma = GEMSMaterial.PECSigma;
                                                mpmInZ.MaterialEpsilon = GEMSMaterial.PECEplison;
                                            }
                                        }

                                        mpmInZ.IsInSingle = true;
                                    }

                                    if (!mpmInZ.IsInSingle)
                                    {
                                        mpmInZ.MaterialSigma = GEMSMaterial.FreeSpaceSigma;
                                        mpmInZ.MaterialEpsilon = GEMSMaterial.FreeSpaceEplison;
                                    }
                                }
                            }

                            //Retrieve the matrial of y axis
                            if (computeY)
                            {
                                foreach (MeshPointMaterialInOneDirection mpmInY in meshPointYList)
                                {
                                    bool isInside = single.PrimaryModel.PositionRelation ( mpmInY.MeshPoint );
                                    if (isInside) //Give the material of the geometry
                                    {
                                        isMeshPointYOnNoThicknessSingle = false;

                                        if (single.PEC)
                                        {
                                            mpmInY.MaterialSigma = GEMSMaterial.PECSigma;
                                            mpmInY.MaterialEpsilon  = GEMSMaterial.PECEplison;
                                        }
                                        else
                                        {
                                            if (single.SingleMaterial != null)
                                            {
                                                mpmInY.MaterialSigma = single.SingleMaterial.SigmaX;
                                                mpmInY.MaterialEpsilon = single.SingleMaterial.EpsilonX;
                                            }
                                            else
                                            {
                                                mpmInY.MaterialSigma = GEMSMaterial.PECSigma;
                                                mpmInY.MaterialEpsilon = GEMSMaterial.PECEplison;
                                            }
                                        }

                                        mpmInY.IsInSingle = true;
                                    }

                                    if (!mpmInY.IsInSingle)
                                    {
                                        mpmInY.MaterialSigma = GEMSMaterial.FreeSpaceSigma;
                                        mpmInY.MaterialEpsilon = GEMSMaterial.FreeSpaceEplison;
                                    }
                                }
                            }

                            //Retrieve the matrial of x axis
                            if (computeX)
                            {
                                foreach (MeshPointMaterialInOneDirection mpmInX in meshPointXList)
                                {
                                    bool isInside = single.PrimaryModel.PositionRelation ( mpmInX.MeshPoint );
                                    if (isInside) //Give the material of the geometry
                                    {
                                        isMeshPointXOnNoThicknessSingle = false;

                                        if (single.PEC)
                                        {
                                            mpmInX.MaterialSigma = GEMSMaterial.PECSigma;
                                            mpmInX.MaterialEpsilon = GEMSMaterial.PECEplison;
                                        }
                                        else
                                        {
                                            if (single.SingleMaterial != null)
                                            {
                                                mpmInX.MaterialSigma = single.SingleMaterial.SigmaX;
                                                mpmInX.MaterialEpsilon = single.SingleMaterial.EpsilonX;
                                            }
                                            else
                                            {
                                                mpmInX.MaterialSigma = GEMSMaterial.PECSigma;
                                                mpmInX.MaterialEpsilon = GEMSMaterial.PECEplison;
                                            }
                                        }
                                        mpmInX.IsInSingle = true;
                                    }

                                    if (!mpmInX.IsInSingle)
                                    {
                                        mpmInX.MaterialSigma = GEMSMaterial.FreeSpaceSigma;
                                        mpmInX.MaterialEpsilon = GEMSMaterial.FreeSpaceEplison;
                                    }
                                }
                            }
                            #endregion

                        }

                        #region Process the final result


                        if (!isMeshPointXOnNoThicknessSingle && computeX)
                        {
                            sigma.X = 0.0f;
                            epsilon.X = 0.0f;

                            foreach (MeshPointMaterialInOneDirection mpmInX in meshPointXList)
                            {
                                if (mpmInX.MaterialSigma == GEMSMaterial.PECSigma)
                                {
                                    sigma.X = GEMSMaterial.PECSigma;
                                    epsilon.X = GEMSMaterial.PECEplison;
                                    break;
                                }
                                else
                                {
                                    sigma.X += mpmInX.MaterialSigma;
                                    epsilon.X += mpmInX.MaterialEpsilon;
                                }
                            }
                            if (meshPointXList.Count > 0)
                            {
                                sigma.X = sigma.X / meshPointXList.Count;
                                epsilon.X = epsilon.X / meshPointXList.Count;
                            }
                        }

                        if (!isMeshPointYOnNoThicknessSingle && computeY)
                        {
                            sigma.Y = 0.0f;
                            epsilon.Y = 0.0f;

                            foreach (MeshPointMaterialInOneDirection mpmInY in meshPointYList)
                            {
                                if (mpmInY.MaterialSigma == GEMSMaterial.PECSigma)
                                {
                                    sigma.Y = GEMSMaterial.PECSigma;
                                    epsilon.Y = GEMSMaterial.PECEplison;
                                    break;
                                }
                                else
                                {
                                    sigma.Y += mpmInY.MaterialSigma;
                                    epsilon.Y += mpmInY.MaterialEpsilon;
                                }
                            }
                            if (meshPointYList.Count > 0)
                            {
                                sigma.Y = sigma.Y / meshPointYList.Count;
                                epsilon.Y = epsilon.Y / meshPointYList.Count;
                            }
                        }

                        if (!isMeshPointZOnNoThicknessSingle && computeZ)
                        {
                            sigma.Z = 0.0f;
                            epsilon.Z = 0.0f;

                            foreach (MeshPointMaterialInOneDirection mpmInZ in meshPointZList)
                            {
                                if (mpmInZ.MaterialSigma == GEMSMaterial.PECSigma)
                                {
                                    sigma.Z = GEMSMaterial.PECSigma;
                                    epsilon.Z = GEMSMaterial.PECEplison;
                                    break;
                                }
                                else
                                {
                                    sigma.Z += mpmInZ.MaterialSigma;
                                    epsilon.Z += mpmInZ.MaterialEpsilon;
                                }
                            }
                            if (meshPointZList.Count > 0)
                            {
                                sigma.Z = sigma.Z / meshPointZList.Count;
                                epsilon.Z = epsilon.Z / meshPointZList.Count;
                            }
                        }

                        #endregion

                        //Store the matrial
                        di.MaterialEpsilon = epsilon;
                        di.MaterialSigma = sigma;

                        //Store the some material 
                        if (i == mesh.MeshPointsInX.Count - 2
                            || j == mesh.MeshPointsInY.Count - 2
                            || k == mesh.MeshPointsInZ.Count - 2)
                        {
                            materialDistribution.Add ( key , di );
                        }

                        //Write to file
                        key.Write ( bw );

                        bw.Write ( di.MaterialSigma.X );
                        bw.Write ( di.MaterialSigma.Y );
                        bw.Write ( di.MaterialSigma.Z );

                        bw.Write ( di.MaterialEpsilon.X );
                        bw.Write ( di.MaterialEpsilon.Y );
                        bw.Write ( di.MaterialEpsilon.Z );
                    }
                }
            }

            return materialDistribution;
        }

        /// <summary>
        /// Retrieve the two neareast mesh points , between which the interset is
        /// The result just contains the indexes of the mesh points
        /// </summary>
        private List<int> RetrieveMeshPoints ( List<float> meshPoints , float intersect )
        {
            List<int> results = new List<int> ( );

            if (intersect >= meshPoints[meshPoints.Count - 1])
            {
                results.Add ( meshPoints.Count - 1 );
                return results;
            }

            if (intersect <= meshPoints[0])
            {
                results.Add ( 0 );
                return results;
            }

            for (int index = 0 ; index < meshPoints.Count - 1 ; index++)
            {
                if (intersect >= meshPoints[index] && intersect <= meshPoints[index + 1])
                {
                    results.Add ( index );
                    results.Add ( index + 1 );

                    return results;
                }
            }

            return results;
        }

        #endregion

        #region Plane Operation Methods

        /// <summary>
        /// Get the round's boundary points ,which are in the mesh
        /// </summary>       
        /// <returns></returns>
        private List<MeshPointKey> GetBoundary ( ITwoDimensionEO eoPlane , out int startIndexOnAlineAxis , out bool flag )
        {
            startIndexOnAlineAxis = -1;
            int[, ,] meshIndexArray = new int[mesh.MeshPointsInX.Count , mesh.MeshPointsInY.Count , mesh.MeshPointsInZ.Count];

            int startX = 0 , startY = 0 , startZ = 0;
            Vector3 nearestCenterMeshPoint = mesh.GetNearestMeshPoint ( eoPlane.CenterVector3 , out startX , out startY , out startZ );

            //Get the boundingbox index
            int minX , minY , minZ;
            int maxX , maxY , maxZ;

            Vector3 meshPoint1 = mesh.GetNearestMeshPoint ( eoPlane.MinVector3 , out minX , out minY , out minZ );
            Vector3 meshPoint2 = mesh.GetNearestMeshPoint ( eoPlane.MaxVector3 , out maxX , out maxY , out maxZ );

            int[,] meshPlane;
            List<MeshPointKey> boundaryMeshPointKeys = new List<MeshPointKey> ( );
            List<Point> boundaryPoints;
            flag = false;

            int n = 0;
            int m = 0;
            switch (eoPlane.AlineAxis)
            {
                case Axis.X:
                    //Sign the value
                    startIndexOnAlineAxis = startX;

                    //Check each mesh point whether is inside or outside the round
                    //if inside,the array element is 1,otherwise 0
                    for (int j = 0 ; j < mesh.MeshPointsInY.Count ; j++)
                    {
                        for (int k = 0 ; k < mesh.MeshPointsInZ.Count ; k++)
                        {
                            Vector3 meshPoint = new Vector3 ( mesh.MeshPointsInX[startX] , mesh.MeshPointsInY[j] , mesh.MeshPointsInZ[k] );

                            if (eoPlane.PositionRelationOneXYPlane ( meshPoint ))
                            {
                                meshIndexArray[startX , j , k] = 1;
                            }
                        }
                    }


                    //enlarge the mesh points
                    for (int j = 0 ; j < mesh.MeshPointsInY.Count ; j++)
                    {
                        for (int k = 0 ; k < mesh.MeshPointsInZ.Count ; k++)
                        {
                            if (meshIndexArray[startX , j , k] == 1)
                            {
                                if (k + 1 < mesh.MeshPointsInZ.Count)
                                {
                                    meshIndexArray[startX , j , k + 1] = ( meshIndexArray[startX , j , k + 1] == 1 ) ? 1 : 2;

                                    if (j - 1 >= 0)
                                        meshIndexArray[startX , j - 1 , k + 1] = ( meshIndexArray[startX , j - 1 , k + 1] == 1 ) ? 1 : 2;
                                }

                                if (j + 1 < mesh.MeshPointsInX.Count)
                                {
                                    meshIndexArray[startX , j + 1 , k] = ( meshIndexArray[startX , j + 1 , k] == 1 ) ? 1 : 2;

                                    if (k - 1 >= 0)
                                        meshIndexArray[startX , j + 1 , k - 1] = ( meshIndexArray[startX , j + 1 , k - 1] == 1 ) ? 1 : 2;
                                }

                                if (k + 1 < mesh.MeshPointsInZ.Count && j + 1 < mesh.MeshPointsInX.Count)
                                    meshIndexArray[startX , j + 1 , k + 1] = ( meshIndexArray[startX , j + 1 , k + 1] == 1 ) ? 1 : 2;

                                if (k - 1 >= 0)
                                    meshIndexArray[startX , j , k - 1] = ( meshIndexArray[startX , j , k - 1] == 1 ) ? 1 : 2;
                                if (j - 1 >= 0)
                                    meshIndexArray[startX , j - 1 , k] = ( meshIndexArray[startX , j - 1 , k] == 1 ) ? 1 : 2;

                                if (k - 1 >= 0 && j - 1 >= 0)
                                    meshIndexArray[startX , j - 1 , k - 1] = ( meshIndexArray[startX , j - 1 , k - 1] == 1 ) ? 1 : 2;
                            }
                        }
                    }

                    //Refill the round plane
                    for (int j = 0 ; j < mesh.MeshPointsInY.Count ; j++)
                    {
                        for (int k = 0 ; k < mesh.MeshPointsInZ.Count ; k++)
                        {
                            if (meshIndexArray[startX , j , k] == 1)
                            {
                                meshIndexArray[startX , j , k] = 2;
                            }
                        }
                    }

                    minY = minY - 2 > 0 ? minY - 2 : 0;
                    minZ = minZ - 2 > 0 ? minZ - 2 : 0;
                    maxY = maxY + 2 <= mesh.MeshPointsInY.Count - 1 ? maxY + 2 : mesh.MeshPointsInY.Count - 1;
                    maxZ = maxZ + 2 <= mesh.MeshPointsInZ.Count - 1 ? maxZ + 2 : mesh.MeshPointsInZ.Count - 1;

                    n = maxY - minY + 1;
                    m = maxZ - minZ + 1;
                    meshPlane = new int[n , m];

                    //Copy the data
                    for (int j = minY ; j <= maxY ; j++)
                    {
                        for (int k = minZ ; k <= maxZ ; k++)
                        {
                            meshPlane[j - minY , k - minZ] = meshIndexArray[startX , j , k];
                        }
                    }
                    boundaryPoints = MathUtility.GetBoundary ( n , m , minY , minZ , meshPlane , out flag );

                    foreach (Point p in boundaryPoints)
                    {
                        MeshPointKey key = MeshPointKey.Key ( startX , p.X , p.Y );
                        boundaryMeshPointKeys.Add ( key );
                    }

                    break;

                case Axis.Y:
                    //Sign the value
                    startIndexOnAlineAxis = startY;

                    //Check each mesh point whether is inside or outside the round
                    //if inside,the array element is 1,otherwise 0
                    for (int i = 0 ; i < mesh.MeshPointsInX.Count ; i++)
                    {
                        for (int k = 0 ; k < mesh.MeshPointsInZ.Count ; k++)
                        {
                            Vector3 meshPoint = new Vector3 ( mesh.MeshPointsInX[i] , mesh.MeshPointsInY[startY] , mesh.MeshPointsInZ[k] );

                            if (eoPlane.PositionRelationOneXYPlane ( meshPoint ))
                            {
                                meshIndexArray[i , startY , k] = 1;
                            }
                        }
                    }

                    //enlarge the mesh points
                    for (int i = 0 ; i < mesh.MeshPointsInX.Count ; i++)
                    {
                        for (int k = 0 ; k < mesh.MeshPointsInZ.Count ; k++)
                        {
                            if (meshIndexArray[i , startY , k] == 1)
                            {
                                if (k + 1 < mesh.MeshPointsInZ.Count)
                                {
                                    meshIndexArray[i , startY , k + 1] = ( meshIndexArray[i , startY , k + 1] == 1 ) ? 1 : 2;

                                    if (i - 1 >= 0)
                                        meshIndexArray[i - 1 , startY , k + 1] = ( meshIndexArray[i - 1 , startY , k + 1] == 1 ) ? 1 : 2;
                                }

                                if (i + 1 < mesh.MeshPointsInX.Count)
                                {
                                    meshIndexArray[i + 1 , startY , k] = ( meshIndexArray[i + 1 , startY , k] == 1 ) ? 1 : 2;

                                    if (k - 1 >= 0)
                                        meshIndexArray[i + 1 , startY , k - 1] = ( meshIndexArray[i + 1 , startY , k - 1] == 1 ) ? 1 : 2;
                                }

                                if (k + 1 < mesh.MeshPointsInZ.Count && i + 1 < mesh.MeshPointsInX.Count)
                                    meshIndexArray[i + 1 , startY , k + 1] = ( meshIndexArray[i + 1 , startY , k + 1] == 1 ) ? 1 : 2;

                                if (k - 1 >= 0)
                                    meshIndexArray[i , startY , k - 1] = ( meshIndexArray[i , startY , k - 1] == 1 ) ? 1 : 2;
                                if (i - 1 >= 0)
                                    meshIndexArray[i - 1 , startY , k] = ( meshIndexArray[i - 1 , startY , k] == 1 ) ? 1 : 2;

                                if (k - 1 >= 0 && i - 1 >= 0)
                                    meshIndexArray[i - 1 , startY , k - 1] = ( meshIndexArray[i - 1 , startY , k - 1] == 1 ) ? 1 : 2;
                            }
                        }
                    }

                    //Refill the round plane
                    for (int i = 0 ; i < mesh.MeshPointsInX.Count ; i++)
                    {
                        for (int k = 0 ; k < mesh.MeshPointsInZ.Count ; k++)
                        {
                            if (meshIndexArray[i , startY , k] == 1)
                            {
                                meshIndexArray[i , startY , k] = 2;
                            }

                            //if (meshIndexArray[i, startY, k] == 1)
                            //    boundaryPoints.Add(new Point(i,k));
                        }
                    }

                    minX = minX - 2 > 0 ? minX - 2 : 0;
                    minZ = minZ - 2 > 0 ? minZ - 2 : 0;
                    maxX = maxX + 2 <= mesh.MeshPointsInX.Count - 1 ? maxX + 2 : mesh.MeshPointsInX.Count - 1;
                    maxZ = maxZ + 2 <= mesh.MeshPointsInZ.Count - 1 ? maxZ + 2 : mesh.MeshPointsInZ.Count - 1;

                    n = maxZ - minZ + 1;
                    m = maxX - minX + 1;
                    meshPlane = new int[n , m];

                    //Copy the data
                    for (int i = minZ; i <=  maxZ; i++)
                    {
                        for (int k =  minX ; k <= maxX ; k++)
                        {
                            meshPlane[i - minZ , k - minX] = meshIndexArray[k , startY , i];
                        }
                    }

                    boundaryPoints = MathUtility.GetBoundary ( n , m , minZ , minX , meshPlane , out flag );

                    foreach (Point p in boundaryPoints)
                    {
                        MeshPointKey key = MeshPointKey.Key ( p.Y, startY , p.X );
                        boundaryMeshPointKeys.Add ( key );
                    }

                  /*  n = maxX - minX + 1;
                    m = maxZ - minZ + 1;
                    meshPlane = new int[n , m];

                    //Copy the data
                    for (int i = minX ; i <= maxX ; i++)
                    {
                        for (int k = minZ ; k <= maxZ ; k++)
                        {
                            meshPlane[i - minX , k - minZ] = meshIndexArray[i , startY , k];
                        }
                    }

                    boundaryPoints = MathUtility.GetBoundary ( n , m , minX , minZ , meshPlane , out flag );

                    foreach (Point p in boundaryPoints)
                    {
                        MeshPointKey key = MeshPointKey.Key ( p.X , startY , p.Y );
                        boundaryMeshPointKeys.Add ( key );
                    }*/

                    break;

                case Axis.Z:
                    //Sign the value
                    startIndexOnAlineAxis = startZ;

                    //Check each mesh point whether is inside or outside the round
                    //if inside,the array element is 1,otherwise 0
                    for (int i = 0 ; i < mesh.MeshPointsInX.Count ; i++)
                    {
                        for (int j = 0 ; j < mesh.MeshPointsInY.Count ; j++)
                        {
                            Vector3 meshPoint = new Vector3 ( mesh.MeshPointsInX[i] , mesh.MeshPointsInY[j] , mesh.MeshPointsInZ[startZ] );

                            if (eoPlane.PositionRelationOneXYPlane ( meshPoint ))
                            {
                                meshIndexArray[i , j , startZ] = 1;
                            }
                        }
                    }

                    //enlarge the mesh points
                    for (int i = 0 ; i < mesh.MeshPointsInX.Count ; i++)
                    {
                        for (int j = 0 ; j < mesh.MeshPointsInY.Count ; j++)
                        {
                            if (meshIndexArray[i , j , startZ] == 1)
                            {
                                if (j + 1 < mesh.MeshPointsInY.Count)
                                {
                                    meshIndexArray[i , j + 1 , startZ] = ( meshIndexArray[i , j + 1 , startZ] == 1 ) ? 1 : 2;

                                    if (i - 1 >= 0)
                                        meshIndexArray[i - 1 , j + 1 , startZ] = ( meshIndexArray[i - 1 , j + 1 , startZ] == 1 ) ? 1 : 2;
                                }

                                if (i + 1 < mesh.MeshPointsInX.Count)
                                {
                                    meshIndexArray[i + 1 , j , startZ] = ( meshIndexArray[i + 1 , j , startZ] == 1 ) ? 1 : 2;

                                    if (j - 1 >= 0)
                                        meshIndexArray[i + 1 , j - 1 , startZ] = ( meshIndexArray[i + 1 , j - 1 , startZ] == 1 ) ? 1 : 2;
                                }

                                if (j + 1 < mesh.MeshPointsInY.Count && i + 1 < mesh.MeshPointsInX.Count)
                                    meshIndexArray[i + 1 , j + 1 , startZ] = ( meshIndexArray[i + 1 , j + 1 , startZ] == 1 ) ? 1 : 2;

                                if (j - 1 >= 0)
                                    meshIndexArray[i , j - 1 , startZ] = ( meshIndexArray[i , j - 1 , startZ] == 1 ) ? 1 : 2;

                                if (i - 1 >= 0)
                                    meshIndexArray[i - 1 , j , startZ] = ( meshIndexArray[i - 1 , j , startZ] == 1 ) ? 1 : 2;

                                if (i - 1 >= 0 && j - 1 >= 0)
                                    meshIndexArray[i - 1 , j - 1 , startZ] = ( meshIndexArray[i - 1 , j - 1 , startZ] == 1 ) ? 1 : 2;
                            }
                        }
                    }

                    //Refill the round plane
                    for (int i = 0 ; i < mesh.MeshPointsInX.Count ; i++)
                    {
                        for (int j = 0 ; j < mesh.MeshPointsInY.Count ; j++)
                        {
                            if (meshIndexArray[i , j , startZ] == 2 || meshIndexArray[i , j , startZ] == 1)
                            {
                                meshIndexArray[i , j , startZ] = 2;
                            }
                        }
                    }

                    minX = minX - 2 > 0 ? minX - 2 : 0;
                    minY = minY - 2 > 0 ? minY - 2 : 0;
                    maxX = maxX + 2 <= mesh.MeshPointsInX.Count - 1 ? maxX + 2 : mesh.MeshPointsInX.Count - 1;
                    maxY = maxY + 2 <= mesh.MeshPointsInY.Count - 1 ? maxY + 2 : mesh.MeshPointsInY.Count - 1;

                    n = maxX - minX + 1;
                    m = maxY - minY + 1;
                    meshPlane = new int[n , m];

                    //Copy the data
                    for (int i = minX ; i <= maxX ; i++)
                    {
                        for (int j = minY ; j <= maxY ; j++)
                        {
                            meshPlane[i - minX , j - minY] = meshIndexArray[i , j , startZ];
                        }
                    }

                    //Calculate the boundary
                    boundaryPoints = MathUtility.GetBoundary ( n , m , minX , minY , meshPlane , out flag );

                    foreach (Point p in boundaryPoints)
                    {
                        MeshPointKey key = MeshPointKey.Key ( p.X , p.Y , startZ );
                        boundaryMeshPointKeys.Add ( key );
                    }

                    break;
                default:
                    break;
            }

            return boundaryMeshPointKeys;
        }

        #endregion

        #endregion
    }
}
