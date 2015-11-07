using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ApexEngine.Math;
using ApexEngine.Scene;
using ApexEngine.Rendering;
namespace ApexEngine.Assets.Apx
{
    public class ApxModelLoader : AssetLoader
    {
        List<Node> nodes = new List<Node>();
        List<Geometry> geoms = new List<Geometry>();
        List<Mesh> meshes = new List<Mesh>();

        List<List<Vector3f>> positions = new List<List<Vector3f>>();
        List<List<Vector3f>> normals = new List<List<Vector3f>>();
        List<List<Vector2f>> texcoords0 = new List<List<Vector2f>>();
        List<List<Vector2f>> texcoords1 = new List<List<Vector2f>>();
        List<List<Vertex>> vertices = new List<List<Vertex>>();
        List<List<int>> faces = new List<List<int>>();

        bool node = false, geom = false;

        Node lastNode = null;
        private void EndModel()
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                List<Vertex> cVerts = vertices[i];
                List<int> cFaces = faces[i];

                List<Vector3f> cPos = positions[i];
                List<Vector3f> cNorm = normals[i];
                List<Vector2f> tc0 = texcoords0[i];
                List<Vector2f> tc1 = texcoords1[i];

                Mesh mesh = meshes[i];
                int stride = 3;

                if (tc1.Count > 0)
                    stride++;
                for (int j = 0; j < cFaces.Count; j+= stride)
                {
                    Vertex v = new Vertex(cPos[cFaces[j]], tc0[cFaces[j + 2]], cNorm[cFaces[j + 1]]);
                    if (tc1.Count > 0)
                    {
                        mesh.GetAttributes().SetAttribute(VertexAttributes.TEXCOORDS1);
                        v.SetTexCoord1(tc1[cFaces[i + 3]]);
                    }
                    cVerts.Add(v);
                }
                List<int> indexData = new List<int>();
                for (int j = 0; j < cFaces.Count/3; j++)
                {
                    indexData.Add(j);
                }
                mesh.SetVertices(cVerts, indexData);
                if (geoms.Count > 0)
                {
                    Geometry parent = geoms[i];
                    parent.SetMesh(mesh);
                }
            }
        }
        public override object Load(string filePath)
        {
            XmlReader xmlReader = XmlReader.Create(filePath);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == ApxExporter.TOKEN_NODE)
                    {
                        node = true;
                        geom = false;
                        string name = xmlReader.GetAttribute(ApxExporter.TOKEN_NAME);
                        Node n = new Node(name);
                        if (lastNode != null)
                            lastNode.AddChild(n);
                        lastNode = n;
                        nodes.Add(n);
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_GEOMETRY)
                    {
                        node = false;
                        geom = true;
                        string name = xmlReader.GetAttribute(ApxExporter.TOKEN_NAME);
                        Geometry g = new Geometry();
                        g.SetName(name);
                        if (lastNode != null)
                            lastNode.AddChild(g);
                        geoms.Add(g);
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_MESH)
                    {
                        meshes.Add(new Mesh());
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_VERTICES)
                    {
                        List<Vertex> newVList = new List<Vertex>();
                        vertices.Add(newVList);

                        List<Vector3f> newPList = new List<Vector3f>();
                        positions.Add(newPList);

                        List<Vector3f> newNList = new List<Vector3f>();
                        normals.Add(newNList);

                        List<Vector2f> newT0List = new List<Vector2f>();
                        texcoords0.Add(newT0List);

                        List<Vector2f> newT1List = new List<Vector2f>();
                        texcoords1.Add(newT1List);
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_POSITION)
                    {
                        List<Vector3f> pos = positions[positions.Count - 1];

                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        float z = float.Parse(xmlReader.GetAttribute("z"));

                        Vector3f position = new Vector3f(x, y, z);
                        pos.Add(position);
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_NORMAL)
                    {
                        List<Vector3f> nor = normals[normals.Count - 1];

                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        float z = float.Parse(xmlReader.GetAttribute("z"));

                        Vector3f normal = new Vector3f(x, y, z);
                        nor.Add(normal);
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_TEXCOORD0)
                    {
                        List<Vector2f> tc0 = texcoords0[texcoords0.Count - 1];

                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                       
                        Vector2f tc = new Vector2f(x, y);
                        tc0.Add(tc);
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_TEXCOORD1)
                    {
                        List<Vector2f> tc1 = texcoords1[texcoords1.Count - 1];

                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));

                        Vector2f tc = new Vector2f(x, y);
                        tc1.Add(tc);
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_FACES)
                    {
                        faces.Add(new List<int>());
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_FACE)
                    {
                        List<int> fList = faces[faces.Count - 1];
                        for (int i =  0; i < 3; i++)
                        {
                            string val = xmlReader.GetAttribute("i" + i.ToString());
                            if (val != "")
                            {
                                string[] tokens = val.Split('/');
                                for (int j = 0; j < tokens.Length; j++)
                                {
                                    fList.Add(int.Parse(tokens[j]));
                                }
                            }
                        }
                    }
                } // start element
                else if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    if (xmlReader.Name == ApxExporter.TOKEN_NODE)
                    {
                        if (lastNode != null)
                        {
                            if (lastNode.GetParent() != null)
                            {
                                lastNode = lastNode.GetParent();
                            }
                            else
                            {
                                lastNode = null;
                            }
                        }
                        node = false;
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_GEOMETRY)
                    {
                        geom = false;
                    }
                    else if (xmlReader.Name == ApxExporter.TOKEN_MODEL)
                    {
                        // end of model, load in meshes
                       EndModel();
                    }
                } // end element
            }
            return nodes[0];
        }
    }
}
