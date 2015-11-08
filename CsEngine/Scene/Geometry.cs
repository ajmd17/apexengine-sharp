using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering;
namespace ApexEngine.Scene
{
    public class Geometry : GameObject
    {
        protected Material material;
        protected Mesh mesh;
        protected Shader shader;
        public Geometry()
            : base()
        {
            material = new Material();
        }
        public void Render(Camera cam)
        {
            if (shader != null && mesh != null)
            {
                shader.Use();
                shader.SetTransforms(GetWorldMatrix(), cam.GetViewMatrix(), cam.GetProjectionMatrix());
                shader.Update(cam, mesh);
                shader.Render(mesh);
                shader.End();
                Shader.Clear();
            }

        }
        public override void UpdateParents()
        {
            base.UpdateParents();
            if (attachedToRoot)
            {
                RenderManager.AddGeometry(this);
            }
            else
            {
                RenderManager.RemoveGeometry(this);
            }
        }
        public Mesh GetMesh()
        {
            return mesh;
        }
        public void SetMesh(Mesh mesh)
        {
            this.mesh = mesh;
        }
        public Shader GetShader()
        {
            return shader;
        }
        public void SetShader(Shader shader)
        {
            this.shader = shader;
        }
    }
}
