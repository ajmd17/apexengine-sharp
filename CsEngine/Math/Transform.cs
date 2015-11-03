using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsEngine.Math
{
    public class Transform
    {
        protected Vector3f translation;
        protected Quaternion rotation;
        protected Vector3f scale;
        protected Matrix4f matrix;
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
            Matrix4f transMat = MatrixUtil.SetToTranslation(translation);
            Matrix4f rotMat = MatrixUtil.SetToRotation(rotation);
            Matrix4f scaleMat = MatrixUtil.SetToScaling(scale);
            Matrix4f rotScale = rotMat.Multiply(scaleMat);
            this.matrix = rotScale.Multiply(transMat);
        }
        public Matrix4f GetMatrix()
        {
            return matrix;
        }
        void SetTranslation(Vector3f v)
        {
            translation.Set(v);
            UpdateMatrix();
        }
        Vector3f GetTranslation()
        {
            return translation;
        }
        void SetRotation(Quaternion q)
        {
            rotation.Set(q);
            UpdateMatrix();
        }
        Quaternion GetRotation()
        {
            return rotation;
        }
        void SetScale(Vector3f v)
        {
            scale.Set(v);
            UpdateMatrix();
        }
        Vector3f GetScale()
        {
            return scale;
        }
    }
}
