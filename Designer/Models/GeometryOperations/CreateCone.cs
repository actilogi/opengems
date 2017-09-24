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
    public class CreateCone : GeometryCreateOperation
    {
        private Vector3WithUnit center = null;
        private Length topRadius = null;
        private Length bottomRadius = null;
        private Length height = null;
        private Axis alineAxis = Axis.X;

        public CreateCone ( int id )
            : base ( id )
        {

        }

        public CreateCone ( int id , GEMSSingle parent )
            : base ( id , parent )
        {

        }

        public CreateCone ( System.Xml.XPath.XPathNavigator navigator , GEMSSingle parent )
            : base ( navigator , parent )
        {

            alineAxis = (Axis)Enum.Parse ( typeof ( Axis ) , navigator.GetAttribute ( "axis" , string.Empty ) );

            //Bottom radius of cylinder
            navigator.MoveToChild ( "BottomRadius" , string.Empty );
            bottomRadius = new Length ( navigator.GetAttribute ( "value" , string.Empty ) , navigator.GetAttribute ( "unit" , string.Empty ) );
            navigator.MoveToParent ( );

            //Top radius of cylinder
            navigator.MoveToChild ( "TopRadius" , string.Empty );
            topRadius = new Length ( navigator.GetAttribute ( "value" , string.Empty ) , navigator.GetAttribute ( "unit" , string.Empty ) );
            navigator.MoveToParent ( );

            //Height of cylinder
            navigator.MoveToChild ( "Height" , string.Empty );
            height = new Length ( navigator.GetAttribute ( "value" , string.Empty ) , navigator.GetAttribute ( "unit" , string.Empty ) );
            navigator.MoveToParent ( );

            //Center of cylinder
            navigator.MoveToChild ( "Center" , string.Empty );
            center = new Vector3WithUnit ( );
            center.X = new Length ( navigator.GetAttribute ( "x" , string.Empty ) , navigator.GetAttribute ( "ux" , string.Empty ) );
            center.Y = new Length ( navigator.GetAttribute ( "y" , string.Empty ) , navigator.GetAttribute ( "uy" , string.Empty ) );
            center.Z = new Length ( navigator.GetAttribute ( "z" , string.Empty ) , navigator.GetAttribute ( "uz" , string.Empty ) );
            navigator.MoveToParent ( );
        }

        #region Parent Class's  Abstract Methods

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <returns> 
        /// The example xml string like:
        ///    <Operation id="4" name="CreateCone" relativeCS="0" axis="0" >
        ///      <Sources>
        ///        <Parent id="4" />
        ///      </Sources>
        ///      <Targets/>
        ///      <BottomRadius unit="3" value="0.2" />
        ///      <TopRadius unit="3" value="0.2" />
        ///      <Height unit="3" value="0.4" />
        ///      <Center x="-0.8" y="0" z="0" ux="3" uy="3" uz="3" />
        ///    </Operation>
        /// </returns>
        public override string BuildOuterXmlString ( )
        {
            StringBuilder coneNodeBuilder = new StringBuilder ( );

            coneNodeBuilder.AppendFormat ( "<Operation id=\"{0}\" name=\"{1}\" relativeCS=\"0\" axis=\"{2}\">" , this.Id , this.Name , (int)this.alineAxis );
            coneNodeBuilder.Append ( "<Sources>" );
            coneNodeBuilder.AppendFormat ( "<Source id=\"{0}\" />" , this.Parent.Id );
            coneNodeBuilder.Append ( "</Sources>" );
            coneNodeBuilder.Append ( "<Targets/>" );
            coneNodeBuilder.Append ( this.bottomRadius.BuildInnserXmlString ( "BottomRadius" ) );
            coneNodeBuilder.Append ( this.topRadius.BuildInnserXmlString ( "TopRadius" ) );
            coneNodeBuilder.Append ( this.height.BuildInnserXmlString ( "Height" ) );
            coneNodeBuilder.Append ( this.center.BuildInnserXmlString ( "Center" ) );
            coneNodeBuilder.Append ( "</Operation>" );

            return coneNodeBuilder.ToString ( );
        }

        public override GeometryOperation Clone ( int newId , GEMSSingle parent )
        {
            CreateCone cone = new CreateCone ( newId , parent );

            cone.AlineAxis = this.alineAxis;
            cone.BottomRadius = new Length ( this.bottomRadius );
            cone.TopRadius = new Length ( this.topRadius );
            cone.Height = new Length ( this.height );
            cone.Center = new Vector3WithUnit ( this.center );

            return cone;
        }

        public override GeometryModel CreateSourceGeometryModel ( )
        {
            ConeModel coneModel = new ConeModel ( );

            float height = this.height.ChangeUnit ( Parent.Parent.Environment.DefaultLengthUnit );
            float radius1 = this.bottomRadius.ChangeUnit ( Parent.Parent.Environment.DefaultLengthUnit );
            float radius2 = this.topRadius.ChangeUnit ( Parent.Parent.Environment.DefaultLengthUnit );

            if (height > 0)
            {
                coneModel.Height  = height;
                coneModel.Radius1 = radius1;
                coneModel.Radius2 = radius2;
            }
            else
            {
                coneModel.Height = -height;
                coneModel.Radius1 = radius2;
                coneModel.Radius2 = radius1;
            }

            //Compute the center and transform matrix
            Vector3 centerVector3 = this.center.GetDirectXVector ( Parent.Parent.Environment.DefaultLengthUnit );
            Matrix worldMatrix = Matrix.Identity;

            //Set the matrix
            switch (this.alineAxis)
            {
                case Axis.X:
                    centerVector3.Add ( new Vector3 ( height / 2.0f , 0 , 0 ) );
                    worldMatrix = Matrix.RotationY ( (float)Math.PI / 2.0f ) * Matrix.Translation ( centerVector3 );
                    break;
                case Axis.Y:
                    centerVector3.Add ( new Vector3 ( 0 , height / 2.0f , 0 ) );
                    worldMatrix = Matrix.RotationX ( -(float)Math.PI / 2.0f ) * Matrix.Translation ( centerVector3 );
                    break;
                case Axis.Z:
                    centerVector3.Add ( new Vector3 ( 0 , 0 , height / 2.0f ) );
                    worldMatrix = Matrix.Translation ( centerVector3 );
                    break;
            }

            coneModel.CenterVector3 = centerVector3;
            coneModel.WorldMatrix = worldMatrix;
            coneModel.AlineAxis = this.alineAxis;

            return coneModel;
        }

        #endregion

        #region Properties

        [CategoryAttribute ( "Geometry" ) , DisplayNameAttribute ( "Axis" ) , DescriptionAttribute ( "Axis of the cone" )]
        public Axis AlineAxis
        {
            get { return alineAxis; }
            set { alineAxis = value; }
        }

        [CategoryAttribute ( "Geometry" ) , DescriptionAttribute ( "Center of the cone" )]
        public Vector3WithUnit Center
        {
            get { return center; }
            set { center = value; }
        }

        [CategoryAttribute ( "Geometry" ) , DescriptionAttribute ( "Bottom radius of the cone" )]
        public Length BottomRadius
        {
            get { return bottomRadius; }
            set
            {
                if (value.Value > 0.0 || (value.Value == 0.0 && topRadius.Value > 0.0))
                    bottomRadius = value;
            }
        }

        [CategoryAttribute ( "Geometry" ) , Description ( "topRadius of the cone" )]
        public Length TopRadius
        {
            get { return topRadius; }
            set
            {
                if (value.Value > 0.0 || (value.Value == 0.0 && bottomRadius.Value > 0.0 ))
                    topRadius = value;
            }
        }

        [CategoryAttribute ( "Geometry" ) , DescriptionAttribute ( "Height of the cone" )]
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
