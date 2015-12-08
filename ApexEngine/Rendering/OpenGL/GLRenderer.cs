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
        public override void CreateMesh(Mesh mesh)
        {
            GL.GenBuffers(1, out mesh.vbo);
            GL.GenBuffers(1, out mesh.ibo);
        }

        public override void UploadMesh(Mesh mesh)
        {
            mesh.size = mesh.indices.Count;
            float[] vertexBuffer = Util.MeshUtil.CreateFloatBuffer(mesh);
            int[] indexBuffer = new int[mesh.indices.Count];
            for (int i = 0; i < mesh.indices.Count; i++)
            {
                indexBuffer[i] = mesh.indices[i];
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBuffer.Length * sizeof(float)), vertexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexBuffer.Length * sizeof(int)), indexBuffer, BufferUsageHint.StaticDraw);
        }

        public override void RenderMesh(Mesh mesh)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vbo);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.POSITIONS))
            {
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetPositionOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0))
            {
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetTexCoord0Offset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
            {
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetTexCoord1Offset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.NORMALS))
            {
                GL.EnableVertexAttribArray(3);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetNormalOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TANGENTS))
            {
                GL.EnableVertexAttribArray(4);
                GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetTangentOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BITANGENTS))
            {
                GL.EnableVertexAttribArray(5);
                GL.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetBitangentOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS))
            {
                GL.EnableVertexAttribArray(6);
                GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetBoneWeightOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEINDICES))
            {
                GL.EnableVertexAttribArray(7);
                GL.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetBoneIndexOffset());
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ibo);
            GL.DrawElements(mesh.primitiveType, mesh.size, DrawElementsType.UnsignedInt, 0);

            if (mesh.GetAttributes().HasAttribute(VertexAttributes.POSITIONS)) GL.DisableVertexAttribArray(0);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0)) GL.DisableVertexAttribArray(1);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1)) GL.DisableVertexAttribArray(2);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.NORMALS)) GL.DisableVertexAttribArray(3);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TANGENTS)) GL.DisableVertexAttribArray(4);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BITANGENTS)) GL.DisableVertexAttribArray(5);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS)) GL.DisableVertexAttribArray(6);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEINDICES)) GL.DisableVertexAttribArray(7);
        }


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
