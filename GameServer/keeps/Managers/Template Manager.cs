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
using DOL.AI.Brain;
using DOL.Database;
using DOL.GS.PacketHandler;
using DOL.GS.ServerProperties;
using DOL.Language;

namespace DOL.GS.Keeps
{
	public class TemplateMgr
	{
		public static void RefreshTemplate(GameKeepGuard guard)
		{
			SetGuardRealm(guard);
			SetGuardGuild(guard);
			SetGuardRespawn(guard);
			SetGuardGender(guard);
			SetGuardModel(guard);
			SetGuardName(guard);
			SetBlockEvadeParryChance(guard);
			SetGuardBrain(guard);
			SetGuardSpeed(guard);
			SetGuardLevel(guard);
			SetGuardResists(guard);
			SetGuardStats(guard);
			SetGuardAggression(guard);
			ClothingMgr.EquipGuard(guard);
			ClothingMgr.SetEmblem(guard);
		}

		private static void SetGuardRealm(GameKeepGuard guard)
		{
			if (guard.Component != null)
			{
				guard.Realm = guard.Component.AbstractKeep.Realm;

				if (guard.Realm != eRealm.None)
				{
					guard.ModelRealm = guard.Realm;
				}
				else
				{
					guard.ModelRealm = (eRealm)Util.Random(1, 3);
				}
			}
			else
			{
				guard.Realm = guard.CurrentZone.Realm;
				guard.ModelRealm = guard.Realm;
			}
		}

		private static void SetGuardGuild(GameKeepGuard guard)
		{
			if (guard.Component == null)
			{
				guard.GuildName = "";
			}
			else if (guard.Component.AbstractKeep.Guild == null)
			{
				guard.GuildName = "";
			}
			else
			{
				guard.GuildName = guard.Component.AbstractKeep.Guild.Name;
			}
		}

		private static void SetGuardRespawn(GameKeepGuard guard)
		{
			if (guard is FrontierHastener)
			{
				guard.RespawnInterval = 5000; // 5 seconds
			}
			else if (guard is GuardLord)
			{
				if (guard.Component != null)
				{
					guard.RespawnInterval = guard.Component.AbstractKeep.LordRespawnTime;
				}
				else
				{
					if (GameServer.Instance.Configuration.ServerType == eGameServerType.GST_PvE
						|| GameServer.Instance.Configuration.ServerType == eGameServerType.GST_PvP)
					{
						// In PvE & PvP servers, lords are really just mobs farmed for seals.
						int iVariance = 1000 * Math.Abs(ServerProperties.Properties.GUARD_RESPAWN_VARIANCE);
						int iRespawn = 60 * ((Math.Abs(ServerProperties.Properties.GUARD_RESPAWN) * 1000) +
							(Util.Random(-iVariance, iVariance)));

						guard.RespawnInterval = (iRespawn > 1000) ? iRespawn : 1000; // Make sure we don't end up with an impossibly low respawn interval.
					}
					else
						guard.RespawnInterval = 10000; // 10 seconds
				}
			}
			else if (guard is MissionMaster)
			{
				if (guard.Realm == eRealm.None && (GameServer.Instance.Configuration.ServerType == eGameServerType.GST_PvE ||
				GameServer.Instance.Configuration.ServerType == eGameServerType.GST_PvP))
				{
					// In PvE & PvP servers, lords are really just mobs farmed for seals.
					int iVariance = 1000 * Math.Abs(ServerProperties.Properties.GUARD_RESPAWN_VARIANCE);
					int iRespawn = 60 * ((Math.Abs(ServerProperties.Properties.GUARD_RESPAWN) * 1000) +
						(Util.Random(-iVariance, iVariance)));

					guard.RespawnInterval = (iRespawn > 1000) ? iRespawn : 1000; // Make sure we don't end up with an impossibly low respawn interval.
				}
				else
					guard.RespawnInterval = 10000; // 10 seconds
			}
			else
			{
				int iVariance = 1000 * Math.Abs(ServerProperties.Properties.GUARD_RESPAWN_VARIANCE);
				int iRespawn = 60 * ((Math.Abs(ServerProperties.Properties.GUARD_RESPAWN) * 1000) +
					(Util.Random(-iVariance, iVariance)));

				guard.RespawnInterval = Util.Random(60, 120) * 60 * 1000; // Make sure we don't end up with an impossibly low respawn interval.
			}
		}

		private static void SetGuardAggression(GameKeepGuard guard)
		{
			if (guard is GuardStaticCaster)
			{
				(guard.Brain as KeepGuardBrain).SetAggression(99, 1850);
			}
			else if (guard is GuardStaticArcher)
			{
				(guard.Brain as KeepGuardBrain).SetAggression(99, 2100);
			}
		}

		public static void SetGuardLevel(GameKeepGuard guard)
		{
			if (guard.Component != null)
			{
				guard.Component.AbstractKeep.SetGuardLevel(guard);
			}
		}

		private static void SetGuardGender(GameKeepGuard guard)
		{
			//portal keep guards are always male
			if (guard.IsPortalKeepGuard)
			{
                guard.Gender = eGender.Male;
			}
			else
			{
				if (Util.Chance(50))
				{
					guard.Gender = eGender.Male;
				}
				else
				{
                    guard.Gender = eGender.Female;
				}
			}
		}


		#region Hastener Models
		public static ushort AlbionHastener = 244;
		public static ushort MidgardHastener = 22;
		public static ushort HiberniaHastener = 1910;
		#endregion

		#region AlbionClassModels
		public static ushort BritonMale = 32;
		public static ushort BritonFemale = 35;
		public static ushort HighlanderMale = 39;
		public static ushort HighlanderFemale = 43;
		public static ushort SaracenMale = 48;
		public static ushort SaracenFemale = 52;
		public static ushort AvalonianMale = 61;
		public static ushort AvalonianFemale = 65;
		public static ushort IcconuMale = 716;
		public static ushort IcconuFemale = 724;
		public static ushort HalfOgreMale = 1008;
		public static ushort HalfOgreFemale = 1020;
        public static ushort MinotaurMaleAlb = 1395;
		#endregion
		#region MidgardClassModels
		public static ushort TrollMale = 137;
		public static ushort TrollFemale = 145;
		public static ushort NorseMale = 503;
		public static ushort NorseFemale = 507;
		public static ushort KoboldMale = 169;
		public static ushort KoboldFemale = 177;
		public static ushort DwarfMale = 185;
		public static ushort DwarfFemale = 193;
		public static ushort ValkynMale = 773;
		public static ushort ValkynFemale = 781;
		public static ushort FrostalfMale = 1051;
		public static ushort FrostalfFemale = 1063;
        public static ushort MinotaurMaleMid = 1407;
		#endregion
		#region HiberniaClassModels
		public static ushort FirbolgMale = 286;
		public static ushort FirbolgFemale = 294;
		public static ushort CeltMale = 302;
		public static ushort CeltFemale = 310;
		public static ushort LurikeenMale = 318;
		public static ushort LurikeenFemale = 326;
		public static ushort ElfMale = 334;
		public static ushort ElfFemale = 342;
		public static ushort SharMale = 1075;
		public static ushort SharFemale = 1087;
		public static ushort SylvianMale = 700;
		public static ushort SylvianFemale = 708;
        public static ushort MinotaurMaleHib = 1419;
		#endregion

		/// <summary>
		/// Sets a guards model
		/// </summary>
		/// <param name="guard">The guard object</param>
		private static void SetGuardModel(GameKeepGuard guard)
		{
			if(!ServerProperties.Properties.AUTOMODEL_GUARDS_LOADED_FROM_DB && !guard.LoadedFromScript)
			{
				return;
			}
			if (guard is FrontierHastener)
			{
				switch (guard.Realm)
				{
					case eRealm.None:
					case eRealm.Albion:
						{
							guard.Model = AlbionHastener;
							guard.Size = 45;
							break;
						}
					case eRealm.Midgard:
						{
							guard.Model = MidgardHastener;
							guard.Size = 50;
							guard.Flags ^= GameNPC.eFlags.GHOST;
							break;
						}
					case eRealm.Hibernia:
						{
							guard.Model = HiberniaHastener;
							guard.Size = 45;
							break;
						}
				}
				return;
			}

			switch (guard.ModelRealm)
			{
				#region None
				case eRealm.None:
				#endregion
				#region Albion
				case eRealm.Albion:
					{
						if (guard is GuardArcher)
						{
							if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = SaracenMale; break;//Saracen Male
									case 1: guard.Model = HighlanderMale; break;//Highlander Male
									case 2: guard.Model = BritonMale; break;//Briton Male
									case 3: guard.Model = IcconuMale; break;//Icconu Male
								}
							}
							else
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = SaracenFemale; break;//Saracen Female
									case 1: guard.Model = HighlanderFemale; break;//Highlander Female
									case 2: guard.Model = BritonFemale; break;//Briton Female
									case 3: guard.Model = IcconuFemale; break;//Icconu Female
								}
							}
						}
						else if (guard is GuardCaster)
						{
							if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = AvalonianMale; break;//Avalonian Male
									case 1: guard.Model = BritonMale; break;//Briton Male
									case 2: guard.Model = HalfOgreMale; break;//Half Ogre Male
								}
							}
							else
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = AvalonianFemale; break;//Avalonian Female
									case 1: guard.Model = BritonFemale; break;//Briton Female
									case 2: guard.Model = HalfOgreFemale; break;//Half Ogre Female
								}
							}
						}
						else if (guard is GuardFighter)
						{
							if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 6))
								{
									case 0: guard.Model = HighlanderMale; break;//Highlander Male
									case 1: guard.Model = BritonMale; break;//Briton Male
									case 2: guard.Model = SaracenMale; break;//Saracen Male
									case 3: guard.Model = AvalonianMale; break;//Avalonian Male
									case 4: guard.Model = HalfOgreMale; break;//Half Ogre Male
									case 5: guard.Model = IcconuMale; break;//Icconu Male
                                    case 6: guard.Model = MinotaurMaleAlb; break;//Minotuar
								}
							}
							else
							{
								switch (Util.Random(0, 5))
								{
									case 0: guard.Model = HighlanderFemale; break;//Highlander Female
									case 1: guard.Model = BritonFemale; break;//Briton Female
									case 2: guard.Model = SaracenFemale; break;//Saracen Female
									case 3: guard.Model = AvalonianFemale; break;//Avalonian Female
									case 4: guard.Model = HalfOgreFemale; break;//Half Ogre Female
									case 5: guard.Model = IcconuFemale; break;//Icconu Female
								}
							}
						}
						else if (guard is GuardHealer)
						{
							if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = HighlanderMale; break;//Highlander Male
									case 1: guard.Model = BritonMale; break;//Briton Male
									case 2: guard.Model = AvalonianMale; break;//Avalonian Male
								}
							}
							else
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = HighlanderFemale; break;//Highlander Female
									case 1: guard.Model = BritonFemale; break;//Briton Female
									case 2: guard.Model = AvalonianFemale; break;//Avalonian Female
								}
							}
						}
						else if (guard is GuardLord || guard is MissionMaster)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = HighlanderMale; break;//Highlander Male
									case 1: guard.Model = BritonMale; break;//Briton Male
									case 2: guard.Model = AvalonianMale; break;//Avalonian Male
                                    case 3: guard.Model = MinotaurMaleAlb; break;//Minotaur
								}
							}
							else
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = HighlanderFemale; break;//Highlander Female
									case 1: guard.Model = BritonFemale; break;//Briton Female
									case 2: guard.Model = AvalonianFemale; break;//Avalonian Female
								}
							}
						}
						else if (guard is GuardStealther)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = SaracenMale; break;//Saracen Male
									case 1: guard.Model = BritonMale; break;//Briton Male
									case 2: guard.Model = IcconuMale; break;//Icconu Male
								}
							}
							else
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = SaracenFemale; break;//Saracen Female
									case 1: guard.Model = BritonFemale; break;//Briton Female
									case 2: guard.Model = IcconuFemale; break;//Icconu Female
								}
							}
						}
						break;
					}
				#endregion
				#region Midgard
				case eRealm.Midgard:
					{
						if (guard is GuardArcher)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 4))
								{
									case 0: guard.Model = NorseMale; break;//Norse Male
									case 1: guard.Model = KoboldMale; break;//Kobold Male
									case 2: guard.Model = DwarfMale; break;//Dwarf Male
									case 3: guard.Model = ValkynMale; break;//Valkyn Male
									case 4: guard.Model = FrostalfMale; break;//Frostalf Male
								}
							}
							else
							{
								switch (Util.Random(0, 4))
								{
									case 0: guard.Model = NorseFemale; break;//Norse Female
									case 1: guard.Model = KoboldFemale; break;//Kobold Female
									case 2: guard.Model = DwarfFemale; break;//Dwarf Female
									case 3: guard.Model = ValkynFemale; break;//Valkyn Female
									case 4: guard.Model = FrostalfFemale; break;//Frostalf Female
								}
							}
						}
						else if (guard is GuardCaster)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = KoboldMale; break;//Kobold Male
									case 1: guard.Model = NorseMale; break;//Norse Male
									case 2: guard.Model = DwarfMale; break;//Dwarf Male
									case 3: guard.Model = FrostalfMale; break;//Frostalf Male
								}
							}
							else
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = KoboldFemale; break;//Kobold Female
									case 1: guard.Model = NorseFemale; break;//Norse Female
									case 2: guard.Model = DwarfFemale; break;//Dwarf Female
									case 3: guard.Model = FrostalfFemale; break;//Frostalf Female
								}
							}
						}
						else if (guard is GuardFighter)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 5))
								{
									case 0: guard.Model = TrollMale; break;//Troll Male
									case 1: guard.Model = NorseMale; break;//Norse Male
									case 2: guard.Model = DwarfMale; break;//Dwarf Male
									case 3: guard.Model = KoboldMale; break;//Kobold Male
									case 4: guard.Model = ValkynMale; break;//Valkyn Male
                                    case 5: guard.Model = MinotaurMaleMid; break;//Minotaur
								}
							}
							else
							{
								switch (Util.Random(0, 4))
								{
									case 0: guard.Model = TrollFemale; break;//Troll Female
									case 1: guard.Model = NorseFemale; break;//Norse Female
									case 2: guard.Model = DwarfFemale; break;//Dwarf Female
									case 3: guard.Model = KoboldFemale; break;//Kobold Female
									case 4: guard.Model = ValkynFemale; break;//Valkyn Female
								}
							}
						}
						else if (guard is GuardHealer)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = DwarfMale; break;//Dwarf Male
									case 1: guard.Model = NorseMale; break;//Norse Male
									case 2: guard.Model = FrostalfMale; break;//Frostalf Male
								}
							}
							else
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = DwarfFemale; break;//Dwarf Female
									case 1: guard.Model = NorseFemale; break;//Norse Female
									case 2: guard.Model = FrostalfFemale; break;//Frostalf Female
								}
							}
						}
						else if (guard is GuardLord || guard is MissionMaster)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 4))
								{
									case 0: guard.Model = DwarfMale; break;//Dwarf Male
									case 1: guard.Model = NorseMale; break;//Norse Male
									case 2: guard.Model = TrollMale; break;//Troll Male
									case 3: guard.Model = KoboldMale; break;//Kobold Male
                                    case 4: guard.Model = MinotaurMaleMid; break;//Minotaur
								}
							}
							else
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = DwarfFemale; break;//Dwarf Female
									case 1: guard.Model = NorseFemale; break;//Norse Female
									case 2: guard.Model = TrollFemale; break;//Troll Female
									case 3: guard.Model = KoboldFemale; break;//Kobold Female
								}
							}
						}
						else if (guard is GuardStealther)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = KoboldMale; break;//Kobold Male
									case 1: guard.Model = NorseMale; break;//Norse Male
									case 2: guard.Model = ValkynMale; break;//Valkyn Male
								}
							}
							else
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = KoboldFemale; break;//Kobold Female
									case 1: guard.Model = NorseFemale; break;//Norse Female
									case 2: guard.Model = ValkynFemale; break;//Valkyn Female
								}
							}
						}
						break;
					}
				#endregion
				#region Hibernia
				case eRealm.Hibernia:
					{
						if (guard is GuardArcher)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = LurikeenMale; break;//Lurikeen Male
									case 1: guard.Model = ElfMale; break;//Elf Male
									case 2: guard.Model = CeltMale; break;//Celt Male
									case 3: guard.Model = SharMale; break;//Shar Male
								}
							}
							else
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = LurikeenFemale; break;//Lurikeen Female
									case 1: guard.Model = ElfFemale; break;//Elf Female
									case 2: guard.Model = CeltFemale; break;//Celt Female
									case 3: guard.Model = SharFemale; break;//Shar Female
								}
							}
						}
						else if (guard is GuardCaster)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 1))
								{
									case 0: guard.Model = ElfMale; break;//Elf Male
									case 1: guard.Model = LurikeenMale; break;//Lurikeen Male
								}
							}
							else
							{
								switch (Util.Random(0, 1))
								{
									case 0: guard.Model = ElfFemale; break;//Elf Female
									case 1: guard.Model = LurikeenFemale; break;//Lurikeen Female
								}
							}
						}
						else if (guard is GuardFighter)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 4))
								{
									case 0: guard.Model = FirbolgMale; break;//Firbolg Male
									case 1: guard.Model = LurikeenMale; break;//Lurikeen Male
									case 2: guard.Model = CeltMale; break;//Celt Male
									case 3: guard.Model = SharMale; break;//Shar Male
                                    case 4: guard.Model = MinotaurMaleHib; break;//Minotaur
								}
							}
							else
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = FirbolgFemale; break;//Firbolg Female
									case 1: guard.Model = LurikeenFemale; break;//Lurikeen Female
									case 2: guard.Model = CeltFemale; break;//Celt Female
									case 3: guard.Model = SharFemale; break;//Shar Female
								}
							}
						}
						else if (guard is GuardHealer)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = CeltMale; break;//Celt Male
									case 1: guard.Model = FirbolgMale; break;//Firbolg Male
									case 2: guard.Model = SylvianMale; break;//Sylvian Male
								}
							}
							else
							{
								switch (Util.Random(0, 2))
								{
									case 0: guard.Model = CeltFemale; break;//Celt Female
									case 1: guard.Model = FirbolgFemale; break;//Firbolg Female
									case 2: guard.Model = SylvianFemale; break;//Sylvian Female
								}
							}
						}
						else if (guard is GuardLord || guard is MissionMaster)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 4))
								{
									case 0: guard.Model = CeltMale; break;//Celt Male
									case 1: guard.Model = FirbolgMale; break;//Firbolg Male
									case 2: guard.Model = LurikeenMale; break;//Lurikeen Male
									case 3: guard.Model = ElfMale; break;//Elf Male
                                    case 4: guard.Model = MinotaurMaleHib; break;//Minotaur
								}
							}
							else
							{
								switch (Util.Random(0, 3))
								{
									case 0: guard.Model = CeltFemale; break;//Celt Female
									case 1: guard.Model = FirbolgFemale; break;//Firbolg Female
									case 2: guard.Model = LurikeenFemale; break;//Lurikeen Female
									case 3: guard.Model = ElfFemale; break;//Elf Female
								}
							}
						}
						else if (guard is GuardStealther)
						{
                            if (guard.Gender == eGender.Male)
							{
								switch (Util.Random(0, 1))
								{
									case 0: guard.Model = ElfMale; break;//Elf Male
									case 1: guard.Model = LurikeenMale; break;//Lurikeen Male
								}
							}
							else
							{
								switch (Util.Random(0, 1))
								{
									case 0: guard.Model = ElfFemale; break;//Elf Female
									case 1: guard.Model = LurikeenFemale; break;//Lurikeen Female
								}
							}
						}
						break;
					}
				#endregion
			}
		}

        /// <summary>
        /// Gets short name of keeps
        /// </summary>
        /// <param name="KeepName">Complete name of the Keep</param>
        private static string GetKeepShortName(string KeepName)
        {
            string ShortName;
            if (KeepName.StartsWith("Caer"))//Albion
            {
                ShortName = KeepName.Substring(5);
            }
			else if (KeepName.StartsWith("Fort"))
			{
				ShortName = KeepName.Substring(5);
			}
			else if (KeepName.StartsWith("Dun"))//Hibernia
            {
                if (KeepName == "Dun nGed")
                {
                    ShortName = "Ged";
                }
                else if (KeepName == "Dun da Behn")
                {
                    ShortName = "Behn";
                }
                else
                {
                    ShortName = KeepName.Substring(4);
                }
            }
			else if (KeepName.StartsWith("Castle"))// Albion Relic
			{
				ShortName = KeepName.Substring(7);
			}
			else//Midgard
			{
				if (KeepName.Contains(" "))
					ShortName = KeepName.Substring(0, KeepName.IndexOf(" ", 0));
				else
					ShortName = KeepName;
			}
            return ShortName;
        }

		/// <summary>
		/// Sets a guards name
		/// </summary>
		/// <param name="guard">The guard object</param>
		private static void SetGuardName(GameKeepGuard guard)
		{
			if (guard is FrontierHastener)
			{
                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Hastener");
				return;
			}
			if (guard is GuardLord)
			{
				if (guard.Component == null)
				{
                    guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Commander", guard.CurrentZone.Description);
                    return;
				}
				else if (guard.IsTowerGuard)
				{
                    guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.TowerCaptain");
                    return;
				}
			}
			switch (guard.ModelRealm)
			{
				#region Albion / None
				case eRealm.None:
				case eRealm.Albion:
					{
						if (guard is GuardArcher)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.BowmanCommander");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Scout");
						}
						else if (guard is GuardCaster)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.MasterWizard");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Wizard");
						}
						else if (guard is GuardFighter)
						{
							if (guard.IsPortalKeepGuard)
							{
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.KnightCommander");
                            }
							else
							{
                                if (guard.Gender == eGender.Male)
                                    guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Armsman");
                                else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Armswoman");
							}
						}
						else if (guard is GuardHealer)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Cleric");
                        }
						else if (guard is GuardLord)
						{
                            if (guard.Gender == eGender.Male)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Lord", GetKeepShortName(guard.Component.AbstractKeep.Name));
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Lady", GetKeepShortName(guard.Component.AbstractKeep.Name));
						}
						else if (guard is GuardStealther)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Infiltrator");
                        }
						else if (guard is MissionMaster)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.CaptainCommander");
                        }
						break;
					}
				#endregion
				#region Midgard
				case eRealm.Midgard:
					{
						if (guard is GuardArcher)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.NordicHunter");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Hunter");
						}
						else if (guard is GuardCaster)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.MasterRunes");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Runemaster");
						}
						else if (guard is GuardFighter)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.NordicJarl");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Huscarl");
						}
						else if (guard is GuardHealer)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Healer");
                        }
						else if (guard is GuardLord)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Jarl", GetKeepShortName(guard.Component.AbstractKeep.Name));
                        }
						else if (guard is GuardStealther)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Shadowblade");
                        }
						else if (guard is MissionMaster)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.HersirCommander");
                        }
						break;
					}
				#endregion
				#region Hibernia
				case eRealm.Hibernia:
					{
						if (guard is GuardArcher)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.MasterRanger");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Ranger");
						}
						else if (guard is GuardCaster)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.MasterEldritch");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Eldritch");
						}
						else if (guard is GuardFighter)
						{
							if (guard.IsPortalKeepGuard)
                                guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Champion");
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Guardian");
						}
						else if (guard is GuardHealer)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Druid");
                        }
						else if (guard is GuardLord)
						{
                            if (guard.Gender == eGender.Male)
                               guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Chieftain", GetKeepShortName(guard.Component.AbstractKeep.Name));
                            else guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Chieftess", GetKeepShortName(guard.Component.AbstractKeep.Name));
                        }
						else if (guard is GuardStealther)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Nightshade");
                        }
						else if (guard is MissionMaster)
						{
                            guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.ChampionCommander");
                        }
						break;
					}
				#endregion
			}

			if (guard.Realm == eRealm.None)
			{
				guard.Name = LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "SetGuardName.Renegade", guard.Name);
			}
        }

		/// <summary>
		/// Sets a guards Block, Parry and Evade change
		/// </summary>
		/// <param name="guard">The guard object</param>
		private static void SetBlockEvadeParryChance(GameKeepGuard guard)
		{
			guard.BlockChance = 0;
			guard.EvadeChance = 0;
			guard.ParryChance = 0;

			if (guard is GuardLord || guard is MissionMaster)
			{
				guard.BlockChance = 15;
				guard.ParryChance = 15;

				if (guard.ModelRealm != eRealm.Albion)
				{
					guard.EvadeChance = 10;
					guard.ParryChance = 5;
				}
			}
			else if (guard is GuardStealther)
			{
				guard.EvadeChance = 30;
			}
			else if (guard is GuardFighter)
			{
				guard.BlockChance = 10;
				guard.ParryChance = 10;

				if (guard.ModelRealm != eRealm.Albion)
				{
					guard.EvadeChance = 5;
					guard.ParryChance = 5;
				}
			}
			else if (guard is GuardHealer)
			{
				guard.BlockChance = 5;
			}
			else if (guard is GuardArcher)
			{
				if (guard.ModelRealm == eRealm.Albion)
				{
					guard.BlockChance = 10;
					guard.EvadeChance = 5;
				}
				else
				{
					guard.EvadeChance = 15;
				}
			}
		}

		/// <summary>
		/// Sets the guards brain
		/// </summary>
		/// <param name="guard">The guard object</param>
		public static void SetGuardBrain(GameKeepGuard guard)
		{
			if (guard.Brain is KeepGuardBrain == false)
			{
				KeepGuardBrain brain = new KeepGuardBrain();
				if (guard is GuardCaster)
					brain = new CasterBrain();
				else if (guard is GuardHealer)
					brain = new HealerBrain();
				else if (guard is GuardLord)
					brain = new LordBrain();

				guard.AddBrain(brain);
				brain.guard = guard;
			}

			if (guard is MissionMaster)
			{
				(guard.Brain as KeepGuardBrain).SetAggression(90, 400);
			}
		}

		/// <summary>
		/// Sets the guards speed
		/// </summary>
		/// <param name="guard">The guard object</param>
		public static void SetGuardSpeed(GameKeepGuard guard)
		{
			if (guard.IsPortalKeepGuard)
			{
				guard.MaxSpeedBase = 575;
			}
			if ((guard is GuardLord && guard.Component != null) || guard is GuardStaticArcher || guard is GuardStaticCaster)
			{
				guard.MaxSpeedBase = 0;
			}
			else if (guard.Level < 250)
			{
				if (guard.Realm == eRealm.None)
				{
					guard.MaxSpeedBase = 200;
				}
				else if (guard.Level < 50)
				{
					guard.MaxSpeedBase = 210;
				}
				else
				{
					guard.MaxSpeedBase = 250;
				}
			}
			else
			{
				guard.MaxSpeedBase = 575;
			}
		}

		/// <summary>
		/// Sets a guards resists
		/// </summary>
		/// <param name="guard">The guard object</param>
		private static void SetGuardResists(GameKeepGuard guard)
		{
			for (int i = (int)eProperty.Resist_First; i <= (int)eProperty.Resist_Last; i++)
			{
				if (guard is GuardLord)
				{
					guard.BaseBuffBonusCategory[i] = 40;
				}
				else if (guard.Level < 50)
				{
					guard.BaseBuffBonusCategory[i] = guard.Level / 2 + 1;
				}
				else
				{
					guard.BaseBuffBonusCategory[i] = 26;
				}
			}
		}

		/// <summary>
		/// Sets a guards stats
		/// </summary>
		/// <param name="guard">The guard object</param>
		private static void SetGuardStats(GameKeepGuard guard)
		{
			if (guard is GuardLord)
			{
				guard.Strength = (short)(Properties.LORD_AUTOSET_STR_BASE + (10 * guard.Level * Properties.LORD_AUTOSET_STR_MULTIPLIER));
				guard.Dexterity = (short)(Properties.LORD_AUTOSET_DEX_BASE + (guard.Level * Properties.LORD_AUTOSET_DEX_MULTIPLIER));
				guard.Constitution = (short)(Properties.LORD_AUTOSET_CON_BASE + (guard.Level * Properties.LORD_AUTOSET_CON_MULTIPLIER));
				guard.Quickness = (short)(Properties.LORD_AUTOSET_QUI_BASE + (guard.Level * Properties.LORD_AUTOSET_QUI_MULTIPLIER));
				guard.Intelligence = (short)(Properties.LORD_AUTOSET_INT_BASE + (guard.Level * Properties.LORD_AUTOSET_INT_MULTIPLIER));
			}
			else
			{
				guard.Strength = (short)(Properties.GUARD_AUTOSET_STR_BASE + (10 * guard.Level * Properties.GUARD_AUTOSET_STR_MULTIPLIER));
				guard.Dexterity = (short)(Properties.GUARD_AUTOSET_DEX_BASE + (guard.Level * Properties.GUARD_AUTOSET_DEX_MULTIPLIER));
				guard.Constitution = (short)(Properties.GUARD_AUTOSET_CON_BASE + (guard.Level * Properties.GUARD_AUTOSET_CON_MULTIPLIER));
				guard.Quickness = (short)(Properties.GUARD_AUTOSET_QUI_BASE + (guard.Level * Properties.GUARD_AUTOSET_QUI_MULTIPLIER));
				guard.Intelligence = (short)(Properties.GUARD_AUTOSET_INT_BASE + (guard.Level * Properties.GUARD_AUTOSET_INT_MULTIPLIER));
			}
		}
	}
}
