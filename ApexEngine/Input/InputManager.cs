using OpenTK.Input;
using System.Collections.Generic;

namespace ApexEngine.Input
{
    public class InputManager
    {
        public int SCREEN_WIDTH, SCREEN_HEIGHT, WINDOW_X, WINDOW_Y, MOUSE_X, MOUSE_Y;
        private List<KeyboardEvent> keyEvts = new List<KeyboardEvent>();
        private List<MouseEvent> mouseEvts = new List<MouseEvent>();
        public List<OpenTK.Input.Key> keysdown = new List<OpenTK.Input.Key>();
        public List<OpenTK.Input.MouseButton> mousebtnsdown = new List<MouseButton>();

        public void AddKeyboardEvent(KeyboardEvent e)
        {
            keyEvts.Add(e);
        }

        public void AddMouseEvent(MouseEvent e)
        {
            mouseEvts.Add(e);
        }

        public int GetMouseX()
        {
            return MOUSE_X; 
        }

        public int GetMouseY()
        {
            return MOUSE_Y;
        }

        public void MouseButtonDown(OpenTK.Input.MouseButton btn)
        {
            if (!mousebtnsdown.Contains(btn))
            {
                for (int i = 0; i < mouseEvts.Count; i++)
                {
                    if (!mouseEvts[i].mouseUpEvt && mouseEvts[i].btn == btn)
                    {
                        mouseEvts[i].evt();
                    }
                }
                mousebtnsdown.Add(btn);
            }
        }

        public bool IsMouseButtonDown(OpenTK.Input.MouseButton btn)
        {
            return mousebtnsdown.Contains(btn);
        }

        public void MouseButtonUp(OpenTK.Input.MouseButton btn)
        {
            if (mousebtnsdown.Contains(btn))
            {
                for (int i = 0; i < mouseEvts.Count; i++)
                {
                    if (mouseEvts[i].mouseUpEvt && mouseEvts[i].btn == btn)
                    {
                        mouseEvts[i].evt();
                    }
                }
                mousebtnsdown.Remove(btn);
            }
        }

        public bool IsMouseButtonUp(OpenTK.Input.MouseButton btn)
        {
            return !IsMouseButtonDown(btn);
        }

        public void KeyDown(OpenTK.Input.Key key)
        {
            if (!keysdown.Contains(key))
            {
                keysdown.Add(key);
            }
        }

        public bool IsKeyDown(OpenTK.Input.Key key)
        {
            return keysdown.Contains(key);
        }

        public void KeyUp(OpenTK.Input.Key key)
        {
            if (keysdown.Contains(key))
            {
                for (int i = 0; i < keyEvts.Count; i++)
                {
                    if (keyEvts[i].key == key)
                    {
                        keyEvts[i].evt();
                    }
                }
                keysdown.Remove(key);
            }
        }

        public bool IsKeyUp(OpenTK.Input.Key key)
        {
            return !IsKeyDown(key);
        }

        public void SetMousePosition(int mx, int my)
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(WINDOW_X + mx, WINDOW_Y + my);
        }

        public void SetMouseVisible(bool isVisible)
        {
            if (!isVisible)
                System.Windows.Forms.Cursor.Hide();
            else
                System.Windows.Forms.Cursor.Show();
        }
    }
}