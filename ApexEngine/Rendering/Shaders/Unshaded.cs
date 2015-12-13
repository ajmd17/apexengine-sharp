using ApexEngine.Assets;
using System;

namespace ApexEngine.Rendering.Shaders
{
    public class Unshaded : Shader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        public Unshaded(ShaderProperties properties)
            : base(properties, (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\unshaded.vert"),
                               (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\unshaded.frag"))
        {
        }

        public override void ApplyMaterial(Material material)
        {
            base.ApplyMaterial(material);
            
            this.SetUniform(MATERIAL_DIFFUSECOLOR, material.GetVector4f(Material.COLOR_DIFFUSE));
        
            int blendMode = material.GetInt(Material.MATERIAL_BLENDMODE);
            if (blendMode == 1)
            {
                RenderManager.Renderer.SetBlend(true);
                RenderManager.Renderer.SetBlendMode(Renderer.BlendMode.AlphaBlend);
            }
        }

        public override void End()
        {
            base.End();

            RenderManager.Renderer.SetBlend(false);
        }

        public override void Update(Environment environment, Camera cam, Mesh mesh)
        {
            base.Update(environment, cam, mesh);

            SetUniform(ENV_FOGSTART, environment.FogStart);
            SetUniform(ENV_FOGEND, environment.FogEnd);

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
