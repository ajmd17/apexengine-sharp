using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Rendering;
using ApexEngine.Assets;

namespace ApexEditor.NormalMapGenerator
{
    public class NormalMapShader : Shader
    {
        private static ShaderTextLoader textLoader = ShaderTextLoader.GetInstance();

        public NormalMapShader(ShaderProperties properties)
            : base(properties, (string)AssetManager.Load(AssetManager.GetAppPath() + "\\shaders\\normal_map_generator\\nm.vert"),
                               (string)AssetManager.Load(AssetManager.GetAppPath() + "\\shaders\\normal_map_generator\\nm.frag"))
        {
        }

        public override void Update(Environment environment, Camera cam, Mesh mesh)
        {
            base.Update(environment, cam, mesh);

            if (currentMaterial != null)
            {
                Texture2D tex = (Texture2D)currentMaterial.GetTexture(Material.TEXTURE_DIFFUSE);
                if (tex != null)
                {
                    Texture.ActiveTextureSlot(0);
                    tex.Use();
                    SetUniform("u_texture", 0);
                    SetUniform("u_imgWidth", tex.Width);
                    SetUniform("u_imgHeight", tex.Height);
                }
                SetUniform("u_delta", currentMaterial.GetFloat("delta_value"));
            }
        }
    }
}
