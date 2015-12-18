using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Math;
using ApexEngine.Assets;

namespace ApexEngine.Rendering.PostProcess.Filters
{
    public class SSAOFilter : PostFilter
    {
        private NormalMapRenderer normalMapRenderer;
        private Texture normalMapTex, noiseTex;

        
        private Matrix4f invProj = new Matrix4f();
        private Vector3f[] kernel = new Vector3f[32];
        private Vector2f noiseScale = new Vector2f(10f, 10f);
        private RenderManager rm;
        private Vector2f resolution = new Vector2f();

        public SSAOFilter(NormalMapRenderer normalMapRenderer) : base(new ShaderProperties().SetProperty("NO_GI", true), (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\post\\ssao.frag"))
        {
            this.normalMapRenderer = normalMapRenderer;
            noiseTex = AssetManager.LoadTexture(AppDomain.CurrentDomain.BaseDirectory + "\\textures\\noise_ssao.png");
            Random rand = new Random();
            for (int i = 0; i < kernel.Length; i++)
                kernel[i] = new Vector3f((float)rand.NextDouble()*10f, (float)rand.NextDouble() * 10f, (float)rand.NextDouble() * 10f);
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
            if (noiseTex != null)
            {
                Texture.ActiveTextureSlot(3);
                noiseTex.Use();
                this.shader.SetUniform("u_rotationNoiseTexture", 3);
            }
            this.shader.SetUniform("u_rotationNoiseScale", noiseScale);
            this.shader.SetUniform("u_kernel", kernel);
            this.shader.SetUniform("u_inverseProjectionMatrix", invProj);
            this.shader.SetUniform("u_projectionMatrix", cam.ProjectionMatrix);
            this.shader.SetUniform("u_radius", 1.0f);
            this.shader.SetUniform("u_view", cam.ViewMatrix);
            this.shader.SetUniform("u_near", cam.Near);
            this.shader.SetUniform("u_far", cam.Far);
            this.shader.SetUniform("distanceThreshold", 0.0001f);
            this.shader.SetUniform("u_resolution", resolution);
            this.shader.SetUniform("u_cameraPosition", cam.Translation);
            this.shader.SetUniform("u_cameraDirection", cam.Direction);
            this.shader.SetUniform("u_invViewProj", cam.InverseViewProjectionMatrix);
        }
    }
}
