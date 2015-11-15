using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Input;
using ApexEngine.Assets;
namespace ApexEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestGame().Run();
        }
    }
}
