using ApexEngine.Rendering;

namespace ApexEngine.Scene.Components
{
    public abstract class GameComponent : EngineComponent
    {
        public Node rootNode = new Node("GameComponent Node");
        private Camera cam;
        private Environment environment;

        public Environment Environment
        {
            get { return environment; }
            set { this.environment = value; }
        }

        public Camera Camera
        {
            get { return cam; }
            set { cam = value; }
        }

        public abstract void Update();

        public abstract void Init();
    }
}