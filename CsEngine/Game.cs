using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Scene;
using ApexEngine.Assets;
namespace ApexEngine
{
    public abstract class Game
    {
        protected Node rootNode = new Node("root");
        protected Camera cam = new DefaultCamera(45f);
        protected string windowTitle = "Apex3D Game";
        public Game()
        {
            
        }
        public Node RootNode
        {
            get { return rootNode; }
            set { rootNode = value; }
        }
        public Camera Camera
        {
            get { return cam; }
            set { cam = value; }
        }
        public void Run()
        {
            using (var game = new GameWindow(1080, 720))
            {
                game.Title = windowTitle;
                game.Load += (sender, e) => InitInternal();
                game.UpdateFrame += (sender, e) => UpdateInternal();
                game.RenderFrame += (sender, e) =>
                {
                    RenderInternal();
                    game.SwapBuffers();
                };
                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                    RenderManager.WINDOW_X = game.X;
                    RenderManager.WINDOW_Y = game.Y;
                    RenderManager.SCREEN_HEIGHT = game.Height;
                    RenderManager.SCREEN_WIDTH = game.Width;
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
                game.VSync = VSyncMode.Off;
                game.Run(60);
            }
        }
        public void InitInternal()
        {
            AssetManager.InitDefaultLoaders();
            Init();
        }
        public void UpdateInternal()
        {
            RenderManager.ElapsedTime += 0.01f;
            cam.Update();
            rootNode.Update();
            Update();
        }
        public void RenderInternal()
        {
            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderManager.Render(cam);
            Render();
        }
        public abstract void Init();
        public abstract void Update();
        public abstract void Render();
    }
}
