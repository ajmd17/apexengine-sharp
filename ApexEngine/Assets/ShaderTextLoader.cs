namespace ApexEngine.Assets
{
    public class ShaderTextLoader : AssetLoader
    {
        private static ShaderTextLoader instance = new ShaderTextLoader();

        public static ShaderTextLoader GetInstance()
        {
            return instance;
        }

        public ShaderTextLoader() : base("vert", "frag", "geom", "comp", "tesc", "tese", "glsl", "gl", "glh")
        {
        }

        public override void ResetLoader()
        {
        }

        public override object Load(LoadedAsset asset)
        {
            string res = ApexEngine.Rendering.Util.ShaderUtil.FormatShaderIncludes(asset.FilePath, System.IO.File.ReadAllText(asset.FilePath));
            return res;
        }
    }
}