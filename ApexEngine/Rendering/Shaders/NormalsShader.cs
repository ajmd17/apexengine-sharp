using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexEngine.Rendering.Shaders
{
    public class NormalsShader : Shader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        public NormalsShader(ShaderProperties properties)
            : base(properties, (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\normals.vert"),
                  (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\normals.frag"))
        {
        }
    }
}
