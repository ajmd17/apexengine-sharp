using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Math;
namespace ApexEngine.Assets.ModelLoaders
{
    public class ObjIndex
    {
        public int vertexIdx, normalIdx, texCoordIdx;
        public ObjIndex(int v_idx, int n_idx, int t_idx)
        {
            vertexIdx = v_idx;
            normalIdx = n_idx;
            texCoordIdx = t_idx;
        }
        public ObjIndex()
        {

        }
    }
    public class ObjModelLoader : AssetLoader
    {
        protected List<List<ObjIndex>> objIndices = new List<List<ObjIndex>>();
        protected List<Vector3f> positions = new List<Vector3f>();
        protected List<Vector3f> normals = new List<Vector3f>();
        protected List<Vector2f> texCoords = new List<Vector2f>();
        protected bool hasTexCoords = false, hasNormals = false;
        private List<ObjIndex> CurrentList()
        {
            if (objIndices.Count == 0)
                NewMesh();
            return objIndices[objIndices.Count - 1];
        }
        private void NewMesh()
        {
            objIndices.Add(new List<ObjIndex>());
        }
        private ObjIndex ParseObjIndex(string token)
        {
            string[] values = token.Split('/');

            ObjIndex res = new ObjIndex();
            res.vertexIdx = int.Parse(values[0]) - 1;
            if (values.Length > 1)
            {
                if (values[1] != "")
                {
                    hasTexCoords = true;
                    res.texCoordIdx = int.Parse(values[1]) - 1;
                }
                if (values.Length > 2)
                {
                    hasNormals = true;
                    res.normalIdx = int.Parse(values[2]) - 1;
                }
            }
            return res;
        }
        public override object Load(string filePath)
        {
            Node node = new Node();
            StreamReader reader = File.OpenText(filePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] tokens = line.Split(' ');
                if (tokens[0] == "v")
                {
                    positions.Add(new Vector3f(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                }
                else if (tokens[0] == "vn")
                {
                    normals.Add(new Vector3f(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                }
                else if (tokens[0] == "vt")
                {
                    texCoords.Add(new Vector2f(float.Parse(tokens[1]), float.Parse(tokens[2])));
                }
                else if (tokens[0] == "f")
                {
                    List<ObjIndex> idx = CurrentList();
                    for (int i = 0; i < tokens.Length - 3; i++)
                    {
                        idx.Add(ParseObjIndex(tokens[1]));
                        idx.Add(ParseObjIndex(tokens[2 + i]));
                        idx.Add(ParseObjIndex(tokens[3 + i]));
                    }
                }
            }
            for (int i = 0; i < objIndices.Count; i++)
            {
                List<ObjIndex> c_idx = objIndices[i];
                List<Vertex> vertices = new List<Vertex>();
                for (int j = 0; j < c_idx.Count; j++)
                {
                    Vertex vert = new Vertex(positions[c_idx[j].vertexIdx],
                                             hasTexCoords ? texCoords[c_idx[j].texCoordIdx] : null,
                                             hasNormals ? normals[c_idx[j].normalIdx] : null);
                    vertices.Add(vert);//, 
                                          //  hasTexCoords ? texCoords[c_idx[j].texCoordIdx] : null, 
                                          //  hasNormals ? normals[c_idx[j].normalIdx] : null));
                }
                Mesh mesh = new Mesh();
                mesh.SetVertices(vertices);
               
                Geometry geom = new Geometry();
                geom.SetName("child_" + i);
                geom.SetMesh(mesh);
                node.AddChild(geom);
            }
            return node;
        }
    }
}
