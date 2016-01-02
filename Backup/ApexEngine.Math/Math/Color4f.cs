using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public class Color4f : Vector4f
    {
        public Color4f() : base() { }

        public Color4f(float r, float g, float b, float a) : base(r, g, b, a) { }

        public Color4f(float rgba) : base(rgba) { }

        public Color4f(Color4f other) : base(other) { }

        public Color4f(Vector3f other) : base(other) { }

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

        public float A
        {
            get { return w; }
            set { w = value; }
        }
    }
}
