using ApexEngine.Scene.Components;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace ApexEngine.Networking
{
    public class ServerGameComponent : GameComponent
    {
        private ServerHandler handler;
        private NetServer s_server;
        private List<NetConnection> connections = new List<NetConnection>();
        private static int id = 0;

        public ServerGameComponent(ServerHandler handler)
        {
            this.handler = handler;
            this.handler.parent = this;
        }

        private void UpdateConnections()
        {
            connections.Clear();

            foreach (NetConnection conn in s_server.Connections)
            {
                connections.Add(conn);
            }
        }

        public void Send(int id, Message msg)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                int clientId = (int)connections[i].Peer.UniqueIdentifier;

                if (clientId == id)
                {
                    NetOutgoingMessage nmsg = connections[i].Peer.CreateMessage(msg.ToString());
                    s_server.SendMessage(nmsg, connections[i], NetDeliveryMethod.ReliableOrdered);
                    return;
                }
            }
        }

        public void Broadcast(Message msg)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                NetOutgoingMessage nmsg = connections[i].Peer.CreateMessage(msg.ToString());
                s_server.SendMessage(nmsg, connections[i], NetDeliveryMethod.ReliableOrdered);
            }
        }

        public void BroadcastExcept(int id, Message msg)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                int clientId = (int)connections[i].Peer.UniqueIdentifier;

                if (clientId != id)
                {
                    NetOutgoingMessage nmsg = connections[i].Peer.CreateMessage(msg.ToString());
                    s_server.SendMessage(nmsg, connections[i], NetDeliveryMethod.ReliableOrdered);
                }
            }
        }

        public void Connect(int port)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.MaximumConnections = 100;
            config.Port = port;
            s_server = new NetServer(config);
            s_server.Start();
        }

        public override void Init()
        {
        }

        public override void Update()
        {
            NetIncomingMessage im;
            while ((im = s_server.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                   case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                        string reason = im.ReadString();
                       // Console.WriteLine(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                       // if (status == NetConnectionStatus.Connected)
                       //     Console.WriteLine("Remote hail: " + im.SenderConnection.RemoteHailMessage.ReadString());
                      //  else
                      //      handler.Handle(new Message(Message.MessageTypes.QUIT_MESSAGE, im.SenderConnection.RemoteUniqueIdentifier));

                        UpdateConnections();
                        break;
                    case NetIncomingMessageType.Data:
                        string str = im.ReadString();
                        handler.Handle(new Message(str));
                        break;
                    default:
                        break;
                }
                s_server.Recycle(im);
            }
        }
    }
}