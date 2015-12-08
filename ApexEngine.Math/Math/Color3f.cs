using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public class Color3f : Vector3f
    {
        public Color3f() : base() { }

        public Color3f(float r, float g, float b) : base(r, g, b) { }

        public Color3f(float rgb) : base(rgb) { }

        public Color3f(Color3f other) : base(other) { }

        public Color3f(Color4f other) : base(other) { }

        public Color3f(Vector3f other) : base(other) { }

        public Color3f(Vector4f other) : base(other) { }

        public float R
        {
            get { return x; }
            set { x = value; }
        }

        public float G
        {
            get { return y; }
            set { y = value; }
        }

        public float B
        {
            get { return z; }
            set { z = value; }
        }
    }
}
