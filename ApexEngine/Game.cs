using System;
using System.Collections.Generic;
using ApexEngine.Assets;
using ApexEngine.Input;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Scene;
using ApexEngine.Scene.Components;
using ApexEngine.Scene.Physics;

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
        private Rendering.Environment environment = new Rendering.Environment();
        private PhysicsWorld physicsWorld;
        Vector3f tmpVec = new Vector3f();

        public Game(Renderer renderer)
        {
            cam = new DefaultCamera(inputManager, 55);
            renderManager = new RenderManager(renderer, cam, new Action(() => { Render(); }));
            renderManager.SpriteRenderer = new SpriteRenderer(this);
            physicsWorld = new PhysicsWorld(new PhysicsDebugDraw(cam));
        }

        public string Title
        {
            get { return windowTitle; }
            set { windowTitle = value; }
        }

        public List<GameComponent> GameComponents
        {
            get { return components; }
        }

        public PhysicsWorld PhysicsWorld
        {
            get { return physicsWorld; }
        }

        public Rendering.Environment Environment
        {
            get { return environment; }
            set { environment = value; }
        }

        public void AddComponent(GameComponent cmp)
        {
            components.Add(cmp);

            cmp.Camera = this.cam;
            cmp.Environment = this.environment;

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
            RenderManager.Renderer.CreateContext(this, 1080, 720);
            physicsWorld.Dispose();
        }

        public void InitInternal()
        {
            renderManager.Init();
            AssetManager.InitDefaultLoaders();
            Init();
        }

        public void UpdateInternal()
        {
            MoveToOrigin();
            foreach (GameComponent cmp in components)
                cmp.Update();
            environment.ElapsedTime += environment.TimePerFrame;
            cam.Update();
            RenderManager.Renderer.SetAudioListenerValues(cam);
            physicsWorld.Update(Environment.TimePerFrame);
            rootNode.Update(renderManager);
            Update();
        }

        public void RenderInternal()
        {
            renderManager.Render(Environment, cam);
            Render();
        }

        private void MoveToOrigin()
        {
            float maxDist = 25;
            Vector3f position = cam.Translation;
            if (position.x > maxDist)
            {
                rootNode.GetLocalTranslation().AddStore(tmpVec.Set(-maxDist, 0, 0));
                rootNode.SetUpdateNeeded();
                cam.Translation.Set(0, position.y, position.z);
                cam.UpdateCamera();
                cam.UpdateMatrix();
                rootNode.Update(renderManager);
                Console.WriteLine("Move");
            }
            if (position.z > maxDist)
            {
                rootNode.GetLocalTranslation().AddStore(tmpVec.Set(0, 0, -maxDist));
                rootNode.SetUpdateNeeded();
                cam.Translation.Set(position.x, position.y, 0);
                cam.UpdateCamera();
                cam.UpdateMatrix();
                rootNode.Update(renderManager);
                Console.WriteLine("Move");
            }
            if (position.x < -maxDist)
            {
                rootNode.GetLocalTranslation().AddStore(tmpVec.Set(maxDist, 0, 0));
                rootNode.SetUpdateNeeded();
                cam.Translation.Set(0, position.y, position.z);
                cam.UpdateCamera();
                cam.UpdateMatrix();
                rootNode.Update(renderManager);
                Console.WriteLine("Move");
            }
            if (position.z < -maxDist)
            {
                rootNode.GetLocalTranslation().AddStore(tmpVec.Set(0, 0, maxDist));
                rootNode.SetUpdateNeeded();
                cam.Translation.Set(position.x, position.y, 0);
                cam.UpdateCamera();
                cam.UpdateMatrix();
                rootNode.Update(renderManager);
                Console.WriteLine("Move");
            }
        }

        public abstract void Init();

        public abstract void Update();

        public abstract void Render();
    }
}