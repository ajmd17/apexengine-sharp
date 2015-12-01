using ApexEngine.Rendering;
using System;
using System.Linq;

namespace ApexEngine.Scene
{
    public class Geometry : GameObject
    {
        protected ShaderProperties g_shaderProperties = new ShaderProperties();
        protected Material material;
        protected Mesh mesh;
        protected Shader shader, depthShader, normalsShader;

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

        public ShaderProperties ShaderProperties
        {
            get { return g_shaderProperties; }
            set { g_shaderProperties = value; }
        }

        public Shader DepthShader
        {
            get { return depthShader; }
            set { depthShader = value; }
        }

        public Shader NormalsShader
        {
            get { return normalsShader; }
            set { normalsShader = value; }
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
                {
                    Shader shader;
                    g_shaderProperties.SetProperty("SKINNING", true).SetProperty("NUM_BONES", mesh.GetSkeleton().GetNumBones());
                    shader = ShaderManager.GetShader(typeof(Rendering.Shaders.DefaultShader), g_shaderProperties);
                    SetShader(shader);
                }
                else
                {
                    Shader shader;
                    shader = ShaderManager.GetShader(typeof(Rendering.Shaders.DefaultShader), g_shaderProperties);
                    SetShader(shader);
                }
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
            }
        }

        public void UpdateShaderProperties()
        {
            string[] keys = Material.Values.Keys.ToArray();
            object[] vals = Material.Values.Values.ToArray();
            for (int i = 0; i < vals.Length; i++)
            {
                if (vals[i] == null) { }
                //g_shaderProperties.SetProperty(keys[i].ToUpper(), false);
                else
                {
                    /*if (vals[i] is int)
                        g_shaderProperties.SetProperty(keys[i].ToUpper(), (int)vals[i]);
                    else if (vals[i] is float)
                        g_shaderProperties.SetProperty(keys[i].ToUpper(), (float)vals[i]);
                    else if (vals[i] is bool)
                        g_shaderProperties.SetProperty(keys[i].ToUpper(), (bool)vals[i]);
                    else if (vals[i] is string)
                        g_shaderProperties.SetProperty(keys[i].ToUpper(), (string)vals[i]);
                    else */
                    if (vals[i] is Texture)
                        g_shaderProperties.SetProperty(keys[i].ToUpper(), true);
                }
            }
            if (shader != null)
            {
                Type shaderType = shader.GetType();
                SetShader(shaderType, g_shaderProperties);
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