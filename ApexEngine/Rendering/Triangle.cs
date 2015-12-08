using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Rendering
{
    public class Triangle
    {
        private Vertex v0, v1, v2;

        public Triangle(Vertex v0, Vertex v1, Vertex v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }

        public Vertex V0
        {
            get { return v0; }
            set { v0 = value; }
        }

        public Vertex V1
        {
            get { return v1; }
            set { v1 = value; }
        }

        public Vertex V2
        {
            get { return v2; }
            set { v2 = value; }
        }
    }
}
