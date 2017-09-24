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

using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

using GEMS.Designer.Models.GeometryModels;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;

namespace GEMS.Designer.Direct3D
{
    public class SingleEOSymbolRender : LineRender
    {
        private SingleEOSymbolModel model;

        public SingleEOSymbolRender(Direct3d d3d, GeometryModel source)
            : base(d3d, source)
        {
        }

        public override void Initialize ( )
        {
            model = source as SingleEOSymbolModel;
            base.Initialize();
        }

        protected override void PerformRender()
        {
            line.Antialias = true;

            d3d.Dx.Transform.World = model.WorldMatrix;

            line.Width = model.Line1Width;
            line.DrawTransform(model.Line1, d3d.View * d3d.Projection, model.Line1Color);

            line.Width = model.Line2Width;
            line.DrawTransform(model.Line2, d3d.View * d3d.Projection, model.Line2Color);     
        } 
    
    }
}
