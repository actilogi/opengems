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
using System.ComponentModel;

using System.Xml;
using System.Xml.XPath;
using GEMS.Designer.Models;

namespace GEMS.Designer.Models.GeometryOperations
{
    public abstract class GeometryOperation
    {
        public enum GeometryOperationType
        {
            CreateCuboid = 0,
            CreateSphere,
            CreateRound,
            CreateRectangle,
            CreateLine,
            CreateCone,
            CreatePoint,
            CreateCylinder,
            Unknown,
        }

        private int id;

        /// <summary>
        /// This attribute expresses which coordinate the single referenced
        /// In this version this attribute does always equal 0 , which means
        /// we cannot change the coordinate.
        /// </summary>
        private const int relativeCS = 0;        
        private GEMSSingle parent;

        public GeometryOperation(XPathNavigator navigator, GEMSSingle parent)
        {
            this.parent = parent;
            this.id = int.Parse(navigator.GetAttribute("id", string.Empty));

        }

        public GeometryOperation(int id,GEMSSingle parent)
        {
            this.id = id;
            this.parent = parent;
        }
       
        public GeometryOperation(int id)
        {
            this.id = id;
        }

        public abstract string BuildOuterXmlString();
        public abstract GeometryOperation Clone(int newId,GEMSSingle parent);

        #region Properties

        [CategoryAttribute("Geometry"), ReadOnlyAttribute(true), DescriptionAttribute("Name of object")]
        public GeometryOperationType Name
        {
            get
            {
                try
                {
                    return (GeometryOperationType)Enum.Parse(typeof(GeometryOperationType),this.GetType().Name);
                }
                catch
                {
                    return GeometryOperationType.Unknown;
                }
            }
        }       

        [Browsable(false)]
        public int Id
        {
            get { return id; }
        }

        [Browsable(false)]
        public GEMSSingle Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        #endregion


        /// <summary>
        /// Create a new geometry operation object
        /// base the information in the xml
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GeometryOperation Create(XPathNavigator navigator, GEMSSingle parent)
        {
            GeometryOperation operation = null;

            try
            {
                //Get the type of the geometry operation
                GeometryOperation.GeometryOperationType operationType = (GeometryOperation.GeometryOperationType)Enum.Parse(typeof(GeometryOperation.GeometryOperationType), navigator.GetAttribute("name", string.Empty));

                //Create the geometry object based type
                switch (operationType)
                {
                    case GeometryOperation.GeometryOperationType.CreateCone:
                        operation = new CreateCone(navigator, parent);
                        break;
                    case GeometryOperation.GeometryOperationType.CreateCylinder:
                        operation = new CreateCylinder(navigator, parent);
                        break;
                    case GeometryOperation.GeometryOperationType.CreateCuboid:
                        operation = new CreateCuboid(navigator, parent);
                        break;
                    case GeometryOperation.GeometryOperationType.CreateLine:
                        operation = new CreateLine(navigator, parent);
                        break;
                    case GeometryOperation.GeometryOperationType.CreatePoint:
                        operation = new CreatePoint(navigator, parent);
                        break;
                    case GeometryOperation.GeometryOperationType.CreateRectangle:
                        operation = new CreateRectangle(navigator, parent);
                        break;
                    case GeometryOperation.GeometryOperationType.CreateSphere:
                        operation = new CreateSphere(navigator, parent);
                        break;
                    case GeometryOperation.GeometryOperationType.CreateRound:
                        operation = new CreateRound(navigator, parent);
                        break;
                    default:
                        break;
                }

            }
            catch
            {
                return operation;
            }

            return operation;
        }
    }
}
