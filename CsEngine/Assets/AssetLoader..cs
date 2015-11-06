using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Assets
{
    public abstract class AssetLoader
    {
        public abstract Object Load(string filePath);
    }
}
