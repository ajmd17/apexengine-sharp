using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if OPENGL
using OpenTK.Graphics.OpenGL;
#else
using OpenTK.Graphics.ES20;
#endif

namespace ApexEngine.Rendering.OpenGL
{
    public class GLRenderer : Renderer
    {

        #if OPENGL

        public override void GenerateMipmap(GenerateMipmapTarget target)
        {
            GL.GenerateMipmap(target);
        }

        public override void FramebufferTexture2D(FramebufferTarget framebuffer, FramebufferAttachment attachment, TextureTarget target, int texID, int level)
        {
            GL.FramebufferTexture2D(framebuffer, attachment, target, texID, level);
        }

        #endif

        public override void GenTextures(int n, out int textures)
        {
            GL.GenTextures(n, out textures);
        }

        public override void GenFramebuffers(int n, out int framebuffers)
        {
            GL.GenFramebuffers(n, out framebuffers);
        }

        public override void Viewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public override void ClearColor(float r, float g, float b, float a)
        {
            GL.ClearColor(r, g, b, a);
        }

        public override void Clear(ClearBufferMask bufferMask)
        {
            GL.Clear(bufferMask);
        }
        
        public override void CopyTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int w, int h)
        {
            GL.CopyTexSubImage2D(target, level, xoffset, yoffset, x, y, w, h);
        }

        public override void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalformat, int w, int h, int border,
                OpenTK.Graphics.OpenGL.PixelFormat format, PixelType pixelType, IntPtr ptr)
        {
    #if OPENGL
            GL.TexImage2D(target, level, internalformat, w, h, border, format, pixelType, ptr);
    #endif
        }
        
        public override void BindTexture(TextureTarget target, int i)
        {
            GL.BindTexture(target, i);
        }

        public override void BindFramebuffer(FramebufferTarget framebuffer, int id)
        {
            GL.BindFramebuffer(framebuffer, id);
        }

        public override void ActiveTexture(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
        }
        
        public override void TexParameter(TextureTarget target, TextureParameterName name, int param)
        {
            GL.TexParameter(target, name, param);
        }

        public override void BindRenderbuffer(RenderbufferTarget target, int id)
        {
            GL.BindRenderbuffer(target, id);
        }
    }
}
