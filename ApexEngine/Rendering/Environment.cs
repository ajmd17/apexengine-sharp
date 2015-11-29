using System.Collections.Generic;
using ApexEngine.Rendering.Light;
using ApexEngine.Math;

namespace ApexEngine.Rendering
{
    public class Environment
    {
        private DirectionalLight directionalLight = new DirectionalLight();
        private AmbientLight ambientLight = new AmbientLight();
        private List<PointLight> pointLights = new List<PointLight>();

        private Texture[] shadowMaps = new Texture[4];
        private Matrix4f[] shadowMatrices = new Matrix4f[4];
        private float[] shadowMapSplits = new float[4];
        private bool shadowsEnabled = false;

        private float fogStart = 40f;
        private float fogEnd = 170f;
        private Vector4f fogColor = new Vector4f(0.3f, 0.3f, 0.3f, 1.0f);

        public float FogStart
        {
            get { return fogStart; }
            set { fogStart = value; }
        }

        public float FogEnd
        {
            get { return fogEnd; }
            set { fogEnd = value; }
        }

        public Vector4f FogColor
        {
            get { return fogColor; }
            set { fogColor.Set(value); }
        }

        public List<PointLight> PointLights
        {
            get { return pointLights; }
        }

        public DirectionalLight DirectionalLight
        {
            get { return directionalLight; }
            set { directionalLight = value; }
        }

        public bool ShadowsEnabled
        {
            get { return shadowsEnabled; }
            set { shadowsEnabled = value; }
        }

        public Texture[] ShadowMaps
        {
            get { return shadowMaps; }
            set { shadowMaps = value; }
        }

        public Matrix4f[] ShadowMatrices
        {
            get { return shadowMatrices; }
            set { shadowMatrices = value; }
        }

        public float[] ShadowMapSplits
        {
            get { return shadowMapSplits; }
            set { shadowMapSplits = value; }
        }

        public AmbientLight AmbientLight
        {
            get { return ambientLight; }
            set { ambientLight = value; }
        }
    }
}