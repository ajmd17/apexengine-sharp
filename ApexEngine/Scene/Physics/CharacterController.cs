using ApexEngine.Input;
using ApexEngine.Math;
using ApexEngine.Rendering.Cameras;
using Jitter.LinearMath;

namespace ApexEngine.Scene.Physics
{
    public class CharacterController : RigidBodyControl
    {
        private DefaultCamera camera;
        private JVector impulse = new JVector(), linv;
        private bool moving;
        private InputManager inputManager;
        private float speed = 3f;

        public CharacterController(DefaultCamera camera, InputManager inputManager, float mass) : base(mass, PhysicsWorld.PhysicsShape.Box)
        {
            this.camera = camera;
            this.inputManager = inputManager;
            camera.KeysEnabled = false;
        }

        public override void Init()
        {
            base.Init();

            Body.Damping = Jitter.Dynamics.RigidBody.DampingType.Angular;

            Body.Material.KineticFriction = 0.3f;
            Body.Material.Restitution = 0.0f;
            Body.AngularVelocity.Set(0, 0, 0);
        }

        public override void Update()
        {
            base.Update();

            Vector3f dir = camera.Direction;
            linv = this.Body.LinearVelocity;

            this.Body.AngularVelocity.Set(0, 0, 0);


            if (inputManager.IsKeyDown(InputManager.KeyboardKey.W))
            {
                impulse.X += dir.x * speed;
                impulse.Z += dir.z * speed;
            }
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.S))
            {
                impulse.X += -dir.x * speed;
                impulse.Z += -dir.z * speed;
            }
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.D))
            {
                impulse.X += -speed * (float)System.Math.Sin(MathUtil.ToRadians(camera.GetYaw() + 90));
                impulse.Z += speed * (float)System.Math.Cos(MathUtil.ToRadians(camera.GetYaw() + 90));
            }
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.A))
            {
                impulse.X += -speed * (float)System.Math.Sin(MathUtil.ToRadians(camera.GetYaw() - 90));
                impulse.Z += speed * (float)System.Math.Cos(MathUtil.ToRadians(camera.GetYaw() - 90));
            }
            impulse.Y = linv.Y;
            if (inputManager.IsKeyDown(InputManager.KeyboardKey.W) ||
                    inputManager.IsKeyDown(InputManager.KeyboardKey.S) ||
                    inputManager.IsKeyDown(InputManager.KeyboardKey.D) ||
                    inputManager.IsKeyDown(InputManager.KeyboardKey.A))
            {
                moving = true;
                Body.LinearVelocity = impulse;
                if (linv.X == 0.0f && linv.Y == 0.0f && linv.Z == 0.0f)
                {
                    this.Body.Position.Set(Body.Position.X + impulse.X * 0.01f, Body.Position.Y + impulse.Y * 0.01f, Body.Position.Z + impulse.Z * 0.01f);
                }
                impulse.Set(0, 0, 0);
            }
            else
            {
                moving = false;
            }
            camera.Translation.Set(Body.Position.X, Body.Position.Y + 3.5f, Body.Position.Z);
        }
    }
}