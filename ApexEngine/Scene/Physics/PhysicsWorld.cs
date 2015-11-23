using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
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

        DefaultCollisionConfiguration collisionConfiguration;
        CollisionDispatcher dispatcher;
        DiscreteDynamicsWorld dynamicWorld;

        public PhysicsWorld(DebugDraw debugDraw)
        {
            collisionConfiguration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConfiguration);
            BroadphaseInterface broadphase = new DbvtBroadphase();
            SequentialImpulseConstraintSolver solver = new SequentialImpulseConstraintSolver();
            dynamicWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, solver, collisionConfiguration);
            dynamicWorld.Gravity = new OpenTK.Vector3(0, -9.81f, 0);
            dynamicWorld.DispatchInfo.AllowedCcdPenetration = 0.05f;
            dynamicWorld.DebugDrawer = debugDraw;
            debugDraw.DebugMode = DebugDrawModes.DrawWireframe;
        }

        public void Dispose()
        {
            dynamicWorld.Dispose();
        }

        public void AddObject(GameObject gameObject)
        {
            AddObject(gameObject, 1.0f);
        }

        public void AddObject(GameObject gameObject, float mass)
        {
            AddObject(gameObject, mass, mass > 0.0f ? PhysicsShape.ConvexMesh : PhysicsShape.StaticMesh);
        }

        public void AddObject(GameObject gameObject, float mass, PhysicsShape physicsShape)
        {
            RigidBodyControl rbc = new RigidBodyControl(mass, physicsShape);
            gameObject.AddController(rbc);
            dynamicWorld.AddRigidBody(rbc.Body);
        }

        public void RemoveObject(GameObject gameObject)
        {
            RigidBodyControl rbc = (RigidBodyControl)gameObject.GetController(typeof(RigidBodyControl));
            dynamicWorld.AddRigidBody(rbc.Body);
            gameObject.RemoveController(rbc);
        }

        public void Update()
        {
            dynamicWorld.StepSimulation(0.01f);
            int numManifolds = dynamicWorld.Dispatcher.NumManifolds;
            for (int i = 0; i < numManifolds; i++)
            {
                PersistentManifold contactManifold = dynamicWorld.Dispatcher.GetManifoldByIndexInternal(i);
                CollisionObject obA = contactManifold.Body0 as CollisionObject;
                CollisionObject obB = contactManifold.Body1 as CollisionObject;

                int numContacts = contactManifold.NumContacts;
                for (int j = 0; j < numContacts; j++)
                {
                    ManifoldPoint pt = contactManifold.GetContactPoint(j);
                    if (pt.Distance < 0.0f)
                    {
                        OpenTK.Vector3 ptA = pt.PositionWorldOnA;
                        OpenTK.Vector3 ptB = pt.PositionWorldOnB;
                        OpenTK.Vector3 normalOnB = pt.NormalWorldOnB;
                    }
                }
            }
        }

        public void DrawDebug()
        {
            dynamicWorld.DebugDrawWorld();
        }
    }
}
