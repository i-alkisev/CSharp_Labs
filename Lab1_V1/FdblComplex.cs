using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    delegate System.Numerics.Complex FdblComplex(double x, double y);

    static class FdblComplexImpl
    {
        public static System.Numerics.Complex F1(double x, double y)
        {
            return new System.Numerics.Complex(x + y, x - y);
        }
        public static System.Numerics.Complex F2(double x, double y)
        {
            return new System.Numerics.Complex(x, y);
        }
    }
}
