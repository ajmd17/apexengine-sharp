using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Scene
{
    public class Node : GameObject
    {
        protected List<GameObject> children = new List<GameObject>();
        public Node() : base()
        {

        }
        public Node(string name)
            : base(name)
        {

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
    }
}
