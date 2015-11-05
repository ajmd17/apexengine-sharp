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
        public static List<OpenTK.Input.Key> keysdown = new List<OpenTK.Input.Key>();
        public static void AddKeyboardEvent(KeyboardEvent e)
        {
            keyEvts.Add(e);
        }
        public static int GetMouseX()
        {
            return RenderManager.WINDOW_X - System.Windows.Forms.Cursor.Position.X + (RenderManager.SCREEN_WIDTH/2);//Mouse.GetState().X;
        }
        public static int GetMouseY()
        {
            return RenderManager.WINDOW_Y - System.Windows.Forms.Cursor.Position.Y + (RenderManager.SCREEN_HEIGHT / 2);//Mouse.GetState().Y;
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
         //   OpenTK.Input.Mouse.SetPosition(RenderManager.WINDOW_X + mx, RenderManager.WINDOW_Y + my);
        }
    }
}
