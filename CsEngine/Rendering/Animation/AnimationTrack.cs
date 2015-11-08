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
        private Vector3f tmpVec = new Vector3f();
        private Quaternion tmpRot = new Quaternion();
        private Keyframe tmpFrame = new Keyframe(0, new Vector3f(), new Quaternion());
        public AnimationTrack(Bone bone)
        {
            SetBone(bone);
        }
        public void SetBone(Bone bone)
        {
            this.bone = bone;
        }
        public Bone GetBone()
        {
            return bone;
        }
        public float GetTrackLength()
        {
            return frames[frames.Count - 1].GetTime();
        }
        public void AddKeyframe(Keyframe frame)
        {
            frames.Add(frame);
        }
        public Keyframe GetPoseAt(float time)
        {
             int first = 0, second = -1;

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

             tmpVec.Set(currentKeyframe.GetTranslation());
             tmpRot.Set(currentKeyframe.GetRotation());
             if (second > first)
             {
                 nextKeyframe = frames[second];

                 float fraction = (time - currentKeyframe.GetTime()) / (nextKeyframe.GetTime() - currentKeyframe.GetTime());
                 tmpVec.LerpStore(nextKeyframe.GetTranslation(), fraction);
                 Quaternion nextrot = nextKeyframe.GetRotation();
                 tmpRot.SlerpStore(nextrot, fraction);
             }
            //   Keyframe res = new Keyframe(time, tmpVec, tmpRot);
            tmpFrame.SetTime(time);
            tmpFrame.SetTranslation(tmpVec);
            tmpFrame.SetRotation(tmpRot);
             return tmpFrame;
        }
    }
}
