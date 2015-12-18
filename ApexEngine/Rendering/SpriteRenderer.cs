using ApexEngine.Math;
using ApexEngine.Rendering.Cameras;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;

namespace ApexEngine.Rendering
{
    

    public class SpriteRenderer
    {
        private static Color4f white = new Color4f(1, 1, 1, 1);

        private Mesh mesh;
        private Geometry geom = new Geometry();
        private Shader shader;

        public Camera camera;
        private static Camera mycam;

        public static Transform ctransform = new Transform();
        public static Transform dtransform = new Transform();
        public static Vector3f ctrans = new Vector3f();
        public static Vector3f cscale = new Vector3f();
        public static Quaternion crot = new Quaternion();

        private static Color4f currentColor = new Color4f(white);

        private static float widthScale, heightScale;

        private Game game;

        public SpriteRenderer(Game game)
        {
            this.game = game;
            this.camera = game.Camera;
            mycam = new OrthoCamera(-1, 1, -1, 1, -1, 1);
        }

        private void Setup()
        {
            float ar = game.InputManager.SCREEN_WIDTH / game.InputManager.SCREEN_HEIGHT;
            mycam.ProjectionMatrix.SetToOrtho(-ar, ar, -1, 1, -1, 1);

            if (mesh == null)
            {
                mesh = MeshFactory.CreateQuad();

                shader = new SpriteShader();

                geom.Mesh = mesh;
                geom.SetShader(shader);
                geom.Material.SetValue(Material.MATERIAL_DEPTHMASK, false);
                geom.Material.SetValue(Material.MATERIAL_DEPTHTEST, false);
                geom.Material.SetValue(Material.MATERIAL_CULLENABLED, false);
            }
        }



        public void Render(Texture2D tex, float x, float y, float width, float height)
        {
            Render(tex, x, y, width, height, white);
        }

        public void Render(Texture2D tex, float x, float y)
        {
            Render(tex, x, y, tex.Width, tex.Height);
        }

        public void Render(Texture2D tex, float x, float y, float width, float height, Color4f color)
        {
            Render(tex, dtransform, x, y, width, height, color);
        }

        public void Render(Texture2D tex, Transform transform, float x, float y, float width, float height, Color4f color)
        {
            Setup();
            currentColor = color;
            geom.Material.SetValue(Material.TEXTURE_DIFFUSE, tex);
            geom.Material.SetValue(Material.MATERIAL_BLENDMODE, 1);

            widthScale = 1f / (game.InputManager.SCREEN_WIDTH);
            heightScale = 1f / (game.InputManager.SCREEN_HEIGHT);
            cscale.Set(width * widthScale, height * heightScale, 1.0f);
            ctrans.x = (2f * ((x * widthScale) + (cscale.x / 2)) - 1f);
            ctrans.y = (2f * ((y * heightScale) + (cscale.y / 2)) - 1f);
            ctrans.z = 1;

            crot = transform.GetRotation();


            RenderManager.Renderer.SetDepthTest(false);
            RenderManager.Renderer.SetDepthMask(false);
            RenderManager.Renderer.SetBlend(true);
            RenderManager.Renderer.SetBlendMode(Renderer.BlendMode.AlphaBlend);

            geom.Render(null, camera);

            RenderManager.Renderer.SetDepthTest(true);
            RenderManager.Renderer.SetDepthMask(true);
            RenderManager.Renderer.SetBlend(false);
        }



        public class SpriteShader : Shader
        {
            public SpriteShader() : base(new ShaderProperties(),
                        "attribute vec3 a_position;" + "attribute vec3 a_normal;" + "attribute vec2 a_texcoord;"
                                + "uniform mat4 u_world; uniform mat4 u_view; uniform mat4 u_proj;"
                                + "varying vec2 v_texcoord;" + "void main() {" + "	v_texcoord = a_texcoord;"
                                + "	gl_Position = u_world * vec4(a_position, 1.0);" + "}",

                        "uniform sampler2D u_texture;" + "uniform vec4 u_color;" + "uniform int B_hasTexture;"
                                + "varying vec2 v_texcoord;" + "void main() {" + " vec4 texcolor = vec4(1.0);\n "
                                + " if (B_hasTexture > 0) {\n"
                                + "		texcolor = texture2D(u_texture, vec2(-v_texcoord.x, -v_texcoord.y));\n" + "	}\n"
                                + "	gl_FragColor = texcolor*u_color;\n" + "}")
            { }

            public override void Update(Environment environment, Camera cam, Mesh mesh)
            {
                if (this.currentMaterial.GetTexture(Material.TEXTURE_DIFFUSE) != null)
                {
                    Texture diffuse = this.currentMaterial.GetTexture(Material.TEXTURE_DIFFUSE);
                    Texture.ActiveTextureSlot(0);
                    diffuse.Use();
                    this.SetUniform("u_texture", 0);
                    this.SetUniform("B_hasTexture", 1);
                }
                else
                {
                    this.SetUniform("B_hasTexture", 0);
                }
                ctransform.SetTranslation(ctrans);
                ctransform.SetRotation(crot);
                ctransform.SetScale(cscale);
                this.SetUniform("u_color", currentColor);
                this.SetUniform("u_world", ctransform.GetMatrix());
                this.SetUniform("u_view", mycam.ViewMatrix);
                this.SetUniform("u_proj", mycam.ProjectionMatrix);
            }

            public override void End()
            {
                base.End();

                Texture2D.Clear();
            }
        }
    }
}