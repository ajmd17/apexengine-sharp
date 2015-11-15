using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Assets;
namespace ApexEngine
{
    public class TestGame : Game
    {
        public override void Init()
        {
            Rendering.Environment.Environment.AmbientLight.Color.Set(0.3f, 0.1f, 0.1f, 1.0f);

            // Test load OBJ
            Node loadedObj2 = (Node)(AssetManager.Load("C:\\Users\\User\\Desktop\\cube.obj"));
          //  rootNode.AddChild(loadedObj2);
            Node loadedObj = (Node)(AssetManager.Load("C:\\Users\\User\\Desktop\\monkey.obj"));
            loadedObj.SetLocalTranslation(new Math.Vector3f(6, 7, 6));
          //  rootNode.AddChild(loadedObj);


            

            // Test load Ogre XML
            Node loadedOgreXml = (Node)(AssetManager.Load(AssetManager.GetAppPath() + "\\assets\\models\\halo\\halo.mesh.xml"));
            loadedOgreXml.SetLocalTranslation(new Math.Vector3f(-6, 0, -6));
            rootNode.AddChild(loadedOgreXml);
            Rendering.Animation.AnimationController anim = (Rendering.Animation.AnimationController)loadedOgreXml.GetController(typeof(Rendering.Animation.AnimationController));
            anim.PlayAnimation(1);
            // Test load Apx
              GameObject loadedApx = AssetManager.LoadModel("C:\\Users\\User\\Desktop\\scene.apx");
              rootNode.AddChild(loadedApx);
            //  Rendering.Animation.AnimationController anim = (Rendering.Animation.AnimationController)loadedApx.GetController(typeof(Rendering.Animation.AnimationController));
            //  anim.PlayAnimation(1);


        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
