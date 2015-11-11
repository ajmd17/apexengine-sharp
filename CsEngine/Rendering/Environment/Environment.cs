using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering.Environment
{
    public class Environment
    {
        private static DirectionalLight directionalLight = new DirectionalLight();
        private static AmbientLight ambientLight = new AmbientLight();
        private static List<LightSource> lightSources = new List<LightSource>();
        public static List<LightSource> LightSources
        {
            get { return lightSources; }
        }
        public static DirectionalLight DirectionalLight
        {
            get { return directionalLight; }
            set { directionalLight = value; }
        }
        public static AmbientLight AmbientLight
        {
            get { return ambientLight; }
            set { ambientLight = value; }
        }

    }
}
