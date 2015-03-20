using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Lidgren.Network;
using Warcraft2.Application.Core;

namespace Warcraft2.Application.Networking
{
    public static class NetworkCommands
    {
        public static MethodInfo mi;

        public enum NetworkCommand
        {
            ReceiveFrame,
            ReceivePlayerActions,
            ReceiveWorld,
            ReceiveGameStart,
            ReceiveEndTurn
        }

        public static void Invoke(NetIncomingMessage msg, Boolean isServer)
        {
            try
            {
                NetworkCommand command = (NetworkCommand)Enum.Parse(typeof(NetworkCommand), msg.ReadString());
                mi = typeof(NetworkCommands).GetMethod(command.ToString());

                if(mi != null)
                {
                    Object o = mi.Invoke(null, new object[] { msg, isServer });
                }
            }
            catch(Exception e)
            {
                RailorLibrary.Data.Cout.W(e.ToString());
            }
        }

        public static void SendFrame()
        {
            NetOutgoingMessage outMsg = GetOutgoingMsg(NetworkCommand.ReceiveFrame,true);
            int currentFrame = Engine.gameEngine.GetCurrentFrame() + 1;
            outMsg.Write(currentFrame);
            SendMessage(outMsg, true);
        }

        public static void ReceiveFrame(NetIncomingMessage msg, Boolean isServer)
        {
            int currentFrame = msg.ReadInt32();
            Engine.DoFrame(currentFrame);
        }

        //public static void SendPlayerActions(List<PlayerAction> playerActions, Boolean isServer)
        //{
        //    String serializedPlayerActions = Serializer.SerializeObject(playerActions);
        //    NetOutgoingMessage outMsg = GetOutgoingMsg(NetworkCommand.ReceivePlayerActions, isServer);
        //    outMsg.Write(serializedPlayerActions);
        //    SendMessage(outMsg, isServer);
        //}

        //public static void ReceivePlayerActions(NetIncomingMessage msg, Boolean isServer)
        //{
        //    String actions = msg.ReadString();
        //    List<PlayerAction> playerActions = Serializer.DeserializeObject<List<PlayerAction>>(actions);
        //    if(isServer)
        //    {
        //        List<PlayerAction> validActions = new List<PlayerAction>();
        //        foreach(PlayerActionInterface pA in playerActions)
        //        {
        //            if(pA.IsValid())
        //            {
        //                validActions.Add((PlayerAction)pA);
        //            }
        //        }

        //        NetOutgoingMessage m = GetOutgoingMsg(NetworkCommand.ReceivePlayerActions, isServer);
        //        m.Write(actions);
        //        SendMessage(m, isServer);
        //    }
        //    else
        //    {
        //        Engine.world.worldActions.AddAction(playerActions);
        //    }
        //}
        //public static void SendWorld(World world, Boolean isServer, NetConnection connection = null)
        //{
        //    NetOutgoingMessage outMsg = GetOutgoingMsg(NetworkCommand.ReceiveWorld, isServer);
        //    outMsg.Write(Serializer.SerializeObject(world));
        //    SendMessage(outMsg, isServer, connection);
        //}
        //public static void ReceiveWorld(NetIncomingMessage msg, Boolean isServer)
        //{
        //    String worldString = msg.ReadString();
        //    World world = Serializer.DeserializeObject<World>(worldString);
        //    Engine.world = world;
        //}

        //public static void SendGameStart(World world, Boolean isServer, NetConnection connection = null)
        //{
        //    GameServer server = Engine.server;
        //    List<NetConnection> connections = server.GetNetServer().Connections;
        //    Players players = new Players();
        //    Random r = new Random();
        //    int playerCounter = 0;
        //    foreach(NetConnection con in connections)
        //    {
        //        Player p = new Player();
        //        p.id = r.Next();
        //        p.idCount = playerCounter;
        //        p.color = Engine.playerColors[playerCounter];
        //        p.SetupResources();
        //        p.AddResource(Gameplay.ResourceType.Gold, 900);
        //        con.Tag = p.id.ToString();
        //        p.name = p.id + "";
        //        players.AddPlayer(p);
        //        playerCounter++;
        //    }

        //    foreach(NetConnection con in connections)
        //    {
        //        NetOutgoingMessage outMsg = GetOutgoingMsg(NetworkCommand.ReceiveGameStart, isServer);
        //        outMsg.Write(Serializer.SerializeObject(players));
        //        outMsg.Write(Serializer.SerializeObject(world));
        //        String tag = (String)con.Tag.ToString();
        //        outMsg.Write(tag);
        //        SendMessage(outMsg, isServer, con);
        //    }
        //}
        //public static void ReceiveGameStart(NetIncomingMessage msg, Boolean isServer)
        //{
        //    try
        //    {
        //        Players p = Serializer.DeserializeObject<Players>(msg.ReadString());
        //        String worldString = msg.ReadString();
        //        World world = Serializer.DeserializeObject<World>(worldString);
        //        Engine.world = world;
        //        Engine.world.players = p;
        //        String str = msg.ReadString();
        //        Engine.world.myPlayerId = int.Parse(str);
        //    }
        //    catch(Exception e)
        //    {

        //    }
        //}

        public static NetOutgoingMessage GetOutgoingMsg(NetworkCommand command, Boolean isServer)
        {
            NetOutgoingMessage outMsg;
            if(Network.IsServer())
            {
                outMsg = Network.getGameServer().CreateOutgoingMessage();
            }
            else
            {
                outMsg = Network.getGameClient().CreateOutgoingMessage();
            }

            outMsg.Write(Enum.GetName(typeof(NetworkCommand), command));
            return outMsg;
        }

        public static void SendMessage(NetOutgoingMessage msg, Boolean isServer, NetConnection connection = null)
        {
            if(Network.IsServer())
            {
                Network.getGameServer().SendMessage(msg, connection);
            }
            else
            {
                Network.getGameClient().SendMessage(msg, connection);
            }
        }
    }
}
