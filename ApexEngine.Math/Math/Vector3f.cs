namespace ApexEngine.Math
{
    public class Vector3f
    {
        public static readonly Vector3f Zero = new Vector3f(0.0f, 0.0f, 0.0f);

        public static readonly Vector3f UnitX = new Vector3f(1.0f, 0.0f, 0.0f);

        public static readonly Vector3f UnitY = new Vector3f(0.0f, 1.0f, 0.0f);

        public static readonly Vector3f UnitZ = new Vector3f(0.0f, 0.0f, 1.0f);

        public static readonly Vector3f One = new Vector3f(1.0f, 1.0f, 1.0f);

        public static readonly Vector3f NaN = new Vector3f(float.NaN);

        public float x, y, z;

        private static Matrix4f tmpRot = new Matrix4f();

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public Vector3f()
        {
            Set(0.0f);
        }

        public Vector3f(Vector3f other)
        {
            Set(other);
        }

        public Vector3f(float x, float y, float z)
        {
            Set(x, y, z);
        }

        public Vector3f(float xyz)
        {
            Set(xyz);
        }

        public Vector3f(Vector4f other)
        {
            Set(other);
        }

        public Vector3f Set(Vector3f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            return this;
        }

        public Vector3f Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
        }

        public Vector3f Set(float xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
            return this;
        }

        public Vector3f Set(Vector4f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            return this;
        }

        public Vector3f Add(Vector3f other)
        {
            Vector3f res = new Vector3f();
            res.x = this.x + other.x;
            res.y = this.y + other.y;
            res.z = this.z + other.z;
            return res;
        }

        public Vector3f AddStore(Vector3f other)
        {
            this.x += other.x;
            this.y += other.y;
            this.z += other.z;
            return this;
        }

        public Vector3f Subtract(Vector3f other)
        {
            Vector3f res = new Vector3f();
            res.x = this.x - other.x;
            res.y = this.y - other.y;
            res.z = this.z - other.z;
            return res;
        }

        public Vector3f SubtractStore(Vector3f other)
        {
            this.x -= other.x;
            this.y -= other.y;
            this.z -= other.z;
            return this;
        }

        public Vector3f Multiply(Vector3f other)
        {
            Vector3f res = new Vector3f();
            res.x = this.x * other.x;
            res.y = this.y * other.y;
            res.z = this.z * other.z;
            return res;
        }

        public Vector3f MultiplyStore(Vector3f other)
        {
            this.x *= other.x;
            this.y *= other.y;
            this.z *= other.z;
            return this;
        }

        public Vector3f Multiply(float scalar)
        {
            Vector3f res = new Vector3f();
            res.x = this.x * scalar;
            res.y = this.y * scalar;
            res.z = this.z * scalar;
            return res;
        }

        public Vector3f MultiplyStore(float scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
            this.z *= scalar;
            return this;
        }

        public Vector3f Multiply(Matrix4f mat)
        {
            Vector3f res = new Vector3f();
            res.Set(x * mat.values[Matrix4f.m00] + y * mat.values[Matrix4f.m01] + z * mat.values[Matrix4f.m02] + mat.values[Matrix4f.m03],
                    x * mat.values[Matrix4f.m10] + y * mat.values[Matrix4f.m11] + z * mat.values[Matrix4f.m12] + mat.values[Matrix4f.m13],
                    x * mat.values[Matrix4f.m20] + y * mat.values[Matrix4f.m21] + z * mat.values[Matrix4f.m22] + mat.values[Matrix4f.m23]);
            return res;
        }

        public Vector3f MultiplyStore(Matrix4f mat)
        {
            return Set(x * mat.values[Matrix4f.m00] + y * mat.values[Matrix4f.m01] + z * mat.values[Matrix4f.m02] + mat.values[Matrix4f.m03],
                    x * mat.values[Matrix4f.m10] + y * mat.values[Matrix4f.m11] + z * mat.values[Matrix4f.m12] + mat.values[Matrix4f.m13],
                    x * mat.values[Matrix4f.m20] + y * mat.values[Matrix4f.m21] + z * mat.values[Matrix4f.m22] + mat.values[Matrix4f.m23]);
        }

        public Vector3f Divide(Vector3f other)
        {
            Vector3f res = new Vector3f();
            res.x = this.x / other.x;
            res.y = this.y / other.y;
            res.z = this.z / other.z;
            return res;
        }

        public Vector3f DivideStore(Vector3f other)
        {
            this.x /= other.x;
            this.y /= other.y;
            this.z /= other.z;
            return this;
        }

        public Vector3f Divide(float scalar)
        {
            Vector3f res = new Vector3f();
            res.x = this.x / scalar;
            res.y = this.y / scalar;
            res.z = this.z / scalar;
            return res;
        }

        public Vector3f DivideStore(float scalar)
        {
            this.x /= scalar;
            this.y /= scalar;
            this.z /= scalar;
            return this;
        }

        public Vector3f Negate()
        {
            return Multiply(-1f);
        }

        public Vector3f NegateStore()
        {
            return MultiplyStore(-1f);
        }

        public Vector3f Lerp(Vector3f to, float amt)
        {
            return Add((to.Subtract(this)).Multiply(amt));
        }

        public Vector3f LerpStore(Vector3f to, float amt)
        {
            AddStore((to.Subtract(this)).Multiply(amt));
            return this;
        }

        public Vector3f Cross(Vector3f other)
        {
            Vector3f res = new Vector3f(this);
            float x1 = (res.y * other.z) - (res.z * other.y);
            float y1 = (res.z * other.x) - (res.x * other.z);
            float z1 = (res.x * other.y) - (res.y * other.x);
            res.Set(x1, y1, z1);
            return res;
        }

        public Vector3f CrossStore(Vector3f other)
        {
            float x1 = (this.y * other.z) - (this.z * other.y);
            float y1 = (this.z * other.x) - (this.x * other.z);
            float z1 = (this.x * other.y) - (this.y * other.x);
            return Set(x1, y1, z1);
        }

        public Vector3f Rotate(Vector3f axis, float angle)
        {
            Vector3f res = new Vector3f(this);
            tmpRot.SetToRotation(axis, angle);
            res.MultiplyStore(tmpRot);
            return res;
        }

        public Vector3f RotateStore(Vector3f axis, float angle)
        {
            tmpRot.SetToRotation(axis, angle);
            return MultiplyStore(tmpRot);
        }

        public Vector3f Normalize()
        {
            Vector3f res = new Vector3f(this);
            float len = Length();
            float len2 = len * len;
            if (len2 == 0 || len2 == 1)
            {
                return res;
            }
            res.MultiplyStore(1.0f / (float)System.Math.Sqrt(len2));
            return res;
        }

        public Vector3f NormalizeStore()
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
            return (float)System.Math.Sqrt(x * x + y * y + z * z);
        }

        public float Dot(Vector3f other)
        {
            return this.x * other.x + this.y * other.y + this.z * other.z;
        }

        public float DistanceSqr(Vector3f other)
        {
            double dx = x - other.x;
            double dy = y - other.y;
            double dz = z - other.z;
            return (float)(dx * dx + dy * dy + dz * dz);
        }

        public float Distance(Vector3f other)
        {
            return (float)System.Math.Sqrt(DistanceSqr(other));
        }

        public Vector4f ToVector4f()
        {
            return new Vector4f(this);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            hash = hash * 23 + z.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3f))
                return false;

            Vector3f vobj = (Vector3f)obj;

            if (vobj.x == x && vobj.y == y && vobj.z == z)
                return true;

            return false;
        }

        public override string ToString()
        {
            return "[" + x + ", " + y + ", " + z + "]";
        }
    }
}