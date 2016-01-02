using System.IO;

namespace ApexEngine.Assets
{
    public class TextLoader : AssetLoader
    {
        private static TextLoader instance = new TextLoader();

        public static TextLoader GetInstance()
        {
            return instance;
        }

        public TextLoader() : base("txt")
        {
        }

        public override void ResetLoader()
        {
        }

        public override object Load(LoadedAsset asset)
        {
            string text = "";
            StreamReader reader = new StreamReader(asset.Data);
            text = reader.ReadToEnd();
            reader.Close();
            return text;
        }
    }
}