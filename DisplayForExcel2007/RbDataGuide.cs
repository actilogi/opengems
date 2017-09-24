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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;

namespace GEMS.Display.Excel2007
{
    // TODO:
    // This is an override of the RequestService method in the ThisAddIn class.
    // To hook up your custom ribbon uncomment this code.
    public partial class ThisAddIn
    {
        private RbDataGuide ribbon;

        protected override object RequestService(Guid serviceGuid)
        {
            if (serviceGuid == typeof(Office.IRibbonExtensibility).GUID)
            {
                if (ribbon == null)
                    ribbon = new RbDataGuide();
                return ribbon;
            }

            return base.RequestService(serviceGuid);
        }
    }

    [ComVisible(true)]
    public class RbDataGuide : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;
        private bool dataGuidControlExists = false;

        public RbDataGuide()
        {
        }

        public void DisplayButtonClick(Office.IRibbonControl control)
        {
            if (!dataGuidControlExists)
            {
                Globals.ThisAddIn.AddDisplayDataGuidPane();
            }
            else
            {
                Globals.ThisAddIn.RemoveDisplayDataGuidPane();
            }
            dataGuidControlExists = !dataGuidControlExists;
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return Properties.Resources.RbDataGuide;
        }

        #endregion

        #region Ribbon Callbacks

        public void OnLoad(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public void OnToggleButton1(Office.IRibbonControl control, bool isPressed)
        {
            if (isPressed)
                MessageBox.Show("Pressed");
            else
                MessageBox.Show("Released");
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
