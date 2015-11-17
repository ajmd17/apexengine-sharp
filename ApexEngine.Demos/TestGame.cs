using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Assets;
namespace ApexEngine.Demos
{
    public class TestGame : Game
    {
        public override void Init()
        {
            Rendering.Environment.Environment.AmbientLight.Color.Set(0.4f, 0.1f, 0.1f, 1.0f);
            Rendering.Environment.Environment.DirectionalLight.Direction.Set(-1f, 1f, 0f);

            // Test an Apex Engine 3D model, with a material created in the material editor
            GameObject loadedApx = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\test_apx.apx");
            loadedApx.SetLocalTranslation(new Math.Vector3f(2.5f, 0, 0));
            rootNode.AddChild(loadedApx);

            // Test an OBJ model, with normal mapping
            GameObject loadedObj = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\sphere16.obj");
            rootNode.AddChild(loadedObj);


            //  Rendering.Animation.AnimationController anim = (Rendering.Animation.AnimationController)loadedApx.GetController(typeof(Rendering.Animation.AnimationController));
            //  anim.PlayAnimation(1);

            //  AddComponent(new ApexEngine.Terrain.SimplexTerrain.SimplexTerrainComponent());
        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
