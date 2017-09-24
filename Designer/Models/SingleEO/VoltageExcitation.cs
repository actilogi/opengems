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
    public class VoltageExcitation : SingleExcitation
    {
        private Voltage excitationVoltage = Voltage.Default;


        public VoltageExcitation(XPathNavigator navigator, GEMSSingle single)
            : base(navigator, single)
        {
            //Excitation Value information
            navigator.MoveToChild("Voltage", string.Empty);
            excitationVoltage = new Voltage(navigator.GetAttribute("value", string.Empty),navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

        }

        public VoltageExcitation(GEMSSingle single)
            : base(single)
        {

        }

        public Voltage ExcitationVoltage
        {
            get { return excitationVoltage; }
            set { excitationVoltage = value; }
        }

        public override string BuildOuterXmlString()
        {
            StringBuilder veNodeBuilder = new StringBuilder();

            veNodeBuilder.AppendFormat("<VoltageExcitation positive=\"{0}\" hard=\"0\" >", this.Positive);
            veNodeBuilder.AppendFormat("<TimeDelay unit=\"{0}\" value=\"{1}\" />", (int)this.TimeDelay.Unit, this.TimeDelay.Value);
            veNodeBuilder.AppendFormat("<PhaseDelay value=\"{0}\" >", this.PhaseDelay);
            veNodeBuilder.AppendFormat("<Frequency unit=\"{0}\" value=\"{1}\" />", (int)this.PhaseFrequency.Unit, this.PhaseFrequency.Value);
            veNodeBuilder.Append("</PhaseDelay>");
            veNodeBuilder.AppendFormat("<Voltage unit=\"{0}\" value=\"{1}\" />", (int)this.ExcitationVoltage.Unit, this.ExcitationVoltage.Value);
            veNodeBuilder.Append("</VoltageExcitation>");

            return veNodeBuilder.ToString();
        }


        public override SingleEO Clone(GEMSSingle parentSingle)
        {
            VoltageExcitation ve = new VoltageExcitation(parentSingle);

            ve.TimeDelay = new Time(this.TimeDelay.Value, this.TimeDelay.Unit);
            ve.Positive = this.Positive;
            ve.PhaseFrequency = new Frequency(this.PhaseFrequency.Value, this.PhaseFrequency.Unit);
            ve.PhaseDelay = this.PhaseDelay;
            ve.ExcitationVoltage = new Voltage(this.ExcitationVoltage.Value, this.ExcitationVoltage.Unit);

            return ve;
        }
    }
}
