using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using ApexEngine;
using ApexEngine.Math;
using ApexEngine.Rendering.PostProcess;
using ApexEngine.Scene;
using ApexEngine.Rendering.Shadows;

namespace ApexEditor
{
    public class SceneEditorGame : Game
    {
        private float holdTime = 0f;
        private bool holding = false;
        private bool renderPhysicsDebug = true;
        private GameObject objectHolding = null;

        public bool RenderDebug
        {
            get { return renderPhysicsDebug; }
            set { renderPhysicsDebug = value; }
        }

        public override void Init()
        {
            ShadowMappingComponent smc;
            RenderManager.AddComponent(smc = new ShadowMappingComponent(cam, Environment));
            smc.RenderMode = ShadowMappingComponent.ShadowRenderMode.Forward;

            InputManager.AddMouseEvent(new ApexEngine.Input.MouseEvent(OpenTK.Input.MouseButton.Left, false, new Action(() =>
            {
                holdTime = 0f;
                holding = true;
            })));
            InputManager.AddMouseEvent(new ApexEngine.Input.MouseEvent(OpenTK.Input.MouseButton.Left, true, new Action(() => 
            {
                if (holdTime < 2f)
                {
                    Vector3f origin = Camera.Translation;
                    Vector3f unprojected = Camera.Unproject(InputManager.GetMouseX(), InputManager.GetMouseY());
                    unprojected.SubtractStore(cam.Translation);
                    Vector3f direction = unprojected.Multiply(1000f);
                    GameObject hitObject;
                    PhysicsWorld.Raycast(origin, direction, out hitObject);
                    if (hitObject != null)
                    {
                        objectHolding = hitObject;
                    }
                }
                holdTime = 0f;
                holding = false;
            })));
        }

        public override void Render()
        {
            if (renderPhysicsDebug)
                PhysicsWorld.DrawDebug();
        }

        public override void Update()
        {
            if (holding)
                holdTime += 0.1f;
            if (holding && holdTime > 2f)
            {
                cam.Enabled = true;
                if (objectHolding != null)
                {

                }
            }
            else
                cam.Enabled = false;

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
