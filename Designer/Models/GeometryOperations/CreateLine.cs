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
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Controls;
using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryModels;

namespace GEMS.Designer.Models.GeometryOperations
{
    public class CreateLine : GeometryCreateOperation
    {        
        private Vector3WithUnit startPoint = null;
        private Vector3WithUnit endPoint = null;

        public CreateLine(int id)
            : base(id)
        {
 
        }

        public CreateLine(int id, GEMSSingle parent)
            : base(id, parent)
        {
        }

        public CreateLine(System.Xml.XPath.XPathNavigator navigator, GEMSSingle parent)
            : base(navigator, parent)
        {

            navigator.MoveToChild("Positions", string.Empty);
            navigator.MoveToFirstChild();
            
            //Start Point of line
            startPoint = new Vector3WithUnit();
            startPoint.X = new Length(navigator.GetAttribute("x", string.Empty),navigator.GetAttribute("ux", string.Empty));
            startPoint.Y = new Length(navigator.GetAttribute("y", string.Empty),navigator.GetAttribute("uy", string.Empty));
            startPoint.Z = new Length(navigator.GetAttribute("z", string.Empty),navigator.GetAttribute("uz", string.Empty));
           

            //End Point of line
            navigator.MoveToNext();
            endPoint = new Vector3WithUnit();
            endPoint.X = new Length(navigator.GetAttribute("x", string.Empty),navigator.GetAttribute("ux", string.Empty));
            endPoint.Y = new Length(navigator.GetAttribute("y", string.Empty),navigator.GetAttribute("uy", string.Empty));
            endPoint.Z = new Length(navigator.GetAttribute("z", string.Empty),navigator.GetAttribute("uz", string.Empty));

            navigator.MoveToParent();
            navigator.MoveToParent();
        }

        #region Parent Class's Abstarct Methods

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns> 
        /// The example xml string like:
        /// <Operation id="7" name="CreateLine" relativeCS="0" >
        /// <Sources>
        ///    <Parent id="7" />
        ///  </Sources>
        ///  <Targets/>
        ///  <Positions>
        ///    <Position x="-0.6" y="0.6" z="0" ux="3" uy="3" uz="3" />
        ///    <Position x="-0.6" y="1.4" z="0" ux="3" uy="3" uz="3" />
        ///  </Positions>
        ///</Operation>
        /// </returns>
        public override string BuildOuterXmlString()
        {
            StringBuilder lineNodeBuilder = new StringBuilder();

            lineNodeBuilder.AppendFormat("<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\">", this.Id, this.Name);
            lineNodeBuilder.Append("<Sources>");
            lineNodeBuilder.AppendFormat("<Source id=\"{0}\" />", this.Parent.Id);
            lineNodeBuilder.Append("</Sources>");
            lineNodeBuilder.Append("<Targets/>");
            lineNodeBuilder.Append("<Positions>");
            lineNodeBuilder.Append(this.startPoint.BuildInnserXmlString("Position"));
            lineNodeBuilder.Append(this.endPoint.BuildInnserXmlString("Position"));
            lineNodeBuilder.Append("</Positions>");
            lineNodeBuilder.Append("</Operation>");

            return lineNodeBuilder.ToString();
        }

        public override GeometryOperation Clone(int newId, GEMSSingle parent)
        {
            CreateLine line = new CreateLine(newId, parent);

            line.StartPoint = new Vector3WithUnit(this.startPoint);
            line.EndPoint = new Vector3WithUnit(this.endPoint);

            return line;
        }

        public override GeometryModel CreateSourceGeometryModel()
        {
            LineModel line = new LineModel();

            line.Node1 = this.startPoint.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);
            line.Node2 = this.endPoint.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);

            Vector3 minVector3 = Vector3.Empty;
            Vector3 maxVector3 = Vector3.Empty;

            minVector3.X = line.Node1.X < line.Node2.X ? line.Node1.X : line.Node2.X;
            maxVector3.Y = line.Node1.Y < line.Node2.Y ? line.Node1.Y : line.Node2.Y;
            minVector3.Z = line.Node1.Z < line.Node2.Z ? line.Node1.Z : line.Node2.Z;
            maxVector3.X = line.Node1.X < line.Node2.X ? line.Node2.X : line.Node1.X;
            minVector3.Y = line.Node1.Y < line.Node2.Y ? line.Node2.Y : line.Node1.Y;
            maxVector3.Z = line.Node1.Z < line.Node2.Z ? line.Node2.Z : line.Node1.Z;

            line.MinVector3 = minVector3;
            line.MaxVector3 = maxVector3;

            return line;
        }

        #endregion

        #region Properties

        [CategoryAttribute("Geometry"), DescriptionAttribute("Start point of the line")]
        public Vector3WithUnit StartPoint
        {
            get { return startPoint; }
            set { startPoint = value; }
        }


        [CategoryAttribute("Geometry"), DescriptionAttribute("End point of the line")]
        public Vector3WithUnit EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; }
        }

        #endregion
    }
}
