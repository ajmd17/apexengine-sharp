using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Math;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using ApexEngine.Rendering;
using ApexEngine.Scene.Components;
using ApexEngine.Terrain.Ecosystem;
using System.Drawing;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainChunkNode : TerrainChunkNode
    {
        private static Shader terrainShader;
        private bool biomesEnabled = false;

        static SimplexTerrainChunkNode()
        {
            terrainShader = ShaderManager.GetShader(typeof(TerrainShader), new ShaderProperties().SetProperty("DEFAULT", true));
        }
        public SimplexTerrainChunkNode(PhysicsWorld physicsWorld, SimplexTerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, SimplexTerrainChunkNode[] neighbors)
           : base(physicsWorld, parentT, x, z, scale, chunkSize, neighbors)
        {
            this.biomesEnabled = false;
        }

        public SimplexTerrainChunkNode(PhysicsWorld physicsWorld, SimplexTerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, SimplexTerrainChunkNode[] neighbors, bool biomesEnabled) 
            : base(physicsWorld, parentT, x, z, scale, chunkSize, neighbors)
        {
            this.biomesEnabled = biomesEnabled;
        }
        
        public override void AddPhysics()
        {
            if (physicsWorld != null)
                physicsWorld.AddObject(this, 0);
        }

        public override void RemovePhysics()
        {
            if (physicsWorld != null)
            {
                physicsWorld.RemoveObject(this);
            }
        }

        public Biome GetBiome(Vector3f position)
        {
            if (!biomesEnabled)
                throw new Exception("Biomes not enabled");
            if (hm != null)
            {
                try
                {
                    return ((SimplexTerrainMesh)hm).biomes[hm.HeightIndexAt((int)position.x, (int)position.z)];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            return null;
        }

        public override void Create()
        {
            this.Name = "simplexterrain_" + x.ToString() + "_" + z.ToString();
            hm = new SimplexTerrainMesh((SimplexTerrainComponent)parentT, x, z, scale, chunkSize, this.biomesEnabled);
            Geometry geom = new Geometry();
            geom.Mesh = hm;
            geom.SetShader(terrainShader);
            AddChild(geom);

            // create alpha map
          
          /*      List<byte> bytes = new List<byte>(); // this list should be filled with values
                int width = this.chunkSize;
                int height = this.chunkSize;
                int bpp = 24;
                SimplexTerrainMesh hm0 = (SimplexTerrainMesh)hm;


                Bitmap bmp = new Bitmap(width, height);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color col = Color.FromArgb((int)((hm0.biomes[hm0.HeightIndexAt(x, y)].AverageTemperature * 0.5f + 0.5f) * 255), 0, 0);
                     //   Console.WriteLine();
                        bmp.SetPixel(x, y, col);

                    }
                }
                

                bmp.Dispose();
            */

        }
    }
}
