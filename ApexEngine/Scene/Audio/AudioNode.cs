using ApexEngine.Audio;
using ApexEngine.Math;
using ApexEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Scene.Audio
{
    public class AudioNode : Node
    {
        private Sound sound;
        private float soundPitch = 1.0f, soundGain = 1.0f;
        private Vector3f velocity = new Vector3f(0, 0, 0);

        public AudioNode(Sound sound) : base()
        {
            this.sound = sound;
        }

        public AudioNode(string name, Sound sound) : base(name)
        {
            this.sound = sound;
        }

        public AudioNode() : base()
        {

        }

        public AudioNode(string name) : base(name)
        {

        }

        public Sound Sound
        {
            get { return sound; }
            set { sound = value; }
        }

        public override void UpdateTransform()
        {
            base.UpdateTransform();

            RenderManager.Renderer.SetAudioValues(sound, soundPitch, soundGain, this.GetWorldTranslation(), this.velocity);
        }

        public override void SetWorldTransformPhysics(Vector3f trans, Quaternion rot, Vector3f scl)
        {
            base.SetWorldTransformPhysics(trans, rot, scl);
            
            RenderManager.Renderer.SetAudioValues(sound, soundPitch, soundGain, this.GetWorldTranslation(), this.velocity);
        }

        public override void SetParent(Node newParent)
        {
            base.SetParent(newParent);

            if (newParent == null)
            {
                if (!sound.IsStopped())
                    Stop();
            }
        }

        public void Play()
        {
            if (attachedToRoot)
                sound.Play();
        }

        public void Stop()
        {
            sound.Stop();
        }

        public void Pause()
        {
            sound.Pause();
        }

    }
}
