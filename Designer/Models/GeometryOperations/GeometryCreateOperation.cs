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
using GEMS.Designer.Models;
using GEMS.Designer.Models.GeometryModels;

namespace GEMS.Designer.Models.GeometryOperations
{
    public abstract class GeometryCreateOperation : GeometryOperation
    {
        public GeometryCreateOperation(XPathNavigator navigator, GEMSSingle parent)
           :base(navigator,parent)
        {

        }

        public GeometryCreateOperation(int id, GEMSSingle parent)
            :base(id, parent)
        {

        }

        public GeometryCreateOperation(int id) : base(id)
        {
 
        }

        public abstract GeometryModel CreateSourceGeometryModel();
    }
}
