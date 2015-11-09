using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.Shaders
{
    public class DefaultShader : Shader
    {
        private static Assets.ShaderTextLoader textLoader = new Assets.ShaderTextLoader();
        public DefaultShader(ShaderProperties properties) 
            : base(properties, (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\default.vert"), 
                  (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\default.frag"))
        {

        }
    }
}
