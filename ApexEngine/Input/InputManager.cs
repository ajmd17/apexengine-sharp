using System.Collections.Generic;

namespace ApexEngine.Input
{
    public class InputManager
    {
        public int SCREEN_WIDTH, SCREEN_HEIGHT, WINDOW_X, WINDOW_Y, MOUSE_X, MOUSE_Y;
        private List<KeyboardEvent> keyEvts = new List<KeyboardEvent>();
        private List<MouseEvent> mouseEvts = new List<MouseEvent>();
        public List<KeyboardKey> keysdown = new List<KeyboardKey>();
        public List<MouseButton> mousebtnsdown = new List<MouseButton>();

        //TODO: add more

        public enum MouseButton
        {
            None,

            Left,
            Right,
            Middle
        }


        //TODO: add more
        public enum KeyboardKey
        {
            None,

            LeftCtrl,
            RightCtrl,
            LeftAlt,
            RightAlt,
            LeftShift,
            RightShift,

            Tab,
            Enter,
            CapsLock,
            Backspace,
            Space,

            LeftArrow,
            RightArrow,
            UpArrow,
            DownArrow,

            Num0,
            Num1,
            Num2,
            Num3,
            Num4,
            Num5,
            Num6,
            Num7,
            Num8,
            Num9,


            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z
        }

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

        public void MouseButtonDown(MouseButton btn)
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

        public bool IsMouseButtonDown(MouseButton btn)
        {
            return mousebtnsdown.Contains(btn);
        }

        public void MouseButtonUp(MouseButton btn)
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

        public bool IsMouseButtonUp(MouseButton btn)
        {
            return !IsMouseButtonDown(btn);
        }

        public void KeyDown(KeyboardKey key)
        {
            if (!keysdown.Contains(key))
            {
                keysdown.Add(key);
            }
        }

        public bool IsKeyDown(KeyboardKey key)
        {
            return keysdown.Contains(key);
        }

        public void KeyUp(KeyboardKey key)
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

        public bool IsKeyUp(KeyboardKey key)
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