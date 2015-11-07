using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Scene;
using ApexEngine.Math;
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
        // private Keyframe currentPose;
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
        private Quaternion boneLocalRot = new Quaternion();
        private Vector3f boneLocalPos = new Vector3f();
        /*

        private Keyframe GetCurrentPose()
        {
            return currentPose;
        }*/
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
            for (int i  = 0; i < children.Count; i++)
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
        }
        public void SetToBindingPose()
        {
            this.boneLocalRot = new Quaternion();
            this.boneLocalPos = this.GetBindTranslation();
            this.poseRot = this.GetBindRotation();
            UpdateTransform();
        }
        public void StoreBindingPose()
        {
            this.invBindPos = this.modelPos.Multiply(-1f);
            this.invBindRot = this.modelRot.Inverse();

            this.boneLocalRot = new Quaternion();
            this.boneLocalPos = new Vector3f();
        }
        public Matrix4f GetBoneMatrix()
        {
            return boneMatrix;
        }
        public Quaternion GetWorldRotation()
        {
            return modelRot.Multiply(boneLocalRot);
        }
        public void SetLocalRotation(Quaternion localRot)
        {
            boneLocalRot.Set(localRot);
            UpdateTransform();
        }
        public override void UpdateTransform()
        {
            tmpMpos.Set(modelPos);
            rotMatrix.SetToTranslation(tmpMpos.Multiply(-1f));
            tmpMatrix.SetToRotation((modelRot.Multiply(poseRot).Multiply(boneLocalRot)).Multiply(invBindRot));
            rotMatrix.MultiplyStore(tmpMatrix);
            tmpMatrix.SetToTranslation(tmpMpos);
            rotMatrix.MultiplyStore(tmpMatrix);
            tmpMatrix.SetToTranslation(boneLocalPos);
            mat = rotMatrix.Multiply(tmpMatrix);
            if (parentBone != null)
                boneMatrix = mat.Multiply(parentBone.boneMatrix);
            else
                boneMatrix = mat;
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
            {
                this.parentBone = (Bone)par;
            }
        }
    }
}
