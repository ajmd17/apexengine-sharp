using ApexEngine.Math;
using ApexEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Plugins.PagingSystem
{

    public class Patch
    {
        public PageState pageState = PageState.UNLOADED;
        public GridTile tile;
        public GameObject entities;
        public Node parentNode;
        private float cacheTime = 0f;
        private float maxCacheTime = 1.5f;

        public Vector3f translation;
        public int entityPerChunk;
        public float chunkSize;

        public enum PageState
        {
            LOADED, UNLOADING, UNLOADED
        }

        public Patch(Node parentNode, GridTile tile)
        {
            this.tile = tile;
            this.parentNode = parentNode;
        }

        public void Update(Vector3f cam)
        {
            if (this.pageState == PageState.UNLOADING)
            {
                if (this.cacheTime > maxCacheTime)
                {
                    this.pageState = PageState.UNLOADED;
                    cacheTime = 0f;
                }
                else
                {
                    cacheTime += 0.2f;
                }
            }
        }
    }

    public class GridTile
    {
        public float width, length, x, z, maxDistance;
        public Vector2f center;

        public GridTile(Vector2f center, float width, float length, float x, float z, float maxDistance)
        {
            this.center = new Vector2f(x + (width / 2f), z + (length / 2f));
            this.width = width;
            this.length = length;
            this.x = x;
            this.z = z;
            this.maxDistance = maxDistance;
        }

        public bool inRange(Vector2f point)
        {
            float dist = center.Distance(point);
            return dist < maxDistance;
        }
    }
}
