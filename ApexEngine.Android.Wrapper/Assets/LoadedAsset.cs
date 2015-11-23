using System.IO;

namespace ApexEngine.Assets
{
    public class LoadedAsset
    {
        private Stream stream;
        private string filePath;

        public Stream Stream
        {
            get { return stream; }
            set { stream = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public LoadedAsset(Stream stream, string filePath)
        {
            this.stream = stream;
            this.filePath = filePath;
        }
    }
}