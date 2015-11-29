using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Rendering;
namespace ApexEngine.Terrain
{
    public class TerrainMaterial : Material
    {
        public const string TEXTURE_DIFFUSE0 = Material.TEXTURE_DIFFUSE;
        public const string TEXTURE_DIFFUSE1 = Material.TEXTURE_DIFFUSE + "_1";
        public const string TEXTURE_DIFFUSE2 = Material.TEXTURE_DIFFUSE + "_2";
        public const string TEXTURE_DIFFUSE3 = Material.TEXTURE_DIFFUSE + "_3";

        public const string TEXTURE_NORMAL0 = Material.TEXTURE_NORMAL;
        public const string TEXTURE_NORMAL1 = Material.TEXTURE_NORMAL + "_1";
        public const string TEXTURE_NORMAL2 = Material.TEXTURE_NORMAL + "_2";
        public const string TEXTURE_NORMAL3 = Material.TEXTURE_NORMAL + "_3";

        public const string TEXTURE_DIFFUSE_SLOPE = "slope_map";
        public const string TEXTURE_NORMAL_SLOPE = "slope_normal_map";
        public const string TERRAIN_SLOPE_TOLERANCE = "tolerance";

        public TerrainMaterial() : base()
        {

        }
    }
}
