using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene.Physics;
using ApexEngine.Terrain.Ecosystem;
using ApexEngine.Util;
using LibNoise;
using System;
using LibNoise.Modifiers;

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


        private WorleyNoise wn;

        //libnoise
        IModule module;
        Perlin terrainTopography;
        Voronoi voronoi;


        public SimplexTerrainComponent() : this(null)
        {
        }

        public SimplexTerrainComponent(PhysicsWorld physicsWorld) : base(physicsWorld)
        {

            wn = new WorleyNoise();


           module = new FastRidgedMultifractal();
            ((FastRidgedMultifractal)module).Frequency = 0.03;
            ((FastRidgedMultifractal)module).NoiseQuality = NoiseQuality.Low;
            ((FastRidgedMultifractal)module).Seed = 12345;
            ((FastRidgedMultifractal)module).OctaveCount = 11;
            ((FastRidgedMultifractal)module).Lacunarity = 2.0;

            voronoi = new Voronoi();
            voronoi.Frequency = 0.05;
            voronoi.Seed = 12345;

         /*
            NoiseQuality quality = NoiseQuality.Low;
            int seed = 12345;
            double frequency = 0.03;
            double lacunarity = 2.0;
            double persistence = 0.5;
            int numOctaves = 8;

            FastBillow fastbillow = new FastBillow();
            fastbillow.Frequency = frequency;
            fastbillow.NoiseQuality = quality;
            fastbillow.Seed = seed;
            fastbillow.OctaveCount = numOctaves;
            fastbillow.Lacunarity = lacunarity;
            fastbillow.Persistence = persistence;


            Perlin perlin = new Perlin();
            perlin.Seed = seed;
            perlin.NoiseQuality = NoiseQuality.Low;
            perlin.Frequency = frequency / 10.0;
            perlin.Lacunarity = lacunarity;
            perlin.OctaveCount = numOctaves;
            perlin.Persistence = persistence;

            FastRidgedMultifractal fastridged = new FastRidgedMultifractal();
            fastridged.Frequency = frequency / 2.0;
            fastridged.NoiseQuality = quality;
            fastridged.Seed = seed;
            fastridged.OctaveCount = numOctaves;
            fastridged.Lacunarity = lacunarity;

            FastNoise fastperlin = new FastNoise();
            fastperlin.Frequency = frequency / 10.0;
            fastperlin.NoiseQuality = quality;
            fastperlin.Seed = seed;
            fastperlin.OctaveCount = numOctaves;
            fastperlin.Lacunarity = lacunarity;
            fastperlin.Persistence = persistence;

            Select fastselector = new Select(fastperlin, fastridged, perlin);
            fastselector.SetBounds(0, 1000);
            fastselector.EdgeFalloff = 0.3;

          

            module = perlin;*/
            




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
            terrainMaterial.SetValue(Material.SHININESS, 0.0f);
            terrainMaterial.SetValue(Material.ROUGHNESS, 0.08f);
        }

        public bool BiomesEnabled
        {
            get { return biomesEnabled; }
            set { biomesEnabled = value; }
        }

        public double GetTopography(double x, double y)
        {
            return (terrainTopography.GetValue(x, y, 10) + 1) / 2.0;
        }

        public float GetWorleyNoise(float x, float y)
        {
            return wn.Get2D(x, y);
        }


        public double GetVoronoiNoise(double x, double y)
        {
            /*  double result = 0;

              for (int i = 0; i < octaves.Length; i++)
              {
                  result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
              }

              return result;*/
            return (voronoi.GetValue(x, y, 10) + 1) / 2.0;
        }


        public double getNoise(int x, int y)
        {
          /*  double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
            }

            return result;*/
            return (module.GetValue(x, y, 10)+1)/2.0;
        }

        public double getNoise(double x, double y)
        {
          /*  double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
            }

            return result;*/

            return (module.GetValue(x, y, 10) + 1) / 2.0;
        }

        public double getNoise(double x, double y, double z)
        {
           /* double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i], z / frequencys[i]) * amplitudes[i];
            }

            return result;*/

            return (module.GetValue(x, y, z) + 1) / 2.0;
        }

        public double getSimplexNoise(double x, double y)
        {
              double result = 0;

              for (int i = 0; i < octaves.Length; i++)
              {
                  result = result + octaves[i].Evaluate(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
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