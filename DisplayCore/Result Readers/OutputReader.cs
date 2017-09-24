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
    public abstract class OutputReader
    {
        protected string name = string.Empty;

        /// <summary>
        /// File of result
        /// </summary>
        protected string outputResultFile = string.Empty;

        /// <summary>
        /// The attribute name of Xml Node needed to read , which will be the value in the KeyValuePair
        /// </summary>
        protected string valueAttribute = string.Empty;    
              
        /// <summary>
        /// //The attribute name of Xml Node needed to read, which will be the key in the KeyValuePair
        /// </summary>                                                      
        protected string keyAttribute = string.Empty;      

        /// <summary>
        ///XPath of Xml Node containing the value attribute,
        /// which is the child of the Xml Node containing the key attribute
        /// </summary>
        protected string xPath = string.Empty;            

        public OutputReader (string fileName, string value , string key , string path , string name)
        {
            this.name = name;
            this.outputResultFile = fileName;
            this.valueAttribute = value;
            this.keyAttribute = key;
            this.xPath = path;
        }

        #region  Public Properties

        public string KeyAttribute
        {
            get { return keyAttribute; }
        }

        public string ValueAttribute
        {
            get { return valueAttribute; }
        }

        public string XPath
        {
            get { return xPath; }
        }

        public string Name
        {
            get { return name; }
        }

        public string OutputResultFile
        {
            get { return outputResultFile; }
        }

        #endregion

        /// <summary>
        /// Load the data from xmlFile
        /// </summary>
        public abstract Result Load ();
    }   
}
