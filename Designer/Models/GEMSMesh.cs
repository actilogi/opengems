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
using System.IO;
using System.Xml.XPath;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Direct3D;
using GEMS.Designer;

namespace GEMS.Designer.Models
{
    #region MeshPointKey Class Defination Code

    public class MeshPointKey : IEqualityComparer<MeshPointKey>
    {
        public int X;
        public int Y;
        public int Z;

        public static MeshPointKey Key ( int i , int j , int k )
        {
            MeshPointKey key = new MeshPointKey ( );

            key.X = i;
            key.Y = j;
            key.Z = k;

            return key;
        }

        #region IEqualityComparer<MeshPointKey> Members

        public bool Equals ( MeshPointKey x , MeshPointKey y )
        {
            return x.X == y.X && x.Y == y.Y && x.Z == y.Z;
        }

        public int GetHashCode ( MeshPointKey obj )
        {
            return base.GetHashCode ( );
        }

        #endregion

        public void Write ( BinaryWriter bw )
        {
            bw.Write ( X );
            bw.Write ( Y );
            bw.Write ( Z );
        }

        public MeshPointKey Clone ( )
        {
            return MeshPointKey.Key ( this.X , this.Y , this.Z );
        }
    }
    #endregion

    #region MeshPointMaterial Class Defination Code

    public class MeshPointMaterial
    {
        private Vector3 meshPoint;
        private Vector3 mtrEpsilon;
        private Vector3 mtrSigma;

        public MeshPointMaterial ( Vector3 meshPoint )
        {
            this.meshPoint = meshPoint;
        }

        public Vector3 MeshPoint
        {
            get { return meshPoint; }
            set { meshPoint = value; }
        }

        public Vector3 MaterialSigma
        {
            get { return mtrSigma; }
            set { mtrSigma = value; }
        }

        public Vector3 MaterialEpsilon
        {
            get { return mtrEpsilon; }
            set { mtrEpsilon = value; }
        }
    }

    #endregion



    public class GEMSMesh
    {

        private GEMSProject parent;

        private List<float> meshPointsInX = new List<float> ( );
        private List<float> meshPointsInY = new List<float> ( );
        private List<float> meshPointsInZ = new List<float> ( );

        private List<GEMSKeyPoint> keyPointsInX = new List<GEMSKeyPoint> ( );
        private List<GEMSKeyPoint> keyPointsInY = new List<GEMSKeyPoint> ( );
        private List<GEMSKeyPoint> keyPointsInZ = new List<GEMSKeyPoint> ( );

        private Vector3 minMeshCell = Vector3.Empty;
        private Vector3 maxMeshCell = Vector3.Empty;

        public GEMSMesh ( GEMSProject parent )
        {
            this.parent = parent;
        }

        public GEMSMesh ( XPathNavigator navigator , GEMSProject parent )
        {
            this.parent = parent;

            //Read the key points in three axises
            ReadKeyPointsInOneAxis ( Axis.X , navigator , keyPointsInX );
            ReadKeyPointsInOneAxis ( Axis.Y , navigator , keyPointsInY );
            ReadKeyPointsInOneAxis ( Axis.Z , navigator , keyPointsInZ );

            //Read the mesh points in three axises
            ReadMeshPointsInOneAxis ( Axis.X , navigator , meshPointsInX );
            ReadMeshPointsInOneAxis ( Axis.Y , navigator , meshPointsInY );
            ReadMeshPointsInOneAxis ( Axis.Z , navigator , meshPointsInZ );

            MinAndMaxMeshSizeInOneAxis ( meshPointsInX , ref minMeshCell.X , ref maxMeshCell.X );
            MinAndMaxMeshSizeInOneAxis ( meshPointsInY , ref  minMeshCell.Y , ref maxMeshCell.Y );
            MinAndMaxMeshSizeInOneAxis ( meshPointsInZ , ref minMeshCell.Z , ref maxMeshCell.Z );

        }

        #region Intialize and Save Methods

        /// <summary>
        /// Read the mesh points in one axis from xml file
        /// </summary>
        private void ReadMeshPointsInOneAxis ( Axis axis , XPathNavigator navigator , List<float> meshPoints )
        {
            //Find this direction
            XPathNavigator directionNode = navigator.SelectSingleNode ( "/Document/MeshPoints[@direction='" + axis.ToString ( ).ToUpper ( ) + "']" );

            if (directionNode != null)
            {
                //Get the mesh points
                XPathNodeIterator nodes = directionNode.SelectChildren ( "MeshPoint" , string.Empty );
                while (nodes.MoveNext ( ))
                {
                    XPathNavigator meshPointNavigator = nodes.Current.Clone ( );
                    float meshPoint = float.Parse ( meshPointNavigator.GetAttribute ( "value" , string.Empty ) );
                    meshPoints.Add ( meshPoint );
                }
            }
        }

        /// <summary>
        /// Read the key points in one axis from xml file
        /// </summary>
        private void ReadKeyPointsInOneAxis ( Axis axis , XPathNavigator navigator , List<GEMSKeyPoint> keyPoints )
        {
            //Find this direction
            XPathNavigator directionNode = navigator.SelectSingleNode ( "/Document/KeyPoints[@direction='" + axis.ToString ( ).ToUpper ( ) + "']" );

            if (directionNode == null)
                return;

            //Get the key points
            XPathNodeIterator nodes = directionNode.SelectChildren ( "KeyPoint" , string.Empty );
            while (nodes.MoveNext ( ))
            {
                XPathNavigator keyPointNavigator = nodes.Current.Clone ( );

                GEMSKeyPoint keyPoint = new GEMSKeyPoint ( keyPointNavigator );

                keyPoints.Add ( keyPoint );
            }
        }


        /// <summary>
        /// Build a xml string containing the information of all mesh points
        /// </summary>
        public string BuildOuterXmlString ( )
        {
            StringBuilder meshNodeBuilder = new StringBuilder ( );

            meshNodeBuilder.Append ( BuildKeyPointsOuterXmlStringInOneAxis ( Axis.X , keyPointsInX ) );
            meshNodeBuilder.Append ( BuildKeyPointsOuterXmlStringInOneAxis ( Axis.Y , keyPointsInY ) );
            meshNodeBuilder.Append ( BuildKeyPointsOuterXmlStringInOneAxis ( Axis.Z , keyPointsInZ ) );

            meshNodeBuilder.Append ( BuildMeshPointsOuterXmlStringInOneAxis ( Axis.X , meshPointsInX ) );
            meshNodeBuilder.Append ( BuildMeshPointsOuterXmlStringInOneAxis ( Axis.Y , meshPointsInY ) );
            meshNodeBuilder.Append ( BuildMeshPointsOuterXmlStringInOneAxis ( Axis.Z , meshPointsInZ ) );

            return meshNodeBuilder.ToString ( );
        }


        /// <summary>
        /// Build a xml string containing mesh points in one axis
        /// </summary>
        /// <param name="axis">Axis need to build</param>
        private string BuildMeshPointsOuterXmlStringInOneAxis ( Axis axis , List<float> meshPoints )
        {
            StringBuilder oneAxisBuilder = new StringBuilder ( );

            oneAxisBuilder.AppendFormat ( "<MeshPoints direction=\"{0}\" >" , axis.ToString ( ).ToUpper ( ) );
            foreach (float meshPoint in meshPoints)
            {
                oneAxisBuilder.AppendFormat ( "<MeshPoint value=\"{0}\" />" , meshPoint );
            }
            oneAxisBuilder.Append ( "</MeshPoints>" );

            return oneAxisBuilder.ToString ( );
        }

        /// <summary>
        /// Build a xml string containing key points in one axis
        /// </summary>
        /// <param name="axis">Axis need to build</param>
        private string BuildKeyPointsOuterXmlStringInOneAxis ( Axis axis , List<GEMSKeyPoint> keyPoints )
        {
            StringBuilder oneAxisBuilder = new StringBuilder ( );

            oneAxisBuilder.AppendFormat ( "<KeyPoints direction=\"{0}\" >" , axis.ToString ( ).ToUpper ( ) );
            foreach (GEMSKeyPoint keyPoint in keyPoints)
            {
                oneAxisBuilder.Append ( keyPoint.BuildOuterXmlString ( ) );
            }
            oneAxisBuilder.Append ( "</KeyPoints>" );

            return oneAxisBuilder.ToString ( );
        }

        #endregion

        #region Simulation Relation Methods

        /// <summary>
        /// Generate the mesh points with the key points
        /// </summary>
        public void GenerateMeshPoints ( float xMeshSize , float yMeshSize , float zMeshSize )
        {
            //Generate the key points
            GenerateKeyPoints ( parent.ComputationalDomain , xMeshSize , yMeshSize , zMeshSize );

            //Generate the mesh points
            GenerateMeshPointsInOneAxis ( meshPointsInX , keyPointsInX );
            GenerateMeshPointsInOneAxis ( meshPointsInY , keyPointsInY );
            GenerateMeshPointsInOneAxis ( meshPointsInZ , keyPointsInZ );

            //Find the min and max cell in each direction
            MinAndMaxMeshSizeInOneAxis ( meshPointsInX , ref minMeshCell.X , ref maxMeshCell.X );
            MinAndMaxMeshSizeInOneAxis ( meshPointsInY , ref  minMeshCell.Y , ref maxMeshCell.Y );
            MinAndMaxMeshSizeInOneAxis ( meshPointsInZ , ref minMeshCell.Z , ref maxMeshCell.Z );
        }

        /// <summary>
        /// Generate the key points with the computational domain information.
        /// There's always only two keypoints in each axis in this version
        /// </summary>
        private void GenerateKeyPoints ( GEMSComputationalDomain domain , float xMeshSize , float yMeshSize , float zMeshSize )
        {
            keyPointsInX.Clear ( );
            keyPointsInY.Clear ( );
            keyPointsInZ.Clear ( );

            //X Axis
            float length = domain.MaxX - domain.MinX;
            GEMSKeyPoint keyPoint = new GEMSKeyPoint ( domain.MinX , xMeshSize , length );
            keyPointsInX.Add ( keyPoint );
            keyPoint = new GEMSKeyPoint ( domain.MaxX , xMeshSize , length );
            keyPointsInX.Add ( keyPoint );

            //Y Axis
            length = domain.MaxY - domain.MinY;
            keyPoint = new GEMSKeyPoint ( domain.MinY , yMeshSize , length );
            keyPointsInY.Add ( keyPoint );
            keyPoint = new GEMSKeyPoint ( domain.MaxY , yMeshSize , length );
            keyPointsInY.Add ( keyPoint );

            //Z Axis
            length = domain.MaxZ - domain.MinZ;
            keyPoint = new GEMSKeyPoint ( domain.MinZ , zMeshSize , length );
            keyPointsInZ.Add ( keyPoint );
            keyPoint = new GEMSKeyPoint ( domain.MaxZ , zMeshSize , length );
            keyPointsInZ.Add ( keyPoint );
        }

        public bool IsParallelable ( )
        {
            return ( MeshCountInX > 0 && MeshCountInY > 0 && MeshCountInZ > 0 );
        }

        public Vector3 GetNearestMeshPoint ( Vector3 targetVector , out int indexX , out int indexY , out int indexZ )
        {
            float nearestX = FindNearestMeshPointInOneAxis ( meshPointsInX , targetVector.X , out indexX );
            float nearestY = FindNearestMeshPointInOneAxis ( meshPointsInY , targetVector.Y , out indexY );
            float nearestZ = FindNearestMeshPointInOneAxis ( meshPointsInZ , targetVector.Z , out indexZ );

            return new Vector3 ( nearestX , nearestY , nearestZ );
        }

        /// <summary>
        /// Clear all the mesh points in each direction
        /// </summary>
        public void Reset ( )
        {
            minMeshCell.X = ( parent.ComputationalDomain.MaxX - parent.ComputationalDomain.MinX ) / 50;
            minMeshCell.Y = ( parent.ComputationalDomain.MaxY - parent.ComputationalDomain.MinY ) / 50;
            minMeshCell.Z = ( parent.ComputationalDomain.MaxZ - parent.ComputationalDomain.MinZ ) / 50;

            meshPointsInX.Clear ( );
            meshPointsInY.Clear ( );
            meshPointsInZ.Clear ( );
        }

        /// <summary>
        /// Generate the mesh points in one axis with the key points of it
        /// </summary>
        /// <returns>The min mesh size</returns>
        private void GenerateMeshPointsInOneAxis ( List<float> meshPoints , List<GEMSKeyPoint> keyPoints )
        {
            meshPoints.Clear ( );

            if (keyPoints.Count != 2)
                return;

            float min = keyPoints[0].KeyPoint;
            float max = keyPoints[1].KeyPoint;
            float range = keyPoints[0].Max;
            float cellSize = keyPoints[0].Min;

            if (range > 0.0f)
            {
                int cellCount = (int)( range / cellSize );
                float offset = range - cellCount * cellSize;

                float interval = cellSize;
                if (offset > 0.0)
                {
                    float cellOffset = offset / cellCount;
                    interval += cellOffset;
                }
                //Error code
                while (min < max)
                {
                    meshPoints.Add ( min );
                    min = min + interval;
                }
                if (min - max < 10e-6f)
                    meshPoints.Add ( max );
                else
                    meshPoints[meshPoints.Count - 1] = max;

            }
        }

        private float FindNearestMeshPointInOneAxis ( List<float> meshPoints , float targetValue, out int index )
        {
            index = 0;
            float nearest = 0.0f;
            float minDist;
            minDist = Math.Abs ( targetValue - meshPoints[0] );
            for (int i = 1 ; i < meshPoints.Count ; i++)
            {
                if (minDist > Math.Abs ( targetValue - meshPoints[i] ))
                {
                    index = i;
                    nearest = meshPoints[i];
                    minDist = Math.Abs ( targetValue - meshPoints[i] );
                }
            }
            return nearest;
        }

        private void MinAndMaxMeshSizeInOneAxis ( List<float> meshPoints , ref float min, ref float max )
        {
            if (meshPoints.Count >= 2)
            {
                min = (float)Math.Abs ( meshPoints[0] - meshPoints[1] );
                max = (float)Math.Abs ( meshPoints[0] - meshPoints[1] );
                for (int index = 1 ; index <= meshPoints.Count - 2 ; index++)
                {
                    float meshsize = (float)Math.Abs ( meshPoints[index] - meshPoints[index + 1] );

                    if (min > meshsize)
                        min = meshsize;

                    if (max < meshsize)
                        max = meshsize;
                }
            }
        }


        #endregion

        #region Public Propteries

        public GEMSProject Parent
        {
            get { return parent; }
        }

        public int MeshCountInX
        {
            get
            {
                if (meshPointsInX.Count == 0)
                    return 0;
                else
                    return meshPointsInX.Count - 1;
            }
        }

        public int MeshCountInY
        {
            get
            {
                if (meshPointsInY.Count == 0)
                    return 0;
                else
                    return meshPointsInY.Count - 1;
            }
        }

        public int MeshCountInZ
        {
            get
            {
                if (meshPointsInZ.Count == 0)
                    return 0;
                else
                    return meshPointsInZ.Count - 1;
            }
        }

        public Vector3 MinMeshCell
        {
            get { return minMeshCell; }
        }

        public Vector3 MaxMeshCell
        {
            get { return maxMeshCell; }
        }

        public List<float> MeshPointsInX
        {
            get { return meshPointsInX; }
        }

        public List<float> MeshPointsInY
        {
            get { return meshPointsInY; }
        }

        public List<float> MeshPointsInZ
        {
            get { return meshPointsInZ; }
        }

        #endregion




    }
}
