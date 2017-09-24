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
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models;
using System.Drawing;

namespace GEMS.Designer.Direct3D
{
    public class DomainRender : BoundingBoxRender
    {
        GEMSComputationalDomain domain = null;        

        public DomainRender(Direct3d d3d,GEMSComputationalDomain domain):base(d3d,Vector3.Empty,Vector3.Empty)
        {
            this.domain = domain;

            if (domain != null)            
            {
                OnGEMSComputationalDomainDataChanged(null,null);

                domain.GEMSComputationalDomain_DataChanged += new GEMSComputationalDomain.GEMSComputationalDomain_DataChangedEventHandler(OnGEMSComputationalDomainDataChanged);

            }
        }

        void OnGEMSComputationalDomainDataChanged(object sender, EventArgs e)
        {
            base.minVector3 = new Vector3(domain.MinX, domain.MinY, domain.MinZ);
            base.maxVector3 = new Vector3(domain.MaxX, domain.MaxY, domain.MaxZ);

            base.Initialize();
        }

        protected override void PerformRender()
        {
            if (domain != null)
            {
                base.PerformRender();
            }
        }

        protected override void PerformDispose ( )
        {
            domain.GEMSComputationalDomain_DataChanged -= new GEMSComputationalDomain.GEMSComputationalDomain_DataChangedEventHandler ( OnGEMSComputationalDomainDataChanged );

            base.PerformDispose ( );
        }
      
    }
}
