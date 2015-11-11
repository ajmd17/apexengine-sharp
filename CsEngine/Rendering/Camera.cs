using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering
{
    public abstract class Camera
    {
        protected Vector3f translation = new Vector3f(0, 0, 0);
        protected Vector3f direction = new Vector3f(0, 0, 1);
        protected Vector3f up = new Vector3f(0, 1, 0);
        protected int width = 512, height = 512;
        protected float near = 0.0f, far = 150.0f;
        protected Matrix4f viewMatrix = new Matrix4f(), projMatrix = new Matrix4f();
        private Vector3f tmpVec = new Vector3f();
        public Camera()
        {

        }
        public Camera(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public Vector3f Translation
        {
            get
            {
                tmpVec.Set(-translation.x, -translation.y, -translation.z);
                return tmpVec;
            }
            set
            {
                translation.Set(value.Multiply(-1));
            }
        }
        public Vector3f Direction
        {
            get { return direction; }
            set { direction.Set(value); }
        }
        public Vector3f Up
        {
            get { return up; }
            set { up.Set(value); }
        }
        public void Rotate(Vector3f axis, float angle)
        {
            direction.RotateStore(axis, angle);
            direction.NormalizeStore();
        }
        public void LookAtDirection(Vector3f dir)
        {
            direction.Set(dir);
        }
        public void LookAt(Vector3f location)
        {
            direction.Set(location.Subtract(translation));
        }
        public void Update()
        {
            UpdateCamera();
            UpdateMatrix();
        }
        public Matrix4f GetViewMatrix()
        {
            return viewMatrix;
        }
        public Matrix4f GetProjectionMatrix()
        {
            return projMatrix;
        }
        public abstract void UpdateMatrix();
        public abstract void UpdateCamera();
    }
}
