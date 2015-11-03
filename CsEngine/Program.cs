using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CsEngine.Math;
using CsEngine.Rendering;
namespace CsEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (var game = new GameWindow())
            {
                Mesh m = new Mesh();
                Shader s = null;
                game.Load += (sender, e) =>
                {
                    s = new Shader("attribute vec3 a_position;\nvoid main() {\ngl_Position = vec4(a_position, 1.0);\n}",
                        "void main() {\n gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);\n}");

                    // setup settings, load textures, sounds
                    List<Vertex> vertices = new List<Vertex>();
                    vertices.Add(new Vertex(new Vector3f(-1.0f, -1.0f, 0.0f), new Vector2f(0.0f, 0.0f)));
                    vertices.Add(new Vertex(new Vector3f(1.0f, -1.0f, 0.0f)));
                    vertices.Add(new Vertex(new Vector3f(1.0f, 1.0f, 0.0f)));
                    m.SetVertices(vertices);
                    game.VSync = VSyncMode.Off;
                };

                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };

                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                };

                game.RenderFrame += (sender, e) =>
                {
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    // render graphics
                    /*GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

                    GL.Begin(PrimitiveType.Triangles);

                    GL.Color3(Color.MidnightBlue);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.Color3(Color.SpringGreen);
                    GL.Vertex2(0.0f, -1.0f);
                    GL.Color3(Color.Ivory);
                    GL.Vertex2(1.0f, 1.0f);

                    GL.End();*/
                    s.Render(m);

                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }
    }
}
