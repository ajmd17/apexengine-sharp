using System.IO;

namespace ApexEngine.Assets
{
    public class LoadedAsset
    {
        private Stream data;
        private string filePath;

        public Stream Data
        {
            get { return data; }
            set { data = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public LoadedAsset(Stream data, string filePath)
        {
            this.data = data;
            this.filePath = filePath;
        }
    }
}