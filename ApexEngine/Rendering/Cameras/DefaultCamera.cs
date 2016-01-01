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
        protected CameraMode camMode = CameraMode.FPSMode;
        protected bool mouseCaptured = false, mouseDragging = false, keysEnabled = true;
        private Vector3f dirCrossY = new Vector3f();
        private float oldX = 0, oldY = 0, magX = 0, magY = 0;
        

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
            KeyboardEvent evt_mouseRelease = new KeyboardEvent(InputManager.KeyboardKey.LeftAlt, () =>
            {
                CenterMouse();
                mouseCaptured = !mouseCaptured;

              //  if (camMode == CameraMode.FPSMode)
             //       inputManager.SetMouseVisible(!mouseCaptured);
            });
            inputManager.AddKeyboardEvent(evt_mouseRelease);
            MouseEvent evt_mouseClick = new MouseEvent(ApexEngine.Input.InputManager.MouseButton.Left, false, () =>
            {
                //    if (camMode == CameraMode.FPSMode)
                CenterMouse();
                mouseDragging = true;
                mouseCaptured = true;
            /*    if (camMode == CameraMode.DragMode)
                    inputManager.SetMouseVisible(!mouseDragging);
                else if (camMode == CameraMode.FPSMode)
                    inputManager.SetMouseVisible(!mouseCaptured);*/
            });
            inputManager.AddMouseEvent(evt_mouseClick);
            MouseEvent evt_mouseUp = new MouseEvent(ApexEngine.Input.InputManager.MouseButton.Left, true, () =>
            {
                //   if (camMode == CameraMode.FPSMode)
                CenterMouse();
                mouseDragging = false;
                /*if (camMode == CameraMode.DragMode)
                    inputManager.SetMouseVisible(!mouseDragging);
                else if (camMode == CameraMode.FPSMode)
                    inputManager.SetMouseVisible(!mouseCaptured);*/
            });
            inputManager.AddMouseEvent(evt_mouseUp);
        }

        public bool KeysEnabled
        {
            get { return keysEnabled; }
            set { keysEnabled = value; }
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
            if (enabled)
                Input();
        }

        protected void Input()
        {
            MouseInput(inputManager.GetMouseX(), inputManager.GetMouseY(), width / 2, height / 2);
            if (keysEnabled)
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
                dirCrossY.Set(direction);
                dirCrossY.CrossStore(Vector3f.UnitY);
                Rotate(Vector3f.UnitY, magX);
                Rotate(dirCrossY, magY);
                inputManager.SetMousePosition(halfWidth, halfHeight);
            }
            else if (camMode == CameraMode.DragMode && mouseDragging)
            {
                magX = (float)x;
                magY = (float)y;
                magX *= -0.1f;
                magY *= -0.1f;
                dirCrossY.Set(direction);
                dirCrossY.CrossStore(Vector3f.UnitY);
                Rotate(Vector3f.UnitY, magX);
                Rotate(dirCrossY, magY);
                inputManager.SetMousePosition(halfWidth, halfHeight);
            }
            oldX = x;
            oldY = y;
        }

        protected void KeyboardInput()
        {
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.W))
            {
                // forwards
                translation.x += direction.x * 0.1f;
                translation.y += direction.y * 0.1f;
                translation.z += direction.z * 0.1f;
            }
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.S))
            {
                // backwards
                translation.x += direction.x * -0.1f;
                translation.y += direction.y * -0.1f;
                translation.z += direction.z * -0.1f;
            }
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.A))
            {
                // left
                translation.x += 0.1f * (float)System.Math.Sin(MathUtil.ToRadians(yaw + 90));
                translation.z -= 0.1f * (float)System.Math.Cos(MathUtil.ToRadians(yaw + 90));
            }
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.D))
            {
                // left
                translation.x += 0.1f * (float)System.Math.Sin(MathUtil.ToRadians(yaw - 90));
                translation.z -= 0.1f * (float)System.Math.Cos(MathUtil.ToRadians(yaw - 90));
            }
        }
    }
}