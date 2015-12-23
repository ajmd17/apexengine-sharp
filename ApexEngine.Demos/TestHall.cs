using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Plugins.Shaders.Post;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Demos
{
    public class TestHall : Game
    {
        public TestHall (Renderer renderer) : base(renderer)
        {

        }

        public override void Init()
        {
            /*  Rendering.Light.PointLight pl = new Rendering.Light.PointLight();
              pl.Color = new Color4f(0.2f, 0.0f, 0.0f, 1.0f);
              pl.Position = new Vector3f(0.0f, 0.0f, 0.0f);
              Environment.PointLights.Add(pl);*/

            Environment.FogColor.Set(0.3f, 0.4f, 0.1f, 0.2f);
            Environment.FogEnd = 360;
            Environment.FogStart = 35;

            Environment.DirectionalLight.Color.Set(0.0f, 0.0f, 0.0f, 1.0f);
            Environment.AmbientLight.Color.Set(1.0f, 1.0f, 1.0f, 1.0f);

            GameObject hall;
            rootNode.AddChild(hall = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\hall\\hall.obj"));
            hall.SetLocalScale(new Math.Vector3f(0.75f, 0.75f, 0.5f));
            PhysicsWorld.AddObject(hall, 0.0f);




            Rendering.NormalMapRenderer nmr;
            RenderManager.AddComponent(nmr = new Rendering.NormalMapRenderer(Environment, Camera));
            RenderManager.PostProcessor.PostFilters.Add(new Rendering.PostProcess.Filters.SSAOFilter(nmr));

            RenderManager.PostProcessor.PostFilters.Add(new FXAAFilter());
        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
