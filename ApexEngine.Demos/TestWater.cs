using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.OpenGL;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Demos
{
    public class TestWater : Game
    {
        public TestWater(GLRenderer renderer) : base(renderer)
        {

        }

        public override void Init()
        {

            GameObject hall;
            rootNode.AddChild(hall = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\hall\\hall.obj"));
            hall.SetLocalScale(new Math.Vector3f(0.75f, 0.75f, 0.5f));
            PhysicsWorld.AddObject(hall, 0.0f);




            Texture normTex = AssetManager.LoadTexture(AssetManager.GetAppPath() + "\\textures\\wave_norm.jpg");

            Mesh quad = MeshFactory.CreateQuad();
            Geometry quadGeom = new Geometry(quad);
            quadGeom.Material.SetValue(Material.TEXTURE_NORMAL, normTex);
            quadGeom.Material.SetValue(Material.MATERIAL_CULLENABLED, false);
            quadGeom.SetLocalRotation(new Math.Quaternion().SetFromAxis(Vector3f.UnitX, 90));

            quadGeom.SetLocalScale(new Vector3f(3, 1, 3));
            quadGeom.Material.SetValue(Material.COLOR_DIFFUSE, new Vector4f(0.1f, 0.4f, 0.9f, 0.3f));
            quadGeom.Material.SetValue(Material.MATERIAL_BLENDMODE, 1);

            quadGeom.Material.Bucket = RenderManager.Bucket.Transparent;

            rootNode.AddChild(quadGeom);
        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
