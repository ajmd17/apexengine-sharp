using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene.Physics;
using ApexEngine.Terrain.Ecosystem;
using ApexEngine.Util;
using LibNoise;
using System;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainComponent : TerrainComponent, BiomeTerrain
    {
        public float[] rHeights;
        private OpenSimplexNoise[] octaves;
        private double[] frequencys;
        private double[] amplitudes;
        private Vector2f[] tmpVec = new Vector2f[8];
        private bool biomesEnabled = false;

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
                octaves[i] = new OpenSimplexNoise(666);
                frequencys[i] = (float)System.Math.Pow(2, i);
                amplitudes[i] = (float)System.Math.Pow(0.5f, octaves.Length - i);
            }

            //Texture splat = AssetManager.LoadTexture("C:\\Users\\User\\Desktop\\test_0_1.png");
///terrainMaterial.SetValue(TerrainMaterial.TEXTURE_SPLAT, splat);

            //terrainMaterial.SetValue(Material.COLOR_DIFFUSE, new Vector4f(0.2f, 0.7f, 0.2f, 1.0f));
            Texture grass = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass4.jpg");
            Texture grass_nrm = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\grass_NRM.jpg");
            Texture dirt = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt.jpg");
            Texture dirt_nrm = AssetManager.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\dirt_NRM.jpg");
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE0, grass);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL0, grass_nrm);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_DIFFUSE_SLOPE, dirt);
            terrainMaterial.SetValue(TerrainMaterial.TEXTURE_NORMAL_SLOPE, dirt_nrm);
            terrainMaterial.SetValue(Material.MATERIAL_CASTSHADOWS, false);
            terrainMaterial.SetValue(Material.SHININESS, 0.1f);
            terrainMaterial.SetValue(Material.ROUGHNESS, 0.08f);
        }

        public bool BiomesEnabled
        {
            get { return biomesEnabled; }
            set { biomesEnabled = value; }
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

        public double getNoise(double x, double y)
        {
            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
            }

            return result;
        }

        public double getNoise(double x, double y, double z)
        {
            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i], z / frequencys[i]) * amplitudes[i];
            }

            return result;
        }

        public Biome GetBiome(Vector3f worldPosition)
        {
            Vector3f chunkSpace1 = WorldToChunkSpace(worldPosition);
            HeightInfo closest = HmWithCoords((int)chunkSpace1.x, (int)chunkSpace1.z);

            if (closest != null)
            {
                chunkSpace1.x -= closest.position.x;
                chunkSpace1.z -= closest.position.y;
                chunkSpace1.MultiplyStore((chunkSize - 1));

                return ((SimplexTerrainChunkNode)closest.chunk).GetBiome(chunkSpace1);
            }
            return null;
        }

        public override void ApplyTerrainMaterial(TerrainChunkNode chunk)
        {
            chunk.GetChildGeom(0).Material = GetMaterial().Clone();
        }

        public override Material GetMaterial()
        {
            return terrainMaterial;
        }

        public override void Init()
        {
        }

        public override TerrainChunkNode CreateNewChunk(PhysicsWorld physicsWorld, TerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, TerrainChunkNode[] neighbors)
        {
            return new SimplexTerrainChunkNode(physicsWorld, (SimplexTerrainComponent)parentT, x, z, scale, chunkSize, (SimplexTerrainChunkNode[])neighbors, biomesEnabled);
        }
    }
}