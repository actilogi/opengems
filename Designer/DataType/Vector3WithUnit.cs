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
using Microsoft.DirectX;
using System.ComponentModel;

namespace GEMS.Designer
{
    [TypeConverter(typeof(Vector3WithUnitConvertor))]
    public class Vector3WithUnit
    {
        private Length x;
        private Length y;
        private Length z;

        public Length X
        {
            get { return x; }
            set { x = value; }

        }

        public Length Y
        {
            get { return y; }
            set { y = value; }

        }

        public Length Z
        {
            get { return z; }
            set { z = value; }

        }

        public Vector3WithUnit()
        {

        }

        public Vector3WithUnit(float value,Length.LengthUnit unit)
        {
            this.x = new Length(value, unit);
            this.y = new Length(value, unit);
            this.z = new Length(value, unit);
        }

        public Vector3WithUnit(Vector3WithUnit vector)
        {
            this.x = new Length(vector.X);
            this.y = new Length(vector.Y);
            this.z = new Length(vector.Z);
        }

        public Vector3WithUnit(Vector3 vector,Length.LengthUnit unit)
        {
            this.x = new Length(vector.X, unit);
            this.y = new Length(vector.Y, unit);
            this.z = new Length(vector.Z, unit);
        }

        public string BuildInnserXmlString(string nodeName)
        {
            string innserXml = string.Format("<{0} x=\"{1}\" y=\"{2}\" z=\"{3}\" ux=\"{4}\" uy=\"{5}\" uz=\"{6}\" />",
                nodeName, x.Value, y.Value, z.Value, (int)x.Unit, (int)y.Unit, (int)z.Unit);

            return innserXml;
        }

        public Vector3 GetDirectXVector(Length.LengthUnit unit)
        {
            return new Vector3(x.ChangeUnit(unit), y.ChangeUnit(unit), z.ChangeUnit(unit));
        }

        #region Vector3WithUnitConvertor Specification
        internal class Vector3WithUnitConvertor : System.ComponentModel.ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(System.String))
                    return true;

                return base.CanConvertTo(context, destinationType);

            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(System.String) &&
                         value is Vector3WithUnit)
                {

                    Vector3WithUnit vector3 = (Vector3WithUnit)value;

                    return vector3.X.ToString() + "," + vector3.Y.ToString() + "," + vector3.Z.ToString();
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

        }
        #endregion
    }
}
