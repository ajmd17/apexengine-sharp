using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Networking
{
    public class Message
    {
        public static class MessageTypes
        {
            public const string PLAYER_SPAWN = "plyr_spawn";
		    public const string PLAYER_MOVE = "plyr_mov";
		    public const string PLAYER_ROTATE = "plyr_rot";
		    public const string ENTITY_SPAWN = "ai_spawn";
		    public const string FETCH_ID = "idfetch";
		    public const string QUIT_MESSAGE = "quit";
		    public const string TEXT = "text";
		    public const string JOIN_GAME = "join";
        }

        string[] values = new string[14];
        string header = "";

        public Message()
        {

        }

        public Message(params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] != null)
                {
                    values[i] = args[i].ToString();
                }
                else
                {
                    values[i] = "";
                }
            }
            header = values[0];
        }

        public Message(String text)
        {
            values = text.Split('~');
            if (values.Length > 1)
            {
                header = values[0];
            }
            else
            {
                header = MessageTypes.TEXT;
            }
        }

        public String get(int index)
        {
            return values[index];
        }

        public String getType()
        {
            return header;
        }
        
        public override string ToString()
        {
            string finalString = "";
            foreach (string value in values)
            {
                if (value != null)
                {
                    finalString += value + "~";
                }
            }
            return finalString;
        }
    }
}
