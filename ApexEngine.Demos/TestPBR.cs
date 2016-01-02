using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering.OpenGL;
using ApexEngine.Assets;
using ApexEngine.Scene;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Rendering.Shadows;
using ApexEngine.Math;

namespace ApexEngine.Demos
{
    public class TestPBR : Game
    {
        public TestPBR(GLRenderer renderer)
            : base(renderer)
        {

        }

        public override void Init()
        {
            Environment.AmbientLight.Color.Set(0.1f, 0.1f, 0.1f, 1.0f);
            Environment.FogColor.Set(0.2f, 0.3f, 0.45f, 1.0f);
            Environment.DirectionalLight.Direction.Set(1f, 1, 1f).NormalizeStore();
            Environment.DirectionalLight.Color.Set(1.0f, 0.8f, 0.7f, 1.0f);
            ((PerspectiveCamera)Camera).FieldOfView = 75;
            Camera.Far = 330;

            ShadowMappingComponent smc;
            RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment, new int[] {2048,1024}));
            smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;



            GameObject grid = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\grid\\grid.obj");
            rootNode.AddChild(grid);




            Node loadedApx = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\mitsuba.obj");
            loadedApx.SetLocalTranslation(new Math.Vector3f(0f, 0, 0));

            Cubemap skybox = Cubemap.LoadCubemap(new string[] { AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_right.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_left.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_top.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_top.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_front.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_back.jpg" });

            Texture nrm = AssetManager.LoadTexture(AssetManager.GetAppPath() + "\\textures\\normal_metal.jpg");

            Random rand = new Random();
            for (int x = 0; x < 5; x++)
            {
                for (int z = 0; z < 5; z++)
                {
                    Node thing = (Node)loadedApx.Clone();
                    thing.SetLocalTranslation(new Vector3f((x * 3) - 6, 0, (z * 3) - 6));
                    thing.GetChildGeom(0).Material.SetValue(Material.TEXTURE_ENV, skybox);
                    thing.GetChildGeom(0).Material.SetValue(Material.SHININESS, 1.0f);
                    thing.GetChildGeom(0).Material.SetValue(Material.ROUGHNESS, 0.5f);
                    thing.GetChildGeom(0).UpdateShaderProperties();

                    thing.GetChildGeom(1).Material.SetValue(Material.COLOR_DIFFUSE, new Color4f(1.0f, 0.0f, 0.0f, 1.0f));
                    thing.GetChildGeom(1).Material.SetValue(Material.TEXTURE_ENV, skybox);
                    thing.GetChildGeom(1).Material.SetValue(Material.SHININESS, (x+1) / 5.0f);
                    thing.GetChildGeom(1).Material.SetValue(Material.ROUGHNESS, (z+1) / 5.0f);
                    thing.GetChildGeom(1).UpdateShaderProperties();
               //     thing.GetChildGeom(1).Material.SetValue(Material.TEXTURE_NORMAL, nrm);

                    rootNode.AddChild(thing);
                }
            }
        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
