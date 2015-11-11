using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Environment
{
    public abstract class LightSource
    {
        protected Vector4f color = new Vector4f(1.0f);
        protected float intensity = 1.0f;
        public Vector4f Color
        {
            get { return color; }
            set { color.Set(value); }
        }
        public float Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }
        public abstract void BindLight(int index, Shader shader);

    }
}
