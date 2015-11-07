using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Animation
{
    public class AnimationTrack
    {
        private Bone bone;
        public List<Keyframe> frames = new List<Keyframe>();
        public void SetBone(Bone bone)
        {
            this.bone = bone;
        }
        public Bone GetBone()
        {
            return bone;
        }
        public Keyframe GetPoseAt(float time)
        {
            /* int first = 0, second = -1;

             Keyframe currentKeyframe = null;
             Keyframe nextKeyframe = null;
             int n = frames.Count - 1;
             for (int i = 0; i < n; i++)
             {
                 if (time >= frames[i].GetTime() && time <= frames[i + 1].GetTime())
                 {
                     first = i;
                     second = i + 1;
                 }
             }
             currentKeyframe = frames[first];

             Vector3f trans = new Vector3f(currentKeyframe.GetTranslation());
             Quaternion rot = currentKeyframe.GetRotation();
             if (second > first)
             {
                 nextKeyframe = frames[second];

                 float fraction = (time - currentKeyframe.GetTime()) / (nextKeyframe.GetTime() - currentKeyframe.GetTime());
                 trans.LerpStore(nextKeyframe.GetTranslation(), fraction);
                 Quaternion nextrot = nextKeyframe.GetRotation();
                 rot.SlerpStore(nextrot, fraction);
             }
             Keyframe res = new Keyframe(time, trans, rot);

             return res;*/
            return null;
        }
    }
}
