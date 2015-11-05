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
        public List<GameObject> GetChildren()
        {
            return children;
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
        }
        public GameObject GetChild(int i)
        {
            return children[i];
        }
        public GameObject GetChild(string name)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].GetName() == name)
                {
                    return children[i];
                }
            }
            return null;
        }
        public override void Update()
        {
            base.Update();
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update();
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
