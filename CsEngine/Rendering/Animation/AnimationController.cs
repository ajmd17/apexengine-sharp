using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Scene.Components;
namespace ApexEngine.Rendering.Animation
{
    public class AnimationController : Controller
    {
        public enum PlayState
        {
            Playing,
            Stopped,
            Paused
        }
        public enum LoopMode
        {
            Loop,
            PlayOnce
        }
        private float speed = 1.0f;
        private float time = 0f;
        private Animation currentAnim;
        private Animation lastAnim;
        private PlayState playState = PlayState.Stopped;
        private LoopMode loopMode = LoopMode.Loop;
        private Skeleton skeleton = null;
        private float blend = 0.0f;
        public AnimationController(Skeleton skeleton)
        {
            this.skeleton = skeleton;
        }
        public Skeleton GetSkeleton()
        {
            return this.skeleton;
        }
        public void SetLoopMode(LoopMode loopMode)
        {
            this.loopMode = loopMode;
        }
        public LoopMode GetLoopMode()
        {
            return loopMode;
        }
        public void SetPlayState(PlayState playState)
        {
            this.playState = playState;
            if (playState == PlayState.Stopped)
            {
                ResetAnimation();
                ClearPose();
            }
        }
        public PlayState GetPlayState()
        {
            return this.playState;
        }
        public void Play()
        {
            SetPlayState(PlayState.Playing);
        }
        public void Pause()
        {
            SetPlayState(PlayState.Paused);
        }
        public void Stop()
        {
            SetPlayState(PlayState.Stopped);
        }
        public Animation GetCurrentAnimation()
        {
            return this.currentAnim;
        }
        public void PlayAnimation(string name, float speed)
        {  
            if (currentAnim == null || currentAnim.GetName() != name || playState != PlayState.Playing || this.speed != speed)
            {
                ResetAnimation();
                SetAnimation(skeleton.GetAnimations().IndexOf(skeleton.GetAnimation(name)));
                SetSpeed(speed);
                SetPlayState(PlayState.Playing);
            }
        }
        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
        public void PlayAnimation(int i, float speed)
        {
            if (i != -1)
            {
                Animation anim = skeleton.GetAnimation(i);
                PlayAnimation(anim.GetName(), speed);
            }
            else
            {
                SetAnimation(-1);
            }
        }
        public void PlayAnimation(int i)
        {
            PlayAnimation(i, 1.0f);
        }
        public void PlayAnimation(string name)
        {
            PlayAnimation(name, 1.0f);
        }
        public void SetAnimation(int index)
        {
            if (currentAnim != null)
            {
                if (lastAnim == null)
                    lastAnim = currentAnim;
                else
                {
                    if (lastAnim != currentAnim)
                        lastAnim = currentAnim;
                }
            }
            if (index != -1)
            {
                currentAnim = skeleton.GetAnimation(index);
                ResetAnimation();
            }
            else
            {
                ClearPose();
            }
        }
        public void ApplyAnimation()
        {
            if (lastAnim != null)
                this.currentAnim.ApplyBlend(time, lastAnim, 1.0f - blend);
            else
                this.currentAnim.Apply(time);
        }
        private void ResetAnimation()
        {
            time = 0f; blend = 0.0f;
            if (currentAnim != null)
            {
                ApplyAnimation();
            }
        }
        public void ClearPose()
        {
            for (int i = 0; i < skeleton.GetNumBones(); i++)
                skeleton.GetBone(i).ClearPose();
        }
        public override void Init()
        {
            
        }

        public override void Update()
        {
            if (playState == PlayState.Playing)
            {
                if (currentAnim != null)
                {
                    if (blend < 1.0f)
                        blend += /* GameTime.GetDeltaTime() */ 0.1f;
                    time += /* GameTime.GetDeltaTime() */ 0.01f * speed;
                    if (time > currentAnim.GetTrackLength())
                    {
                        time = 0f;
                        if (loopMode == LoopMode.PlayOnce)
                        {
                            Stop();
                            // if (handler != null)
                             //   handler.OnAnimDone(currentAnim.GetName());
                        }
                        // if (handler != null)
                        //    handler.OnAnimLoop(currentAnim.GetName());
                    }
                    ApplyAnimation();
                }
                else
                {
                    SetPlayState(PlayState.Stopped);
                }
            }
        }
    }
}
