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
using System.ComponentModel;

using System.Xml;
using System.Xml.XPath;
using System.Drawing.Design;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models.GeometryModels;

using GEMS.Designer.Forms;

namespace GEMS.Designer.Models
{
    [TypeConverter(typeof(SingleEOConvertor)), 
        Editor(typeof(UISingleEOEditor), typeof(UITypeEditor))]
    public abstract class SingleEO
    {
        private GEMSSingle parent;
        protected int positive = 1;

        public static Color DefaultExcitationColor = Color.Red;
        public static Color DefaultOutputColor = Color.Blue;

        public SingleEO(GEMSSingle single)
        {
            this.parent = single;
        }

        public static SingleEO Load(XPathNavigator navigator, GEMSSingle parent)
        {
            SingleEO singleEO = null;

            switch (parent.CreateOperation.Name)
            {
                case GeometryOperation.GeometryOperationType.CreateLine:      //Voltage Excitation or Output
                    if (navigator.MoveToChild("VoltageExcitation", string.Empty))
                    {
                        singleEO = new VoltageExcitation(navigator, parent);
                    }
                    else if (navigator.MoveToChild("VoltageOutput", string.Empty))
                    {
                        singleEO = new VoltageOutput(navigator, parent);
                    }

                    break;
                case GeometryOperation.GeometryOperationType.CreatePoint:     //FieldPointOutPut
                    if (navigator.MoveToChild("FieldOnPointOutput", string.Empty))
                    {
                        //((Point)operations[0]).BFieldOutput = true;
                        singleEO = new PointOutput(navigator,parent);
                    }
                    break;
                case GeometryOperation.GeometryOperationType.CreateRectangle:  //Current Excitation or Output
                case GeometryOperation.GeometryOperationType.CreateRound:  //Current Excitation or Output
                    if (navigator.MoveToChild("CurrentExcitation", string.Empty))
                    {
                        singleEO = new CurrentExcitation(navigator, parent);
                    }
                    else if (navigator.MoveToChild("CurrentOutput", string.Empty))
                    {
                        singleEO = new CurrentOutput(navigator, parent);
                    }
                    break;
                default:
                    break;
            }

            return singleEO;
        }

        public abstract string BuildOuterXmlString();
        public abstract SingleEO Clone(GEMSSingle parentSignal);

        public SingleEOSymbolModel CreateSingleEOSymbolModel(IEOModel sourceModel)
        {
            SingleEOSymbolModel singleEOSymbol = new SingleEOSymbolModel(this.positive);

            Vector3 eoStart = sourceModel.EOStart; 
            Vector3 eoEnd  = sourceModel.EOEnd;

            //Following code just is a render rule
            if (!(sourceModel is LineModel))
            {
                if (this.positive == 1)
                {
                    eoEnd = eoStart + eoEnd;
                }
                else
                {
                    eoEnd = eoStart - eoEnd;

                    Vector3 temp = eoEnd;
                    eoEnd = eoStart;
                    eoStart = temp;
                }
            }

            singleEOSymbol.Node1 = eoStart;
            singleEOSymbol.Node2 = eoEnd;

            singleEOSymbol.Initialize();

            return singleEOSymbol;
        }

        #region Properties

        public GEMSSingle Parent
        {
            get { return parent; }
        }

        public int Positive
        {
            get { return positive; }
            set { positive = value; }
        }

        #endregion

        #region SignalIOConvertor Specification
        internal class SingleEOConvertor : System.ComponentModel.StringConverter
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
                         value is SingleEO)
                {

                    SingleEO io = (SingleEO)value;

                    return io.Parent.CurrentType;
                }

                if (destinationType == typeof(System.String) && value == null)
                {
                    return "N/A";
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

        }
        #endregion

        #region UISignalIOEditor Specification

        internal class UISingleEOEditor : System.Drawing.Design.UITypeEditor
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

                //Cannot to edit
                if (value == null)
                    return value;

                //Get the io object
                SingleEO io = value as SingleEO;

                if (io is PointOutput)  //There's no interface for point output
                {
                    FieldOutputForm form = new FieldOutputForm(io as PointOutput);
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        io.parent.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                    }
                    form.Dispose();
                }
                else
                {
                    ExcitationOutputForm form = new ExcitationOutputForm(io);
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        io.parent.SingleDataChangedAlarm(GEMSSingle.SingleDataChangedEventArgs.DataChangeType.EOChanged);
                    }
                    form.Dispose();
                }

                return io;
            }

        }
        #endregion
    }
  
}
