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
using ApexEngine.Rendering.Util;

namespace ApexEditor
{
    public class SceneEditorGame : Game
    {
        private float holdTime = 0f;
        private bool holding = false;
        private bool renderPhysicsDebug = true;
        public GameObject objectHolding = null;
        private bool centered = false;
        private Geometry[] axisArrows = null;
        private bool movingX = false, movingY = false, movingZ = false;
        private CamModes camMode = CamModes.Freelook;
        public Vector3f offsetLoc = new Vector3f();
        public int lastMouseX = 0, lastMouseY = 0;
        private bool boundingBoxes = false;

        public enum CamModes
        {
            Freelook,
            Grab,
            Rotate
        };

        public SceneEditorGame() : base(new ApexEngine.Rendering.OpenGL.GLRenderer())
        {

        }

        public bool BoundingBoxes
        {
            get { return boundingBoxes; }
            set { boundingBoxes = value; }
        }

        public bool MovingX
        {
            get { return movingX; }
            set { movingX = value; }
        }

        public bool MovingY
        {
            get { return movingY; }
            set { movingY = value; }
        }

        public bool MovingZ
        {
            get { return movingZ; }
            set { movingZ = value; }
        }

        public bool Centered
        {
            get { return centered; }
            set { centered = value; }
        }

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

            InputManager.AddMouseEvent(new ApexEngine.Input.MouseEvent(ApexEngine.Input.InputManager.MouseButton.Left, false, new Action(() =>
            {
                if (!holding)
                {
                    Ray ray = cam.GetCameraRay(InputManager.GetMouseX(), InputManager.GetMouseY());

                    Dictionary<Vector3f, GameObject> intersections = new Dictionary<Vector3f, GameObject>();
                    Vector3f closestIntersection = new Vector3f(float.MaxValue);
                    float minDistance = float.MaxValue;
                    List<GameObject> objectList = ApexEngine.Rendering.Util.RenderUtil.GatherObjects(rootNode);
                    foreach (GameObject g in objectList)
                    {
                        if (objectHolding == null || g != objectHolding)
                        {
                            if (g.HasController(typeof(ApexEngine.Scene.Physics.RigidBodyControl)))
                            {
                                Vector3f intersection = g.GetWorldBoundingBox().Intersect(ray);
                                if (intersection != null && !intersections.ContainsKey(intersection))
                                {
                                    intersections.Add(intersection, g);
                                }
                            }
                        }
                    }

                    if (intersections.Count == 0)
                        objectHolding = null;

                    foreach (Vector3f i in intersections.Keys)
                    {
                        float dist = i.Distance(Camera.Translation);
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            closestIntersection.Set(i);
                        }
                    }

                    GameObject hitObject = null;
                    
                    if (intersections.TryGetValue(closestIntersection, out hitObject))
                    {
                        if (hitObject != null)
                        {
                            if (hitObject.HasController(typeof(ApexEngine.Scene.Physics.RigidBodyControl)))
                            {
                                // needs rigid body control or else the node is "locked"
                                // also we need this to test for intersection in the physics world
                                objectHolding = hitObject;
                                foreach (Geometry g in axisArrows)
                                {
                                    g.SetLocalTranslation((!centered ? objectHolding.GetLocalTranslation() : objectHolding.GetLocalTranslation().Add(objectHolding.GetLocalBoundingBox().Center.Subtract(new Vector3f(0f, objectHolding.GetLocalBoundingBox().Center.Y, 0f)))));
                                    g.UpdateTransform();
                                }
                            }
                            else
                            {
                                objectHolding = null;
                            }
                        }
                    }
                    holdTime = 0f;
                    holding = true;
                    /*
                    
                    GameObject hitObject;
                    PhysicsWorld.Raycast(origin, direction, out hitObject);
                    objectHolding = hitObject;
                    holdTime = 0f;
                    holding = true;
                    if (hitObject != null)
                    {
                        foreach (Geometry g in axisArrows)
                        {
                            g.SetLocalTranslation((!centered ? objectHolding.GetLocalTranslation() : objectHolding.GetLocalTranslation().Add(objectHolding.GetLocalBoundingBox().Center.Subtract(new Vector3f(0f, objectHolding.GetLocalBoundingBox().Center.Y, 0f)))));
                            g.UpdateTransform();
                        }
                    }*/
                }
            })));
            InputManager.AddMouseEvent(new ApexEngine.Input.MouseEvent(ApexEngine.Input.InputManager.MouseButton.Left, true, new Action(() => 
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
            if (boundingBoxes)
            {
                for (int i = 0; i < RenderManager.GeometryList.Count; i++)
                {
                    RenderBoundingBox(RenderManager.GeometryList[i].GetWorldBoundingBox());
                }
            }
            if (camMode == CamModes.Grab)
                foreach (Geometry g in axisArrows)
                    g.Render(Environment, Camera);
        }

        private void RenderBoundingBox(BoundingBox boundingBox)
        {
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(15.0f, 100.0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(cam.ViewMatrix.GetInvertedValues());
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(cam.ProjectionMatrix.GetInvertedValues());
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.LineWidth(1.5f);
            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(1.0f, 0.0f, 0.0f);
            RenderUtil.RenderBoundingBox(boundingBox);
            

            GL.End();
            GL.Disable(EnableCap.PolygonOffsetLine);
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
                            if (!centered)
                            {
                                objectHolding.SetLocalTranslation(rayHit);
                                foreach (Geometry g in axisArrows)
                                {
                                    g.SetLocalTranslation(objectHolding.GetLocalTranslation());
                                    g.UpdateTransform();
                                }
                            }
                            else if (centered)
                            {
                                Vector3f offset = new Vector3f(objectHolding.GetLocalBoundingBox().Center);
                                objectHolding.SetLocalTranslation(rayHit.Subtract(offset));
                                Console.WriteLine(objectHolding.GetLocalBoundingBox().Center);

                                foreach (Geometry g in axisArrows)
                                {
                                    g.SetLocalTranslation(objectHolding.GetLocalTranslation().Add(offset));
                                    g.UpdateTransform();
                                }
                            }
                        }
                    }
                }
            }
            else
                cam.Enabled = false;

            if (camMode == CamModes.Grab)
            {
                float scalar = 25.0f;
               /* if (movingX)
                {
                    if (objectHolding != null)
                    {
                        Vector3f currentTrans = new Vector3f(objectHolding.GetWorldTranslation());
                        Vector3f unprojected = Camera.Unproject(InputManager.GetMouseX(), InputManager.GetMouseY());
                        unprojected.SubtractStore(cam.Translation);
                        // currentTrans.X = (offsetLoc.x + startMouse - InputManager.GetMouseX()) / 20f;

                        unprojected.MultiplyStore(scalar);

                        currentTrans.x = unprojected.x;

                        foreach (Geometry g in axisArrows)
                        {
                            g.SetLocalTranslation(unprojected);
                            g.UpdateTransform();
                        }

                        objectHolding.SetLocalTranslation(unprojected);
                    }

                }
                else if (movingY)
                {
                    if (objectHolding != null)
                    {
                        Vector3f currentTrans = new Vector3f(objectHolding.GetWorldTranslation());
                        Vector3f unprojected = Camera.Unproject(InputManager.GetMouseX(), InputManager.GetMouseY());
                        unprojected.SubtractStore(cam.Translation);
                        // currentTrans.Y = (offsetLoc.y + startMouse - InputManager.GetMouseX())/20f;//offsetLoc.y + unprojected.Y * scalar;

                        unprojected.MultiplyStore(scalar);

                        currentTrans.y = unprojected.y;

                        foreach (Geometry g in axisArrows)
                        {
                            g.SetLocalTranslation(unprojected);
                            g.UpdateTransform();
                        }

                        objectHolding.SetLocalTranslation(unprojected);
                    }

                }
                else if (movingZ)
                {
                    if (objectHolding != null)
                    {
                        Vector3f currentTrans = new Vector3f(objectHolding.GetWorldTranslation());
                        Vector3f unprojected = Camera.Unproject(InputManager.GetMouseX(), InputManager.GetMouseY());
                        unprojected.SubtractStore(cam.Translation);

                        unprojected.MultiplyStore(scalar);

                        currentTrans.z = unprojected.z;

                        foreach (Geometry g in axisArrows)
                        {
                            g.SetLocalTranslation(unprojected);
                            g.UpdateTransform();
                        }

                        objectHolding.SetLocalTranslation(unprojected);
                    }

                }*/
            }

            if (InputManager.IsMouseButtonDown(ApexEngine.Input.InputManager.MouseButton.Right))
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
