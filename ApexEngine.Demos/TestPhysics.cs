using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Rendering.OpenGL;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering.Shadows;

namespace ApexEngine.Demos
{
    public class TestPhysics : Game
    {
        public TestPhysics(GLRenderer renderer) : base(renderer)
        {

        }

        public override void Init()
        {
            ShadowMappingComponent smc;
            RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment, new int[] { 2048, 1024 }));
            smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;



          //  Node floor = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\scene.apx");
          //  rootNode.AddChild(floor);
          //  PhysicsWorld.AddObject(floor, 0);
            Geometry cube = new Geometry(MeshFactory.CreateCube(new Vector3f(-15, -0.5f, -15f), new Vector3f(15f, 0.5f, 15f)));
            rootNode.AddChild(cube);
            PhysicsWorld.AddObject(cube, 0);

            Node n = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\monkeyhq.obj");
            n.SetLocalTranslation(new Vector3f(0, 50, 0));
            rootNode.AddChild(n);
            //PhysicsWorld.AddObject(n, 5, Scene.Physics.PhysicsWorld.PhysicsShape.Box);
            PhysicsWorld.AddCharacter((DefaultCamera)cam, InputManager, n, 1);

           
        }

        public override void Render()
        {
            this.PhysicsWorld.DrawDebug();
        }

        public override void Update()
        {
        }
    }
}
