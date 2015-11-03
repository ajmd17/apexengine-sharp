using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsEngine.Math
{
    public class Vector3f
    {
        public float x, y, z;
        public Vector3f()
        {
            Set(0.0f);
        }
        public Vector3f(Vector3f other)
        {
            Set(other);
        }
        public Vector3f(float _x, float _y, float _z)
        {
            Set(_x, _y, _z);
        }
        public Vector3f(float _xyz)
        {
            Set(_xyz);
        }
        public Vector3f Set(Vector3f other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            return this;
        }
        public Vector3f Set(float _x, float _y, float _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            return this;
        }
        public Vector3f Set(float _xyz)
        {
            this.x = _xyz;
            this.y = _xyz;
            this.z = _xyz;
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
        public Vector3f Cross(Vector3f other)
        {
            Vector3f res = new Vector3f();
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
    }
}
