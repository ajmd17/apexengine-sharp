using ApexEngine.Math;
using System;
using System.Collections.Generic;

namespace ApexEngine.Networking
{
    public class ServerHandler
    {
        public ServerGameComponent parent;
        private Action<Message> handler;

        public ServerHandler(Action<Message> handler)
        {
            this.handler = handler;
        }

        private static PlayerInfo PlayerWithID(List<PlayerInfo> players, String id)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetID().Equals(id))
                {
                    return players[i];
                }
            }
            return null;
        }

        public void Handle(Message msg)
        {
            if (msg.GetType().Equals(Message.MessageTypes.PLAYER_SPAWN))
            {
                String id = msg.get(1);
                String screenName = msg.get(2);
                float x_pos = float.Parse(msg.get(3));
                float y_pos = float.Parse(msg.get(4));
                float z_pos = float.Parse(msg.get(5));
                PlayerInfo pi = new PlayerInfo(id, screenName, new Vector3f(x_pos, y_pos, z_pos));
                for (int i = 0; i < ServerList.players.Count; i++)
                {
                    if (!id.Equals(ServerList.players[i].GetID()))
                    {
                        parent.Send(int.Parse(id), new Message(Message.MessageTypes.PLAYER_SPAWN,
                                ServerList.players[i].GetID(), ServerList.players[i].GetScreenName(),
                                ServerList.players[i].GetPosition().x, ServerList.players[i].GetPosition().y,
                                ServerList.players[i].GetPosition().z));
                    }
                }
                ServerList.players.Add(pi);
                parent.BroadcastExcept(int.Parse(pi.GetID()), msg); // broadcast
                                                                           // that
                                                                           // the
                                                                           // player
                                                                           // has
                                                                           // spawned
            }
            else if (msg.GetType().Equals(Message.MessageTypes.PLAYER_MOVE))
            {
                String id = msg.get(1);
                float x_pos = float.Parse(msg.get(3));
                float y_pos = float.Parse(msg.get(4));
                float z_pos = float.Parse(msg.get(5));
                PlayerInfo p = PlayerWithID(ServerList.players, id);
                if (p != null)
                {
                    p.GetPosition().Set(new Vector3f(x_pos, y_pos, z_pos));
                }
                parent.Broadcast(msg);
            }
            else if (msg.getType().Equals(Message.MessageTypes.QUIT_MESSAGE))
            {
                String id = msg.get(1);
                PlayerInfo p = PlayerWithID(ServerList.players, id);
                if (p != null)
                {
                    ServerList.players.Remove(p);
                }
                parent.Broadcast(msg);
            }
            else
            { // else, just broadcast the message
                parent.Broadcast(msg);
            }
        }
    }
}