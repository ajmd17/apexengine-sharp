using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Math;
using ApexEngine.Scene;
using ApexEngine.Rendering;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainChunkNode : TerrainChunkNode
    {
        SimplexTerrainComponent parentT;

        public SimplexTerrainChunkNode(SimplexTerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, SimplexTerrainChunkNode[] neighbors) : base()
        {
            this.x = x;
            this.z = z;
            this.neighbors = neighbors;
            this.scale = scale;
            this.chunkSize = chunkSize;
            this.parentT = parentT;
        }
        
        public override void AddPhysics()
        {
          //  throw new NotImplementedException();
        }

        public override void Create()
        {
            hm = new SimplexTerrainMesh(parentT, x, z, scale, chunkSize);
            Geometry geom = new Geometry();
            geom.Mesh = hm;
           // geom.setShader(TerrainForwardLightingShader.class);
           // geom.setLocalScale(new Vec3f(1,1,1));
          //  geom.getMaterial().set(Material.CULL_FACE, Material.CULL_FACE_BACK);
          //  geom.getMaterial().set(Material.CASTS_SHADOWS, false);
            AddChild(geom);
        }

        public override float GetHeight(Vector3f position)
        {
            if (hm != null)
            {
                try
                {
                    return hm.heights[hm.HeightIndexAt((int)position.x, (int)position.z)] * hm.scale.y;
                }
                catch (Exception ex)
                {
                    return float.NaN;
                }
            }
            return float.NaN;
        }

        public override Vector3f GetNormal(Vector3f position)
        {
            throw new NotImplementedException();
        }

        public override void RemovePhysics()
        {
           // throw new NotImplementedException();
        }
    }
}
