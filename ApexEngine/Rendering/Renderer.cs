using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if OPENGL
using OpenTK.Graphics.OpenGL;
#else
using OpenTK.Graphics.ES20;
#endif

namespace ApexEngine.Rendering
{
    public abstract class Renderer
    {
        #if OPENGL

        public abstract void GenerateMipmap(GenerateMipmapTarget target);

        public abstract void FramebufferTexture2D(FramebufferTarget framebuffer, FramebufferAttachment attachment, TextureTarget target, int texID, int level);

        #endif
        /// <summary>
        /// Generate required buffers for the mesh
        /// </summary>
        /// <param name="mesh"></param>
        public abstract void CreateMesh(Mesh mesh);

        /// <summary>
        /// Upload mesh data to the GPU
        /// </summary>
        /// <param name="mesh"></param>
        public abstract void UploadMesh(Mesh mesh);

        /// <summary>
        /// Render the mesh
        /// </summary>
        /// <param name="mesh"></param>
        public abstract void RenderMesh(Mesh mesh);

        public abstract void GenTextures(int n, out int textures);

        public abstract void GenFramebuffers(int n, out int framebuffers);

        public abstract void Viewport(int x, int y, int width, int height);

        public abstract void ClearColor(float r, float g, float b, float a);

        public abstract void Clear(ClearBufferMask bufferMask);

        public abstract void CopyTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int w, int h);

        public abstract void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalformat, int w, int h, int border,
                OpenTK.Graphics.OpenGL.PixelFormat format, PixelType pixelType, IntPtr ptr);

        public abstract void BindTexture(TextureTarget target, int i);

        public abstract void BindFramebuffer(FramebufferTarget framebuffer, int id);

        public abstract void ActiveTexture(TextureUnit unit);

        public abstract void TexParameter(TextureTarget target, TextureParameterName name, int param);

        public abstract void BindRenderbuffer(RenderbufferTarget target, int id);
    }
}
