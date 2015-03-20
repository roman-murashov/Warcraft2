using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Networking
{
    public static class Network
    {
        static GameClient gameClient;
        static GameServer gameServer;
        public static int port = 5000;

        public static void StartClient()
        {
            if(gameClient == null)
            {
                gameClient = new GameClient();
            }
        }

        public static void StartServer()
        {
            if(gameServer == null)
            {
                gameServer = new GameServer(port);
                gameServer.Start();
            }
        }

        public static GameClient getGameClient()
        {
            return gameClient;
        }

        public static GameServer getGameServer()
        {
            return gameServer;
        }

        public static int GetFirstFreePort()
        {
            var startingAtPort = 5000;
            var maxNumberOfPortsToCheck = 500;
            var range = Enumerable.Range(startingAtPort, maxNumberOfPortsToCheck);
            var portsInUse =
                from p in range
                join used in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners()
                    on p equals used.Port
                select p;

            var FirstFreeUDPPortInRange = range.Except(portsInUse).FirstOrDefault();

            if(FirstFreeUDPPortInRange > 0)
            {
                return FirstFreeUDPPortInRange;
            }

            return -1;
        }

        public static void UpdateNetwork()
        {
            if(gameServer != null)
            {
                gameServer.ReadMessages();
            }

            if(gameClient != null)
            {
                gameClient.ReadMessages();
            }
        }

        public static Boolean IsServer()
        {
            return gameServer != null;
        }
    }
}
