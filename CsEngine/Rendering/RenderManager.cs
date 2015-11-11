using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Scene;
namespace ApexEngine.Rendering
{
    public class RenderManager
    {
        public static int SCREEN_WIDTH, SCREEN_HEIGHT, WINDOW_X, WINDOW_Y;
        protected static List<Geometry> geometries = new List<Geometry>();
        private static float elapsedTime = 0f;
        public static float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }
        public static void AddGeometry(Geometry geom)
        {
            if (!geometries.Contains(geom))
                geometries.Add(geom);
        }
        public static void RemoveGeometry(Geometry geom)
        {
            if (geometries.Contains(geom))
                geometries.Remove(geom);
        }
        public static void Render(Camera cam)
        {
            for (int i = 0; i < geometries.Count; i++)
            {
                geometries[i].Render(cam);
            }
        }
    }
}
