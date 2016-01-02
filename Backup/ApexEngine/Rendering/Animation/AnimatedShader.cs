using ApexEngine.Math;
using System.Collections.Generic;

namespace ApexEngine.Rendering.Animation
{
    public abstract class AnimatedShader : Shader
    {
        private List<string> boneNames = new List<string>();
        private bool isSkinningInit = false;

        public AnimatedShader(ShaderProperties properties, string vs_code, string fs_code)
            : base(properties, vs_code, fs_code)
        {
        }

        public AnimatedShader(ShaderProperties properties, string vs_code, string fs_code, string gs_code)
            : base(properties, vs_code, fs_code, gs_code)
        {
        }

        private void InitSkinning(Mesh mesh)
        {
            for (int i = 0; i < properties.GetInt("NUM_BONES"); i++)
            {
                boneNames.Add("Bone[" + i + "]");
            }
        }

        private void UpdateSkinning(Mesh mesh)
        {
            for (int i = 0; i < boneNames.Count; i++)
            {
                Matrix4f boneMat = mesh.GetSkeleton().GetBone(i).GetBoneMatrix();
                SetUniform(boneNames[i], boneMat);
            }
        }

        public override void Update(Environment environment, Camera cam, Mesh mesh)
        {
            base.Update(environment, cam, mesh);
            if (mesh != null && mesh.GetSkeleton() != null)
            {
                if (!isSkinningInit)
                {
                    InitSkinning(mesh);
                    isSkinningInit = true;
                }
                UpdateSkinning(mesh);
            }
        }
    }
}