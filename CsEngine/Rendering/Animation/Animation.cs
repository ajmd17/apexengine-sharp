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
            return 0;// tracks[tracks.Count - 1].GetTrackLength();
        }
    }
}
