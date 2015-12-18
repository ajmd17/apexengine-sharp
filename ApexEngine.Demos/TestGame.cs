using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Networking;
using ApexEngine.Plugins.PagingSystem;
using ApexEngine.Plugins.Shaders.Post;
using ApexEngine.Plugins.Skydome;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Rendering.Shadows;
using ApexEngine.Scene;
using ApexEngine.Terrain;
using System;

namespace ApexEngine.Demos
{
    public class TestGame : Game
    {

        Texture2D tex;

        public TestGame() : base(new Rendering.OpenGL.GLRenderer())
        {
        }

        public override void Init()
        {
            Environment.AmbientLight.Color.Set(0.2f, 0.15f, 0.1f, 1.0f);
            Environment.DirectionalLight.Direction.Set(0.4f, 1, 0.4f).NormalizeStore() ;
            Environment.DirectionalLight.Color.Set(1.0f, 0.9f, 0.8f, 1.0f);
            ((PerspectiveCamera)Camera).FieldOfView = 75;
            /*
                        // Test an Apex Engine 3D model, with a material created in the material editor
                        Node loadedApx = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\test_apx.apx");
                        loadedApx.SetLocalTranslation(new Math.Vector3f(0f, 45, 0));
                        rootNode.AddChild(loadedApx);
                        PhysicsWorld.AddObject(loadedApx, 1, Scene.Physics.PhysicsWorld.PhysicsShape.Box);

                        // Test an OBJ model, with normal mapping
                        GameObject loadedObj = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\house.obj");
                        //  ((Node)loadedObj).GetChildGeom(0).Material.SetValue(Material.TEXTURE_HEIGHT, Texture.LoadTexture("C:\\Users\\User\\Pictures\\Brick_14_UV_H_CM_1_DISP.jpg"));
                        rootNode.AddChild(loadedObj);
                        PhysicsWorld.AddObject(loadedObj, 0.0f);

                        // rootNode.AddChild(new Geometry(Rendering.Util.MeshFactory.CreateCube(new Vector3f(-1,-1,-1), new Vector3f(1,1,1))));
                        //  loadedObj.SetLocalScale(new Math.Vector3f(2, 0.5f, 2f));
                        //  Rendering.Animation.AnimationController anim = (Rendering.Animation.AnimationController)loadedApx.GetController(typeof(Rendering.Animation.AnimationController));
                        //  anim.PlayAnimation(1);

                        AddComponent(new ApexEngine.Terrain.SimplexTerrain.SimplexTerrainComponent(PhysicsWorld));
                        //AddComponent(new ApexEngine.Terrain.ModelTerrain.ModelTerrainComponent(AssetManager.GetAppPath() + "\\models\\terrain", "terrain", "obj"));

                        Geometry quadGeom = new Geometry(Rendering.Util.MeshFactory.CreateCube(new Vector3f(-15f, 0.4f, -15f), new Vector3f(15f, 0.5f, 15f)));
                       // quadGeom.SetLocalTranslation(new Vector3f(0, -8, 0));
                       // rootNode.AddChild(quadGeom);
                       // PhysicsWorld.AddObject(quadGeom, 0.0f);

                        ShadowMappingComponent smc;
                        RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment));
                        smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;

                        Rendering.Light.PointLight pl = new Rendering.Light.PointLight();
                        pl.Color = new Vector4f(0.2f, 0.0f, 0.0f, 1.0f);
                        pl.Position = new Vector3f(0.0f, 7.0f,15.0f);
                   //    Environment.PointLights.Add(pl);

                        Rendering.Light.PointLight pl2 = new Rendering.Light.PointLight();
                        pl2.Color = new Vector4f(0.0f, 0.3f, 0.1f, 1.0f);
                        pl2.Position = new Vector3f(0.0f, 1.0f, 0.0f);
                       //   Environment.PointLights.Add(pl2);
                        Cubemap skybox = Cubemap.LoadCubemap(new string[] { AssetManager.GetAppPath() + "\\textures\\frozendusk\\frozendusk_right.jpg",
                                                             AssetManager.GetAppPath() + "\\textures\\frozendusk\\frozendusk_left.jpg",
                                                             AssetManager.GetAppPath() + "\\textures\\frozendusk\\frozendusk_top.jpg",
                                                             AssetManager.GetAppPath() + "\\textures\\frozendusk\\frozendusk_top.jpg",
                                                             AssetManager.GetAppPath() + "\\textures\\frozendusk\\frozendusk_front.jpg",
                                                             AssetManager.GetAppPath() + "\\textures\\frozendusk\\frozendusk_back.jpg" });

                        // loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.TEXTURE_ENV, skybox);
                         loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.SHININESS, 0.4f);
                         loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.ROUGHNESS, 0.2f);
                         //loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.METALNESS, 0.7f);
                         loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.SPECULAR_EXPONENT, 2f);
                         loadedApx.GetChildNode(0).GetChildGeom(0).UpdateShaderProperties();
                         Console.WriteLine(loadedApx.GetChildNode(0).GetChildGeom(0).ShaderProperties);*/

            Node terrainModel = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\landscape\\landscape.obj");
            Geometry terrainGeom = terrainModel.GetChildGeom(0);
            terrainGeom.SetShader(typeof(Terrain.TerrainShader));
            Material terrainMaterial = new Material();

            Texture grass = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass.jpg");
            Texture grass_nrm = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass_NRM.jpg");
            Texture dirt = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt.jpg");
            Texture dirt_nrm = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt_NRM.jpg");
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE0, grass);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL0, grass_nrm);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE_SLOPE, dirt);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL_SLOPE, dirt_nrm);
            terrainMaterial.SetValue(Material.MATERIAL_CASTSHADOWS, false);
            terrainMaterial.SetValue(Material.SHININESS, 0.15f);
            terrainMaterial.SetValue(Material.ROUGHNESS, 0.1f);

            terrainGeom.Material = terrainMaterial;

            terrainModel.SetLocalScale(new Vector3f(1, 0.5f, 1));
            rootNode.AddChild(terrainModel);

            PhysicsWorld.AddObject(terrainModel, 0f);

            AddComponent(new SkydomeComponent());

            /* ApexEngine.Terrain.SimplexTerrain.SimplexTerrainComponent terrain = new ApexEngine.Terrain.SimplexTerrain.SimplexTerrainComponent(PhysicsWorld);
             terrain.ChunkAdded += new Terrain.TerrainComponent.ChunkAddedHandler(OnChunkAdd);

             AddComponent(terrain);*/

            ShadowMappingComponent smc;
            RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment));
            smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;

       /*       Rendering.NormalMapRenderer nmr;
              RenderManager.AddComponent(nmr = new Rendering.NormalMapRenderer(Environment, Camera));
              RenderManager.PostProcessor.PostFilters.Add(new Rendering.PostProcess.Filters.SSAOFilter(nmr));*/

    //        RenderManager.PostProcessor.PostFilters.Add(new FXAAFilter());

               GameObject loadedObj = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\house.obj");
               ((Node)loadedObj).GetChildGeom(0).Material.SetValue(Material.TEXTURE_HEIGHT, AssetManager.LoadTexture("C:\\Users\\User\\Pictures\\Brick_14_UV_H_CM_1_DISP.jpg"));
               rootNode.AddChild(loadedObj);
               PhysicsWorld.AddObject(loadedObj, 0.0f);
       //     rootNode.AddChild(new Geometry(Rendering.Util.MeshFactory.CreateCube(new Vector3f(-1, -1, -1), new Vector3f(1, 1, 1))));

            /*     GameObject scene = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\scene.apx");
                 PhysicsWorld.AddObject(scene, 0f);
                 rootNode.AddChild(scene);*/

            GrassPopulator grassPop;
            rootNode.AddController(grassPop = new GrassPopulator(PhysicsWorld, cam));
            grassPop.GenPatches(6, 5, 64);

            /*Rendering.NormalMapRenderer nmr;
            RenderManager.AddComponent(nmr = new Rendering.NormalMapRenderer(Environment, Camera));
            RenderManager.PostProcessor.PostFilters.Add(new Rendering.PostProcess.Filters.SSAOFilter(nmr));*/

            /*
            Geometry waterGeom = new Geometry(MeshFactory.CreateQuad());
            waterGeom.SetLocalScale(new Math.Vector3f(10));
            waterGeom.SetLocalRotation(new Math.Quaternion().SetFromAxis(Vector3f.UNIT_X, 90));
            waterGeom.SetShader(typeof(WaterShader));
            waterGeom.Material.SetValue(Material.TEXTURE_DIFFUSE, Texture.LoadTexture(AssetManager.GetAppPath() + "\\textures\\water.jpg"));
            waterGeom.Material.SetValue(Material.TEXTURE_NORMAL, Texture.LoadTexture(AssetManager.GetAppPath() + "\\textures\\water_NRM.jpg"));
            rootNode.AddChild(waterGeom);*/
            //   Console.WriteLine("\n\n" + ShaderUtil.FormatShaderProperties((string)ShaderTextLoader.GetInstance().Load(AssetManager.GetAppPath() + "\\shaders\\default.frag"), new ShaderProperties().SetProperty("NORMALS", true)));


            /*
            ServerGameComponent serv;
            this.AddComponent(serv = new ServerGameComponent(new ServerHandler((Message msg) => { })));
            serv.Connect(2222);*/
        }

        public void OnChunkAdd(Terrain.TerrainChunkNode chunk, EventArgs e)
        {
            if (!chunk.HasController(typeof(GrassPopulator)))
            {
                GrassPopulator grass;
                chunk.AddController(grass = new GrassPopulator(PhysicsWorld, cam));
                grass.GenPatches(chunk);
            }
        }

        public override void Render()
        {
            //      PhysicsWorld.DrawDebug();
        }

        public override void Update()
        {
            // Console.WriteLine(RootNode.GetChildNode(0).GetChild(0).GetWorldRotation());
        }
    }
}