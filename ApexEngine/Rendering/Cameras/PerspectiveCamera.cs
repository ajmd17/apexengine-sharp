using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Cameras
{
    public class PerspectiveCamera : Camera
    {
        protected Quaternion rotation = new Quaternion();
        protected float fov = 45f, yaw, roll, pitch;
        public PerspectiveCamera() 
            : base()
        {

        }
        public PerspectiveCamera(float fov, int width, int height)
            : base(width, height)
        {
            this.fov = fov;
        }
        public override void UpdateMatrix()
        {
            rotation.SetToLookAt(direction, up);
            viewMatrix.SetToLookAt(translation, translation.Add(direction), up);
            projMatrix.SetToProjection(fov, width, height, 0.5f, 100f);
            yaw = rotation.GetYaw();
            roll = rotation.GetRoll();
            pitch = rotation.GetPitch();
        }
        public override void UpdateCamera()
        {
        }
        public float GetYaw()
        {
            return yaw;
        }
        public float GetRoll()
        {
            return roll;
        }
        public float GetPitch()
        {
            return pitch;
        }
    }
}
