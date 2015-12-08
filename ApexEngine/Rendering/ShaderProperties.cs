using System.Collections.Generic;
using System.Linq;

namespace ApexEngine.Rendering
{
    public class ShaderProperties
    {
        public Dictionary<string, object> values = new Dictionary<string, object>();

        public ShaderProperties()
        {

        }

        public ShaderProperties(ShaderProperties other)
        {
            foreach (string str in other.values.Keys)
            {
                values.Add(str, other.values[str]);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is ShaderProperties)
            {
                return Equals((ShaderProperties)obj);
            }
            else
            {
                return false;
            }
        }

        public ShaderProperties Combine(ShaderProperties other)
        {
            string[] keys_other = other.values.Keys.ToArray();
            object[] vals_other = other.values.Values.ToArray();
            for (int i = 0; i < keys_other.Length; i++)
            {
                if (!values.ContainsKey(keys_other[i]))
                {
                    values.Add(keys_other[i], vals_other[i]);
                }
            }
            return this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(ShaderProperties s1)
        {
            return Util.ShaderUtil.CompareShader(this, s1);
        }

        public ShaderProperties SetProperty(string name, object val)
        {
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

        public bool GetBool(string name)
        {
            object obj = GetValue(name);
            if (obj is bool)
            {
                return (bool)obj;
            }
            return false;
        }

        public int GetInt(string name)
        {
            object obj = GetValue(name);
            if (obj is int)
            {
                return (int)obj;
            }
            return 0;
        }

        public float GetFloat(string name)
        {
            object obj = GetValue(name);
            if (obj is float)
            {
                return (float)obj;
            }
            return float.NaN;
        }

        public override string ToString()
        {
            string res = "Shader Properties:\n{\n";

            string[] keys = values.Keys.ToArray();
            object[] vals = values.Values.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                res += "\t" + keys[i] + ": " + vals[i].ToString() + "\n";
            }
            res += "}";

            return res;
        }
    }
}