using System.Collections.Generic;
using ApexEngine.Math;

namespace ApexEngine.Rendering.Util
{
    public class MeshFactory
    {
        public static Mesh CreateQuad()
        {
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(new Vertex(new Vector3f(-1f, -1f, 0), new Vector2f(0, 0), new Vector3f(0, 0, 1)));
            vertices.Add(new Vertex(new Vector3f(1, -1f, 0), new Vector2f(1f, 0), new Vector3f(0, 0, 1)));
            vertices.Add(new Vertex(new Vector3f(1f, 1f, 0), new Vector2f(1f, 1f), new Vector3f(0, 0, 1)));
            vertices.Add(new Vertex(new Vector3f(-1f, 1f, 0), new Vector2f(0, 1f), new Vector3f(0, 0, 1)));
            mesh.SetVertices(vertices);
            mesh.PrimitiveType = Mesh.PrimitiveTypes.TriangleFan;
            return mesh;
        }

        public static Mesh CreateCube(Vector3f min, Vector3f max)
        {
            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, max.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, max.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(max.x, min.y, min.z)));
            vertices.Add(new Vertex(new Vector3f(min.x, max.y, min.z)));


            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            return mesh;
        }
    }
}
