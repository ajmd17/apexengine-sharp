using ApexEngine.Math;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;
using System.Collections.Generic;

namespace ApexEngine.Rendering.PostProcess
{
    public class PostProcessor
    {
        private Framebuffer fbo;
        private Texture colorTexture, depthTexture;
        private RenderManager renderManager;
        private Camera cam;
        private List<PostFilter> postFilters = new List<PostFilter>();
        private Geometry quadGeom;

        public PostProcessor(RenderManager rm, Camera cam)
        {
            this.renderManager = rm;
            this.cam = cam;
            fbo = new Framebuffer(cam.Width, cam.Height);
        }

        public Texture ColorTexture
        {
            get { return colorTexture; }
            set { colorTexture = value; }
        }

        public Texture DepthTexture
        {
            get { return depthTexture; }
            set { depthTexture = value; }
        }

        public List<PostFilter> PostFilters
        {
            get { return postFilters; }
        }

        public void Init()
        {
            fbo.Init();
            Mesh mesh = MeshFactory.CreateQuad();
            mesh.Material.SetValue(Material.MATERIAL_DEPTHMASK, false);
            mesh.Material.SetValue(Material.MATERIAL_DEPTHTEST, false);
            quadGeom = new Geometry(mesh);
            postFilters.Add(new Filters.DefaultPostFilter());
        }

        public void Capture()
        {
            if (fbo.Width != cam.Width || fbo.Height != cam.Height)
            {
                fbo.Width = cam.Width;
                fbo.Height = cam.Height;
                fbo.Init();
            }
            fbo.Capture();
            RenderManager.Renderer.Clear(true, true, false);
        }

        public void Release()
        {
            fbo.Release();
            colorTexture = fbo.ColorTexture;
            depthTexture = fbo.DepthTexture;
            for (int i = postFilters.Count-1; i > -1 ; i--)
            {
                PostFilter pf = postFilters[i];
                pf.Cam = cam;
                pf.ColorTexture = colorTexture;
                pf.DepthTexture = depthTexture;
                pf.Shader.Use();
                pf.Update();

                quadGeom.SetShader(pf.Shader);
                quadGeom.Render(null, cam);

                pf.End();

                if (pf.SaveColorTexture)
                    renderManager.SaveScreenToTexture(cam, colorTexture);
            }
        }
    }
}