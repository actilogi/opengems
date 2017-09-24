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

namespace GEMS.Designer.Tools
{
    public abstract class Tool
    {
        protected MainForm workspace = null;

        public Tool(MainForm workspace)
        {
            this.workspace = workspace;
        }

        public void Activate()
        {
            OnActivate();
        }

         /// <summary>
        /// This method is called when the tool is being activated; that is, when the
        /// user has chosen to use this tool by clicking on it on a toolbar.
        /// </summary>
        protected virtual void OnActivate()
        {

        }

        public void Deactivate()
        {
            OnDeactivate();           
        }

        /// <summary>
        /// This method is called when the tool is being deactivated; that is, when the
        /// user has chosen to use another tool by clicking on another tool on a
        /// toolbar.
        /// </summary>
        protected virtual void OnDeactivate()
        {

        }

    }
}
