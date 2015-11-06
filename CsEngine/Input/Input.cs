using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using ApexEngine.Rendering;
namespace ApexEngine.Input
{
    public class Input
    {
        private static List<KeyboardEvent> keyEvts = new List<KeyboardEvent>();
        private static List<MouseEvent> mouseEvts = new List<MouseEvent>();
        public static List<OpenTK.Input.Key> keysdown = new List<OpenTK.Input.Key>();
        public static List<OpenTK.Input.MouseButton> mousebtnsdown = new List<MouseButton>();
        public static void AddKeyboardEvent(KeyboardEvent e)
        {
            keyEvts.Add(e);
        }
        public static void AddMouseEvent(MouseEvent e)
        {
            mouseEvts.Add(e);
        }
        public static int GetMouseX()
        {
            return RenderManager.WINDOW_X - System.Windows.Forms.Cursor.Position.X + (RenderManager.SCREEN_WIDTH/2);//Mouse.GetState().X;
        }
        public static int GetMouseY()
        {
            return RenderManager.WINDOW_Y - System.Windows.Forms.Cursor.Position.Y + (RenderManager.SCREEN_HEIGHT / 2);//Mouse.GetState().Y;
        }
        public static void MouseButtonDown(OpenTK.Input.MouseButton btn)
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
        public static bool IsMouseButtonDown(OpenTK.Input.MouseButton btn)
        {
            return mousebtnsdown.Contains(btn);
        }
        public static void MouseButtonUp(OpenTK.Input.MouseButton btn)
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
        public static bool IsMouseButtonUp(OpenTK.Input.MouseButton btn)
        {
            return !IsMouseButtonDown(btn);
        }
        public static void KeyDown(OpenTK.Input.Key key)
        {
            if (!keysdown.Contains(key))
            {
                keysdown.Add(key);
            }
        }
        public static bool IsKeyDown(OpenTK.Input.Key key)
        {
            return keysdown.Contains(key);
        }
        public static void KeyUp(OpenTK.Input.Key key)
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
        public static bool IsKeyUp(OpenTK.Input.Key key)
        {
            return !IsKeyDown(key);
        }
        public static void SetMousePosition(int mx, int my)
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(RenderManager.WINDOW_X + mx , RenderManager.WINDOW_Y + my );
        }
        public static void SetMouseVisible(bool isVisible)
        {
            if (!isVisible)
                System.Windows.Forms.Cursor.Hide();
            else
                System.Windows.Forms.Cursor.Show();
        }
    }
}
