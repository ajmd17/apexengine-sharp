using ApexEngine.Networking;
using ApexEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.ClientTest
{
    public class TestClient : Game
    {
        ClientGameComponent client;
        public TestClient(Renderer renderer) : base(renderer)
        {
        }

        public override void Init()
        {
            this.AddComponent(client = new ClientGameComponent(new ClientHandler((Message msg) => { })));
            client.Connect("localhost", 2222);
        }

        public override void Render()
        {
        }

        public override void Update()
        {


        }
    }
}
