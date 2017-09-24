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

namespace GEMS.Designer
{
	// The Command class is used to implement the Command pattern in Allen.
	// Each command manages a set of UI elements that provide a user different ways
	// of initiating the same action.  For example, an application might allow the 
	// user to Save data using the File menu, a toolbar button, and a context menu.  
	// A single Command object could manage the menu item, toolbar button and context
	// menu as a unit.
	//
	// Command objects provide a unified means of calling the action method, as well as 
	// setting the enabled state of all of the related ui elements using a single line
	// of code.
	public class Command
	{
		// The EnableChanged event is raised when the IsEnabled value of the command 
		// changes. This is handled in the Commander objects to set the Enabled property
		// of the managed controls.
		public delegate void EnableChangedEventHandler(object sender, Command.EnableChangedEventArgs e);
		public virtual event EnableChangedEventHandler EnableChanged;
		public delegate void Action();
		
		private Action m_action;
		private bool m_isEnabled = true;

        private string firedItemName = string.Empty;

		// The IsEnabled property is used to set/retrieve the enabled state of all UI
		// controls that the command manages
		public bool IsEnabled
		{
			get
			{
				return m_isEnabled;
			}

			set
			{
				if (m_isEnabled != value)
				{
					m_isEnabled = value;
					if (EnableChanged != null)
					{
						EnableChanged(this, new EnableChangedEventArgs(IsEnabled));
					}
				}
			}
		}

		public Command(Action action)
		{
			m_action = action;
		}

		// Invokes the method assigned to this command.
		public void Execute()
		{
			m_action();
		}
		
		// Arguments passed to the EnableChanged event.
		public class EnableChangedEventArgs : EventArgs
		{
			private bool m_isEnabled = false;

			public bool IsEnabled
			{
				get
				{
					return m_isEnabled;
				}
			}

			public EnableChangedEventArgs(bool isEnabled)
			{
				m_isEnabled = isEnabled;
			}
		}

        public string FiredItemName
        {
            get { return firedItemName; }
            set { firedItemName = value; }
        }
	}
}
