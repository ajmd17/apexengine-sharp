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
            string res = System.IO.File.ReadAllText(asset.FilePath);
            return res;
        }
    }
}