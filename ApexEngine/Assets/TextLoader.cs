using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public override object Load(string filePath)
        {
            string res = System.IO.File.ReadAllText(filePath);
            return res;
        }
    }
}
