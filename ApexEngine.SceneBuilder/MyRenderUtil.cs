using ApexEngine.Math;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEditor
{
    class MyRenderUtil
    {

        public static void RenderBoundingBox(BoundingBox boundingBox)
        {
            float offset = 0.1f;
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            GL.Vertex3(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
        }
    }
}
