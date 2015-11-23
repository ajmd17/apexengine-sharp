using ApexEngine.Assets;
using ApexEngine.Input;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Scene;
using ApexEngine.Scene.Components;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace ApexEngine
{
    public abstract class Game
    {
        private InputManager inputManager = new InputManager();
        protected Node rootNode = new Node("root");
        protected Camera cam;
        protected string windowTitle = "Apex3D Game";
        private RenderManager renderManager;
        protected List<GameComponent> components = new List<GameComponent>();
        private Environment environment = new Environment();

        public Game()
        {
            cam = new DefaultCamera(inputManager, 55);
            renderManager = new RenderManager(new ApexEngine.Rendering.OpenGL.GLRenderer(), cam);
        }

        public List<GameComponent> GameComponents
        {
            get { return components; }
        }

        public Environment Environment
        {
            get { return environment; }
            set { environment = value; }
        }

        public void AddComponent(GameComponent cmp)
        {
            components.Add(cmp);
            cmp.cam = cam;
            rootNode.AddChild(cmp.rootNode);
            cmp.Init();
        }

        public void RemoveComponent(GameComponent cmp)
        {
            components.Remove(cmp);
            rootNode.RemoveChild(cmp.rootNode);
        }

        public InputManager InputManager
        {
            get { return inputManager; }
        }

        public RenderManager RenderManager
        {
            get { return renderManager; }
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
            using (var game = new GameWindow(1080, 720, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24)))
            {
                game.Title = windowTitle;
                game.Load += (sender, e) => InitInternal();
                game.UpdateFrame += (sender, e) =>
                {
                    inputManager.WINDOW_X = game.X;
                    inputManager.WINDOW_Y = game.Y;
                    foreach (GameComponent cmp in components)
                        cmp.Update();
                    UpdateInternal();
                };
                game.RenderFrame += (sender, e) =>
                {
                    RenderInternal();
                    game.SwapBuffers();
                };
                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                    inputManager.SCREEN_HEIGHT = game.Height;
                    inputManager.SCREEN_WIDTH = game.Width;
                    cam.Width = game.Width;
                    cam.Height = game.Height;
                };
                game.KeyDown += (sender, e) =>
                {
                    inputManager.KeyDown(e.Key);
                };
                game.KeyUp += (sender, e) =>
                {
                    inputManager.KeyUp(e.Key);
                };
                game.MouseDown += (sender, e) =>
                {
                    inputManager.MouseButtonDown(e.Button);
                };
                game.MouseUp += (sender, e) =>
                {
                    inputManager.MouseButtonUp(e.Button);
                };
                game.VSync = VSyncMode.Off;
                game.Run(60);
            }
        }

        public void InitInternal()
        {
            renderManager.Init();
            AssetManager.InitDefaultLoaders();
            Init();
        }

        public void UpdateInternal()
        {
            RenderManager.ElapsedTime += 0.01f;
            cam.Update();
            rootNode.Update(renderManager);
            Update();
        }

        public void RenderInternal()
        {
            renderManager.Render(Environment, cam);
            Render();
        }

        public abstract void Init();

        public abstract void Update();

        public abstract void Render();
    }
}