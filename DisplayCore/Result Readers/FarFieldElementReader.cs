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

namespace GEMS.Display.Core
{
    public class FarFieldElementReader : OutputReader
    {
        public FarFieldElementReader ( string fileName , string value , string key , string path , string name )
            : base ( fileName , value , key , path , name )
        {

        }

        /// <summary>
        /// Read the result from xml file
        /// </summary>
        public override Result Load ()
        {
            XPathDocument document = new XPathDocument ( outputResultFile );
            XPathNavigator navigator = document.CreateNavigator ( );
            XPathNodeIterator nodes = navigator.Select( xPath );

            Result1D result = new Result1D ( DataDomainType.FrequencyDomain,keyAttribute , name );
            while (nodes.MoveNext ( ))
            {
                //Value
                XPathNavigator elementNode = nodes.Current.Clone ( );
                float value = float.Parse ( elementNode.GetAttribute (valueAttribute , string.Empty ) );
                result.Values.Add ( value );

                //Key
                elementNode.MoveToParent ( );
                float key = float.Parse ( elementNode.GetAttribute ( keyAttribute , string.Empty ) );
                result.Keys.Add ( key );
                
            }

            return result;
        }
    }


}
