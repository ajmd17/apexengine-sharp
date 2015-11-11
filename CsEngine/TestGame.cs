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
            // Test load OBJ
            Node loadedObj = (Node)(AssetManager.Load("C:\\Users\\User\\Desktop\\monkey.obj"));
            loadedObj.SetLocalTranslation(new Math.Vector3f(6, 7, 6));
            rootNode.AddChild(loadedObj);

            // Test load Ogre XML
            Node loadedOgreXml = (Node)(AssetManager.Load(AssetManager.GetAppPath() + "\\assets\\models\\halo\\halo.mesh.xml"));
            loadedOgreXml.SetLocalTranslation(new Math.Vector3f(-6, 0, -6));
            rootNode.AddChild(loadedOgreXml);

            // Test load Apx
            GameObject loadedApx = AssetManager.LoadModel("C:\\Users\\User\\Documents\\Apex3D\\test_guy_2.apx");
            rootNode.AddChild(loadedApx);
            Rendering.Animation.AnimationController anim = (Rendering.Animation.AnimationController)loadedApx.GetController(typeof(Rendering.Animation.AnimationController));
            anim.PlayAnimation(1);

            ShaderProperties prop1 = new ShaderProperties().SetProperty("Hi", true);
            ShaderProperties prop2 = new ShaderProperties().SetProperty("Hi", true);
            Console.WriteLine(Rendering.Util.ShaderUtil.CompareShader(prop1, prop2));

        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
