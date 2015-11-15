using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

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
        public Framebuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public void Use()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferID);
        }
        public static void Clear()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void Init()
        {
            GL.GenFramebuffers(1, out framebufferID);
            GL.GenTextures(1, out colorTextureID);
            GL.GenTextures(1, out depthTextureID);
            Use();
            GL.BindTexture(TextureTarget.Texture2D, colorTextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Int, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorTextureID, 0);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthTextureID);
            GL.BindTexture(TextureTarget.Texture2D, depthTextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent16, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Int, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTextureID, 0);
            Clear();
            colorTexture = new Texture(colorTextureID);
            depthTexture = new Texture(depthTextureID);
        }
        public void Capture()
        {
            Use();
            GL.Viewport(0, 0, width, height); 
        }
        public void Release()
        {
            Clear();
            Texture.Clear();
        }
    }
}
