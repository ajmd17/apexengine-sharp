using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering.Shadows;
using ApexEngine.Rendering.Cameras;

namespace ApexEngine.Demos
{
    public class TestGame : Game
    {
        public override void Init()
        {
            Environment.AmbientLight.Color.Set(0.2f, 0.15f, 0.1f, 1.0f);
            Environment.DirectionalLight.Direction.Set(0.7f, 1, 0.7f);
            Environment.DirectionalLight.Color.Set(1.0f, 0.9f, 0.8f, 1.0f);


            ((PerspectiveCamera)Camera).FieldOfView = 75;

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

            /*Node boxNode = new Node();
            Geometry box = new Geometry(Rendering.Util.MeshFactory.CreateCube(new Vector3f(-1), new Vector3f(1)));
            boxNode.AddChild(box);
            boxNode.SetLocalTranslation(new Vector3f(0, 12, 0));
            rootNode.AddChild(boxNode);
            PhysicsWorld.AddObject(boxNode);*/


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
             loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.SHININESS, 1.0f);
             loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.ROUGHNESS, 0.3f);
             //loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.METALNESS, 0.7f);
             loadedApx.GetChildNode(0).GetChildGeom(0).Material.SetValue(Material.SPECULAR_EXPONENT, 10f);
             loadedApx.GetChildNode(0).GetChildGeom(0).UpdateShaderProperties();
             Console.WriteLine(loadedApx.GetChildNode(0).GetChildGeom(0).ShaderProperties);
           // rootNode.AddChild(AssetManager.LoadModel(AssetManager.GetAppPath() + "\\pbr_monkey.apx"));
          //  Assets.Apx.ApxExporter.ExportModel(AssetManager.GetAppPath() + "\\pbr_monkey.apx", loadedApx);
         //   RenderManager.PostProcessor.PostFilters.Add(new Rendering.PostProcess.GammaCorrectionFilter());
            Rendering.NormalMapRenderer nmr;
            RenderManager.AddComponent(nmr = new Rendering.NormalMapRenderer(Environment, Camera));
            RenderManager.PostProcessor.PostFilters.Add(new Rendering.PostProcess.Filters.SSAOFilter(nmr));
        }

        public override void Render()
        {
            //  PhysicsWorld.DrawDebug();
        }

        public override void Update()
        {
           // Console.WriteLine(RootNode.GetChildNode(0).GetChild(0).GetWorldRotation());
        }
    }
}
