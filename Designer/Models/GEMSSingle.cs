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
using System.Drawing.Design;

using System.ComponentModel;

using System.Xml;
using System.Xml.XPath;

using GEMS.Designer.Direct3D;
using GEMS.Designer.Controls;
using GEMS.Designer.Forms;
using GEMS.Designer.Models.GeometryOperations;
using GEMS.Designer.Models.GeometryModels;

namespace GEMS.Designer.Models
{


    /// <summary>
    /// This class expresses an em simulation object containing a single geometry.
    /// We maybe will add another class named Group to express an em simulation object 
    /// containing multiple single geometries
    /// </summary>
    [DefaultPropertyAttribute ( "Name" )]
    public class GEMSSingle
    {
        #region Internal Members

        //Control members
        private int id = -1;
        private string name = string.Empty;
        private GEMSProject parent;

        //Simulation members
        private GEMSMaterial material = null;
        private Color singleColor = DefaultSingleColor;
        private bool pec;
        private SingleEO currentEO = null;

        //Geometry members
        public static int MaxTransparency = 255; //opaque
        public static int MinTransparency = 100;
        private static Color DefaultSingleColor = Color.FromArgb ( MaxTransparency , 132 , 132 , 193 );

        //Each single is associated with some geometry operations,
        //such as :create a geometry , move it , rotate it and combine with others.
        //We'll add them this stack ,so we can undo it 
        //except the first operation: Create a geometry!!!, which is to say : 
        //one single has only one create geometry operation , 
        //and if this operation is undoed, the single will be deleted too!
        //Each single will be displayed on the screen , so we'll take the  GeometryCreateOperation 
        //to create a mesh , and using other operations to create some transfer matrices.
        private List<GeometryOperation> operations = new List<GeometryOperation> ( );
        private GeometryCreateOperation createOperation = null;

        private GeometryModel primaryModel = null;  //Created based all the geometry operations
        private SingleEOSymbolModel eoSymbolModel = null; //Created based the primatry model and the SingleEO object

        #endregion

        #region Delegate and Event

        public class SingleDataChangedEventArgs : EventArgs
        {
            public enum DataChangeType
            {
                DisplayStyleChanged , //Color and transparncy changed
                GeometryChanged ,     //Geometry attributes changed
                EOChanged             //Excitation or output has been added to the single
            }

            public DataChangeType changedType;

            public SingleDataChangedEventArgs ( DataChangeType changedType )
            {
                this.changedType = changedType;
            }
        }

        public delegate void GEMSSingle_DataChangedEventHandler ( object sender , SingleDataChangedEventArgs e );
        public virtual event GEMSSingle_DataChangedEventHandler GEMSSingle_DataChanged;

        public void SingleDataChangedAlarm ( SingleDataChangedEventArgs.DataChangeType changedType )
        {
            this.parent.IsUpdated = true;

            //Update the single model based the changed type
            switch (changedType)
            {
                case SingleDataChangedEventArgs.DataChangeType.DisplayStyleChanged:
                    {
                        UpdateModelDisplayStyle ( );
                    }
                    break;
                case SingleDataChangedEventArgs.DataChangeType.EOChanged:
                    {
                        UpdateModelDisplayStyle ( );
                        UpdateSingleEOSymbolModel ( );
                    }
                    break;
                case SingleDataChangedEventArgs.DataChangeType.GeometryChanged:
                    {
                        UpdatePrimaryModel ( );
                        UpdateSingleEOSymbolModel ( );
                    }
                    break;
                default:
                    break;
            }

            if (this.GEMSSingle_DataChanged != null)
            {
                SingleDataChangedEventArgs args = new SingleDataChangedEventArgs ( changedType );

                this.GEMSSingle_DataChanged ( this , args );
            }
        }

        #endregion

        #region Public Properties

        [BrowsableAttribute ( false )]
        public int Id
        {
            get { return id; }
        }

        [CategoryAttribute ( "Single Settings" ) , DescriptionAttribute ( "Name of the single" )]
        public string Name
        {
            get { return name; }
            set
            {

                //Here, check whether the name is unique
                int sameNameSignalIndex = -1;
                foreach (GEMSSingle single in Parent.Singles)
                {
                    if (single.Name == value && single.Id != id)
                    {
                        sameNameSignalIndex = single.Id;
                        break;
                    }
                }

                if (sameNameSignalIndex == -1)
                    name = value;
            }
        }

        /// <summary>
        /// Get or set the single color's RGB parts
        /// </summary>
        [CategoryAttribute ( "Display Settings" ) , DisplayNameAttribute ( "Color" ) , DescriptionAttribute ( "Color of the geometry of the single" )]
        public Color BaseSingleColor
        {
            get
            {
                return Color.FromArgb ( singleColor.R , singleColor.G , singleColor.B );
            }
            set
            {
                singleColor = Color.FromArgb ( singleColor.A , value );
            }
        }

        /// <summary>
        /// Get or set the single color's A part
        /// </summary>
        [CategoryAttribute ( "Display Settings" ) , Editor ( typeof ( UITransparencyEditor ) , typeof ( UITypeEditor ) ) ,
       DescriptionAttribute ( "Transparency(from 255 to 60) of the geometry of the single when it's been selected. 60 means max transparency ,and 255 means opaque" )]
        public int Transparency
        {
            get
            {
                return singleColor.A;
            }
            set
            {
                singleColor = Color.FromArgb ( value , singleColor );
            }
        }

        [BrowsableAttribute ( false )]
        public Color SingleColor
        {
            get { return singleColor; }
        }

        [CategoryAttribute ( "Single Settings" ) , DescriptionAttribute ( "PEC" )]
        public bool PEC
        {
            get { return pec; }
            set { 

                pec = value;

                if (pec && this.currentEO != null)
                {
                    this.CurrentEO = null;
                    this.SingleDataChangedAlarm ( SingleDataChangedEventArgs.DataChangeType.EOChanged );
                }
            }
        }

        [BrowsableAttribute ( false )]
        public List<GeometryOperation> Operations
        {
            get
            {
                return operations;
            }
        }

        [BrowsableAttribute ( false )]
        public string CurrentType
        {
            get
            {
                if (currentEO == null)
                    return "N/A";
                else
                    return currentEO.GetType ( ).Name;
            }
        }

        [CategoryAttribute ( "Single Settings" ) ,
            DisplayNameAttribute ( "E/O" ) ,
            DescriptionAttribute ( "" )]
        public SingleEO CurrentEO
        {
            get { return currentEO; }
            set
            {

                currentEO = value;

                if (currentEO == null)
                {
                    this.singleColor = GEMSSingle.DefaultSingleColor;
                }
                else
                {
                    if (currentEO is SingleExcitation)
                        this.singleColor = SingleEO.DefaultExcitationColor;
                    else
                        this.singleColor = SingleEO.DefaultOutputColor;

                    //this.material = null; //clear material

                }
            }
        }

        [CategoryAttribute ( "Single Settings" ) ,
            DisplayName ( "Material" ) ,
            DescriptionAttribute ( "float click to choose the material of this single" )]
        public GEMSMaterial SingleMaterial
        {
            get { return material; }
            set
            {

                GEMSMaterial newMaterial = value as GEMSMaterial;

                //Add id of this single to the used id list of new material
                if (newMaterial != null)
                    newMaterial.UsedSignalIds.Add ( this.id );

                //Remove id of this single from the used id list of old material
                if (material != null)
                    material.UsedSignalIds.Remove ( this.id );

                //Set new value
                material = value;
            }
        }

        [BrowsableAttribute ( false )]
        public GEMSProject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        [BrowsableAttribute ( false )]
        public GeometryCreateOperation CreateOperation
        {
            get { return createOperation; }
        }

        [BrowsableAttribute ( false )]
        public GeometryModel PrimaryModel
        {
            get { return primaryModel; }
        }

        [BrowsableAttribute ( false )]
        public SingleEOSymbolModel EOSymbolModel
        {
            get { return eoSymbolModel; }
        }

        #endregion

        #region Constructure Methods

        public GEMSSingle ( int id , GEMSProject parent )
        {
            this.id = id;
            this.parent = parent;
        }

        public GEMSSingle ( XPathNavigator navigator , GEMSProject parent )
        {
            this.parent = parent;

            //Basic information
            id = int.Parse ( navigator.GetAttribute ( "id" , string.Empty ) );
            name = navigator.GetAttribute ( "name" , string.Empty );
            float transparency = float.Parse ( navigator.GetAttribute ( "transparent" , string.Empty ) );
            pec = navigator.GetAttribute ( "pec" , string.Empty ) == "0" ? false:true;

            //Material
            int materialId = int.Parse ( navigator.GetAttribute ( "material" , string.Empty ) );
            //Find the material in the material list of the project
            material = parent.SearchMaterial ( materialId );
            if (material != null)
                material.UsedSignalIds.Add ( this.id );

            //Color of the geometry belongs to this single
            navigator.MoveToChild ( "Color" , string.Empty );
            int blue = int.Parse ( navigator.GetAttribute ( "blue" , string.Empty ) );
            int red = int.Parse ( navigator.GetAttribute ( "red" , string.Empty ) );
            int green = int.Parse ( navigator.GetAttribute ( "green" , string.Empty ) );
            singleColor = Color.FromArgb ( GetModelMaterialTransparency ( transparency ) , red , green , blue );
            navigator.MoveToParent ( );

            //Operations
            LoadOperations ( navigator );

            //Information of excitation or output
            currentEO = SingleEO.Load ( navigator , this );

            UpdatePrimaryModel ( );
            UpdateSingleEOSymbolModel ( );
        }

        /// <summary>
        /// Load all operations from xml
        /// </summary>
        private void LoadOperations ( XPathNavigator navigator )
        {
            //Information of geometry operation
            navigator.MoveToChild ( "Operations" , string.Empty );
            XPathNodeIterator nodes = navigator.SelectChildren ( "Operation" , string.Empty );

            while (nodes.MoveNext ( ))
            {
                XPathNavigator operationNavigator = nodes.Current.Clone ( );

                string operationId = operationNavigator.GetAttribute ( "id" , string.Empty );
                if (operationId != "-1")
                {
                    XPathNavigator operationNode = navigator.SelectSingleNode ( "/Document/Operations/Operation[@id='" + operationId + "']" );

                    if (operationNode != null)
                    {
                        //Get the geometry operation
                        GeometryOperation operation =  GeometryOperation.Create ( operationNode , this );

                        if (operation is GeometryCreateOperation)
                            createOperation = operation as GeometryCreateOperation;

                        //Add it to the list
                        operations.Add ( operation );

                        //Add it to the list of GEMS Project
                        Parent.AddOperation ( operation );
                    }
                }
            }
            navigator.MoveToParent ( );
        }

        public void InitilizeSingle ( GeometryCreateOperation createOperation )
        {
            this.name = "Single" + id.ToString ( );

            //Binding with create operation
            this.createOperation = createOperation;
            this.createOperation.Parent = this;
            this.operations.Add ( createOperation );

            //Create the geometry model to display
            UpdatePrimaryModel ( );
        }

        /// <summary>
        /// Build a xml string containing the information of this object
        /// </summary>
        public string BuildOuterXmlString ( )
        {
            StringBuilder singlelNodeBuilder = new StringBuilder ( );

            singlelNodeBuilder.AppendFormat ( "<Single material=\"{0}\" transparent=\"{1}\" pec=\"{2}\" id=\"{3}\" name=\"{4}\" >" ,
                this.material != null ? this.material.Id : -1 , GetSingelTransparency ( BaseSingleColor.A ) , this.pec ? "1" : "0" , this.id , this.name );

            singlelNodeBuilder.AppendFormat ( "<Color blue=\"{0}\" red=\"{1}\" green=\"{2}\" />" ,
                this.singleColor.B , this.singleColor.R , this.singleColor.G );

            //singlelNodeBuilder.AppendFormat("<Density value=\"{0}\" />",this.density);
            singlelNodeBuilder.Append ( "<Density value=\"0\" />" );

            singlelNodeBuilder.Append ( "<Operations>" );
            foreach (GeometryOperation operation in operations)
                singlelNodeBuilder.AppendFormat ( "<Operation id=\"{0}\"/>" , operation.Id );
            singlelNodeBuilder.Append ( "</Operations>" );

            //Excitation or Output
            if (this.currentEO != null)
            {
                singlelNodeBuilder.Append ( this.currentEO.BuildOuterXmlString ( ) );
            }

            //Tail
            singlelNodeBuilder.Append ( "</Single>" );

            return singlelNodeBuilder.ToString ( );
        }

        /// <summary>
        /// Create an single object with same information of original single
        /// </summary>
        public GEMSSingle Clone ( int newId )
        {
            //Create a new single with information copyed from the specified single
            GEMSSingle newSingle = new GEMSSingle ( newId , this.parent );

            //Basic information 
            newSingle.name          = this.Name + "_" + newId.ToString ( );
            newSingle.pec           = this.PEC;
            newSingle.singleColor = this.BaseSingleColor;

            //Material
            newSingle.material = this.material;

            //Excitation or Output
            if (currentEO != null)
                newSingle.currentEO = this.currentEO.Clone ( newSingle );

            //Operation
            newSingle.operations = new List<GeometryOperation> ( );

            //Copy each operation mementoes
            foreach (GeometryOperation operation in this.operations)
            {
                GeometryOperation newOperation = operation.Clone ( parent.CreateNewOperationId ( ) , newSingle );
                newSingle.operations.Add ( newOperation );

                if (newOperation is GeometryCreateOperation)
                {
                    newSingle.createOperation = newOperation as GeometryCreateOperation;
                }

                //Add the operation to the list of GEMS Project
                Parent.AddOperation ( newOperation );
            }

            //Create the model for new single
            newSingle.UpdatePrimaryModel ( );
            newSingle.UpdateSingleEOSymbolModel ( );

            return newSingle;
        }
        #endregion

        #region Geometry Methods

        /// <summary>
        ///Create a geometry model which'll be rendered by GeometryRender object:
        ///the create opertion will generate a source basic geometry primitivies and a transform matrix,
        ///and another operation will multiply additional matrices to the original transform matrix. 
        /// </summary>
        private void UpdatePrimaryModel ( )
        {
            //The first loop is to find the GeometryCreateOperation object and create a basic geometry model.
            foreach (GeometryOperation operation in operations)
            {
                if (operation is GeometryCreateOperation)
                {
                    //Create a source geometry model
                    primaryModel                = createOperation.CreateSourceGeometryModel ( );
                    primaryModel.ModelColor     = singleColor;
                    break;
                }
            }

            //The second loop is to process the remained GeometryOperation objects
            foreach (GeometryOperation operation in operations)
            {
                if (!( operation is GeometryCreateOperation ))
                {
                    //Do something
                    continue;
                }
            }

        }

        /// <summary>
        /// Create the SingleEO Symbol model which is just a line based on the primary model
        /// to indicate this primary model is an excitation or output
        /// </summary>
        private void UpdateSingleEOSymbolModel ( )
        {
            if (currentEO == null)
            {
                eoSymbolModel = null;
                return;
            }

            //Check whether the single is an excitaion or output
            //If ture, it'll create an additional eo symbol geometry model
            //except the point output which'll just be changed color
            if (currentEO != null && primaryModel is IEOModel)
            {
                //Create an additional model
                eoSymbolModel   = currentEO.CreateSingleEOSymbolModel ( primaryModel as IEOModel );
                eoSymbolModel.ModelColor            = singleColor;
            }
            else if (this.currentEO is PointOutput)
            {
                primaryModel.ModelColor = this.singleColor;
            }
        }

        /// <summary>
        /// Update the color and transparency of the single's model
        /// </summary>
        private void UpdateModelDisplayStyle ( )
        {
            if (primaryModel != null)
            {
                primaryModel.ModelColor     = singleColor;
            }

            if (eoSymbolModel != null)
            {
                eoSymbolModel.ModelColor = singleColor;
            }
        }

        /// <summary>
        /// Get the interger transparency value for geometry model from float single transparency
        /// </summary>
        public static int GetModelMaterialTransparency ( float transparency )
        {
            return (int)( transparency *  ( MaxTransparency - MinTransparency ) + MinTransparency );
        }

        /// <summary>
        /// Get the float single transparency value from  interger model transparency
        /// </summary>
        public static float GetSingelTransparency ( int transparency )
        {
            return (float)( transparency - MinTransparency ) / ( MaxTransparency - MinTransparency );
        }

        #endregion
    }


    #region UITransparencyEditor specification

    internal class UITransparencyEditor : System.Drawing.Design.UITypeEditor
    {
        public override bool GetPaintValueSupported ( ITypeDescriptorContext context )
        {
            return true;
        }

        public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context )
        {
            if (context != null && context.Instance != null)
            {
                //if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return base.GetEditStyle ( context );
        }

        [RefreshProperties ( RefreshProperties.All )]
        public override object EditValue ( ITypeDescriptorContext context , System.IServiceProvider provider , object value )
        {
            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue ( provider , value );
            }

            GEMSSingle single = context.Instance as GEMSSingle;

            if (single != null)
            {
                TransparentSettingForm form = new TransparentSettingForm ( single.SingleColor );

                if (form.ShowDialog ( ) == System.Windows.Forms.DialogResult.OK)
                {
                    value = form.Transparency;
                }
                form.Dispose ( );
            }
            return value;
        }

        public override void PaintValue ( PaintValueEventArgs e )
        {
            GEMSSingle single = e.Context.Instance as GEMSSingle;

            if (single != null)
            {
                SolidBrush brush = new SolidBrush ( single.SingleColor );

                e.Graphics.FillRectangle ( brush , e.Bounds );

                brush.Dispose ( );
            }
            else
                base.PaintValue ( e );
        }

    }

    #endregion
}
