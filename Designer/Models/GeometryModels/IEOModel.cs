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

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace GEMS.Designer.Models.GeometryModels
{
    /// <summary>
    /// Only the child of GeometryModel will implemente this interface.
    /// The model class implemented this interface means 
    /// that the relationed GEMSSingle object could be setted as Excitation or Output,
    /// so it need to create a SingleEOSymbolModel object , and the EOStart and EOEnd property 
    /// of this interface are necessary for creating the SingleEOSymbolModel object.
    /// </summary>
    public interface IEOModel
    {
        Vector3 EOStart
        {
            get;
        }

        Vector3 EOEnd
        {
            get;
        }
    }

    public interface IOneDimensionEO
    {
        List<MeshPointKey> MoveToMeshPoint(GEMSMesh mesh);
    }

    public interface ITwoDimensionEO
    {
        Axis AlineAxis
        {
            get;
        }

        Vector3 CenterVector3
        {
            get;
        }

        Vector3 MinVector3
        {
            get;
        }

        Vector3 MaxVector3
        {
            get;
        }

        bool PositionRelationOneXYPlane(Vector3 vertex);
    }
}
