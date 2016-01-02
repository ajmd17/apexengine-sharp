using ApexEngine.Assets.Util;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Animation;
using ApexEngine.Scene;
using System.Collections.Generic;
using System.Xml;

namespace ApexEngine.Assets.OgreXml
{
    public class Submesh
    {
        public List<int> faces = new List<int>();
        public List<Vertex> vertices = new List<Vertex>();
    }

    public class OgreXmlModelLoader : AssetLoader
    {
        private static OgreXmlModelLoader instance = new OgreXmlModelLoader();

        public static OgreXmlModelLoader GetInstance()
        {
            return instance;
        }

        public List<Vector3f> positions = new List<Vector3f>();
        public List<Vector3f> normals = new List<Vector3f>();
        public List<Vector2f> texCoords = new List<Vector2f>();
        public List<int> faces = new List<int>();
        public bool useSubmeshes = false;
        public Dictionary<int, BoneAssign[]> boneAssigns = new Dictionary<int, BoneAssign[]>();
        public List<Submesh> subMeshes = new List<Submesh>();
        public Skeleton skeleton = new Skeleton();

        public OgreXmlModelLoader() : base("mesh.xml")
        {
        }

        public override void ResetLoader()
        {
            positions.Clear();
            normals.Clear();
            texCoords.Clear();
            faces.Clear();
            useSubmeshes = false;
            boneAssigns.Clear();
            subMeshes.Clear();
            skeleton = new Skeleton();
        }

        private Submesh CurrentSubmesh()
        {
            return subMeshes[subMeshes.Count - 1];
        }

        private void AddToBoneAssigns(int vidx, BoneAssign assign)
        {
            if (boneAssigns.ContainsKey(vidx))
            {
                BoneAssign[] clist = boneAssigns[vidx];
                if (clist[0] == null)
                    clist[0] = assign;
                else if (clist[1] == null)
                    clist[1] = assign;
                else if (clist[2] == null)
                    clist[2] = assign;
                else if (clist[3] == null)
                    clist[3] = assign;
            }
            else
            {
                BoneAssign[] clist = new BoneAssign[4];
                boneAssigns.Add(vidx, clist);
                if (clist[0] == null)
                    clist[0] = assign;
                else if (clist[1] == null)
                    clist[1] = assign;
                else if (clist[2] == null)
                    clist[2] = assign;
                else if (clist[3] == null)
                    clist[3] = assign;
            }
        }

        private void LoopThrough(List<int> faces, ref List<Vertex> outVerts)
        {
            for (int i = 0; i < faces.Count; i++)
            {
                Vertex v = new Vertex(positions[faces[i]],
                                      texCoords.Count > 0 ? texCoords[faces[i]] : null,
                                      normals.Count > 0 ? normals[faces[i]] : null);

                if (boneAssigns.ContainsKey(faces[i]))
                {
                    BoneAssign[] vertBoneAssigns = boneAssigns[faces[i]];
                    for (int j = 0; j < vertBoneAssigns.Length; j++)
                    {
                        if (vertBoneAssigns[j] != null)
                        {
                            v.AddBoneIndex(vertBoneAssigns[j].GetBoneIndex());
                            v.AddBoneWeight(vertBoneAssigns[j].GetBoneWeight());
                        }
                    }
                }
                outVerts.Add(v);
            }
        }

        public override object Load(LoadedAsset asset)
        {
            XmlReader xmlReader = XmlReader.Create(asset.Data);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "position")
                    {
                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        float z = float.Parse(xmlReader.GetAttribute("z"));
                        Vector3f vec = new Vector3f(x, y, z);
                        positions.Add(vec);
                    }
                    else if (xmlReader.Name == "normal")
                    {
                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        float z = float.Parse(xmlReader.GetAttribute("z"));
                        Vector3f vec = new Vector3f(x, y, z);
                        normals.Add(vec);
                    }
                    else if (xmlReader.Name == "texcoord")
                    {
                        float x = float.Parse(xmlReader.GetAttribute(0));
                        float y = float.Parse(xmlReader.GetAttribute(1));
                        Vector2f tc = new Vector2f(x, -y);
                        texCoords.Add(tc);
                    }
                    else if (xmlReader.Name == "face")
                    {
                        if (!useSubmeshes)
                        {
                            faces.Add(int.Parse(xmlReader.GetAttribute(0)));
                            faces.Add(int.Parse(xmlReader.GetAttribute(1)));
                            faces.Add(int.Parse(xmlReader.GetAttribute(2)));
                        }
                        else if (useSubmeshes)
                        {
                            CurrentSubmesh().faces.Add(int.Parse(xmlReader.GetAttribute(0)));
                            CurrentSubmesh().faces.Add(int.Parse(xmlReader.GetAttribute(1)));
                            CurrentSubmesh().faces.Add(int.Parse(xmlReader.GetAttribute(2)));
                        }
                    }
                    else if (xmlReader.Name == "skeletonlink")
                    {
                        string parentPath = System.IO.Directory.GetParent(asset.FilePath).ToString();
                        string skeletonPath = parentPath + "\\" + xmlReader.GetAttribute(0) + ".xml";
                        skeleton = (Skeleton)AssetManager.Load(skeletonPath, OgreXmlSkeletonLoader.GetInstance());
                    }
                    else if (xmlReader.Name == "vertexboneassignment" || xmlReader.Name == "boneassignment")
                    {
                        int vidx = int.Parse(xmlReader.GetAttribute("vertexindex"));
                        float boneWeight = float.Parse(xmlReader.GetAttribute("weight"));
                        int boneIndex = int.Parse(xmlReader.GetAttribute("boneindex"));
                        AddToBoneAssigns(vidx, new BoneAssign(vidx, boneWeight, boneIndex));
                    }
                    else if (xmlReader.Name == "submesh")
                    {
                        useSubmeshes = true;
                        if (xmlReader.GetAttribute("operationtype") != null)
                        {
                            // material
                            Submesh sm = new Submesh();
                            subMeshes.Add(sm);
                        }
                    }
                }
            }
            xmlReader.Close();
            List<Vertex> vertices = new List<Vertex>();
            if (!useSubmeshes)
            {
                LoopThrough(faces, ref vertices);
            }
            else
            {
                for (int i = subMeshes.Count - 1; i > -1; i--)
                {
                    Submesh s = subMeshes[i];
                    if (s.faces.Count > 0)
                        LoopThrough(s.faces, ref s.vertices);
                    else
                        subMeshes.Remove(s);
                }
            }

            if (skeleton.GetNumBones() > 0)
            {
                for (int i = 0; i < skeleton.GetNumBones(); i++)
                    skeleton.GetBone(i).SetToBindingPose();
                skeleton.GetBone(0).CalculateBindingRotation();
                skeleton.GetBone(0).CalculateBindingTranslation();
                for (int i = 0; i < skeleton.GetNumBones(); i++)
                {
                    skeleton.GetBone(i).StoreBindingPose();
                    skeleton.GetBone(i).ClearPose();
                }
                skeleton.GetBone(0).UpdateTransform();
            }
            bool hasAnimations = skeleton.GetAnimations().Count > 0;
            AnimationController animControl = new AnimationController(skeleton);
            Node res = new Node();
            if (!useSubmeshes)
            {
                Mesh mesh = new Mesh();
                mesh.SetSkeleton(skeleton);
                mesh.SetVertices(vertices);
                Geometry geom = new Geometry();
                geom.Mesh = mesh;
                res.AddChild(geom);
            }
            else
            {
                for (int i = 0; i < subMeshes.Count; i++)
                {
                    Submesh sm = subMeshes[i];

                    Mesh mesh = new Mesh();
                    mesh.SetSkeleton(skeleton);
                    mesh.SetVertices(sm.vertices);
                    Geometry geom = new Geometry();
                    geom.Mesh = mesh;
                    res.AddChild(geom);
                }
            }
            if (hasAnimations)
            {
                res.AddController(animControl);
            }
            ResetLoader();
            return res;
        }
    }
}