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

namespace ApexEngine.Demos
{
    public class TestPhysics : Game
    {
        public TestPhysics(GLRenderer renderer) : base(renderer)
        {

        }

        public override void Init()
        {


            Node floor = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\scene.apx");
            rootNode.AddChild(floor);
            PhysicsWorld.AddObject(floor, 0);


            Node n = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\sphere16.obj");
            n.SetLocalTranslation(new Vector3f(0, 50, 0));
            rootNode.AddChild(n);
            //PhysicsWorld.AddObject(n, 1);
            PhysicsWorld.AddCharacter((DefaultCamera)cam, InputManager, n, 1);


        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
