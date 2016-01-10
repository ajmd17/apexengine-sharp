using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using System;
using ApexEngine.Rendering.Shaders;

namespace ApexEngine.Plugins.PagingSystem
{
    public class GrassPopulator : Populator
    {
        private static Geometry model;
        private PhysicsWorld physicsWorld;
        private Vector3f tmpOrigin = new Vector3f(), tmpDir = new Vector3f();

        public GrassPopulator(PhysicsWorld physicsWorld, Camera cam) : base(cam, true, 0.65f)
        {
            this.physicsWorld = physicsWorld;
        }

        static GrassPopulator()
        {
            model = ((Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\grass\\grass.obj")).GetChildGeom(0);
            model.Material.SetValue(Material.COLOR_DIFFUSE, new Vector4f(0.5f, 0.48f, 0.52f, 1f));
            model.Material.SetValue(Material.MATERIAL_BLENDMODE, 1);
            model.Material.SetValue(Material.MATERIAL_CASTSHADOWS, false);
            model.Material.SetValue(Material.MATERIAL_ALPHADISCARD, 0.2f);
            model.Material.SetValue(Material.SPECULAR_EXPONENT, 60);
            model.Material.SetValue(Material.SHININESS, 0.1f);
            model.Material.SetValue(Material.ROUGHNESS, 0.0f);
            model.Material.SetValue(Material.MATERIAL_CULLENABLED, false);
            model.Material.SetValue("fade_start", 30.0f);
            model.Material.SetValue("fade_end", 40.0f);
            model.Material.Bucket = RenderManager.Bucket.Transparent;
            model.SetShader(typeof(GrassShader));
        }


        public override Type GetShaderType()
        {
            return typeof(ApexEngine.Rendering.Shaders.GrassShader);
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
            Geometry m = (Geometry)model.Clone();
            m.SetLocalTranslation(translation);
            m.SetLocalScale(new Vector3f((float)this.RandomDouble(0.25, 0.4)));
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
                    GridTile tile = new GridTile(chunkLoc, chunkSize, chunkSize, chunkLoc.x, chunkLoc.y, 50f);
                    Patch patch = new Patch( (Node)parent, tile);

                    patch.translation = new Vector3f(chunkLoc.x, 0, chunkLoc.y);
                    patch.chunkSize = chunkSize;
                    patch.entityPerChunk = numEntityPerChunk;

                    patches.Add(patch);
                }
            }
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