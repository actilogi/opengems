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
    public class CreatePoint : GeometryCreateOperation
    {
        private Vector3WithUnit position = null;

        public CreatePoint(int id)
            : base(id)
        {
 
        }

        public CreatePoint(int id, GEMSSingle parent)
            : base(id, parent)
        {
        }

        public CreatePoint(System.Xml.XPath.XPathNavigator navigator, GEMSSingle parent)
            : base(navigator, parent)
        {
            //Position of Point
            navigator.MoveToChild("Position", string.Empty);
            position = new Vector3WithUnit();
            position.X = new Length(navigator.GetAttribute("x", string.Empty), navigator.GetAttribute("ux", string.Empty));
            position.Y = new Length(navigator.GetAttribute("y", string.Empty), navigator.GetAttribute("uy", string.Empty));
            position.Z = new Length(navigator.GetAttribute("z", string.Empty), navigator.GetAttribute("uz", string.Empty));
            navigator.MoveToParent();
        }

       #region Parent Class's Abstarct Methods

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns> 
        /// The example xml string like:
        ///<Operation id="5" name="CreatePoint" relativeCS="0" >
        ///  <Sources>
        ///    <Parent id="5" />
        ///  </Sources>
        /// <Targets/>
        ///  <Position x="1" y="-1" z="0" ux="3" uy="3" uz="3" />
        ///</Operation>
        /// </returns>
        public override string BuildOuterXmlString()
        {
            StringBuilder pointNodeBuilder = new StringBuilder();

            pointNodeBuilder.AppendFormat("<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\" >", this.Id, this.Name);
            pointNodeBuilder.Append("<Sources>");
            pointNodeBuilder.AppendFormat("<Source id=\"{0}\" />", this.Parent.Id);
            pointNodeBuilder.Append("</Sources>");
            pointNodeBuilder.Append("<Targets/>");
            pointNodeBuilder.Append(this.position.BuildInnserXmlString("Position"));
            pointNodeBuilder.Append("</Operation>");

            return pointNodeBuilder.ToString();
        }

        public override GeometryOperation Clone(int newId, GEMSSingle parent)
        {
            CreatePoint point = new CreatePoint(newId, parent);

            point.Position = new Vector3WithUnit(this.position);

            return point;
        }

        public override GeometryModel CreateSourceGeometryModel()
        {
            PointModel point = new PointModel();

            point.Position = this.position.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);

            //Get the bounding box
            point.MinVector3 = point.Position;
            point.MaxVector3 = point.Position;

            point.MinVector3.TransformCoordinate(point.WorldMatrix);
            point.MaxVector3.TransformCoordinate(point.WorldMatrix);

            return point;
        }

       #endregion

        #region Properties

        [CategoryAttribute("Geometry"), DescriptionAttribute("Position of the point")]
        public Vector3WithUnit Position
        {
            get { return position; }
            set { position = value; }
        }

      
        #endregion
    }
}
