using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.Shaders
{
    public class DefaultShader : Animation.AnimatedShader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();
        public DefaultShader(ShaderProperties properties) 
            : base(properties, (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\default.vert"), 
                  (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\default.frag"))
        {

        }
        public override void Update(Camera cam, Mesh mesh)
        {
            base.Update(cam, mesh);
            Environment.Environment.DirectionalLight.BindLight(0, this);
            Environment.Environment.AmbientLight.BindLight(0, this);
        }
    }
}
