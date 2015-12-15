using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using System.Collections.Generic;
using System.IO;

namespace ApexEngine.Assets.Obj
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
        private static ObjModelLoader instance = new ObjModelLoader();

        public static ObjModelLoader GetInstance()
        {
            return instance;
        }

        protected List<string> names = new List<string>();
        protected List<string> namesMtl = new List<string>();
        protected List<List<ObjIndex>> objIndices = new List<List<ObjIndex>>();
        protected List<Vector3f> positions = new List<Vector3f>();
        protected List<Vector3f> normals = new List<Vector3f>();
        protected List<Vector2f> texCoords = new List<Vector2f>();
        protected List<Material> materials = new List<Material>();
        protected List<Material> mtlOrder = new List<Material>();
        protected bool hasTexCoords = false, hasNormals = false;

        public override void ResetLoader()
        {
            objIndices.Clear();
            hasTexCoords = false;
            hasNormals = false;
            texCoords.Clear();
            positions.Clear();
            normals.Clear();
            names.Clear();
            namesMtl.Clear();
            materials.Clear();
            mtlOrder.Clear();
        }

        private List<ObjIndex> CurrentList()
        {
            if (objIndices.Count == 0)
                NewMesh("child_0");
            return objIndices[objIndices.Count - 1];
        }

        public ObjModelLoader() : base("obj")
        {
        }

        private void NewMesh(string name)
        {
            int counter = 0;

            string nm = name;

            while (names.Contains(nm))
            {
                counter++;
                nm = name + "_" + counter.ToString();
            }
            objIndices.Add(new List<ObjIndex>());
            names.Add((counter == 0 ? name : name + "_" + counter.ToString()));
            namesMtl.Add(name);
        }

        private ObjIndex ParseObjIndex(string token)
        {
            string[] values = token.Split('/');
            ObjIndex res = new ObjIndex();
            if (values.Length > 0)
            {
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
            }
            return res;
        }

        public static string[] RemoveEmptyStrings(string[] data)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != "")
                {
                    result.Add(data[i]);
                }
            }
            string[] res = result.ToArray();
            return res;
        }

        public Material MaterialWithName(string name)
        {
            foreach (Material m in materials)
            {
                if (m.GetName() == name)
                    return m;
            }
            return new Material().SetName(name);
        }

        public override object Load(LoadedAsset asset)
        {
            Node node = new Node();
            string modelName = Path.GetFileNameWithoutExtension(asset.FilePath);
            node.Name = modelName;
            StreamReader reader = new StreamReader(asset.Data);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] tokens = line.Split(' ');
                tokens = RemoveEmptyStrings(tokens);
                if (tokens.Length == 0 || tokens[0] == "#")
                {
                }
                else if (tokens[0] == "v")
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
                else if (tokens[0] == "mtllib")
                {
                    string libLoc = tokens[1];
                    string parentPath = System.IO.Directory.GetParent(asset.FilePath).ToString();
                    string mtlPath = parentPath + "\\" + libLoc;
                    materials = (List<Material>)AssetManager.Load(mtlPath, MtlAssetLoader.GetInstance());
                }
                else if (tokens[0] == "usemtl")
                {
                    string matName = tokens[1];
                    //Material material = MaterialWithName(matName);
                    //mtlOrder.Add(material);
                    NewMesh(matName);
                }
                else if (tokens[0] == "g")
                {
                }
            }
            reader.Close();
            for (int i = 0; i < objIndices.Count; i++)
            {
                List<ObjIndex> c_idx = objIndices[i];
                List<Vertex> vertices = new List<Vertex>();
                for (int j = 0; j < c_idx.Count; j++)
                {
                    Vertex vert = new Vertex(positions[c_idx[j].vertexIdx],
                                             hasTexCoords ? texCoords[c_idx[j].texCoordIdx] : null,
                                             hasNormals ? normals[c_idx[j].normalIdx] : null);
                    vertices.Add(vert);
                }
                Mesh mesh = new Mesh();
                mesh.SetVertices(vertices);

                Geometry geom = new Geometry();
                geom.Name = names[i];
                geom.Mesh = mesh;
                geom.Material = MaterialWithName(namesMtl[i]);
                node.AddChild(geom);
            }
            ResetLoader();
            return node;
        }
    }
}