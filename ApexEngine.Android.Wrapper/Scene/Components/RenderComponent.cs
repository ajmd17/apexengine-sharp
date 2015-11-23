using ApexEngine.Rendering;

namespace ApexEngine.Scene.Components
{
    public abstract class RenderComponent : EngineComponent
    {
        public RenderManager renderManager;

        public abstract void Init();

        public abstract void Render();

        public abstract void Update();
    }
}