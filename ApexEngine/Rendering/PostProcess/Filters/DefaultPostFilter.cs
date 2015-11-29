using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.PostProcess.Filters
{
    public class DefaultPostFilter : PostFilter
    {
        public DefaultPostFilter() : base(new Shaders.Post.DefaultPostShader(new ShaderProperties()))
        {

        }

        public override void End()
        {
            
        }

        public override void Update()
        {
            Texture diffTex = colorTexture;
            if (diffTex != null)
            {
                Texture.ActiveTextureSlot(0);
                diffTex.Use();
                shader.SetUniform("u_texture", 0);
            }
        }
    }
}
