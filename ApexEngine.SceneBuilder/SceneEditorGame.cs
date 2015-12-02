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
using ApexEngine.Assets;
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
        private Geometry[] axisArrows = null;
        private CamModes camMode = CamModes.Freelook;

        public enum CamModes
        {
            Freelook,
            Grab,
            Rotate
        };

        public CamModes CamMode
        {
            get { return camMode; }
            set { camMode = value; }
        }

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
            axisArrows = new Geometry[3];
            Node axisNode = (Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\axis_arrows\\untitled.obj");
            axisArrows[0] = axisNode.GetChildGeom(0);
            axisArrows[1] = axisNode.GetChildGeom(1);
            axisArrows[2] = axisNode.GetChildGeom(2);

            foreach (Geometry g in axisArrows)
            {
                g.Material.SetValue(ApexEngine.Rendering.Material.MATERIAL_DEPTHTEST, false);
                //g.Material.SetValue(ApexEngine.Rendering.Material.MATERIAL_DEPTHMASK, false);
            } 

            InputManager.AddMouseEvent(new ApexEngine.Input.MouseEvent(OpenTK.Input.MouseButton.Left, false, new Action(() =>
            {
                if (!holding)
                {
                    Vector3f origin = Camera.Translation;
                    Vector3f unprojected = Camera.Unproject(InputManager.GetMouseX(), InputManager.GetMouseY());
                    unprojected.SubtractStore(cam.Translation);
                    Vector3f direction = unprojected.Multiply(1000f);
                    GameObject hitObject;
                    PhysicsWorld.Raycast(origin, direction, out hitObject);
                    objectHolding = hitObject;
                    holdTime = 0f;
                    holding = true;
                    if (hitObject != null)
                    {
                        foreach (Geometry g in axisArrows)
                        {
                            g.SetLocalTranslation(hitObject.GetLocalTranslation());
                            g.UpdateTransform();
                        }
                    }
                }
            })));
            InputManager.AddMouseEvent(new ApexEngine.Input.MouseEvent(OpenTK.Input.MouseButton.Left, true, new Action(() => 
            {
                holdTime = 0f;
                holding = false;
            })));
        }

        public override void Render()
        {
            if (renderPhysicsDebug)
            {
                GL.Disable(EnableCap.CullFace);
                GL.Enable(EnableCap.PolygonOffsetFill);
                GL.Enable(EnableCap.DepthTest);
                GL.PolygonOffset(2f, 2f);
                PhysicsWorld.DrawDebug();
                GL.Disable(EnableCap.PolygonOffsetFill);
                GL.Enable(EnableCap.CullFace);
            }
            if (camMode == CamModes.Grab)
                foreach (Geometry g in axisArrows)
                    g.Render(Environment, Camera);
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
                    Vector3f origin = Camera.Translation;
                    Vector3f direction = Camera.Direction.Multiply(1000f);
                    Vector3f rayHit;
                    GameObject hitObject;
                    PhysicsWorld.Raycast(origin, direction, out rayHit, out hitObject);
                    if (rayHit != null && objectHolding != null && hitObject != objectHolding)
                    {
                        if (camMode == CamModes.Grab)
                        {
                            objectHolding.SetLocalTranslation(rayHit);

                            foreach (Geometry g in axisArrows)
                            {
                                g.SetLocalTranslation(objectHolding.GetLocalTranslation());
                                g.UpdateTransform();
                            }
                        }
                    }
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
