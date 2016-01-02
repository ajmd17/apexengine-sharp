using ApexEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Assets
{
    public class SoundLoader : AssetLoader
    {
        private static SoundLoader instance = new SoundLoader();

        public static SoundLoader GetInstance()
        {
            return instance;
        }

        public SoundLoader() : base("wav")
        {

        }

        public override object Load(LoadedAsset asset)
        {
            return RenderManager.Renderer.LoadAudio(asset);
        }

        public override void ResetLoader()
        {
        }
    }
}
