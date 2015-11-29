using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Util;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainComponent : TerrainComponent
    {
        public float[] rHeights;
        OpenSimplexNoise[] octaves;
        double[] frequencys;
        double[] amplitudes;
        private Vector2f[] tmpVec = new Vector2f[8];

        public SimplexTerrainComponent() : this(null)
        {
            
        }

        public SimplexTerrainComponent(PhysicsWorld physicsWorld) : base(physicsWorld)
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
                octaves[i] = new OpenSimplexNoise(234);
                frequencys[i] = (float)System.Math.Pow(2, i);
                amplitudes[i] = (float)System.Math.Pow(0.5f, octaves.Length - i);
            }
            //terrainMaterial.SetValue(Material.COLOR_DIFFUSE, new Vector4f(0.2f, 0.7f, 0.2f, 1.0f));
            Texture grass = Texture.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass2.jpg");
            Texture grass_nrm = Texture.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass_NRM.jpg");
            Texture dirt = Texture.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt.jpg");
            Texture dirt_nrm = Texture.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt_NRM.jpg");
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE0, grass);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL0, grass_nrm);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE_SLOPE, dirt);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL_SLOPE, dirt_nrm);
            terrainMaterial.SetValue(Material.MATERIAL_CASTSHADOWS, false);
           // terrainMaterial.SetValue(Material.SHININESS, 0f);
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

        public override void ApplyTerrainMaterial(TerrainChunkNode chunk)
        {
            chunk.GetChildGeom(0).Material = GetMaterial();
        }

        public override Material GetMaterial()
        {
            return terrainMaterial;
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

        public override TerrainChunkNode CreateNewChunk(PhysicsWorld physicsWorld, TerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, TerrainChunkNode[] neighbors)
        {
            return new SimplexTerrainChunkNode(physicsWorld, (SimplexTerrainComponent)parentT, x, z, scale, chunkSize, (SimplexTerrainChunkNode[])neighbors);
        }

    }
}
