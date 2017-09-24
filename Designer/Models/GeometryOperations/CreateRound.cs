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
    public class CreateRound : GeometryCreateOperation
    {
        private Vector3WithUnit center = null;
        private Length radius = null;
        private Axis alineAxis = Axis.X;

        public CreateRound(int id)
            : base(id)
        {
 
        }

        public CreateRound(int id, GEMSSingle parent)
            : base(id, parent)
        {
        }

        public CreateRound(System.Xml.XPath.XPathNavigator navigator, GEMSSingle parent)
            : base(navigator, parent)
        {

            alineAxis = (Axis)Enum.Parse(typeof(Axis), navigator.GetAttribute("axis", string.Empty));

            //Radius of circle
            navigator.MoveToChild("Radius",string.Empty);
            radius = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Center of circle
            navigator.MoveToChild("Center", string.Empty);
            center = new Vector3WithUnit();
            center.X = new Length(navigator.GetAttribute("x", string.Empty), navigator.GetAttribute("ux", string.Empty));
            center.Y = new Length(navigator.GetAttribute("y", string.Empty), navigator.GetAttribute("uy", string.Empty));
            center.Z = new Length(navigator.GetAttribute("z", string.Empty), navigator.GetAttribute("uz", string.Empty));
            navigator.MoveToParent();
        }

       #region Parent Class's Abstarct Methods

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns>
        /// The example xml string like:
        ///<Operation id="9" name="CreateRound" relativeCS="0" axis="2" >
        ///  <Sources>
        ///    <Parent id="9" />
        ///  </Sources>
        ///  <Targets/>
        ///  <Radius unit="3" value="1.0198" />
        ///  <Center x="-2.2" y="-4" z="0" ux="3" uy="3" uz="3" />
        ///</Operation>
        /// </returns>
        public override string BuildOuterXmlString()
        {
            StringBuilder circleNodeBuilder = new StringBuilder();

            circleNodeBuilder.AppendFormat("<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\" axis=\"{2}\">", this.Id, this.Name, (int)this.alineAxis);
            circleNodeBuilder.Append("<Sources>");
            circleNodeBuilder.AppendFormat("<Source id=\"{0}\" />", this.Parent.Id);
            circleNodeBuilder.Append("</Sources>");
            circleNodeBuilder.Append("<Targets/>");
            circleNodeBuilder.Append(this.radius.BuildInnserXmlString("Radius"));
            circleNodeBuilder.Append(this.center.BuildInnserXmlString("Center"));
            circleNodeBuilder.Append("</Operation>");

            return circleNodeBuilder.ToString();
        }

        public override GeometryOperation Clone(int newId, GEMSSingle parent)
        {
            CreateRound circle = new CreateRound(newId, parent);

            circle.AlineAxis = this.alineAxis;
            circle.Radius = new Length(this.radius);
            circle.Center = new Vector3WithUnit(this.center);

            return circle;
        }

        public override GeometryModel CreateSourceGeometryModel()
        {
            RoundModel round = new RoundModel();

            round.Radius =  this.radius.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);

            Vector3 centerVector3 = this.center.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);
            Matrix worldMatrix = Matrix.Identity;

            Vector3 eoStart = Vector3.Empty;
            Vector3 eoEnd = Vector3.Empty;

            switch(this.alineAxis)
            {
                case Axis.X:
                    eoStart = centerVector3 + new Vector3(0, 0, round.Radius);
                    eoEnd = new Vector3(GeometryModel.DefaultEOLineLength, 0, 0);

                    worldMatrix = Matrix.RotationY(-(float)Math.PI / 2.0f)
                             * Matrix.Translation(centerVector3);
                    break;  
                case Axis.Y:
                    eoStart = centerVector3 + new Vector3(0, 0, round.Radius);
                    eoEnd = new Vector3(0, GeometryModel.DefaultEOLineLength, 0);

                    worldMatrix = Matrix.RotationX(-(float)Math.PI / 2.0f)
                             * Matrix.Translation(centerVector3);
                    break;
                case Axis.Z:
                    eoStart = centerVector3 + new Vector3(0, -round.Radius, 0);
                    eoEnd = new Vector3(0, 0, GeometryModel.DefaultEOLineLength);

                    worldMatrix = Matrix.Translation(centerVector3);
                    break;
            }

            round.CenterVector3 = centerVector3;
            round.WorldMatrix = worldMatrix;
            round.EOStart = eoStart;
            round.EOEnd = eoEnd;
            round.AlineAxis = this.alineAxis;

            return round;
        }

       #endregion

        #region Properties

        [CategoryAttribute("Geometry"), DisplayNameAttribute("Axis"), DescriptionAttribute("Axis of the circle")]
        public Axis AlineAxis
        {
            get { return alineAxis; }
            set { alineAxis = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Center of the circle")]
        public Vector3WithUnit Center
        {
            get { return center; }
            set { center = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Radius of the circle")]
        public Length Radius
        {
            get { return radius; }
            set
            {
                if (value.Value > 0.0)
                    radius = value;
            }
        }

        #endregion
    }
}
