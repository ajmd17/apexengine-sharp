using System.IO;

namespace ApexEngine.Assets
{
    public class LoadedAsset
    {
        private object data;
        private string filePath;

        public object Data
        {
            get { return data; }
            set { data = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public LoadedAsset(object data, string filePath)
        {
            this.data = data;
            this.filePath = filePath;
        }
    }
}