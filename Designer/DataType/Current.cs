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
    public class Current
    {
        //Rate between length units and the meter.
        private static float[] Ratio = new float[] { 1.0E-6f, 1.0E-3f, 1.0f, 1.0E3f };
        private const float defaultValue = 1.0f;
        private const CurrentUnit defaultUnit = CurrentUnit.mA;

        public enum CurrentUnit
        {
            A = 0,
            mA,
            amp,
            kA,
        }

        private float value;
        private CurrentUnit unit = defaultUnit;

        public Current(float value, CurrentUnit unit)
        {
            this.value = value;
            this.unit   = unit;
        }

        public Current(string value, string unit)
        {
            try
            {
                this.value = float.Parse(value);
                this.unit = (CurrentUnit)Enum.Parse(typeof(CurrentUnit), unit);
            }
            catch
            {
                this.value = defaultValue;
                this.unit = defaultUnit;
            }
        }   

        public static Current Default
        {
            get{
                return new Current(defaultValue, defaultUnit);
            }
        }


        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public CurrentUnit Unit
        { 
            get { return unit; } 
            set { this.unit = value; } 
        }

        public float ChangeUnit(CurrentUnit newUnit)
        {
            return value * Ratio[(int)unit]/ Ratio[(int)newUnit];
        }
    }
}
