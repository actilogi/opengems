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
    public class PointOutput : SingleOutput
    {
        private uint eFieldX = 0;
        private uint hFieldX = 0;
        private uint eFieldY = 0;
        private uint hFieldY = 0;
        private uint eFieldZ = 0;
        private uint hFieldZ = 0;

        public PointOutput(GEMSSingle single)
            : base(single)            
        {

        }

        public PointOutput(XPathNavigator navigator, GEMSSingle single)
            : base(single)
        {
            //Basic information
            navigator.MoveToChild("TimeDomain",String.Empty);
            uint fieldEnableFlag = uint.Parse ( navigator.GetAttribute ( "tdEnabled" , string.Empty ) );

            RetrieveEachDirectionFieldEnableFlag(fieldEnableFlag);          

        }


        public override string BuildOuterXmlString()
        {
            StringBuilder poNodeBuilder = new StringBuilder();

            poNodeBuilder.Append("<FieldOnPointOutput>");
            poNodeBuilder.AppendFormat("<TimeDomain tdEnabled=\"{0}\" >",this.GetFieldEnableFlag());
            poNodeBuilder.Append("</TimeDomain>");
            poNodeBuilder.Append("</FieldOnPointOutput>");

            return poNodeBuilder.ToString();
        }

        public override SingleEO Clone(GEMSSingle parentSingle)
        {
            PointOutput po = new PointOutput(parentSingle);

            return po;
        }

        /// <summary>
        /// This method used to build a long value contain all the field flag
        /// 0x1 -- E Field along X, +
        /// 0x2 -- E Field along X, -
        /// 0x4 -- H Field along X, +
        /// 0x8 -- H Field along X, -
        /// 0x100 -- E Field along Y, +
        /// 0x200 -- E Field along Y, -
        /// 0x400 -- H Field along Y, +
        /// 0x800 -- H Field along Y, -
        /// 0x10000 -- E Field along Z, +
        /// 0x20000 -- E Field along Z, -
        /// 0x40000 -- H Field along Z, +
        /// 0x80000 -- H Field along Z, +
        /// </summary>
        public uint GetFieldEnableFlag ( )
        {
            uint fieldEnableFlag = 0x00;

            fieldEnableFlag |= eFieldX;

            fieldEnableFlag |= hFieldX << 2;

            fieldEnableFlag |= eFieldY << 8;

            fieldEnableFlag |= hFieldY << 10;

            fieldEnableFlag |= eFieldZ << 16;

            fieldEnableFlag |= hFieldZ << 18;

            return fieldEnableFlag;
        }

        public void RetrieveEachDirectionFieldEnableFlag ( uint fieldEnableFlag )
        {
            eFieldX = fieldEnableFlag & 0x3;
            hFieldX = (fieldEnableFlag >> 2)& 0x3;
            eFieldY = (fieldEnableFlag >> 8) & 0x3;
            hFieldY = (fieldEnableFlag >> 10) & 0x3;
            eFieldZ = (fieldEnableFlag >> 16) & 0x3;
            hFieldZ = (fieldEnableFlag >> 18) & 0x3;
        }

        public uint EFieldX
        {
            get { return eFieldX; }
            set { eFieldX = value; }
        }

        public uint HFieldX
        {
            get { return hFieldX; }
            set { hFieldX = value; }
        }

        public uint EFieldY
        {
            get { return eFieldY; }
            set { eFieldY = value; }
        }

        public uint HFieldY
        {
            get { return hFieldY; }
            set { hFieldY = value; }
        }

        public uint EFieldZ
        {
            get { return eFieldZ; }
            set { eFieldZ = value; }
        }

        public uint HFieldZ
        {
            get { return hFieldZ; }
            set { hFieldZ = value; }
        }
    }
}
