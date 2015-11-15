using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine;
namespace ApexEditor
{
    public class SceneEditorGame : Game
    {
        public override void Init()
        {
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
