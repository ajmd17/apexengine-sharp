using System.Collections.Generic;

namespace ApexEngine.Rendering
{
    public class ShaderProperties
    {
        public Dictionary<string, object> values = new Dictionary<string, object>();

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
    }
}