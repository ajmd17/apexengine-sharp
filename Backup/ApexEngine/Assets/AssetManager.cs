using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ApexEngine.Assets
{
    public class AssetManager
    {
        private static Dictionary<string, AssetLoader> loaders = new Dictionary<string, AssetLoader>();
        private static Dictionary<string, object> loadedAssets = new Dictionary<string, object>();

        public static void InitDefaultLoaders()
        {
            // register the default asset loaders
            // create a static instance of each loader
            Assets.SoundLoader.GetInstance();
            Assets.TextureLoader.GetInstance();
            Assets.ShaderTextLoader.GetInstance();
            Assets.TextLoader.GetInstance();
            Assets.Apx.ApxModelLoader.GetInstance();
            Assets.OgreXml.OgreXmlModelLoader.GetInstance();
            Assets.Obj.ObjModelLoader.GetInstance();
            Assets.Obj.MtlAssetLoader.GetInstance();
        }

        public static string GetAppPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static AssetLoader GetDefaultLoader(string ext)
        {
            if (loaders.ContainsKey(ext))
            {
                return loaders[ext];
            }
            else if (loaders.ContainsKey("." + ext))
            {
                return loaders["." + ext];
            }
            if (ext.StartsWith("."))
            {
                if (loaders.ContainsKey(ext.Substring(1)))
                    return loaders[ext.Substring(1)];
            }
            return null;
        }

        public static void RegisterLoader(string ext, AssetLoader loader)
        {
            if (!loaders.ContainsKey(ext))
            {
                loaders.Add(ext, loader);
            }
            else
            {
                loaders[ext] = loader; // replace
            }
        }

        public static object Load(string filePath, AssetLoader loader)
        {
          //  object alreadyLoaded = null;
         //   if (loadedAssets.TryGetValue(filePath, out alreadyLoaded))
          //  {
          //      return alreadyLoaded;
          //  }

            if (!System.IO.File.Exists(filePath))
                throw new System.IO.FileNotFoundException("The file \"" + filePath + "\" does not exist!");
            loader.ResetLoader();


            Stream stream = new FileStream(filePath, FileMode.Open);

            LoadedAsset asset = new LoadedAsset(stream, filePath);

            object loaded = loader.Load(asset);

            stream.Close();

          //  loadedAssets.Add(filePath, loaded);

            return loaded;
        }

        public static object Load(string filePath)
        {
            foreach (string str in loaders.Keys)
            {
                if (filePath.EndsWith(str, StringComparison.OrdinalIgnoreCase))
                {
                    AssetLoader loader = loaders[str];
                    loader.ResetLoader();

                    return Load(filePath, loader);
                }
            }
            throw new KeyNotFoundException("Could not find a registered asset loader for the filetype! File: " + filePath);
        }

        public static Scene.GameObject LoadModel(string filePath)
        {
            object loadedObject = Load(filePath);
            if (loadedObject == null)
                return null;
            if (loadedObject is Scene.GameObject)
                return (Scene.GameObject)loadedObject;
            return null;
        }

        public static Rendering.Texture LoadTexture(string filePath)
        {
            return (Rendering.Texture)Load(filePath, TextureLoader.GetInstance());
        }

        public static string LoadString(string filePath)
        {
            return (string)Load(filePath, TextLoader.GetInstance());
        }

        public static string LoadResourceTextFile(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}