using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Plugins.PagingSystem
{
    public class GrassPopulator : Populator
    {
        Geometry model;
        private PhysicsWorld physicsWorld;

        public GrassPopulator(PhysicsWorld physicsWorld, Camera cam) : base(cam, true)
        {
            this.physicsWorld = physicsWorld;
            model = ((Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\grass\\grass.obj")).GetChildGeom(0);
             model.SetLocalScale(new Vector3f(0.5f));
            model.Material.SetValue(Material.COLOR_DIFFUSE, new Vector4f(0.9f, 0.7f, 0.6f, 1f));
            model.Material.SetValue(Material.MATERIAL_BLENDMODE, 1);
            model.Material.SetValue(Material.MATERIAL_CASTSHADOWS, false);
            model.Material.SetValue(Material.MATERIAL_ALPHADISCARD, 0.28f);
            model.Material.SetValue(Material.SHININESS, 0.03f);
            model.Material.SetValue(Material.ROUGHNESS, 0.1f);
            model.Material.SetValue(Material.MATERIAL_CULLENABLED, false);
            model.Material.Bucket = RenderManager.Bucket.Transparent;
        }
        
        public override GameObject CreateEntity(Vector3f translation, Vector3f slope)
        {
            Geometry m = (Geometry)model.Clone();
            m.SetLocalTranslation(translation);
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

            Vector2f cent = origin.Add(new Vector2f(parentSize / 2));
            for (int x = 0; x < numChunks; x++)
            {
                for (int z = 0; z < numChunks; z++)
                {
                    Vector2f offset = new Vector2f(x * chunkSize, z * chunkSize);
                    Vector2f chunkLoc = origin.Add(offset).Subtract(max.Divide(2));
                    GridTile tile = new GridTile(chunkLoc, chunkSize, chunkSize, chunkLoc.x, chunkLoc.y, 25f);
                    Patch patch = new Patch(CreateEntityNode(new Vector3f(chunkLoc.x, 0, chunkLoc.y), parent, chunkSize, numEntityPerChunk), (Node)parent, tile);
                    patches.Add(patch);
                }
            }


        }

        public override float GetHeight(float x, float z)
        {
            Vector3f origin = new Vector3f(x, 100, z);
            Vector3f dir = new Vector3f(0, -1.001f, 0).Multiply(1000f);
            Vector3f rayHit;
            GameObject hitObject;
            physicsWorld.Raycast(origin, dir, out rayHit, out hitObject);
            if (rayHit != null)
            {
                return rayHit.y;
            }
            return 30;
        }

        public override void Init()
        {
         //   this.GenPatches(GameObject, new Vector2f(0,0), new Vector2f(0, 0), 3, 6, 64f);
        }

       
    }
}
