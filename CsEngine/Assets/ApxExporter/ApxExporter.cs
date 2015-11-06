using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ApexEngine.Scene;
namespace ApexEngine.Assets.ApxExporter
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
        public static void ExportModel(GameObject gameObject, string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(path, settings);
            writer.WriteStartElement("Foo");
            writer.WriteAttributeString("Bar", "Some & value");
            writer.WriteElementString("Nested", "data");
            writer.WriteEndElement();

            writer.Close();
        }
    }
}
