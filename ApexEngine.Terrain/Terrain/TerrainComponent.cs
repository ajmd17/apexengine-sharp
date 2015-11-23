using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene.Components;
using ApexEngine.Scene.Physics;

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

        protected PhysicsWorld physicsWorld;

        public TerrainComponent()
        {
        }

        public TerrainComponent(PhysicsWorld physicsWorld)
        {
            this.physicsWorld = physicsWorld;
        }

        public abstract void OnAddChunk(TerrainChunkNode chunk);

        public abstract void OnRemoveChunk(TerrainChunkNode chunk);

        public abstract float GetHeight(Vector3f worldPosition);

        public abstract Vector3f GetNormal(Vector3f worldPosition);

        public abstract Material GetMaterial();
    }
}