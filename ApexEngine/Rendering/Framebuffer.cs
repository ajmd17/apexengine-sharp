using System;
using OpenTK.Graphics.OpenGL;

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
            RenderManager.Renderer.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferID);
        }

        public static void Clear()
        {
            RenderManager.Renderer.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Init()
        {
            if (framebufferID == 0) RenderManager.Renderer.GenFramebuffers(1, out framebufferID);
            if (colorTextureID == 0) RenderManager.Renderer.GenTextures(1, out colorTextureID);
            if (depthTextureID == 0) RenderManager.Renderer.GenTextures(1, out depthTextureID);

            Use();
            RenderManager.Renderer.BindTexture(TextureTarget.Texture2D, colorTextureID);
            RenderManager.Renderer.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Int, IntPtr.Zero);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            RenderManager.Renderer.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorTextureID, 0);

            RenderManager.Renderer.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthTextureID);
            RenderManager.Renderer.BindTexture(TextureTarget.Texture2D, depthTextureID);
            RenderManager.Renderer.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Int, IntPtr.Zero);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            RenderManager.Renderer.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTextureID, 0);
            Clear();

            if (colorTexture == null) colorTexture = new Texture2D(colorTextureID);
            if (depthTexture == null) depthTexture = new Texture2D(depthTextureID);
        }

        public void Capture()
        {
            Use();
            GL.Viewport(0, 0, width, height);
        }

        public void Release()
        {
            Clear();
            Texture2D.Clear();
        }
    }
}