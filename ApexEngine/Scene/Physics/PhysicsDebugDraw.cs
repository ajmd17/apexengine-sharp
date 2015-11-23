using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ApexEngine.Rendering;

namespace ApexEngine.Scene.Physics
{
    public class PhysicsDebugDraw : DebugDraw
    {
        private Camera cam;
        private DebugDrawModes debugMode;

        public PhysicsDebugDraw(Camera cam)
        {
            this.cam = cam;
        }

        public override DebugDrawModes DebugMode
        {
            get
            {
                return debugMode;
            }

            set
            {
                debugMode = value;
            }
        }

        public override void Draw3dText(ref Vector3 location, string textString)
        {
            throw new NotImplementedException();
        }

        public override void DrawContactPoint(ref Vector3 pointOnB, ref Vector3 normalOnB, float distance, int lifeTime, Color4 color)
        {
            throw new NotImplementedException();
        }

        public override void DrawLine(ref Vector3 from, ref Vector3 to, Color4 color)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(cam.ViewMatrix.Invert().GetInvertedValues());
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(cam.ProjectionMatrix.Invert().GetInvertedValues());

            GL.Begin(PrimitiveType.Lines);

            GL.Color4(0.0f, 1.0f, 0.0f, 1.0f);
            GL.Vertex3(from.X, from.Y, from.Z);
            GL.Vertex3(to.X, to.Y, to.Z);

            GL.End();
        }

        public override void ReportErrorWarning(string warningString)
        {
            throw new NotImplementedException();
        }
    }
}
