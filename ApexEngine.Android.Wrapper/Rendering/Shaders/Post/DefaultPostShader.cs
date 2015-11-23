namespace ApexEngine.Rendering.Shaders.Post
{
    public class DefaultPostShader : PostShader
    {
        private const string FRAGMENT_CODE = "uniform sampler2D u_texture;\n"
                                   + "varying vec2 v_texCoord0;\n"
                                   + "void main()\n"
                                   + "{\n"
                                   + "  gl_FragColor = texture(u_texture, v_texCoord0);\n"
                                   + "}\n";

        public DefaultPostShader(ShaderProperties props) : base(props, FRAGMENT_CODE)
        {
        }
    }
}