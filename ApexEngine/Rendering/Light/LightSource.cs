using ApexEngine.Math;

namespace ApexEngine.Rendering.Light
{
    public abstract class LightSource 
    {
        protected Color4f color = new Color4f(1.0f);
        protected float intensity = 1.0f;

        public Color4f Color
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