using ApexEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Terrain.Ecosystem
{
    public interface BiomeTerrain
    {
        Biome GetBiome(Vector3f worldPosition);
    }
}
