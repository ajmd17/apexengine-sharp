using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Math
{
    public abstract class Matrix
    {
        public abstract float[] GetValues();

        public Matrix() { }
        
        public Matrix(Matrix other)
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
        }
    }
}
