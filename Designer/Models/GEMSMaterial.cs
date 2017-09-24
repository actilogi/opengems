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
using System.Xml.XPath;
using System.Xml;
using System.ComponentModel;
using System.Drawing.Design;
using GEMS.Designer.Forms;

namespace GEMS.Designer.Models
{
    /// <summary>
    /// Material of the geometry
    /// </summary>
    [TypeConverter(typeof(MaterialConvertor)),
        Editor(typeof(UIMaterialEditor), typeof(UITypeEditor)),]
    public class GEMSMaterial
    {
        public enum MaterialStatus
        {
            Unavailable = 0,
            Used,
            Unused,
        }

        public static float FreeSpaceEplison = 1.0f;
        public static float FreeSpaceSigma = 0.0f;
        public static float PECSigma = 1E30f;
        public static float PECEplison = 1.0f; 
       
        private int id = -1;
        private string name = "N/A";
        private string detail = string.Empty;

        private float epsilonX = 1;
        private float sigmaX = 0;
        private float muX = 1;
        private float musigmaX = 0;

        private List<int> usedSignalIds = null;
        private GEMSProject m_project;
 
        /// <summary>
        /// Construct of material
        /// </summary>
        public GEMSMaterial(int id,GEMSProject parent)
        {
            if (id != -1)   //Create a available material
            {
                this.id = id;
                this.usedSignalIds = new List<int>();
            }

            this.m_project = parent;
        }

        public GEMSMaterial(XPathNavigator navigator,GEMSProject parent)
        {
            detail = navigator.GetAttribute("detail", string.Empty);
            id = int.Parse(navigator.GetAttribute("id", string.Empty));
            name = navigator.GetAttribute("name", string.Empty);

            navigator.MoveToChild("Epsilon", string.Empty);
            epsilonX = float.Parse(navigator.GetAttribute("x", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("Sigma", string.Empty);
            sigmaX = float.Parse(navigator.GetAttribute("x", string.Empty));
            navigator.MoveToParent();

            navigator.MoveToChild("Mu", string.Empty);
            muX = float.Parse(navigator.GetAttribute("x", string.Empty));
            navigator.MoveToParent();
            
            navigator.MoveToChild("Musigma", string.Empty);
            musigmaX = float.Parse(navigator.GetAttribute("x", string.Empty));
            navigator.MoveToParent();

            this.m_project = parent;
            this.usedSignalIds = new List<int>();
       }      
       
        /// <summary>
        /// Build the xml string containing the information of the object
        /// </summary>        
        public string BuildOuterXmlString()
        {
            //Construct the xml string 
            StringBuilder materialNodeBuilder = new StringBuilder();

            materialNodeBuilder.AppendFormat("<Material detail=\"{0}\" isotropic=\"1\" priority=\"0\" id=\"{1}\" name=\"{2}\" >", this.detail, this.id, this.name);
            materialNodeBuilder.AppendFormat("<Epsilon x=\"{0}\" y=\"{0}\" z=\"{0}\" />", this.epsilonX);
            materialNodeBuilder.AppendFormat("<Sigma x=\"{0}\" y=\"{0}\" z=\"{0}\" />", this.sigmaX);
            materialNodeBuilder.AppendFormat("<Mu x=\"{0}\" y=\"{0}\" z=\"{0}\" />", this.muX);
            materialNodeBuilder.AppendFormat("<Musigma x=\"{0}\" y=\"{0}\" z=\"{0}\" />", this.musigmaX);
            materialNodeBuilder.Append("</Material>");
            
            return materialNodeBuilder.ToString();
        }

        #region Properties

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Detail
        {
            get
            {
                return this.detail;
            }
            set
            {
                this.detail = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }   

        public float EpsilonX
        {
            get
            {
                return this.epsilonX;
            }
            set
            {
                this.epsilonX = value;
            }
        }

        public float SigmaX
        {
            get
            {
            return this.sigmaX;
            }
            set
            {
            this.sigmaX = value;
            }
        }

        public float MuX
        {
            get
            {
            return this.muX;
            }
            set
            {
            this.muX = value;
            }
        }


        public float MusigmaX
        {
            get
            {
                return this.musigmaX;
            }
            set
            {
                this.musigmaX = value;
            }
        }

        public GEMSProject Parent
        {
            get { return m_project; }
        }

        public MaterialStatus Status
        {
            get {
                if(usedSignalIds != null)
                {
                    if (usedSignalIds.Count == 0)
                        return MaterialStatus.Unused;
                    else if (usedSignalIds.Count > 0)
                        return MaterialStatus.Used;
                    else
                        return MaterialStatus.Unavailable;
                }
                else
                    return MaterialStatus.Unavailable;
            }
        }

        public List<int> UsedSignalIds
        {
            get { return usedSignalIds; }
        }

  
        #endregion
    }

    #region MaterialConvertor specification

    internal class MaterialConvertor : System.ComponentModel.SingleConverter
    {

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(System.String))
                return true;

            return base.CanConvertTo(context, destinationType);

        }


        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                     value is GEMSMaterial)
            {

                GEMSMaterial material = (GEMSMaterial)value;

                return material.Name;
            }

            if (destinationType == typeof(System.String) && value == null)
            {
                return "N/A";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion

    #region UIMaterialEditor specification

    internal class UIMaterialEditor : System.Drawing.Design.UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }

            GEMSSingle currentSingle = context.Instance as GEMSSingle;

            if (currentSingle != null)
            {
                MaterialListForm from = new MaterialListForm ( currentSingle );

                if (from.ShowDialog ( ) == System.Windows.Forms.DialogResult.OK)
                {
                    value = currentSingle.SingleMaterial;

                    if (currentSingle.CurrentEO != null)
                    {
                        currentSingle.CurrentEO = null;
                        currentSingle.SingleDataChangedAlarm ( GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged );

                    }

                }
                from.Dispose ( );
            }

            return value;
        }

    }

    #endregion
}
