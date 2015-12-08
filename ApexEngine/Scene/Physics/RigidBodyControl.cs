using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Scene.Components;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Util;
using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using Jitter.DataStructures;

namespace ApexEngine.Scene.Physics
{
    public class RigidBodyControl : Controller
    {
        private RigidBody body;
        private Vector3f tmpVec0 = new Vector3f(), tmpVec1 = new Vector3f(), tmpScl = new Vector3f(1, 1, 1);
        private ApexEngine.Math.Quaternion tmpRot0 = new ApexEngine.Math.Quaternion();
        private float mass = 1f;
        private PhysicsWorld.PhysicsShape physicsShape = PhysicsWorld.PhysicsShape.ConvexMesh;
        private Vector3f origin = new Vector3f();
        private JVector tmpJVec = new JVector();
        private BoundingBox boundingBox;
        private Vector3f offset = new Vector3f();

        public RigidBodyControl()
        {
            mass = 1.0f;
        }

        public RigidBodyControl(float mass)
        {
            this.mass = mass;
        }

        public RigidBodyControl(float mass, PhysicsWorld.PhysicsShape physicsShape)
        {
            this.mass = mass;
            this.physicsShape = physicsShape;
        }

        ~RigidBodyControl()
        {
            body = null;
            boundingBox = null;
        }

        public RigidBody Body
        {
            get { return body; }
        }

        public Vector3f Origin
        {
            get { return origin; }
        }

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public Vector3f Offset
        {
            get { return offset; }
            set { offset.Set(value); }
        }
        
        public void SetTranslation(Vector3f pos)
        {
            if (body != null)
            {
                tmpJVec.Set(pos.x, pos.y, pos.z);
                body.Position = tmpJVec;
                // origin.SubtractStore(boundingBox.Center);
            }
            origin.Set(GameObject.GetUpdatedWorldTranslation());
        }

        public Vector3f GetTranslation()
        {
            if (body != null)
                tmpVec1.Set(body.Position.X, body.Position.Y, body.Position.Z);
            return tmpVec1;
        }

        private Shape CreateShape()
        {
            Shape shape = null;
            boundingBox = new BoundingBox();
            List<Geometry> geom = MeshUtil.GatherGeometry(GameObject);
            List<Mesh> meshes = new List<Mesh>();
            List<Matrix4f> matrices = new List<Matrix4f>();
            Vector3f mytrans = GameObject.GetUpdatedWorldTranslation();
            if (GameObject is Geometry)
            {
                Geometry g_obj = (Geometry)GameObject;

                Mesh m = g_obj.Mesh;
                meshes.Add(m);
                Transform ttransform = new Transform();
                ttransform.SetTranslation(g_obj.GetUpdatedWorldTranslation());
                ttransform.SetRotation(g_obj.GetUpdatedWorldRotation());
                ttransform.SetScale(g_obj.GetUpdatedWorldScale());
                Matrix4f matrix = ttransform.GetMatrix();
                BoundingBox tmpBB = m.CreateBoundingBox();
                matrices.Add(matrix);
                boundingBox.Extend(tmpBB);
            }
            foreach (Geometry g in geom)
            {
                if (g != GameObject)
                {
                    Mesh m = g.Mesh;
                    meshes.Add(m);
                    Transform ttransform = new Transform();
                    ttransform.SetTranslation(g.GetUpdatedWorldTranslation().Subtract(mytrans));
                    ttransform.SetRotation(g.GetUpdatedWorldRotation());
                    ttransform.SetScale(g.GetUpdatedWorldScale());
                    Matrix4f matrix = ttransform.GetMatrix();
                    BoundingBox tmpBB = m.CreateBoundingBox(matrix);
                    matrices.Add(matrix);
                    boundingBox.Extend(tmpBB);
                }
            }
            if (physicsShape == PhysicsWorld.PhysicsShape.StaticMesh)
            {
                List<JVector> jvec = new List<JVector>();
                List<TriangleVertexIndices> tv = new List<TriangleVertexIndices>();
                List<Vector3f> vertexPositions = new List<Vector3f>();

                for (int m = 0; m < meshes.Count; m++)
                {
                    for (int v = 0; v < meshes[m].vertices.Count; v++)
                    {
                        Vector3f myvec = meshes[m].vertices[v].GetPosition().Multiply(matrices[m]);
                        jvec.Add(new JVector(myvec.x, myvec.y, myvec.z));
                    }
                    for (int i = 0; i < meshes[m].indices.Count; i += 3)
                    {
                        tv.Add(new TriangleVertexIndices(meshes[m].indices[i + 2], meshes[m].indices[i + 1], meshes[m].indices[i]));
                    }
                }
                Octree oct = new Octree(jvec, tv);
                TriangleMeshShape trimesh = new TriangleMeshShape(oct);
                jvec.Clear();
                tv.Clear();
                shape = trimesh;
            }
            else if (physicsShape == PhysicsWorld.PhysicsShape.ConvexMesh)
            {
                List<JVector> jvec = new List<JVector>();
                List<Vector3f> vertexPositions = new List<Vector3f>();

                for (int m = 0; m < meshes.Count; m++)
                {
                    for (int v = 0; v < meshes[m].indices.Count; v++)
                    {
                        Vector3f vec = meshes[m].vertices[meshes[m].indices[v]].GetPosition().Multiply(matrices[m]);
                        jvec.Add(new JVector(vec.x, vec.y, vec.z));
                    }
                }

                ConvexHullShape hullShape = new ConvexHullShape(jvec);
                shape = hullShape;
            }
            else if (physicsShape == PhysicsWorld.PhysicsShape.Box)
            {
                Mesh boxMesh = MeshFactory.CreateCube(boundingBox.Min, boundingBox.Max);
                List<JVector> jvec = new List<JVector>();
                List<TriangleVertexIndices> tv = new List<TriangleVertexIndices>();
                for (int v = 0; v < boxMesh.vertices.Count; v++)
                {
                    Vector3f myvec = boxMesh.vertices[v].GetPosition();
                    jvec.Add(new JVector(myvec.x, myvec.y, myvec.z));
                }
                for (int i = 0; i < boxMesh.indices.Count; i += 3)
                {
                    tv.Add(new TriangleVertexIndices(boxMesh.indices[i + 2], boxMesh.indices[i + 1], boxMesh.indices[i]));
                }

                Octree oct = new Octree(jvec, tv);
                TriangleMeshShape trimesh = new TriangleMeshShape(oct);
                shape = trimesh;

            }
            return shape;
        }

        public void Reinit()
        {
            body.Shape = CreateShape();
       //     origin.Set(GameObject.GetUpdatedWorldTranslation());
      //      body.Position = new JVector(origin.x, origin.y, origin.z);
        }

        public override void Init()
        {
            

            body = new RigidBody(CreateShape());
            body.Tag = GameObject;

            origin.Set(GameObject.GetUpdatedWorldTranslation());
            body.Position = new JVector(origin.x, origin.y, origin.z);
            
            if (mass == 0)
                body.IsStatic = true;
        }

        public override void Update()
        {
            JVector vec = body.Position;
            tmpVec0.Set(vec.X, vec.Y, vec.Z);
           // tmpVec0.SubtractStore(boundingBox.Center);
           // if (mass > 0.0f)
           if (!GameObject.GetWorldTranslation().Equals(tmpVec0))
                GameObject.SetWorldTransformPhysics(tmpVec0, tmpRot0, tmpScl);
            /*body.Orientation.
            tmpRot0.Set(rot.X, rot.Y, rot.Z, rot.W);
            tmpRot0.InverseStore();
            GameObject.SetLocalRotation(tmpRot0);*/
        }
    }
}