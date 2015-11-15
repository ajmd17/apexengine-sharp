using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public override object Load(string filePath)
        {
            return Rendering.Texture.LoadTexture(filePath);
        }

        public override void ResetLoader()
        {
            throw new NotImplementedException();
        }
    }
}
