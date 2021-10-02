using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    struct DataItem
    {
        public double x { get; set; }
        public double y { get; set; }
        public System.Numerics.Complex Value { get; set; }
        public DataItem(double x, double y, System.Numerics.Complex Value)
        {
            this.x = x;
            this.y = y;
            this.Value = Value;
        }
        public string ToLongString(string format)
        {
            return $"({x.ToString(format)}, {y.ToString(format)}) Value={Value.ToString(format)} abs(Value)={Value.Magnitude.ToString(format)}";
        }
        public override string ToString()
        {
            return $"({x}, {y}) Value={Value}";
        }
    }
}
