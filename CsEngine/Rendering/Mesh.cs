using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace ApexEngine.Rendering
{
    public class Mesh
    {
        protected int vbo, ibo, size;
        protected Animation.Skeleton skeleton = null;
        public int vertexSize;
        public VertexAttributes attribs = new VertexAttributes();
        public List<Vertex> vertices = new List<Vertex>();
        public List<int> indices = new List<int>();
        public Mesh()
        {
            vbo = 0;
            ibo = 0;
        }
        public Animation.Skeleton GetSkeleton()
        {
            return skeleton;
        }
        public void SetSkeleton(Animation.Skeleton skeleton)
        {
            this.skeleton = skeleton;
        }
        public VertexAttributes GetAttributes()
        {
            return attribs;
        }
        public void SetVertices(List<Vertex> vertices)
        {
            List<int> idc = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                idc.Add(i);
            }
            SetVertices(vertices, idc);
        }
        public void SetVertices(List<Vertex> vertices, List<int> indices)
        {
            Create();
            this.vertices = vertices;
            this.indices = indices;
            if (vertices[0].GetPosition() != null)
		        this.attribs.SetAttribute(VertexAttributes.POSITIONS);
	        if (vertices[0].GetNormal() != null)
		        this.attribs.SetAttribute(VertexAttributes.NORMALS);
	        if (vertices[0].GetTexCoord0() != null)
		        this.attribs.SetAttribute(VertexAttributes.TEXCOORDS0);
	        if (vertices[0].GetTexCoord1() != null)
		        this.attribs.SetAttribute(VertexAttributes.TEXCOORDS1);
	        if (vertices[0].GetTangent() != null)
		        this.attribs.SetAttribute(VertexAttributes.TANGENTS);
	        if (vertices[0].GetBitangent() != null)
		        this.attribs.SetAttribute(VertexAttributes.BITANGENTS);
	        if (vertices[0].GetBoneWeight(0) != 0 || vertices[0].GetBoneWeight(1) != 0 ||
		        vertices[0].GetBoneWeight(2) != 0 || vertices[0].GetBoneWeight(3) != 0)
		        this.attribs.SetAttribute(VertexAttributes.BONEWEIGHTS);
	        if (vertices[0].GetBoneIndex(0) != 0 || vertices[0].GetBoneIndex(1) != 0 ||
		        vertices[0].GetBoneIndex(2) != 0 || vertices[0].GetBoneIndex(3) != 0)
		        this.attribs.SetAttribute(VertexAttributes.BONEINDICES);
            UpdateMesh();
        }
        void Create()
        {
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out ibo);
        }
        void UpdateMesh()
        {
            size = indices.Count;
            float[] vertexBuffer = MeshUtil.CreateFloatBuffer(this);
            int[] indexBuffer = new int[indices.Count];
            for (int i = 0; i < indices.Count; i++)
            {
                indexBuffer[i] = indices[i];
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBuffer.Length * sizeof(float)), vertexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexBuffer.Length * sizeof(int)), indexBuffer, BufferUsageHint.StaticDraw);
        }
        public void Render()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            if (GetAttributes().HasAttribute(VertexAttributes.POSITIONS))
            {
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetPositionOffset());
            }
            if (GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0))
            {
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetTexCoord0Offset());
            }
            if (GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
            {
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetTexCoord1Offset());
            }
            if (GetAttributes().HasAttribute(VertexAttributes.NORMALS))
            {
                GL.EnableVertexAttribArray(3);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetNormalOffset());
            }
            if (GetAttributes().HasAttribute(VertexAttributes.TANGENTS))
            {
                GL.EnableVertexAttribArray(4);
                GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetTangentOffset());
            }
            if (GetAttributes().HasAttribute(VertexAttributes.BITANGENTS))
            {
                GL.EnableVertexAttribArray(5);
                GL.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetBitangentOffset());
            }
            if (GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS))
            {
                GL.EnableVertexAttribArray(6);
                GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetBoneWeightOffset());
            }
            if (GetAttributes().HasAttribute(VertexAttributes.BONEINDICES))
            {
                GL.EnableVertexAttribArray(7);
                GL.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, vertexSize * sizeof(float), GetAttributes().GetBoneIndexOffset());
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.DrawElements(BeginMode.Triangles, size, DrawElementsType.UnsignedInt, 0);
            if (GetAttributes().HasAttribute(VertexAttributes.POSITIONS)) GL.DisableVertexAttribArray(0);
	        if (GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0)) GL.DisableVertexAttribArray(1);
	        if (GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1)) GL.DisableVertexAttribArray(2);
	        if (GetAttributes().HasAttribute(VertexAttributes.NORMALS)) GL.DisableVertexAttribArray(3);
	        if (GetAttributes().HasAttribute(VertexAttributes.TANGENTS)) GL.DisableVertexAttribArray(4);
	        if (GetAttributes().HasAttribute(VertexAttributes.BITANGENTS)) GL.DisableVertexAttribArray(5);
	        if (GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS)) GL.DisableVertexAttribArray(6);
	        if (GetAttributes().HasAttribute(VertexAttributes.BONEINDICES)) GL.DisableVertexAttribArray(7);
        }
    }

}
