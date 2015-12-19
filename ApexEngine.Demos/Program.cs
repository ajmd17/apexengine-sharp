using ApexEngine.Rendering.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexEngine.Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            new ApexEngine.Demos.TestHall(new GLRenderer()).Run();
        }
    }
}
