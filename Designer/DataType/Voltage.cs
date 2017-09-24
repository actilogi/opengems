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
    public class Voltage
    {
        //Rate between length units and the meter.
        public static float[] Ratio = new float[] { 1.0E-3f, 1.0f, 1.0E3f, 1.0E6f };  
        private const float defaultValue = 1.0f;
        private const VoltageUnit defaultUnit = VoltageUnit.Volt;

        public enum VoltageUnit
        {
            mV = 0,
            Volt,
            kV,
            MV,
        }

        private float value;
        private VoltageUnit unit = defaultUnit;

        public Voltage(float value, VoltageUnit unit)
        {
            this.value = value;
            this.unit   = unit;
        }

        public Voltage(string value, string unit)
        {
            try
            {
                this.value = float.Parse(value);
                this.unit = (VoltageUnit)Enum.Parse(typeof(VoltageUnit), unit);
            }
            catch
            {
                this.value = defaultValue;
                this.unit = defaultUnit;
            }
        }

        public static Voltage Default
        {
            get
            {
                return new Voltage(defaultValue, defaultUnit);
            }
        }

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public VoltageUnit Unit
        { 
            get { return unit; } 
            set { this.unit = value; } 
        }

        public float ChangeUnit(VoltageUnit newUnit)
        {
            return value * Ratio[(int)unit]/ Ratio[(int)newUnit];
        }
    }
}
