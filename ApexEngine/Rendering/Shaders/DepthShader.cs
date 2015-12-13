using ApexEngine.Assets;
using System;

namespace ApexEngine.Rendering.Shaders
{
    public class DepthShader : Shader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        public DepthShader(ShaderProperties properties)
            : base(properties, (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\depth.vert"),
                  (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\depth.frag"))
        {
        }
    }
}
