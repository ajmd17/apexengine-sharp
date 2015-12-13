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

        public static float Min(float a, float b)
        {
            return System.Math.Min(a, b);
        }

        public static float Max(float a, float b)
        {
            return System.Math.Max(a, b);
        }

        public static Vector2f Min(Vector2f a, Vector2f b)
        {
            return new Vector2f(Min(a.X, b.X), Min(a.Y, b.Y));
        }

        public static Vector2f Max(Vector2f a, Vector2f b)
        {
            return new Vector2f(Max(a.X, b.X), Max(a.Y, b.Y));
        }

        public static Vector3f Min(Vector3f a, Vector3f b)
        {
            return new Vector3f(Min(a.X, b.X), Min(a.Y, b.Y), Min(a.Z, b.Z));
        }

        public static Vector3f Max(Vector3f a, Vector3f b)
        {
            return new Vector3f(Max(a.X, b.X), Max(a.Y, b.Y), Max(a.Z, b.Z));
        }

        public static Vector4f Min(Vector4f a, Vector4f b)
        {
            return new Vector4f(Min(a.X, b.X), Min(a.Y, b.Y), Min(a.Z, b.Z), Min(a.W, b.W));
        }

        public static Vector4f Max(Vector4f a, Vector4f b)
        {
            return new Vector4f(Max(a.X, b.X), Max(a.Y, b.Y), Max(a.Z, b.Z), Max(a.W, b.W));
        }
        
        public static Vector2f Round(Vector2f vec)
        {
            return new Vector2f((float)System.Math.Round(vec.X), (float)System.Math.Round(vec.Y));
        }

        public static Vector3f Round(Vector3f vec)
        {
            return new Vector3f((float)System.Math.Round(vec.X), (float)System.Math.Round(vec.Y), (float)System.Math.Round(vec.Z));
        }

        public static Vector4f Round(Vector4f vec)
        {
            return new Vector4f((float)System.Math.Round(vec.X), (float)System.Math.Round(vec.Y), (float)System.Math.Round(vec.Z), (float)System.Math.Round(vec.W));
        }
    }
}