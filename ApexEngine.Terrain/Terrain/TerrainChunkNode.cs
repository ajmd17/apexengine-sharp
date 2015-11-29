using ApexEngine.Math;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using System;

namespace ApexEngine.Terrain
{
    public abstract class TerrainChunkNode : Node
    {
        protected PhysicsWorld physicsWorld;
        protected int x, z;
        protected Vector3f scale;
        protected int chunkSize;
        public TerrainMesh hm;
        public TerrainChunkNode[] neighbors = new TerrainChunkNode[4];
        protected TerrainComponent parentT;

        public int X
        {
            get { return x; }
        }

        public int Z
        {
            get { return z; }
        }

        public Vector3f Scale
        {
            get { return scale; }
        }

        public int ChunkSize
        {
            get { return chunkSize; }
        }

        public TerrainChunkNode(PhysicsWorld physicsWorld, TerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, TerrainChunkNode[] neighbors) : base("TerrainChunkNode")
        {
            this.physicsWorld = physicsWorld;
            this.x = x;
            this.z = z;
            this.neighbors = neighbors;
            this.scale = scale;
            this.chunkSize = chunkSize;
            this.parentT = parentT;
        }

        public abstract void Create();

        public abstract void AddPhysics();

        public abstract void RemovePhysics();

        public virtual float GetHeight(Vector3f position)
        {
            if (hm != null)
            {
                try
                {
                    return hm.heights[hm.HeightIndexAt((int)position.x, (int)position.z)] * hm.scale.y;
                }
                catch (Exception ex)
                {
                    return float.NaN;
                }
            }
            return float.NaN;
        }

        public virtual Vector3f GetNormal(Vector3f position)
        {
            return null;
        }
    }
}