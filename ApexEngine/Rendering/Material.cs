using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering
{
    public class Material
    {
        public const string MATERIAL_NAME = "matname";
        public const string MATERIAL_ALPHADISCARD = "alpha_discard";
        public const string MATERIAL_BLENDMODE = "blendmode";
        // 0 = opaque, 1 = transparent
        public const string MATERIAL_CASTSHADOWS = "cast_shadows";

        public const string COLOR_DIFFUSE = "diffuse";
        public const string COLOR_SPECULAR = "specular";
        public const string COLOR_AMBIENT = "ambient";

        public const string TEXTURE_DIFFUSE = "diffuse_map";
        public const string TEXTURE_NORMAL = "normal_map";
        public const string TEXTURE_SPECULAR = "specular_map";

        public const string SHININESS = "shininess";
        public const string TECHNIQUE_SPECULAR = "spec_technique";
        public const string TECHNIQUE_PER_PIXEL_LIGHTING = "per_pixel_lighting";

        private Vector2f tmpVec2 = new Vector2f();
        private Vector3f tmpVec3 = new Vector3f();
        private Vector4f tmpVec4 = new Vector4f();
        public Dictionary<string, object> values = new Dictionary<string, object>();
        public Material()
        {
            SetValue(SHININESS, 45f);
            SetValue(COLOR_DIFFUSE, new Vector4f(1.0f));
            SetValue(COLOR_SPECULAR, new Vector4f(1.0f));
            SetValue(COLOR_AMBIENT, new Vector4f(0.0f));
            SetValue(TECHNIQUE_SPECULAR, 1);
            SetValue(TECHNIQUE_PER_PIXEL_LIGHTING, 1);
            SetValue(MATERIAL_BLENDMODE, 0);
            SetValue(MATERIAL_CASTSHADOWS, 1);
        }
        public Dictionary<string, object> Values
        {
            get { return values; }
        }
        public Material SetName(string name)
        {
            return SetValue(MATERIAL_NAME, name);
        }
        public string GetName()
        {
            return GetString(MATERIAL_NAME);
        }
        public Material SetValue(string name, object val)
        {
            if (values.ContainsKey(name))
            { 
                values[name] = val;
                return this;
            }
            values.Add(name, val);
            return this;
        }
        public object GetValue(string name)
        {
            if (values.ContainsKey(name))
                return values[name];
            else
                return null;
        }
        public Texture GetTexture(string name)
        {
            object obj = GetValue(name);
            if (obj != null && obj is Texture)
            {
                return (Texture)obj;
            }
            return null;
        }
        public string GetString(string name)
        {
            object obj = GetValue(name);
            if (obj != null && obj is string)
            {
                return (string)obj;
            }
            return "";
        }
        public int GetInt(string name)
        {
            object obj = GetValue(name);
            if (obj != null && obj is int)
            {
                return (int)obj;
            }
            return 0;
        }
        public float GetFloat(string name)
        {
            object obj = GetValue(name);
            if (obj != null && obj is float)
            {
                return (float)obj;
            }
            return float.NaN;
        }
        public Vector2f GetVector2f(string name)
        {
            object obj = GetValue(name);
            if (obj != null && obj is Vector2f)
            {
                return (Vector2f)obj;
            }
            return tmpVec2;
        }
        public Vector3f GetVector3f(string name)
        {
            object obj = GetValue(name);
            if (obj != null && obj is Vector3f)
            {
                return (Vector3f)obj;
            }
            return tmpVec3;
        }
        public Vector4f GetVector4f(string name)
        {
            object obj = GetValue(name);
            if (obj != null && obj is Vector4f)
            {
                return (Vector4f)obj;
            }
            return tmpVec4;
        }
    }
}
