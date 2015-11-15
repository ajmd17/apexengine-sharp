using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Environment
{
    public class DirectionalLight : LightSource
    {
        public const string D_LIGHT_NAME = "Env_DirectionalLight";
        public const string D_LIGHT_DIRECTION = "direction";
        public const string D_LIGHT_COLOR = "color";

        protected Vector3f direction = new Vector3f();
        public DirectionalLight()
        {
            direction.Set(-1.0f, 1.0f, -1.0f);
            color.Set(1f, 0.9f, 0.8f, 1.0f);
        }
        public DirectionalLight(Vector3f dir)
        {
            direction.Set(dir);
            color.Set(1f, 0.9f, 0.8f, 1.0f);
        }
        public DirectionalLight(Vector3f dir, Vector4f clr)
        {
            direction.Set(dir);
            color.Set(color);
        }
        public override void BindLight(int index, Shader shader)
        {
            shader.SetUniform(D_LIGHT_NAME + "." + D_LIGHT_DIRECTION, this.direction);
            shader.SetUniform(D_LIGHT_NAME + "." + D_LIGHT_COLOR, this.color);
        }
    }
}
