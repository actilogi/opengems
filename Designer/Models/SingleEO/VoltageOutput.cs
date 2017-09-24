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
    public class VoltageOutput : SingleOutput
    {

        public VoltageOutput(XPathNavigator navigator, GEMSSingle single)
            : base(single)
        {
            //Basic information
            positive = int.Parse(navigator.GetAttribute("positive", string.Empty));
 
        }

        public VoltageOutput(GEMSSingle single)
            : base(single)            
        {

        }
  

        public override string BuildOuterXmlString()
        {
            StringBuilder voNodeBuilder = new StringBuilder();

            voNodeBuilder.AppendFormat("<VoltageOutput positive=\"{0}\" />", this.Positive);

            return voNodeBuilder.ToString();
        }

        public override SingleEO Clone(GEMSSingle parentSingle)
        {
            VoltageOutput vo = new VoltageOutput(parentSingle);

            vo.Positive = this.Positive;

            return vo;
        }
    }
}
