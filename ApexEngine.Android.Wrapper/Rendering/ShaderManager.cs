using System;
using System.Collections.Generic;

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
                    if (Util.ShaderUtil.CompareShader(shaders[i].GetProperties(), properties))
                        return shaders[i];
                }
            }

            Shader shader = (Shader)Activator.CreateInstance(shaderType, properties);
            shaders.Add(shader);
            return shader;
        }
    }
}