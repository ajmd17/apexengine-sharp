using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.Animation
{
    public class Skeleton
    {
        protected List<Bone> bones = new List<Bone>();
        protected List<Animation> animations = new List<Animation>();
        public void AddAnimation(Animation anim)
        {
            animations.Add(anim);
        }
        public List<Animation> GetAnimations()
        {
            return animations;
        }
        public Animation GetAnimation(string name)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].GetName() == name)
                {
                    return animations[i];
                }
            }
            return null;
        }
        public Animation GetAnimation(int i)
        {
            return animations[i];
        }
        public List<Bone> GetBones()
        {
            return bones;
        }
        public int GetNumBones()
        {
            return bones.Count;
        }
        public void AddBone(Bone bone)
        {
            bones.Add(bone);
        }
        public Bone GetBone(string name)
        {
            for (int i = 0; i < bones.Count; i++)
            {
                if (bones[i].Name == name)
                {
                    return bones[i];
                }
            }
            return null;
        }
        public Bone GetBone(int i)
        {
            return bones[i];
        }
    }
}
