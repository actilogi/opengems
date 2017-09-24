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
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GEMS.Designer.Forms;
using GEMS.Designer.Panes;
using GEMS.Designer.Properties;
using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Tools;

namespace GEMS.Designer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Command members

        //Commands of setting view to the model
        private Command commandResetView= null;
		private Command commandXYView = null;
        private Command commandYZView = null;
        private Command commandZXView = null;
        private Command commandFitView = null;
        private Command commandDomainView = null;

        //Commands of file operations
        private Command commandFileOpen = null;
        private Command commandFileSave = null;
        private Command commandFileSaveAs = null;
        private Command commandFileNew = null;
        private Command commandExit = null;
        private Command commandSaveScreenshot = null;

        //Commands of single edit
        private Command commandCut = null;
        private Command commandCopy = null;
        private Command commandPaste = null;

        //Commands of simulation settings
        private Command commandSource = null;
        private Command commandDomain = null;
        private Command commandGeneralOptions = null;
        private Command commandMesh = null;
        private Command commandParallel = null;
        private Command commandHuygensBox = null;
        private Command commandPreCalculate = null;

        //Commands of Single EO
        private Command commandVoltageExcitation = null;
        private Command commandVoltageOutput = null;
        private Command commandCurrentExcitation = null;
        private Command commandCurrentOutput = null;
        private Command commandPointOutput = null;
        private Command commandRemoveExcitation = null;
        private Command commandRemoveOutput = null;

        //Commands of Modeling
        private Command commandSelect = null;
        private Command commandCreateCubiod = null;
        private Command commandCreateCone = null;
        private Command commandCreateCylinder = null;
        private Command commandCreatePoint = null;
        private Command commandCreateLine = null;
        private Command commandCreateRectangle = null;
        private Command commandCreateRound = null;
        private Command commandCreateSphere = null;

        //Helper
        private Command commandAbout = null;
        private Command commandHelperContent = null;

        #endregion

        #region Internal Members

        GEMSProject m_project = null;
        Tool lastActiveTool = null;

        private const int MaxRecentFileCount = 10;

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeCommands();

            //Create a project let user to edit
            CreateNewProject();

            SelectTool();            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();

            SaveCurrentProject ( );

        }   
      
        /// <summary>
        /// This function is used to initialize all the commands in the system
        /// </summary>
        private void InitializeCommands()
        {
           	// Wire up menus and toolbar buttons
            #region View Setup Commands

            commandResetView = new Command(new Command.Action(paneModelView.SceneRender.ResetView));
            ToolStripButtonCommander.Connect(tsbResetView, commandResetView);
            ToolStripMenuItemCommander.Connect(miResetView, commandResetView);

            commandXYView = new Command ( new Command.Action ( paneModelView.SceneRender.SetXYView ) );
            ToolStripButtonCommander.Connect(tsbXYView, commandXYView);
            ToolStripMenuItemCommander.Connect(miXYView, commandXYView);

            commandYZView = new Command ( new Command.Action ( paneModelView.SceneRender.SetYZView ) );
            ToolStripButtonCommander.Connect(tsbYZView, commandYZView);
            ToolStripMenuItemCommander.Connect(miYZView, commandYZView);

            commandZXView = new Command ( new Command.Action ( paneModelView.SceneRender.SetZXView ) );
            ToolStripButtonCommander.Connect(tsbZXView, commandZXView);
            ToolStripMenuItemCommander.Connect(miZXView, commandZXView);

            commandFitView = new Command ( new Command.Action ( paneModelView.SceneRender.SetFitView ) );
            ToolStripButtonCommander.Connect(tsbFitView, commandFitView);
            ToolStripMenuItemCommander.Connect(miFitView, commandFitView);

            commandDomainView = new Command(new Command.Action(ViewDomain));
            ToolStripButtonCommander.Connect(tsbViewDomain, commandDomainView);

            #endregion

            #region File Operations Commands

            commandFileOpen = new Command(new Command.Action(this.OpenExistedProject));
            ToolStripButtonCommander.Connect(tsbFileOpen, commandFileOpen);
            ToolStripMenuItemCommander.Connect(miFileOpen, commandFileOpen);

            commandFileSave = new Command(new Command.Action(this.SaveProject));
            ToolStripButtonCommander.Connect(tsbFileSave, commandFileSave);
            ToolStripMenuItemCommander.Connect(miFileSave, commandFileSave);

            commandFileSaveAs = new Command(new Command.Action(this.SaveProjectAs));
            ToolStripMenuItemCommander.Connect(miFileSaveAs, commandFileSaveAs);

            commandFileNew = new Command(new Command.Action(this.CreateNewProject));
            ToolStripButtonCommander.Connect(tsbFileNew, commandFileNew);
            ToolStripMenuItemCommander.Connect(miFileNew, commandFileNew);

            commandExit = new Command(new Command.Action(this.Exit));
            ToolStripMenuItemCommander.Connect(miExit, commandExit);

            commandSaveScreenshot = new Command ( new Command.Action ( this.SaveScreenshot ) );
            ToolStripMenuItemCommander.Connect ( miSaveScreenshot , commandSaveScreenshot );

            #endregion

            #region Single Edit           
            
            commandCut = new Command ( new Command.Action ( this.CutSingle ) );
            ToolStripButtonCommander.Connect ( tsbCut , commandCut );
            ToolStripMenuItemCommander.Connect ( miCut , commandCut );

            commandCopy = new Command ( new Command.Action ( this.CopySingle ) );
            ToolStripButtonCommander.Connect ( tsbCopy , commandCopy );
            ToolStripMenuItemCommander.Connect ( miCopy , commandCopy );

            commandPaste = new Command ( new Command.Action ( this.PasteSingle ) );
            ToolStripButtonCommander.Connect ( tsbPaste , commandPaste );
            ToolStripMenuItemCommander.Connect ( miPaste, commandPaste );

            #endregion

            #region Simulation Settings

            commandSource = new Command(new Command.Action(this.SetupExcitationSource));
            ToolStripButtonCommander.Connect(tsbSource, commandSource);
            ToolStripMenuItemCommander.Connect(miSource, commandSource);

            commandDomain = new Command(new Command.Action(this.SetupDomain));
            ToolStripButtonCommander.Connect(tsbDomain, commandDomain);
            ToolStripMenuItemCommander.Connect(miDomain, commandDomain);

            commandGeneralOptions = new Command(new Command.Action(this.SetupGeneralOptions));
            ToolStripMenuItemCommander.Connect(miGeneralOptions, commandGeneralOptions);

            commandMesh = new Command(new Command.Action(this.SetupMesh));
            ToolStripButtonCommander.Connect(tsbMeshSize, commandMesh);
            ToolStripMenuItemCommander.Connect(miMeshSize, commandMesh);

            commandParallel = new Command(new Command.Action(this.SetupParallelInfo));
            ToolStripButtonCommander.Connect(tsbParallelInfo, commandParallel);
            ToolStripMenuItemCommander.Connect(miPrallelInfo, commandParallel);

            commandHuygensBox = new Command(new Command.Action(this.SetupHuygensBox));
            ToolStripButtonCommander.Connect(tsbHuygensBox, commandHuygensBox);

            commandPreCalculate = new Command(new Command.Action(this.Precalculate));
            ToolStripButtonCommander.Connect(tsbPrecalculate, commandPreCalculate);

           #endregion

            #region Single EO Settings
            commandVoltageExcitation = new Command(new Command.Action(this.SetupVoltageExcitation));
            ToolStripButtonCommander.Connect(tsbVoltageSource, commandVoltageExcitation);

            commandVoltageOutput = new Command(new Command.Action(this.SetupVoltageOutput));
            ToolStripButtonCommander.Connect(tsbVoltageOutput, commandVoltageOutput);

            commandCurrentExcitation = new Command(new Command.Action(this.SetupCurrentExcitation));
            ToolStripButtonCommander.Connect(tsbCurrentSource, commandCurrentExcitation);

            commandCurrentOutput = new Command(new Command.Action(this.SetupCurrentOutput));
            ToolStripButtonCommander.Connect(tsbCurrentOutput, commandCurrentOutput);

            commandPointOutput = new Command(new Command.Action(this.SetupPointOutput));
            ToolStripButtonCommander.Connect(tsbPointOutput, commandPointOutput);

            commandRemoveExcitation = new Command(new Command.Action(this.RemoveEO));
            ToolStripButtonCommander.Connect(tsbRemoveExcitation, commandRemoveExcitation);

            #endregion

            #region Modeling Settings
            commandSelect = new Command(new Command.Action(this.SelectTool));
            ToolStripButtonCommander.Connect(tsbSelect, commandSelect);

            commandCreateCubiod = new Command(new Command.Action(this.CreateCubiod));
            ToolStripButtonCommander.Connect(tsbCube, commandCreateCubiod);

            commandCreateCone = new Command(new Command.Action(this.CreateCone));
            ToolStripButtonCommander.Connect(tsbCone, commandCreateCone);

            commandCreatePoint = new Command(new Command.Action(this.CreatePoint));
            ToolStripButtonCommander.Connect(tsbPoint, commandCreatePoint);

            commandCreateLine = new Command(new Command.Action(this.CreateLine));
            ToolStripButtonCommander.Connect(tsbLine, commandCreateLine);

            commandCreateRectangle = new Command(new Command.Action(this.CreateRectangle));
            ToolStripButtonCommander.Connect(tsbRectangle, commandCreateRectangle);

            commandCreateSphere = new Command(new Command.Action(this.CreateSphere));
            ToolStripButtonCommander.Connect(tsbSphere, commandCreateSphere);

            commandCreateCylinder = new Command(new Command.Action(this.CreateCylinder));
            ToolStripButtonCommander.Connect(tsbCylinder, commandCreateCylinder);

            commandCreateRound = new Command(new Command.Action(this.CreateRound));
            ToolStripButtonCommander.Connect(tsbCircle, commandCreateRound);


            #endregion


            #region Helper

            commandAbout = new Command(new Command.Action(this.ShowAbout));
            ToolStripMenuItemCommander.Connect(miAbout, commandAbout);

            commandHelperContent = new Command ( new Command.Action ( this.ShowHelperContent ) );
            ToolStripMenuItemCommander.Connect ( miHelpContent , commandHelperContent );

            #endregion
        }

        #region File I/O Methods

        /// <summary>
        /// Open a existed project with specified file name
        /// </summary>
        private void OpenExistedProject()
        {
            SaveCurrentProject();

            // Displays an OpenFileDialog so the user can select a gpj file.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "OpenGEMS Project(*.gpj)|*.gpj";
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Select an OpenGEMS Project File";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .GPJ file was selected, open it.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Create new project
                m_project = new GEMSProject();

                BindProjectWithWiews();                
                    
                //Load the project information from selected file
                m_project.Load(openFileDialog1.FileName);

                this.Text = "OpenGEMS Designer - " + m_project.CurrentFileName;
                //Update the RecentFile MenuItem
               // this.UpdateRecentFilesMenuItem(openFileDialog1.FileName);
            }
        } 
     
       
        /// <summary>
        /// Create a new project
        /// </summary>
        private void CreateNewProject()
        {
            SaveCurrentProject();

            //Create new project
            m_project = new GEMSProject();

            BindProjectWithWiews();

            //Load the project information from selected file
            m_project.Initialize();

            this.Text = "OpenGEMS Designer - " + m_project.CurrentFileName;

        }


        private void OpenRecentProject()
        {
            SaveCurrentProject();

            //Create new project
            m_project = new GEMSProject();

            BindProjectWithWiews();

        }

        /// <summary>
        /// Save the project file
        /// </summary>
        private void SaveProject()
        {
            if (m_project == null)
                return;

            if (m_project.CurrentFileName == string.Empty)
            {
                //This is a new project , so let user select a filename
                SaveProjectAs();
            }
            else
                m_project.Save();

            this.Text = "OpenGEMS Designer - " + m_project.CurrentFileName;
        }

        /// <summary>
        /// Save the project file to a new file
        /// </summary>
        private void SaveProjectAs()
        {
            if (m_project == null)
                return;

            //Displays an SaveFileDialog,so user can select where to save
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "OpenGEMS Project(*.gpj)|*.gpj";
            saveFileDialog1.Title = "Save File As";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Save the project to the selected file
                m_project.PreCalculationFileName = Path.ChangeExtension ( saveFileDialog1.FileName , "pcf" );
                m_project.Save(saveFileDialog1.FileName);

                this.Text = "OpenGEMS Designer - " + m_project.CurrentFileName;
            }
        }

        /// <summary>
        /// Check the project whether updated, and ask user whether to save
        /// </summary>
        private void SaveCurrentProject()
        {
            if (m_project != null)
            {
                if (m_project.IsUpdated)
                {
                    //Ask user whether to save the file
                    if (MessageBox.Show("Current project has been modified , do you want to save it?",
                        "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        SaveProject();
                    }
                }
            }
        }

        /// <summary>
        ///Bind the project with views which need to display the informations of it
        /// </summary>
        private void BindProjectWithWiews()
        {
            this.paneObjects.Project = m_project;
            this.paneProperties.Project = m_project;
            this.paneModelView.Project = m_project;
            this.gobalModelPane.Project = m_project;
        }

        /// <summary>
        ///Save the current screenshot of modelspace
        /// </summary>
        private void SaveScreenshot ( )
        {
            //Displays an SaveFileDialog,so user can select where to save
            SaveFileDialog saveFileDialog1 = new SaveFileDialog ( );
            saveFileDialog1.Filter = "jpg(*.jpg)|*.jpg";
            saveFileDialog1.Title = "Save Screenshot As";

            if (saveFileDialog1.ShowDialog ( ) == DialogResult.OK)
            {
                this.ModelWorkSpace.SaveScreenshot ( saveFileDialog1.FileName );
            }
        }  

        public void Exit()
        {

            this.Close();
        }

        #endregion

        #region Single Edit Methods

        private void CutSingle ( )
        {
            if (m_project != null)
                m_project.CutSingle ( );
        }

        private void CopySingle ( )
        {
            if (m_project != null)
                m_project.CopySingle ( );
        }

        private void PasteSingle ( )
        {
            if (m_project != null)
                m_project.PasteNewSingle ( );
        }

        #endregion

        #region Simulation Relation Methods

        private void SetupExcitationSource()
        {
            if (m_project != null)
            {
                ExcitationSourceForm form = new ExcitationSourceForm ( m_project.ExcitationSource );
                form.ShowDialog ( );
            }
        }

        /// <summary>
        ///Setup the computational domain of the project,
        ///but users can  setup it only after they have completing setuping the exication source.
        /// </summary>
        private void SetupDomain()
        {
            if(m_project == null)
                return;

            if (m_project.CanSetupComputationalDomain())
            {
                ComputationalDomainForm form = new ComputationalDomainForm(m_project.ComputationalDomain, this.paneModelView.SceneRender);
                form.ShowDialog();
            }
            else
            {
                SetupExcitationSource();

                if (m_project.CanSetupComputationalDomain())
                {
                    ComputationalDomainForm form = new ComputationalDomainForm(m_project.ComputationalDomain, this.paneModelView.SceneRender);
                    form.ShowDialog();
                }
            }
            
        }

        private void SetupGeneralOptions()
        {
            GeneralOptionsForm form = new GeneralOptionsForm();

            form.ShowDialog();            
        }

        private void SetupMesh()
        {
            if (m_project.CanSetupMesh())
            {
                MeshSizeForm form = new MeshSizeForm(m_project.Mesh);
                form.ShowDialog();
            }
            else
                MessageBox.Show("Simulation area should not be zero!");

        }

        private void SetupParallelInfo()
        {
            if (m_project.CanSetupParallel())
            {
                ParallelInfoForm form = new ParallelInfoForm(m_project);
                form.ShowDialog();

                this.paneModelView.Refresh ( );
            }
            else {
                MessageBox.Show("Mesh size should not be zero!");
            }
        }

        private void SetupHuygensBox()
        {
            if (m_project.CanSetupHuygensBox())
            {
                HuygenBoxForm form = new HuygenBoxForm(m_project);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Mesh size should not be zero!");
            }
        }

        private void Precalculate()
        {
           // paneModelView.SceneRender.RenderMeshPoint ( m_project.GetMeshPoint ( ) );
           // return;

            if (m_project.IsUpdated)
            {
                if (MessageBox.Show ( "Your project has been updated , and the project file'll be saved before pre calucualte!","Warning",MessageBoxButtons.OKCancel ) == DialogResult.OK)
                {
                    SaveProject ( );
                }
                else
                    return;
            }

            if (m_project.CanPreCalculate())
            {
                PreCalculateForm form = new PreCalculateForm(m_project);
                form.ShowDialog();


            }
            else
            {
                MessageBox.Show("Mesh size should not be zero!");
            }
        }

        private void ViewDomain()
        {
            if (m_project != null)
            {
                if (!this.tsbViewDomain.Checked)
                {
                    this.tsbViewDomain.Checked = true;
                    this.paneModelView.SceneRender.DisplayDomainRender();
                }
                else
                {
                    this.tsbViewDomain.Checked = false;
                    this.paneModelView.SceneRender.UnDisplayDomainRender();
                }
            }  
            
        }

        #endregion 

        #region  Single EO Methods

        private void SetupVoltageExcitation()
        {
            if(m_project != null)
            {
                if(m_project.CurrentSelectedObject is GEMSSingle || m_project.CurrentSelectedObject is GeometryOperation)
                {
                    GEMSSingle targetSingle = null;
                    if (m_project.CurrentSelectedObject is GEMSSingle)
                        targetSingle = (GEMSSingle)m_project.CurrentSelectedObject;
                    else
                        targetSingle = ((GeometryOperation)m_project.CurrentSelectedObject).Parent;

                    if(targetSingle.CreateOperation is CreateLine)
                    {
                        VoltageExcitation ve = null;
                        if (targetSingle.CurrentEO is VoltageExcitation)
                            ve = targetSingle.CurrentEO as VoltageExcitation;
                        else
                            ve = new VoltageExcitation(targetSingle);

                        ExcitationOutputForm form = new ExcitationOutputForm(ve);
                        if(form.ShowDialog() == DialogResult.OK)
                        {
                            targetSingle.CurrentEO = ve;
                            targetSingle.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                        }
                    }
                }
            }
        }

        private void SetupVoltageOutput()
        {
            if (m_project != null)
            {
                if (m_project.CurrentSelectedObject is GEMSSingle || m_project.CurrentSelectedObject is GeometryOperation)
                {
                    GEMSSingle targetSingle = null;
                    if (m_project.CurrentSelectedObject is GEMSSingle)
                        targetSingle = (GEMSSingle)m_project.CurrentSelectedObject;
                    else
                        targetSingle = ((GeometryOperation)m_project.CurrentSelectedObject).Parent;

                    if (targetSingle.CreateOperation is CreateLine)
                    {
                        VoltageOutput vo = null;
                        if (targetSingle.CurrentEO is VoltageOutput)
                            vo = targetSingle.CurrentEO as VoltageOutput;
                        else
                            vo = new VoltageOutput(targetSingle);

                        ExcitationOutputForm form = new ExcitationOutputForm(vo);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            targetSingle.CurrentEO = vo;                            
                            targetSingle.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                        }
                    }
                }
            }
        }

        private void SetupCurrentExcitation()
        {
            if (m_project != null)
            {
                if (m_project.CurrentSelectedObject is GEMSSingle || m_project.CurrentSelectedObject is GeometryOperation)
                {
                    GEMSSingle targetSingle = null;
                    if (m_project.CurrentSelectedObject is GEMSSingle)
                        targetSingle = (GEMSSingle)m_project.CurrentSelectedObject;
                    else
                        targetSingle = ((GeometryOperation)m_project.CurrentSelectedObject).Parent;

                    if (targetSingle.CreateOperation is CreateRectangle || targetSingle.CreateOperation is CreateRound)
                    {
                        CurrentExcitation ce = null;
                        if (targetSingle.CurrentEO is CurrentExcitation)
                            ce = targetSingle.CurrentEO as CurrentExcitation;
                        else
                            ce = new CurrentExcitation(targetSingle);

                        ExcitationOutputForm form = new ExcitationOutputForm(ce);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            targetSingle.CurrentEO = ce;
                            targetSingle.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                        }
                    }
                }
            }
        }

        private void SetupCurrentOutput()
        {
            if (m_project != null)
            {
                if (m_project.CurrentSelectedObject is GEMSSingle || m_project.CurrentSelectedObject is GeometryOperation)
                {
                    GEMSSingle targetSingle = null;
                    if (m_project.CurrentSelectedObject is GEMSSingle)
                        targetSingle = (GEMSSingle)m_project.CurrentSelectedObject;
                    else
                        targetSingle = ((GeometryOperation)m_project.CurrentSelectedObject).Parent;

                    if (targetSingle.CreateOperation is CreateRectangle || targetSingle.CreateOperation is CreateRound)
                    {
                        CurrentOutput co = null;
                        if (targetSingle.CurrentEO is CurrentOutput)
                            co = targetSingle.CurrentEO as CurrentOutput;
                        else
                            co = new CurrentOutput(targetSingle);

                        ExcitationOutputForm form = new ExcitationOutputForm(co);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            targetSingle.CurrentEO = co;
                            targetSingle.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                        }
                    }
                }
            }
        }

        private void SetupPointOutput()
        {
            if (m_project != null)
            {
                if (m_project.CurrentSelectedObject is GEMSSingle || m_project.CurrentSelectedObject is GeometryOperation)
                {
                    GEMSSingle targetSingle = null;
                    if (m_project.CurrentSelectedObject is GEMSSingle)
                        targetSingle = (GEMSSingle)m_project.CurrentSelectedObject;
                    else
                        targetSingle = ((GeometryOperation)m_project.CurrentSelectedObject).Parent;

                    if (targetSingle.CreateOperation is CreatePoint)
                    {
                        PointOutput po = null;                        
                        if (targetSingle.CurrentEO is PointOutput)
                        {
                            po = targetSingle.CurrentEO as PointOutput;
                        }
                        else
                        {
                            po = new PointOutput(targetSingle);
                        }

                        FieldOutputForm form = new FieldOutputForm(po);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            targetSingle.CurrentEO = po;
                            targetSingle.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                        }
                    }
                }
            }
        }

        private void RemoveEO()
        {
            if (m_project != null)
            {
                if (m_project.CurrentSelectedObject is GEMSSingle 
                    || m_project.CurrentSelectedObject is GeometryOperation)
                {
                    GEMSSingle targetSingle = null;
                    if (m_project.CurrentSelectedObject is GEMSSingle)
                        targetSingle = (GEMSSingle)m_project.CurrentSelectedObject;
                    else
                        targetSingle = ((GeometryOperation)m_project.CurrentSelectedObject).Parent;

                    targetSingle.CurrentEO = null;
                    targetSingle.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                }
            }
        }      

        #endregion        

        #region Modeling Methods

        public void SelectTool()
        {
            CreateModelingTool(typeof(SelectTool));            
        }

        private void CreateCubiod()
        {
            CreateModelingTool(typeof(CuboidTool));
        }       

        private void CreateCone()
        {
            CreateModelingTool(typeof(ConeTool));
        }

        private void CreatePoint()
        {
            CreateModelingTool(typeof(PointTool));
        }

        private void CreateLine()
        {
            CreateModelingTool(typeof(LineTool));
        }

        private void CreateRectangle()
        {
            CreateModelingTool(typeof(RectangleTool));
        }

        private void CreateSphere()
        {
            CreateModelingTool(typeof(SphereTool));
        }

        private void CreateCylinder()
        {
            CreateModelingTool(typeof(CylinderTool));
        }

        private void CreateRound()
        {
            CreateModelingTool(typeof(RoundTool));
        }

        private void CreateModelingTool(Type toolType)
        {
            if (lastActiveTool != null)
            {
                if (lastActiveTool.GetType() == toolType)
                {
                    lastActiveTool.Deactivate();
                    lastActiveTool = new DefaultTool(this);
                    lastActiveTool.Activate();
                    return;
                }
                else
                {
                    lastActiveTool.Deactivate();
                    lastActiveTool = null;
                }
            }

            Type[] types = new Type[1];
            types[0] = typeof(GEMS.Designer.MainForm);
            ConstructorInfo constructorInfoObj = toolType.GetConstructor(types);
            object[] paras = new object[1];
            paras[0] = this;
            lastActiveTool = (Tool)constructorInfoObj.Invoke(paras);
            lastActiveTool.Activate();
        }


        #endregion

        #region Public properties

        public ModelPane ModelWorkSpace
        {
            get { return this.paneModelView; }
        }

        public GEMSProject currentProject
        {
            get { return this.m_project; }
        }

        public bool EnableSelectedTool
        {
            set { this.tsbSelect.Checked = value; }
        }

        public bool EnableCreateCuboidTool
        {
            set { this.tsbCube.Checked = value; }
        }

        public bool EnableCreateRoundTool
        {
            set { this.tsbCircle.Checked = value; }
        }

        public bool EnableCreateRectangleTool
        {
            set { this.tsbRectangle.Checked = value; }
        }

        public bool EnableCreateCylinderTool
        {
            set { this.tsbCylinder.Checked = value; }
        }

        public bool EnableCreateSphereTool
        {
            set { this.tsbSphere.Checked = value; }
        }

        public bool EnableCreateConeTool
        {
            set { this.tsbCone.Checked = value; }
        }

        public bool EnableCreateLineTool
        {
            set { this.tsbLine.Checked = value; }
        }

        public bool EnableCreatePointTool
        {
            set { this.tsbPoint.Checked = value; }
        }
        #endregion 

        
        #region Helper

        private void ShowAbout ()
        {
            new AboutBox ( ).ShowDialog ( );
        }

        private void ShowHelperContent ( )
        {
 
        }

        #endregion

    }
}