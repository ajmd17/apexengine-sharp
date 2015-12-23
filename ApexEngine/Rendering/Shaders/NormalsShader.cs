using ApexEngine.Assets;
using System;

namespace ApexEngine.Rendering.Shaders
{
    public class NormalsShader : Shader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        public NormalsShader(ShaderProperties properties)
            : base(properties, (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\normals.vert"),
                  (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\normals.frag"))
        {
        }

        public override void ApplyMaterial(Material material)
        {
            base.ApplyMaterial(material);

            this.SetUniform(MATERIAL_AMBIENTCOLOR, material.GetVector4f(Material.COLOR_AMBIENT));
            this.SetUniform(MATERIAL_DIFFUSECOLOR, material.GetVector4f(Material.COLOR_DIFFUSE));
            this.SetUniform(MATERIAL_SPECULARCOLOR, material.GetVector4f(Material.COLOR_SPECULAR));
            this.SetUniform(MATERIAL_SHININESS, material.GetFloat(Material.SHININESS));
            this.SetUniform(MATERIAL_ROUGHNESS, material.GetFloat(Material.ROUGHNESS));
            this.SetUniform(MATERIAL_METALNESS, material.GetFloat(Material.METALNESS));
            this.SetUniform(MATERIAL_SPECULARTECHNIQUE, material.GetInt(Material.TECHNIQUE_SPECULAR));
            this.SetUniform(MATERIAL_SPECULAREXPONENT, material.GetFloat(Material.SPECULAR_EXPONENT));
            this.SetUniform(MATERIAL_PERPIXELLIGHTING, material.GetInt(Material.TECHNIQUE_PER_PIXEL_LIGHTING));

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
            }
        }
    }
}