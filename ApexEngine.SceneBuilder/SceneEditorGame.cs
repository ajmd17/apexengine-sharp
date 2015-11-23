using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using ApexEngine;
using ApexEngine.Math;
using ApexEngine.Rendering.PostProcess;

namespace ApexEditor
{
    public class SceneEditorGame : Game
    {
        DepthVisualizerFilter depthVisualizer;
        public override void Init()
        {
            depthVisualizer = new DepthVisualizerFilter();
            InputManager.AddMouseEvent(new ApexEngine.Input.MouseEvent(OpenTK.Input.MouseButton.Left, false, new Action(() => 
            {
                float x = -1.0f * ((float)InputManager.GetMouseX() / InputManager.SCREEN_WIDTH);
                float y = 1.0f * ((float)InputManager.GetMouseY() / InputManager.SCREEN_HEIGHT);

                // little hacking to get depth from screen
                float z = 0f;
                GL.ReadPixels<float>(-1*(InputManager.GetMouseX() - (InputManager.SCREEN_WIDTH / 2)), 
                                  InputManager.SCREEN_HEIGHT - -1*((InputManager.GetMouseY() - (InputManager.SCREEN_HEIGHT / 2))), 
                                  1, 1, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Float, ref z);


                Vector3f pos_screenSpace = new Vector3f(x, y, z);
                Console.WriteLine(pos_screenSpace);
            }
            )));
        }

        public override void Render()
        {
        }

        public override void Update()
        {
            if (InputManager.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
            {
                if (InputManager.GetMouseY() > 0)
                {
                    cam.Translation.AddStore(cam.Direction.Multiply(0.5f));
                }
                else
                {
                    cam.Translation.AddStore(cam.Direction.Multiply(-0.5f));
                }
            }
        }
    }
}
