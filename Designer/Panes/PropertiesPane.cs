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

using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;

namespace GEMS.Designer.Panes
{
    public partial class PropertiesPane : UserControl, IObserver
    {
        GEMSProject m_project = null;
        object lastSelectedObject = null;

        public PropertiesPane()
        {
            InitializeComponent();           
            
        }

        // IObserver.Proejct
        public GEMSProject Project
        {
            set
            {
                m_project = (GEMSProject)value;

                m_project.CurrentObjectChanged += new GEMSProject.CurrentObjectChangedEventHandler(PositionChanged);
            }
        }

        private void PositionChanged(object sender, EventArgs e)
        {
            if (m_project.CurrentSelectedObject != null)
            {
                propertyGrid.SelectedObject = m_project.CurrentSelectedObject;

                if (this.m_project.CurrentSelectedObject is GEMSSingle)
                {
                    //Subscribe datachanged event of this single
                    GEMSSingle single = this.m_project.CurrentSelectedObject as GEMSSingle;
                    single.GEMSSingle_DataChanged += new GEMSSingle.GEMSSingle_DataChangedEventHandler ( OnGEMSSingleDataChanged );
                }
            }
            else
            {
                m_project.CurrentSelectedObject = m_project.Environment;
            }
            this.lastSelectedObject = propertyGrid.SelectedObject;

        }

        private void OnGEMSSingleDataChanged(object sender, GEMSSingle.SingleDataChangedEventArgs e)
        {
            //rebinding the data
            propertyGrid.SelectedObject = m_project.CurrentSelectedObject;
        }


        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if(e.ChangedItem.PropertyDescriptor.ComponentType == typeof(Length))
            {
                this.propertyGrid.Refresh();                
            }

            if (m_project.CurrentSelectedObject is GeometryOperation)
            {
                GEMSSingle changedSingle = ((GeometryOperation)m_project.CurrentSelectedObject).Parent;

                changedSingle.SingleDataChangedAlarm ( GEMSSingle.SingleDataChangedEventArgs.DataChangeType.GeometryChanged );

                changedSingle.Parent.DataChangedAlarm ( GEMSProject.DataChangedEventArgs.DataChangeType.SingleEdited , changedSingle );
            }

            if (m_project.CurrentSelectedObject is GEMSSingle)
            {
                GEMSSingle changedSingle = ((GEMSSingle)m_project.CurrentSelectedObject);
                changedSingle.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.DisplayStyleChanged);

                //Maybe change the name of the single 
                changedSingle.Parent.DataChangedAlarm(GEMSProject.DataChangedEventArgs.DataChangeType.SingleEdited, changedSingle);
            }

            if (m_project.CurrentSelectedObject is GEMSEnvironment)
            {
                ((GEMSEnvironment)m_project.CurrentSelectedObject).GridOptionChangedAlarm();
            }
        }

        private void propertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        {
            if (lastSelectedObject is GEMSSingle)
            {
                //Unscribe the event
                GEMSSingle single = lastSelectedObject as GEMSSingle;
                single.GEMSSingle_DataChanged -= new GEMSSingle.GEMSSingle_DataChangedEventHandler(this.OnGEMSSingleDataChanged);
            }
        }  

    }
}
