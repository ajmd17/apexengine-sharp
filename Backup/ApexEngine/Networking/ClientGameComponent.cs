using ApexEngine.Scene.Components;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ApexEngine.Networking
{
    public class ClientGameComponent : GameComponent
    {
        private NetClient s_client;
        private ClientHandler handler;

        public ClientGameComponent(ClientHandler handler)
        {
            this.handler = handler;
        }

        private void GotMessage(object peer)
        {
            NetIncomingMessage im;
            while ((im = s_client.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                        if (status == NetConnectionStatus.Connected)
                            SendMessage(new Message(Message.MessageTypes.JOIN_GAME));
                            

                        if (status == NetConnectionStatus.Disconnected)
                            SendMessage(new Message(Message.MessageTypes.QUIT_MESSAGE, s_client.UniqueIdentifier));
                        
                        break;
                    default:
                        string chat = im.ReadString();
                        this.handler.Handle(new Message(chat));
                        break;
                }
                s_client.Recycle(im);
            }
        }

        public void Connect(string address, int port)
        {
            try
            {
                s_client.Start();
                NetOutgoingMessage hail = s_client.CreateMessage("Hello World");
                s_client.Connect(address, port, hail);
            }
            catch ( Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Disconnect()
        {
            s_client.Disconnect("User disconnected from server");
        }

        public void SendMessage(Message msg)
        {
            NetOutgoingMessage nmsg = s_client.CreateMessage(msg.ToString());
            s_client.SendMessage(nmsg, NetDeliveryMethod.ReliableOrdered);
            s_client.FlushSendQueue();
        }
    
        public override void Init()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.AutoFlushSendQueue = false;
            s_client = new NetClient(config);

            s_client.RegisterReceivedCallback(new SendOrPostCallback(GotMessage));
        }

        public override void Update()
        {
        }
    }
}
