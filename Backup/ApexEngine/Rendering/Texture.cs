using ApexEngine.Assets;

namespace ApexEngine.Rendering
{
    public abstract class Texture
    {
        protected int id;

        public enum WrapMode
        {
            Repeat,
            ClampToBorder,
            ClampToEdge
        }

        public enum FilterMode
        {
            Linear,
            Nearest,
            Mipmap
        }

        public static Texture LoadTexture(LoadedAsset asset)
        {
            return RenderManager.Renderer.LoadTexture2D(asset);
        }

        public static int GenTextureID()
        {
            return RenderManager.Renderer.GenTexture();
        }

        public Texture(int id)
        {
            this.id = id;
        }

        public int GetID()
        {
            return this.id;
        }

        public void SetID(int id)
        {
            this.id = id;
        }

        public static void ActiveTextureSlot(int slot)
        {
            RenderManager.Renderer.ActiveTextureSlot(slot);
        }

        public abstract void Use();

        public abstract void SetFilter(int min, int mag);

        public abstract void GenerateMipmap();
    }
}