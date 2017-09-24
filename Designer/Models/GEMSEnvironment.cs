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
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.ComponentModel;

namespace GEMS.Designer.Models
{
    public class GEMSEnvironment
    {
        //Environment members
        private Length gridSize = null;
        private float gridOffset = 0.0f;
        private GridPlane gridPlane = GridPlane.XY;
        private bool gridDisplayed = false;

        //Default length unit of all the geometries and grid
        //Will be recorded in the config fill
        private Length.LengthUnit defaultLengthUnit = Length.LengthUnit.mm;

        #region Delegate and Event

        public delegate void GridOptionChangedEventHandler(object sender, EventArgs e);
        public virtual event GridOptionChangedEventHandler GridOptionChanged;

        public void GridOptionChangedAlarm()
        {
            if (this.GridOptionChanged != null)
            {
                this.GridOptionChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        /// <summary>
        /// Create a new GEMSEnvironment object and load the information from the specified file
        /// </summary>
        public GEMSEnvironment(XPathNavigator navigator)
        {
            //Read the Unit information
            navigator.MoveToChild("Units", string.Empty);
            navigator.MoveToFirstChild();
            defaultLengthUnit = (Length.LengthUnit)Enum.Parse(typeof(Length.LengthUnit), navigator.GetAttribute("value", string.Empty));
            navigator.MoveToParent();
            navigator.MoveToParent();

            //Read the grid information
            navigator.MoveToChild("Grid", "");
            gridSize = new Length(navigator.GetAttribute("size", ""),navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();
       }

        /// <summary>
        /// Create a new GEMSEnvironment object with default values
        /// </summary>
        public GEMSEnvironment()
        {
            try
            {
                //From the application settings to reader the current default length unit
                this.defaultLengthUnit = (Length.LengthUnit)GEMS.Designer.Properties.Settings.Default.DefaultLengthUnit;
            }
            catch{
                this.defaultLengthUnit = Length.LengthUnit.mm;

            }

            this.gridSize = new Length(0.2f, this.defaultLengthUnit);

         }

        /// <summary>
        /// Build a xml string containing the information of this oject
        /// </summary>
        /// <param name="navigator"></param>
        public string BuildOuterXmlString()
        {
            StringBuilder environmentNodeBuilder = new StringBuilder();

            environmentNodeBuilder.Append("<Environment>");
            environmentNodeBuilder.Append("<Units>");
            environmentNodeBuilder.AppendFormat("<Unit value=\"{0}\" name=\"length\" />", (int)this.defaultLengthUnit);
            environmentNodeBuilder.Append("</Units>");
            environmentNodeBuilder.AppendFormat("<Grid size=\"{0}\" unit=\"{1}\" />", this.gridSize.Value,(int)this.gridSize.Unit);
            environmentNodeBuilder.Append("</Environment>");

            return environmentNodeBuilder.ToString();

        }

        [CategoryAttribute("Grid Options"),DisplayNameAttribute("Grid Size") ]
        public Length GridSize
        {
            get { return gridSize; }
            set { gridSize = value; }
        }

        [CategoryAttribute("Grid Options"), DisplayNameAttribute("Grid Offset")]
        public float GidOffset
        {
            get { return gridOffset; }
            set { gridOffset = value; }
        }

        [CategoryAttribute("Grid Options"), DisplayNameAttribute("Grid Plane")]
        public GridPlane GridPlane
        {
            get { return gridPlane; }
            set { gridPlane = value; }
        }

        [CategoryAttribute("Grid Options"), DisplayNameAttribute("Grid Display")]
        public bool GridDisplayed
        {
            get { return gridDisplayed; }
            set { gridDisplayed = value; }
        }

        [CategoryAttribute("Default Options"), 
        DisplayNameAttribute("Length Unit"),
        ReadOnlyAttribute(true),
        DescriptionAttribute("Default length unit of new created object")]
        public Length.LengthUnit DefaultLengthUnit
        {
            get { return defaultLengthUnit; }
            set { defaultLengthUnit = value; }
        }
        

    }
}
