using ApexEngine.Scene.Components;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexEngine.Rendering
{
    public class NormalMapRenderer : RenderComponent
    {
        private Framebuffer fbo;
        private Camera cam;
        private Environment env;
        private Texture texture;

        public NormalMapRenderer(Environment env, Camera cam)
        {
            this.cam = cam;
            this.env = env;
        }

        public Texture NormalMap
        {
            get { return texture; }
        }

        public override void Init()
        {
            fbo = new Framebuffer(cam.Width, cam.Height);
            fbo.Init();
        }

        public override void Render()
        {
            if (fbo.Width != cam.Width || fbo.Height != cam.Height)
            {
                fbo.Width = cam.Width;
                fbo.Height = cam.Height;
                fbo.Init();
            }
            fbo.Capture();

            RenderManager.Renderer.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            renderManager.RenderBucketNormals(env, cam, RenderManager.Bucket.Opaque);
            renderManager.RenderBucketNormals(env, cam, RenderManager.Bucket.Transparent);

            fbo.Release();
            texture = fbo.ColorTexture;
        }

        public override void Update()
        {
        }
    }
}
