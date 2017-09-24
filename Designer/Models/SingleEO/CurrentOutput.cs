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
    public class CurrentOutput : SingleOutput
    {
        public CurrentOutput(XPathNavigator navigator, GEMSSingle single)
            : base(single)
        {
            //Basic information
            positive = int.Parse(navigator.GetAttribute("positive", string.Empty));
        }

        public CurrentOutput(GEMSSingle single)
            : base(single)
        {

        }
       
        public override string BuildOuterXmlString()
        {
            StringBuilder coNodeBuilder = new StringBuilder();

            coNodeBuilder.AppendFormat("<CurrentOutput positive=\"{0}\" />", this.Positive);

            return coNodeBuilder.ToString();
        }

        public override SingleEO Clone(GEMSSingle parentSingle)
        {
            CurrentOutput co = new CurrentOutput(parentSingle);

            co.Positive = this.Positive;

            return co;
        }
    }
}
