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
using GEMS.Designer.Models;

namespace GEMS.Designer.Models.GeometryModels
{
    public class LineModel : GeometryModel , IEOModel , IOneDimensionEO
    {
        protected Vector3 node1;
        protected Vector3 node2;

        public Vector3 Node1
        {
            get { return node1; }
            set { node1 = value; }
        }

        public Vector3 Node2
        {
            get { return node2; }
            set { node2 = value; }
        }

        public Vector3 EOStart
        {
            get { return node1; }
        }

        public Vector3 EOEnd
        {
            get { return node2; }
        }


        public override bool PositionRelation ( Vector3 vertex )
        {
            return PositionRelation(vertex,node1,node2);
        }

        public bool PositionRelation ( Vector3 vertex ,Vector3 startNode,Vector3 endNode)
        {
            //Ensure the vertex is between the startNode and endNode
            Vector3 lineDir1 = Vector3.Normalize(vertex - startNode);
            Vector3 lineDir2 = Vector3.Normalize(endNode - vertex);

            if (lineDir1.Length() == 0 || lineDir2.Length() == 0) // vertex is startNode or endNode
                return true;
            else if (1 - Vector3.Dot ( lineDir1 , lineDir2 ) <= GeometryModel.DefaultDistanceDelta)
                return true;
            else
                return false;            
        }

        public List<MeshPointKey> MoveToMeshPoint ( GEMSMesh mesh )
        {
            List<MeshPointKey> keys = new List<MeshPointKey> ( );

            if (mesh != null)
            {
                int startX , startY , startZ;
                int endX , endY , endZ;

                Vector3 meshPoint1 = mesh.GetNearestMeshPoint ( this.node1 , out startX , out startY , out startZ );
                Vector3 meshPoint2 = mesh.GetNearestMeshPoint ( this.node2 , out endX , out endY , out endZ );

                keys.Add ( MeshPointKey.Key ( startX , startY , startZ ) );
                keys.Add ( MeshPointKey.Key ( endX , endY , endZ ) );
            }

            return keys;
        }
        
    }
}
