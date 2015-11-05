using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Input;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Cameras
{
    public class DefaultCamera : PerspectiveCamera
    {
        float oldX = 0, oldY = 0, magX = 0, magY = 0;
        public DefaultCamera()
            : base()
        {
            this.fov = 45;
        }
        public DefaultCamera(float fov)
            : base()
        {
            this.fov = fov;
        }
        public override void UpdateCamera()
        {
            this.width = RenderManager.SCREEN_WIDTH;
            this.height = RenderManager.SCREEN_HEIGHT;
            Input();
        }
        protected void Input()
        {
            MouseInput(ApexEngine.Input.Input.GetMouseX(), ApexEngine.Input.Input.GetMouseY(), width/2, height/2);
            KeyboardInput();
        }
        protected void MouseInput(int x, int y, int halfWidth, int halfHeight)
        {
            float xDiff = (float)(oldX - x);
		    float yDiff = (float)(oldY - y);
		    magX = (float)x;
		    magY = (float)y;
		    oldX = magX;
		    oldY = magY;
		    magX *= -0.1f;
		    magY *= -0.1f;
		    Vector3f dirCrossY = direction.Cross(Vector3f.UNIT_Y);
		    Rotate(Vector3f.UNIT_Y, magX);
		    Rotate(dirCrossY, magY);
            ApexEngine.Input.Input.SetMousePosition(halfWidth, halfHeight);
        }
        protected void KeyboardInput()
        {
            if (ApexEngine.Input.Input.IsKeyDown(OpenTK.Input.Key.W))
            {
                // forward
                translation.x -= direction.x * 0.1f;
               // translation.y += direction.y * 0.1f;
                translation.z -= direction.z*0.1f;

            }
        }
    }
}
