using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering;
namespace ApexEngine.Scene
{
    public class Geometry : GameObject
    {
        Texture tmpTexture;
        protected Material material;
        protected Mesh mesh;
        protected Shader shader;
        public Geometry()
            : base()
        {
            tmpTexture = Texture.LoadTexture("C:\\Users\\User\\Pictures\\grass_grass_0105_02_preview.jpg");
            material = new Material();
        }
        public Material Material
        {
            get { return material; }
            set { material = value; }
        }
        public void Render(Camera cam)
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
                shader.SetTransforms(GetWorldMatrix(), cam.GetViewMatrix(), cam.GetProjectionMatrix());
                shader.Update(cam, mesh);
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
