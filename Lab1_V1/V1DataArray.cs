using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    class V1DataArray : V1Data
    {
        public int CountXTicks { get; }
        public int CountYTicks { get; }
        public double XTick { get; }
        public double YTick { get; }
        public System.Numerics.Complex[,] Grid { get; }
        public V1DataArray(string DataId, DateTime Date) : base(DataId, Date)
        {
            Grid = new System.Numerics.Complex[0, 0];
        }
        public V1DataArray(string DataId, DateTime Date,
                           int CountXTicks, int CountYTicks,
                           double XTick, double YTick, FdblComplex F) : base(DataId, Date)
        {
            Grid = new System.Numerics.Complex[CountXTicks, CountYTicks];
            this.CountXTicks = CountXTicks;
            this.CountYTicks = CountYTicks;
            this.XTick = XTick;
            this.YTick = YTick;
            for (int i = 0; i < CountXTicks; ++i)
            {
                for (int j = 0; j < CountYTicks; ++j)
                {
                    Grid[i, j] = F(i * XTick, j * YTick);
                }
            }
        }
        public override int Count => CountXTicks * CountYTicks;
        public override double AverageValue
        {
            get
            {
                int count = CountXTicks * CountYTicks;
                if (count == 0) return 0;
                double sum = 0;
                for (int i = 0; i < CountXTicks; ++i)
                {
                    for (int j = 0; j < CountYTicks; ++j)
                    {
                        sum += Grid[i, j].Magnitude;
                    }
                }
                return sum / count;
            }
        }
        public override string ToString()
        {
            string ret = base.ToString();
            ret += $"\nCountXTicks={CountXTicks}\tXTick={XTick}\nCountYTicks={CountYTicks}\tYTick={YTick}";
            return ret;
        }
        public override string ToLongString(string format)
        {
            string ret = ToString();
            for (int y = CountYTicks - 1; y >= 0 ; --y)
            {
                ret += "\n";
                for (int x = 0; x < CountXTicks; ++x)
                {
                    ret += Grid[x, y].ToString(format) + " ";
                }
            }
            return ret;
        }
        public static explicit operator V1DataList(V1DataArray arr)
        {
            var ret = new V1DataList(arr.DataId, arr.Date);
            for (int i = 0; i < arr.CountXTicks; ++i)
            {
                for (int j = 0; j < arr.CountYTicks; ++j)
                {
                    ret.Add(new DataItem(i * arr.XTick, j * arr.YTick, arr.Grid[i, j]));
                }
            }
            return ret;
        }

    }
}
