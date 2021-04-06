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
using System.Linq;
using DOL.Database;
using GameServerUtility;

namespace DOL.GS.PacketHandler.Client.v168
{
	[PacketHandler(PacketHandlerType.TCP, eClientPackets.RegionListRequest, "Handles sending the region overview", eClientStatus.None)]
	public class RegionListRequestHandler : IPacketHandler
	{
		public void HandlePacket(GameClient client, GSPacketIn packet)
		{
			if (client.Version >= GameClient.eClientVersion.Version1127)
			{
				var regionListRequestHandler1127 = new RegionListRequestHandler1127();
				regionListRequestHandler1127.HandlePacket(client, packet);
				return;
			}
			
			var slot = packet.ReadByte();
			if (slot >= 0x14)
				slot += 300 - 0x14;
			else if (slot >= 0x0A)
				slot += 200 - 0x0A;
			else
				slot += 100;
			var character = client.Account.Characters.FirstOrDefault(c => c.AccountSlot == slot);
			
			client.Out.SendRegions();
		}
	}
	
	

	public class RegionListRequestHandler1127 : IPacketHandler
	{
		public void HandlePacket(GameClient client, GSPacketIn packet)
		{
			byte charIndex = (byte)packet.ReadByte();
			
			if (client.Player == null && client.Account.Characters != null && client.ClientState == GameClient.eClientState.CharScreen)
			{
				var charSlot = CalculateCharSlot(client, charIndex);
				var character = FindCharacter(client, charSlot);
				if (character != null)
				{
					AuditMgr.AddAuditEntry(client, AuditType.Character, AuditSubtype.CharacterLogin, "", character.Name);
				}
				else
				{
					client.Player = null;
					client.ActiveCharIndex = -1;
				}
			}
			client.Out.SendRegions();
		}

		private static int CalculateCharSlot(GameClient client, byte charIndex)
		{
			int realmOffset = charIndex - (client.Account.Realm * 10 - 10);
			return client.Account.Realm * 100 + realmOffset;
		}

		private static DOLCharacters FindCharacter(GameClient client, int charSlot)
		{
			Utility.DEBUG_LOG($"Char Slot: {charSlot}");
			foreach (DOLCharacters character in client.Account.Characters)
			{
				if (character != null && character.AccountSlot == charSlot)
				{
					Utility.DEBUG_LOG(character.ToString());
					Utility.DEBUG_LOG(character.AccountSlot.ToString());
					client.LoadPlayer(character);
					return character;
				}
			}

			return null;
		}
	}
}
