using ApexEngine.Rendering;

namespace ApexEngine.Scene.Components
{
    public abstract class GameComponent : EngineComponent
    {
        public Node rootNode = new Node("GameComponent Node");
        public Camera cam;

        public abstract void Update();

        public abstract void Init();
    }
}