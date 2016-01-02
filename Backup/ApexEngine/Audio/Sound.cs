using ApexEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Audio
{
    public class Sound
    {
        private int bufferID, sourceID;

        public Sound()
        {
        }

        public int Buffer
        {
            get { return bufferID; }
            set { bufferID = value; }
        }

        public int Source
        {
            get { return sourceID; }
            set { sourceID = value; }
        }

        public bool IsPlaying()
        {
            return RenderManager.Renderer.GetPlayState(this) == Renderer.AudioPlayState.Playing;
        }

        public bool IsPaused()
        {
            return RenderManager.Renderer.GetPlayState(this) == Renderer.AudioPlayState.Paused;
        }

        public bool IsStopped()
        {
            return RenderManager.Renderer.GetPlayState(this) == Renderer.AudioPlayState.Stopped;
        }

        public void Play()
        {
            RenderManager.Renderer.PlaySound(this);
        }

        public void Stop()
        {
            RenderManager.Renderer.StopSound(this);
        }

        public void Pause()
        {
            RenderManager.Renderer.PauseSound(this);
        }
    }
}
