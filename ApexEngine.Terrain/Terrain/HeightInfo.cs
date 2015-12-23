using ApexEngine.Math;
using ApexEngine.Scene.Components;

namespace ApexEngine.Terrain
{
    public class HeightInfo
    {
        private TerrainComponent component;
        public Vector2f midpoint = new Vector2f();
        public Vector2f position = new Vector2f();
        public TerrainChunkNode chunk;
        public TerrainComponent.PageState pageState = TerrainComponent.PageState.Unloaded;
        public Vector2f[] neighbors = new Vector2f[8];
        private bool hasPhysics = false;

        public HeightInfo(TerrainComponent component, Vector2f pos, TerrainChunkNode chunk)
        {
            this.position = pos;
            this.chunk = chunk;
            this.component = component;
        }

        private float unloadTime = 0f;
        private float physicsTime = 0;

        public void UpdateChunk()
        {
            if (pageState != TerrainComponent.PageState.Unloaded)
            {
            }
            else if (pageState == TerrainComponent.PageState.Unloaded)
            {
                if (hasPhysics)
                {
                    chunk.RemovePhysics();
                    for (int i = 0; i < chunk.Controllers.Count; i++)
                    {
                        Controller ctrlr = chunk.Controllers[i];
                        ctrlr.Destroy();
                        chunk.RemoveController(ctrlr);
                        ctrlr = null;
                    }
                    hasPhysics = false;
                }
                component.OnRemoveChunk(chunk);
            }
            if (pageState == TerrainComponent.PageState.Loaded)
            {
                if (!hasPhysics)
                {
                    chunk.AddPhysics();
                    hasPhysics = true;
                    component.OnAddChunk(chunk);
                }
                unloadTime = 0f;
            }
            else if (pageState == TerrainComponent.PageState.Unloading)
            {
                if (unloadTime > 5f)
                {
                    pageState = TerrainComponent.PageState.Unloaded;
                }
                unloadTime += component.Environment.TimePerFrame * 10f;
            }
        }
    }
}