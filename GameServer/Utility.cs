using System;
using System.Reflection;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOL.Network;
using log4net;

namespace GameServerUtility
{
    public static class Utility
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void DEBUG_LOG(string message) => Log.Info($"DEV_DEBUG || {message}");
        public static void PACKET_LOG(GSPacketIn packet)
        {
            if (packet.ID == (int) eClientPackets.PingRequest)
            {
                return;
            }
                
            Log.Info($"\nDEV_PACKET || ID: {packet.ID} - {GetPacketName(packet.ID)}\n{packet.ToHumanReadable()}");
        }

        private static string GetPacketName(ushort id)
        {
            return Enum.GetName(typeof(eClientPackets), id);
        }

        public static void COMMAND_LOG(string command, string[] parsed = null, ScriptMgr.GameCommand gameCommand = null)
        {
            Log.Info($"\nDEV_COMMAND || Command:{command}\nParsed{parsed?.ToString() ?? string.Empty}\nGame Command:{gameCommand?.ToString() ?? string.Empty}");
        }
        
    }
}