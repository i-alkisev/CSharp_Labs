using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    abstract class V1Data
    {
        public string DataId { get; }
        public DateTime Date { get; }
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
    }
}
