using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.PostProcess.Filters
{
    public class DepthVisualizerFilter : PostFilter
    {
        const string FRAGMENT_CODE = "#version 150\n" +
                                     "uniform sampler2D u_depthTex;\n" +
                                     "varying vec2 v_texCoord0;\n" + 
                                     "void main() {\n" +
                                     "  gl_FragColor = texture2D(u_depthTex, v_texCoord0);\n}\n";

        public DepthVisualizerFilter() : base(FRAGMENT_CODE)
        {

        }

        public override void End()
        {
        }

        public override void Update()
        {
            Texture.ActiveTextureSlot(0);
            depthTexture.Use();
            shader.SetUniform("u_depthTex", 0);
        }
    }
}
