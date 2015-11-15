using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.Cameras
{
    public class OrthoCamera : Camera
    {
        protected float left, right, bottom, top;
        
        public float Left
        {
            get { return left; }
            set { left = value; }
        }

        public float Right
        {
            get { return right; }
            set { right = value; }
        }

        public float Top
        {
            get { return top; }
            set { top = value; }
        }

        public float Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        public OrthoCamera(float left, float right, float bottom, float top, float near, float far)
        {
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.top = top;
            this.near = near;
            this.far = far;
        }

        public override void UpdateCamera()
        {
        }

        public override void UpdateMatrix()
        {
            this.projMatrix.SetToOrtho(left, right, bottom, top, near, far);
        }
    }
}
