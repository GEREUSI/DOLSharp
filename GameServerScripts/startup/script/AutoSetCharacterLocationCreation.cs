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
using System.Reflection;
using DOL.Database;
using log4net;

using DOL.Events;
using DOL.GS.ServerProperties;
using DOL.GS.PacketHandler;
using GameServerUtility;

namespace DOL.GS.GameEvents
{
	public static class AutoSetCharacterLocationCreation
	{		
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		
		[ScriptLoadedEvent]
		public static void OnScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			Utility.DEBUG_LOG("Set character initial location script loaded");
			GameEventMgr.AddHandler(DatabaseEvent.CharacterCreated, OnCharacterCreate);
		}

		[ScriptUnloadedEvent]
		public static void OnScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			GameEventMgr.RemoveHandler(DatabaseEvent.CharacterCreated, OnCharacterCreate);
		}
		
		public static void OnCharacterCreate(DOLEvent e, object sender, EventArgs args)
		{
			OnCharacterCreate(e, sender, args as CharacterEventArgs);
		}
		
		private static void OnCharacterCreate(DOLEvent e, object sender, CharacterEventArgs args)
		{
			if (args == null)
			{
				return;
			}
			
			switch ((eRealm)args.Character.Realm)
			{
				case eRealm.Hibernia: 
					UpdateCharacterPosition(args.Character, 238, 533492, 534970, 5408, 2491);
					break;
				case eRealm.Midgard:
					UpdateCharacterPosition(args.Character, 238, 570944, 540963, 5408, 1058);
					break;
				case eRealm.Albion:
					UpdateCharacterPosition(args.Character, 238, 562576, 573860, 5408, 3026);
					break;
			}
			
		}

		private static void UpdateCharacterPosition(DOLCharacters character,int region, int x, int y, int z, int heading)
		{
			character.Region = region;
			character.BindRegion = region;
			character.Xpos = x;
			character.BindXpos = x;
			character.Ypos = y;
			character.BindYpos = y;
			character.Zpos = z;
			character.BindZpos = z;
			character.BindHeading = heading;
		}
	}
}
