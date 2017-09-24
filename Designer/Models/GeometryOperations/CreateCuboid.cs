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
    public class CreateCuboid : GeometryCreateOperation
    {
        private Vector3WithUnit refPoint = null;
        private Length width = null;
        private Length depth = null;
        private Length height = null;

        public CreateCuboid(int id): base(id)
        {
 
        }

        public CreateCuboid(int id, GEMSSingle parent):base(id,parent)
        {
        }

        public CreateCuboid(System.Xml.XPath.XPathNavigator navigator, GEMSSingle parent)
            :base(navigator,parent)
        {

            //Width of box
            navigator.MoveToChild("Width",string.Empty);
            width = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Depth of box
            navigator.MoveToChild("Depth", string.Empty);
            depth = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Height of box
            navigator.MoveToChild("Height", string.Empty);
            height = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Reference point of box
            navigator.MoveToChild("RefPoint", string.Empty);
            refPoint = new Vector3WithUnit();
            refPoint.X = new Length(navigator.GetAttribute("x", string.Empty), navigator.GetAttribute("ux", string.Empty));
            refPoint.Y = new Length(navigator.GetAttribute("y", string.Empty), navigator.GetAttribute("uy", string.Empty));
            refPoint.Z = new Length(navigator.GetAttribute("z", string.Empty), navigator.GetAttribute("uz", string.Empty));
            navigator.MoveToParent();

        }

        #region Parent Class's Abstract Methods

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns> 
        /// The example xml string like:
        ///<Operation id="0" name="CreateCuboid" relativeCS="0" >
        ///  <Sources>
        ///    <Parent id="0" />
        ///  </Sources>
        ///  <Targets/>
        ///  <Width unit="3" value="-0.1" />
        ///  <Depth unit="3" value="0.4" />
        ///  <Height unit="3" value="-0.8" />
        ///  <RefPoint x="-0.4" y="-2.6" z="0" ux="3" uy="3" uz="3" />
        ///</Operation>
        /// </returns>
        public override string BuildOuterXmlString()
        {
            StringBuilder rectangleNodeBuilder = new StringBuilder();

            rectangleNodeBuilder.AppendFormat("<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\" >", this.Id, this.Name);
            rectangleNodeBuilder.Append("<Sources>");
            rectangleNodeBuilder.AppendFormat("<Source id=\"{0}\" />", this.Parent.Id);
            rectangleNodeBuilder.Append("</Sources>");
            rectangleNodeBuilder.Append("<Targets/>");
            rectangleNodeBuilder.Append(this.width.BuildInnserXmlString("Width"));
            rectangleNodeBuilder.Append(this.depth.BuildInnserXmlString("Depth"));
            rectangleNodeBuilder.Append(this.height.BuildInnserXmlString("Height"));
            rectangleNodeBuilder.Append(this.refPoint.BuildInnserXmlString("RefPoint"));
            rectangleNodeBuilder.Append("</Operation>");

            return rectangleNodeBuilder.ToString();
        }

        public override GeometryOperation Clone(int newId, GEMSSingle parent)
        {
            CreateCuboid box = new CreateCuboid(newId, parent);

            box.Width = new Length(this.width);
            box.Height = new Length(this.height);
            box.Depth = new Length(this.depth);
            box.RefPoint = new Vector3WithUnit(this.refPoint);

            return box;
        }

        public override GeometryModel CreateSourceGeometryModel()
        {
            CuboidModel cuboid = new CuboidModel();

            float width = this.width.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);
            float height = this.height.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);
            float depth = this.depth.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);

            cuboid.Width = Math.Abs(width);
            cuboid.Depth = Math.Abs(depth);
            cuboid.Height = Math.Abs(height);

            //Compute the center vector3
            cuboid.CenterVector3 = this.RefPoint.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);
            cuboid.CenterVector3 = Vector3.Add(cuboid.CenterVector3,new Vector3(width / 2.0f, depth / 2.0f, height / 2.0f));

            //Set the matrix
            cuboid.WorldMatrix = Matrix.Translation(cuboid.CenterVector3);

            return cuboid;
        }

        #endregion

        #region Properties

        [CategoryAttribute("Geometry"), DescriptionAttribute("Reference point of box")]
        public Vector3WithUnit RefPoint
        {
            get { return refPoint; }
            set { refPoint = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Width of the box")]
        public Length Width
        {
            get { return width; }
            set
            {
                if (value.Value > 0.0 || value.Value < 0.0)
                    width = value;
            }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Height of the box")]
        public Length Height
        {
            get { return height; }
            set {
                if (value.Value > 0.0 || value.Value < 0.0)
                    height = value;
            }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Depth of the box")]
        public Length Depth
        {
            get { return depth; }
            set
            {
                if (value.Value > 0.0 || value.Value < 0.0)
                    depth = value;
            }
        }
       
        #endregion
    }
}
