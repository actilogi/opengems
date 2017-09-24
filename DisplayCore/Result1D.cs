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

namespace GEMS.Display.Core
{
    public class Result1D : Result
    {
        private string valueLabel;

        private List<float> values = new List<float> ( );        

        public Result1D ( DataDomainType domain ,string keyLabel , string valueLabel )
            : base ( domain , keyLabel )
        {
            this.valueLabel = valueLabel;
        }

        
        public List<float> Values
        {
            get { return values; }
        }

        public string ValueLabel
        {
            get { return valueLabel; }
        }

        
    }
}
