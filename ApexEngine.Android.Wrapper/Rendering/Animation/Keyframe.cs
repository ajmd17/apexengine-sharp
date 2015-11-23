using ApexEngine.Math;

namespace ApexEngine.Rendering.Animation
{
    public class Keyframe
    {
        private float time;
        private Vector3f translation = new Vector3f(Vector3f.ZERO);
        private Quaternion rotation = new Quaternion();
        private Quaternion tmpRot = new Quaternion();
        private Vector3f tmpVec = new Vector3f();

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
            tmpVec.Set(GetTranslation());
            tmpVec.LerpStore(to.GetTranslation(), amt);
            tmpRot.Set(GetRotation());
            tmpRot.SlerpStore(to.GetRotation(), amt);
            Keyframe c = new Keyframe(MathUtil.Lerp(GetTime(), to.GetTime(), amt), tmpVec, tmpRot);
            return c;
        }
    }
}