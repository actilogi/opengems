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

using System.Text.RegularExpressions;

namespace GEMS.Designer.Models
{
    public class GEMSHuygensBox
    {
        private const string RegularPatternString = @"((^[-+]?\d*\.?\d*$),)+";

        private GEMSProject parent;

        private bool isEnable = false;
        private int minX = 1;
        private int minY = 1;
        private int minZ = 1;
        private int maxX = 1;
        private int maxY = 1;
        private int maxZ = 1;

        private bool isPhiEnable = true;
        private float thetaStart = 0f;
        private float thetaEnd = 180f;
        private float thetaStep = 1f;
        private List<float> phis = new List<float>();

        private bool isThetaEnable = true;
        private float phiStart = 0f;
        private float phiEnd = 360f;
        private float phiStep = 1f;
        private List<float> thetas = new List<float>();

        private List<float> frequencys = new List<float>();
        private Frequency.FrequencyUnit frequencyUnit = Frequency.FrequencyUnit.GHz;
        private bool isMcOnly = false;
        private byte apertureField = 0x3F;
        private Vector3WithUnit referencePoint;


        public GEMSHuygensBox(GEMSProject parent)
        {
            this.parent = parent;
            referencePoint = new Vector3WithUnit(0.0f, parent.Environment.DefaultLengthUnit);
            phis.Add(0);
            phis.Add(90);
            thetas.Add(90);
        }

        public GEMSHuygensBox(XPathNavigator navigator, GEMSProject parent)
        {
            this.parent = parent;

            isEnable = (int.Parse(navigator.GetAttribute("enabled", string.Empty)) == 1);
            isMcOnly = (int.Parse(navigator.GetAttribute("mconly", string.Empty)) == 1);

            navigator.MoveToChild("Geometry", string.Empty);
            minX = int.Parse(navigator.GetAttribute("minx", string.Empty));
            minY = int.Parse(navigator.GetAttribute("miny", string.Empty));
            minZ = int.Parse(navigator.GetAttribute("minz", string.Empty));
            maxX = int.Parse(navigator.GetAttribute("maxx", string.Empty));
            maxY = int.Parse(navigator.GetAttribute("maxy", string.Empty));
            maxZ = int.Parse(navigator.GetAttribute("maxz", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("RefPoint", string.Empty);
            referencePoint = new Vector3WithUnit();
            referencePoint.X = new Length(navigator.GetAttribute("x", string.Empty), navigator.GetAttribute("ux", string.Empty));
            referencePoint.Y = new Length(navigator.GetAttribute("y", string.Empty), navigator.GetAttribute("uy", string.Empty));
            referencePoint.Z = new Length(navigator.GetAttribute("z", string.Empty), navigator.GetAttribute("uz", string.Empty));
            navigator.MoveToParent();

            //Frequency list
            navigator.MoveToChild("FrequencyList", string.Empty);
            frequencyUnit = (Frequency.FrequencyUnit)Enum.Parse(typeof(Frequency.FrequencyUnit), navigator.GetAttribute("unit", string.Empty));
            XPathNodeIterator nodes = navigator.SelectChildren("Frequency",string.Empty);
            while (nodes.MoveNext())
            {
                frequencys.Add(float.Parse(nodes.Current.GetAttribute("value", string.Empty)));
            }
            navigator.MoveToParent();

            navigator.MoveToChild("TwoD", string.Empty);
            //Phi plane
            navigator.MoveToChild("PhiPlane",string.Empty);
            isPhiEnable = (int.Parse(navigator.GetAttribute("enabled", string.Empty)) == 1);
            nodes = navigator.SelectChildren("Phi", string.Empty);
            phis.Clear();
            while (nodes.MoveNext())
            {
                phis.Add(float.Parse(nodes.Current.GetAttribute("value", string.Empty)));
            }
            navigator.MoveToChild("Theta", string.Empty);
            thetaStart = float.Parse(navigator.GetAttribute("from",string.Empty));
            thetaStep = float.Parse(navigator.GetAttribute("step", string.Empty));
            thetaEnd = float.Parse(navigator.GetAttribute("to", string.Empty));
            navigator.MoveToParent();
            navigator.MoveToParent();

            //Theta plane
            navigator.MoveToChild("ThetaPlane", string.Empty);
            isThetaEnable = (int.Parse(navigator.GetAttribute("enabled", string.Empty)) == 1);
            nodes = navigator.SelectChildren("Theta", string.Empty);
            thetas.Clear();
            while (nodes.MoveNext())
            {
                thetas.Add(float.Parse(nodes.Current.GetAttribute("value", string.Empty)));
            }
            navigator.MoveToChild("Phi", string.Empty);
            phiStart = float.Parse(navigator.GetAttribute("from", string.Empty));
            phiStep = float.Parse(navigator.GetAttribute("step", string.Empty));
            phiEnd = float.Parse(navigator.GetAttribute("to", string.Empty));
            navigator.MoveToParent();
            navigator.MoveToParent();

            navigator.MoveToParent();

            navigator.MoveToChild("ApertureField", string.Empty);
            apertureField = byte.Parse(navigator.GetAttribute("value",string.Empty));
            navigator.MoveToParent();
        }

         /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        /// <param name="navigator"></param>
        public string BuildOuterXmlString()        
        {
            StringBuilder HuygensboxBuilder = new StringBuilder();

            HuygensboxBuilder.AppendFormat("<HuygensBox mconly=\"{0}\" enabled=\"{1}\" >",isMcOnly ? 1 : 0,isEnable ? 1:0);
            HuygensboxBuilder.AppendFormat("<Geometry maxx=\"{0}\" maxy=\"{1}\" minx=\"{2}\" maxz=\"{3}\" miny=\"{4}\" minz=\"{5}\" />",
                maxX,maxY,maxZ,minX,minY,minZ);

            HuygensboxBuilder.Append(referencePoint.BuildInnserXmlString("RefPoint"));
            HuygensboxBuilder.Append("<TwoD>");

            HuygensboxBuilder.AppendFormat("<PhiPlane enabled=\"{0}\" >", isPhiEnable ? 1 : 0);
            foreach (float phi in phis)
            {
                HuygensboxBuilder.AppendFormat("<Phi value=\"{0}\" />", phi);
            }
            HuygensboxBuilder.AppendFormat("<Theta from=\"{0}\" step=\"{1}\" to=\"{2}\" />", thetaStart,thetaStep,thetaEnd);
            HuygensboxBuilder.Append("</PhiPlane>");

            HuygensboxBuilder.AppendFormat("<ThetaPlane enabled=\"{0}\" >", isThetaEnable ? 1 : 0);
            foreach (float theta in thetas)
            {
                HuygensboxBuilder.AppendFormat("<Theta value=\"{0}\" />", theta);
            }
            HuygensboxBuilder.AppendFormat("<Phi from=\"{0}\" step=\"{1}\" to=\"{2}\" />", phiStart, phiStep, phiEnd);
            HuygensboxBuilder.Append("</ThetaPlane>");

            HuygensboxBuilder.Append("</TwoD>");

            HuygensboxBuilder.AppendFormat("<FrequencyList unit=\"{0}\" >", (int)frequencyUnit);
            foreach (float frequency in frequencys)
            {
                HuygensboxBuilder.AppendFormat("<Frequency value=\"{0}\" />", frequency);
            }
            HuygensboxBuilder.Append("</FrequencyList>");

            HuygensboxBuilder.AppendFormat("<ApertureField outEnabled=\"0\"  value=\"{0}\" />", apertureField);

            HuygensboxBuilder.Append("</HuygensBox>");

            return HuygensboxBuilder.ToString();
        }


        public GEMSHuygensBox Clone()
        {
            GEMSHuygensBox newBox = new GEMSHuygensBox(this.parent);

            newBox.isEnable = this.isEnable;
            newBox.minX = this.minX;
            newBox.maxX = this.maxX;
            newBox.minY = this.minY;
            newBox.maxY = this.maxY;
            newBox.minZ = this.minZ;
            newBox.maxZ = this.maxZ;
            newBox.referencePoint = new Vector3WithUnit(this.referencePoint);

            newBox.isPhiEnable = this.isPhiEnable;
            newBox.thetaStart = this.thetaStart;
            newBox.thetaEnd = this.thetaEnd;
            newBox.thetaStep = this.thetaStep;
            newBox.phis.Clear();
            newBox.phis.AddRange(this.phis);

            newBox.isThetaEnable = this.isThetaEnable;
            newBox.phiStart = this.phiStart;
            newBox.phiEnd = this.phiEnd;
            newBox.phiStep = this.phiStep;
            newBox.thetas.Clear();
            newBox.thetas.AddRange(this.thetas);

            newBox.frequencys.AddRange(this.frequencys);
            newBox.frequencyUnit = this.frequencyUnit;
            newBox.apertureField = this.apertureField;
            newBox.isMcOnly = this.isMcOnly;

            return newBox;
        }

        public void ResetRange()
        {
            this.minX = 1;
            this.maxX = 1;
            this.minY = 1;
            this.maxY = 1;
            this.minZ = 1;
            this.maxZ = 1;

        }

        #region Public Properties

        public GEMSProject Parent
        {
            get { return parent; }
        }

        public int MinX
        {
            get { return minX; }
            set { minX = value; }
        }

        public int MaxX
        {
            get { return maxX; }
            set { maxX = value; }
        }

        public int MinY
        {
            get { return minY; }
            set { minY = value; }
        }

        public int MaxY
        {
            get { return maxY; }
            set { maxY = value; }
        }

        public int MinZ
        {
            get { return minZ; }
            set { minZ = value; }
        }

        public int MaxZ
        {
            get { return maxZ; }
            set { maxZ = value; }
        }

        public float PhiStart
        {
            get { return phiStart; }
            set { phiStart = value; }
        }

        public float PhiEnd
        {
            get { return phiEnd; }
            set { phiEnd = value; }
        }

        public float PhiStep
        {
            get { return phiStep; }
            set { phiStep = value; }
        }

        public float ThetaStart
        {
            get { return thetaStart; }
            set { thetaStart = value; }
        }

        public float ThetaEnd
        {
            get { return thetaEnd; }
            set { thetaEnd = value; }
        }

        public float ThetaStep
        {
            get { return thetaStep; }
            set { thetaStep = value; }
        }

        public List<float> FrequencyList
        {
            get { return frequencys; }
        }

        public Frequency.FrequencyUnit FrequencyUnit
        {
            get { return frequencyUnit; }
            set { frequencyUnit = value; }
        }


        public bool IsPhiEnable
        {
            get { return isPhiEnable; }
            set { isPhiEnable = value; }
        }

        public List<float> PhiList
        {
            get { return phis; }
            set { phis = value; }
        }

        public string PhiString
        {
            get {
                string phiString = string.Empty;
                foreach (float phi in phis)
                {
                    phiString += phi.ToString() + ",";
                }
                return phiString.Substring(0, phiString.Length - 1);
            }            
        }

        public bool IsThetaEnable        
        {
            get { return isThetaEnable; }
            set { isThetaEnable = value; }
        }

        public string ThetaString
        {
            get
            {
                string thetaString = string.Empty;
                foreach (float theta in thetas)
                {
                    thetaString += theta.ToString() + ",";
                }
                return thetaString.Substring(0, thetaString.Length - 1);
            }            
        }

        public List<float> ThetaList
        {
            get { return thetas; }
            set { thetas = value; }
        }

        public bool IsMegneticCurrentOnly        
        {
            get { return isMcOnly; }
            set { isMcOnly = value; }
        }

        public byte ApertureField
        {
            get { return apertureField; }
            set { apertureField = value; }
        }

        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }

        public Vector3WithUnit ReferencePoint
        {
            get { return referencePoint; }
            set { referencePoint = value; }
        } 

        #endregion
    }
}
