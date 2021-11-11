using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        public override IEnumerator<DataItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
        public bool SaveBinary(string filename)
        {
            FileStream fout = null;
            bool ret = true;
            try
            {
                fout = File.Create(filename);
                BinaryWriter writer = new BinaryWriter(fout);

                writer.Write(DataId);
                writer.Write(Date.ToString());
                writer.Write(Items.Count.ToString());
                foreach (var Item in Items)
                {
                    writer.Write(Item.x.ToString());
                    writer.Write(Item.y.ToString());
                    writer.Write(Item.Value.Real.ToString());
                    writer.Write(Item.Value.Imaginary.ToString());
                }
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ret = false;
            }
            finally
            {
                if (fout != null) fout.Close();
            }
            return ret;
        }

        public bool LoadBinary(string filename)
        {
            FileStream fin = null;
            bool ret = true;
            try
            {
                fin = File.OpenRead(filename);
                BinaryReader reader = new BinaryReader(fin);

                DataId = reader.ReadString();
                Date = DateTime.Parse(reader.ReadString());
                int n = Convert.ToInt32(reader.ReadString());
                Items.Clear();
                for (int i = 0; i < n; ++i)
                {
                    double x = Convert.ToDouble(reader.ReadString());
                    double y = Convert.ToDouble(reader.ReadString());
                    double value_real = Convert.ToDouble(reader.ReadString());
                    double value_imaginary = Convert.ToDouble(reader.ReadString());
                    Items.Add(new DataItem(x, y, new System.Numerics.Complex(value_real, value_imaginary)));
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ret = false;
            }
            finally
            {
                if (fin != null) fin.Close();
            }
            return ret;
        }
    }
}
