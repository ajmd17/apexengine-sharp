using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.Shaders
{
    public class DepthShader : Shader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        public DepthShader(ShaderProperties properties)
            : base(properties, (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\depth.vert"),
                  (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\depth.frag"))
        {
        }
    }
}
