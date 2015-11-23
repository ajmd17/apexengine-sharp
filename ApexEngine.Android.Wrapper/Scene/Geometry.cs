using ApexEngine.Rendering;
using System;

namespace ApexEngine.Scene
{
    public class Geometry : GameObject
    {
        protected Material material;
        protected Mesh mesh;
        protected Shader shader, depthShader;
        protected RenderManager.Bucket bucket = RenderManager.Bucket.Opaque;

        public Geometry()
            : base()
        {
            material = new Material();
        }

        public Geometry(Mesh mesh) : this()
        {
            this.mesh = mesh;
        }

        public Geometry(Mesh mesh, Shader shader) : this(mesh)
        {
            this.shader = shader;
        }

        public Shader DepthShader
        {
            get { return depthShader; }
            set { depthShader = value; }
        }

        public RenderManager.Bucket Bucket
        {
            get { return bucket; }
            set { bucket = value; }
        }

        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public void Render(Rendering.Environment environment, Camera cam)
        {
            if (shader == null)
            {
                if (mesh.GetSkeleton() != null)
                    SetShader(typeof(Rendering.Shaders.DefaultShader), new ShaderProperties()
                        .SetProperty("SKINNING", true)
                        .SetProperty("NUM_BONES", mesh.GetSkeleton().GetNumBones()));
                else
                    SetShader(typeof(Rendering.Shaders.DefaultShader), new ShaderProperties());
            }
            if (mesh != null)
            {
                shader.Use();
                shader.ApplyMaterial(material);
                shader.SetTransforms(GetWorldMatrix(), cam.ViewMatrix, cam.ProjectionMatrix);
                shader.Update(environment, cam, mesh);
                shader.Render(mesh);
                shader.End();
                Shader.Clear();
                Texture.Clear();
            }
        }

        public override void UpdateParents()
        {
            base.UpdateParents();
            if (renderManager != null)
            {
                if (attachedToRoot)
                {
                    renderManager.AddGeometry(this);
                }
                else
                {
                    renderManager.RemoveGeometry(this);
                }
            }
        }

        public Mesh Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public Shader GetShader()
        {
            return shader;
        }

        public void SetShader(Shader shader)
        {
            this.shader = shader;
        }

        public void SetShader(Type shaderType)
        {
            SetShader(ShaderManager.GetShader(shaderType));
        }

        public void SetShader(Type shaderType, ShaderProperties properties)
        {
            SetShader(ShaderManager.GetShader(shaderType, properties));
        }
    }
}