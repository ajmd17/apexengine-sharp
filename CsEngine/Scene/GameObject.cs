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
        protected List<Components.Controller> controls = new List<Components.Controller>();
        protected Vector3f localTranslation;
        protected Vector3f localScale;
        protected Quaternion localRotation;
        protected Transform worldTransform;
        protected Matrix4f worldMatrix;
        protected bool attachedToRoot = false;
        protected string name = "";
        protected Node parent;
        private Vector3f wtrans = new Vector3f(), wscl = new Vector3f();
        private Quaternion wrot = new Quaternion();
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
        public void AddController(Components.Controller ctrl)
        {
            if (!controls.Contains(ctrl))
            {
                controls.Add(ctrl);
                ctrl.Init();
            }
        }
        public void RemoveController(Components.Controller ctrl)
        {
            if (controls.Contains(ctrl))
            {
                controls.Remove(ctrl);
            }
        }
        public bool HasController(Type ctrlType)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].GetType() == ctrlType)
                {
                    return true;
                }
            }
            return false;
        }
        public Components.Controller GetController(Type ctrlType)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].GetType() == ctrlType)
                {
                    return controls[i];
                }
            }
            return null;
        }
        public string Name
        {
            get { return this.name; }
            set { name = value; }
        }
        public virtual void SetParent(Node newParent)
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
        public virtual Vector3f GetLocalTranslation()
        {
            return localTranslation;
        }
        public virtual Vector3f LocalTranslation
        {
            get { return localTranslation; }
            set { localTranslation.Set(value); SetUpdateNeeded(); }
        }
        public virtual Vector3f GetWorldTranslation()
        {
            return worldTransform.GetTranslation();
        }
        public virtual Vector3f WorldTranslation
        {
            get { return worldTransform.GetTranslation(); }
        }
        public virtual void SetLocalTranslation(Vector3f vec)
        {
            localTranslation.Set(vec);
            SetUpdateNeeded();
        }
        public virtual Vector3f GetLocalScale()
        {
            return localScale;
        }
        public virtual Vector3f GetWorldScale()
        {
            return worldTransform.GetScale();
        }
        public virtual Vector3f WorldScale
        {
            get { return worldTransform.GetScale(); }
        }
        public virtual void SetLocalScale(Vector3f vec)
        {
            localScale.Set(vec);
            SetUpdateNeeded();
        }
        public virtual Vector3f LocalScale
        {
            get { return localScale; }
            set { localScale.Set(value); SetUpdateNeeded(); }
        }
        public virtual Quaternion GetLocalRotation()
        {
            return localRotation;
        }
        public virtual Quaternion LocalRotation
        {
            get { return localRotation; }
            set { localRotation.Set(value); SetUpdateNeeded(); }
        }
        public virtual Quaternion GetWorldRotation()
        {
            return worldTransform.GetRotation();
        }
        public virtual Quaternion WorldRotation
        {
            get { return worldTransform.GetRotation(); }
        }
        public virtual void SetLocalRotation(Quaternion quat)
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
            wtrans.Set(0, 0, 0);
	        UpdateWorldTranslation(wtrans);
	        return wtrans;
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
            wscl.Set(1, 1, 1);
            UpdateWorldScale(wscl);
            return wscl;
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
            wrot.SetToIdentity();
            UpdateWorldRotation(wrot);
            return wrot;
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
            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].Update();
            }
        }
        public virtual void UpdateTransform()
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
                while (par.Name != "root")
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
