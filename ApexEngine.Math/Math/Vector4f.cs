namespace ApexEngine.Math
{
    public class Vector4f
    {
        public static readonly Vector4f UnitXW = new Vector4f(1.0f, 0.0f, 0.0f, 1.0f);

        public static readonly Vector4f UnitYW = new Vector4f(0.0f, 1.0f, 0.0f, 1.0f);

        public static readonly Vector4f UnitZW = new Vector4f(0.0f, 0.0f, 1.0f, 1.0f);

        public static readonly Vector4f UnitX = new Vector4f(1.0f, 0.0f, 0.0f, 0.0f);

        public static readonly Vector4f UnitY = new Vector4f(0.0f, 1.0f, 0.0f, 0.0f);

        public static readonly Vector4f UnitZ = new Vector4f(0.0f, 0.0f, 1.0f, 0.0f);

        public static readonly Vector4f UnitW = new Vector4f(0.0f, 0.0f, 0.0f, 1.0f);

        public static readonly Vector4f One = new Vector4f(1.0f);

        public static readonly Vector4f Zero = new Vector4f(0.0f);

        public float x, y, z, w;

        public Vector4f()
        {
            Set(0.0f);
        }

        public Vector4f(Vector4f other)
        {
            Set(other);
        }

        public Vector4f(float x, float y, float z, float w)
        {
            Set(x, y, z, w);
        }

        public Vector4f(float xyzw)
        {
            Set(xyzw);
        }

        public Vector4f(Vector3f other)
        {
            Set(other);
        }

        public Vector4f Set(Vector4f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            this.w = other.w;
            return this;
        }

        public Vector4f Set(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            return this;
        }

        public Vector4f Set(float xyzw)
        {
            this.x = xyzw;
            this.y = xyzw;
            this.z = xyzw;
            this.w = xyzw;
            return this;
        }

        public Vector4f Set(Vector3f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            this.w = 1.0f;
            return this;
        }

        public Vector4f Add(Vector4f other)
        {
            Vector4f res = new Vector4f();
            res.x = this.x + other.x;
            res.y = this.y + other.y;
            res.z = this.z + other.z;
            res.w = this.w + other.w;
            return res;
        }

        public Vector4f AddStore(Vector4f other)
        {
            this.x += other.x;
            this.y += other.y;
            this.z += other.z;
            this.w += other.w;
            return this;
        }

        public Vector4f Subtract(Vector4f other)
        {
            Vector4f res = new Vector4f();
            res.x = this.x - other.x;
            res.y = this.y - other.y;
            res.z = this.z - other.z;
            res.w = this.w - other.w;
            return res;
        }

        public Vector4f SubtractStore(Vector4f other)
        {
            this.x -= other.x;
            this.y -= other.y;
            this.z -= other.z;
            this.w -= other.w;
            return this;
        }

        public Vector4f Multiply(Vector4f other)
        {
            Vector4f res = new Vector4f();
            res.x = this.x * other.x;
            res.y = this.y * other.y;
            res.z = this.z * other.z;
            res.w = this.w * other.w;
            return res;
        }

        public Vector4f MultiplyStore(Vector4f other)
        {
            this.x *= other.x;
            this.y *= other.y;
            this.z *= other.z;
            this.w *= other.w;
            return this;
        }

        public Vector4f Multiply(float scalar)
        {
            Vector4f res = new Vector4f();
            res.x = this.x * scalar;
            res.y = this.y * scalar;
            res.z = this.z * scalar;
            res.w = this.w * scalar;
            return res;
        }

        public Vector4f MultiplyStore(float scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
            this.z *= scalar;
            this.w *= scalar;
            return this;
        }

        public Vector4f Divide(Vector4f other)
        {
            Vector4f res = new Vector4f();
            res.x = this.x / other.x;
            res.y = this.y / other.y;
            res.z = this.z / other.z;
            res.w = this.w / other.w;
            return res;
        }

        public Vector4f DivideStore(Vector4f other)
        {
            this.x /= other.x;
            this.y /= other.y;
            this.z /= other.z;
            this.w /= other.w;
            return this;
        }

        public Vector4f Divide(float scalar)
        {
            Vector4f res = new Vector4f();
            res.x = this.x / scalar;
            res.y = this.y / scalar;
            res.z = this.z / scalar;
            res.w = this.w / scalar;
            return res;
        }

        public Vector4f DivideStore(float scalar)
        {
            this.x /= scalar;
            this.y /= scalar;
            this.z /= scalar;
            this.w /= scalar;
            return this;
        }

        public Vector4f Negate()
        {
            return Multiply(-1f);
        }

        public Vector4f NegateStore()
        {
            return MultiplyStore(-1f);
        }

        public Vector4f Normalize()
        {
            Vector4f res = new Vector4f(this);
            float len = Length();
            float len2 = len * len;
            if (len2 == 0 || len2 == 1)
            {
                return res;
            }
            res.MultiplyStore(1.0f / (float)System.Math.Sqrt(len2));
            return res;
        }

        public Vector4f NormalizeStore()
        {
            float len = Length();
            float len2 = len * len;
            if (len2 == 0 || len2 == 1)
            {
                return this;
            }
            return MultiplyStore(1.0f / (float)System.Math.Sqrt(len2));
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        public float Dot(Vector4f other)
        {
            return this.x * other.x + this.y * other.y + this.z * other.z + this.w * other.w;
        }

        public float DistanceSqr(Vector4f other)
        {
            double dx = x - other.x;
            double dy = y - other.y;
            double dz = z - other.z;
            double dw = w - other.w;
            return (float)(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        public float Distance(Vector4f other)
        {
            return (float)System.Math.Sqrt(DistanceSqr(other));
        }

        public Vector3f ToVector3f()
        {
            return new Vector3f(this);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            hash = hash * 23 + z.GetHashCode();
            hash = hash * 23 + w.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector4f))
                return false;

            Vector4f vobj = (Vector4f)obj;

            if (vobj.x == x && vobj.y == y && vobj.z == z && vobj.w == w)
                return true;

            return false;
        }

        public override string ToString()
        {
            return "[" + x + ", " + y + ", " + z + ", " + w + "]";
        }
    }
}