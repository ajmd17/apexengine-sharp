using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering
{
    public class ShaderManager
    {
        private static List<Shader> shaders = new List<Shader>();
        public static Shader GetShader(Type shaderType)
        {
            return GetShader(shaderType, new ShaderProperties());
        }
        public static Shader GetShader(Type shaderType, ShaderProperties properties)
        {
            for (int i = 0; i < shaders.Count; i++)
            {
                if (shaders[i].GetType() == shaderType)
                {
                    if (CompareShader(shaders[i].GetProperties(), properties))
                        return shaders[i];
                }
            }
            Shader shader = (Shader)Activator.CreateInstance(shaderType, properties);
            shaders.Add(shader);
            return shader;
        }
        public static bool CompareShader(ShaderProperties a, ShaderProperties b)
        {
            if (a.values.Count != b.values.Count)
                return false;
            foreach (var pair in a.values)
            {
                object value;
                if (b.values.TryGetValue(pair.Key, out value))
                {
                    if (value != pair.Value)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
