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
/*
 * Author:	SmallHorse
 * Date:	10th August, 2004
 * This script should be put in /scripts/gameevents directory.
 * This event script demonstractes how to set callback functions
 * for some global server events as well as how to display a
 * custom dialog to the player and react to the response!
 */

using System;
using System.Reflection;
using DOL.Events;
using DOL.GS.PacketHandler;
using log4net;

namespace DOL.GS.GameEvents
{
	//First, declare our Event and have it implement the IGameEvent interface
	public class DOLTestServer
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		//This function is implemented from the IGameEvent
		//interface and is called on serverstart when the
		//events need to be initialized
		[ScriptLoadedEvent]
		public static void OnScriptCompiled(DOLEvent e, object sender, EventArgs args)
		{
			if (!ServerProperties.Properties.LOAD_EXAMPLES)
				return;
			//We want to be notified whenever a new character is created
			GameEventMgr.AddHandler(DatabaseEvent.CharacterCreated, new DOLEventHandler(DOLTestCharacterCreation));
			//We want to be notified whenever a player enters the world
			GameEventMgr.AddHandler(GamePlayerEvent.GameEntered, new DOLEventHandler(DOLTestPlayerEnterWorld));

			/* Not yet ;-)
			NonEnterableArea area1 = new NonEnterableArea("Teleport Area", 535405, 479515, 0, 2000);
			area1.Mode = eNonEnterableAreaMode.Teleport;
			NonEnterableArea area2 = new NonEnterableArea("Damage Area", 538405, 479515, 0, 2000);
			area2.Mode = eNonEnterableAreaMode.Damage;
			NonEnterableArea area3 = new NonEnterableArea("Instant Kill Area", 540405, 479515, 0, 2000);
			area3.Mode = eNonEnterableAreaMode.InstantKill;

			WorldMgr.GetRegion(1).AddArea(area1);
			WorldMgr.GetRegion(1).AddArea(area2);
			WorldMgr.GetRegion(1).AddArea(area3);
			*/
			//Output success message
			if (log.IsInfoEnabled)
				log.Info("DOLTestServer initialized");
		}

		[ScriptUnloadedEvent]
		public static void OnScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			if (!ServerProperties.Properties.LOAD_EXAMPLES)
				return;
			GameEventMgr.RemoveHandler(DatabaseEvent.CharacterCreated, new DOLEventHandler(DOLTestCharacterCreation));
			GameEventMgr.RemoveHandler(GamePlayerEvent.GameEntered, new DOLEventHandler(DOLTestPlayerEnterWorld));
		}

		//After registering for the OnPlayerEnterWorldFirstTime event this
		//function is called whenever a character enters the game
		public static void DOLTestPlayerEnterWorld(DOLEvent e, object sender, EventArgs args)
		{
			GamePlayer player = sender as GamePlayer;
			if (player == null)
				return;

			//We send a nice output message to the player when he enters the game
			player.Out.SendMessage("Welcome to the DAWN OF LIGHT Testserver!", eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}

		//After registering for the OnCharacterCreation Event this function
		//is called whenever a new character is created!
		public static void DOLTestCharacterCreation(DOLEvent e, object sender, EventArgs args)
		{
			CharacterEventArgs chArgs = args as CharacterEventArgs;
			if (chArgs == null)
				return;

			//We want our new characters to start with some money
			//chArgs.Character.Gold = 10;
			//chArgs.Character.Silver = 50;
			// since at least money loot is available we dont need start money
		}
	}
}