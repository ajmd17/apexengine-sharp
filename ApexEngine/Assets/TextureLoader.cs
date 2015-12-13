using System;

namespace ApexEngine.Assets
{
    public class TextureLoader : AssetLoader
    {
        private static TextureLoader instance = new TextureLoader();

        public static TextureLoader GetInstance()
        {
            return instance;
        }

        public TextureLoader() : base("png", "jpg", "jpeg", "bmp", "tiff", "gif")
        {
        }

        public override object Load(LoadedAsset asset)
        {
            return Rendering.Texture.LoadTexture(asset.FilePath);
        }

        public override void ResetLoader()
        {
            throw new NotImplementedException();
        }
    }
}