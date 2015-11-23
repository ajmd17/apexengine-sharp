using ApexEngine.Math;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;

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

        public TerrainChunkNode(PhysicsWorld physicsWorld) : base("TerrainChunkNode")
        {
            this.physicsWorld = physicsWorld;
        }

        public abstract void Create();

        public abstract void AddPhysics();

        public abstract void RemovePhysics();

        public abstract float GetHeight(Vector3f position);

        public abstract Vector3f GetNormal(Vector3f position);
    }
}