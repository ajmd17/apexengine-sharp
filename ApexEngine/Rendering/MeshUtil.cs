using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering
{
    public static class MeshUtil
    {
        public static float[] CreateFloatBuffer(Mesh mesh)
        {
            List<Vertex> a = mesh.vertices;
	        int vertSize = 0;
            int prevSize = 0;
            int offset = 0;

	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.POSITIONS))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetPositionOffset(offset);
		        prevSize = 3;
		        vertSize += prevSize;
	        }
	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetTexCoord0Offset(offset);
		        prevSize = 2;
		        vertSize += prevSize;
	        }
	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetTexCoord1Offset(offset);
		        prevSize = 2;
		        vertSize += prevSize;
	        }
	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.NORMALS))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetNormalOffset(offset);
		        prevSize = 3;
		        vertSize += prevSize;
	        }
	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.TANGENTS))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetTangentOffset(offset);
		        prevSize = 3;
		        vertSize += prevSize;
	        }
	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.BITANGENTS))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetBitangentOffset(offset);
		        prevSize = 3;
		        vertSize += prevSize;
	        }
	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetBoneWeightOffset(offset);
		        prevSize = 4;
		        vertSize += prevSize;
	        }
	        if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEINDICES))
	        {
		        offset += prevSize * 4;
		        mesh.GetAttributes().SetBoneIndexOffset(offset);
		        prevSize = 4;
		        vertSize += prevSize;
	        }
	        mesh.vertexSize = vertSize;

            List<float> floatBuffer = new List<float>();
            for (int i = 0; i < mesh.vertices.Count; i++)
            {
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.POSITIONS))
                {
                    if (mesh.vertices[i].GetPosition() != null)
                    {
                        floatBuffer.Add(mesh.vertices[i].GetPosition().x);
                        floatBuffer.Add(mesh.vertices[i].GetPosition().y);
                        floatBuffer.Add(mesh.vertices[i].GetPosition().z);
                    }
                }
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0))
                {
                    if (mesh.vertices[i].GetTexCoord0() != null)
                    {
                        floatBuffer.Add(mesh.vertices[i].GetTexCoord0().x);
                        floatBuffer.Add(mesh.vertices[i].GetTexCoord0().y);
                    }
                }
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
                {
                    if (mesh.vertices[i].GetTexCoord1() != null)
                    {
                        floatBuffer.Add(mesh.vertices[i].GetTexCoord1().x);
                        floatBuffer.Add(mesh.vertices[i].GetTexCoord1().y);
                    }
                }
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.NORMALS))
                {
                    if (mesh.vertices[i].GetNormal() != null)
                    {
                        floatBuffer.Add(mesh.vertices[i].GetNormal().x);
                        floatBuffer.Add(mesh.vertices[i].GetNormal().y);
                        floatBuffer.Add(mesh.vertices[i].GetNormal().z);
                    }
                }
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.TANGENTS))
                {
                    if (mesh.vertices[i].GetTangent() != null)
                    {
                        floatBuffer.Add(mesh.vertices[i].GetTangent().x);
                        floatBuffer.Add(mesh.vertices[i].GetTangent().y);
                        floatBuffer.Add(mesh.vertices[i].GetTangent().z);
                    }
                }
                if (mesh.GetAttributes().HasAttribute(VertexAttributes.BITANGENTS))
                {
                    if (mesh.vertices[i].GetBitangent() != null)
                    {
                        floatBuffer.Add(mesh.vertices[i].GetBitangent().x);
                        floatBuffer.Add(mesh.vertices[i].GetBitangent().y);
                        floatBuffer.Add(mesh.vertices[i].GetBitangent().z);
                    }
                }
		        if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS))
		        {
                    floatBuffer.Add(a[i].GetBoneWeight(0));
                    floatBuffer.Add(a[i].GetBoneWeight(1));
                    floatBuffer.Add(a[i].GetBoneWeight(2));
                    floatBuffer.Add(a[i].GetBoneWeight(3));
		        }
		        if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEINDICES))
		        {
                    floatBuffer.Add(a[i].GetBoneIndex(0));
                    floatBuffer.Add(a[i].GetBoneIndex(1));
                    floatBuffer.Add(a[i].GetBoneIndex(2));
                    floatBuffer.Add(a[i].GetBoneIndex(3));
		        }
            }
            float[] finalFloatBuffer = new float[floatBuffer.Count];
            for (int i = 0; i < floatBuffer.Count; i++)
            {
                finalFloatBuffer[i] = floatBuffer[i];
            }
            return finalFloatBuffer;
        }
    }
}
