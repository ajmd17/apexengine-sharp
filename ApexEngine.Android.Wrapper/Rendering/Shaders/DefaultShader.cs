using System;

namespace ApexEngine.Rendering.Shaders
{
    public class DefaultShader : Animation.AnimatedShader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        public DefaultShader(ShaderProperties properties)
            : base(properties, (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\default.vert"),
                               (string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\default.frag"))
        {
        }

        public override void Update(Environment environment, Camera cam, Mesh mesh)
        {
            base.Update(environment, cam, mesh);

            environment.DirectionalLight.BindLight(0, this);
            environment.AmbientLight.BindLight(0, this);

            if (currentMaterial != null)
            {
                Texture diffuseTex = currentMaterial.GetTexture(Material.TEXTURE_DIFFUSE);
                if (diffuseTex != null)
                {
                    Texture.ActiveTextureSlot(0);
                    diffuseTex.Use();
                    SetUniform("Material_DiffuseMap", 0);
                    SetUniform("Material_HasDiffuseMap", 1);
                }
                else
                {
                    SetUniform("Material_HasDiffuseMap", 0);
                }

                Texture normalTex = currentMaterial.GetTexture(Material.TEXTURE_NORMAL);
                if (normalTex != null)
                {
                    Texture.ActiveTextureSlot(1);
                    normalTex.Use();
                    SetUniform("Material_NormalMap", 1);
                    SetUniform("Material_HasNormalMap", 1);
                }
                else
                {
                    SetUniform("Material_HasNormalMap", 0);
                }

                Texture heightTex = currentMaterial.GetTexture(Material.TEXTURE_HEIGHT);
                if (heightTex != null)
                {
                    Texture.ActiveTextureSlot(2);
                    heightTex.Use();
                    SetUniform("Material_HeightMap", 2);
                    SetUniform("Material_HasHeightMap", 1);
                }
                else
                {
                    SetUniform("Material_HasHeightMap", 0);
                }
            }
            if (environment.ShadowsEnabled)
            {
                SetUniform("Env_ShadowsEnabled", 1);
                for (int i = 0; i < 4; i++)
                {
                    Texture.ActiveTextureSlot(3 + i);
                    environment.ShadowMaps[i].Use();
                    SetUniform("Env_ShadowMap" + i.ToString(), 3 + i);
                    SetUniform("Env_ShadowMatrix" + i.ToString(), environment.ShadowMatrices[i]);
                    SetUniform("Env_ShadowMapSplits[" + i.ToString() + "]", environment.ShadowMapSplits[i]);
                }
            }
        }
    }
}