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
    public class PointModel : GeometryModel, IOneDimensionEO
    {
        private Vector3 position;
  
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

     
        public override bool PositionRelation(Vector3 vertex)
        {
             //Have not implemented so far
            return false;
        }

        public List<MeshPointKey> MoveToMeshPoint(GEMSMesh mesh)
        {
            List<MeshPointKey> keys = new List<MeshPointKey>();

            if (mesh != null)
            {
                int indexX, indexY, indexZ;

                Vector3 meshPoint = mesh.GetNearestMeshPoint(this.position, out indexX, out indexY, out indexZ);

                keys.Add(MeshPointKey.Key(indexX, indexY, indexZ));
            }

            return keys;
        }
    }
}
