using System;
using System.Collections.Generic;
using System.Text;

namespace GEMS.Display.Core
{
    /// <summary>
    /// This class is used to denote a length value with unit, such as "0.2mm" , "0.2cm", and so on
    /// </summary>
    public class FrequencyFormator
    {
        //Rate between length units and the meter.
        private static float[] Ratio = new float[] { 1.0f, 1.0E3f, 1.0E6f, 1.0E9f, 1.0E12f, 1.0E15f };

        public enum FrequencyUnit
        {
            Hz = 0,
            KHz,
            MHz,
            GHz,
            THz,
            PHz,
        }

        public static string Format ( float value , FrequencyUnit unit)
        {
            string formattedValue = string.Format ( "{0}{1}",value/Ratio[(int)unit],unit.ToString());
            return formattedValue;
        }

        public static float ChangedUnit ( float value , FrequencyUnit oldUnit , FrequencyUnit newUnit )
        {
            return value * Ratio[(int)oldUnit] / Ratio[(int)newUnit];
        }
    }
}
