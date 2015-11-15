using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.Animation
{
    public class Animation
    {
        protected string name;
        protected List<AnimationTrack> tracks = new List<AnimationTrack>();
        public Animation(string name)
        {
            SetName(name);
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return name;
        }
        public List<AnimationTrack> GetTracks()
        {
            return tracks;
        }
        public AnimationTrack GetTrack(int i)
        {
            return tracks[i];
        }
        public void AddTrack(AnimationTrack track)
        {
            tracks.Add(track);
        }
        public float GetTrackLength()
        {
            return tracks[tracks.Count - 1].GetTrackLength();
        }
        public void Apply(float time)
        {
            for (int i = 0; i < tracks.Count; i++)
            {
                AnimationTrack track = tracks[i];
                track.GetBone().ClearPose();
                track.GetBone().ApplyPose(track.GetPoseAt(time));
            }
        }
        public void ApplyBlend(float time, Animation toBlend, float blendAmt)
        {
            for (int i = 0; i < tracks.Count; i++)
            {
                AnimationTrack track = tracks[i];
                if (blendAmt <= 0.001f)
                {
                    track.GetBone().ClearPose();
                }
                if (track.GetBone().GetCurrentPose() != null)
                    track.GetBone().ApplyPose(track.GetBone().GetCurrentPose().Blend(track.GetPoseAt(time), Math.MathUtil.Clamp(blendAmt, 0f, 1f)));
                else
                    track.GetBone().ApplyPose(track.GetPoseAt(time));
            }
        }
    }
}
