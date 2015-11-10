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
                game.Title = "Apex3D";
                Node rootNode = new Node();
                Mesh m = new Mesh();
                Rendering.Cameras.DefaultCamera mycam = new Rendering.Cameras.DefaultCamera(45f);
                game.Load += (sender, e) =>
                {
                    AssetManager.InitDefaultLoaders();
                    rootNode.SetName("root");

                    //TextLoader txt= new TextLoader();
                    // Rendering.Animation.AnimatedShader animShader = new Rendering.Animation.AnimatedShader();
                    Rendering.Animation.AnimatedShader animShader = (Rendering.Animation.AnimatedShader)ShaderManager.GetShader(typeof(Rendering.Animation.AnimatedShader));

                    Node loadedObj = (Node)(AssetManager.Load("C:\\Users\\User\\Desktop\\monkey.obj"));

                   
                    rootNode.AddChild(loadedObj);

                    Node n = (Node)(AssetManager.Load("C:\\Users\\User\\Documents\\Game Engine\\TestLib\\res\\models\\game\\player1\\player1-shoes01.mesh.xml"));
                    for (int i = 0; i < n.GetChildren().Count; i++)
                    {
                        if (n.GetChild(i) is Geometry)
                        {
                            Geometry g = (Geometry)n.GetChild(i);
                            g.SetShader(animShader);
                        }
                    }
                    Geometry gg = (Geometry)n.GetChild(0);
                   // gg.GetMesh().GetSkeleton().GetBone("pelvis").SetLocalRotation(new Math.Quaternion().SetFromAxis(Vector3f.UNIT_XYZ, 40));
                     Console.WriteLine(gg.GetMesh().GetSkeleton().GetBone(0).GetBindRotation());
                 //   rootNode.AddChild(n);

                     Rendering.Animation.AnimationController anim = ((Rendering.Animation.AnimationController)n.GetController(typeof(Rendering.Animation.AnimationController)));

                    


                  GameObject apx = AssetManager.LoadModel("C:\\Users\\User\\Documents\\Apex3D\\test_guy.apx");
                   rootNode.AddChild(apx);
                    apx.SetLocalTranslation(new Vector3f(0, 5, 5));
                     Assets.Apx.ApxExporter.ExportModel(apx, "C:\\Users\\User\\Documents\\Apex3D\\test_guy_2.apx");
                    //   gg.GetMesh().GetSkeleton().AddAnimation(((Geometry)apx.GetChild(0)).GetMesh().GetSkeleton().GetAnimation(0));
                    // anim.PlayAnimation(0, 2.0f);
                    Rendering.Animation.AnimationController anim2 = ((Rendering.Animation.AnimationController)apx.GetController(typeof(Rendering.Animation.AnimationController)));
                    anim2.PlayAnimation(1, 1.0f);
                     //((Geometry)apx.GetChild(0)).GetMesh().GetSkeleton().GetBone("pelvis").SetLocalRotation(new Math.Quaternion().SetFromAxis(Vector3f.UNIT_X, 40));
                    //   Console.WriteLine(anim.GetCurrentAnimation().GetName());
                    // ApexEngine.Assets.Apx.ApxExporter.ExportModel(loadedObj, "C:\\Users\\User\\Desktop\\cube.apx");
                    // Node n2 = (Node)(new ApexEngine.Assets.Apx.ApxModelLoader().Load("C:\\Users\\User\\Desktop\\cube.apx"));
                    // ((Geometry)(n2.GetChild(0))).SetShader(s);
                    // rootNode.AddChild(n2);

                    // Console.WriteLine((new Vector3f(0f).LerpStore(new Vector3f(5f), 0.5f)));
                    // Console.WriteLine(n2.GetChild(0).GetName());
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
