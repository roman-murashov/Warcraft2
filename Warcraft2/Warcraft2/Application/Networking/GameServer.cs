using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Warcraft2.Application.Networking
{
    public class GameServer
    {
        NetPeerConfiguration netConfig = new NetPeerConfiguration("Warcraft2");
        NetServer server;
        Boolean running = false;
        public GameServer(int port)
        {
            netConfig.Port = port;
        }

        public Boolean Start()
        {
            Boolean working = false;
            if(running == false)
            {
                try
                {
                    server = new NetServer(netConfig);
                    server.Start();
                    working = true;
                }
                catch(Exception e)
                {
                    working = false;
                    RailorLibrary.Data.Cout.W(e.Message);
                }
            }
            else
            {
                working = true;
            }

            return working;
        }


        public void Stop()
        {
            try
            {
                server.Shutdown("Shutting Down");
            }
            catch(Exception e)
            {
                RailorLibrary.Data.Cout.W(e.Message);
            }

        }

        public void ReadMessages()
        {
            try
            {
                NetIncomingMessage msg;
                while((msg = server.ReadMessage()) != null)
                {
                    //RailorLibrary.Data.Cout.W(msg.ReadString());
                    switch(msg.MessageType)
                    {

                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(msg.ReadString());
                            break;

                        case NetIncomingMessageType.Data:
                            DataReceived(msg);
                            break;

                        case NetIncomingMessageType.DiscoveryRequest:
                            RailorLibrary.Data.Cout.W("Server received discovery request");
                            server.SendDiscoveryResponse(null, msg.SenderEndPoint);
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                            if(status == NetConnectionStatus.Connected)
                            {
                                PlayerConnected(msg);
                            }
                            else if(status == NetConnectionStatus.Disconnected)
                            {
                                PlayerDisconnected(msg);
                            }
                            break;

                        default:
                            Console.WriteLine("Unhandled type: " + msg.MessageType);
                            break;
                    }

                    server.Recycle(msg);
                }
            }
            catch(Exception e)
            {
                RailorLibrary.Data.Cout.W(e.ToString());
            }
        }

        protected void DataReceived(NetIncomingMessage msg)
        {
            NetworkCommands.Invoke(msg, true);
        }

        protected void PlayerConnected(NetIncomingMessage msg)
        {
            //NetworkCommands.SendWorld(Engine.world, true, msg.SenderConnection);
        }

        protected void PlayerDisconnected(NetIncomingMessage msg)
        {
            Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " disconnected!");
        }

        public NetServer GetNetServer()
        {
            return server;
        }

        public Boolean IsRunning()
        {
            if(server != null)
            {
                return server.Status == NetPeerStatus.Running;
            }
            return false;
        }

        public void SendMessage(NetOutgoingMessage msg, NetConnection connection = null)
        {
            if(connection != null)
            {
                server.SendMessage(msg, connection, NetDeliveryMethod.ReliableOrdered);
            }
            else
            {
                server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public NetOutgoingMessage CreateOutgoingMessage()
        {
            NetOutgoingMessage msg = server.CreateMessage();
            return msg;
        }
    }
}
