namespace ApexEngine.Scene.Components
{
    public abstract class Controller
    {
        private GameObject gameObject;

        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }

        public abstract void Init();

        public abstract void Update();
    }
}