using ApexEngine.Math;
using System.Collections.Generic;

namespace ApexEngine.Rendering
{
    public class Material
    {
        public const string MATERIAL_NAME = "matname";
        public const string MATERIAL_ALPHADISCARD = "alpha_discard";
        public const string MATERIAL_BLENDMODE = "blendmode"; // 0 = opaque, 1 = transparent
        public const string MATERIAL_DEPTHTEST = "depthtest";
        public const string MATERIAL_DEPTHMASK = "depthmask";
        public const string MATERIAL_FACETOCULL = "cullface"; // 0 = back, 1 = front
        public const string MATERIAL_CULLENABLED = "cullenable";

        public const string MATERIAL_CASTSHADOWS = "cast_shadows";

        public const string COLOR_DIFFUSE = "diffuse";
        public const string COLOR_SPECULAR = "specular";
        public const string COLOR_AMBIENT = "ambient";

        public const string TEXTURE_DIFFUSE = "diffuse_map";
        public const string TEXTURE_NORMAL = "normal_map";
        public const string TEXTURE_SPECULAR = "specular_map";
        public const string TEXTURE_HEIGHT = "height_map";
        public const string TEXTURE_ENV = "env_map";

        public const string SPECULAR_EXPONENT = "spec_exponent";
        public const string SHININESS = "shininess";
        public const string ROUGHNESS = "roughness";
        public const string METALNESS = "metalness";
        public const string TECHNIQUE_SPECULAR = "spec_technique";
        public const string TECHNIQUE_PER_PIXEL_LIGHTING = "per_pixel_lighting";

        private Vector2f tmpVec2 = new Vector2f();
        private Vector3f tmpVec3 = new Vector3f();
        private Vector4f tmpVec4 = new Vector4f();
        public Dictionary<string, object> values = new Dictionary<string, object>();
        protected RenderManager.Bucket bucket = RenderManager.Bucket.Opaque;
        private Shader shader, depthShader, normalsShader;

        public Material()
        {
            SetValue(COLOR_DIFFUSE, new Vector4f(1.0f));
            SetValue(COLOR_SPECULAR, new Vector4f(1.0f));
            SetValue(COLOR_AMBIENT, new Vector4f(0.0f));
            SetValue(TECHNIQUE_SPECULAR, 1);
            SetValue(TECHNIQUE_PER_PIXEL_LIGHTING, 1);
            SetValue(SHININESS, 0.5f);
            SetValue(ROUGHNESS, 0.2f);
            SetValue(METALNESS, 0.0f);
            SetValue(SPECULAR_EXPONENT, 20f);
            SetValue(MATERIAL_BLENDMODE, 0);
            SetValue(MATERIAL_CASTSHADOWS, 1);
            SetValue(MATERIAL_DEPTHMASK, true);
            SetValue(MATERIAL_DEPTHTEST, true);
            SetValue(MATERIAL_ALPHADISCARD, 0.1f);
            SetValue(MATERIAL_CULLENABLED, true);
            SetValue(MATERIAL_FACETOCULL, 0);
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

        public RenderManager.Bucket Bucket
        {
            get { return bucket; }
            set { bucket = value; }
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

        public bool GetBool(string name)
        {
            object obj = GetValue(name);
            if (obj != null)
            {
                if (obj is bool)
                {
                    return (bool)obj;
                }
                else if (obj is int)
                {
                    return (int)obj > 0 ? true : false;
                }
            }
            return false;
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

        public Shader Shader
        {
            get { return shader; }
            set { shader = value; }
        }

        public Shader DepthShader
        {
            get { return depthShader; }
            set { depthShader = value; }
        }

        public Shader NormalsShader
        {
            get { return normalsShader; }
            set { normalsShader = value; }
        }

        public Material Clone()
        {
            Material res = new Material();
            res.Bucket = this.Bucket;
            res.Shader = this.Shader;
            res.DepthShader = this.DepthShader;
            res.NormalsShader = this.NormalsShader;
            foreach (string str in values.Keys)
            {
                res.SetValue(str, values[str]);
            }
            return res;
        }
    }
}