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

namespace GEMS.Designer
{
    /// <summary>
    /// This class is used to denote a length value with unit, such as "0.2mm" , "0.2cm", and so on
    /// </summary>
    public class Frequency
    {
        //Rate between length units and the meter.
        private static float[] Ratio = new float[] { 1.0f, 1.0E3f, 1.0E6f, 1.0E9f, 1.0E12f, 1.0E15f };
        private const float defaultValue = 0.0f;
        private const FrequencyUnit defaultUnit = FrequencyUnit.GHz;

        public enum FrequencyUnit
        {
            Hz = 0,
            KHz,
            MHz,
            GHz,
            THz,
            PHz,
        }

        private float value;
        private FrequencyUnit unit = defaultUnit;

        public Frequency(float value, FrequencyUnit unit)
        {
            this.value = value;
            this.unit   = unit;
        }

        public Frequency(string value, string unit)
        {
            try
            {
                this.value = float.Parse(value);
                this.unit = (FrequencyUnit)Enum.Parse(typeof(FrequencyUnit), unit);
            }
            catch
            {
                this.value = defaultValue;
                this.unit = defaultUnit;
            }
        }

        public static Frequency Default
        {
            get
            {
                return new Frequency(defaultValue, defaultUnit);
            }
        }

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public FrequencyUnit Unit
        { 
            get { return unit; } 
            set { this.unit = value; } 
        }

        public float ChangeUnit(FrequencyUnit newUnit)
        {
            return value * Ratio[(int)unit]/ Ratio[(int)newUnit];
        }

        public static float ChangeUnit(float value, FrequencyUnit oldUnit, FrequencyUnit newUnit)
        {
            return value * Ratio[(int)oldUnit] / Ratio[(int)newUnit];
        }

        public override string ToString()
        {
            return value.ToString() + unit.ToString();
        }
    }
}
