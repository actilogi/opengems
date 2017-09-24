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
    public class Time
    {
        //Rate between length units and the meter.
        private static float[] Ratio = new float[] { 1.0E-12f, 1.0E-9f, 1.0E-6f, 1.0E-3f, 1.0f };
        private const float defaultValue = 0.0f;
        private const TimeUnit defaultUnit = TimeUnit.ns;

        public enum TimeUnit
        {
            ps = 0,
            ns,
            us,
            ms,
            sec,
        }

        private float value;
        private TimeUnit unit = defaultUnit;

        public Time(float value, TimeUnit unit)
        {
            this.value = value;
            this.unit   = unit;
        }

        public Time(string value, string unit)
        {
            try
            {
                this.value = float.Parse(value);
                this.unit = (TimeUnit)Enum.Parse(typeof(TimeUnit), unit);
            }
            catch
            {
                this.value = defaultValue;
                this.unit = defaultUnit;
            }
        }

        public static Time Default
        {
            get
            {
                return new Time(defaultValue, defaultUnit);
            }
        }

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }
   
        public TimeUnit Unit
        { 
            get { return unit; } 
            set { this.unit = value; } 
        }

        public float ChangeUnit(TimeUnit newUnit)
        {
            return value * Ratio[(int)unit]/ Ratio[(int)newUnit];
        }

        public static float ChangeUnit ( float oldValue , TimeUnit oldUnit , TimeUnit newUnit )
        {
            return oldValue * Ratio[(int)oldUnit]/ Ratio[(int)newUnit];
        }
    }
}
