using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Scene.Components;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Util;
using BulletSharp;

namespace ApexEngine.Scene.Physics
{
    public class RigidBodyControl : Controller
    {
        private RigidBody body;
        private Vector3f tmpVec0 = new Vector3f();
        private Quaternion tmpRot0 = new Quaternion();
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

        private static OpenTK.Vector3 ConvertVec(Vector3f orig)
        {
            return new OpenTK.Vector3(orig.x, orig.y, orig.z);
        }

        public override void Init()
        {
            CollisionShape shape = null;

            if (physicsShape == PhysicsWorld.PhysicsShape.StaticMesh)
            {
                TriangleMesh trimesh = new TriangleMesh();
                List<Mesh> meshes = MeshUtil.GatherMeshes(GameObject);
                List<Vector3f> vertexPositions = new List<Vector3f>();

                for (int m = 0; m < meshes.Count; m++)
                {
                    for (int v = 0; v < meshes[m].indices.Count; v++)
                    {
                        vertexPositions.Add(meshes[m].vertices[meshes[m].indices[v]].GetPosition());
                    }
                }

                for (int i = 0; i < vertexPositions.Count; i += 3)
                {
                    trimesh.AddTriangle(ConvertVec(vertexPositions[i]), ConvertVec(vertexPositions[i + 1]), ConvertVec(vertexPositions[i + 2]));
                }

                shape = new BvhTriangleMeshShape(trimesh, true, true);
            }
            else if (physicsShape == PhysicsWorld.PhysicsShape.ConvexMesh)
            {
                ConvexHullShape hullShape = new ConvexHullShape();

                List<Mesh> meshes = MeshUtil.GatherMeshes(GameObject);
                List<Vector3f> vertexPositions = new List<Vector3f>();

                for (int m = 0; m < meshes.Count; m++)
                {
                    for (int v = 0; v < meshes[m].indices.Count; v++)
                    {
                        vertexPositions.Add(meshes[m].vertices[meshes[m].indices[v]].GetPosition());
                    }
                }

                for (int i = 0; i < vertexPositions.Count; i++)
                {
                    hullShape.AddPoint(ConvertVec(vertexPositions[i]));
                }

                shape = hullShape;
            }

            OpenTK.Vector3 localInertia;
            shape.CalculateLocalInertia(mass, out localInertia);
            MotionState motionState = new DefaultMotionState(OpenTK.Matrix4.CreateTranslation(GameObject.GetLocalTranslation().x, GameObject.GetLocalTranslation().y, GameObject.GetLocalTranslation().z));
            RigidBodyConstructionInfo info = new RigidBodyConstructionInfo(mass, motionState, shape, localInertia);
            body = new RigidBody(info);
        }

        public override void Update()
        {
            OpenTK.Vector3 vec = body.WorldTransform.ExtractTranslation();
            tmpVec0.Set(vec.X, vec.Y, vec.Z);
            GameObject.SetLocalTranslation(tmpVec0);

            OpenTK.Quaternion rot = body.WorldTransform.ExtractRotation();
            tmpRot0.Set(rot.X, rot.Y, rot.Z, rot.W);
            tmpRot0.InverseStore();
            GameObject.SetLocalRotation(tmpRot0);
        }
    }
}
