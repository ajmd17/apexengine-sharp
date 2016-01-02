using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Networking
{
    public class ClientHandler
    {
        public Action<Message> handle;

        public ClientHandler(Action<Message> handle)
        {
            this.handle = handle;
        }

        public void Handle(Message msg)
        {
            this.handle(msg);
        }
    }
}
