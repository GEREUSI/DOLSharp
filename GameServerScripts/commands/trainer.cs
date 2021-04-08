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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DOL.Database;
using DOL.GS.PacketHandler;
using GameServerUtility;

namespace DOL.GS.Commands
{
	[CmdAttribute(
		"&trainer",
		ePrivLevel.Player,
		"Teleport to a trainer",
		"/trainer")]
	public class TrainerCommandHandler : AbstractCommandHandler, ICommandHandler
	{
		public readonly string TrainerClassTemplate = "DOL.GS.Trainer.{0}Trainer";
		public void OnCommand(GameClient client, string[] args)
		{
			var player = client.Player;
			var trainerClassType = string.Format(TrainerClassTemplate, player.CharacterClass.Name);
			var trainers = GetPlayerTrainers(trainerClassType, player);
			if (args.Length == 1)
			{
				player.Out.SendMessage("Please select the trainer you want to teleport to:", eChatType.CT_Important, eChatLoc.CL_SystemWindow);
				foreach (var trainer in trainers)
				{
					player.Out.SendMessage($"{trainer.Key}) {trainer.Value.Name}", eChatType.CT_Important, eChatLoc.CL_SystemWindow);
				}
			} 
			else if (args.Length == 2)
			{
				try
				{
					var selection = int.Parse(args[1]);
					var trainer = trainers[selection];
					player.MoveTo(trainer.Region, trainer.X, trainer.Y, trainer.Z, trainer.Heading);
				}
				catch
				{
					DisplaySyntax(client);
				}
			}
			else
			{
				DisplaySyntax(client);
			}
		}

		private static Dictionary<int, Mob> GetPlayerTrainers(string trainerClassType, GamePlayer player)
		{
			const string whereExpression = "`ClassType` = @ClassType AND `Realm` = @Realm";
			var queryParameters = new List<QueryParameter>()
			{
				new QueryParameter("@ClassType", trainerClassType),
				new QueryParameter("@Realm", (int) player.Realm)
			};
			var i = 0;
			var trainers = GameServer.Database
				.SelectObjects<Mob>(whereExpression,queryParameters)
				.ToDictionary(mob => ++i, mob => mob);
			
			return trainers;
		}
	}
	
	
}