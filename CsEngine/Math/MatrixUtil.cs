using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public static class MatrixUtil
    {
        public static Matrix4f SetToTranslation(Vector3f v)
        {
	        Matrix4f res = new Matrix4f();

	        res.values[Matrix4f.m00] = 1.0f;
	        res.values[Matrix4f.m01] = 0.0f;
	        res.values[Matrix4f.m02] = 0.0f;
	        res.values[Matrix4f.m03] = v.x;

	        res.values[Matrix4f.m10] = 0.0f;
	        res.values[Matrix4f.m11] = 1.0f;
	        res.values[Matrix4f.m12] = 0.0f;
	        res.values[Matrix4f.m13] = v.y;

	        res.values[Matrix4f.m20] = 0.0f;
	        res.values[Matrix4f.m21] = 0.0f;
	        res.values[Matrix4f.m22] = 1.0f;
	        res.values[Matrix4f.m23] = v.z;

	        res.values[Matrix4f.m30] = 0.0f;
	        res.values[Matrix4f.m31] = 0.0f;
	        res.values[Matrix4f.m32] = 0.0f;
	        res.values[Matrix4f.m33] = 1.0f;

	        return res;
        }
        public static Matrix4f SetToRotation(Quaternion q)
        {
            float xx = q.x * q.x;
            float xy = q.x * q.y;
            float xz = q.x * q.z;
            float xw = q.x * q.w;
            float yy = q.y * q.y;
            float yz = q.y * q.z;
            float yw = q.y * q.w;
            float zz = q.z * q.z;
            float zw = q.z * q.w;

            Matrix4f res = new Matrix4f();

            res.values[Matrix4f.m00] = 1f - 2f * (yy + zz);
            res.values[Matrix4f.m10] = 2f * (xy - zw);
            res.values[Matrix4f.m20] = 2f * (xz + yw);
            res.values[Matrix4f.m30] = 0f;
            res.values[Matrix4f.m01] = 2f * (xy + zw);
            res.values[Matrix4f.m11] = 1f - 2f * (xx + zz);
            res.values[Matrix4f.m21] = 2f * (yz - xw);
            res.values[Matrix4f.m31] = 0;
            res.values[Matrix4f.m02] = 2 * (xz - yw);
            res.values[Matrix4f.m12] = 2 * (yz + xw);
            res.values[Matrix4f.m22] = 1 - 2 * (xx + yy);
            res.values[Matrix4f.m32] = 0;
            res.values[Matrix4f.m03] = 0;
            res.values[Matrix4f.m13] = 0;
            res.values[Matrix4f.m23] = 0;
            res.values[Matrix4f.m33] = 1;

            return res;
        }
        public static Matrix4f SetToRotation(Vector3f axis, float angle)
        {
	        Matrix4f res = new Matrix4f();

	        Quaternion tempQ = new Quaternion();
	        tempQ.SetFromAxis(axis, angle);

	        res = SetToRotation(tempQ);
	        return res;
        }
        public static Matrix4f SetToScaling(Vector3f v)
        {
	        Matrix4f res = new Matrix4f();

	        res.values[Matrix4f.m00] = v.x;
	        res.values[Matrix4f.m01] = 0.0f;
	        res.values[Matrix4f.m02] = 0.0f;
	        res.values[Matrix4f.m03] = 0.0f;

	        res.values[Matrix4f.m10] = 0.0f;
	        res.values[Matrix4f.m11] = v.y;
	        res.values[Matrix4f.m12] = 0.0f;
	        res.values[Matrix4f.m13] = 0.0f;

	        res.values[Matrix4f.m20] = 0.0f;
	        res.values[Matrix4f.m21] = 0.0f;
	        res.values[Matrix4f.m22] = v.z;
	        res.values[Matrix4f.m23] = 0.0f;

	        res.values[Matrix4f.m30] = 0.0f;
	        res.values[Matrix4f.m31] = 0.0f;
	        res.values[Matrix4f.m32] = 0.0f;
	        res.values[Matrix4f.m33] = 1.0f;

	        return res;
        }
        public static Matrix4f SetToProjection(float fov, float w, float h, float n, float f)
        {
	        float ar = w / h;
	        float tanHalfFov = (float)System.Math.Tan(MathUtil.ToRadians(fov / 2.0f));
	        float range = n - f;
	        Matrix4f res = new Matrix4f();

	        res.values[Matrix4f.m00] = 1.0f / (tanHalfFov * ar);
	        res.values[Matrix4f.m01] = 0.0f;
	        res.values[Matrix4f.m02] = 0.0f;
	        res.values[Matrix4f.m03] = 0.0f;

	        res.values[Matrix4f.m10] = 0.0f;
	        res.values[Matrix4f.m11] = 1.0f / (tanHalfFov);
	        res.values[Matrix4f.m12] = 0.0f;
	        res.values[Matrix4f.m13] = 0.0f;

	        res.values[Matrix4f.m20] = 0.0f;
	        res.values[Matrix4f.m21] = 0.0f;
	        res.values[Matrix4f.m22] = (-n - f) / range;
	        res.values[Matrix4f.m23] = (2.0f * f * n) / range;

	        res.values[Matrix4f.m30] = 0.0f;
	        res.values[Matrix4f.m31] = 0.0f;
	        res.values[Matrix4f.m32] = 1.0f;
	        res.values[Matrix4f.m33] = 0.0f;

	        return res;
                }
        public static Matrix4f SetToLookAt(Vector3f dir, Vector3f up)
        {
	        Matrix4f res = new Matrix4f();

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

	        res.values[Matrix4f.m00] = l_vex.x;
	        res.values[Matrix4f.m01] = l_vex.y;
	        res.values[Matrix4f.m02] = l_vex.z;

	        res.values[Matrix4f.m10] = l_vey.x;
	        res.values[Matrix4f.m11] = l_vey.y;
	        res.values[Matrix4f.m12] = l_vey.z;

	        res.values[Matrix4f.m20] = l_vez.x;
	        res.values[Matrix4f.m21] = l_vez.y;
	        res.values[Matrix4f.m22] = l_vez.z;

	        return res;
        }
        public static Matrix4f SetToLookAt(Vector3f pos, Vector3f target, Vector3f up)
        {
	        Matrix4f lookAt, res;
	        Vector3f tempVec = new Vector3f();
	        tempVec.Set(target);
	        tempVec.SubtractStore(pos);
	        lookAt = SetToLookAt(tempVec, up);
	        res = SetToTranslation(pos).Multiply(lookAt);
	        return res;
        }
    }
}
