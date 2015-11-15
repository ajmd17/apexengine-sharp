using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Scene;
using ApexEngine.Scene.Components;
using OpenTK.Graphics.OpenGL;
namespace ApexEngine.Rendering
{
    public class RenderManager
    {
        protected List<Geometry> geometries = new List<Geometry>();
        protected List<RenderComponent> components = new List<RenderComponent>();
        private static float elapsedTime = 0f;
        public List<RenderComponent> RenderComponents
        {
            get { return components; }
        }
        public void AddComponent(RenderComponent cmp)
        {
            components.Add(cmp);
            cmp.renderManager = this;
            cmp.Init();
        }
        public void RemoveComponent(RenderComponent cmp)
        {
            components.Remove(cmp);
            cmp.renderManager = null;
        }
        public static float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }
        public List<Geometry> GeometryList
        {
            get { return geometries; }
        }
        public void AddGeometry(Geometry geom)
        {
            if (!geometries.Contains(geom))
                geometries.Add(geom);
        }
        public void RemoveGeometry(Geometry geom)
        {
            if (geometries.Contains(geom))
                geometries.Remove(geom);
        }
        public void Render(Camera cam)
        {
            GL.Viewport(0, 0, cam.Width, cam.Height);
            foreach (RenderComponent rc in components)
            {
                rc.Render();
                rc.Update();
            }
            for (int i = 0; i < geometries.Count; i++)
            {
                if (geometries[i].AttachedToRoot)
                    geometries[i].Render(cam);
            }
        }
    }
}
