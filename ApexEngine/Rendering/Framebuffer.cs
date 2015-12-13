namespace ApexEngine.Rendering
{
    public class Framebuffer
    {
        private int width, height, framebufferID, colorTextureID, depthTextureID;
        private Texture colorTexture, depthTexture;

        public Texture ColorTexture
        {
            get { return colorTexture; }
        }

        public Texture DepthTexture
        {
            get { return depthTexture; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public Framebuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Use()
        {
            RenderManager.Renderer.BindFramebuffer(framebufferID);
        }

        public static void Clear()
        {
            RenderManager.Renderer.BindFramebuffer(0);
        }

        public void Init()
        {
            if (framebufferID == 0) RenderManager.Renderer.GenFramebuffers(1, out framebufferID);
            if (colorTextureID == 0) RenderManager.Renderer.GenTextures(1, out colorTextureID);
            if (depthTextureID == 0) RenderManager.Renderer.GenTextures(1, out depthTextureID);

            Use();

            RenderManager.Renderer.SetupFramebuffer(framebufferID, colorTextureID, depthTextureID, width, height);

            Clear();

            if (colorTexture == null) colorTexture = new Texture2D(colorTextureID);
            if (depthTexture == null) depthTexture = new Texture2D(depthTextureID);
        }

        public void Capture()
        {
            Use();
            RenderManager.Renderer.Viewport(0, 0, width, height);
        }

        public void Release()
        {
            Clear();
            Texture2D.Clear();
        }
    }
}