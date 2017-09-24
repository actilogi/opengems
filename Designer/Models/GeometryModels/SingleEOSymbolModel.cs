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
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace GEMS.Designer.Models.GeometryModels
{
    public class SingleEOSymbolModel : LineModel
    {        
        private Vector3[] line1 = new Vector3[2];
        private Vector3[] line2 = new Vector3[2];

        private int line1Width;
        private int line2Width;

        private Color line1Color;
        private Color line2Color;

        private int positive;

        public SingleEOSymbolModel(int positive)
        {
            this.positive = positive;
        }

        public void Initialize()
        {
            Vector3 position2 = new Vector3();

           if (positive == 1) //it's positive , draw the line from start to end
            {
                position2 = node1 + (node2 - node1) * (1 - GeometryModel.DefaultEOLineLengthRatio);

                line1Width = GeometryModel.DefaultLineWidth * 2;
                line2Width = GeometryModel.DefaultLineWidth;
            }
            else   //it's inpositive , draw the line from to start
            {
                position2 = node1 + (node2 - node1) * GeometryModel.DefaultEOLineLengthRatio;

                line1Width = GeometryModel.DefaultLineWidth;
                line2Width = GeometryModel.DefaultLineWidth * 2;
            }

            line1[0] = this.node1;
            line1[1] = position2;

            line2[0] = position2;
            line2[1] = this.node2;
            
        }

        public new Color ModelColor
        {
            set {
                this.modelColor = value;

                if (positive == 1)
                {
                    line1Color = this.modelColor;
                    line2Color = Color.Black;
                }
                else
                {
                    line1Color = Color.Black;
                    line2Color = this.modelColor;
                }
            }
            get { return modelColor; }
        }
        
        public Vector3[] Line1
        {
            get { return line1; }
        }

        public int Line1Width
        {
            get { return line1Width; }
        }       

        public Vector3[] Line2
        {
            get { return line2; }
        }

        public int Line2Width
        {
            get { return line2Width; }
        }

        public Color Line2Color
        {
            get { return line2Color; }
        }

        public Color Line1Color
        {
            get { return line1Color; }
        }
    }
}
