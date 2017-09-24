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

namespace GEMS.Designer
{
    /// <summary>
    /// This class is used to denote a length value with unit, such as "0.2mm" , "0.2cm", and so on
    /// </summary>
    [TypeConverter ( typeof ( LengthConvertor ) )]
    public class Length
    {
        //Rate between length units and the meter.
        private static float[] Ratio = new float[] { 1.0E-9f , 1.0E-6f , 2.54E-5f , 1.0E-3f , 1.0E-2f , 2.54E-2f , 3.048E-1f , 1.0f };
        private const float defaultValue = 0.0f;
        private const LengthUnit defaultUnit = LengthUnit.mm;

        public enum LengthUnit
        {
            nm=0 ,
            um ,
            mil ,
            mm ,
            cm ,
            inch ,
            foot ,
            meter ,
        }

        private float value;
        private LengthUnit unit = defaultUnit;

        public Length ( float value , LengthUnit unit )
        {
            this.value = value;
            this.unit   = unit;
        }

        public Length ( string value , string unit )
        {
            try
            {
                this.value = float.Parse ( value );
                this.unit = (LengthUnit)Enum.Parse ( typeof ( LengthUnit ) , unit );
            }
            catch
            {
                this.value = defaultValue;
                this.unit = defaultUnit;
            }
        }

        public Length ( Length length )
        {
            this.value = length.value;
            this.unit = length.unit;
        }

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public LengthUnit Unit
        {
            get { return unit; }
            set { this.unit = value; }

        }

        public float ChangeUnit ( LengthUnit newUnit )
        {
            if (newUnit == this.unit)
                return value;

            return value * Ratio[(int)unit]/ Ratio[(int)newUnit];
        }

        public override string ToString ( )
        {
            return value.ToString ( ) + unit.ToString ( );
        }

        public string BuildInnserXmlString ( string nodeName )
        {
            string innserXml = string.Format ( "<{0} unit=\"{1}\" value=\"{2}\" />" , nodeName , (int)unit , value );

            return innserXml;
        }

        public static float RatioValue ( Length.LengthUnit unit )
        {
            return Ratio[(int)unit];
        }

        public static bool operator> ( Length a , Length b )
        {
            return a.ChangeUnit ( LengthUnit.meter ) > b.ChangeUnit ( LengthUnit.meter );
        }

        public static bool operator< ( Length a , Length b )
        {
            return a.ChangeUnit ( LengthUnit.meter ) <=  b.ChangeUnit ( LengthUnit.meter );
        }      

        public override bool Equals ( object obj )
        {
            Length target = obj as Length;

            if (target != null)
            {
                return this.value == target.value && this.unit == target.unit;
            }
            else
                return base.Equals ( obj );
        }

        public override int GetHashCode ( )
        {
            return base.GetHashCode ( );
        }

        #region LengthConvertor Specification

        internal class LengthConvertor : System.ComponentModel.ExpandableObjectConverter
        {
            public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
            {
                if (destinationType == typeof ( System.String ))
                    return true;

                return base.CanConvertTo ( context , destinationType );

            }

            public override bool CanConvertFrom ( ITypeDescriptorContext context , Type sourceType )
            {
                if (sourceType == typeof ( System.String ))
                    return true;

                return base.CanConvertFrom ( context , sourceType );

            }

            public override object ConvertFrom ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value )
            {
                if (value is string)
                {
                    try
                    {
                        string s = ( (string)value ).Trim ( );
                        if (s != string.Empty)
                        {
                            string[] units = Enum.GetNames ( typeof ( Length.LengthUnit ) );

                            for (int i = 0 ; i < units.Length ; i++)
                            {
                                int unitLoc = s.IndexOf ( units[i] ); //Find the unit in the string
                                float length;

                                if (( unitLoc != -1 ) && float.TryParse ( s.Substring ( 0 , unitLoc ) , out length ))
                                {
                                    Length.LengthUnit unit = (Length.LengthUnit)Enum.Parse ( typeof ( Length.LengthUnit ) , units[i] );
                                    Length lengthValue = new Length ( length , unit );

                                    return lengthValue;
                                } 
                            }
                        }
                        else
                            throw new ArgumentException ("Can not convert '" + (string)value + "' to type Length" );

                    }
                    catch
                    {
                        throw new ArgumentException ( "Can not convert '" + (string)value + "' to type Length" );
                    }
                }

                return base.ConvertFrom ( context , culture , value );

            }

            public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType )
            {
                if (destinationType == typeof ( System.String ) &&
                         value is Length)
                {

                    Length length = (Length)value;

                    return length.ToString ( );
                }

                return base.ConvertTo ( context , culture , value , destinationType );
            }

        }

        #endregion
    }
}
