using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine;
using ApexEngine.Rendering;
using ApexEngine.Rendering.PostProcess;

namespace ApexEngine.Plugins
{
    public class RaytracerFilter : PostFilter
    {
        private float time = 0f;
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();
        public RaytracerFilter() : base((string)textLoader.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\post\\raytracer2.frag"))
        {
           
        }

        public override void End()
        {
        }

        public override void Update()
        {
            time += 0.01f;
            shader.SetUniform("u_camPos", cam.Translation);
            shader.SetUniform("u_camDir", cam.Direction);
            shader.SetUniform("u_time", time);
        }
    }
}
