using System.Collections.Generic;
using ApexEngine.Rendering.Light;
using ApexEngine.Math;

namespace ApexEngine.Rendering
{
    public class Environment
    {
        private DirectionalLight directionalLight = new DirectionalLight();
        private AmbientLight ambientLight = new AmbientLight();
        private List<LightSource> lightSources = new List<LightSource>();
        private Texture[] shadowMaps = new Texture[4];
        private Matrix4f[] shadowMatrices = new Matrix4f[4];
        private float[] shadowMapSplits = new float[4];
        private bool shadowsEnabled = false;

        public List<LightSource> LightSources
        {
            get { return lightSources; }
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