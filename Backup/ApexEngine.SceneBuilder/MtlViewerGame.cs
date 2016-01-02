using ApexEngine;
using ApexEngine.Math;

namespace ApexEditor
{
    public class MtlViewerGame : Game
    {
        private bool rotate = true;
        private Quaternion rot = new Quaternion();
        private float rotTime = 0f;

        public MtlViewerGame() : base(new ApexEngine.Rendering.OpenGL.GLRenderer())
        {
            Environment.AmbientLight.Color.Set(0, 0, 0, 1);
        }

        public bool Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }

        public override void Init()
        {
            this.Environment.DirectionalLight.Direction.Set(-1, 1, -1);
        }

        public override void Render()
        {
        }

        public override void Update()
        {
            if (rotate)
            {
                rotTime += 1f;
                rot.SetFromAxis(Vector3f.UnitY, rotTime);
                rootNode.GetChild(0).SetLocalRotation(rot);
            }
        }
    }
}