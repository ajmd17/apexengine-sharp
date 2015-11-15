using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Scene;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Util;
namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainComponent : TerrainComponent
    {
        public float[] rHeights;
        public List<HeightInfo> heightmaps;
        Vector3f scale = new Vector3f(1);
        int chunkWidth;
        int chunkHeight;
        int rows = 4;
        int cols = 4;
        int chunkSize = 64;
        Node box;
        Material terrainMaterial = new Material();
        OpenSimplexNoise[] octaves;
        double[] frequencys;
        double[] amplitudes;
        private Vector3f wt = new Vector3f();
        private Vector2f tmpCenter = new Vector2f();
        int maxDist = 2;
        float updateTime = 2, maxUpdateTime = 2f, queueTime = 2, maxQueueTime = .5f;
        Vector2f v2cp = new Vector2f();
        Vector3f cp = new Vector3f();
        private List<Vector2f> queue = new List<Vector2f>();
        private Vector2f[] tmpVec = new Vector2f[8];

        public SimplexTerrainComponent()
        {
            rHeights = new float[4];
            int numberOfOctaves = 8;
            octaves = new OpenSimplexNoise[numberOfOctaves];
            frequencys = new double[numberOfOctaves];
            amplitudes = new double[numberOfOctaves];
            for (int i = 0; i < 8; i++)
                tmpVec[i] = new Vector2f();

            for (int i = 0; i < numberOfOctaves; i++)
            {
                octaves[i] = new OpenSimplexNoise(2346);
                frequencys[i] = (float)System.Math.Pow(2, i);
                amplitudes[i] = (float)System.Math.Pow(0.5f, octaves.Length - i);
            }
        }

        public double getNoise(int x, int y)
        {
            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
            }

            return result;
        }

        public HeightInfo HmWithCoords(int x, int z)
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

        public HeightInfo HmWithCoords(float x, float z)
        {
            return HmWithCoords((int)x, (int)z);
        }

        private Vector2f[] GetNeighbors(HeightInfo origin)
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

        public override Material GetMaterial()
        {
            return terrainMaterial;
        }

        public override void Init()
        {
            heightmaps = new List<HeightInfo>();
        }

        public override void OnAddChunk(TerrainChunkNode chunk)
        {
            //throw new NotImplementedException();
        }

        public override void OnRemoveChunk(TerrainChunkNode chunk)
        {
            //throw new NotImplementedException();
        }
        
        public override Vector3f GetNormal(Vector3f worldPosition)
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
            SimplexTerrainChunkNode closest = (SimplexTerrainChunkNode)GetChunk((int)wt.x, (int)wt.z);
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

        public override float GetHeight(Vector3f worldPosition)
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
            SimplexTerrainChunkNode closest = (SimplexTerrainChunkNode)GetChunk((int)wt.x, (int)wt.z);
            if (closest != null)
            {
                Vector3f chunkSpace = new Vector3f(worldPosition);
                chunkSpace.DivideStore(scale);
                return closest.GetHeight(chunkSpace);
            }
            return float.NaN;
        }

        public TerrainChunkNode GetChunk(int x, int z)
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

        private void AddChunk(int x, int z)
        {
            HeightInfo hi = HmWithCoords(x, z);

            if (hi == null)
            {
                Vector2f vec = new Vector2f(x, z);
                if (vec.Distance(this.v2cp) < maxDist)
                {
                    SimplexTerrainChunkNode hmtc = new SimplexTerrainChunkNode(this, x, z, scale, chunkSize, null);

                    heightmaps.Add(hi = new HeightInfo(this, vec, hmtc));
                    hi.chunk.Create();
                    hmtc.GetChildGeom(0).Material = terrainMaterial;
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

        public Vector2f ScaleDivide(Vector2f a)
        {
            return a.Multiply(1f / ((int)chunkSize - 1)).Multiply(new Vector2f(scale.x, scale.z));
        }

        private void AddToQueue(Vector2f chunk)
        {
            queue.Add(chunk);
        }

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
                queueTime += 0.1f;//GameTime.getDeltaTime() * 10f;
            }
            if (updateTime > maxUpdateTime)
            {
                cp.Set(cam.Translation).SubtractStore(new Vector3f((chunkSize * scale.x) / 2)).SubtractStore(rootNode.GetWorldTranslation()).MultiplyStore(1f / (((int)chunkSize - 1) * (scale.x)));

                v2cp.Set(cp.x, cp.z);
                Console.WriteLine(rootNode.Children.Count);//heightmaps.Count);
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
                updateTime += 0.1f;//GameTime.getDeltaTime();
            }
        }

    }
}
