using ApexEngine.Math;
using ApexEngine.Scene;

namespace ApexEngine.Rendering.Animation
{
    public class Bone : Node
    {
        private Matrix4f tmpMatrix = new Matrix4f();
        protected Bone parentBone = null;
        private Vector3f modelPos = new Vector3f();
        private Vector3f axis = new Vector3f();
        private float angle = 0f;
        private Vector3f localTrans = new Vector3f();
        private Keyframe currentPose;
        protected Quaternion modelRot = new Quaternion();
        protected Matrix4f mat = new Matrix4f();
        private Quaternion bindRot = new Quaternion();
        protected Matrix4f boneMatrix = new Matrix4f();
        private Vector3f bindTrans = new Vector3f();
        private Vector3f bindAxis = new Vector3f();
        private float bindAngle = 0f;
        private Quaternion invBindRot = new Quaternion();
        private Vector3f invBindPos = new Vector3f();
        private Vector3f tmpMpos = new Vector3f();
        private Matrix4f rotMatrix = new Matrix4f();
        private Quaternion poseRot = new Quaternion();
        private Quaternion tmpRot = new Quaternion();

        public Keyframe GetCurrentPose()
        {
            return currentPose;
        }

        public Bone(string name)
        {
            this.name = name;
        }

        public void SetParent(Bone parentBone)
        {
            this.parentBone = parentBone;
        }

        public Quaternion GetPoseRotation()
        {
            return poseRot;
        }

        public Quaternion GetInverseBindRotation()
        {
            return invBindRot;
        }

        public Vector3f GetInverseBindPosition()
        {
            return invBindPos;
        }

        public Vector3f GetBindTranslation()
        {
            return bindTrans;
        }

        public void SetBindTranslation(Vector3f bindTrans)
        {
            this.bindTrans = bindTrans;
        }

        public Quaternion GetBindRotation()
        {
            return bindRot;
        }

        public void SetBindRotation(Quaternion bindRot)
        {
            this.bindRot = bindRot;
        }

        public Vector3f GetModelTranslation()
        {
            return modelPos;
        }

        public Quaternion GetModelRotation()
        {
            return modelRot;
        }

        public Quaternion CalculateBindingRotation()
        {
            if (parentBone != null)
            {
                modelRot = parentBone.modelRot.Multiply(GetBindRotation());
            }
            else
            {
                modelRot = GetBindRotation();
            }
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is Bone)
                {
                    Bone b = (Bone)children[i];
                    b.CalculateBindingRotation();
                }
            }
            return modelRot;
        }

        private void CalculateBindingTranslation(Vector3f outv)
        {
            if (parentBone != null)
            {
                outv.Set(parentBone.GetModelRotation().Multiply(GetBindTranslation()));
                outv.AddStore(parentBone.GetModelTranslation());
            }
            else
                outv.Set(GetBindTranslation());
        }

        public Vector3f CalculateBindingTranslation()
        {
            Vector3f outv = new Vector3f();
            CalculateBindingTranslation(outv);
            modelPos.Set(outv);
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is Bone)
                {
                    Bone b = (Bone)children[i];
                    b.CalculateBindingTranslation();
                }
            }
            return modelPos;
        }

        public void SetBindAxisAngle(Vector3f axis, float angleRad)
        {
            SetBindRotation(new Quaternion().SetFromAxisRadNorm(axis, angleRad));
            bindAxis = axis;
            bindAngle = angle;
        }

        public Vector3f GetBindAxis()
        {
            return bindAxis;
        }

        public float GetBindAngle()
        {
            return bindAngle;
        }

        public void SetToBindingPose()
        {
            this.localRotation = new Quaternion();
            this.localTranslation.Set(GetBindTranslation());
            this.poseRot.Set(GetBindRotation());
            UpdateTransform();
        }

        public void StoreBindingPose()
        {
            this.invBindPos = this.modelPos.Multiply(-1f);
            this.invBindRot = this.modelRot.Inverse();

            this.localRotation = new Quaternion();
            this.localTranslation = new Vector3f();
        }

        public void ClearPose()
        {
            poseRot.SetToIdentity();
            UpdateTransform();
        }

        public void ApplyPose(Keyframe pose)
        {
            currentPose = pose;
            SetLocalTranslation(pose.GetTranslation());
            poseRot = pose.GetRotation();
            UpdateTransform();
        }

        public override void Update(RenderManager renderManager)
        {
            // do nothing
        }

        public Matrix4f GetBoneMatrix()
        {
            return boneMatrix;
        }

        public override Quaternion GetWorldRotation()
        {
            return modelRot.Multiply(localRotation);
        }

        public override void SetLocalRotation(Quaternion localRot)
        {
            localRotation.Set(localRot);
            UpdateTransform();
        }

        public void SetLocalRotation(Vector3f axis, float deg)
        {
            localRotation.SetFromAxis(axis, deg);
            UpdateTransform();
        }

        public override void UpdateTransform()
        {
            tmpMpos.Set(modelPos);
            tmpMpos.MultiplyStore(-1f);
            rotMatrix.SetToTranslation(tmpMpos);

            tmpRot.Set(modelRot);
            tmpRot.MultiplyStore(poseRot);
            tmpRot.MultiplyStore(localRotation);
            tmpRot.MultiplyStore(invBindRot);

            tmpMatrix.SetToRotation(tmpRot);
            rotMatrix.MultiplyStore(tmpMatrix);
            tmpMpos.MultiplyStore(-1f);
            tmpMatrix.SetToTranslation(tmpMpos);
            rotMatrix.MultiplyStore(tmpMatrix);
            tmpMatrix.SetToTranslation(localTranslation);
            rotMatrix.MultiplyStore(tmpMatrix);
            boneMatrix.Set(rotMatrix);
            if (parentBone != null)
            {
                boneMatrix.MultiplyStore(parentBone.boneMatrix);
            }
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is Bone)
                {
                    Bone b = (Bone)children[i];
                    b.UpdateTransform();
                }
            }
        }

        public override void SetParent(Node par)
        {
            base.SetParent(par);
            if (par is Bone)
                this.parentBone = (Bone)par;
        }
    }
}