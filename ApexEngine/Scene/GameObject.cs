using ApexEngine.Math;
using System;
using System.Collections.Generic;

namespace ApexEngine.Scene
{
    public abstract class GameObject
    {
        protected Rendering.RenderManager renderManager = null;
        private bool updateNeeded = true;
        protected List<Components.Controller> controls = new List<Components.Controller>();
        protected Transform worldTransform = new Transform();
        protected Vector3f localTranslation;
        protected Vector3f localScale;
        protected Quaternion localRotation;
        protected Matrix4f worldMatrix = new Matrix4f();
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
                ctrl.GameObject = this;
                ctrl.Init();
            }
        }

        public void RemoveController(Components.Controller ctrl)
        {
            if (controls.Contains(ctrl))
            {
                ctrl.GameObject = null;
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

        public Vector3f GetUpdatedWorldTranslation()
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

        public Vector3f GetUpdatedWorldScale()
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

        public Quaternion GetUpdatedWorldRotation()
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

        public virtual void Update(Rendering.RenderManager renderManager)
        {
            this.renderManager = renderManager;
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
            if (!HasController(typeof(Physics.RigidBodyControl)))
            {
                worldTransform.SetTranslation(worldTrans);
                worldTransform.SetRotation(worldRot);
                worldTransform.SetScale(worldScale);
            }
            else
            {
                Vector3f diff = GetPhysicsOrigin().Subtract(worldTrans);
                SetPhysicsTranslation(GetPhysicsTranslation().Subtract(diff));
            }

            worldMatrix = worldTransform.GetMatrix();
        }

        public virtual void UpdateParents()
        {
            attachedToRoot = CalcAttachedToRoot();
        }

        Vector3f GetPhysicsTranslation()
        {
            Physics.RigidBodyControl rbc = (Physics.RigidBodyControl)GetController(typeof(Physics.RigidBodyControl));
            return rbc.GetTranslation();
        }

        Vector3f GetPhysicsOrigin()
        {
            Physics.RigidBodyControl rbc = (Physics.RigidBodyControl)GetController(typeof(Physics.RigidBodyControl));
            return rbc.Origin;
        }

        public void SetPhysicsTranslation(Vector3f trans)
        {
            foreach (Components.Controller sc in controls)
            {
                if (sc is Physics.RigidBodyControl)
                 ((Physics.RigidBodyControl)sc).SetTranslation(trans);
            }
        }
        
        public virtual void SetWorldTransformPhysics(Vector3f trans, Quaternion rot, Vector3f scl)
        {
            worldTransform.SetTranslation(trans);
            worldTransform.SetRotation(rot);
            worldTransform.SetScale(scl);
            worldMatrix = worldTransform.GetMatrix();
        }

        public bool AttachedToRoot
        {
            get { return attachedToRoot; }
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