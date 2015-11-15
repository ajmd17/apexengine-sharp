using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public class Matrix4f
    {
        private static Matrix4f tmpMat = new Matrix4f();
        public const int m00 = 0, m01 = 1, m02 = 2, m03 = 3,
                         m10 = 4, m11 = 5, m12 = 6, m13 = 7,
                         m20 = 8, m21 = 9, m22 = 10, m23 = 11,
                         m30 = 12, m31 = 13, m32 = 14, m33 = 15;
        public float[] values = new float[16];
        public Matrix4f()
        {
            this.SetToIdentity();
        }
        public Matrix4f(float m00, float m01, float m02, float m03,
                        float m10, float m11, float m12, float m13,
                        float m20, float m21, float m22, float m23,
                        float m30, float m31, float m32, float m33)
        {
            this.Set(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
        }
        public Matrix4f(Matrix4f other)
        {
            this.Set(other);
        }
        public Matrix4f Set(float m00, float m01, float m02, float m03,
                        float m10, float m11, float m12, float m13,
                        float m20, float m21, float m22, float m23,
                        float m30, float m31, float m32, float m33)
        {
            this.values[Matrix4f.m00] = m00;
            this.values[Matrix4f.m01] = m01;
            this.values[Matrix4f.m02] = m02;
            this.values[Matrix4f.m03] = m03;

            this.values[Matrix4f.m10] = m10;
            this.values[Matrix4f.m11] = m11;
            this.values[Matrix4f.m12] = m12;
            this.values[Matrix4f.m13] = m13;

            this.values[Matrix4f.m20] = m20;
            this.values[Matrix4f.m21] = m21;
            this.values[Matrix4f.m22] = m22;
            this.values[Matrix4f.m23] = m23;

            this.values[Matrix4f.m30] = m30;
            this.values[Matrix4f.m31] = m31;
            this.values[Matrix4f.m32] = m32;
            this.values[Matrix4f.m33] = m33;

            return this;
        }
        public Matrix4f Set(Matrix4f other)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = other.values[i];
            }
            return this;
        }
        public Matrix4f Multiply(Matrix4f other)
        {
	        Matrix4f res = new Matrix4f();
	
	        float _m00 = this.values[m00] * other.values[m00] + this.values[m10] * other.values[m01] + this.values[m20] * other.values[m02] + this.values[m30] * other.values[m03];
	        float _m01 = this.values[m01] * other.values[m00] + this.values[m11] * other.values[m01] + this.values[m21] * other.values[m02] + this.values[m31] * other.values[m03];
	        float _m02 = this.values[m02] * other.values[m00] + this.values[m12] * other.values[m01] + this.values[m22] * other.values[m02] + this.values[m32] * other.values[m03];
	        float _m03 = this.values[m03] * other.values[m00] + this.values[m13] * other.values[m01] + this.values[m23] * other.values[m02] + this.values[m33] * other.values[m03];
	        float _m10 = this.values[m00] * other.values[m10] + this.values[m10] * other.values[m11] + this.values[m20] * other.values[m12] + this.values[m30] * other.values[m13];
	        float _m11 = this.values[m01] * other.values[m10] + this.values[m11] * other.values[m11] + this.values[m21] * other.values[m12] + this.values[m31] * other.values[m13];
	        float _m12 = this.values[m02] * other.values[m10] + this.values[m12] * other.values[m11] + this.values[m22] * other.values[m12] + this.values[m32] * other.values[m13];
	        float _m13 = this.values[m03] * other.values[m10] + this.values[m13] * other.values[m11] + this.values[m23] * other.values[m12] + this.values[m33] * other.values[m13];
	        float _m20 = this.values[m00] * other.values[m20] + this.values[m10] * other.values[m21] + this.values[m20] * other.values[m22] + this.values[m30] * other.values[m23];
	        float _m21 = this.values[m01] * other.values[m20] + this.values[m11] * other.values[m21] + this.values[m21] * other.values[m22] + this.values[m31] * other.values[m23];
	        float _m22 = this.values[m02] * other.values[m20] + this.values[m12] * other.values[m21] + this.values[m22] * other.values[m22] + this.values[m32] * other.values[m23];
	        float _m23 = this.values[m03] * other.values[m20] + this.values[m13] * other.values[m21] + this.values[m23] * other.values[m22] + this.values[m33] * other.values[m23];
	        float _m30 = this.values[m00] * other.values[m30] + this.values[m10] * other.values[m31] + this.values[m20] * other.values[m32] + this.values[m30] * other.values[m33];
	        float _m31 = this.values[m01] * other.values[m30] + this.values[m11] * other.values[m31] + this.values[m21] * other.values[m32] + this.values[m31] * other.values[m33];
	        float _m32 = this.values[m02] * other.values[m30] + this.values[m12] * other.values[m31] + this.values[m22] * other.values[m32] + this.values[m32] * other.values[m33];
	        float _m33 = this.values[m03] * other.values[m30] + this.values[m13] * other.values[m31] + this.values[m23] * other.values[m32] + this.values[m33] * other.values[m33];

	        res.values[m00] = _m00;
	        res.values[m01] = _m01;
	        res.values[m02] = _m02;
	        res.values[m03] = _m03;

	        res.values[m10] = _m10;
	        res.values[m11] = _m11;
	        res.values[m12] = _m12;
	        res.values[m13] = _m13;

	        res.values[m20] = _m20;
	        res.values[m21] = _m21;
	        res.values[m22] = _m22;
	        res.values[m23] = _m23;

	        res.values[m30] = _m30;
	        res.values[m31] = _m31;
	        res.values[m32] = _m32;
	        res.values[m33] = _m33;

	        return res;
        }
        public Matrix4f MultiplyStore(Matrix4f other)
        {
	        float _m00 = this.values[m00] * other.values[m00] + this.values[m10] * other.values[m01] + this.values[m20] * other.values[m02] + this.values[m30] * other.values[m03];
	        float _m01 = this.values[m01] * other.values[m00] + this.values[m11] * other.values[m01] + this.values[m21] * other.values[m02] + this.values[m31] * other.values[m03];
	        float _m02 = this.values[m02] * other.values[m00] + this.values[m12] * other.values[m01] + this.values[m22] * other.values[m02] + this.values[m32] * other.values[m03];
	        float _m03 = this.values[m03] * other.values[m00] + this.values[m13] * other.values[m01] + this.values[m23] * other.values[m02] + this.values[m33] * other.values[m03];
	        float _m10 = this.values[m00] * other.values[m10] + this.values[m10] * other.values[m11] + this.values[m20] * other.values[m12] + this.values[m30] * other.values[m13];
	        float _m11 = this.values[m01] * other.values[m10] + this.values[m11] * other.values[m11] + this.values[m21] * other.values[m12] + this.values[m31] * other.values[m13];
	        float _m12 = this.values[m02] * other.values[m10] + this.values[m12] * other.values[m11] + this.values[m22] * other.values[m12] + this.values[m32] * other.values[m13];
	        float _m13 = this.values[m03] * other.values[m10] + this.values[m13] * other.values[m11] + this.values[m23] * other.values[m12] + this.values[m33] * other.values[m13];
	        float _m20 = this.values[m00] * other.values[m20] + this.values[m10] * other.values[m21] + this.values[m20] * other.values[m22] + this.values[m30] * other.values[m23];
	        float _m21 = this.values[m01] * other.values[m20] + this.values[m11] * other.values[m21] + this.values[m21] * other.values[m22] + this.values[m31] * other.values[m23];
	        float _m22 = this.values[m02] * other.values[m20] + this.values[m12] * other.values[m21] + this.values[m22] * other.values[m22] + this.values[m32] * other.values[m23];
	        float _m23 = this.values[m03] * other.values[m20] + this.values[m13] * other.values[m21] + this.values[m23] * other.values[m22] + this.values[m33] * other.values[m23];
	        float _m30 = this.values[m00] * other.values[m30] + this.values[m10] * other.values[m31] + this.values[m20] * other.values[m32] + this.values[m30] * other.values[m33];
	        float _m31 = this.values[m01] * other.values[m30] + this.values[m11] * other.values[m31] + this.values[m21] * other.values[m32] + this.values[m31] * other.values[m33];
	        float _m32 = this.values[m02] * other.values[m30] + this.values[m12] * other.values[m31] + this.values[m22] * other.values[m32] + this.values[m32] * other.values[m33];
	        float _m33 = this.values[m03] * other.values[m30] + this.values[m13] * other.values[m31] + this.values[m23] * other.values[m32] + this.values[m33] * other.values[m33];

	        this.values[m00] = _m00;
	        this.values[m01] = _m01;
	        this.values[m02] = _m02;
	        this.values[m03] = _m03;

	        this.values[m10] = _m10;
	        this.values[m11] = _m11;
	        this.values[m12] = _m12;
	        this.values[m13] = _m13;

	        this.values[m20] = _m20;
	        this.values[m21] = _m21;
	        this.values[m22] = _m22;
	        this.values[m23] = _m23;

	        this.values[m30] = _m30;
	        this.values[m31] = _m31;
	        this.values[m32] = _m32;
	        this.values[m33] = _m33;

	        return this;
        }
        public override string ToString()
        {
            string res = "[";
            res += values[Matrix4f.m00] + ", " + values[Matrix4f.m10] + ", " + values[Matrix4f.m20] + ", " + values[Matrix4f.m30] + "\n";
	        res += values[Matrix4f.m01] + ", " + values[Matrix4f.m11] + ", " + values[Matrix4f.m21] + ", " + values[Matrix4f.m31] + "\n";
	        res += values[Matrix4f.m02] + ", " + values[Matrix4f.m12] + ", " + values[Matrix4f.m22] + ", " + values[Matrix4f.m31] + "\n";
	        res += values[Matrix4f.m03] + ", " + values[Matrix4f.m13] + ", " + values[Matrix4f.m23] + ", " + values[Matrix4f.m33];
	        res += "]";
            return res;
        }
        public Matrix4f SetToTranslation(Vector3f v)
        {
            this.SetToIdentity();

            this.values[Matrix4f.m00] = 1.0f;
            this.values[Matrix4f.m01] = 0.0f;
            this.values[Matrix4f.m02] = 0.0f;
            this.values[Matrix4f.m03] = v.x;

            this.values[Matrix4f.m10] = 0.0f;
            this.values[Matrix4f.m11] = 1.0f;
            this.values[Matrix4f.m12] = 0.0f;
            this.values[Matrix4f.m13] = v.y;

            this.values[Matrix4f.m20] = 0.0f;
            this.values[Matrix4f.m21] = 0.0f;
            this.values[Matrix4f.m22] = 1.0f;
            this.values[Matrix4f.m23] = v.z;

            this.values[Matrix4f.m30] = 0.0f;
            this.values[Matrix4f.m31] = 0.0f;
            this.values[Matrix4f.m32] = 0.0f;
            this.values[Matrix4f.m33] = 1.0f;

            return this;
        }
        public Matrix4f SetToRotation(Quaternion q)
        {
            this.SetToIdentity();

            float xx = q.x * q.x;
            float xy = q.x * q.y;
            float xz = q.x * q.z;
            float xw = q.x * q.w;
            float yy = q.y * q.y;
            float yz = q.y * q.z;
            float yw = q.y * q.w;
            float zz = q.z * q.z;
            float zw = q.z * q.w;

            this.values[Matrix4f.m00] = 1f - 2f * (yy + zz);
            this.values[Matrix4f.m10] = 2f * (xy - zw);
            this.values[Matrix4f.m20] = 2f * (xz + yw);
            this.values[Matrix4f.m30] = 0f;
            this.values[Matrix4f.m01] = 2f * (xy + zw);
            this.values[Matrix4f.m11] = 1f - 2f * (xx + zz);
            this.values[Matrix4f.m21] = 2f * (yz - xw);
            this.values[Matrix4f.m31] = 0f;
            this.values[Matrix4f.m02] = 2f * (xz - yw);
            this.values[Matrix4f.m12] = 2f * (yz + xw);
            this.values[Matrix4f.m22] = 1f - 2f * (xx + yy);
            this.values[Matrix4f.m32] = 0f;
            this.values[Matrix4f.m03] = 0f;
            this.values[Matrix4f.m13] = 0f;
            this.values[Matrix4f.m23] = 0f;
            this.values[Matrix4f.m33] = 1f;

            return this;
        }
        public Matrix4f SetToRotation(Vector3f axis, float angle)
        {
            this.SetToIdentity();

            Quaternion tempQ = new Quaternion();
            tempQ.SetFromAxis(axis, angle);

            SetToRotation(tempQ);
            return this;
        }
        public Matrix4f SetToScaling(Vector3f v)
        {
            this.SetToIdentity();

            this.values[Matrix4f.m00] = v.x;
            this.values[Matrix4f.m01] = 0.0f;
            this.values[Matrix4f.m02] = 0.0f;
            this.values[Matrix4f.m03] = 0.0f;

            this.values[Matrix4f.m10] = 0.0f;
            this.values[Matrix4f.m11] = v.y;
            this.values[Matrix4f.m12] = 0.0f;
            this.values[Matrix4f.m13] = 0.0f;

            this.values[Matrix4f.m20] = 0.0f;
            this.values[Matrix4f.m21] = 0.0f;
            this.values[Matrix4f.m22] = v.z;
            this.values[Matrix4f.m23] = 0.0f;

            this.values[Matrix4f.m30] = 0.0f;
            this.values[Matrix4f.m31] = 0.0f;
            this.values[Matrix4f.m32] = 0.0f;
            this.values[Matrix4f.m33] = 1.0f;

            return this;
        }
        public Matrix4f SetToProjection(float fov, float w, float h, float n, float f)
        {
            this.SetToIdentity();

            float ar = w / h;
            float tanHalfFov = (float)System.Math.Tan(MathUtil.ToRadians(fov / 2.0f));
            float range = n - f;

            this.values[Matrix4f.m00] = 1.0f / (tanHalfFov * ar);
            this.values[Matrix4f.m01] = 0.0f;
            this.values[Matrix4f.m02] = 0.0f;
            this.values[Matrix4f.m03] = 0.0f;

            this.values[Matrix4f.m10] = 0.0f;
            this.values[Matrix4f.m11] = 1.0f / (tanHalfFov);
            this.values[Matrix4f.m12] = 0.0f;
            this.values[Matrix4f.m13] = 0.0f;

            this.values[Matrix4f.m20] = 0.0f;
            this.values[Matrix4f.m21] = 0.0f;
            this.values[Matrix4f.m22] = (-n - f) / range;
            this.values[Matrix4f.m23] = (2.0f * f * n) / range;

            this.values[Matrix4f.m30] = 0.0f;
            this.values[Matrix4f.m31] = 0.0f;
            this.values[Matrix4f.m32] = 1.0f;
            this.values[Matrix4f.m33] = 0.0f;

            return this;
        }
        public Matrix4f SetToIdentity()
        {
            this.values[m00] = 1.0f;
            this.values[m01] = 0.0f;
            this.values[m02] = 0.0f;
            this.values[m03] = 0.0f;

            this.values[m10] = 0.0f;
            this.values[m11] = 1.0f;
            this.values[m12] = 0.0f;
            this.values[m13] = 0.0f;

            this.values[m20] = 0.0f;
            this.values[m21] = 0.0f;
            this.values[m22] = 1.0f;
            this.values[m23] = 0.0f;

            this.values[m30] = 0.0f;
            this.values[m31] = 0.0f;
            this.values[m32] = 0.0f;
            this.values[m33] = 1.0f;

            return this;
        }
        public Matrix4f SetToLookAt(Vector3f dir, Vector3f up)
        {
            this.SetToIdentity();

            Vector3f l_vez = new Vector3f();
            Vector3f l_vex = new Vector3f();
            Vector3f l_vey = new Vector3f();
            l_vez.Set(dir);
            l_vez.NormalizeStore();
            l_vex.Set(dir);
            l_vex.NormalizeStore();
            l_vex.CrossStore(up);
            l_vex.NormalizeStore();
            l_vey.Set(l_vex);
            l_vey.CrossStore(l_vez);
            l_vey.NormalizeStore();

            this.values[Matrix4f.m00] = l_vex.x;
            this.values[Matrix4f.m01] = l_vex.y;
            this.values[Matrix4f.m02] = l_vex.z;

            this.values[Matrix4f.m10] = l_vey.x;
            this.values[Matrix4f.m11] = l_vey.y;
            this.values[Matrix4f.m12] = l_vey.z;

            this.values[Matrix4f.m20] = l_vez.x;
            this.values[Matrix4f.m21] = l_vez.y;
            this.values[Matrix4f.m22] = l_vez.z;

            return this;
        }
        public Matrix4f SetToLookAt(Vector3f pos, Vector3f target, Vector3f up)
        {
            this.SetToIdentity();

            Matrix4f lookAt = new Matrix4f(), res = new Matrix4f();
            Vector3f tempVec = new Vector3f();
            tempVec.Set(target);
            tempVec.SubtractStore(pos);
            tmpMat.SetToTranslation(pos.Multiply(-1f));
            lookAt.SetToLookAt(tempVec, up);
            this.Set(tmpMat);
            this.MultiplyStore(lookAt);
            return this;
        }
        public Matrix4f SetToOrtho(float left, float right, float bottom, float top, float near, float far)
        {
            float x_orth = 2 / (right - left);
            float y_orth = 2 / (top - bottom);
            float z_orth = -2 / (far - near);

            float tx = -(right + left) / (right - left);
            float ty = -(top + bottom) / (top - bottom);
            float tz = -(far + near) / (far - near);

            values[m00] = x_orth;
            values[m10] = 0;
            values[m20] = 0;
            values[m30] = 0;
            values[m01] = 0;
            values[m11] = y_orth;
            values[m21] = 0;
            values[m31] = 0;
            values[m02] = 0;
            values[m12] = 0;
            values[m22] = z_orth;
            values[m32] = 0;
            values[m03] = tx;
            values[m13] = ty;
            values[m23] = tz;
            values[m33] = 1;

            return this;
        }
    }
}
