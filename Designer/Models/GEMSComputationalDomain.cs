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

namespace GEMS.Designer.Models
{
    public class GEMSComputationalDomain
    {
        private GEMSProject parent;

        private Vector3WithUnit minVector3;
        private Vector3WithUnit maxVector3;

        private BoundaryCondition conditionXmin = BoundaryCondition.UPML;
        private BoundaryCondition conditionYmin = BoundaryCondition.UPML;
        private BoundaryCondition conditionZmin = BoundaryCondition.UPML;
        private BoundaryCondition conditionXmax = BoundaryCondition.UPML;
        private BoundaryCondition conditionYmax = BoundaryCondition.UPML;
        private BoundaryCondition conditionZmax = BoundaryCondition.UPML;

        #region Delegate and Event

        public delegate void GEMSComputationalDomain_DataChangedEventHandler(object sender, EventArgs e);

        //This event will be raised after the divisions are updated
        public virtual event GEMSComputationalDomain_DataChangedEventHandler GEMSComputationalDomain_DataChanged;

        public void DomainDataChangedAlarm()
        {
            this.parent.IsUpdated = true;

            if (this.GEMSComputationalDomain_DataChanged != null)
            {
                this.GEMSComputationalDomain_DataChanged(this, EventArgs.Empty);
            }
        }

        #endregion
        /// <summary>
        /// Create a new ComputationalDomain object and load the information from the specified file
        /// </summary>
        /// <param name="navigator"></param>
        /// <returns></returns>
        public GEMSComputationalDomain(XPathNavigator navigator,GEMSProject parent)
        {
            this.parent = parent;

            minVector3 = new Vector3WithUnit();
            maxVector3 = new Vector3WithUnit();

            navigator.MoveToChild("X0", string.Empty);
            minVector3.X = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            conditionXmin = (BoundaryCondition)Enum.Parse(typeof(BoundaryCondition), navigator.GetAttribute("condition", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("X1", string.Empty);
            maxVector3.X = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            conditionXmax = (BoundaryCondition)Enum.Parse(typeof(BoundaryCondition), navigator.GetAttribute("condition", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("Y0", string.Empty);
            minVector3.Y = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            conditionYmin = (BoundaryCondition)Enum.Parse(typeof(BoundaryCondition), navigator.GetAttribute("condition", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("Y1", string.Empty);
            maxVector3.Y = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            conditionYmax = (BoundaryCondition)Enum.Parse(typeof(BoundaryCondition), navigator.GetAttribute("condition", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("Z0", string.Empty);
            minVector3.Z = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            conditionZmin = (BoundaryCondition)Enum.Parse(typeof(BoundaryCondition), navigator.GetAttribute("condition", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("Z1", string.Empty);
            maxVector3.Z = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            conditionZmax = (BoundaryCondition)Enum.Parse(typeof(BoundaryCondition), navigator.GetAttribute("condition", string.Empty));
            navigator.MoveToParent();
        }

        /// <summary>
        /// Create a new ComputationalDomain object and set default values
        /// </summary>
        public GEMSComputationalDomain(GEMSProject parent)
        {
            this.parent = parent;

            minVector3 = new Vector3WithUnit(0.0f, parent.Environment.DefaultLengthUnit);
            maxVector3 = new Vector3WithUnit(0.0f, parent.Environment.DefaultLengthUnit);
        }

        /// <summary>
        /// Build a xml string containing the information of the object
        /// </summary>
        /// <returns>
        /// The example xml string is:
        ///<Domain pmlAlpha="0" kvalue="0" pmlLayer="6" pmlType="0" pmlK="1" >
        ///  <X0 unit="3" value="-1.6" distance="10" condition="4" />
        ///  <X1 unit="3" value="1" distance="10" condition="4" />
        ///  <Y0 unit="3" value="-2.6" distance="10" condition="4" />
        ///  <Y1 unit="3" value="1.4" distance="10" condition="4" />
        ///  <Z0 unit="3" value="-0.8" distance="10" condition="4" />
        ///  <Z1 unit="3" value="0.4" distance="10" condition="4" />
        ///</Domain>
        /// </returns>
        public string BuildOuterXmlString()
        {
            StringBuilder domainNodeBuilder = new StringBuilder();

            domainNodeBuilder.Append("<Domain pmlAlpha=\"0\" kvalue=\"1.0\" pmlLayer=\"6\" pmlType=\"0\" pmlK=\"1\" >");

            domainNodeBuilder.AppendFormat("<X0 unit=\"{0}\" value=\"{1}\" distance=\"0\" condition=\"{2}\" />",
                (int)this.minVector3.X.Unit, this.minVector3.X.Value, (int)this.conditionXmin);
            domainNodeBuilder.AppendFormat("<X1 unit=\"{0}\" value=\"{1}\" distance=\"0\" condition=\"{2}\" />",
                (int)this.maxVector3.X.Unit, this.maxVector3.X.Value, (int)this.conditionXmax);

            domainNodeBuilder.AppendFormat("<Y0 unit=\"{0}\" value=\"{1}\" distance=\"0\" condition=\"{2}\" />",
                (int)this.minVector3.Y.Unit, this.minVector3.Y.Value, (int)this.conditionYmin);
            domainNodeBuilder.AppendFormat("<Y1 unit=\"{0}\" value=\"{1}\" distance=\"0\" condition=\"{2}\" />",
                (int)this.maxVector3.Y.Unit, this.maxVector3.Y.Value, (int)this.conditionYmax);

            domainNodeBuilder.AppendFormat("<Z0 unit=\"{0}\" value=\"{1}\" distance=\"0\" condition=\"{2}\" />",
                (int)this.minVector3.Z.Unit, this.minVector3.Z.Value, (int)this.conditionZmin);
            domainNodeBuilder.AppendFormat("<Z1 unit=\"{0}\" value=\"{1}\" distance=\"0\" condition=\"{2}\" />",
                (int)this.maxVector3.Z.Unit, this.maxVector3.Z.Value, (int)this.conditionZmax);

            domainNodeBuilder.Append("</Domain>");

            return domainNodeBuilder.ToString();
        }

        /// <summary>
        /// Ensure whether the simulation area is zero.
        /// The simulation area is not zero only when there're at least two axis ranges which are not zero.
        /// </summary>
        public bool IsSimulationAreaNotZero()
        {
            float rangeX = MaxX - MinX;
            float rangeY = MaxY - MinY;
            float rangeZ = MaxZ - MinZ;

            return (rangeX > 0.0 && rangeY > 0.0)
                || (rangeX > 0.0 && rangeZ > 0.0)
                || (rangeZ > 0.0 && rangeY > 0.0); 
        }

        #region Properties

        public Vector3WithUnit MinVector3
        {
            get { return minVector3; }
        }

        public Vector3WithUnit MaxVector3
        {
            get { return maxVector3; }
        }

        public float MinX
        {
            get { return minVector3.X.ChangeUnit(parent.Environment.DefaultLengthUnit); }
        }

        public float MinY
        {
            get { return minVector3.Y.ChangeUnit(parent.Environment.DefaultLengthUnit); }
        }

        public float MinZ
        {
            get { return minVector3.Z.ChangeUnit(parent.Environment.DefaultLengthUnit); }
        }

        public float MaxX
        {
            get { return maxVector3.X.ChangeUnit(parent.Environment.DefaultLengthUnit); }
        }

        public float MaxY
        {
            get { return maxVector3.Y.ChangeUnit(parent.Environment.DefaultLengthUnit); }
        }

        public float MaxZ
        {
            get { return maxVector3.Z.ChangeUnit(parent.Environment.DefaultLengthUnit); }
        }

        
        public BoundaryCondition ConditionXmin
        {
            get { return conditionXmin; }
            set { conditionXmin = value; }
        }

        public BoundaryCondition ConditionYmin
        {
            get { return conditionYmin; }
            set { conditionYmin = value; }
        }

        public BoundaryCondition ConditionZmin
        {
            get { return conditionZmin; }
            set { conditionZmin = value; }
        }

        public BoundaryCondition ConditionXmax
        {
            get { return conditionXmax; }
            set { conditionXmax = value; }
        }

        public BoundaryCondition ConditionYmax
        {
            get { return conditionYmax; }
            set { conditionYmax = value; }
        }

        public BoundaryCondition ConditionZmax
        {
            get { return conditionZmax; }
            set { conditionZmax = value; }
        }

        public GEMSProject Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        #endregion
    }
}
