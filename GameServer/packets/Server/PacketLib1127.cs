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
using DOL.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace DOL.GS.PacketHandler
{
	[PacketLib(1127, GameClient.eClientVersion.Version1127)]
	public class PacketLib1127 : PacketLib1126
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Constructs a new PacketLib for Client Version 1.127
		/// </summary>
		/// <param name="client">the gameclient this lib is associated with</param>
		public PacketLib1127(GameClient client)
			: base(client)
		{
		}
		
		public override void SendLoginGranted(byte color)
		{
			using (GSTCPPacketOut packet = new GSTCPPacketOut(GetPacketCode(eServerPackets.LoginGranted)))
			{
				packet.WritePascalString(m_gameClient.Account.Name);
				packet.WritePascalString(GameServer.Instance.Configuration.ServerNameShort); //server name
				packet.WriteByte(0x05);
				var type = color == 0 ? 7 : color;
				packet.WriteByte((byte)type);
				packet.WriteByte(0x00);
				SendTCP(packet);
			}
		}

		public override void SendMessage(string msg, eChatType type, eChatLoc loc)
		{
			// if (m_gameClient.ClientState == GameClient.eClientState.CharScreen)
			// {
			// 	return;
			// }

			GSTCPPacketOut pak = new GSTCPPacketOut(GetPacketCode(eServerPackets.Message));
			{                
				pak.WriteByte((byte)type);
				pak.WriteString(AssembleWithPrefix(loc, msg));
				SendTCP(pak);
			}
		}

		private static string AssembleWithPrefix(eChatLoc location, string message)
		{
			switch (location)
			{
				case eChatLoc.CL_ChatWindow:
					return "@@" + message;
				case eChatLoc.CL_PopupWindow:
					return "##" + message;
				default:
					return message;
			}
		}
	}
}
