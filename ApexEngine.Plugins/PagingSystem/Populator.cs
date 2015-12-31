using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;
using ApexEngine.Scene.Components;
using ApexEngine.Terrain;
using System;
using System.Collections.Generic;

namespace ApexEngine.Plugins.PagingSystem
{

    public abstract class Populator : Controller
    {
        protected List<Patch> patches = new List<Patch>();
        private float updateTime = 0f, maxUpdateTime = 5f;
        protected bool batchGeometry = true;
        protected Camera cam;
        private Vector2f tmpVec = new Vector2f(), tmpVec2 = new Vector2f();
        protected Random rand;

        public Populator(Camera cam, bool batchGeometry, int seed)
        {
            this.cam = cam;
            this.batchGeometry = batchGeometry;
            rand = new Random(seed);
        }

        public Populator(Camera cam, bool batchGeometry) : this(cam, batchGeometry, 12345)
        {
        }

        public Populator(Camera cam) : this(cam, true)
        {
        }

        protected double RandomDouble(double a, double b)
        {
            return a + rand.NextDouble() * (b - a);
        }

        /** Create a single entity. */

        public abstract GameObject CreateEntity(Vector3f translation, Vector3f slope);

        public Node CreateEntityNode(Vector3f translation, GameObject parentNode, float chunkSize, int entityPerChunk)
        {
            Node n = new Node();
            float mult = chunkSize / (float)entityPerChunk;
            parentNode.UpdateTransform();

            for (int x = 0; x < entityPerChunk; x++)
            {
                for (int z = 0; z < entityPerChunk; z++)
                {
                    float xLoc = (float)RandomDouble(0, chunkSize);
                    float yLoc = 3;
                    float zLoc = (float)RandomDouble(0, chunkSize);



                    yLoc = GetHeight(parentNode.GetWorldTranslation().x + translation.x + xLoc, parentNode.GetWorldTranslation().z + translation.z + zLoc);
                    //	Vec3f norm = getNormal(parentNode, translation.x + x * 4, translation.z + z * 4);
                    if (yLoc != float.NaN)
                    {
                        GameObject entity = CreateEntity(new Vector3f(xLoc, yLoc, zLoc), Vector3f.Zero);
                       // entity.SetLocalScale(new Vector3f((float)RandomDouble(0.1f, 0.5f)));
                        entity.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, (float)RandomDouble(0, 359)));
                        //  n.AddChild(CreateEntity(new Vector3f(x * mult, y, z * mult), Vector3f.ZERO));
                        n.AddChild(entity);
                    }
                }
            }
            if (batchGeometry)
            {
                Node merged = new Node();
                merged.AddChild(new Geometry(MeshUtil.MergeMeshes(n)));
                merged.SetLocalTranslation(translation);
                merged.GetChildGeom(0).SetShader(GetShaderType());
                for (int i = 0; i < n.Children.Count; i++)
                {
                    n.Children[i] = null;
                }
                n = null;
                return merged;
            }
            n.SetLocalTranslation(translation);
            return n;
        }

        public abstract Type GetShaderType();

        public abstract void GenPatches(GameObject parent,
                Vector2f origin,
                Vector2f center,
                int numChunks,
                int numEntityPerChunk,
                float parentSize);

        public void GenPatches(TerrainChunkNode terrain, int numPatches, int numEntityPerChunk)
        {
            GenPatches(terrain, new Vector2f(0,0), new Vector2f(0, 0), numPatches, numEntityPerChunk, terrain.ChunkSize*terrain.Scale.x);
        }

        public void GenPatches(int numPatches, int numEntityPerChunk, float totalSize)
        {
            GenPatches(GameObject, new Vector2f(0, 0), new Vector2f(0, 0), numPatches, numEntityPerChunk, totalSize);
        }

        public abstract float GetHeight(float x, float z);

        public override void Update()
        {
            tmpVec.x = cam.Translation.x;
            tmpVec.y = cam.Translation.z;
            if (updateTime > maxUpdateTime)
            {
                // do update
                for (int i = 0; i < patches.Count; i++)
                {
                    Patch p = patches[i];
                    tmpVec2.x = p.parentNode.GetWorldTranslation().x;
                    tmpVec2.y = p.parentNode.GetWorldTranslation().z;
                    if (p.tile.inRange(tmpVec.Subtract(tmpVec2)))
                    {
                        if (p.pageState != Patch.PageState.LOADED)
                        {
                            if (p.entities == null)
                            {
                                p.entities = this.CreateEntityNode(p.translation, p.parentNode, p.chunkSize, p.entityPerChunk);
                            }
                            p.parentNode.AddChild(p.entities);
                            p.pageState = Patch.PageState.LOADED;
                        }
                    }
                    else
                    {
                        if (p.pageState != Patch.PageState.UNLOADED)
                        {
                            if (p.pageState == Patch.PageState.LOADED)
                            {
                                p.pageState = Patch.PageState.UNLOADING;
                            } 
                            else if (p.pageState == Patch.PageState.UNLOADING)
                            {
                                
                            }
                        }
                        else
                        { // unloaded
                            if (p.entities != null)
                            {
                                p.parentNode.RemoveChild(p.entities);
                                p.entities.Dispose();
                                p.entities = null;
                            }
                        }
                    }
                }
                updateTime = 0f;
            }
            else
            {
                updateTime += 0.1f;
            }
            for (int i = 0; i < patches.Count; i++)
            {
                patches[i].Update(cam.Translation);
            }
        }
    }
}