using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    class V1DataList : V1Data
    {
        public List<DataItem> Items { get; }
        public V1DataList(string DataId, DateTime Date) : base(DataId, Date)
        {
            Items = new List<DataItem>();
        }
        public bool Add(DataItem newItem)
        {
            bool predicate(DataItem Item)
            {
                const double eps = 0.00001;
                return Math.Abs(newItem.x - Item.x) <= eps && Math.Abs(newItem.y - Item.y) <= eps;
            };
            if (Items.FindIndex(predicate) != -1) return false;
            Items.Add(newItem);
            return true;
        }
        public int AddDefaults(int nItems, FdblComplex F)
        {
            int added = 0;
            for (int i = 0; i < nItems; ++i)
            {
                double x = (double)i;
                double y = x * x;
                if (Add(new DataItem(x, y, F(x, y)))) ++added;
            }
            return added;
        }
        public override int Count
        {
            get
            {
                return Items.Count;
            }
        }
        public override double AverageValue
        {
            get
            {
                int count = Items.Count;
                if (count == 0) return 0;
                double sum = 0;
                foreach (DataItem Item in Items)
                {
                    sum += Item.Value.Magnitude;
                }
                return sum / count;
            }
        }
        public override string ToString()
        {
            return base.ToString() + $"\nCountElems={Items.Count}";
        }
        public override string ToLongString(string format)
        {
            string ret = ToString();
            foreach (DataItem Item in Items)
            {
                ret += "\n" + Item.ToLongString(format);
            }
            return ret;
        }
    }
}
