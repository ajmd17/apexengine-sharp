using System.Collections.Generic;

namespace ApexEngine.Math
{
    public class BoundingBox
    {
        private Matrix4f matrix = new Matrix4f();
        private Vector3f max = new Vector3f(float.MinValue), min = new Vector3f(float.MaxValue), center = new Vector3f();
        public Vector3f dimension = new Vector3f();
        private Transform transform = new Transform();
        private Vector3f extent = new Vector3f(float.NaN);
        private Vector3f[] corners = new Vector3f[8];

        public BoundingBox()
        {
            for (int i = 0; i < corners.Length; i++)
                corners[i] = new Vector3f();
        }

        public BoundingBox(Vector3f dimMin, Vector3f dimMax)
        {
            for (int i = 0; i < corners.Length; i++)
                corners[i] = new Vector3f();
            CreateBoundingBox(dimMin, dimMax);
        }

        public Vector3f Extent
        {
            get { return extent; }
        }

        public Matrix4f Matrix
        {
            get { return matrix; }
            set { matrix.Set(value); }
        }

        public Vector3f Min
        {
            get { return min; }
        }

        public Vector3f Max
        {
            get { return max; }
        }

        public Vector3f Center
        {
            get { return center; }
        }

        public Vector3f[] Corners
        {
            get { return corners; }
        }

        private void UpdateCorners()
        {
            corners[0].Set(max.x, max.y, max.z);
            corners[1].Set(min.x, max.y, max.z);
            corners[2].Set(min.x, max.y, min.z);
            corners[3].Set(max.x, max.y, min.z);
            corners[4].Set(max.x, min.y, max.z);
            corners[5].Set(min.x, min.y, max.z);
            corners[6].Set(min.x, min.y, min.z);
            corners[7].Set(max.x, min.y, min.z);
        }

        public BoundingBox CreateBoundingBox(Vector3f minimum, Vector3f maximum)
        {
            return Set(minimum, maximum);
        }

        public BoundingBox Extend(BoundingBox b)
        {
             return Extend(b.Min).Extend(b.Max);
        }

        public BoundingBox Extend(Vector3f point)
        {
            return Set(MathUtil.Min(min, point), MathUtil.Max(max, point));
        }

        public BoundingBox Set(Vector3f minimum, Vector3f maximum)
        {
            min.Set(minimum);
            max.Set(maximum);

            center.Set(min).AddStore(max).MultiplyStore(0.5f);

            extent.Set(maximum);
            extent.SubtractStore(minimum);

            dimension.Set(max).SubtractStore(min).MultiplyStore(0.5f);

            UpdateCorners();

            return this;
        }

        public BoundingBox Clear()
        {
            return Set(new Vector3f(0), new Vector3f(0));
        }

        /// <summary>
        /// Get the intersection point between the bounding box and a ray.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public Vector3f Intersect(Ray ray)
        {
            const float epsilon = 1e-6f;
            float tMin = float.NaN, tMax = float.NaN;

            if (System.Math.Abs(ray.Direction.X) < epsilon)
            {
                if (ray.Position.X < Min.X || ray.Position.X > Max.X)
                    return Vector3f.NaN;
            }
            else
            {
                tMin = (Min.X - ray.Position.X) / ray.Direction.X;
                tMax = (Max.X - ray.Position.X) / ray.Direction.X;

                if (tMin > tMax)
                {
                    float temp = tMin;
                    tMin = tMax;
                    tMax = temp;
                }
            }

            if (System.Math.Abs(ray.Direction.Y) < epsilon)
            {
                if (ray.Position.Y < Min.Y || ray.Position.Y > Max.Y)
                    return Vector3f.NaN;
            }
            else
            {
                float tMinY = (Min.Y - ray.Position.Y) / ray.Direction.Y;
                float tMaxY = (Max.Y - ray.Position.Y) / ray.Direction.Y;

                if (tMinY > tMaxY)
                {
                    float temp = tMinY;
                    tMinY = tMaxY;
                    tMaxY = temp;
                }

                if ((tMin != float.NaN && tMin > tMaxY) || (tMax != float.NaN && tMinY > tMax))
                    return Vector3f.NaN;

                if (tMin == float.NaN || tMinY > tMin) tMin = tMinY;
                if (tMax == float.NaN || tMaxY < tMax) tMax = tMaxY;
            }

            if (System.Math.Abs(ray.Direction.Z) < epsilon)
            {
                if (ray.Position.Z < Min.Z || ray.Position.Z > Max.Z)
                    return Vector3f.NaN;
            }
            else
            {
                float tMinZ = (Min.Z - ray.Position.Z) / ray.Direction.Z;
                float tMaxZ = (Max.Z - ray.Position.Z) / ray.Direction.Z;

                if (tMinZ > tMaxZ)
                {
                    float temp = tMinZ;
                    tMinZ = tMaxZ;
                    tMaxZ = temp;
                }

                if ((tMin != float.NaN && tMin > tMaxZ) || (tMax != float.NaN && tMinZ > tMax))
                    return Vector3f.NaN;

                if (tMin == float.NaN || tMinZ > tMin) tMin = tMinZ;
                if (tMax == float.NaN || tMaxZ < tMax) tMax = tMaxZ;
            }

            if ((tMin == float.NaN && tMin < 0) && tMax > 0) return Vector3f.NaN;

            if (tMin < 0) return Vector3f.NaN;

            return ray.Position.Add(ray.Direction.Multiply(tMin));
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + Max.GetHashCode();
            hash = hash * 23 + Min.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoundingBox))
                return false;

            BoundingBox bb_obj = (BoundingBox)obj;

            if (bb_obj.Max.Equals(Max) && bb_obj.Min.Equals(Min))
                return true;

            return false;
        }

        public override string ToString()
        {
            string str = "Max: " + Max.ToString() + "\nMin: " + Min.ToString();
            return str;
        }
    }
}