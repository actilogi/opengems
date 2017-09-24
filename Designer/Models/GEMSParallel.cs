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

using GEMS.Designer;

namespace GEMS.Designer.Models
{
    public class GEMSParallel 
    {
        private GEMSProject parent;
        
        private List<int> divisionInX = new List<int>();
        private List<int> divisionInY = new List<int>();
        private List<int> divisionInZ = new List<int>();

        private int meshCountInX;
        private int meshCountInY;
        private int meshCountInZ;

        private List<GEMSParallelArea> areaList = new List<GEMSParallelArea>();

        private MeshPointKey startPecPoint;
        private MeshPointKey endPecPoint;

        //Mininum mesh count in each division
        private const int minMeshCount = 2;

        private int minAreaMeshCount = -1;
        private int maxAreaMeshCount = -1;

        #region Delegate and Event

        public delegate void GEMSParallel_DataChangedEventHandler(object sender, EventArgs e);

        //This event will be raised after the divisions are updated
        public virtual event GEMSParallel_DataChangedEventHandler GEMSParallel_DataChanged;

        public void ParallelDataChangedAlarm()
        {
            this.parent.IsUpdated = true;

            if (this.GEMSParallel_DataChanged != null)
            {
                this.GEMSParallel_DataChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Construction and Output Methods

        public GEMSParallel(GEMSProject parent)
        {
            this.parent = parent;
            this.startPecPoint = MeshPointKey.Key(-1, -1, -1);
            this.endPecPoint = MeshPointKey.Key(-1, -1, -1);
        }

        public GEMSParallel(XPathNavigator navigator, GEMSProject parent)
        {
            this.parent = parent;

            //Read the division information
            navigator.MoveToChild("Division",string.Empty);
            ReadDivisionInfoInOneAxis(Axis.X, divisionInX, navigator);
            ReadDivisionInfoInOneAxis(Axis.Y, divisionInY, navigator);
            ReadDivisionInfoInOneAxis(Axis.Z, divisionInZ, navigator);
            navigator.MoveToParent();

            //Read the area information
            /*navigator.MoveToChild("Areas", string.Empty);
            XPathNodeIterator nodes = navigator.SelectChildren("Area", string.Empty);
            while (nodes.MoveNext())
            {
                XPathNavigator areaNavigator = nodes.Current.Clone();
                GEMSParallelArea area = new GEMSParallelArea(areaNavigator);
                areaList.Add(area);

                int areaMeshCount = (area.End.X - area.Start.X) 
                    * (area.End.Y - area.Start.Y) 
                    * (area.End.Z - area.Start.Z);

                if (minAreaMeshCount < 0)
                    minAreaMeshCount = areaMeshCount;
                else
                    minAreaMeshCount = minAreaMeshCount < areaMeshCount ? minAreaMeshCount : areaMeshCount;

                if (maxAreaMeshCount < 0)
                    maxAreaMeshCount = areaMeshCount;
                else
                    maxAreaMeshCount = maxAreaMeshCount > areaMeshCount ? maxAreaMeshCount : areaMeshCount;

            }
            navigator.MoveToParent();*/

            //Create area list based the division information
            UpdateAreaList ( );

            if (divisionInX.Count > 0)
                meshCountInX = divisionInX[divisionInX.Count - 1];
            else
                meshCountInX = parent.Mesh.MeshCountInX;

            if (divisionInY.Count > 0)
                meshCountInY = divisionInY[divisionInY.Count - 1];
            else
                meshCountInY = parent.Mesh.MeshCountInY;

            if (divisionInZ.Count > 0)
                meshCountInZ = divisionInZ[divisionInZ.Count - 1];
            else
                meshCountInZ = parent.Mesh.MeshCountInZ;
        }

        /// <summary>
        /// Read the division information in one axis
        /// </summary>
        /// <param name="axis">Axis which need to read</param>
        private void ReadDivisionInfoInOneAxis(Axis axis, List<int> divisions, XPathNavigator navigator)
        {
            navigator.MoveToChild(axis.ToString().ToUpper(), string.Empty);
            XPathNodeIterator nodes = navigator.SelectChildren("Pos", string.Empty);
            while (nodes.MoveNext())
            {
                XPathNavigator posNavigator = nodes.Current.Clone();
                int pos = int.Parse(posNavigator.GetAttribute("value", string.Empty));
                divisions.Add(pos);
            }
            navigator.MoveToParent();
        }

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        public string BuildOuterXmlString()
        {
            StringBuilder parallelNodeBuilder = new StringBuilder();

            parallelNodeBuilder.Append("<Parallel>");

            parallelNodeBuilder.Append("<Division>");
            parallelNodeBuilder.Append(BuildDivisionXmlStringInOneAxis(Axis.X, divisionInX));
            parallelNodeBuilder.Append(BuildDivisionXmlStringInOneAxis(Axis.Y, divisionInY));
            parallelNodeBuilder.Append(BuildDivisionXmlStringInOneAxis(Axis.Z, divisionInZ));
            parallelNodeBuilder.Append("</Division>");

            parallelNodeBuilder.Append("<Areas>");
            foreach (GEMSParallelArea area in areaList)
            {
                if(!area.IsPEC)
                    parallelNodeBuilder.Append(area.BuildOuterXmlString());
            }
            parallelNodeBuilder.Append("</Areas>");
            parallelNodeBuilder.Append("</Parallel>");

            return parallelNodeBuilder.ToString();
        }

        /// <summary>
        /// Build the xml string containing division information in one axis
        /// </summary>
        private string BuildDivisionXmlStringInOneAxis(Axis direction, List<int> divisions)
        {
            StringBuilder oneDirectionDivisionBuilder = new StringBuilder();

            oneDirectionDivisionBuilder.AppendFormat("<{0}>", direction.ToString().ToUpper());
            foreach (int pos in divisions)
            {
                oneDirectionDivisionBuilder.AppendFormat("<Pos value=\"{0}\" />", pos);
            }
            oneDirectionDivisionBuilder.AppendFormat("</{0}>", direction.ToString().ToUpper());

            return oneDirectionDivisionBuilder.ToString();
        }

        public GEMSParallel Clone()
        {
            GEMSParallel newParallelInfo = new GEMSParallel(this.parent);

            newParallelInfo.meshCountInX = this.meshCountInX;
            newParallelInfo.meshCountInY = this.meshCountInY;
            newParallelInfo.meshCountInZ = this.meshCountInZ;

            if(this.divisionInX.Count == 0)
                newParallelInfo.UpdateDivisionInX(1);

            if (this.divisionInX.Count == 0)
                newParallelInfo.UpdateDivisionInX(1);
            else
                newParallelInfo.divisionInX.AddRange(this.divisionInX);

            if (this.divisionInY.Count == 0)
                newParallelInfo.UpdateDivisionInY(1);
            else
               newParallelInfo.divisionInY.AddRange(this.divisionInY);

           if (this.divisionInZ.Count == 0)
               newParallelInfo.UpdateDivisionInZ(1);
           else
               newParallelInfo.divisionInZ.AddRange(this.divisionInZ);

           newParallelInfo.areaList = new List<GEMSParallelArea>();
           foreach (GEMSParallelArea area in areaList)
           {
               GEMSParallelArea newArea = new GEMSParallelArea();
               newArea.IsPEC = area.IsPEC;
               newArea.Start = area.Start.Clone();
               newArea.End = area.End.Clone();
               newParallelInfo.areaList.Add(newArea);
           }

           newParallelInfo.minAreaMeshCount = this.minAreaMeshCount;
           newParallelInfo.maxAreaMeshCount = this.maxAreaMeshCount;

           return newParallelInfo;
       }

        #endregion

       #region Division Update Methods

       public void InitializeDivision()
        {
            this.meshCountInX = parent.Mesh.MeshCountInX;
            this.meshCountInY = parent.Mesh.MeshCountInY;
            this.meshCountInZ = parent.Mesh.MeshCountInZ;

            divisionInX.Clear();
            divisionInY.Clear();
            divisionInZ.Clear();

            UpdateDivisionInX(1);
            UpdateDivisionInY(1);
            UpdateDivisionInZ(1);
        }

        public void UpdateDivisionInX(int newDivision)
        {
            if (divisionInX.Count != newDivision + 1)
            {
                Divide(newDivision, meshCountInX, divisionInX);

                UpdateAreaList();

                ParallelDataChangedAlarm ( );

            }
        }   

        public void UpdateDivisionInY(int newDivision)
        {
            if (divisionInY.Count != newDivision + 1)
            {
                Divide(newDivision, meshCountInY, divisionInY);

                UpdateAreaList();

                ParallelDataChangedAlarm ( );

            }
        }

        public void UpdateDivisionInZ(int newDivision)
        {
            if (divisionInZ.Count != newDivision + 1)
            {
                Divide(newDivision, meshCountInZ, divisionInZ);

                UpdateAreaList();

                ParallelDataChangedAlarm ( );

            }
        }

        private void Divide(int divisionCount, int meshCount, List<int> divisions)
        {
            //Remove all the division
            divisions.Clear();

            if (meshCount == 0)
                return;

            //Compute the average interval between each two division positions
            int interval = meshCount / divisionCount;
            int mount = interval * divisionCount;
            int offset = meshCount - mount;

            //Distribute the average interval to every division position
            int[] intervals = new int[divisionCount];
            for (int i = 0; i < divisionCount; i++)
            {
                intervals[i] = interval;
            }

            //Distribute the remain mesh point from the middle division position
            for (int start = divisionCount / 2 - offset / 2, i = 0; i < offset; i++)
            {
                intervals[i + start] += 1;
            }

            //Create division positions
            int distributedMount = 0;
            divisions.Add(distributedMount);
            for (int i = 0; i < divisionCount; i++)
            {
                distributedMount += intervals[i];
                divisions.Add(distributedMount);
            }
        }

        public void UpdateAreaList()        
        {
            areaList.Clear();

            minAreaMeshCount = -1;
            maxAreaMeshCount = -1;

            int zMax = divisionInZ.Count > 0 ? divisionInZ.Count - 1 : 1;
            int yMax = divisionInY.Count > 0 ? divisionInY.Count - 1 : 1;
            int xMax = divisionInX.Count > 0 ? divisionInX.Count - 1 : 1;

            for (int i = 0; i < zMax; i++)
                for (int j = 0; j < yMax; j++)
                    for (int k = 0; k < xMax; k++)
                    {
                        MeshPointKey min = new MeshPointKey();
                        MeshPointKey max = new MeshPointKey();

                        if (divisionInX.Count >= 2)
                        {
                            min.X = divisionInX[k];
                            max.X = divisionInX[k + 1];
                        }

                        if (divisionInY.Count >= 2)
                        {
                            min.Y = divisionInY[j];
                            max.Y = divisionInY[j + 1];
                        }

                        if (divisionInZ.Count >= 2)
                        {
                            min.Z = divisionInZ[i];
                            max.Z = divisionInZ[i + 1];
                        }

                        int areaMeshCount = (max.X - min.X) * (max.Y - min.Y) * (max.Z - min.Z);

                        if (minAreaMeshCount < 0)
                            minAreaMeshCount = areaMeshCount;
                        else
                            minAreaMeshCount = minAreaMeshCount < areaMeshCount ? minAreaMeshCount : areaMeshCount;

                        if (maxAreaMeshCount < 0)
                            maxAreaMeshCount = areaMeshCount;
                        else
                            maxAreaMeshCount = maxAreaMeshCount > areaMeshCount ? maxAreaMeshCount : areaMeshCount;

                        GEMSParallelArea area = new GEMSParallelArea();
                        area.Start = min;
                        area.End = max;
                        this.areaList.Add(area);
                    }
        }      
        
        #endregion       

        #region Public Properties

        public List<int> DivisionInX
        {
            get { return divisionInX; }
        }

        public List<int> DivisionInY
        {
            get { return divisionInY; }
        }

        public List<int> DivisionInZ
        {
            get { return divisionInZ; }
        }

        public int DivisionMaxCountInX
        {
            get { return this.meshCountInX / minMeshCount; }
        }

        public int DivisionMaxCountInY
        {
            get { return this.meshCountInY / minMeshCount; }
        }

        public int DivisionMaxCountInZ
        {
            get { return this.meshCountInZ / minMeshCount; }
        }

        public int DivisionCountInX
        {
            get {
                if (divisionInX.Count == 0)
                    return 1;
                else
                    return divisionInX.Count - 1;
            }
        }

        public int DivisionCountInY
        {
            get
            {
                if (divisionInY.Count == 0)
                    return 1;
                else
                    return divisionInY.Count - 1;
            }
        }

        public int DivisionCountInZ
        {
            get
            {
                if (divisionInZ.Count == 0)
                    return 1;
                else
                    return divisionInZ.Count - 1;
            }
        }

        public int CPUNumber
        {
            get { return this.DivisionCountInX * DivisionCountInY * DivisionCountInZ; }
        }

        public float Balance
        {
            get { return (float)minAreaMeshCount/(float)maxAreaMeshCount ; }
        }

        public List<GEMSParallelArea> AreaList
        {
            get { return areaList; }
        }

        public GEMSProject Parent
        {
            get { return parent; }
        }

        #endregion
    }
}
