using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.PostProcess;

namespace ApexEngine.Plugins.Shaders.Post
{
    public class FXAAFilter : PostFilter
    {
        private Vector2f screenSize = new Vector2f();

        private const string FRAG_CODE = "uniform sampler2D u_sceneTex;\n"
                + "uniform vec2 frameBufSize;\n"
                + "varying vec2 v_texCoord0;\n"
                + "\n"
                + "void main() {\n"
                + "    //gl_FragColor.xyz = texture2D(u_sceneTex,v_texCoord0).xyz;\n"
                + "    //return;\n"
                + "\n"
                + "    float FXAA_SPAN_MAX = 8.0;\n"
                + "    float FXAA_REDUCE_MUL = 1.0/8.0;\n"
                + "    float FXAA_REDUCE_MIN = 1.0/128.0;\n"
                + "\n"
                + "    vec3 rgbNW=texture2D(u_sceneTex,v_texCoord0+(vec2(-1.0,-1.0)/frameBufSize)).xyz;\n"
                + "    vec3 rgbNE=texture2D(u_sceneTex,v_texCoord0+(vec2(1.0,-1.0)/frameBufSize)).xyz;\n"
                + "    vec3 rgbSW=texture2D(u_sceneTex,v_texCoord0+(vec2(-1.0,1.0)/frameBufSize)).xyz;\n"
                + "    vec3 rgbSE=texture2D(u_sceneTex,v_texCoord0+(vec2(1.0,1.0)/frameBufSize)).xyz;\n"
                + "    vec3 rgbM=texture2D(u_sceneTex,v_texCoord0).xyz;\n"
                + "\n"
                + "    vec3 luma=vec3(0.299, 0.587, 0.114);\n"
                + "    float lumaNW = dot(rgbNW, luma);\n"
                + "    float lumaNE = dot(rgbNE, luma);\n"
                + "    float lumaSW = dot(rgbSW, luma);\n"
                + "    float lumaSE = dot(rgbSE, luma);\n"
                + "    float lumaM  = dot(rgbM,  luma);\n"
                + "\n"
                + "    float lumaMin = min(lumaM, min(min(lumaNW, lumaNE), min(lumaSW, lumaSE)));\n"
                + "    float lumaMax = max(lumaM, max(max(lumaNW, lumaNE), max(lumaSW, lumaSE)));\n"
                + "\n"
                + "    vec2 dir;\n"
                + "    dir.x = -((lumaNW + lumaNE) - (lumaSW + lumaSE));\n"
                + "    dir.y =  ((lumaNW + lumaSW) - (lumaNE + lumaSE));\n"
                + "\n"
                + "    float dirReduce = max(\n"
                + "        (lumaNW + lumaNE + lumaSW + lumaSE) * (0.25 * FXAA_REDUCE_MUL),\n"
                + "        FXAA_REDUCE_MIN);\n"
                + "\n"
                + "    float rcpDirMin = 1.0/(min(abs(dir.x), abs(dir.y)) + dirReduce);\n"
                + "\n"
                + "    dir = min(vec2( FXAA_SPAN_MAX,  FXAA_SPAN_MAX),\n"
                + "          max(vec2(-FXAA_SPAN_MAX, -FXAA_SPAN_MAX),\n"
                + "          dir * rcpDirMin)) / frameBufSize;\n"
                + "\n"
                + "    vec3 rgbA = (1.0/2.0) * (\n"
                + "        texture2D(u_sceneTex, v_texCoord0.xy + dir * (1.0/3.0 - 0.5)).xyz +\n"
                + "        texture2D(u_sceneTex, v_texCoord0.xy + dir * (2.0/3.0 - 0.5)).xyz);\n"
                + "    vec3 rgbB = rgbA * (1.0/2.0) + (1.0/4.0) * (\n"
                + "        texture2D(u_sceneTex, v_texCoord0.xy + dir * (0.0/3.0 - 0.5)).xyz +\n"
                + "        texture2D(u_sceneTex, v_texCoord0.xy + dir * (3.0/3.0 - 0.5)).xyz);\n"
                + "    float lumaB = dot(rgbB, luma);\n"
                + "\n"
                + "    if((lumaB < lumaMin) || (lumaB > lumaMax)){\n"
                + "        gl_FragColor.xyz=rgbA;\n"
                + "    }else{\n"
                + "        gl_FragColor.xyz=rgbB;\n"
                + "    }\n"
                + "}";

        public FXAAFilter() : base(FRAG_CODE)
        {
        }

        public override void End()
        {
        }

        public override void Update()
        {
            screenSize.Set(this.cam.Width, this.cam.Height);

            Texture.ActiveTextureSlot(0);
            this.colorTexture.Use();
            shader.SetUniform("u_sceneTex", 0);

            shader.SetUniform("frameBufSize", screenSize);
        }
    }
}