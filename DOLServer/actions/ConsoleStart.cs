/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOLGameServerConsole;
using log4net;

namespace DOL.DOLServer.Actions
{
	/// <summary>
	/// Handles console start requests of the gameserver
	/// </summary>
	public class ConsoleStart : IAction
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// returns the name of this action
		/// </summary>
		public string Name
		{
			get { return "--start"; }
		}

		/// <summary>
		/// returns the syntax of this action
		/// </summary>
		public string Syntax
		{
			get { return "--start [-config=./config/serverconfig.xml]"; }
		}

		/// <summary>
		/// returns the description of this action
		/// </summary>
		public string Description
		{
			get { return "Starts the DOL server in console mode"; }
		}


		private static bool StartServer()
		{
			Console.WriteLine("Starting the server");
			bool start = GameServer.Instance.Start();
			return start;
		}

		// ReSharper disable once FunctionNeverReturns
		public void OnAction(Hashtable parameters)
		{
			Console.WriteLine("Starting GameServer ... please wait a moment!");
			FileInfo configFile;
			FileInfo currentAssembly = null;
			if (parameters["-config"] != null)
			{
				Console.WriteLine("Using config file: " + parameters["-config"]);
				configFile = new FileInfo((String)parameters["-config"]);
			}
			else
			{
				currentAssembly = new FileInfo(Assembly.GetEntryAssembly().Location);
				configFile = new FileInfo(currentAssembly.DirectoryName + Path.DirectorySeparatorChar + "config" + Path.DirectorySeparatorChar + "serverconfig.xml");
			}

			GameServerConfiguration config = new GameServerConfiguration();
			if (configFile.Exists)
			{
				config.LoadFromXMLFile(configFile);
			}
			else
			{
				if (!configFile.Directory.Exists)
					configFile.Directory.Create();
				config.SaveToXMLFile(configFile);
				if (File.Exists(currentAssembly.DirectoryName + Path.DirectorySeparatorChar + "DOLConfig.exe"))
				{
					Console.WriteLine("No config file found, launching with default config and embedded database... (SQLite)");
				}
			}

			GameServer.CreateInstance(config);
			StartServer();

			while (true) { }
		}
	}
}