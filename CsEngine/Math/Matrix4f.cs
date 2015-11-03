using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsEngine.Math
{
    public class Matrix4f
    {
        public const int m00 = 0, m01 = 1, m02 = 2, m03 = 3,
                         m10 = 4, m11 = 5, m12 = 6, m13 = 7,
                         m20 = 8, m21 = 9, m22 = 10, m23 = 11,
                         m30 = 12, m31 = 13, m32 = 14, m33 = 15;
        public float[] values = new float[16];
        public void LoadIdentity()
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
    }
}
