using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public class Transform
    {
        protected Vector3f translation;
        protected Quaternion rotation;
        protected Vector3f scale;
        protected Matrix4f matrix, transMat = new Matrix4f(), 
                                   rotMat = new Matrix4f(), 
                                   scaleMat = new Matrix4f(),
                                   rotScale = new Matrix4f();
        public Transform()
        {
            translation = new Vector3f(0, 0, 0);
            rotation = new Quaternion(0, 0, 0, 1);
            scale = new Vector3f(1, 1, 1);
        }
        public Transform(Transform other)
        {
            this.translation = other.translation;
            this.rotation = other.rotation;
            this.scale = other.scale;
        }
        protected void UpdateMatrix()
        {
            transMat.SetToTranslation(translation);
            rotMat.SetToRotation(rotation);
            scaleMat.SetToScaling(scale);
            rotScale = rotMat.Multiply(scaleMat);
            this.matrix = rotScale.Multiply(transMat);
        }
        public Matrix4f GetMatrix()
        {
            return matrix;
        }
        public void SetTranslation(Vector3f v)
        {
            translation.Set(v);
            UpdateMatrix();
        }
        public Vector3f GetTranslation()
        {
            return translation;
        }
        public void SetRotation(Quaternion q)
        {
            rotation.Set(q);
            UpdateMatrix();
        }
        public Quaternion GetRotation()
        {
            return rotation;
        }
        public void SetScale(Vector3f v)
        {
            scale.Set(v);
            UpdateMatrix();
        }
        public Vector3f GetScale()
        {
            return scale;
        }
    }
}
