using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Animation
{
    public class AnimatedShader : Shader
    {
        List<string> strValues = new List<string>();
        bool isSkinningInit = false;
        private static Assets.ShaderTextLoader textLoader = new Assets.ShaderTextLoader();
        public AnimatedShader(ShaderProperties properties) 
            : base(properties, (string)textLoader.Load(Assets.AssetManager.GetAppPath() + "\\shaders\\anim.vert"), 
                  (string)textLoader.Load(Assets.AssetManager.GetAppPath() + "\\shaders\\anim.frag"))
        {

        }
        private void InitSkinning(Mesh mesh)
        {
            for (int i = 0; i < mesh.GetSkeleton().GetNumBones(); i++)
            {
                strValues.Add("Bone[" + i + "]");
            }
        }
        private void UpdateSkinning(Mesh mesh)
        {
            for (int i = 0; i < mesh.GetSkeleton().GetNumBones(); i++)
            {
                 Matrix4f boneMat = mesh.GetSkeleton().GetBone(i).GetBoneMatrix();
                 SetUniform(strValues[i], boneMat);
            }
        }
        public override void Update(Camera cam, Mesh mesh)
        {
            base.Update(cam, mesh);
            if (!isSkinningInit)
            {
                InitSkinning(mesh);
                isSkinningInit = true;
            }
            UpdateSkinning(mesh);
        }
    }
}
