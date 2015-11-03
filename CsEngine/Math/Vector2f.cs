using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsEngine.Math
{
    public class Vector2f
    {
        public float x, y;
        public Vector2f()
        {
            Set(0.0f);
        }
        public Vector2f(Vector2f other)
        {
            Set(other);
        }
        public Vector2f(float _x, float _y)
        {
            Set(_x, _y);
        }
        public Vector2f(float _xy)
        {
            Set(_xy);
        }
        public Vector2f Set(Vector2f other)
        {
            this.x = other.x;
            this.y = other.y;
            return this;
        }
        public Vector2f Set(float _x, float _y)
        {
            this.x = _x;
            this.y = _y;
            return this;
        }
        public Vector2f Set(float _xy)
        {
            this.x = _xy;
            this.y = _xy;
            return this;
        }
        public Vector2f Add(Vector2f other)
        {
            Vector2f res = new Vector2f();
            res.x = this.x + other.x;
            res.y = this.y + other.y;
            return res;
        }
        public Vector2f AddStore(Vector2f other)
        {
            this.x += other.x;
            this.y += other.y;
            return this;
        }
        public Vector2f Subtract(Vector2f other)
        {
            Vector2f res = new Vector2f();
            res.x = this.x - other.x;
            res.y = this.y - other.y;
            return res;
        }
        public Vector2f SubtractStore(Vector2f other)
        {
            this.x -= other.x;
            this.y -= other.y;
            return this;
        }
        public Vector2f Multiply(Vector2f other)
        {
            Vector2f res = new Vector2f();
            res.x = this.x * other.x;
            res.y = this.y * other.y;
            return res;
        }
        public Vector2f MultiplyStore(Vector2f other)
        {
            this.x *= other.x;
            this.y *= other.y;
            return this;
        }
        public Vector2f Multiply(float scalar)
        {
            Vector2f res = new Vector2f();
            res.x = this.x * scalar;
            res.y = this.y * scalar;
            return res;
        }
        public Vector2f MultiplyStore(float scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
            return this;
        }
        public Vector2f Divide(Vector2f other)
        {
            Vector2f res = new Vector2f();
            res.x = this.x / other.x;
            res.y = this.y / other.y;
            return res;
        }
        public Vector2f DivideStore(Vector2f other)
        {
            this.x /= other.x;
            this.y /= other.y;
            return this;
        }
        public Vector2f Divide(float scalar)
        {
            Vector2f res = new Vector2f();
            res.x = this.x / scalar;
            res.y = this.y / scalar;
            return res;
        }
        public Vector2f DivideStore(float scalar)
        {
            this.x /= scalar;
            this.y /= scalar;
            return this;
        }
        public Vector2f Normalize()
        {
            Vector2f res = new Vector2f(this);
            float len = Length();
            float len2 = len * len;
            if (len2 == 0 || len2 == 1)
            {
                return res;
            }
            res.MultiplyStore(1.0f / (float)System.Math.Sqrt(len2));
            return res;
        }
        public Vector2f NormalizeStore()
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
	        return (float)System.Math.Sqrt(x * x + y * y);
        }
        public float Dot(Vector2f other)
        {
	        return this.x * other.x + this.y * other.y; 
        }
    }
}
