using ApexEngine.Assets;
using ApexEngine.Audio;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Shadows;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;
using ApexEngine.Scene.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Demos
{
    public class TestSounds : Game
    {
        public TestSounds(Renderer renderer) : base(renderer)
        {
        }

        public override void Init()
        {
           // ShadowMappingComponent smc;
           // RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment));
           // smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;

            

            Geometry floor = new Geometry(MeshFactory.CreateCube(new Math.Vector3f(-5f, -0.5f, -5f), new Math.Vector3f(5, 0.5f, 5f)));
            rootNode.AddChild(floor);
            PhysicsWorld.AddObject(floor, 0.0f);



            Node test = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\sphere16.obj");
            test.SetLocalTranslation(new Math.Vector3f(0, 25, 0));
            rootNode.AddChild(test);

            PhysicsWorld.AddObject(test, 5.0f);


            Sound cat = (Sound)AssetManager.Load(AssetManager.GetAppPath() + "\\sounds\\cat.wav");
            AudioNode an = new AudioNode(cat);
            test.AddChild(an);

            InputManager.AddKeyboardEvent(new Input.KeyboardEvent(Input.InputManager.KeyboardKey.Space, () => { an.Play(); }));

        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
