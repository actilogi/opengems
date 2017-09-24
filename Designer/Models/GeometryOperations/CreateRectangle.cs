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
    public class CreateRectangle : GeometryCreateOperation
    {
        private Vector3WithUnit refPoint = null;
        private Length width = null;
        private Length height = null;
        private Axis alineAxis = Axis.X;

        public CreateRectangle(int id)
            : base(id)
        {
 
        }


        public CreateRectangle(int id, GEMSSingle parent)
            : base(id, parent)
        {
        }

        public CreateRectangle(System.Xml.XPath.XPathNavigator navigator, GEMSSingle parent)
            : base(navigator, parent)
        {


            alineAxis = (Axis)Enum.Parse(typeof(Axis), navigator.GetAttribute("axis", string.Empty));

            //Width of rectangle
            navigator.MoveToChild("Width",string.Empty);
            width = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Height of rectangle
            navigator.MoveToChild("Height", string.Empty);
            height = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Reference point of rectangle
            navigator.MoveToChild("RefPoint", string.Empty);
            refPoint = new Vector3WithUnit();
            refPoint.X = new Length(navigator.GetAttribute("x", string.Empty), navigator.GetAttribute("ux", string.Empty));
            refPoint.Y = new Length(navigator.GetAttribute("y", string.Empty), navigator.GetAttribute("uy", string.Empty));
            refPoint.Z = new Length(navigator.GetAttribute("z", string.Empty), navigator.GetAttribute("uz", string.Empty));
            navigator.MoveToParent();
        }

       #region Parent Class's Abstarct Methods

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns> 
        /// The example xml string like:
        ///<Operation id="6" name="CreateRectangle" relativeCS="0" axis="2" >
        ///  <Sources>
        ///    <Parent id="6" />
        ///  </Sources>
        ///  <Targets/>
        ///  <Width unit="3" value="-0.2" />
        ///  <Height unit="3" value="0.6" />
        ///  <RefPoint x="-0.8" y="0.6" z="0" ux="3" uy="3" uz="3" />
        ///</Operation>
        /// </returns>
        public override string BuildOuterXmlString()
        {
            StringBuilder rectangleNodeBuilder = new StringBuilder();

            rectangleNodeBuilder.AppendFormat("<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\" axis=\"{2}\">", this.Id, this.Name,(int)this.alineAxis);
            rectangleNodeBuilder.Append("<Sources>");
            rectangleNodeBuilder.AppendFormat("<Source id=\"{0}\" />", this.Parent.Id);
            rectangleNodeBuilder.Append("</Sources>");
            rectangleNodeBuilder.Append("<Targets/>");
            rectangleNodeBuilder.Append(this.width.BuildInnserXmlString("Width"));
            rectangleNodeBuilder.Append(this.height.BuildInnserXmlString("Height"));
            rectangleNodeBuilder.Append(this.refPoint.BuildInnserXmlString("RefPoint"));
            rectangleNodeBuilder.Append("</Operation>");

            return rectangleNodeBuilder.ToString();
        }

        public override GeometryOperation Clone(int newId, GEMSSingle parent)
        {
            CreateRectangle rectangle = new CreateRectangle(newId, parent);

            rectangle.AlineAxis = this.alineAxis;
            rectangle.Width = new Length(this.width);
            rectangle.Height = new Length(this.height);
            rectangle.RefPoint = new Vector3WithUnit(this.refPoint);

            return rectangle;
        }

        public override GeometryModel CreateSourceGeometryModel()
        {
            RectangleModel rectangle = new RectangleModel();

            float width = this.width.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);
            float height = this.height.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);

            rectangle.Width = Math.Abs(width);
            rectangle.Height = Math.Abs(height);

            Vector3 centerVector3 = this.refPoint.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);
            Matrix worldMatrix = Matrix.Identity ;
            Vector3 eoStart = Vector3.Empty;
            Vector3 eoEnd = Vector3.Empty;

            switch(this.alineAxis)
            {
                case Axis.X:
                    eoStart = centerVector3 + new Vector3(0, 0, height);
                    eoEnd = new Vector3(GeometryModel.DefaultEOLineLength, 0, 0);

                    centerVector3.Add(new Vector3(0, width / 2.0f, height / 2.0f));

                    worldMatrix = Matrix.RotationX((float)Math.PI / 2.0f)
                        * Matrix.RotationZ((float)Math.PI / 2.0f)
                        * Matrix.Translation(centerVector3);
                    break;  
                case Axis.Y:
                    eoStart = centerVector3 + new Vector3(height, 0, 0);
                    eoEnd = new Vector3(0, GeometryModel.DefaultEOLineLength, 0);

                    centerVector3.Add(new Vector3(height / 2.0f, 0, width / 2.0f));

                    worldMatrix = Matrix.RotationY((float)Math.PI / 2.0f)
                        * Matrix.RotationZ((float)Math.PI / 2.0f)
                        * Matrix.Translation(centerVector3);
                    break;
                case Axis.Z:
                    eoStart = centerVector3 + new Vector3(0, height, 0);
                    eoEnd = new Vector3(0, 0, GeometryModel.DefaultEOLineLength);

                    centerVector3.Add(new Vector3(width / 2.0f, height / 2.0f, 0));
                    worldMatrix = Matrix.Translation(centerVector3);
                    break;
            }

            rectangle.WorldMatrix = worldMatrix;
            rectangle.CenterVector3 = centerVector3;
            rectangle.EOStart = eoStart;
            rectangle.EOEnd = eoEnd;
            rectangle.AlineAxis = this.alineAxis;

            return rectangle;
        }

       #endregion

        #region Properties

        [CategoryAttribute("Geometry"), DisplayNameAttribute("Axis"), DescriptionAttribute("Axis of the cylinder")]
        public Axis AlineAxis
        {
            get { return alineAxis; }
            set { alineAxis = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Reference point of rectangle")]
        public Vector3WithUnit RefPoint
        {
            get { return refPoint; }
            set { refPoint = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Width of the rectangle")]
        public Length Width
        {
            get { return width; }
            set
            {
                if (value.Value > 0.0 || value.Value < 0.0)
                    width = value;
            }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Height of the rectangle")]
        public Length Height
        {
            get { return height; }
            set
            {
                if (value.Value > 0.0 || value.Value < 0.0)
                    height = value;
            }
        }

        #endregion
    }
}
