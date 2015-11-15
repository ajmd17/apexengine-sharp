using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public class Vector4f
    {
        public static Vector4f UNIT_XW = new Vector4f(1.0f, 0.0f, 0.0f, 1.0f);
        public static Vector4f UNIT_YW = new Vector4f(0.0f, 1.0f, 0.0f, 1.0f);
        public static Vector4f UNIT_ZW = new Vector4f(0.0f, 0.0f, 1.0f, 1.0f);
        public static Vector4f UNIT_X = new Vector4f(1.0f, 0.0f, 0.0f, 0.0f);
        public static Vector4f UNIT_Y = new Vector4f(0.0f, 1.0f, 0.0f, 0.0f);
        public static Vector4f UNIT_Z = new Vector4f(0.0f, 0.0f, 1.0f, 0.0f);
        public static Vector4f UNIT_W = new Vector4f(0.0f, 0.0f, 0.0f, 1.0f);
        public static Vector4f UNIT_XYZW = new Vector4f(1.0f, 1.0f, 1.0f, 1.0f);
        public float x, y, z, w;
        public Vector4f()
        {
            Set(0.0f);
        }
        public Vector4f(Vector4f other)
        {
            Set(other);
        }
        public Vector4f(float _x, float _y, float _z, float _w)
        {
            Set(_x, _y, _z, _w);
        }
        public Vector4f(float _xyzw)
        {
            Set(_xyzw);
        }
        public Vector4f Set(Vector4f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            this.w = other.w;
            return this;
        }
        public Vector4f Set(float _x, float _y, float _z, float _w)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.w = _w;
            return this;
        }
        public Vector4f Set(float _xyzw)
        {
            this.x = _xyzw;
            this.y = _xyzw;
            this.z = _xyzw;
            this.w = _xyzw;
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
        public override string ToString()
        {
            return "[" + x + ", " + y + ", " + z + ", " + w + "]";
        }
    }
}
