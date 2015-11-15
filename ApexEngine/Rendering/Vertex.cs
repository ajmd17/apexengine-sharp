using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering
{
    public class Vertex
    {
        public Vector3f position;
        public Vector3f normal;
        public Vector2f texCoord0;
        public Vector2f texCoord1;
        public Vector3f tangent;
        public Vector3f bitangent;
        public float[] boneWeights = new float[] { 0.0f, 0.0f, 0.0f, 0.0f };
        public int[] boneIndices = new int[] { 0, 0, 0, 0 };
        private int boneISize = 0, boneWSize = 0;
        public Vertex()
        {

        }
        public Vertex(Vector3f pos)
        {
            position = pos;
        }
        public Vertex(Vector3f pos, Vector2f tc0)
        {
            position = pos;
            texCoord0 = tc0;
        }
        public Vertex(Vector3f pos, Vector2f tc0, Vector3f norm)
        {
            position = pos;
            texCoord0 = tc0;
            normal = norm;
        }
        public Vertex(Vertex other)
        {
            this.position = other.position;
            this.normal = other.normal;
            this.texCoord0 = other.texCoord0;
            this.texCoord1 = other.texCoord1;
            this.tangent = other.tangent;
            this.bitangent = other.bitangent;
            for (int i = 0; i < 4; i++)
            {
                this.boneIndices[i] = other.boneIndices[i];
                this.boneWeights[i] = other.boneWeights[i];
            }
        }
        public void SetPosition(Vector3f pos) { position = pos; }
        public void SetTexCoord0(Vector2f tc0) { texCoord0 = tc0; }
        public void SetTexCoord1(Vector2f tc1) { texCoord0 = tc1; }
        public void SetNormal(Vector3f norm) { normal = norm; }
        public void SetTangent(Vector3f tang) { tangent = tang; }
        public void SetBitangent(Vector3f bitang) { bitangent = bitang; }
        public void SetBoneWeight(int i, float val) { boneWeights[i] = val; }
        public void SetBoneIndex(int i, int val) { boneIndices[i] = val; }
        public Vector3f GetPosition() { return position; }
        public Vector2f GetTexCoord0() { return texCoord0; }
        public Vector2f GetTexCoord1() { return texCoord1; }
        public Vector3f GetNormal() { return normal; }
        public Vector3f GetTangent() { return tangent; }
        public Vector3f GetBitangent() { return bitangent; }
        public float GetBoneWeight(int i) { return boneWeights[i]; }
        public int GetBoneIndex(int i) { return boneIndices[i]; }
        public void AddBoneWeight(float weight)
        {
            if (boneWSize < 4)
            {
                boneWeights[boneWSize++] = weight;
            }
        }
        public void AddBoneIndex(int idx)
        {
            if (boneISize < 4)
            {
                boneIndices[boneISize++] = idx;
            }
        }
    }
}
