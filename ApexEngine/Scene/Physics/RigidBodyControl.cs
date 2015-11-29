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
        private Vector3f tmpVec0 = new Vector3f();
        private ApexEngine.Math.Quaternion tmpRot0 = new ApexEngine.Math.Quaternion();
        private float mass = 1f;
        private PhysicsWorld.PhysicsShape physicsShape = PhysicsWorld.PhysicsShape.ConvexMesh;

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

        public RigidBody Body
        {
            get { return body; }
        }

        public override void Init()
        {
            Shape shape = null;
            GameObject.UpdateTransform();
            if (physicsShape == PhysicsWorld.PhysicsShape.StaticMesh)
            {
                List<JVector> jvec = new List<JVector>();
                List<TriangleVertexIndices> tv = new List<TriangleVertexIndices>();
                List<Mesh> meshes = MeshUtil.GatherMeshes(GameObject);
                List<Vector3f> vertexPositions = new List<Vector3f>();

                for (int m = 0; m < meshes.Count; m++)
                {
                    for (int v = 0; v < meshes[m].vertices.Count; v++)
                    {
                        Vector3f myvec = meshes[m].vertices[v].GetPosition();
                        jvec.Add(new JVector(myvec.x, myvec.y, myvec.z));
                    }
                    for (int i = 0; i < meshes[m].indices.Count; i+=3)
                    {
                        tv.Add(new TriangleVertexIndices(meshes[m].indices[i +2], meshes[m].indices[i + 1], meshes[m].indices[i]));
                    }
                }
                Octree oct = new Octree(jvec, tv);
                TriangleMeshShape trimesh = new TriangleMeshShape(oct);
                shape = trimesh;
            }
            else if (physicsShape == PhysicsWorld.PhysicsShape.ConvexMesh)
            {
                List<JVector> jvec = new List<JVector>();
                List<Mesh> meshes = MeshUtil.GatherMeshes(GameObject);
                List<Vector3f> vertexPositions = new List<Vector3f>();

                for (int m = 0; m < meshes.Count; m++)
                {
                    for (int v = 0; v < meshes[m].indices.Count; v++)
                    {
                        Vector3f vec = meshes[m].vertices[meshes[m].indices[v]].GetPosition();
                        jvec.Add(new JVector(vec.x, vec.y, vec.z));
                    }
                }

                ConvexHullShape hullShape = new ConvexHullShape(jvec);
                shape = hullShape;
            }
            else if (physicsShape == PhysicsWorld.PhysicsShape.Box)
            {
                shape = new BoxShape(2, 2, 2); // TODO: Calculate mesh bounding box
            }
            body = new RigidBody(shape);
            body.Tag = GameObject;
            body.Position = new JVector(GameObject.GetWorldTranslation().x, GameObject.GetWorldTranslation().y, GameObject.GetWorldTranslation().z);
            
            if (mass == 0)
                body.IsStatic = true;
        }

        public override void Update()
        {
            JVector vec = body.Position;
            tmpVec0.Set(vec.X, vec.Y, vec.Z);
            if (mass > 0.0f)
                GameObject.SetLocalTranslation(tmpVec0);
            /*body.Orientation.
            tmpRot0.Set(rot.X, rot.Y, rot.Z, rot.W);
            tmpRot0.InverseStore();
            GameObject.SetLocalRotation(tmpRot0);*/
        }
    }
}