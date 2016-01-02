using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Plugins.PagingSystem;
using ApexEngine.Plugins.Shaders.Post;
using ApexEngine.Plugins.Skydome;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Rendering.Shadows;
using ApexEngine.Scene;
using System;

namespace ApexEngine.Demos
{
    public class TestGame : Game
    {
        private Node thing;
        private ApexEngine.Terrain.SimplexTerrain.SimplexTerrainComponent terrain;
        private Texture2D tex;
        private bool added = false;

        Node tree;

        public TestGame(Renderer renderer) : base(renderer)
        {
        }

        public override void Init()
        {
            Environment.AmbientLight.Color.Set(0.1f, 0.2f, 0.3f, 1.0f);
            Environment.FogColor.Set(0.2f, 0.3f, 0.45f, 1.0f);
            Environment.DirectionalLight.Direction.Set(1f, 1, 1f).NormalizeStore();
            Environment.DirectionalLight.Color.Set(1.0f, 0.88f, 0.6f, 1.0f);
            ((PerspectiveCamera)Camera).FieldOfView = 60;
            Camera.Far = 330;

            ShadowMappingComponent smc;
            RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment, new int[] {1024, 1024}));
            smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;

          /*  thing = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\house.obj");

            for (int i = 0; i < 1; i++)
            {
                GameObject thing1 = thing.Clone();
                rootNode.AddChild(thing1);
                thing1.SetLocalTranslation(new Vector3f(i * 7, -0.5f, 0));
            }*/


        //    tree = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\tree\\pine\\LoblollyPine.obj");
        //    tree.SetLocalScale(new Vector3f(0.35f));

            
                        // Test an Apex Engine 3D model, with a material created in the material editor
            /*
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

                        Rendering.Light.PointLight pl = new Rendering.Light.PointLight();
                        pl.Color = new Vector4f(0.2f, 0.0f, 0.0f, 1.0f);
                        pl.Position = new Vector3f(0.0f, 7.0f,15.0f);
                   //    Environment.PointLights.Add(pl);

                        Rendering.Light.PointLight pl2 = new Rendering.Light.PointLight();
                        pl2.Color = new Vector4f(0.0f, 0.3f, 0.1f, 1.0f);
                        pl2.Position = new Vector3f(0.0f, 1.0f, 0.0f);
                       //   Environment.PointLights.Add(pl2);*/
            Node loadedApx = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\mitsuba.obj");
            loadedApx.SetLocalTranslation(new Math.Vector3f(0f, 0.0f, 0));

            Cubemap skybox = Cubemap.LoadCubemap(new string[] { AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_right.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_left.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_top.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_top.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_front.jpg",
                                                                 AssetManager.GetAppPath() + "\\textures\\lostvalley\\lostvalley_back.jpg" });

    
            for (int x = 0; x < 5; x++)
            {
                for (int z = 0; z < 5; z++)
                {
                    Node thing = (Node)loadedApx.Clone();
                    thing.SetLocalTranslation(new Vector3f(x * 4, 0, z * 4));
                    thing.GetChildGeom(0).Material.SetValue(Material.COLOR_DIFFUSE, new Color4f(0.5f, 0.5f, 0.5f, 1.0f));
                    thing.GetChildGeom(0).Material.SetValue(Material.TEXTURE_ENV, skybox);
                    thing.GetChildGeom(0).Material.SetValue(Material.SHININESS, 1.0f);
                    thing.GetChildGeom(0).Material.SetValue(Material.ROUGHNESS, 0.5f);
                    thing.GetChildGeom(0).UpdateShaderProperties();

                    thing.GetChildGeom(1).Material.SetValue(Material.COLOR_DIFFUSE, new Color4f(0.901f, 0.808f, 0.502f, 1.0f));
                    thing.GetChildGeom(1).Material.SetValue(Material.TEXTURE_ENV, skybox);
                    thing.GetChildGeom(1).Material.SetValue(Material.SHININESS, (x + 1) / 5.0f);
                    thing.GetChildGeom(1).Material.SetValue(Material.ROUGHNESS, (z + 1) / 5.0f);
                    thing.GetChildGeom(1).UpdateShaderProperties();
                    //     thing.GetChildGeom(1).Material.SetValue(Material.TEXTURE_NORMAL, nrm);

                    rootNode.AddChild(thing);
                }
            }




            /* Node terrainModel = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\landscape\\landscape.obj");
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
 */

            terrain = new ApexEngine.Terrain.SimplexTerrain.SimplexTerrainComponent(PhysicsWorld);
            terrain.BiomesEnabled = true;
            terrain.ChunkAdded += new Terrain.TerrainComponent.ChunkAddedHandler(OnChunkAdd);
            terrain.ChunkRemoved += new Terrain.TerrainComponent.ChunkRemovedHandler(OnChunkRemove);

            AddComponent(terrain);

            AddComponent(new SkydomeComponent());

            //  tex = (Texture2D)AssetManager.Load(AssetManager.GetAppPath() + "\\textures\\apex3d.png");
            // Sprite sprite = new Sprite();
            // rootNode.AddChild(sprite);

            /*  ShadowMappingComponent smc;
              RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment));
              smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;
              */

            //        Rendering.NormalMapRenderer nmr;
            //        RenderManager.AddComponent(nmr = new Rendering.NormalMapRenderer(Environment, Camera));
            //        RenderManager.PostProcessor.PostFilters.Add(new Rendering.PostProcess.Filters.SSAOFilter(nmr));

            RenderManager.PostProcessor.PostFilters.Add(new FXAAFilter());




           /* Node n = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\monkeyhq.obj");
            n.SetLocalTranslation(new Vector3f(0, 100, 0));
            n.GetChildGeom(0).Material.SetValue(Material.COLOR_DIFFUSE, new Color4f(0.0f, 0.0f, 0.0f, 1.0f));
            n.GetChildGeom(0).Material.SetValue(Material.SHININESS, 1.0f);
            n.GetChildGeom(0).Material.SetValue(Material.ROUGHNESS, 0.3f);
            rootNode.AddChild(n);
            PhysicsWorld.AddObject(n, 250, Scene.Physics.PhysicsWorld.PhysicsShape.Box);*/
            //   PhysicsWorld.AddCharacter((DefaultCamera)cam, InputManager, n, 1);


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
                grass.GenPatches(chunk, 5, 6);
            }
            if (!chunk.HasController(typeof(RockPopulator)))
            {
                RockPopulator rock;
                chunk.AddController(rock = new RockPopulator(PhysicsWorld, cam));
                rock.GenPatches(chunk, 2, 2);
            }
        /*    
            for (int i = 0; i < chunk.hm.heights.Length; i++)
            {
                chunk.hm.heights[i] = 0f;
            }
            chunk.hm.RebuildTerrainMesh();*/

           
        }

        public void OnChunkRemove(Terrain.TerrainChunkNode chunk, EventArgs e)
        {
           if (chunk.HasController(typeof(GrassPopulator)))
            {
                GrassPopulator grass = (GrassPopulator)chunk.GetController(typeof(GrassPopulator));
                grass.Destroy();
                grass.Destroy();
            }
            if (chunk.HasController(typeof(RockPopulator)))
            {
                RockPopulator rock = (RockPopulator)chunk.GetController(typeof(RockPopulator));
                rock.Destroy();
                rock = null;
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