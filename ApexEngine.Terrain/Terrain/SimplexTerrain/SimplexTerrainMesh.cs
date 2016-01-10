using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Terrain.Ecosystem;
using System;
using System.Collections.Generic;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainMesh : TerrainMesh
    {
        protected int x, z;
        private int chunkSize;
        private SimplexTerrainComponent parent;
        private bool generateBiomes = false;
        public Biome[] biomes;


        public SimplexTerrainMesh(SimplexTerrainComponent parent, int xstart, int zstart, Vector3f scale, int chunkSize) :
            this(parent, xstart, zstart, scale, chunkSize, false)
        {
        }

        public SimplexTerrainMesh(SimplexTerrainComponent parent, int xstart, int zstart, Vector3f scale, int chunkSize, bool generateBiomes) : base()
        {
            try
            {
                this.generateBiomes = generateBiomes;
                this.parent = parent;
                this.x = xstart;
                this.z = zstart;
                this.chunkSize = chunkSize;
                this.scale = scale;
                this.GetHeights(this.x, this.z);

                this.RebuildTerrainMesh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            heights = null;
            biomes = null;
        }

        public override int HeightIndexAt(int x, int z)
        {
            int size = (chunkSize);
            return (((x + size) % size) + ((z + size) % size) * size);
        }

        public Biome GetBiomeAt(int x, int z)
        {
            int biomeIndex = HeightIndexAt(x, z);
            return biomes[biomeIndex];
        }

        public float[] GetHeights(int xstart, int zstart)
        {
            int size = chunkSize;

            height = size - 1;
            width = size - 1;

            heights = new float[size * size];

            biomes = new Biome[heights.Length];

            vertexArray = new Vertex[heights.Length];
            indexArray = new int[width * height * 6];

            for (int xx = 0; xx < size; xx++)
            {
                for (int yy = 0; yy < size; yy++)
                {
                    float _x = yy + ((int)x * (size - 1));
                    float _y = xx + ((int)z * (size - 1));


                    float biomeHeight = 1f, temperature = 1f;

                    int heightIndex = HeightIndexAt(yy, xx);

                    float terrainHeight;

                    if (this.generateBiomes)
                    {

                        biomeHeight = (float)(parent.getSimplexNoise(((double)_y*0.4), ((double)_x*0.4)));



                        temperature = (float)(parent.getSimplexNoise(((double)_y), ((double)_x)));




                        terrainHeight = (float)parent.getSimplexNoise(_x, _y);

                        float mountainHeight = (float)parent.getNoise(_x*0.1f, _y*0.1f);



                        Biome biome = new Biome();

                        biome.AverageTemperature = temperature;

                        if (biomeHeight < 0.3f)
                        {
                            biome.Topography = Biome.BiomeTopography.Plains;
                        }
                        else
                        {
                            biome.Topography = Biome.BiomeTopography.Hills;
                        }

                        biomeHeight = (float)System.Math.Abs(biomeHeight) * 2;
                        biomeHeight *= biomeHeight;

                        biomes[heightIndex] = biome;



                        heights[heightIndex] = MathUtil.Lerp(terrainHeight*2, mountainHeight*70f, MathUtil.Clamp(biomeHeight, 0.0f, 1.0f));
                    }
                    else
                    {
                        terrainHeight = (float)parent.getNoise(_x, _y);
                        heights[heightIndex] = terrainHeight * 25f;
                    }

                }
            }
            return heights;
        }
    }
}