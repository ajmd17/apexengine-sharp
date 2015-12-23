using ApexEngine.Math;
using ApexEngine.Rendering;
using System;
using System.Collections.Generic;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainMesh : TerrainMesh
    {
        protected int x, z;
        private int chunkSize;
        private SimplexTerrainComponent parent;

        public SimplexTerrainMesh(SimplexTerrainComponent parent, int xstart, int zstart, Vector3f scale, int chunkSize) : base()
        {
            try
            {
                this.parent = parent;
                this.x = xstart;
                this.z = zstart;
                this.chunkSize = chunkSize;
                this.scale = scale;
                this.GetHeights(this.x, this.z);
                this.BuildVertices();
                this.BuildIndices();
                this.CalcNormals(indexArray, vertexArray);
                List<Vertex> newVertexArray = new List<Vertex>();
                List<int> newIndexArray = new List<int>();
                for (int i = 0; i < indexArray.Length; i++)
                    newIndexArray.Add(indexArray[i]);
                for (int i = 0; i < vertexArray.Length; i++)
                    newVertexArray.Add(vertexArray[i]);
                this.SetVertices(newVertexArray, newIndexArray);

                indexArray = null;
                vertexArray = null;
                this.heights = null;
        //        newVertexArray.Clear();
        //        newIndexArray.Clear()
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public override int HeightIndexAt(int x, int z)
        {
            int size = (chunkSize);
            return (((x + size) % size) + ((z + size) % size) * size);
        }

        public float[] GetHeights(int xstart, int zstart)
        {
            int size = chunkSize;

            height = size - 1;
            width = size - 1;

            heights = new float[size * size];
            vertexArray = new Vertex[heights.Length];
            indexArray = new int[width * height * 6];

            for (int xx = 0; xx < size; xx++)
            {
                for (int yy = 0; yy < size; yy++)
                {
                    heights[HeightIndexAt(yy, xx)] = (float)parent.getNoise(yy + ((int)x * (size - 1)), xx + ((int)z * (size - 1))) * 25f;//this.scale.y;
                }
            }
            return heights;
        }
    }
}