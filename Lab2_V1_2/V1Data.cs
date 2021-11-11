using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    abstract class V1Data: IEnumerable<DataItem>
    {
        public string DataId { get; protected set; }
        public DateTime Date { get; protected set; }
        public V1Data(string DataId, DateTime Date)
        {
            this.DataId = DataId;
            this.Date = Date;
        }
        public abstract int Count { get; }
        public abstract double AverageValue { get; }
        public abstract string ToLongString(string format);
        public override string ToString()
        {
            return $"DataId: \"{DataId}\"\tDate: {Date}";
        }
        public abstract IEnumerator<DataItem> GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
