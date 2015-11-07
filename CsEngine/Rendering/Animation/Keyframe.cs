using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Animation
{
    public class Keyframe
    {
        private float time;
        private Vector3f translation = new Vector3f(Vector3f.ZERO);
        private Quaternion rotation = new Quaternion();
        public Keyframe(float time, Vector3f translation, Quaternion rotation)
        {
            this.time = time;
            this.translation = translation;
            this.rotation = rotation;
        }
        public Keyframe(float time, Vector3f translation, Vector3f axis, float angleRad)
        {
            this.time = time;
            this.translation = translation;
            this.rotation = new Quaternion().SetFromAxisRadNorm(axis.Multiply(-1), angleRad);
        }
        public Quaternion GetRotation()
        {
            return rotation;
        }
        public void SetRotation(Quaternion q)
        {
            this.rotation = q;
        }
        public float GetTime()
        {
            return time;
        }
        public void SetTime(float t)
        {
            this.time = t;
        }
        public Vector3f GetTranslation()
        {
            return translation;
        }
        public void SetTranslation(Vector3f v)
        {
            this.translation = v;
        }
        public Keyframe Blend(Keyframe to, float amt)
        {
            Vector3f trans = new Vector3f();
            trans.Set(GetTranslation());
            trans.LerpStore(to.GetTranslation(), amt);
            Quaternion q4 = new Quaternion();
            q4.Set(GetRotation());
            q4.SlerpStore(to.GetRotation(), amt);
            Keyframe c = new Keyframe(GetTime(), trans, q4);
            return c;
        }
    }
}
