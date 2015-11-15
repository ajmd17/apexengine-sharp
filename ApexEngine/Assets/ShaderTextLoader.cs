using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Assets
{
    public class ShaderTextLoader : AssetLoader
    {
        private static ShaderTextLoader instance = new ShaderTextLoader();
        public static ShaderTextLoader GetInstance()
        {
            return instance;
        }
        public ShaderTextLoader() : base ("vert", "frag", "geom", "comp", "tesc", "tese", "glsl", "gl", "glh")
        {

        }
        public override void ResetLoader()
        {
        }
        public override object Load(string filePath)
        {
            string res = ApexEngine.Rendering.Util.ShaderUtil.FormatShaderIncludes(filePath, System.IO.File.ReadAllText(filePath));
            return res;
        }
    }
}
