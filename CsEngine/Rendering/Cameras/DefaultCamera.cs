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
        protected enum CameraMode
        {
            FPSMode,
            DragMode,
            ChaseMode
        };
        private InputManager inputManager = null;
        protected CameraMode camMode = CameraMode.DragMode;
        protected bool mouseCaptured = false, mouseDragging = false;
        float oldX = 0, oldY = 0, magX = 0, magY = 0;
        public DefaultCamera(InputManager inputManager)
            : base()
        {
            this.inputManager = inputManager;
        }
        public DefaultCamera(InputManager inputManager, float fov)
            : base()
        {
            this.fov = fov;
            this.inputManager = inputManager;
            KeyboardEvent evt_mouseRelease = new KeyboardEvent(OpenTK.Input.Key.AltLeft, () =>
            {
                    CenterMouse();
                    mouseCaptured = !mouseCaptured;

                    if (camMode == CameraMode.FPSMode)
                        inputManager.SetMouseVisible(!mouseCaptured);
                });
            inputManager.AddKeyboardEvent(evt_mouseRelease);
            MouseEvent evt_mouseClick = new MouseEvent(OpenTK.Input.MouseButton.Left, false, () =>
            {
                CenterMouse();
                mouseDragging = true;
                mouseCaptured = true;
             //   if (camMode == CameraMode.DragMode)
                    inputManager.SetMouseVisible(!mouseDragging);
               // else if (camMode == CameraMode.FPSMode)
               //     ApexEngine.Input.Input.SetMouseVisible(!mouseCaptured);
            });
           inputManager.AddMouseEvent(evt_mouseClick);
            MouseEvent evt_mouseUp = new MouseEvent(OpenTK.Input.MouseButton.Left, true, () =>
            {
                CenterMouse();
                mouseDragging = false;
                if (camMode == CameraMode.DragMode)
                    inputManager.SetMouseVisible(!mouseDragging);
              //  else if (camMode == CameraMode.FPSMode)
               //     ApexEngine.Input.Input.SetMouseVisible(!mouseCaptured);
            });
            inputManager.AddMouseEvent(evt_mouseUp);
        }
        protected void CenterMouse()
        {
            if (enabled)
            {
                int halfWidth = inputManager.SCREEN_WIDTH / 2;
                int halfHeight = inputManager.SCREEN_HEIGHT / 2;
                inputManager.SetMousePosition(halfWidth, halfHeight);
            }
        }
        public override void UpdateCamera()
        {
            this.width = inputManager.SCREEN_WIDTH;
            this.height = inputManager.SCREEN_HEIGHT;
            Input();
        }
        protected void Input()
        {
            // mouseDragging = ApexEngine.Input.Input.IsMouseButtonDown(OpenTK.Input.MouseButton.Left);
            Console.WriteLine(inputManager.GetMouseX() + ", " + inputManager.GetMouseY());
            MouseInput(inputManager.GetMouseX(), inputManager.GetMouseY(), width/2, height/2);
            KeyboardInput();
        }
        protected void MouseInput(int x, int y, int halfWidth, int halfHeight)
        {
            if (camMode == CameraMode.FPSMode && mouseCaptured)
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
                inputManager.SetMousePosition(halfWidth, halfHeight);
            }
            else if (camMode == CameraMode.DragMode && mouseDragging)
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
                inputManager.SetMousePosition(halfWidth, halfHeight);
            }
        }
        protected void KeyboardInput()
        {
            if (inputManager.IsKeyDown(OpenTK.Input.Key.W))
            {
                // forwards
                translation.x += direction.x * 0.1f;
                translation.y += direction.y * 0.1f;
                translation.z += direction.z * 0.1f;
            }
            if (inputManager.IsKeyDown(OpenTK.Input.Key.S))
            {
                // backwards
                translation.x += direction.x * -0.1f;
                translation.y += direction.y * -0.1f;
                translation.z += direction.z * -0.1f;

            }
            if (inputManager.IsKeyDown(OpenTK.Input.Key.A))
            {
                // left
               translation.x += 0.1f * (float)System.Math.Sin(MathUtil.ToRadians(yaw + 90));
			   translation.z -= 0.1f * (float)System.Math.Cos(MathUtil.ToRadians(yaw + 90));
            }
            if (inputManager.IsKeyDown(OpenTK.Input.Key.D))
            {
                // left
                translation.x += 0.1f * (float)System.Math.Sin(MathUtil.ToRadians(yaw - 90));
                translation.z -= 0.1f * (float)System.Math.Cos(MathUtil.ToRadians(yaw - 90));
            }
        }
    }
}
