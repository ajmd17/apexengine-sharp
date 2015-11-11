using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApexEngine.Assets
{
    public class AssetManager
    {
        private static Dictionary<string, AssetLoader> loaders = new Dictionary<string, AssetLoader>();
        public static void InitDefaultLoaders()
        {
            // register the default asset loaders
            // create a static instance of each loader
            Assets.TextureLoader.GetInstance();
            Assets.ShaderTextLoader.GetInstance();
            Assets.TextLoader.GetInstance();
            Assets.Apx.ApxModelLoader.GetInstance();
            Assets.OgreXml.OgreXmlModelLoader.GetInstance();
            Assets.Obj.ObjModelLoader.GetInstance();
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
            if (!System.IO.File.Exists(filePath))
                throw new System.IO.FileNotFoundException("The file \"" + filePath + "\" does not exist!");
            loader.ResetLoader();
            return loader.Load(filePath);
        }
        public static object Load(string filePath)
        {
            foreach (string str in loaders.Keys)
            {
                if (filePath.EndsWith(str, StringComparison.OrdinalIgnoreCase))
                    return loaders[str].Load(filePath);
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
