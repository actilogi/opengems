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
    public abstract class SingleExcitation : SingleEO
    {
        private Time timeDelay = Time.Default;

        private float phaseDelay = 0.0f;
        private Frequency phaseFrequency = Frequency.Default;

        private const int hard = 0;

        public SingleExcitation(XPathNavigator navigator, GEMSSingle single)
            : base(single)
        {
            //Basic information
            positive = int.Parse(navigator.GetAttribute("positive", string.Empty));

            //TimeDelay information
            navigator.MoveToChild("TimeDelay", string.Empty);
            timeDelay = new Time(navigator.GetAttribute("value", string.Empty),navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();
 
            //PhaseDelay information
            navigator.MoveToChild("PhaseDelay", string.Empty);
            phaseDelay = float.Parse(navigator.GetAttribute("value", string.Empty));

            //Frequency information
            navigator.MoveToFirstChild();
            phaseFrequency = new Frequency(navigator.GetAttribute("value", string.Empty),navigator.GetAttribute("unit", string.Empty));
            navigator.MoveToParent();
            navigator.MoveToParent();       

        }

        public SingleExcitation(GEMSSingle signal)
            : base(signal)
        {

        }
   
        #region Properties

        public Time TimeDelay
        {
            get { return timeDelay; }
            set { timeDelay = value; }
        }

        public Frequency PhaseFrequency
        {
            get { return phaseFrequency; }
            set { phaseFrequency = value; }
        }

        public float PhaseDelay
        {
            get { return phaseDelay; }
            set { phaseDelay = value; }
        }

        #endregion
    }
}
