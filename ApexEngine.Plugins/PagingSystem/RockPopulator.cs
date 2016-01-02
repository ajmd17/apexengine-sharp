using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using System;

namespace ApexEngine.Plugins.PagingSystem
{
    public class RockPopulator : Populator
    {
        private static Geometry model;
        private PhysicsWorld physicsWorld;
        private Vector3f tmpOrigin = new Vector3f(), tmpDir = new Vector3f();

        public RockPopulator(PhysicsWorld physicsWorld, Camera cam) : base(cam, true)
        {
            this.physicsWorld = physicsWorld;
        }

        static RockPopulator()
        {
            model = ((Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\rocks\\rock_0.obj")).GetChildGeom(0);
            model.Material.SetValue(Material.SPECULAR_EXPONENT, 60);
            model.Material.SetValue(Material.SHININESS, 0.3f);
            model.Material.SetValue(Material.ROUGHNESS, 0.2f);
        }

        public override void Destroy()
        {
            for (int i = patches.Count - 1; i > -1; i--)
            {
                if (patches[i].entities != null)
                    patches[i].entities.Dispose();
                patches[i].entities = null;
                patches[i].tile = null;
                patches[i] = null;
            }
            patches.Clear();
        }

        public override GameObject CreateEntity(Vector3f translation, Vector3f slope)
        {
            int numThingsInCluster = rand.Next(3, 9);
            Node clusterNode = new Node();
            for (int i = 0; i < numThingsInCluster; i++)
            {
                Vector3f clusterLoc;
                clusterLoc = new Vector3f((float)RandomDouble(-3f, 3f), 0, (float)RandomDouble(-3f, 3f));
                

                Geometry m = (Geometry)model.Clone();

                Vector3f pieceLoc;

                if (i > 0)
                    pieceLoc = clusterLoc.Add(translation);
                else
                    pieceLoc = translation;

            //    pieceLoc.y = this.GetHeight(pieceLoc.x, pieceLoc.z);
                
                m.SetLocalTranslation(pieceLoc);

                m.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, (float)RandomDouble(0, 359)));
                m.SetLocalScale(new Vector3f((float)this.RandomDouble(1.0, 1.9)));
                clusterNode.AddChild(m);
            }

            return clusterNode;
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
                        GridTile tile = new GridTile(chunkLoc, chunkSize, chunkSize, chunkLoc.x, chunkLoc.y, 85f);
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