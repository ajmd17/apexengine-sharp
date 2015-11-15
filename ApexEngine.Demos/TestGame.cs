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
            Rendering.Environment.Environment.AmbientLight.Color.Set(0.3f, 0.1f, 0.1f, 1.0f);
            Rendering.Environment.Environment.DirectionalLight.Direction.Set(-1f, 1f, 0f);


            GameObject loadedApx = AssetManager.LoadModel("C:\\Users\\User\\Desktop\\test2.apx");
              rootNode.AddChild(loadedApx);
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
