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
using System.Windows;
using System.Windows.Forms;

namespace GEMS.Designer.Controls
{
    /// <summary>
    /// This class will ensure the text be number
    /// </summary>
    public class NumberTextBox : TextBox
    {
        string oldValue = string.Empty;

        protected override void OnLeave(EventArgs e)
        {
            if (base.Text.Trim() != string.Empty)
            {
                float v = 0.0f;

                if (!float.TryParse(base.Text.Trim(), out v))
                    base.Text = oldValue;
            }
            base.OnLeave(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            oldValue = base.Text.Trim();

            base.OnEnter(e);
        }

        public float Value
        {
            get{
                float v;
                if (float.TryParse(base.Text.Trim(), out v))
                {
                    return v;
                }
                else
                    return 0.0f;
            }
            set { base.Text = value.ToString(); }
        }
    }
}
