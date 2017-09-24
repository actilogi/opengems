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
using System.Drawing;
using System.Xml;
using System.Xml.XPath;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Controls;
using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryModels;

namespace GEMS.Designer.Models.GeometryOperations
{
    public class CreateSphere : GeometryCreateOperation
    {
        private Vector3WithUnit center = null;
        private Length radius = null;

        public CreateSphere(int id)
            : base(id)
        {
 
        }

        public CreateSphere(int id, GEMSSingle parent)
            : base(id, parent)
        {
        }

        public CreateSphere(System.Xml.XPath.XPathNavigator navigator, GEMSSingle parent)
            : base(navigator, parent)
        {

            //Radius of sphere
            navigator.MoveToChild("Radius",string.Empty);
            radius = new Length(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

            //Center of sphere
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
        ///     <Operation id="1" name="CreateSphere" relativeCS="0" >
        ///         <Sources>
        ///             <Parent id="1" />
        ///         </Sources>
        ///         <Targets/>
        ///         <Radius unit="3" value="0.4" />
        ///         <Center x="-0.6" y="-1.4" z="0" ux="3" uy="3" uz="3" />
        ///     </Operation>
        /// </returns>
        public override string BuildOuterXmlString()
        {
            StringBuilder sphereNodeBuilder = new StringBuilder();

            sphereNodeBuilder.AppendFormat("<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\" >",this.Id,this.Name);
            sphereNodeBuilder.Append("<Sources>");
            sphereNodeBuilder.AppendFormat("<Source id=\"{0}\" />", this.Parent.Id);
            sphereNodeBuilder.Append("</Sources>");
            sphereNodeBuilder.Append("<Targets/>");
            sphereNodeBuilder.Append(this.radius.BuildInnserXmlString("Radius"));
            sphereNodeBuilder.Append(this.center.BuildInnserXmlString("Center"));
            sphereNodeBuilder.Append("</Operation>");

            return sphereNodeBuilder.ToString();
        }

        public override GeometryOperation Clone(int newId, GEMSSingle parent)
        {
            CreateSphere sphere = new CreateSphere(newId, parent);

            sphere.Radius = new Length(this.radius);
            sphere.Center = new Vector3WithUnit(this.center);

            return sphere;
        }

        public override GeometryModel CreateSourceGeometryModel()
        {
            SphereModel sphere = new SphereModel();

            sphere.Radius = this.radius.ChangeUnit(Parent.Parent.Environment.DefaultLengthUnit);

            //Set the matrix
            sphere.CenterVector3 = this.center.GetDirectXVector(Parent.Parent.Environment.DefaultLengthUnit);
            sphere.WorldMatrix = Matrix.Translation(sphere.CenterVector3);

            return sphere;
        }

       #endregion

        #region Properties

        [CategoryAttribute("Geometry"),DescriptionAttribute("Center of the Sphere")]
        public Vector3WithUnit Center
        {
            get { return center; }
            set { center = value; }
        }

        [CategoryAttribute("Geometry"), DescriptionAttribute("Radius of the Sphere")]
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
