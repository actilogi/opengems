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
    public class CreateCylinder : GeometryCreateOperation
    {        
        private Vector3WithUnit center = null;
        private Length radius = null;
        private Length height = null;
        private Axis alineAxis = Axis.X;

        public CreateCylinder(int id)
            : base(id)
        {
 
        }


        public CreateCylinder(int id, GEMSSingle parent)
            : base(id, parent)
        {
        }

        public CreateCylinder(System.Xml.XPath.XPathNavigator navigator, GEMSSingle parent)
            : base(navigator, parent)
        {
            alineAxis = (Axis)Enum.Parse(typeof(Axis),navigator.GetAttribute("axis", string.Empty));

            //Radius of cylinder
            navigator.MoveToChild("Radius", string.Empty);
            radius = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Height of cylinder
            navigator.MoveToChild("Height", string.Empty);
            height = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Center of cylinder
            navigator.MoveToChild("Center", string.Empty);
            center = new Vector3WithUnit();
            center.X = new Length(navigator.GetAttribute("x", string.Empty), navigator.GetAttribute("ux", string.Empty));
            center.Y = new Length(navigator.GetAttribute("y", string.Empty), navigator.GetAttribute("uy", string.Empty));
            center.Z = new Length(navigator.GetAttribute("z", string.Empty), navigator.GetAttribute("uz", string.Empty));
            navigator.MoveToParent();
        }

        #region Parent Class's Abstract Methods

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns> 
        /// The example xml string like:
        /// <Operation id="10" name="CreateCylinder" relativeCS="1" axis="2" >
        ///  <Sources>
        ///    <Parent id="10" />
        ///  </Sources>
        ///  <Targets/>
        ///  <Radius unit="3" value="1.45602" />
        ///  <Height unit="3" value="3.4" />
        ///  <Center x="0" y="0.2" z="0" ux="3" uy="3" uz="3" />
        ///</Operation>
        /// </returns>
        public override string BuildOuterXmlString()
        {
            StringBuilder cylinderNodeBuilder = new StringBuilder();

            cylinderNodeBuilder.AppendFormat("<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\" axis=\"{2}\">", this.Id, this.Name, (int)this.alineAxis);
            cylinderNodeBuilder.Append("<Sources>");
            cylinderNodeBuilder.AppendFormat("<Source id=\"{0}\" />", this.Parent.Id);
            cylinderNodeBuilder.Append("</Sources>");
            cylinderNodeBuilder.Append("<Targets/>");
            cylinderNodeBuilder.Append(this.radius.BuildInnserXmlString("Radius"));
            cylinderNodeBuilder.Append(this.height.BuildInnserXmlString("Height"));
            cylinderNodeBuilder.Append(this.center.BuildInnserXmlString("Center"));
            cylinderNodeBuilder.Append("</Operation>");

            return cylinderNodeBuilder.ToString();
        }

        public override GeometryOperation Clone(int newId, GEMSSingle parent)
        {
            CreateCylinder cylinder = new CreateCylinder(newId, parent);

            cylinder.AlineAxis = this.alineAxis;
            cylinder.Radius = new Length(this.radius);
            cylinder.Height = new Length(this.height);
            cylinder.Center = new Vector3WithUnit(this.center);

            return cylinder;
        }

        public override GEMS.Designer.Models.GeometryModels.GeometryModel CreateSourceGeometryModel()
        {
            CylinderModel cylinder = new CylinderModel();

            float height = this.height.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);
            float radius1 = this.radius.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);

            cylinder.Height = Math.Abs(height);
            cylinder.Radius = radius1;

            //Compute the center vector3
            Vector3 centerVector3 = this.center.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);
            Matrix worldMatrix = Matrix.Identity;

            //Set the matrix
            switch(this.alineAxis)
            {
                case Axis.X:
                    centerVector3.Add(new Vector3(height / 2.0f, 0, 0));
                    worldMatrix = Matrix.RotationY(-(float)Math.PI / 2.0f) * Matrix.Translation(centerVector3);
                    break;  
                case Axis.Y:
                    centerVector3.Add(new Vector3(0, height / 2.0f, 0));
                    worldMatrix = Matrix.RotationX((float)Math.PI / 2.0f) * Matrix.Translation(centerVector3);
                    break;
                case Axis.Z:
                    centerVector3.Add(new Vector3(0, 0, height / 2.0f));
                    worldMatrix = Matrix.Translation(centerVector3);
                    break;
            }

            cylinder.AlineAxis = this.alineAxis;
            cylinder.WorldMatrix = worldMatrix;
            cylinder.CenterVector3 = centerVector3;

            return cylinder;
        }
        #endregion

        #region Properties

        [CategoryAttribute("Geometry"), DisplayNameAttribute("Axis"), DescriptionAttribute("Axis of the cylinder")]
        public Axis AlineAxis
        {
            get { return alineAxis; }
            set { alineAxis = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Center of the cylinder")]
        public Vector3WithUnit Center
        {
            get { return center; }
            set { center = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Radius of the cylinder")]
        public Length Radius
        {
            get { return radius; }
            set {
                if (value.Value > 0.0)
                    radius = value; 

            }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Height of the cylinder")]
        public Length Height
        {
            get { return height; }
            set {
                if (value.Value > 0.0 || value.Value < 0.0)
                   height = value; 
            }
        }

      
        #endregion
    }
}
