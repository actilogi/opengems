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
    public class GEMSExcitationSource
    {
        private GEMSProject parent;

        private PluseType sourcePluseType;
        private int lossness;
        private Frequency maxFrequency;

        /// <summary>
        /// Create a new excitation object and load the information from the specified file
        /// </summary>
        /// <param name="navigator"></param>
        /// <returns></returns>
        public GEMSExcitationSource(XPathNavigator navigator, GEMSProject parent)
        {
            this.parent = parent;

            navigator.MoveToChild("Pulse", "");
            sourcePluseType = (PluseType)Enum.Parse(typeof(PluseType), navigator.GetAttribute("kind", ""));

            if (sourcePluseType != PluseType.None)
            {
                navigator.MoveToFirstChild();
                lossness = int.Parse(navigator.GetAttribute("lossness", ""));
                navigator.MoveToParent();
            }
            navigator.MoveToParent();

            navigator.MoveToChild("Max", "");
            MaxFrequency = new Frequency(navigator.GetAttribute("value", ""),navigator.GetAttribute("unit", ""));
            navigator.MoveToParent();
       }        
        
        /// <summary>
        /// Create a new excitation object with default values
        /// </summary>
        public GEMSExcitationSource(GEMSProject parent)
        {
            this.parent = parent;
            
            this.sourcePluseType = PluseType.Gaussian;
            this.lossness = 0;
            this.maxFrequency = new Frequency ( 1 , Frequency.FrequencyUnit.GHz );
        }

        /// <summary>
        /// Build a xml string containing the information of the object
        /// </summary>
        public string BuildOuterXmlString()
        {
            StringBuilder ecNodeBuilder = new StringBuilder();

            ecNodeBuilder.Append("<Frequency>");

            if(sourcePluseType == PluseType.None)
                ecNodeBuilder.Append("<Pulse kind=\"-1\" />");
            else
            {
                ecNodeBuilder.AppendFormat("<Pulse kind=\"{0}\" >",(int)this.sourcePluseType);
                ecNodeBuilder.AppendFormat("<{0} lossness=\"{1}\" />", this.sourcePluseType.ToString(), this.lossness);
                ecNodeBuilder.Append("</Pulse>");
            }

            ecNodeBuilder.Append("<Min unit=\"3\" value=\"0\" />");
            ecNodeBuilder.AppendFormat("<Max unit=\"{0}\" value=\"{1}\" />",(int)this.maxFrequency.Unit,this.maxFrequency.Value);
            ecNodeBuilder.Append("</Frequency>");

            return ecNodeBuilder.ToString();
        }

        public GEMSExcitationSource Clone()
        {
            GEMSExcitationSource newSource = new GEMSExcitationSource(this.parent);

            newSource.lossness = this.lossness;
            newSource.maxFrequency = new Frequency(this.maxFrequency.Value,this.maxFrequency.Unit);
            newSource.sourcePluseType = this.sourcePluseType;

            return newSource;
        }


        #region Properties

        public PluseType SourcePluseType
        {
            get
            {
                return sourcePluseType;
            }
            set
            {
                sourcePluseType = value;
            }
        }

        public int Lossness
        {
            get
            {
                return lossness;
            }
            set
            {
                lossness = value;
            }
        }

        public Frequency MaxFrequency
        {
            get
            {
                return maxFrequency;
            }
            set
            {
                maxFrequency = value;
            }
        }

        public GEMSProject Parent
        {
            get { return parent; }
        }

        #endregion
    }

}
