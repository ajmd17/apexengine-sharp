using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public static class MathUtil
    {
        public const float PI = 3.14159265358979f;
        public static float ToDegrees(float rad)
        {
            return rad * 180.0f / PI;
        }
        public static float ToRadians(float deg)
        {
            return deg * PI / 180.0f;
        }
        public static float Clamp(float val, float min, float max)
        {
            if (val > max)
            {
                return max;
            }
            if (val < min)
            {
                return min;
            }
            return val;
        }
        public static float Lerp(float from, float to, float amt)
        {
            return from + amt * (to - from);
        }
    }
}
