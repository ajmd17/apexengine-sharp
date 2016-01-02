using System;

namespace ApexEngine.Math
{
    public class Ray
    {
        private Vector3f direction = new Vector3f(), position = new Vector3f();

        public Ray()
        {
        }

        public Ray(Vector3f direction, Vector3f position)
        {
            this.direction.Set(direction);
            this.position.Set(position);
        }

        public Vector3f Direction
        {
            get { return direction; }
            set { direction.Set(value); }
        }

        public Vector3f Position
        {
            get { return position; }
            set { position.Set(value); }
        }
    }
}