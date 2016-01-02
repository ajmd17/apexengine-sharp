using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexEngine.Rendering.PostProcess.Filters
{
    public class GammaCorrectionFilter : PostFilter
    {
        const string FRAGMENT_CODE = "#version 150\n" +
                                     "uniform sampler2D u_tex;\n" +
                                     "varying vec2 v_texCoord0;\n" +
                                     "void main() {\n" +
                                     "  float gamma = 1.5;\n  vec4 colorTex = texture2D(u_tex, v_texCoord0);\n" +
                                     "  gl_FragColor.rgb = pow(colorTex.rgb, vec3(1.0/gamma));\n  gl_FragColor.a = 1.0;\n  }\n";

        public GammaCorrectionFilter() : base(FRAGMENT_CODE)
        {

        }

        public override void End()
        {
        }

        public override void Update()
        {
            Texture.ActiveTextureSlot(0);
            colorTexture.Use();
            shader.SetUniform("u_tex", 0);
        }
    }
}
