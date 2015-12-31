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

                    float terrainHeight = (float)parent.getNoise(_x, _y);

                    float biomeHeight = 1f, temperature = 1f;

                    int heightIndex = HeightIndexAt(yy, xx);

                    if (this.generateBiomes)
                    {
                        biomeHeight = (float)(parent.getNoise(((double)_y) * 0.1, ((double)_x) * 0.1));

                        temperature = (float)(parent.getNoise(((double)_y), terrainHeight, ((double)_x)));

                        Biome biome = new Biome();

                        biome.AverageTemperature = temperature;

                        if (System.Math.Abs(biomeHeight) < 0.3f)
                        {
                            biome.Topography = Biome.BiomeTopography.Plains;
                        }
                        else
                        {
                            biome.Topography = Biome.BiomeTopography.Hills;
                        }

                        biomeHeight *= 6 * (float)System.Math.Abs(biomeHeight);
                        biomes[heightIndex] = biome;
                    }

                    heights[heightIndex] = terrainHeight * biomeHeight * 25f;
                }
            }
            return heights;
        }
    }
}