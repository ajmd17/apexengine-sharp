using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace ApexEngine.Rendering
{
    public class VertexAttributes
    {
        public const int POSITIONS = 0, NORMALS = 1, TEXCOORDS0 = 2, TEXCOORDS1 = 3,
                         TANGENTS = 4, BITANGENTS = 5, BONEWEIGHTS = 6, BONEINDICES = 7;
        protected int posOffset, tc0Offset, tc1Offset, normalOffset, 
                   boneIndexOffset, boneWeightOffset, tangentOffset, bitangentOffset;
        List<int> attribs = new List<int>();
        public VertexAttributes()
        {

        }
        public void SetAttribute(int attrib)
        {
            if (!attribs.Contains(attrib))
            {
                attribs.Add(attrib);
            }
        }
        public bool HasAttribute(int attrib)
        {
            return attribs.Contains(attrib);
        }
        public int GetPositionOffset() { return posOffset; }
        public int GetTexCoord0Offset() { return tc0Offset; }
        public int GetTexCoord1Offset() { return tc1Offset; }
        public int GetNormalOffset() { return normalOffset; }
        public int GetTangentOffset() { return tangentOffset; }
        public int GetBitangentOffset() { return bitangentOffset; }
        public int GetBoneWeightOffset() { return boneWeightOffset; }
        public int GetBoneIndexOffset() { return boneIndexOffset; }
        public void SetPositionOffset(int offset) { posOffset = offset; }
        public void SetTexCoord0Offset(int offset) { tc0Offset = offset; }
        public void SetTexCoord1Offset(int offset) { tc1Offset = offset; }
        public void SetNormalOffset(int offset) { normalOffset = offset; }
        public void SetTangentOffset(int offset) { tangentOffset = offset; }
        public void SetBitangentOffset(int offset) { bitangentOffset = offset; }
        public void SetBoneWeightOffset(int offset) { boneWeightOffset = offset; }
        public void SetBoneIndexOffset(int offset) { boneIndexOffset = offset; }
        public override string ToString()
        {
            string fstr = "Vertex Attributes:\n";
            for (int i = 0; i < attribs.Count; i++)
            {
                int p = attribs[i];
		        string str = "";
		        if (p == VertexAttributes.POSITIONS)
		        {
			        str = "Positions";
		        } else if (p == VertexAttributes.TEXCOORDS0)
		        {
			        str = "Texture Coordinates (0)";
		        } else if (p == VertexAttributes.TEXCOORDS1)
		        {
			        str = "Texture Coordinates (1)";
		        } else if (p == VertexAttributes.NORMALS)
		        {
			        str = "Normals";
		        } else if (p == VertexAttributes.TANGENTS)
		        {
			        str = "Tangents";
		        } else if (p == VertexAttributes.BITANGENTS)
		        {
			        str = "Bitangents";
		        } else if (p == VertexAttributes.BONEWEIGHTS)
		        {
			        str = "Bone Weights";
		        } else if (p == VertexAttributes.BONEINDICES)
		        {
			        str = "Bone Indices";
		        }
                fstr += "\t" + str + "\n";
            }
            return fstr;
        }
    }
}
