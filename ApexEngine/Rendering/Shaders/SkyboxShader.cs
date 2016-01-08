using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Assets;

namespace ApexEngine.Rendering.Shaders
{
    public class SkyboxShader : Shader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        public SkyboxShader(ShaderProperties properties)
            : base(properties, (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\skybox.vert"),
                               (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\skybox.frag"))
        {
        }

        public override void ApplyMaterial(Material material)
        {
            base.ApplyMaterial(material);
        }

        public override void End()
        {
            base.End();
            RenderManager.Renderer.SetBlend(false);
        }

        public override void Update(Environment environment, Camera cam, Mesh mesh)
        {
            base.Update(environment, cam, mesh);

            if (currentMaterial != null)
            {

                Texture envTex = currentMaterial.GetTexture(Material.TEXTURE_ENV);
                if (envTex != null)
                {
                    Texture.ActiveTextureSlot(3);
                    envTex.Use();
                    SetUniform("Material_EnvironmentMap", 3);
                    SetUniform("Material_HasEnvironmentMap", 1);
                }
                else
                {
                    SetUniform("Material_HasEnvironmentMap", 0);
                }
            }
        }
    }
}
