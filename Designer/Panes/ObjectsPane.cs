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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using GEMS.Designer.Models;
using GEMS.Designer;
using GEMS.Designer.Models.GeometryOperations;

namespace GEMS.Designer.Panes
{
    public partial class ObjectsPane : UserControl, IObserver
    {
        TreeNode root;
        TreeNode singleNodes;
        TreeNode selectedNode;

        GEMSProject m_project = null;

        public ObjectsPane()
        {
            InitializeComponent();
        }

        public GEMSProject Project
        {
            set
            {
                m_project = value;
                m_project.DataChanged += new GEMSProject.DataChangedEventHandler(OnGEMSProjectDataChanged);
                m_project.CurrentObjectChanged += new GEMSProject.CurrentObjectChangedEventHandler(OnGEMSProject_CurrentObjectChanged);
            }
        }



        private void ObjectsPane_Load(object sender, EventArgs e)
        {
            //Initialize the root of the tree
            root = new TreeNode("Project", 1, 1);
            tvObjects.Nodes.Add(root);
            tvObjects.SelectedNode = root;

            //Add the two main nodes of the root
            singleNodes = new TreeNode("Shapes", 1, 1);
            root.Nodes.Add(singleNodes);
            root.Expand();

            singleNodes.Expand();
        }


        void OnGEMSProject_CurrentObjectChanged(object sender, EventArgs e)
        {
            if (sender is GEMSProject)
            {
                if (tvObjects.SelectedNode.Tag != m_project.CurrentSelectedObject)
                {
                    if (m_project.CurrentSelectedObject == null)
                    {
                        tvObjects.SelectedNode = root;
                        return;
                    }

                    //Find the node and selected it
                    if (m_project.CurrentSelectedObject is GEMSSingle)
                    {
                        foreach (TreeNode node in singleNodes.Nodes)
                        {
                            if (node.Tag == m_project.CurrentSelectedObject)
                            {
                                tvObjects.SelectedNode = node;
                                singleNodes.Expand();
                                node.Expand();
                                break;
                            }
                        }
                    }

                    if (m_project.CurrentSelectedObject is GeometryOperation)
                    {
                        foreach (TreeNode singleNode in singleNodes.Nodes)
                        {
                            if (singleNode.Tag == ((GeometryOperation)m_project.CurrentSelectedObject).Parent)
                            {
                                singleNodes.Expand();
                                singleNode.Expand();
                                foreach (TreeNode operationNode in singleNode.Nodes)
                                {
                                    if (operationNode.Tag == m_project.CurrentSelectedObject)
                                    {
                                        tvObjects.SelectedNode = operationNode;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Update the tree view controls based the new information of project
        /// </summary>
        void OnGEMSProjectDataChanged(object sender, GEMSProject.DataChangedEventArgs e)
        {
            if (sender is GEMSProject)
            {
                switch (e.changedType)
                {
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SingleEdited:
                        {
                            if ((tvObjects.SelectedNode.Tag is GEMSSingle) && (tvObjects.SelectedNode.Tag == e.changedSingle))
                            {                            
                               tvObjects.SelectedNode.Text = e.changedSingle.Name;
                               break;
                            }
                        }
                        break;
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SingleCutted:
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SingleDeleted:
                        {
                            //Find the corresponding tree node
                            TreeNode removedNode = null;
                            int index = 0;
                            foreach (TreeNode node in singleNodes.Nodes)
                            {
                                if (node.Tag == e.changedSingle)
                                {
                                    removedNode = node;
                                    break;
                                }
                                index++;
                            }

                            if (removedNode != null)
                            {
                                singleNodes.Nodes.Remove(removedNode);

                                if (index == singleNodes.Nodes.Count && index > 0)
                                {
                                    index = singleNodes.Nodes.Count - 1;
                                    tvObjects.SelectedNode = singleNodes.Nodes[index];
                                }
                                else
                                    tvObjects.SelectedNode = root;

                            }
                            break;
                        }
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SinglePasted:
                    case GEMSProject.DataChangedEventArgs.DataChangeType.SingleCreated:
                        {
                            if (e != null && e.changedSingle != null)
                            {
                                //Create a new node
                                TreeNode newSingleNode = new TreeNode(e.changedSingle.Name);
                                newSingleNode.Tag = e.changedSingle;

                                foreach (GeometryOperation operation in e.changedSingle.Operations)
                                {
                                    TreeNode operationNode = new TreeNode(operation.Name.ToString());
                                    operationNode.Tag = operation;

                                    newSingleNode.Nodes.Add(operationNode);
                                }

                                //Add the single node at the tree tail
                                this.singleNodes.Nodes.Add(newSingleNode);

                                //Select and expand it
                                newSingleNode.Expand();                                
                                tvObjects.SelectedNode = newSingleNode;
                            }
                            break;
                        }
                    case GEMSProject.DataChangedEventArgs.DataChangeType.Initialized:
                        {
                            //Clear all the old nodes
                            singleNodes.Nodes.Clear();

                            root.Tag = m_project.Environment;

                            //Add the single nodes to the tree
                            foreach (GEMSSingle single in m_project.Singles)
                            {
                                TreeNode singleNode = new TreeNode(single.Name);
                                singleNode.Tag = single;

                                foreach (GeometryOperation operation in single.Operations)
                                {
                                    TreeNode operationNode = new TreeNode(operation.Name.ToString());
                                    operationNode.Tag = operation;

                                    singleNode.Nodes.Add(operationNode);
                                }

                                singleNodes.Nodes.Add(singleNode);
                            }

                            singleNodes.Expand();
                            this.tvObjects.SelectedNode = root;
                            m_project.CurrentSelectedObject = root.Tag;
                        }
                        break;
                }

            }
        }

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (m_project != null)
            {
                m_project.CurrentSelectedObject = tvObjects.SelectedNode.Tag;
            }
        }

        private void tvObjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_project != null)
            {
                m_project.PerformKeyDown ( e );
            }
        }


    }
}
