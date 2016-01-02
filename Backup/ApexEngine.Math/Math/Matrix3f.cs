using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public class Matrix3f : Matrix
    {
        public const int m00 = 0, m01 = 1, m02 = 2, m03 = 3,
                         m10 = 4, m11 = 5, m12 = 6, m13 = 7,
                         m20 = 8, m21 = 9, m22 = 10, m23 = 11;

        public float[] values = new float[12];


        public Matrix3f() { }

        public Matrix3f(Matrix other) : base(other) { }

        public override float[] GetValues()
        {
            return values;
        }
        
        public Matrix3f Set(Matrix other)
        {
            if (other.GetValues().Length >= GetValues().Length)
            {
                for (int i = 0; i < GetValues().Length; i++)
                {
                    GetValues()[i] = other.GetValues()[i];
                }
            }
            else if (other.GetValues().Length < GetValues().Length)
            {
                for (int i = 0; i < other.GetValues().Length; i++)
                {
                    GetValues()[i] = other.GetValues()[i];
                }
            }

            return this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix3f))
                return false;

            Matrix3f m_obj = (Matrix3f)obj;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != m_obj.values[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            for (int i = 0; i < values.Length; i++)
                hash = hash * 23 + values[i].GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            string res = "[";
            res += values[Matrix3f.m00] + ", " + values[Matrix3f.m10] + ", " + values[Matrix3f.m20] + "\n";
            res += values[Matrix3f.m01] + ", " + values[Matrix3f.m11] + ", " + values[Matrix3f.m21] + "\n";
            res += values[Matrix3f.m02] + ", " + values[Matrix3f.m12] + ", " + values[Matrix3f.m22] + "\n";
            res += values[Matrix3f.m03] + ", " + values[Matrix3f.m13] + ", " + values[Matrix3f.m23];
            res += "]";
            return res;
        }
    }
}
