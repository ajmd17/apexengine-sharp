using ApexEngine.Math;
using ApexEngine.Scene;
using OpenTK.Graphics.OpenGL;
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
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(new Vertex(new Vector3f(-1f, -1f, 0), new Vector2f(0, 0)));
            vertices.Add(new Vertex(new Vector3f(1, -1f, 0), new Vector2f(1f, 0)));
            vertices.Add(new Vertex(new Vector3f(1f, 1f, 0), new Vector2f(1f, 1f)));
            vertices.Add(new Vertex(new Vector3f(-1f, 1f, 0), new Vector2f(0, 1f)));
            mesh.SetVertices(vertices);
            mesh.PrimitiveType = BeginMode.TriangleFan;
            quadGeom = new Geometry(mesh);
            postFilters.Add(new DefaultPostFilter());
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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Release()
        {
            fbo.Release();
            colorTexture = fbo.ColorTexture;
            depthTexture = fbo.DepthTexture;
            for (int i = postFilters.Count - 1; i > -1; i--)
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