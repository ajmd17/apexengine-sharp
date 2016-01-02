using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;

namespace ApexEngine.Rendering.Light
{
    public class PointLight : LightSource
    {
        public const string P_LIGHT_NAME = "Env_PointLights";
        public const string P_LIGHT_POSITION = "position";
        public const string P_LIGHT_COLOR = "color";

        private Vector3f position = new Vector3f();

        public Vector3f Position
        {
            get { return position; }
            set { position.Set(value); }
        }

        public override void BindLight(int index, Shader shader)
        {
            shader.SetUniform(P_LIGHT_NAME + "[" + index.ToString() + "]." + P_LIGHT_POSITION, position);
            shader.SetUniform(P_LIGHT_NAME + "[" + index.ToString() + "]." + P_LIGHT_COLOR, color);
        }
    }
}
