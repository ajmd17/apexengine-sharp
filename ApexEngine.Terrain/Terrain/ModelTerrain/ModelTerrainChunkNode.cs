using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Math;
using ApexEngine.Scene;
using ApexEngine.Scene.Physics;
using ApexEngine.Rendering;

namespace ApexEngine.Terrain.ModelTerrain
{
    public class ModelTerrainChunkNode : TerrainChunkNode
    {
        private GameObject objToAdd;

        public ModelTerrainChunkNode(GameObject objToAdd, PhysicsWorld physicsWorld, ModelTerrainComponent parentT, int x, int z, Vector3f scale, int chunkSize, ModelTerrainChunkNode[] neighbors) 
            : base(physicsWorld, parentT, x, z, scale, chunkSize, neighbors)
        {
            this.objToAdd = objToAdd;
        }

        public override void AddPhysics()
        {
        }

        public override void Create()
        {
            if (objToAdd != null)
            {
                AddChild(objToAdd);
            }
                
        }

        public override void RemovePhysics()
        {
        }
    }
}
