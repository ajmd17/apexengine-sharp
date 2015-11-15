using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Assets.Util
{
    public class BoneAssign
    {
        int vertIndex, boneIndex;
        float weight;
        public int GetVertexIndex()
        {
            return vertIndex;
        }
        public void SetVertexIndex(int i)
        {
            this.vertIndex = i;
        }
        public float GetBoneWeight()
        {
            return weight;
        }
        public void SetBoneWeight(float f)
        {
            weight = f;
        }
        public int GetBoneIndex()
        {
            return boneIndex;
        }
        public void SetBoneIndex(int i)
        {
            boneIndex = i;
        }
        public BoneAssign(int vertIndex, float weight, int boneIndex)
        {
            this.vertIndex = vertIndex;
            this.weight = weight;
            this.boneIndex = boneIndex;
        }
    }
}
