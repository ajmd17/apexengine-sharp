using ApexEngine.Math;
using ApexEngine.Scene;
using System.Collections.Generic;

namespace ApexEngine.Rendering.Util
{
    public static class RenderUtil
    {
        public static List<Mesh> GatherMeshes(GameObject gameObject)
        {
            List<Mesh> meshes = new List<Mesh>();

            if (gameObject is Node)
            {
                GatherMeshes((Node)gameObject, meshes);
            }
            else if (gameObject is Geometry)
            {
                meshes.Add(((Geometry)gameObject).Mesh);
            }

            return meshes;
        }

        public static List<Mesh> GatherMeshes(GameObject gameObject, List<Matrix4f> worldTransforms)
        {
            List<Mesh> meshes = new List<Mesh>();
            if (worldTransforms == null)
                worldTransforms = new List<Matrix4f>();

            if (gameObject is Node)
            {
                GatherMeshes((Node)gameObject, meshes, worldTransforms);
            }
            else if (gameObject is Geometry)
            {
                meshes.Add(((Geometry)gameObject).Mesh);
                Transform ttransform = new Transform();
                ttransform.SetTranslation(gameObject.GetUpdatedWorldTranslation());
                ttransform.SetRotation(gameObject.GetUpdatedWorldRotation());
                ttransform.SetScale(gameObject.GetUpdatedWorldScale());
                Matrix4f matrix = ttransform.GetMatrix();
                worldTransforms.Add(matrix);
            }

            return meshes;
        }

        private static void GatherMeshes(Node node, List<Mesh> meshes)
        {
            foreach (GameObject child in node.Children)
            {
                if (child is Node)
                {
                    GatherMeshes((Node)child, meshes);
                }
                else if (child is Geometry)
                {
                    meshes.Add(((Geometry)child).Mesh);
                }
            }
        }

        private static void GatherMeshes(Node node, List<Mesh> meshes, List<Matrix4f> worldTransforms)
        {
            foreach (GameObject child in node.Children)
            {
                if (child is Node)
                {
                    GatherMeshes((Node)child, meshes, worldTransforms);
                }
                else if (child is Geometry)
                {
                    meshes.Add(((Geometry)child).Mesh);
                    Transform ttransform = new Transform();
                    ttransform.SetTranslation(child.GetUpdatedWorldTranslation());
                    ttransform.SetRotation(child.GetUpdatedWorldRotation());
                    ttransform.SetScale(child.GetUpdatedWorldScale());
                    Matrix4f matrix = ttransform.GetMatrix();
                    worldTransforms.Add(matrix);
                }
            }
        }

        public static List<Geometry> GatherGeometry(GameObject gameObject)
        {
            List<Geometry> geoms = new List<Geometry>();

            if (gameObject is Node)
            {
                GatherGeometry((Node)gameObject, geoms);
            }
            else if (gameObject is Geometry)
            {
                geoms.Add((Geometry)gameObject);
            }

            return geoms;
        }

        private static void GatherGeometry(Node node, List<Geometry> geoms)
        {
            foreach (GameObject child in node.Children)
            {
                if (child is Node)
                {
                    GatherGeometry((Node)child, geoms);
                }
                else if (child is Geometry)
                {
                    geoms.Add((Geometry)child);
                }
            }
        }

        public static List<GameObject> GatherObjects(GameObject gameObject)
        {
            List<GameObject> objs = new List<GameObject>();

            objs.Add(gameObject);
            if (gameObject is Node)
            {
                GatherObjects((Node)gameObject, objs);
            }

            return objs;
        }

        private static void GatherObjects(Node node, List<GameObject> objs)
        {
            foreach (GameObject child in node.Children)
            {
                if (child is Node)
                {
                    objs.Add(child);
                    GatherObjects((Node)child, objs);
                }
                else if (child is Geometry)
                {
                    objs.Add(child);
                }
            }
        }

        public static void RenderBoundingBox(BoundingBox boundingBox)
        {
            float offset = 0.1f;
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Max.z + offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Max.x + offset, boundingBox.Min.y - offset, boundingBox.Min.z - offset);
            RenderManager.Renderer.DrawVertex(boundingBox.Min.x - offset, boundingBox.Max.y + offset, boundingBox.Min.z - offset);
        }
    }
}