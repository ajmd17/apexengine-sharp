using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;

namespace ApexEngine.Scene.Physics
{
    public class PhysicsWorld
    {
        public enum PhysicsShape
        {
            Box,
            Sphere,
            StaticMesh,
            ConvexMesh
        };

        CollisionSystem collisionSystem = new CollisionSystemSAP();
        World world;
        PhysicsDebugDraw debugDraw;

        public PhysicsWorld(PhysicsDebugDraw debugDraw)
        {
            collisionSystem.UseTriangleMeshNormal = false;
            world = new World(collisionSystem);
            this.debugDraw = debugDraw;
        }

        public void Raycast(Vector3f origin, Vector3f direction, out Vector3f hitPoint)
        {
            Jitter.LinearMath.JVector outNormal;
            RigidBody outBody;
            float outFraction;
            bool hit = world.CollisionSystem.Raycast(new Jitter.LinearMath.JVector(origin.x, origin.y, origin.z),
                                          new Jitter.LinearMath.JVector(direction.x, direction.y, direction.z),
                                          null, out outBody, out outNormal, out outFraction);
            if (hit)
                hitPoint = origin.Add(direction.Multiply(outFraction));
            else
                hitPoint = null;
        }

        public void Raycast(Vector3f origin, Vector3f direction, out GameObject hitObject)
        {
            Jitter.LinearMath.JVector outNormal;
            RigidBody outBody;
            float outFraction;
            bool hit = world.CollisionSystem.Raycast(new Jitter.LinearMath.JVector(origin.x, origin.y, origin.z),
                                          new Jitter.LinearMath.JVector(direction.x, direction.y, direction.z),
                                          null, out outBody, out outNormal, out outFraction);
            if (hit)
                hitObject = (GameObject)outBody.Tag;
            else
                hitObject = null;
        }

        public void Raycast(Vector3f origin, Vector3f direction, out Vector3f hitPoint, out GameObject hitObject)
        {
            Jitter.LinearMath.JVector outNormal;
            RigidBody outBody;
            float outFraction;
            bool hit = world.CollisionSystem.Raycast(new Jitter.LinearMath.JVector(origin.x, origin.y, origin.z),
                                          new Jitter.LinearMath.JVector(direction.x, direction.y, direction.z),
                                          null, out outBody, out outNormal, out outFraction);
            if (hit)
            {
                hitObject = (GameObject)outBody.Tag;
                hitPoint = origin.Add(direction.Multiply(outFraction));
            }
            else
            {
                hitObject = null;
                hitPoint = null;
            }
        }

        public void Dispose()
        {
            //dynamicWorld.Dispose();
        }

        public void AddObject(GameObject gameObject)
        {
            AddObject(gameObject, 1.0f);
        }

        public void AddObject(GameObject gameObject, float mass)
        {
            AddObject(gameObject, mass, mass > 0 ? PhysicsShape.ConvexMesh : PhysicsShape.StaticMesh);
        }

        public void AddObject(GameObject gameObject, float mass, PhysicsShape physicsShape)
        {
            RigidBodyControl rbc = new RigidBodyControl(mass, physicsShape);
            gameObject.AddController(rbc);
            world.AddBody(rbc.Body);
            rbc.Body.EnableDebugDraw = true;
            rbc.Body.DebugDraw(debugDraw);
        }

        public void RemoveObject(GameObject gameObject)
        {
            RigidBodyControl rbc = (RigidBodyControl)gameObject.GetController(typeof(RigidBodyControl));
            if (rbc != null && world.RigidBodies.Contains(rbc.Body))
            {
                world.RemoveBody(rbc.Body);
                gameObject.RemoveController(rbc);
                rbc = null;
            }
        }

        public void Update()
        {
            world.Step(0.01f, false);
        }

        public void DrawDebug()
        {
            foreach (RigidBody body in world.RigidBodies)
            {
                body.DebugDraw(debugDraw);
            }
        }
    }
}