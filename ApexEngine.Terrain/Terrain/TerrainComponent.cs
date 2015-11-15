using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine;
using ApexEngine.Scene;
using ApexEngine.Scene.Components;
using ApexEngine.Math;
using ApexEngine.Rendering;
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
        public abstract void OnAddChunk(TerrainChunkNode chunk);
        public abstract void OnRemoveChunk(TerrainChunkNode chunk);
        public abstract float GetHeight(Vector3f worldPosition);
        public abstract Vector3f GetNormal(Vector3f worldPosition);
        public abstract Material GetMaterial();

    }
}
