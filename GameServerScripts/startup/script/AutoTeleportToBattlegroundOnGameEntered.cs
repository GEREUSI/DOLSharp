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
using System.ComponentModel.Design;
using System.Reflection;
using DOL.Database;
using log4net;

using DOL.Events;
using DOL.GS.ServerProperties;
using DOL.GS.PacketHandler;
using GameServerUtility;

namespace DOL.GS.GameEvents
{
	public static class AutoTeleportToBattlegroundOnGameEntered
	{		
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		
		[ScriptLoadedEvent]
		public static void OnScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			Utility.DEBUG_LOG("Teleport to battle ground script loaded");
			GameEventMgr.AddHandler(GamePlayerEvent.GameEntered, OnGameEntered);
		}

		[ScriptUnloadedEvent]
		public static void OnScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			GameEventMgr.RemoveHandler(GamePlayerEvent.GameEntered, OnGameEntered);
		}
		
		public static void OnGameEntered(DOLEvent e, object sender, EventArgs args)
		{
			OnGameEntered(e, sender, args as CharacterEventArgs);
		}
		
		private static void OnGameEntered(DOLEvent e, object sender, CharacterEventArgs args)
		{
			GamePlayer player = sender as GamePlayer;
			if (player == null)
				return;
			switch (player.Realm)
			{
				case eRealm.Albion: player.MoveTo(238, 563499, 573780, 5408, 4094);; break;
				case eRealm.Midgard: player.MoveTo(238, 570011, 541058, 5408, 2059);; break;
				case eRealm.Hibernia: player.MoveTo(238, 534198, 534372, 5408, 1521);; break;
			}
			
		}
	}
}
