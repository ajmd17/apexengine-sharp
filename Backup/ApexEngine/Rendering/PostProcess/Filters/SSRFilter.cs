using ApexEngine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.PostProcess.Filters
{
    public class SSRFilter : PostFilter
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();
        private NormalMapRenderer normalMapRenderer;
        private Texture normalMapTex;

        public SSRFilter(NormalMapRenderer normalMapRenderer) : base((string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\post\\ssr.frag"))
        {
            this.normalMapRenderer = normalMapRenderer;
        }

        public override void End()
        {
        }

        public override void Update()
        {
            normalMapTex = normalMapRenderer.NormalMap;
            if (colorTexture != null)
            {
                Texture.ActiveTextureSlot(0);
                colorTexture.Use();
                shader.SetUniform("u_texture", 0);
            }
            if (depthTexture != null)
            {
                Texture.ActiveTextureSlot(1);
                depthTexture.Use();
                shader.SetUniform("u_depthTexture", 1);
            }
            if (normalMapTex != null)
            {
                Texture.ActiveTextureSlot(2);
                normalMapTex.Use();
                shader.SetUniform("u_normalTexture", 2);
            }
            shader.SetUniform("u_viewMatrix", cam.ViewMatrix);
            shader.SetUniform("u_projectionMatrix", cam.ProjectionMatrix);
            shader.SetUniform("u_cameraPosition", cam.Translation);
            shader.SetUniform("u_near", cam.Near);
            shader.SetUniform("u_far", cam.Far);
            shader.SetUniform("u_width", cam.Width);
            shader.SetUniform("u_height", cam.Height);
        }
    }
}
