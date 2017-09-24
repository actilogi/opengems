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
using System.Xml.XPath;
using System.Xml;

namespace GEMS.Designer.Models
{
    public class GEMSParallelArea
    {
        private bool isPEC = false;
        private MeshPointKey start;
        private MeshPointKey end;

        public GEMSParallelArea()
        {

        }

        public GEMSParallelArea(XPathNavigator navigator)
        {
            start = new MeshPointKey();
            start.Z = int.Parse(navigator.GetAttribute("z1", string.Empty));
            start.Y = int.Parse(navigator.GetAttribute("y1", string.Empty));
            start.X = int.Parse(navigator.GetAttribute("x1", string.Empty));

            end = new MeshPointKey();
            end.Z = int.Parse(navigator.GetAttribute("z2", string.Empty));
            end.Y = int.Parse(navigator.GetAttribute("y2", string.Empty));
            end.X = int.Parse(navigator.GetAttribute("x2", string.Empty));
        }

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        public string BuildOuterXmlString()
        {
            StringBuilder areaNodeBuilder = new StringBuilder();

            areaNodeBuilder.AppendFormat("<Area z1=\"{0}\" z2=\"{1}\" y1=\"{2}\" y2=\"{3}\" x1=\"{4}\" x2=\"{5}\" />", 
                start.Z, end.Z, start.Y, end.Y, start.X, end.X);

            return areaNodeBuilder.ToString();
        }

        public MeshPointKey Start
        {
            get { return start; }
            set { start = value; }
        }

        public MeshPointKey End
        {
            get { return end; }
            set { end = value; }
        }

        public bool IsPEC
        {
            get { return isPEC; }
            set { isPEC = value; }
        }
    }

}
