using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using ApexEngine.Rendering.Shaders;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene.Components.Controllers;

namespace ApexEngine.Plugins.PagingSystem
{
    public class TreePopulator : Populator
    {
        private static Node tree;
        private PhysicsWorld physicsWorld;
        private Vector3f tmpOrigin = new Vector3f(), tmpDir = new Vector3f();

        public TreePopulator(PhysicsWorld physicsWorld, Camera cam)
            : base(cam, false, 0.4f)
        {
            this.physicsWorld = physicsWorld;
        }

        static TreePopulator()
        {


          /*  tree = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\tree\\pine\\LoblollyPine.obj");

            tree.GetChildGeom(1).Material.SetValue("tree_height", tree.GetLocalBoundingBox().Max.y);
            tree.GetChildGeom(1).SetShader(typeof(BarkShader));

            tree.GetChildGeom(1).DepthShader = ShaderManager.GetShader(typeof(BarkShader), new ShaderProperties()
                .SetProperty("DEPTH", true)
                .SetProperty(Material.TEXTURE_DIFFUSE, true));

            tree.GetChildGeom(2).Material.SetValue("tree_height", tree.GetLocalBoundingBox().Max.y);
            tree.GetChildGeom(2).Material.SetValue(Material.MATERIAL_BLENDMODE, 1);
            tree.GetChildGeom(2).Material.SetValue(Material.MATERIAL_CULLENABLED, false);
            tree.GetChildGeom(2).Material.SetValue(Material.MATERIAL_ALPHADISCARD, 0.7f);
            tree.GetChildGeom(2).Material.Bucket = Rendering.RenderManager.Bucket.Transparent;
            tree.GetChildGeom(2).SetShader(typeof(LeafShader));

            tree.GetChildGeom(2).DepthShader = ShaderManager.GetShader(typeof(LeafShader), new ShaderProperties()
                .SetProperty("DEPTH", true)
                .SetProperty(Material.TEXTURE_DIFFUSE, true));*/

            tree = new Node();

            Geometry g = new Geometry(MeshFactory.CreateQuad());
            g.SetLocalScale(new Vector3f(15));
            g.SetLocalTranslation(new Vector3f(0, 10, 0));
            g.Material.SetValue(Material.MATERIAL_CASTSHADOWS, 0);
            g.Material.SetValue(Material.MATERIAL_BLENDMODE, 1);
            g.Material.SetValue(Material.TEXTURE_DIFFUSE, AssetManager.LoadTexture(AssetManager.GetAppPath() + "\\models\\tree\\pine\\billboard.png"));
            g.Material.SetValue(Material.SHININESS, 0.0f);
            g.Material.SetValue(Material.MATERIAL_ALPHADISCARD, 0.5f);
            g.Material.Bucket = RenderManager.Bucket.Transparent;
            g.SetShader(typeof(GrassShader));
            g.Material.SetValue("fade_start", 250.0f);
            g.Material.SetValue("fade_end", 260.0f);
            tree.AddChild(g);

        }

        public override void Destroy()
        {
            for (int i = patches.Count - 1; i > -1; i--)
            {
                patches[i].entities = null;
                patches[i].tile = null;
                patches[i] = null;
            }
            patches.Clear();
        }

        public override GameObject CreateEntity(Vector3f translation, Vector3f slope)
        {

            Node m = (Node)tree.Clone();
            m.AddController(new BillboardControl(this.cam));
                Vector3f pieceLoc;
                    pieceLoc = translation;

                //    pieceLoc.y = this.GetHeight(pieceLoc.x, pieceLoc.z);

                m.SetLocalTranslation(pieceLoc);

                m.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, (float)RandomDouble(0, 359)));
                m.SetLocalScale(new Vector3f((float)this.RandomDouble(0.8f, 1.2f)));
               // ((Geometry)m).SetShader(typeof(DefaultShader));
            

            return m;
        }

        public override void GenPatches(GameObject parent,
                Vector2f origin,
                Vector2f center,
                int numChunks,
                int numEntityPerChunk,
                float parentSize)
        {
            float chunkSize = parentSize / ((float)numChunks);

            Vector2f max = new Vector2f((numChunks) * chunkSize);
            max.DivideStore(2);

            for (int x = 0; x < numChunks; x++)
            {
                for (int z = 0; z < numChunks; z++)
                {

                    Vector2f offset = new Vector2f(x * chunkSize, z * chunkSize);
                    Vector2f chunkLoc = origin.Add(offset).SubtractStore(max);
                    GridTile tile = new GridTile(chunkLoc, chunkSize, chunkSize, chunkLoc.x, chunkLoc.y, 270);
                    Patch patch = new Patch((Node)parent, tile);

                    patch.translation = new Vector3f(chunkLoc.x, 0, chunkLoc.y);
                    patch.chunkSize = chunkSize;
                    patch.entityPerChunk = numEntityPerChunk;

                    patches.Add(patch);


                }
            }
        }

        public override Type GetShaderType()
        {
            return typeof(ApexEngine.Rendering.Shaders.DefaultShader);
        }

        public override float GetHeight(float x, float z)
        {
            tmpOrigin.Set(x, 500, z);
            tmpDir.Set(0, -1.001f, 0).MultiplyStore(1000f);
            Vector3f rayHit;
            GameObject hitObject;
            physicsWorld.Raycast(tmpOrigin, tmpDir, out rayHit, out hitObject);
            if (rayHit != null)
            {
                return rayHit.y;
            }
            return float.NaN;
        }

        public override void Init()
        {
            //   this.GenPatches(GameObject, new Vector2f(0,0), new Vector2f(0, 0), 3, 6, 64f);
        }
    }
}
