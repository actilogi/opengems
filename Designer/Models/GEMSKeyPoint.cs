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
using System.Xml;
using System.Xml.XPath;

namespace GEMS.Designer.Models
{
    public class GEMSKeyPoint
    {
        private float keyPoint;
        private float min;
        private float max;

        public GEMSKeyPoint(float keyPoint, float min, float max)
        {
            this.keyPoint = keyPoint;
            this.min = min;
            this.max = max;
        }

        public GEMSKeyPoint(XPathNavigator navigator)
        {
            keyPoint = float.Parse(navigator.GetAttribute("value", string.Empty));
            min = float.Parse(navigator.GetAttribute("min", string.Empty));
            max = float.Parse(navigator.GetAttribute("max", string.Empty));
        }

         /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        public string BuildOuterXmlString()
        {
            StringBuilder keyPointBuilder = new StringBuilder();

            keyPointBuilder.AppendFormat("<KeyPoint status=\"0\" ignore=\"0\" ratio=\"1\" value=\"{0}\" min=\"{1}\" max=\"{2}\" />",
                keyPoint,min,max);

            return keyPointBuilder.ToString();
        }

        public float KeyPoint
        {
            get { return keyPoint; }
            set { keyPoint = value; }
        }

        public float Min
        {
            get { return min; }
            set { min = value; }
        }

        public float Max
        {
            get { return max; }
            set { max = value; }
        }
    }
}
