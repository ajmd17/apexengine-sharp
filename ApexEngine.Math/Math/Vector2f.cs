namespace ApexEngine.Math
{
    public class Vector2f
    {
        public static readonly Vector2f UnitX = new Vector2f(1.0f, 0.0f);

        public static readonly Vector2f UnitY = new Vector2f(0.0f, 1.0f);

        public static readonly Vector2f One = new Vector2f(1.0f, 1.0f);

        public static readonly Vector2f Zero = new Vector2f(0.0f, 0.0f);
        
        public float x, y;

        public Vector2f()
        {
            Set(0.0f);
        }

        public Vector2f(Vector2f other)
        {
            Set(other);
        }

        public Vector2f(float x, float y)
        {
            Set(x, y);
        }

        public Vector2f(float xy)
        {
            Set(xy);
        }

        public Vector2f Set(Vector2f other)
        {
            this.x = other.x;
            this.y = other.y;
            return this;
        }

        public Vector2f Set(float x, float y)
        {
            this.x = x;
            this.y = y;
            return this;
        }

        public Vector2f Set(float xy)
        {
            this.x = xy;
            this.y = xy;
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

        public Vector2f Negate()
        {
            return Multiply(-1f);
        }

        public Vector2f NegateStore()
        {
            return MultiplyStore(-1f);
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

        public float DistanceSqr(Vector2f other)
        {
            double dx = x - other.x;
            double dy = y - other.y;
            return (float)(dx * dx + dy * dy);
        }

        public float Distance(Vector2f other)
        {
            return (float)System.Math.Sqrt(DistanceSqr(other));
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2f))
                return false;

            Vector2f vobj = (Vector2f)obj;

            if (vobj.x == x && vobj.y == y)
                return true;

            return false;
        }

        public override string ToString()
        {
            return "[" + x + ", " + y + "]";
        }
    }
}