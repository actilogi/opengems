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
using System.Windows.Forms;

namespace GEMS.Designer
{
	// Commander-derived classes exist to hook up Windows Forms UI elements to 
	// Command objects using an adapter pattern.  The adapter approach permits a 
	// single Command object to sink events from dissimilar ui elements, and to set
	// state on dissimilar ui elements.
	//
	// This implementation provides Commanders for 
	// -menu items
	// -toolbar buttons
	//
	// Commanders exhibit the Adapter pattern in two ways:
	// - They adapt the different kinds of events fired by Clients (e.g. menu
	//   items and tool bar buttons) to invoke the Execute methods of their
	//   Commands.
	// - They adapt the EnableChanged events fired by Commands to set the
	//   Enabled property of their Clients.
	
	// base Commander class
	public abstract class Commander
	{
		protected Command m_command;
		protected abstract void HandleEnableChangedEvent(object sender, Command.EnableChangedEventArgs e);

		protected Commander(Command command)
		{
			m_command = command;
			m_command.EnableChanged += new Command.EnableChangedEventHandler(this.HandleEnableChangedEvent);
		}
	}
	
	// MenuItemCommander class
    public class ToolStripMenuItemCommander : Commander
	{
        private ToolStripMenuItem m_item;

		protected ToolStripMenuItemCommander(ToolStripMenuItem item, Command command) : base(command)
		{
			m_item = item;
			m_item.Click += new EventHandler(this.HandleUIEvent);
		}

		protected override void HandleEnableChangedEvent(object sender, Command.EnableChangedEventArgs e)
		{
			m_item.Enabled = e.IsEnabled;
		}

		private void HandleUIEvent(object sender, EventArgs e)
		{
            m_command.FiredItemName = this.m_item.Text.Trim();

			m_command.Execute();
		}

		// Connect is a shared (static) method that performs the task of adapting a menu
		// item to a command.  The commander exists only to wire up the two objects -- 
		// it is not used further
        public static void Connect(ToolStripMenuItem item, Command command)
		{
            ToolStripMenuItemCommander unused = new ToolStripMenuItemCommander(item, command);
		}       
        
	}
	
	public class ToolStripButtonCommander : Commander
	{
        private ToolStripButton m_button;

        protected ToolStripButtonCommander(ToolStripButton button, Command command)
            : base(command)
		{
			m_button = button;
            button.Click += new EventHandler(HandleUIEvent);
		}

		protected override void HandleEnableChangedEvent(object sender, Command.EnableChangedEventArgs e)
		{
			m_button.Enabled = e.IsEnabled;
		}

        private void HandleUIEvent(object sender, EventArgs e)
		{
			//if ()
			{
				m_command.Execute();
			}
		}

		// Connect is a shared (static) method that performs the task of adapting a toolbar
		// button to a command.  The commander exists only to wire up the two objects -- 
		// it is not used further
        public static void Connect(ToolStripButton button, Command command)
		{
            ToolStripButtonCommander unused = new ToolStripButtonCommander(button, command);
		}
	}
}
