using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;

namespace ApexEngine.Terrain.ModelTerrain
{
    public class ModelTerrainComponent : TerrainComponent
    {
        private string directory = "";
        private string defaultName = "";
        private string defaultExt = "";

        public ModelTerrainComponent(string directory, string defaultFilename, string defaultExtension)
        {
            this.directory = directory;
            this.defaultName = defaultFilename;
            if (defaultExtension.StartsWith("."))
                this.defaultExt = defaultExtension;
            else
                this.defaultExt = "." + defaultExtension;
        }

        public override TerrainChunkNode CreateNewChunk(PhysicsWorld physicsWorld, TerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, TerrainChunkNode[] neighbors)
        {
            string assetLoc = directory + "\\" + defaultName + "_" + x.ToString() + "_" + z.ToString() + defaultExt;
            GameObject loadedObj = null;
            if (System.IO.File.Exists(assetLoc))
            {
                loadedObj = Assets.AssetManager.LoadModel(assetLoc);
            }
            else
            {
                assetLoc = directory + "\\" + defaultName + "_0_0" + defaultExt;
                if (System.IO.File.Exists(assetLoc))
                {
                    loadedObj = Assets.AssetManager.LoadModel(assetLoc);
                }
            }
            return new ModelTerrainChunkNode(loadedObj, physicsWorld, (ModelTerrainComponent)parentT, x, z, scale, chunkSize, (ModelTerrainChunkNode[])neighbors);
        }

        public override Material GetMaterial()
        {
            return terrainMaterial;
        }

        public override void ApplyTerrainMaterial(TerrainChunkNode chunk)
        {
            List<Geometry> geoms = Rendering.Util.MeshUtil.GatherGeometry(chunk);
            foreach (Geometry g in geoms)
                g.Material = GetMaterial();
        }

        public override void Init()
        {
        }

        public override void OnAddChunk(TerrainChunkNode chunk)
        {
        }

        public override void OnRemoveChunk(TerrainChunkNode chunk)
        {
        }
    }
}
