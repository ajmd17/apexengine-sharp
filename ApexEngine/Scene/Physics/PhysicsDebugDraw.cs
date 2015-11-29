using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ApexEngine.Rendering;
using Jitter;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace ApexEngine.Scene.Physics
{
    public class PhysicsDebugDraw : IDebugDrawer
    {
        private Camera cam;

        public PhysicsDebugDraw(Camera cam)
        {
            this.cam = cam;
        }

        public void DrawLine(JVector start, JVector end)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(cam.ViewMatrix.Invert().GetInvertedValues());
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(cam.ProjectionMatrix.Invert().GetInvertedValues());

            GL.Begin(PrimitiveType.Lines);

            GL.Color4(0.0f, 1.0f, 0.0f, 1.0f);
            GL.Vertex3(start.X, start.Y, start.Z);
            GL.Vertex3(end.X, end.Y, end.Z);

            GL.End();
        }

        public void DrawPoint(JVector pos)
        {
            throw new NotImplementedException();
        }

        public void DrawTriangle(JVector pos1, JVector pos2, JVector pos3)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(cam.ViewMatrix.GetInvertedValues());
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(cam.ProjectionMatrix.GetInvertedValues());
            
            GL.Begin(PrimitiveType.Lines);

            GL.Color4(0.0f, 1.0f, 0.0f, 1.0f);
            GL.Vertex3(pos1.X, pos1.Y, pos1.Z);
            GL.Vertex3(pos2.X, pos2.Y, pos2.Z);
            GL.Vertex3(pos3.X, pos3.Y, pos3.Z);

            GL.End();
        }
    }
}