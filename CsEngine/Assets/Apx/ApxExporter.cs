using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Animation;
using ApexEngine.Scene;
using ApexEngine.Math;
using ApexEngine.Assets.Util;
namespace ApexEngine.Assets.Apx
{
    public class ApxExporter
    {
        public const string TOKEN_FACES = "faces";
	    public const string TOKEN_FACE = "face";
	    public const string TOKEN_VERTEX = "vertex";
	    public const string TOKEN_VERTICES = "vertices";
	    public const string TOKEN_POSITION = "position";
	    public const string TOKEN_MESH = "mesh";
	    public const string TOKEN_NODE = "node";
	    public const string TOKEN_NAME = "name";
	    public const string TOKEN_ID = "id";
	    public const string TOKEN_PARENT = "parent";
	    public const string TOKEN_GEOMETRY = "geom";
	    public const string TOKEN_TEXCOORD0 = "uv0";
	    public const string TOKEN_TEXCOORD1 = "uv1";
	    public const string TOKEN_TEXCOORD2 = "uv2";
	    public const string TOKEN_TEXCOORD3 = "uv3";
	    public const string TOKEN_NORMAL = "normal";
	    public const string TOKEN_BONEWEIGHT = "bone_weight";
	    public const string TOKEN_BONEINDEX = "bone_index";
	    public const string TOKEN_VERTEXINDEX = "vertex_index";
	    public const string TOKEN_MATERIAL = "material";
	    public const string TOKEN_MATERIAL_PROPERTY = "material_property";
	    public const string TOKEN_SHADER = "shader";
	    public const string TOKEN_SHADERPROPERTIES = "shader_properties";
	    public const string TOKEN_SHADERPROPERTY = "property";
	    public const string TOKEN_CLASS = "class";
	    public const string TOKEN_TYPE = "type";
	    public const string TOKEN_TYPE_UNKNOWN = "unknown";
	    public const string TOKEN_TYPE_string = "string";
	    public const string TOKEN_TYPE_BOOLEAN = "boolean";
	    public const string TOKEN_TYPE_FLOAT = "float";
	    public const string TOKEN_TYPE_INT = "int";
	    public const string TOKEN_TYPE_COLOR = "color";
	    public const string TOKEN_TYPE_TEXTURE = "texture";
	    public const string TOKEN_VALUE = "value";
	    public const string TOKEN_HAS_POSITIONS = "has_positions";
	    public const string TOKEN_HAS_NORMALS = "has_normals";
	    public const string TOKEN_HAS_TEXCOORDS = "has_texcoords";
	    public const string TOKEN_HAS_BONES = "has_bones";
	    public const string TOKEN_SKELETON = "skeleton";
	    public const string TOKEN_BONE = "bone";
	    public const string TOKEN_SKELETON_ASSIGN = "skeleton_assign";
	    public const string TOKEN_ANIMATIONS = "animations";
	    public const string TOKEN_ANIMATION = "animation";
	    public const string TOKEN_ANIMATION_TRACK = "track";
	    public const string TOKEN_KEYFRAME = "keyframe";
	    public const string TOKEN_KEYFRAME_TRANSLATION = "keyframe_translation";
	    public const string TOKEN_KEYFRAME_ROTATION = "keyframe_rotation";
	    public const string TOKEN_TIME = "time";
	    public const string TOKEN_BONE_ASSIGNS = "bone_assigns";
	    public const string TOKEN_BONE_ASSIGN = "bone_assign";
	    public const string TOKEN_BONE_BINDPOSITION = "bind_position";
	    public const string TOKEN_BONE_BINDROTATION = "bind_rotation";
	    public const string TOKEN_MODEL = "model";
	    public const string TOKEN_TRANSLATION = "translation";
	    public const string TOKEN_ROTATION = "rotation";
	    public const string TOKEN_CONTROL = "control";
        private static List<Skeleton> skeletons = new List<Skeleton>();
        public static void ExportModel(GameObject gameObject, string path)
        {
            skeletons.Clear();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(path, settings);

            writer.WriteStartElement(TOKEN_MODEL);
            SaveObject(gameObject, writer);
            SaveSkeletons(writer);
            writer.WriteEndElement();
            writer.Close();
        }
        private static void SaveObject(GameObject gameObject, XmlWriter writer)
        {
            if (gameObject is Node)
            {
                SaveNode((Node)gameObject, writer);
            }
            else if (gameObject is Geometry)
            {
                SaveGeometry((Geometry)gameObject, writer);
            }
        }
        private static void SaveNode(Node node, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_NODE);
            writer.WriteAttributeString(TOKEN_NAME, node.GetName());
            writer.WriteStartElement(TOKEN_TRANSLATION);
            writer.WriteAttributeString("x", node.GetLocalTranslation().x.ToString());
            writer.WriteAttributeString("y", node.GetLocalTranslation().y.ToString());
            writer.WriteAttributeString("z", node.GetLocalTranslation().z.ToString());
            writer.WriteEndElement();
            for (int i = 0; i < node.GetChildren().Count; i++)
            {
                SaveObject(node.GetChild(i), writer);
            }
            writer.WriteEndElement();
        }
        private static void SaveGeometry(Geometry geom, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_GEOMETRY);
            writer.WriteAttributeString(TOKEN_NAME, geom.GetName());
            writer.WriteStartElement(TOKEN_TRANSLATION);
            writer.WriteAttributeString("x", geom.GetLocalTranslation().x.ToString());
            writer.WriteAttributeString("y", geom.GetLocalTranslation().y.ToString());
            writer.WriteAttributeString("z", geom.GetLocalTranslation().z.ToString());
            writer.WriteEndElement();
            if (geom.GetMesh() != null)
            {
                SaveMesh(geom.GetMesh(), writer);
            }
            writer.WriteEndElement();
        }
        private static void SaveMesh(Mesh mesh, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_MESH);

            List<Vector3f> positions = new List<Vector3f>();
            List<Vector3f> normals = new List<Vector3f>();
            List<Vector2f> texcoords0 = new List<Vector2f>();
            List<Vector2f> texcoords1 = new List<Vector2f>();
            List<BoneAssign> boneAssigns = new List<BoneAssign>();

            List<int> facesP = new List<int>();
            List<int> facesN = new List<int>();
            List<int> facesT0 = new List<int>();
            List<int> facesT1 = new List<int>();
            List<Vertex> tmpVerts = new List<Vertex>();

            for (int i = 0; i < mesh.indices.Count; i++)
            {
                tmpVerts.Add(mesh.vertices[mesh.indices[i]]);
            }
            for (int i = 0; i < tmpVerts.Count; i++)
            {
                Vertex v = tmpVerts[i];
                if (!positions.Contains(v.GetPosition()))
                    positions.Add(v.GetPosition());
                if (!normals.Contains(v.GetNormal()))
                    normals.Add(v.GetNormal());
                if (!texcoords0.Contains(v.GetTexCoord0()))
                    texcoords0.Add(v.GetTexCoord0());
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
                {
                    if (!texcoords1.Contains(v.GetTexCoord1()))
                        texcoords1.Add(v.GetTexCoord1());
                }
                facesP.Add(positions.IndexOf(v.GetPosition()));
                facesN.Add(normals.IndexOf(v.GetNormal()));
                facesT0.Add(texcoords0.IndexOf(v.GetTexCoord0()));
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
                {
                    facesT1.Add(texcoords1.IndexOf(v.GetTexCoord1()));
                }
                for (int j = 0; j < 4; j++)
                {
                    if (tmpVerts[i].GetBoneWeight(j) > 0f)
                    {
                        boneAssigns.Add(new BoneAssign(i, 
                                        tmpVerts[i].GetBoneWeight(j),
                                        (int)tmpVerts[i].GetBoneIndex(j)));
                    }
                }
            }

            writer.WriteAttributeString(TOKEN_HAS_POSITIONS, (facesP.Count > 0).ToString());
            writer.WriteAttributeString(TOKEN_HAS_NORMALS, (facesN.Count > 0).ToString());
            writer.WriteAttributeString(TOKEN_HAS_TEXCOORDS, (facesT0.Count > 0).ToString());

            writer.WriteStartElement(TOKEN_VERTICES);
            

            for (int i = 0; i < positions.Count; i++)
            {
                writer.WriteStartElement(TOKEN_POSITION);
                writer.WriteAttributeString("x", positions[i].x.ToString());
                writer.WriteAttributeString("y", positions[i].y.ToString());
                writer.WriteAttributeString("z", positions[i].z.ToString());
                writer.WriteEndElement();
            }
            for (int i = 0; i < normals.Count; i++)
            {
                writer.WriteStartElement(TOKEN_NORMAL);
                writer.WriteAttributeString("x", normals[i].x.ToString());
                writer.WriteAttributeString("y", normals[i].y.ToString());
                writer.WriteAttributeString("z", normals[i].z.ToString());
                writer.WriteEndElement();
            }
            for (int i = 0; i < texcoords0.Count; i++)
            {
                writer.WriteStartElement(TOKEN_TEXCOORD0);
                writer.WriteAttributeString("x", texcoords0[i].x.ToString());
                writer.WriteAttributeString("y", texcoords0[i].y.ToString());
                writer.WriteEndElement();
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
            {
                for (int i = 0; i < texcoords1.Count; i++)
                {
                    writer.WriteStartElement(TOKEN_TEXCOORD1);
                    writer.WriteAttributeString("x", texcoords1[i].x.ToString());
                    writer.WriteAttributeString("y", texcoords1[i].y.ToString());
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement(); // vertices

            writer.WriteStartElement(TOKEN_FACES);

            int stride = 3;
            if (facesT1.Count > 0)
                stride++;
            for (int i = 0; i < facesP.Count; i+=stride)
            {
                writer.WriteStartElement(TOKEN_FACE);
                writer.WriteAttributeString("i0", facesP[i].ToString() + "/" +
                                                  facesN[i].ToString() + "/" +
                                                  facesT0[i].ToString() +
                                                  ((facesT1.Count > 0) ? "/" + facesT1[i].ToString() : ""));

                writer.WriteAttributeString("i1", facesP[i+1].ToString() + "/" +
                                                  facesN[i + 1].ToString() + "/" +
                                                  facesT0[i + 1].ToString() +
                                                  ((facesT1.Count > 0) ? "/" + facesT1[i + 1].ToString() : ""));

                writer.WriteAttributeString("i2", facesP[i + 2].ToString() + "/" +
                                                  facesN[i + 2].ToString() + "/" +
                                                  facesT0[i + 2].ToString() +
                                                  ((facesT1.Count > 0) ? "/" + facesT1[i + 2].ToString() : ""));
                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // faces


            if (boneAssigns.Count > 0)
            {
                writer.WriteStartElement(TOKEN_BONE_ASSIGNS);
                for (int i = 0; i < boneAssigns.Count; i++)
                {
                    writer.WriteStartElement(TOKEN_BONE_ASSIGN);
                    writer.WriteAttributeString(TOKEN_VERTEXINDEX, boneAssigns[i].GetVertexIndex().ToString());
                    writer.WriteAttributeString(TOKEN_BONEINDEX, boneAssigns[i].GetBoneIndex().ToString());
                    writer.WriteAttributeString(TOKEN_BONEWEIGHT, boneAssigns[i].GetBoneWeight().ToString());
                    writer.WriteEndElement(); // bone assign
                }
                writer.WriteEndElement(); // bone assigns
            }
            if (mesh.GetSkeleton() != null)
            {
                if (!skeletons.Contains(mesh.GetSkeleton()))
                    skeletons.Add(mesh.GetSkeleton());
                writer.WriteStartElement(TOKEN_SKELETON_ASSIGN);
                writer.WriteAttributeString(TOKEN_ID, skeletons.IndexOf(mesh.GetSkeleton()).ToString());
                writer.WriteEndElement(); // skeleton assign
            }


            writer.WriteEndElement(); // end mesh
        }
        private static void SaveSkeletons(XmlWriter writer)
        {
            foreach (Skeleton s in skeletons)
            {
                if (s.GetNumBones() > 0)
                {
                    SaveSkeleton(s, writer);
                }
            }
        }
        private static void SaveSkeleton(Skeleton s, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_SKELETON);
            foreach (Bone b in s.GetBones())
            {
                SaveBone(b, writer);
            }
            if (s.GetAnimations().Count > 0)
            {
                SaveAnimations(s, writer);
            }
            writer.WriteEndElement();
        }
        private static void SaveAnimations(Skeleton s, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_ANIMATIONS);
            foreach (Animation anim in s.GetAnimations())
            {
                SaveAnimation(anim, writer);
            }
            writer.WriteEndElement();
        }
        private static void SaveAnimation(Animation anim, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_ANIMATION);

            writer.WriteAttributeString(TOKEN_NAME, anim.GetName());
            for (int i = 0; i < anim.GetTracks().Count; i++)
            {
                writer.WriteStartElement(TOKEN_ANIMATION_TRACK);
                writer.WriteAttributeString(TOKEN_BONE, anim.GetTrack(i).GetBone().GetName());
                for (int j = 0; j < anim.GetTrack(i).frames.Count; j++)
                {
                    SaveKeyframe(anim.GetTrack(i).frames[j], writer);
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        private static void SaveKeyframe(Keyframe frame, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_KEYFRAME);

            writer.WriteAttributeString(TOKEN_TIME, frame.GetTime().ToString());

            writer.WriteStartElement(TOKEN_KEYFRAME_TRANSLATION);
            writer.WriteAttributeString("x", frame.GetTranslation().x.ToString());
            writer.WriteAttributeString("y", frame.GetTranslation().y.ToString());
            writer.WriteAttributeString("z", frame.GetTranslation().z.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(TOKEN_KEYFRAME_ROTATION);
            writer.WriteAttributeString("x", frame.GetRotation().x.ToString());
            writer.WriteAttributeString("y", frame.GetRotation().y.ToString());
            writer.WriteAttributeString("z", frame.GetRotation().z.ToString());
            writer.WriteAttributeString("w", frame.GetRotation().w.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        private static void SaveBone(Bone b, XmlWriter writer)
        {
            writer.WriteStartElement(TOKEN_BONE);

            writer.WriteAttributeString(TOKEN_NAME, b.GetName());
            if (b.GetParent() != null)
                writer.WriteAttributeString(TOKEN_PARENT, b.GetParent().GetName());

            writer.WriteStartElement(TOKEN_BONE_BINDPOSITION);
            writer.WriteAttributeString("x", b.GetBindTranslation().x.ToString());
            writer.WriteAttributeString("y", b.GetBindTranslation().y.ToString());
            writer.WriteAttributeString("z", b.GetBindTranslation().z.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(TOKEN_BONE_BINDROTATION);
            writer.WriteAttributeString("x", b.GetBindRotation().x.ToString());
            writer.WriteAttributeString("y", b.GetBindRotation().y.ToString());
            writer.WriteAttributeString("z", b.GetBindRotation().z.ToString());
            writer.WriteAttributeString("w", b.GetBindRotation().w.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
