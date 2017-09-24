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
    public class Result2D : Result
    {
        private string valueLabel1;
        private string valueLabel2;

        private List<float> values1 = new List<float> ( );
        private List<float> values2 = new List<float> ( );

        public Result2D ( DataDomainType domain , string keyLabel , string valueLabel1 , string valueLabel2 )
            : base ( domain , keyLabel )
        {
            this.valueLabel1 = valueLabel1;
            this.valueLabel2 = valueLabel2;
        }
        
        public List<float> Values1
        {
            get { return values1; }
        }

        public List<float> Values2
        {
            get { return values2; }
        }

        public string ValueLabel1
        {
            get { return valueLabel1; }
        }

        public string ValueLabel2
        {
            get { return valueLabel2; }
        }
    }
}
