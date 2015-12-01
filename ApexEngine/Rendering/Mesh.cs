using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using ApexEngine.Math;

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
        private BeginMode primitiveType = BeginMode.Triangles;
        private BoundingBox boundingBox = null;

        public Mesh()
        {
            vbo = 0;
            ibo = 0;
        }

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        public BeginMode PrimitiveType
        {
            get { return primitiveType; }
            set { primitiveType = value; }
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
            //  MeshUtil.CalculateTangents(this.vertices, this.indices);
            if (vertices[0].GetPosition() != null)
                attribs.SetAttribute(VertexAttributes.POSITIONS);
            if (vertices[0].GetNormal() != null)
                attribs.SetAttribute(VertexAttributes.NORMALS);
            if (vertices[0].GetTexCoord0() != null)
                attribs.SetAttribute(VertexAttributes.TEXCOORDS0);
            if (vertices[0].GetTexCoord1() != null)
                attribs.SetAttribute(VertexAttributes.TEXCOORDS1);
            if (vertices[0].GetTangent() != null)
                attribs.SetAttribute(VertexAttributes.TANGENTS);
            if (vertices[0].GetBitangent() != null)
                attribs.SetAttribute(VertexAttributes.BITANGENTS);
            if (vertices[0].GetBoneWeight(0) != 0 || vertices[0].GetBoneWeight(1) != 0 ||
                vertices[0].GetBoneWeight(2) != 0 || vertices[0].GetBoneWeight(3) != 0)
                attribs.SetAttribute(VertexAttributes.BONEWEIGHTS);
            if (vertices[0].GetBoneIndex(0) != 0 || vertices[0].GetBoneIndex(1) != 0 ||
                vertices[0].GetBoneIndex(2) != 0 || vertices[0].GetBoneIndex(3) != 0)
                attribs.SetAttribute(VertexAttributes.BONEINDICES);
            UpdateMesh();
        }

        private void Create()
        {
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out ibo);
        }

        private void UpdateMesh()
        {
            size = indices.Count;
            float[] vertexBuffer = Util.MeshUtil.CreateFloatBuffer(this);
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
            GL.DrawElements(primitiveType, size, DrawElementsType.UnsignedInt, 0);

            if (GetAttributes().HasAttribute(VertexAttributes.POSITIONS)) GL.DisableVertexAttribArray(0);
            if (GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0)) GL.DisableVertexAttribArray(1);
            if (GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1)) GL.DisableVertexAttribArray(2);
            if (GetAttributes().HasAttribute(VertexAttributes.NORMALS)) GL.DisableVertexAttribArray(3);
            if (GetAttributes().HasAttribute(VertexAttributes.TANGENTS)) GL.DisableVertexAttribArray(4);
            if (GetAttributes().HasAttribute(VertexAttributes.BITANGENTS)) GL.DisableVertexAttribArray(5);
            if (GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS)) GL.DisableVertexAttribArray(6);
            if (GetAttributes().HasAttribute(VertexAttributes.BONEINDICES)) GL.DisableVertexAttribArray(7);
        }

        public BoundingBox CreateBoundingBox()
        {
            return CreateBoundingBox(vertices, indices);
        }


        public BoundingBox CreateBoundingBox(List<Vertex> vertices, List<int> indices)
        {
            BoundingBox b = new BoundingBox(new Vector3f(float.MaxValue), new Vector3f(float.MinValue));
            //Vertex v_min = new Vertex(new Vector3f(0.0f)), v_max = new Vertex(new Vector3f(0.0f));
            for (int i = 0; i < indices.Count; i++)
            {
                b.Extend(vertices[indices[i]].GetPosition());
               /* if (v.GetPosition().x < v_min.GetPosition().x) v_min.GetPosition().x = v.GetPosition().x;
                if (v.GetPosition().y < v_min.GetPosition().y) v_min.GetPosition().y = v.GetPosition().y;
                if (v.GetPosition().z < v_min.GetPosition().z) v_min.GetPosition().z = v.GetPosition().z;

                if (v.GetPosition().x > v_max.GetPosition().x) v_max.GetPosition().x = v.GetPosition().x;
                if (v.GetPosition().y > v_max.GetPosition().y) v_max.GetPosition().y = v.GetPosition().y;
                if (v.GetPosition().z > v_max.GetPosition().z) v_max.GetPosition().z = v.GetPosition().z;*/
            }
            return b;
        }
    }
}