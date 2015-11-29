using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Math;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using ApexEngine.Rendering;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class SimplexTerrainChunkNode : TerrainChunkNode
    {

        public SimplexTerrainChunkNode(PhysicsWorld physicsWorld, SimplexTerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, SimplexTerrainChunkNode[] neighbors) 
            : base(physicsWorld, parentT, x, z, scale, chunkSize, neighbors)
        {
        }
        
        public override void AddPhysics()
        {
            if (physicsWorld != null)
                physicsWorld.AddObject(this, 0);
        }

        public override void RemovePhysics()
        {
            if (physicsWorld != null)
                physicsWorld.RemoveObject(this);
        }

        public override void Create()
        {
            this.Name = "simplexterrain_" + x.ToString() + "_" + z.ToString();
            hm = new SimplexTerrainMesh((SimplexTerrainComponent)parentT, x, z, scale, chunkSize);
            Geometry geom = new Geometry();
            geom.Mesh = hm;
            geom.SetShader(ShaderManager.GetShader(typeof(TerrainShader)));
            AddChild(geom);
        }
    }
}
