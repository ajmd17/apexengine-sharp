using ApexEngine.Scene.Components;

namespace ApexEngine.Rendering.Shadows
{
    public class ShadowMappingComponent : RenderComponent
    {
        public enum ShadowRenderMode
        {
            Forward,
            Deferred
        };

        private ShadowRenderMode shadowRenderMode = ShadowRenderMode.Forward;
        protected Camera viewcam;
        protected float[] splits = new float[] { 4f, 15f, 30f, 60f };
        protected int[] shadowMapSize = new int[] { 512, 512 };
        protected ShadowMapRenderer[] shadowCams = new ShadowMapRenderer[4];
        private ShadowPostFilter postFilter;
        private Environment environment;

        public ShadowMappingComponent(Camera cam, Environment environment) : this(cam, environment, new int[] { 2048, 2048 })
        {
        }

        public ShadowMappingComponent(Camera cam, Environment environment, int[] shadowMapSize)
        {
            this.viewcam = cam;
            this.environment = environment;
            this.shadowMapSize = shadowMapSize;
        }

        public ShadowRenderMode RenderMode
        {
            get { return shadowRenderMode; }
            set
            {
                shadowRenderMode = value;
                if (shadowRenderMode == ShadowRenderMode.Forward)
                {
                    if (postFilter != null)
                    {
                        if (renderManager.PostProcessor.PostFilters.Contains(postFilter))
                        {
                            renderManager.PostProcessor.PostFilters.Remove(postFilter);
                        }
                    }
                }
                else
                {
                    if (postFilter != null)
                    {
                        if (!renderManager.PostProcessor.PostFilters.Contains(postFilter))
                        {
                            renderManager.PostProcessor.PostFilters.Add(postFilter);
                        }
                    }
                    else
                    {
                        postFilter = new ShadowPostFilter(this);
                        renderManager.PostProcessor.PostFilters.Add(postFilter);
                    }
                }
            }
        }

        public ShadowMapRenderer[] ShadowCams
        {
            get { return shadowCams; }
        }

        public float[] Splits
        {
            get { return splits; }
            set { splits = value; }
        }

        public override void Init()
        {
            for (int i = 0; i < shadowCams.Length; i++)
            {
                shadowCams[i] = new ShadowMapRenderer(environment, shadowMapSize, splits, renderManager, viewcam, splits[i]);
                shadowCams[i].Init();
            }
        }

        public override void Render()
        {
            for (int i = 0; i < shadowCams.Length; i++)
            {
                shadowCams[i].Render();
            }
        }

        public override void Update()
        {
            if (shadowRenderMode == ShadowRenderMode.Forward)
                environment.ShadowsEnabled = true;
            else
                environment.ShadowsEnabled = false;

            for (int i = 0; i < shadowCams.Length; i++)
            {
                if (environment.DirectionalLight != null)
                {
                    shadowCams[i].LightDirection.x = -environment.DirectionalLight.Direction.x;
                    shadowCams[i].LightDirection.y = -environment.DirectionalLight.Direction.y;
                    shadowCams[i].LightDirection.z = -environment.DirectionalLight.Direction.z;
                }
                environment.ShadowMaps[i] = shadowCams[i].ShadowMap;
                environment.ShadowMatrices[i] = shadowCams[i].ViewProjectionMatrix;
                environment.ShadowMapSplits = splits;
            }
        }
    }
}