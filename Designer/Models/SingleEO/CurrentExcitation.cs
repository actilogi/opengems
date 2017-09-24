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
    public class CurrentExcitation : SingleExcitation
    {
        private Current excitationCurrent = Current.Default;


        public CurrentExcitation(XPathNavigator navigator, GEMSSingle single)
            : base(navigator, single)
        {
            //Excitation Value information
            navigator.MoveToChild("Current", string.Empty);
            excitationCurrent = new Current(navigator.GetAttribute("value", string.Empty), navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();

        }

        public CurrentExcitation(GEMSSingle single)
            : base(single)
        {

        }

        public Current ExcitationCurrent
        {
            get { return excitationCurrent; }
            set { excitationCurrent = value; }
        }

        #region Parent Class's Abstarct Methods

        public override string BuildOuterXmlString()
        {
            StringBuilder ceNodeBuilder = new StringBuilder();

            ceNodeBuilder.AppendFormat("<CurrentExcitation positive=\"{0}\" hard=\"0\" >", this.Positive);
            ceNodeBuilder.AppendFormat("<TimeDelay unit=\"{0}\" value=\"{1}\" />", (int)this.TimeDelay.Unit, this.TimeDelay.Value);
            ceNodeBuilder.AppendFormat("<PhaseDelay value=\"{0}\" >", this.PhaseDelay);
            ceNodeBuilder.AppendFormat("<Frequency unit=\"{0}\" value=\"{1}\" />", (int)this.PhaseFrequency.Unit, this.PhaseFrequency.Value);
            ceNodeBuilder.Append("</PhaseDelay>");
            ceNodeBuilder.AppendFormat("<Current unit=\"{0}\" value=\"{1}\" />", (int)this.ExcitationCurrent.Unit, this.ExcitationCurrent.Value);
            ceNodeBuilder.Append("</CurrentExcitation>");

            return ceNodeBuilder.ToString();
        }

        public override SingleEO Clone(GEMSSingle parentSingle)
        {
            CurrentExcitation ce = new CurrentExcitation(parentSingle);

            ce.TimeDelay = new Time(this.TimeDelay.Value, this.TimeDelay.Unit);
            ce.Positive = this.Positive;
            ce.PhaseFrequency = new Frequency(this.PhaseFrequency.Value, this.PhaseFrequency.Unit);
            ce.PhaseDelay = this.PhaseDelay;
            ce.ExcitationCurrent = new Current(this.ExcitationCurrent.Value, this.ExcitationCurrent.Unit);

            return ce;
        }
        
        #endregion
    }
}
