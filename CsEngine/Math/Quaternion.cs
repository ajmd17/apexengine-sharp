using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsEngine.Math
{
    public class Quaternion
    {
        public float x, y, z, w;
        public Quaternion()
        {
            Set(0.0f, 0.0f, 0.0f, 1.0f);
        }
        public Quaternion(Quaternion other)
        {
            Set(other);
        }
        public Quaternion(float _x, float _y, float _z, float _w)
        {
            Set(_x, _y, _z, _w);
        }
        public Quaternion Set(Quaternion other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            this.w = other.w;
            return this;
        }
        public Quaternion Set(float _x, float _y, float _z, float _w)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.w = _w;
            return this;
        }
        public Quaternion Multiply(Quaternion other)
        {
            Quaternion res = new Quaternion();
            float x1 = x * other.w + y * other.z - z * other.y + w * other.x;
            float y1 = -x * other.z + y * other.w + z * other.x + w * other.y;
            float z1 = x * other.y - y * other.x + z * other.w + w * other.z;
            float w1 = -x * other.x - y * other.y - z * other.z + w * other.w;
            res.x = x1;
            res.y = y1;
            res.z = z1;
            res.w = w1;
            return res;
        }
        public Quaternion MultiplyStore(Quaternion other)
        {
	        float x1 = x * other.w + y * other.z - z * other.y + w * other.x;
            float y1 = -x * other.z + y * other.w + z * other.x + w * other.y;
            float z1 = x * other.y - y * other.x + z * other.w + w * other.z;
            float w1 = -x * other.x - y * other.y - z * other.z + w * other.w;
	        this.x = x1;
	        this.y = y1;
	        this.z = z1;
	        this.w = w1;
	        return this;
        }
        public float Normalize()
        {
	        return w * w + x * x + y * y + z * z;
        }
        public Quaternion Inverse()
        {
	        float n = Normalize();
	        Quaternion res = new Quaternion();
	        if (n > 0.0)
	        {
		        float invN = 1.0f / n;
		        res.Set(-x * invN, -y * invN, -z * invN, w * invN);
	        }
	        return res;
        }
        public Quaternion InverseStore()
        {
	        float n = Normalize();
	        if (n > 0.0)
	        {
		        float invN = 1.0f / n;
		        this.x = -x * invN;
		        this.y = -y * invN;
		        this.z = -z * invN;
		        this.w = w * invN;
	        }
	        return this;
        }
        public int GetGimbalPole()
        {
	        float t = y * x + z * w;
	        return t > 0.499f ? 1 : (t < -0.499f ? -1 : 0);
        }
        public float GetRollRad()
        {
	        int pole = GetGimbalPole();
	        return pole == 0 ? (float)System.Math.Atan2(2.0f * (w * z + y * x), 1.0f - 2.0f * (x * x + z * z)) : (float)pole * 2.0f * (float)System.Math.Atan2(y, w);
        }
        public float GetRoll()
        {
	        return MathUtil.ToDegrees(GetRollRad());
        }
        public float GetPitchRad()
        {
	        int pole = GetGimbalPole();
	        return pole == 0 ? (float)System.Math.Asin(MathUtil.Clamp(2.0f * (w * x - z * y), -1.0f, 1.0f)) : pole * MathUtil.PI * 0.5f;
        }
        public float GetPitch()
        {
	        return MathUtil.ToDegrees(GetPitchRad());
        }
        public float GetYawRad()
        {
	        int pole = GetGimbalPole();
            return pole == 0 ? (float)System.Math.Atan2(2.0f * (y * w + x * z), 1.0f - 2.0f * (y * y + x * x)) : 0.0f;
        }
        public float GetYaw()
        {
	        return MathUtil.ToDegrees(GetYawRad());
        }
        public Quaternion SetFromAxis(Vector3f axis, float deg)
        {
	        return SetFromAxisRad(axis, MathUtil.ToRadians(deg));
        }
        public Quaternion SetFromAxisRad(Vector3f axis, float rad)
        {
	        Vector3f newVec = new Vector3f(axis);
	        newVec.NormalizeStore();
	        return SetFromAxisRadNorm(newVec, rad);
        }
        public Quaternion SetFromAxisRadNorm(Vector3f axis, float rad)
        {
	        if (axis.x == 0.0f && axis.y == 0.0f && axis.z == 0.0f)
	        {
		        Set(0.0f, 0.0f, 0.0f, 1.0f);
	        } 
	        else 
	        {
		        float halfAngle = rad / 2.0f;
		        float sinHalfAngle = (float)System.Math.Sin(halfAngle);
		        this.w = (float)System.Math.Cos(halfAngle);
		        this.x = sinHalfAngle * axis.x;
		        this.y = sinHalfAngle * axis.y;
		        this.z = sinHalfAngle * axis.z;
	        }
	        return this;
        }
        public Quaternion SetFromAxes(float xx, float xy, float xz, 
									        float yx, float yy, float yz, 
									        float zx, float zy, float zz)
        {
	        float t = xx + yy + zz;
	        if (t >= 0.0f)
	        {
		        float s = (float)System.Math.Sqrt(t + 1);
		        this.w = 0.5f * s;
		        this.x = (zy - yz) * (0.5f / s);
		        this.y = (xz - zx) * (0.5f / s);
		        this.z = (yx - xy) * (0.5f / s);
	        }
	        else if ((xx > yy) && (xx > zz))
	        {
                float s = (float)System.Math.Sqrt(1 + xx - yy - zz);
		        this.x = s * 0.5f;
		        this.y = (yx + xy) * (0.5f / s);
		        this.z = (xz + zx) * (0.5f / s);
		        this.w = (zy - yz) * (0.5f / s);
	        }
	        else if (yy > zz)
	        {
                float s = (float)System.Math.Sqrt(1 + yy - xx - zz);
		        this.y = s * 0.5f;
		        this.x = (yx + xy) * (0.5f / s);
		        this.z = (zy + yz) * (0.5f / s);
		        this.w = (xz - zx) * (0.5f / s);
	        }
	        else
	        {
                float s = (float)System.Math.Sqrt(1 + zz - xx - yy);
		        this.z = s * 0.5f;
		        this.x = (xz + zx) * (0.5f / s);
		        this.y = (zy + yz) * (0.5f / s);
		        this.w = (yx - xy) * (0.5f / s);
	        }
	        return this;
        }
        public Quaternion SetFromMatrix(Matrix4f mat)
        {
	        SetFromAxes(mat.values[Matrix4f.m00], mat.values[Matrix4f.m10], mat.values[Matrix4f.m20], 
				        mat.values[Matrix4f.m01], mat.values[Matrix4f.m11], mat.values[Matrix4f.m21], 
				        mat.values[Matrix4f.m02], mat.values[Matrix4f.m12], mat.values[Matrix4f.m22]);
	        return this;
        }
        public Quaternion SetToLookAt(Vector3f dir, Vector3f up)
        {
	        Vector3f tempZ = new Vector3f();
	        Vector3f tempX = new Vector3f();
	        Vector3f tempY = new Vector3f();
	        tempZ.Set(dir);
	        tempZ.NormalizeStore();
	        tempX.Set(up);
	        tempX.CrossStore(dir);
	        tempX.NormalizeStore();
	        tempY.Set(dir);
	        tempY.CrossStore(tempX);
	        tempY.NormalizeStore();
	        SetFromAxes(tempX.x, tempX.y, tempX.z, 
            		        tempY.x, tempY.y,tempY.z,
            		        tempZ.x, tempZ.y, tempZ.z);
	        return this;
        }
    }
}
