using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene.Components;
using ApexEngine.Scene.Physics;
using System;
using System.Collections.Generic;

namespace ApexEngine.Terrain
{
    public abstract class TerrainComponent : GameComponent
    {
        public enum PageState
        {
            Loaded,
            Unloading,
            Unloaded
        }

        protected int chunkSize = 32;
        protected PhysicsWorld physicsWorld;
        protected List<HeightInfo> heightmaps;
        protected Material terrainMaterial = new TerrainMaterial();
        protected List<Vector2f> queue = new List<Vector2f>();
        protected Vector3f scale = new Vector3f(1);
        private float updateTime = 6, maxUpdateTime = 6f, queueTime = 6, maxQueueTime = 6f;
        private Vector2f v2cp = new Vector2f();
        private Vector3f cp = new Vector3f();
        private float maxDist = 1.8f;
        private Vector3f wt = new Vector3f();
        private Vector2f tmpCenter = new Vector2f();

        public TerrainComponent()
        {
            heightmaps = new List<HeightInfo>();
        }

        public TerrainComponent(PhysicsWorld physicsWorld)
        {
            this.physicsWorld = physicsWorld;
            heightmaps = new List<HeightInfo>();
        }

        protected HeightInfo HmWithCoords(int x, int z)
        {
            for (int i = 0; i < heightmaps.Count; i++)
            {
                if (heightmaps[i].position.x == x && heightmaps[i].position.y == z)
                {
                    return heightmaps[i];
                }
            }

            return null;
        }

        protected HeightInfo HmWithCoords(float x, float z)
        {
            return HmWithCoords((int)x, (int)z);
        }

        protected Vector2f[] GetNeighbors(HeightInfo origin)
        {
            Vector2f[] nb = new Vector2f[8];
            nb[0] = new Vector2f(origin.position.x + 1, origin.position.y);
            nb[1] = new Vector2f(origin.position.x - 1, origin.position.y);
            nb[2] = new Vector2f(origin.position.x, origin.position.y + 1);
            nb[3] = new Vector2f(origin.position.x, origin.position.y - 1);
            nb[4] = new Vector2f(origin.position.x + 1, origin.position.y - 1);
            nb[5] = new Vector2f(origin.position.x - 1, origin.position.y - 1);
            nb[6] = new Vector2f(origin.position.x + 1, origin.position.y + 1);
            nb[7] = new Vector2f(origin.position.x - 1, origin.position.y - 1);
            return nb;
        }

        protected TerrainChunkNode GetChunk(int x, int z)
        {
            for (int i = 0; i < this.heightmaps.Count; i++)
            {
                HeightInfo hi = heightmaps[i];
                if (((int)hi.position.x) == x && ((int)hi.position.y) == z)
                {
                    return hi.chunk;
                }
            }
            return null;
        }

        protected Vector2f ScaleDivide(Vector2f a)
        {
            return a.Multiply(1f / ((int)chunkSize - 1)).Multiply(new Vector2f(scale.x, scale.z));
        }

        protected void AddToQueue(Vector2f chunk)
        {
            queue.Add(chunk);
        }

        private void AddChunk(int x, int z)
        {
            HeightInfo hi = HmWithCoords(x, z);

            if (hi == null)
            {
                Vector2f vec = new Vector2f(x, z);
                if (vec.Distance(this.v2cp) < maxDist)
                {
                    TerrainChunkNode hmtc = CreateNewChunk(physicsWorld, this, x, z, scale, chunkSize, null);

                    heightmaps.Add(hi = new HeightInfo(this, vec, hmtc));
                    hi.chunk.Create();
                    ApplyTerrainMaterial(hmtc);
                    hi.chunk.SetLocalTranslation(new Vector3f(x * (chunkSize - 1) * scale.x, 0, z * (chunkSize - 1) * scale.z));
                    hi.midpoint = new Vector2f(hi.chunk.GetLocalTranslation().x - ((chunkSize - 1) * scale.x / 2), hi.chunk.GetLocalTranslation().z - (chunkSize - 1) * scale.z / 2);

                    Vector2f[] neighbors = GetNeighbors(hi);
                    hi.neighbors = neighbors;
                    rootNode.AddChild(hi.chunk);

                    hi.pageState = PageState.Loaded;
                }
            }
            else
            {
            }
        }

        public abstract TerrainChunkNode CreateNewChunk(PhysicsWorld physicsWorld, TerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, TerrainChunkNode[] neighbors);

        public override void Update()
        {
            if (queue.Count > 0)
            {
                if (queueTime > maxQueueTime)
                {
                    Vector2f v = queue[0];
                    int x = (int)v.x;
                    int y = (int)v.y;
                    AddChunk(x, y);
                    queue.Remove(v);
                    queueTime = 0f;
                }
                queueTime += 0.1f;
            }
            if (updateTime > maxUpdateTime)
            {
                /* cp.Set(cam.Translation)
                     .SubtractStore(new Vector3f((chunkSize * scale.x) / 2))
                     .SubtractStore(rootNode.GetWorldTranslation())
                     .MultiplyStore(1f / (((int)chunkSize - 1) * (scale.x)));*/

                cp.Set(cam.Translation);
                cp.SubtractStore(rootNode.GetWorldTranslation());
                cp.MultiplyStore(1f / (((int)chunkSize - 1) * (scale.x)));



                v2cp.Set(cp.x, cp.z);
                Console.WriteLine(rootNode.Children.Count);
                for (int i = heightmaps.Count - 1; i > -1; i--)
                {
                    HeightInfo hinf = heightmaps[i];

                    if (hinf.pageState == PageState.Loaded)
                    {
                        if (hinf.position.Distance(v2cp) > maxDist)
                        {
                            hinf.pageState = PageState.Unloaded;
                        }
                        else
                        {
                            foreach (Vector2f v2 in hinf.neighbors)
                            {
                                AddChunk((int)v2.x, (int)v2.y);
                            }
                        }
                    }
                    else if (hinf.pageState == PageState.Unloading)
                    {
                    }
                    else if (hinf.pageState == PageState.Unloaded)
                    {
                        rootNode.RemoveChild(hinf.chunk);
                        heightmaps.Remove(hinf);
                        hinf.chunk.hm = null;
                        hinf.chunk = null;
                    }
                    hinf.UpdateChunk();
                }
                AddChunk((int)cp.x, (int)cp.z);
                updateTime = 0f;
            }
            else
            {
                updateTime += 0.1f;
            }
        }

        public Vector3f GetNormal(Vector3f worldPosition)
        {
            wt.Set(worldPosition);
            wt.DivideStore(new Vector3f((chunkSize - 1) * scale.x));
            if (wt.x < 0f)
            {
                wt.x -= 1;
            }
            if (wt.z < 0f)
            {
                wt.z -= 1;
            }
            wt.x = (int)wt.x;
            wt.z = (int)wt.z;
            TerrainChunkNode closest = (TerrainChunkNode)GetChunk((int)wt.x, (int)wt.z);
            if (closest != null)
            {
                Vector3f chunkSpace = new Vector3f(worldPosition);
                chunkSpace.DivideStore(scale);
                chunkSpace.SubtractStore(closest.GetLocalTranslation());
                chunkSpace.SubtractStore(rootNode.GetWorldTranslation());
                return closest.GetNormal(chunkSpace);
            }
            return null;
        }

        public float GetHeight(Vector3f worldPosition)
        {
            wt.Set(worldPosition);
            wt.DivideStore(new Vector3f((chunkSize - 1) * scale.x));
            if (wt.x < 0f)
            {
                wt.x -= 1;
            }
            if (wt.z < 0f)
            {
                wt.z -= 1;
            }
            wt.x = (int)wt.x;
            wt.z = (int)wt.z;
            TerrainChunkNode closest = (TerrainChunkNode)GetChunk((int)wt.x, (int)wt.z);
            if (closest != null)
            {
                Vector3f chunkSpace = new Vector3f(worldPosition);
                chunkSpace.DivideStore(scale);
                return closest.GetHeight(chunkSpace);
            }
            return float.NaN;
        }

        public abstract void ApplyTerrainMaterial(TerrainChunkNode chunk);

        public abstract void OnAddChunk(TerrainChunkNode chunk);

        public abstract void OnRemoveChunk(TerrainChunkNode chunk);

        public abstract Material GetMaterial();
    }
}