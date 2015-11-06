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
            
            using (var game = new GameWindow(1080, 720))
            {
                Node rootNode = new Node();
                Mesh m = new Mesh();
                Shader s = null;
                Rendering.Cameras.DefaultCamera mycam = new Rendering.Cameras.DefaultCamera(45f);
                game.Load += (sender, e) =>
                {
                    rootNode.SetName("root");
                    s = new Shader("attribute vec3 a_position;\nattribute vec3 a_normal;\nvarying vec3 v_normal;\nuniform mat4 u_world;\nuniform mat4 u_view;\nuniform mat4 u_proj;\nvoid main() {\nv_normal = a_normal;\ngl_Position = u_proj * u_view*  u_world * vec4(a_position, 1.0);\n}",
                        "varying vec3 v_normal;\nvoid main() {\n gl_FragColor = vec4(v_normal, 1.0);\n}");
                    
                    

                    Node loadedObj = (Node)(new Assets.ModelLoaders.ObjModelLoader().Load("C:\\Users\\User\\Desktop\\cube.obj"));
                    ((Geometry)(loadedObj.GetChild(0))).SetShader(s);
                //    Console.WriteLine(((Geometry)(loadedObj.GetChild(0))).GetMesh().vertices[0].GetNormal());
                    rootNode.AddChild(loadedObj);
                    ApexEngine.Assets.ApxExporter.ApxExporter.ExportModel(loadedObj, "C:\\Users\\User\\Desktop\\cube.apx");
                  /*  List<Vertex> vertices = new List<Vertex>();
                    vertices.Add(new Vertex(new Vector3f(-1.0f, -1.0f, 0.0f)));
                    vertices.Add(new Vertex(new Vector3f(1.0f, -1.0f, 0.0f)));
                    vertices.Add(new Vertex(new Vector3f(1.0f, 1.0f, 0.0f)));
                    for (int x = -3; x < 3; x++)
                    {
                        for (int z = -3; z < 3; z++)
                        {

                            Geometry g = new Geometry();
                            m.SetVertices(vertices);
                            g.SetMesh(m);
                            g.SetShader(s);
                            rootNode.AddChild(g);
                            g.SetLocalTranslation(new Vector3f((float)System.Math.Sin(x)*5, 0.0f, (float)System.Math.Sin(z)*5+8));
                        }
                    }*/
                    game.VSync = VSyncMode.Off;


                };

                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                    RenderManager.WINDOW_X = game.X;
                    RenderManager.WINDOW_Y = game.Y;
                    RenderManager.SCREEN_HEIGHT = game.Height;
                    RenderManager.SCREEN_WIDTH = game.Width;
                };
                float rot = 5f;
                game.UpdateFrame += (sender, e) =>
                {
                    rot += 0.01f;
                   // mycam.translation.z += 0.01f;
                  //  mycam.Rotate(new Vector3f(0, 1, 0), rot);
                    // add game logic, input handling
                 //   mycam.Rotate(new Vector3f(1, 0, 0), (float)System.Math.Sin(rot)*0.1f);
                    mycam.Update();
                    rootNode.Update();
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                };
                game.KeyDown += (sender, e) =>
                {
                      Input.Input.KeyDown(e.Key);
                };
                game.KeyUp += (sender, e) =>
                {
                    Input.Input.KeyUp(e.Key);
                };
                game.MouseDown += (sender, e) =>
                {
                    Input.Input.MouseButtonDown(e.Button);
                };
                game.MouseUp += (sender, e) =>
                {
                    Input.Input.MouseButtonUp(e.Button);
                };
                game.RenderFrame += (sender, e) =>
                {
                    GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    RenderManager.Render(mycam);
                  
                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }
    }
}
