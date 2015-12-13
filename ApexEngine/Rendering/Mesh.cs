using ApexEngine.Math;
using System.Collections.Generic;

namespace ApexEngine.Rendering
{
    public class Mesh
    {
        public enum PrimitiveTypes
        {
            Triangles,
            TriangleStrip,
            TriangleFan,
            Quads,
            QuadStrip,
            Lines,
            Points,
            Patches
        }

        public int vbo, ibo, size, vertexSize;
        protected Animation.Skeleton skeleton = null;
        public VertexAttributes attribs = new VertexAttributes();
        public List<Vertex> vertices = new List<Vertex>();
        public List<int> indices = new List<int>();
        public PrimitiveTypes primitiveType = PrimitiveTypes.Triangles;
        private BoundingBox boundingBox = null;
        private Material material = new Material();

        public Mesh()
        {
            vbo = 0;
            ibo = 0;
        }

        ~Mesh()
        {
            if (skeleton != null)
            {
                if (skeleton.GetAnimations() != null)
                    skeleton.GetAnimations().Clear();
                if (skeleton.GetBones() != null)
                    skeleton.GetBones().Clear();
                skeleton = null;
            }
        }

        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        public PrimitiveTypes PrimitiveType
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
            this.vertices = vertices;
            this.indices = indices;
            if (vertices.Count > 0)
            {
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
            }
            UpdateMesh();
        }

        private void Create()
        {
        }

        private void UpdateMesh()
        {
            RenderManager.Renderer.CreateMesh(this);

            RenderManager.Renderer.UploadMesh(this);
        }

        public void Render()
        {
            RenderManager.Renderer.RenderMesh(this);
        }

        public Mesh Clone()
        {
            Mesh newMesh = new Mesh();
            newMesh.SetVertices(vertices, indices);
            newMesh.primitiveType = this.primitiveType;
            newMesh.skeleton = this.skeleton;
            newMesh.Material = this.Material.Clone();
            return newMesh;
        }

        public BoundingBox CreateBoundingBox()
        {
            return CreateBoundingBox(vertices, indices);
        }

        public BoundingBox CreateBoundingBox(Vector3f worldTranslation)
        {
            return CreateBoundingBox(vertices, indices, worldTranslation);
        }

        public BoundingBox CreateBoundingBox(Matrix4f worldTransform)
        {
            return CreateBoundingBox(vertices, indices, worldTransform);
        }

        public BoundingBox CreateBoundingBox(List<Vertex> vertices, List<int> indices, Matrix4f worldTransform)
        {
            BoundingBox b = new BoundingBox(new Vector3f(float.MaxValue), new Vector3f(float.MinValue));
            for (int i = 0; i < indices.Count; i++)
            {
                b.Extend(vertices[indices[i]].GetPosition().Multiply(worldTransform));
            }
            return b;
        }

        public BoundingBox CreateBoundingBox(List<Vertex> vertices, List<int> indices, Vector3f worldTranslation)
        {
            BoundingBox b = new BoundingBox(new Vector3f(float.MaxValue), new Vector3f(float.MinValue));
            for (int i = 0; i < indices.Count; i++)
            {
                b.Extend(vertices[indices[i]].GetPosition().Add(worldTranslation));
            }
            return b;
        }

        public BoundingBox CreateBoundingBox(List<Vertex> vertices, List<int> indices)
        {
            BoundingBox b = new BoundingBox(new Vector3f(float.MaxValue), new Vector3f(float.MinValue));
            for (int i = 0; i < indices.Count; i++)
            {
                b.Extend(vertices[indices[i]].GetPosition());
            }
            return b;
        }
    }
}