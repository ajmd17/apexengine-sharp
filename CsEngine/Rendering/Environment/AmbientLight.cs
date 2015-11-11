using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Environment
{
    public class AmbientLight : LightSource
    {
        public const string A_LIGHT_NAME = "Env_AmbientLight";
        public const string A_LIGHT_INTENSITY = "intensity";
        public const string A_LIGHT_COLOR = "color";
        public AmbientLight()
        {
            color.Set(0.07f, 0.07f, 0.07f, 0.0f);
        }
        public AmbientLight(Vector4f color)
        {
            this.color.Set(color);
        }
        public AmbientLight(float r, float g, float b, float a)
        {
            this.color.Set(r, g, b, a);
        }
        public override void BindLight(int index, Shader shader)
        {
            shader.SetUniform(A_LIGHT_NAME + "." + A_LIGHT_INTENSITY, this.intensity);
            shader.SetUniform(A_LIGHT_NAME + "." + A_LIGHT_COLOR, this.color);
        }
    }
}
