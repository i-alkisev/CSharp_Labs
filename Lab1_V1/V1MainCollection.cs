using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    class V1MainCollection
    {
        private List<V1Data> DataList;
        public V1MainCollection()
        {
            DataList = new List<V1Data>();
        }
        public int Count
        {
            get
            {
                return DataList.Count;
            }
        }
        public V1Data this[int idx]
        {
            get
            {
                return DataList[idx];
            }
        }
        public bool Contains(string Id)
        {
            bool V1DataComparator(V1Data V1DataItem)
            {
                return V1DataItem.DataId == Id;
            };
            return DataList.FindIndex(V1DataComparator) != -1;
        }
        public bool Add(V1Data V1DataItem)
        {
            if (Contains(V1DataItem.DataId)) return false;
            DataList.Add(V1DataItem);
            return true;
        }
        public string ToLongString(string format)
        {
            string ret = "V1MainCollection:";
            foreach (V1Data V1DataItem in DataList)
            {
                ret += "\n" + V1DataItem.ToLongString(format) + "\n";
            }
            return ret;
        }
        public override string ToString()
        {
            string ret = "V1MainCollection:";
            foreach (V1Data V1DataItem in DataList)
            {
                ret += "\n" + V1DataItem.ToString() + "\n";
            }
            return ret;
        }
    }
}
