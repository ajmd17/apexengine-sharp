using ApexEngine.Math;
using ApexEngine.Rendering;
using System;
using System.Linq;

namespace ApexEngine.Scene
{
    public class Geometry : GameObject
    {
        protected ShaderProperties g_shaderProperties = new ShaderProperties();
        protected Mesh mesh;
        protected Shader shader, depthShader, normalsShader;
        protected BoundingBox worldBoundingBox, localBoundingBox;

        public Geometry()
            : base()
        {
        }

        public Geometry(Mesh mesh) : this()
        {
            this.mesh = mesh;
        }

        public Geometry(Mesh mesh, Shader shader) : this(mesh)
        {
            this.shader = shader;
        }

        public override BoundingBox GetWorldBoundingBox()
        {
            if (worldBoundingBox == null)
            {
                worldBoundingBox = new BoundingBox();
                UpdateWorldBoundingBox();
            }
            return worldBoundingBox;
        }

        public override BoundingBox GetLocalBoundingBox()
        {
            if (localBoundingBox == null)
            {
                localBoundingBox = new BoundingBox();
                UpdateLocalBoundingBox();
            }
            return localBoundingBox;
        }

        public override void UpdateWorldBoundingBox()
        {
            if (worldBoundingBox != null)
                if (mesh != null)
                    worldBoundingBox = mesh.CreateBoundingBox(GetWorldMatrix());
        }
        
        public override void UpdateLocalBoundingBox()
        {
            if (localBoundingBox != null)
                if (mesh != null)
                    localBoundingBox = mesh.CreateBoundingBox();
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
            get { return mesh.Material; }
            set { if (mesh != null) { mesh.Material = value; }  }
        }

        public void SetDefaultShader()
        {
            if (mesh.GetSkeleton() != null)
            {
                Shader shader;
                g_shaderProperties.SetProperty("SKINNING", true).SetProperty("NUM_BONES", mesh.GetSkeleton().GetNumBones());
                ShaderProperties p = new ShaderProperties(g_shaderProperties);
                p.SetProperty("DEFAULT", true);
                shader = ShaderManager.GetShader(typeof(Rendering.Shaders.DefaultShader), p);
                SetShader(shader);
            }
            else
            {
                Shader shader;
                ShaderProperties p = new ShaderProperties(g_shaderProperties);
                p.SetProperty("DEFAULT", true);
                shader = ShaderManager.GetShader(typeof(Rendering.Shaders.DefaultShader), p);
                SetShader(shader);
            }
        }

        public virtual void Render(Rendering.Environment environment, Camera cam)
        {
            if (shader == null)
            {
                SetDefaultShader();
            }
            if (mesh != null)
            {
                shader.Use();
                shader.ApplyMaterial(Material);
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
            SetShader(ShaderManager.GetShader(shaderType, new ShaderProperties().SetProperty("DEFAULT", true)));
        }

        public void SetShader(Type shaderType, ShaderProperties properties)
        {
            ShaderProperties p = new ShaderProperties(properties);
            p.SetProperty("DEFAULT", true);
            SetShader(ShaderManager.GetShader(shaderType, p));
        }

        public override GameObject Clone()
        {
            Geometry res = new Geometry();
            res.SetLocalTranslation(this.GetLocalTranslation());
            res.SetLocalScale(this.GetLocalScale());
            res.SetLocalRotation(this.GetLocalRotation());
            res.Mesh = this.mesh.Clone();
            res.shader = this.shader;
            res.depthShader = this.depthShader;
            res.normalsShader = this.normalsShader;
            res.ShaderProperties = this.ShaderProperties;
            return res;
        }
    }
}