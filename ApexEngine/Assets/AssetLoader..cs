using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Assets
{
    public abstract class AssetLoader
    {
        public abstract Object Load(string filePath);
        public abstract void ResetLoader();
        public AssetLoader(params string[] extensions)
        {
            foreach (string ext in extensions)
            {
                AssetManager.RegisterLoader(ext, this);
            }
        }
    }
}
