using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Assets
{
    public class ShaderTextLoader : AssetLoader
    {
        public override object Load(string filePath)
        {
            string res = ApexEngine.Rendering.Util.ShaderUtil.FormatShaderIncludes(filePath, System.IO.File.ReadAllText(filePath));
            return res;
        }
    }
}
