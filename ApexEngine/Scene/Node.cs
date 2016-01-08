using ApexEngine.Math;
using System.Collections.Generic;
using System;

namespace ApexEngine.Scene
{
    public class Node : GameObject
    {
        protected List<GameObject> children = new List<GameObject>();
        protected BoundingBox worldBoundingBox, localBoundingBox;

        public Node() : base()
        {
        }

        public Node(string name)
            : base(name)
        {
        }

        public override BoundingBox GetWorldBoundingBox()
        {
            if (worldBoundingBox == null)
            {
                worldBoundingBox = new BoundingBox();
                UpdateWorldBoundingBox();
            }
            return worldBoundingBox;
        }

        public override BoundingBox GetLocalBoundingBox()
        {
            if (localBoundingBox == null)
            {
                localBoundingBox = new BoundingBox();
                UpdateLocalBoundingBox();
            }
            return localBoundingBox;
        }

        public override void UpdateWorldBoundingBox()
        {
            if (worldBoundingBox != null)
            {
                worldBoundingBox.Clear();
                foreach (GameObject child in children)
                {
                    worldBoundingBox.Extend(child.GetWorldBoundingBox());
                }
            }
        }

        public override void UpdateLocalBoundingBox()
        {

            if (localBoundingBox != null)
            {
                localBoundingBox.Clear();
                foreach (GameObject child in children)
                {
                    localBoundingBox.Extend(child.GetLocalBoundingBox());
                }
            }
        }

        public List<GameObject> Children
        {
            get { return children; }
        }

        public void AddChild(GameObject child)
        {
            children.Add(child);
            child.SetParent(this);
        }

        public void RemoveChild(GameObject child)
        {
            children.Remove(child);
            child.SetParent(null);
            child.Update(renderManager);
        }

        public GameObject GetChild(int i)
        {
            return children[i];
        }

        public Geometry GetChildGeom(int i)
        {
            return (Geometry)children[i];
        }

        public Node GetChildNode(int i)
        {
            return (Node)children[i];
        }

        public GameObject GetChild(string name)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Name == name)
                {
                    return children[i];
                }
            }
            return null;
        }

        public Geometry GetChildGeom(string name)
        {
            return (Geometry)GetChild(name);
        }

        public Node GetChildNode(string name)
        {
            return (Node)GetChild(name);
        }

        public override void SetWorldTransformPhysics(Vector3f trans, Quaternion rot, Vector3f scl)
        {
            base.SetWorldTransformPhysics(trans, rot, scl);
            foreach (GameObject gi in children)
                gi.SetWorldTransformPhysics(trans.Add(gi.GetLocalTranslation()), rot.Multiply(gi.GetLocalRotation()), scl.Multiply(gi.GetLocalScale()));
        }

        public override void Update(Rendering.RenderManager renderManager)
        {
            base.Update(renderManager);
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(renderManager);
            }
        }

        public override void SetUpdateNeeded()
        {
            base.SetUpdateNeeded();
            for (int i = 0; i < children.Count; i++)
            {
                children[i].SetUpdateNeeded();
            }
        }

        public override void Dispose()
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Dispose();
            }
        }

        public override GameObject Clone()
        {
            Node res = new Node(this.name);
            res.SetLocalTranslation(this.GetLocalTranslation());
            res.SetLocalScale(this.GetLocalScale());
            res.SetLocalRotation(this.GetLocalRotation());
            for (int i = 0; i< children.Count; i++)
            {
                res.AddChild(children[i].Clone());
            }
            for (int i = 0; i < controls.Count; i++)
            {
                res.AddController(controls[i]);
            }
            return res;
        }
    }
}