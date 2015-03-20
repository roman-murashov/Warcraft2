using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Warcraft2.Application.Networking
{
    public class GameClient
    {
        NetPeerConfiguration netConfig = new NetPeerConfiguration("Warcraft2");
        NetClient client;
        public GameClient()
        {
            netConfig.Port = 0;
            client = new NetClient(netConfig);
            client.Start();
        }

        public void Connect(String ip, int port)
        {
            try
            {
                NetConnection connection = client.Connect(ip, port);
            }
            catch(Exception e)
            {
                RailorLibrary.Data.Cout.W(e.Message);
            }

        }


        public void Disconnect()
        {
            try
            {
                client.Shutdown("Shutting Down");
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
                while((msg = client.ReadMessage()) != null)
                {
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

                        case NetIncomingMessageType.DiscoveryResponse:
                            RailorLibrary.Data.Cout.W("Client received discovery request");
                            ReceivedDiscoveryResponse(msg);
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                            if(status == NetConnectionStatus.Connected)
                            {
                                ServerConnected(msg);
                            }
                            else if(status == NetConnectionStatus.Disconnected)
                            {
                                ServerDisconnected(msg);
                            }
                            break;

                        default:
                            Console.WriteLine("Unhandled type: " + msg.MessageType);
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                RailorLibrary.Data.Cout.W(e.ToString());
            }
        }

        protected void DataReceived(NetIncomingMessage msg)
        {
            NetworkCommands.Invoke(msg, false);
        }

        protected void ServerConnected(NetIncomingMessage msg)
        {

        }

        protected void ServerDisconnected(NetIncomingMessage msg)
        {
            Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " disconnected!");
        }


        protected void ReceivedDiscoveryResponse(NetIncomingMessage msg)
        {
            client.Connect(msg.SenderEndPoint);
        }

        public void SendMessage(String message)
        {
            NetOutgoingMessage msg = client.CreateMessage(message);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendDiscoveryRequest()
        {
            client.DiscoverLocalPeers(Network.port);
        }

        public NetClient GetNetClient()
        {
            return client;
        }

        public void SendMessage(NetOutgoingMessage msg, NetConnection connection)
        {
            RailorLibrary.Data.Cout.W("Client sent message");
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }


        public NetOutgoingMessage CreateOutgoingMessage()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            return msg;
        }
    }
}
