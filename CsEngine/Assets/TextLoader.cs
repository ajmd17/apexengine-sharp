using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Assets
{
    public class TextLoader : AssetLoader
    {
        public override object Load(string filePath)
        {
            string res = System.IO.File.ReadAllText(filePath);
            return res;
        }
    }
}
