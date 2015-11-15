using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Rendering;
using ApexEngine.Math;
namespace ApexEngine.Terrain
{
    public abstract class TerrainMesh : Mesh
    {
        protected Vertex[] vertexArray;
        protected int[] indexArray;
        public float[] heights;
        public int width, height;
        public Vector3f scale;
        public TerrainMesh() : base()
        {
        }
        public abstract int HeightIndexAt(int x, int z);
        protected void AddNormal(int vertIndex, Vertex[] verts, float x, float y, float z)
        {
            int i = vertIndex;

            verts[i].GetNormal().x += x;
            verts[i].GetNormal().y += y;
            verts[i].GetNormal().z += z;
        }
        protected void NormalizeNormal(int vertIndex, Vertex[] verts)
        {

            int i = vertIndex;

            float x = verts[i].GetNormal().x;
            float y = verts[i].GetNormal().y;
            float z = verts[i].GetNormal().z;

            float num2 = ((x * x) + (y * y)) + (z * z);
            float num = 1f / (float)System.Math.Sqrt(num2);
            x *= num;
            y *= num;
            z *= num;

            verts[i].GetNormal().x = x;
            verts[i].GetNormal().y = y;
            verts[i].GetNormal().z = z;
        }
        protected void CalcNormals(int[] idc, Vertex[] verts)
        {

            for (int i = 0; i < idc.Length; i += 3)
            {
                int i1 = idc[i];
                int i2 = idc[i + 1];
                int i3 = idc[i + 2];

                // p1
                float x1 = verts[i1].GetPosition().x;
                float y1 = verts[i1].GetPosition().y;
                float z1 = verts[i1].GetPosition().z;

                // p2
                float x2 = verts[i2].GetPosition().x;
                float y2 = verts[i2].GetPosition().y;
                float z2 = verts[i2].GetPosition().z;

                // p3
                float x3 = verts[i3].GetPosition().x;
                float y3 = verts[i3].GetPosition().y;
                float z3 = verts[i3].GetPosition().z;

                // u = p3 - p1
                float ux = x3 - x1;
                float uy = y3 - y1;
                float uz = z3 - z1;

                // v = p2 - p1
                float vx = x2 - x1;
                float vy = y2 - y1;
                float vz = z2 - z1;

                // n = cross(v, u)
                float nx = (vy * uz) - (vz * uy);
                float ny = (vz * ux) - (vx * uz);
                float nz = (vx * uy) - (vy * ux);

                // normalize(n)
                float num2 = ((nx * nx) + (ny * ny)) + (nz * nz);
                float num = 1f / (float)System.Math.Sqrt(num2);
                nx *= num;
                ny *= num;
                nz *= num;

                AddNormal(idc[i], verts, nx, ny, nz);
                AddNormal(idc[i + 1], verts, nx, ny, nz);
                AddNormal(idc[i + 2], verts, nx, ny, nz);
            }

            for (int i = 0; i < (verts.Length); i++)
            {
                NormalizeNormal(i, verts);
            }
        }
        public void BuildVertices()
        {
            int heightPitch = height + 1;
            int widthPitch = width + 1;

            int idx = 0;
            int hIdx = 0;
            // int strength = 10; // multiplier for height map
            for (int z = 0; z < heightPitch; z++)
            {
                for (int x = 0; x < widthPitch; x++)
                {

                    // POSITION
                    vertexArray[idx++] = new Vertex(new Vector3f(scale.x * x, heights[hIdx++] * scale.y, scale.z * z), 
                                                    new Vector2f((float)-x / (float)widthPitch, (float)-z / (float)heightPitch),
                                                    new Vector3f());
                }
            }
        }
        protected void BuildIndices()
        {
            int idx = 0;
            short pitch = (short)(width + 1);
            short i1 = 0;
            short i2 = 1;
            short i3 = (short)(1 + pitch);
            short i4 = pitch;

            short row = 0;

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    indexArray[idx++] = i1;
                    indexArray[idx++] = i3;
                    indexArray[idx++] = i2;

                    indexArray[idx++] = i3;
                    indexArray[idx++] = i1;
                    indexArray[idx++] = i4;

                    i1++;
                    i2++;
                    i3++;
                    i4++;
                }

                row += pitch;
                i1 = row;
                i2 = (short)(row + 1);
                i3 = (short)(i2 + pitch);
                i4 = (short)(row + pitch);
            }
        }
    }
}
