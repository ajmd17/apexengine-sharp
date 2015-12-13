namespace ApexEngine.Rendering
{
    public class Texture2D : Texture
    {
        protected string texturePath = "";
        protected int width, height;

        public Texture2D(int id) : base(id)
        {
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public string TexturePath
        {
            get { return texturePath; }
            set { texturePath = value; }
        }

        public static void Clear()
        {
            RenderManager.Renderer.BindTexture2D(0);
        }

        public override void Use()
        {
            RenderManager.Renderer.BindTexture2D(id);
        }

        public void SetWrap(int s, int t)
        {
            Use();
            RenderManager.Renderer.TextureWrap2D(WrapMode.Repeat, WrapMode.Repeat);
        }

        public override void SetFilter(int min, int mag)
        {
            Use();
            RenderManager.Renderer.TextureFilter2D(min, mag);
        }

        public override void GenerateMipmap()
        {
            Use();
            RenderManager.Renderer.GenerateMipmap2D();
        }

        public override string ToString()
        {
            return texturePath;
        }
    }
}