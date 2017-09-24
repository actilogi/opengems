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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GEMS.Designer.Models;
using GEMS.Designer.Controls;

namespace GEMS.Designer.Forms
{
    public partial class MeshSizeDetailsForm : Form
    {
        private GEMSMesh mesh = null;
        private readonly Font normalFont;
        private readonly Font blodFont;
        private readonly FontFamily displayFontFamily = new FontFamily ( "Times New Roman" );

        public MeshSizeDetailsForm (GEMSMesh mesh)
        {
            InitializeComponent ( );

            this.mesh = mesh;

            normalFont = new Font ( displayFontFamily , 10 , FontStyle.Regular , GraphicsUnit.Point );
            blodFont = new Font ( displayFontFamily , 10 , FontStyle.Bold , GraphicsUnit.Point );

        }

        private void MeshSizeDetailsForm_Load ( object sender , EventArgs e )
        {
            StringBuilder messageBuilder = new StringBuilder ( );         

            string message = "\nX-direction :\n";
            ertxtMeshDetails.AppendTextAsRtf ( message , this.blodFont , GEMS.Designer.Controls.RtfColor.Blue );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "Number of Cells : {0}\n" , mesh.MeshCountInX );
            messageBuilder.AppendFormat ( "Max Cell Size : {0}\n" , mesh.MaxMeshCell.X );
            messageBuilder.AppendFormat ( "Min Cell Size : {0}\n" , mesh.MinMeshCell.X );
            messageBuilder.AppendFormat ( "Max Ratio : {0}\n" , mesh.MaxMeshCell.X / mesh.MinMeshCell.X );
            ertxtMeshDetails.AppendTextAsRtf ( messageBuilder.ToString() , this.normalFont , GEMS.Designer.Controls.RtfColor.Black );

            message = "\nY-direction :\n";
            ertxtMeshDetails.AppendTextAsRtf ( message , this.blodFont , GEMS.Designer.Controls.RtfColor.Blue );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "Number of Cells : {0}\n" , mesh.MeshCountInY );
            messageBuilder.AppendFormat ( "Max Cell Size : {0}\n" , mesh.MaxMeshCell.Y );
            messageBuilder.AppendFormat ( "Min Cell Size : {0}\n" , mesh.MinMeshCell.Y );
            messageBuilder.AppendFormat ( "Max Ratio : {0}\n" , mesh.MaxMeshCell.Y / mesh.MinMeshCell.Y );
            ertxtMeshDetails.AppendTextAsRtf ( messageBuilder.ToString ( ) , this.normalFont , GEMS.Designer.Controls.RtfColor.Black );

            message = "\nZ-direction :\n";
            ertxtMeshDetails.AppendTextAsRtf ( message , this.blodFont , GEMS.Designer.Controls.RtfColor.Blue );
            messageBuilder.Remove ( 0 , messageBuilder.Length );
            messageBuilder.AppendFormat ( "Number of Cells : {0}\n" , mesh.MeshCountInZ );
            messageBuilder.AppendFormat ( "Max Cell Size : {0}\n" , mesh.MaxMeshCell.Z );
            messageBuilder.AppendFormat ( "Min Cell Size : {0}\n" , mesh.MinMeshCell.Z );
            messageBuilder.AppendFormat ( "Max Ratio : {0}\n" , mesh.MaxMeshCell.Z / mesh.MinMeshCell.Z );
            ertxtMeshDetails.AppendTextAsRtf ( messageBuilder.ToString ( ) , this.normalFont , GEMS.Designer.Controls.RtfColor.Black );

        }        
        
    }
}