using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Cameras;
using OpenTK.Graphics.OpenGL;

namespace ApexEngine.Rendering.Shadows
{
    public class ShadowMapRenderer
    {
        protected RenderManager rm;
        protected OrthoCamera s_cam;
        protected Framebuffer fbo;
        protected Texture tex;
        protected Camera v_cam;
        protected float splitDist;
        protected BoundingBox bb = new BoundingBox();
        protected Vector3f[] frustumCornersLS = new Vector3f[8];
        protected Vector3f[] frustumCornersWS = new Vector3f[8];
        protected Vector3f maxes = new Vector3f(float.MinValue, float.MinValue, float.MinValue);
        protected Vector3f mins = new Vector3f(float.MaxValue, float.MaxValue, float.MaxValue);
        protected Vector3f centerPos = new Vector3f();
        protected Vector3f lightDirection = new Vector3f(-1);
        protected Matrix4f newView = new Matrix4f();
        protected float[] splits;
        protected int shadowMapWidth, shadowMapHeight;
        private Matrix4f viewProjectionMatrix = new Matrix4f();

        private Vector3f[] fPoints = new Vector3f[]{new Vector3f(),
                                     new Vector3f(),
                                     new Vector3f(),
                                     new Vector3f(),
                                     new Vector3f(),
                                     new Vector3f(),
                                     new Vector3f(),
                                     new Vector3f()};

        private Vector3f tmpVec = new Vector3f();
        private Environment environment;

        public ShadowMapRenderer(Environment environment, int[] size, float[] splits, RenderManager rm, Camera viewcam, float splitDistance)
        {
            this.rm = rm;
            this.environment = environment;
            this.v_cam = viewcam;
            this.splitDist = splitDistance;
            Vector3f ld = environment.DirectionalLight.Direction.Normalize();
            this.lightDirection = new Vector3f(-ld.x, -ld.y, -ld.z);
            this.splits = splits;
            this.shadowMapWidth = size[0];
            this.shadowMapHeight = size[1];

            for (int i = 0; i < 8; i++)
            {
                frustumCornersLS[i] = new Vector3f();
                frustumCornersWS[i] = new Vector3f();
            }
        }

        public Matrix4f ViewProjectionMatrix
        {
            get { return s_cam.ViewProjectionMatrix; }
            set { s_cam.ViewProjectionMatrix.Set(value); }
        }

        public Vector3f LightDirection
        {
            get { return lightDirection; }
            set { lightDirection.Set(value); }
        }

        public Texture ShadowMap
        {
            get { return tex; }
        }

        private void Transform(Vector3f[] inVec, Vector3f[] outVec, Matrix4f mat)
        {
            for (int i = 0; i < inVec.Length; i++)
            {
                outVec[i].Set(inVec[i]);
                outVec[i].MultiplyStore(mat);
            }
        }

        public void Init()
        {
            fbo = new Framebuffer(shadowMapWidth, shadowMapHeight);
            s_cam = new OrthoCamera(-5, 5, -5, 5, -5, 5);
            fbo.Init();
            s_cam.Far = splitDist;
        }

        private void UpdateFrustumPoints(Vector3f[] pts)
        {
            bb.Clear();
            bb.Extend(MathUtil.Round(v_cam.Translation.Add(new Vector3f(s_cam.Far, s_cam.Far, s_cam.Far))));
            bb.Extend(MathUtil.Round(v_cam.Translation.Add(new Vector3f(-s_cam.Far, -s_cam.Far, -s_cam.Far))));
            for (int i = 0; i < bb.Corners.Length; i++)
            {
                pts[i].Set(bb.Corners[i]);
            }
        }

        public void Update()
        {
            UpdateFrustumPoints(fPoints);
            centerPos.Set(0, 0, 0);
            frustumCornersWS = fPoints;
            for (int i = 0; i < 8; i++)
            {
                centerPos.AddStore(frustumCornersWS[i]);
            }
            centerPos.DivideStore(8f);
            newView.SetToLookAt(tmpVec.Set(centerPos).SubtractStore(lightDirection), centerPos, Vector3f.UNIT_Y);
            Transform(frustumCornersWS, frustumCornersLS, newView);
            maxes.Set(float.MinValue, float.MinValue, float.MinValue);
            mins.Set(float.MaxValue, float.MaxValue, float.MaxValue);
            for (int i = 0; i < frustumCornersLS.Length; i++)
            {
                if (frustumCornersLS[i].x > maxes.x)
                {
                    maxes.x = frustumCornersLS[i].x;
                }
                else if (frustumCornersLS[i].x < mins.x)
                {
                    mins.x = frustumCornersLS[i].x;
                }
                if (frustumCornersLS[i].y > maxes.y)
                {
                    maxes.y = frustumCornersLS[i].y;
                }
                else if (frustumCornersLS[i].y < mins.y)
                {
                    mins.y = frustumCornersLS[i].y;
                }
                if (frustumCornersLS[i].z > maxes.z)
                {
                    maxes.z = frustumCornersLS[i].z;
                }
                else if (frustumCornersLS[i].z < mins.z)
                {
                    mins.z = frustumCornersLS[i].z;
                }
            }
            s_cam.ViewMatrix = newView;
            s_cam.ProjectionMatrix.SetToOrtho(mins.x, maxes.x, mins.y, maxes.y, -splits[3], splits[3]);
            viewProjectionMatrix.Set(s_cam.ViewMatrix);
            viewProjectionMatrix.MultiplyStore(s_cam.ProjectionMatrix);
            s_cam.ViewProjectionMatrix.Set(s_cam.ViewMatrix);
            s_cam.ViewProjectionMatrix.MultiplyStore(s_cam.ProjectionMatrix);
            s_cam.InverseViewProjectionMatrix.Set(s_cam.ViewProjectionMatrix);
            s_cam.InverseViewProjectionMatrix.InvertStore();
        }

        public void Render()
        {
            Update();
            Capture();

            GL.Enable(EnableCap.DepthClamp);
            Shader.FrontFace = FrontFaceDirection.Cw;
            rm.RenderBucketDepth(environment, s_cam, RenderManager.Bucket.Opaque);
            rm.RenderBucketDepth(environment, s_cam, RenderManager.Bucket.Transparent);
            Shader.FrontFace = FrontFaceDirection.Ccw;
            GL.Disable(EnableCap.DepthClamp);

            Release();
        }

        public void Capture()
        {
            fbo.Capture();
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Release()
        {
            fbo.Release();
            tex = fbo.DepthTexture;
        }
    }
}