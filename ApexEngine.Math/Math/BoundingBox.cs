using System.Collections.Generic;

namespace ApexEngine.Math
{
    public class BoundingBox
    {
        private Matrix4f matrix = new Matrix4f();
        private Vector3f max = new Vector3f(float.MinValue), min = new Vector3f(float.MaxValue), center = new Vector3f();
        public Vector3f dimension = new Vector3f();
        private Transform transform = new Transform();
        private bool updateNeeded = false;
        private Vector3f worldExtent = new Vector3f(float.NaN);
        private Vector3f[] corners = new Vector3f[8];

        public BoundingBox()
        {
            //Clear();
        }

        public BoundingBox(Vector3f dimMin, Vector3f dimMax)
        {
            CreateBoundingBox(dimMin, dimMax);
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
            corners[0] = new Vector3f(max.x, max.y, max.z);
            corners[1] = new Vector3f(min.x, max.y, max.z);
            corners[2] = new Vector3f(min.x, max.y, min.z);
            corners[3] = new Vector3f(max.x, max.y, min.z);
            corners[4] = new Vector3f(max.x, min.y, max.z);
            corners[5] = new Vector3f(min.x, min.y, max.z);
            corners[6] = new Vector3f(min.x, min.y, min.z);
            corners[7] = new Vector3f(max.x, min.y, min.z);
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
            // min.Set(minimum.x < maximum.x ? minimum.x : maximum.x, minimum.y < maximum.y ? minimum.y : maximum.y,
            //     minimum.z < maximum.z ? minimum.z : maximum.z);
            //max.Set(minimum.x > maximum.x ? minimum.x : maximum.x, minimum.y > maximum.y ? minimum.y : maximum.y,
            //     minimum.z > maximum.z ? minimum.z : maximum.z);
            min.Set(minimum);
            max.Set(maximum);
            center.Set(min).AddStore(max).MultiplyStore(0.5f);
            dimension.Set(max).SubtractStore(min).MultiplyStore(0.5f);
            UpdateCorners();
            updateNeeded = true;
            return this;
        }

        public BoundingBox Clear()
        {
            return Set(new Vector3f(0), new Vector3f(0));
        }
    }
}