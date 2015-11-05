using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Scene
{
    public abstract class GameObject
    {
        private bool updateNeeded = true;
        protected Vector3f localTranslation;
        protected Vector3f localScale;
        protected Quaternion localRotation;
        protected Transform worldTransform;
        protected Matrix4f worldMatrix;
        protected bool attachedToRoot = false;
        protected string name = "";
        protected Node parent;
        public GameObject()
        {
            localTranslation = new Vector3f(0, 0, 0);
            localScale = new Vector3f(1, 1, 1);
            localRotation = new Quaternion(0, 0, 0, 1);
            worldTransform = new Transform();
            parent = null;
        }
        public GameObject(string name)
        {
            this.name = name;
            localTranslation = new Vector3f(0, 0, 0);
            localScale = new Vector3f(1, 1, 1);
            localRotation = new Quaternion(0, 0, 0, 1);
            worldTransform = new Transform();
            parent = null;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return this.name;
        }
        public void SetParent(Node newParent)
        {
            this.parent = newParent;
            SetUpdateNeeded();
            if (parent == null)
            {
                UpdateParents();
            }
        }
        public Node GetParent()
        {
            return parent;
        }
        public Vector3f GetLocalTranslation()
        {
            return localTranslation;
        }
        public Vector3f GetWorldTranslation()
        {
            return worldTransform.GetTranslation();
        }
        public void SetLocalTranslation(Vector3f vec)
        {
            localTranslation.Set(vec);
            SetUpdateNeeded();
        }
        public Vector3f GetLocalScale()
        {
            return localScale;
        }
        public Vector3f GetWorldScale()
        {
            return worldTransform.GetScale();
        }
        public void SetLocalScale(Vector3f vec)
        {
            localScale.Set(vec);
            SetUpdateNeeded();
        }
        public Quaternion GetLocalRotation()
        {
            return localRotation;
        }
        public Quaternion GetWorldRotation()
        {
            return worldTransform.GetRotation();
        }
        public void SetLocalRotation(Quaternion quat)
        {
            localRotation.Set(quat);
            SetUpdateNeeded();
        }
        protected void UpdateWorldTranslation(Vector3f outw)
        {
	        outw.AddStore(localTranslation);
	        if (parent != null)
	        {
		        Node p = parent;
		        p.UpdateWorldTranslation(outw);
	        }
        }
        protected Vector3f GetUpdatedWorldTranslation()
        {
	        Vector3f wTrans = new Vector3f();
	        UpdateWorldTranslation(wTrans);
	        return wTrans;
        }
        protected void UpdateWorldScale(Vector3f outw)
        {
            outw.MultiplyStore(localScale);
            if (parent != null)
            {
                Node p = parent;
                p.UpdateWorldScale(outw);
            }
        }
        protected Vector3f GetUpdatedWorldScale()
        {
            Vector3f wScl = new Vector3f(1, 1, 1);
            UpdateWorldScale(wScl);
            return wScl;
        }
        protected void UpdateWorldRotation(Quaternion outw)
        {
            outw.MultiplyStore(localRotation);
            if (parent != null)
            {
                Node p = parent;
                p.UpdateWorldRotation(outw);
            }
        }
        protected Quaternion GetUpdatedWorldRotation()
        {
            Quaternion wRot = new Quaternion(0, 0, 0, 1);
            UpdateWorldRotation(wRot);
            return wRot;
        }
        public Matrix4f GetWorldMatrix()
        {
            return worldMatrix;
        }
        public virtual void SetUpdateNeeded()
        {
            updateNeeded = true;
        }
        public virtual void Update()
        {
            if (updateNeeded)
            {
                UpdateTransform();
                UpdateParents();
                updateNeeded = false;
            }
        }
        protected void UpdateTransform()
        {
            Vector3f worldTrans = GetUpdatedWorldTranslation();
            Quaternion worldRot = GetUpdatedWorldRotation();
            Vector3f worldScale = GetUpdatedWorldScale();

            worldTransform.SetTranslation(worldTrans);
            worldTransform.SetRotation(worldRot);
            worldTransform.SetScale(worldScale);

            worldMatrix = worldTransform.GetMatrix();
        }
        public virtual void UpdateParents()
        {
            attachedToRoot = CalcAttachedToRoot();
        }
        protected bool CalcAttachedToRoot()
        {
            if (name == "root")
                return true;
            if (parent == null)
                return false;
            else
            {
                Node par = parent;
                while (par.GetName() != "root")
                {
                    Node pp = par.GetParent();
                    if (pp != null)
                    {
                        par = pp;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
