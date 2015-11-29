using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ApexEngine.Rendering;
using ApexEngine.Math;
namespace ApexEngine.Assets.Obj
{
    public class MtlAssetLoader : AssetLoader
    {
        private static MtlAssetLoader instance = new MtlAssetLoader();
        public static MtlAssetLoader GetInstance()
        {
            return instance;
        }
        private List<Material> materials = new List<Material>();
        public MtlAssetLoader() : base("mtl")
        { }
        public static string[] RemoveEmptyStrings(string[] data)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != "")
                {
                    result.Add(data[i]);
                }
            }
            string[] res = result.ToArray();
            return res;
        }
        public override object Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return materials;
            }
            StreamReader reader = File.OpenText(filePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] tokens = line.Split(' ');
                tokens = RemoveEmptyStrings(tokens);
                if (tokens.Length == 0 || tokens[0] == "#")
                {
                }
                else if (tokens[0] == "newmtl")
                {
                    string name = tokens[1];
                    materials.Add(new Material());
                    materials[materials.Count - 1].SetName(name);
                }
                else if (tokens[0] == "Ka") // ambient color
                {
                    string x_str = tokens[1];
                    string y_str = tokens[2];
                    string z_str = tokens[3];
                    float x, y, z;
                    x = float.Parse(x_str);
                    y = float.Parse(y_str);
                    z = float.Parse(z_str);
                    Vector4f color = new Vector4f(x, y, z, 1.0f);
                   // materials[materials.Count - 1].SetValue(Material.COLOR_AMBIENT, color);
                }
                else if (tokens[0] == "Kd") // ambient color
                {
                    string x_str = tokens[1];
                    string y_str = tokens[2];
                    string z_str = tokens[3];
                    float x, y, z;
                    x = float.Parse(x_str);
                    y = float.Parse(y_str);
                    z = float.Parse(z_str);
                    Vector4f color = new Vector4f(x, y, z, 1.0f);
                  //  materials[materials.Count - 1].SetValue(Material.COLOR_DIFFUSE, color);
                }
                else if (tokens[0] == "Ks") // ambient color
                {
                    string x_str = tokens[1];
                    string y_str = tokens[2];
                    string z_str = tokens[3];
                    float x, y, z;
                    x = float.Parse(x_str);
                    y = float.Parse(y_str);
                    z = float.Parse(z_str);
                    Vector4f color = new Vector4f(x, y, z, 1.0f);
                   // materials[materials.Count - 1].SetValue(Material.COLOR_SPECULAR, color);
                }
                else if (tokens[0] == "Ns") // ambient color
                {
                    string spec = tokens[1];
                    float spec_f = float.Parse(spec);
                    materials[materials.Count - 1].SetValue(Material.SPECULAR_EXPONENT, spec_f / 4f);
                }
                else if (tokens[0].ToLower() == "map_kd") // diffuse map
                {
                    string texName = tokens[tokens.Length - 1];
                    string parentPath = System.IO.Directory.GetParent(filePath).ToString();
                    string texPath = parentPath + "\\" + texName;
                    if (File.Exists(texPath))
                    {
                        Texture tex = Texture.LoadTexture(texPath);
                        materials[materials.Count - 1].SetValue(Material.TEXTURE_DIFFUSE, tex);
                    }
                    else if (File.Exists(texName))
                    {
                        Texture tex = Texture.LoadTexture(texName);
                        materials[materials.Count - 1].SetValue(Material.TEXTURE_DIFFUSE, tex);
                    }
                }
                else if (tokens[0].ToLower() == "map_bump") // normal map
                {
                    string texName = tokens[tokens.Length - 1];
                    string parentPath = System.IO.Directory.GetParent(filePath).ToString();
                    string texPath = parentPath + "\\" + texName;
                    if (File.Exists(texPath))
                    {
                        Texture tex = Texture.LoadTexture(texPath);
                        materials[materials.Count - 1].SetValue(Material.TEXTURE_NORMAL, tex);
                    }
                    else if (File.Exists(texName))
                    {
                        Texture tex = Texture.LoadTexture(texName);
                        materials[materials.Count - 1].SetValue(Material.TEXTURE_NORMAL, tex);
                    }
                }
            }
            return materials;
        }

        public override void ResetLoader()
        {
            materials.Clear();
        }
    }
}
