using ApexEngine.Assets;
using ApexEngine.Audio;
using ApexEngine.Plugins.PagingSystem;
using ApexEngine.Plugins.Shaders.Post;
using ApexEngine.Plugins.Skydome;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Rendering.Shadows;
using ApexEngine.Scene;
using ApexEngine.Scene.Audio;
using ApexEngine.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Demos
{
    public class TestLandscape : Game
    {
        AudioNode an;
        
        public TestLandscape(Renderer renderer) : base(renderer)
        {
        }

        public override void Init()
        {
            Environment.AmbientLight.Color.Set(0.2f, 0.15f, 0.1f, 1.0f);
            Environment.DirectionalLight.Direction.Set(1.9f, 0.35f, 1.9f).NormalizeStore();
            Environment.DirectionalLight.Color.Set(0.5f, 0.3f, 0.1f, 1.0f);
            ((PerspectiveCamera)Camera).FieldOfView = 75;

            ShadowMappingComponent smc;
            RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment));
            smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;

            Geometry landscape = ((Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\landscape\\map.obj")).GetChildGeom(0);
            rootNode.AddChild(landscape);


            landscape.SetShader(typeof(Terrain.TerrainShader));
            Material terrainMaterial = new Material();

            Texture grass = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass.jpg");
            Texture grass_nrm = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass_NRM.jpg");
            Texture dirt = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt.jpg");
            Texture dirt_nrm = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt_NRM.jpg");



            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE0, grass);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_SCALE_0, 16);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL0, grass_nrm);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE_SLOPE, dirt);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL_SLOPE, dirt_nrm);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_SCALE_SLOPE, 16);
            terrainMaterial.SetValue(Material.MATERIAL_CASTSHADOWS, false);
            terrainMaterial.SetValue(Material.SHININESS, 0.15f);
            terrainMaterial.SetValue(Material.ROUGHNESS, 0.8f);


            landscape.Material = terrainMaterial;

            landscape.SetLocalScale(new Math.Vector3f(3f, 2f, 3f));

            PhysicsWorld.AddObject(landscape, 0.0f);



            AddComponent(new SkydomeComponent());


          //  Rendering.NormalMapRenderer nmr;
          //  RenderManager.AddComponent(nmr = new Rendering.NormalMapRenderer(Environment, Camera));
         //   RenderManager.PostProcessor.PostFilters.Add(new Rendering.PostProcess.Filters.SSAOFilter(nmr));


            GrassPopulator grassPop;
            rootNode.AddController(grassPop = new GrassPopulator(PhysicsWorld, cam));
            grassPop.GenPatches(12, 3, 128);

            
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
