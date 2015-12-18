using ApexEngine.Rendering.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.ClientTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            new TestClient(new GLRenderer()).Run();
        }
    }
}
