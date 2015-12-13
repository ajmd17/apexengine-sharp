using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Rendering;
using ApexEngine.Rendering.PostProcess;
using ApexEngine.Assets;
using ApexEngine.Math;

namespace ApexEditor.NormalMapGenerator
{
    public class BlurPostFilter : PostFilter
    {
        private static ShaderTextLoader textLoader = ShaderTextLoader.GetInstance();
        private Vector2f scale = new Vector2f(0.005f, 0.005f);

        public BlurPostFilter(bool horizontal) : base(horizontal ? (string)AssetManager.Load(AssetManager.GetAppPath() + "\\shaders\\normal_map_generator\\blurpass_h.frag") :
                                                      (string)AssetManager.Load(AssetManager.GetAppPath() + "\\shaders\\normal_map_generator\\blurpass_v.frag"))
        {

        }

        public override void End()
        {
        }

        public override void Update()
        {
            Texture.ActiveTextureSlot(0);
            colorTexture.Use();
            shader.SetUniform("tex", 0);
            shader.SetUniform("u_scale", scale);
            shader.SetUniform("u_texWidth", cam.Width);
            shader.SetUniform("u_texHeight", cam.Height);
        }
    }
}
