using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Lab1
{
    class V1DataArray : V1Data
    {
        public int CountXTicks { get; private set; }
        public int CountYTicks { get; private set; }
        public double XTick { get; private set; }
        public double YTick { get; private set; }
        public System.Numerics.Complex[,] Grid { get; private set; }
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
        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < CountXTicks; ++i)
            {
                for (int j = 0; j < CountYTicks; ++j)
                {
                    yield return new DataItem(XTick * i, YTick * j, Grid[i, j]);
                }
            }
            yield break;
        }
        public bool SaveAsText(string filename)
        {
            FileStream fout = null;
            bool ret = true;
            try
            {
                fout = File.Create(filename);
                StreamWriter writer = new StreamWriter(fout);

                writer.WriteLine(DataId);
                writer.WriteLine(Date);
                writer.WriteLine(CountXTicks);
                writer.WriteLine(CountYTicks);
                writer.WriteLine(XTick);
                writer.WriteLine(YTick);
                for (int i = 0; i < CountXTicks; ++i)
                {
                    for (int j = 0; j < CountYTicks; ++j)
                    {
                        writer.WriteLine(Grid[i, j].Real);
                        writer.WriteLine(Grid[i, j].Imaginary);
                    }
                }

                writer.Close();
            }
            catch(Exception e)
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
        public bool LoadAsText(string filename)
        {
            FileStream fin = null;
            bool ret = true;
            try
            {
                fin = File.OpenRead(filename);
                StreamReader reader = new StreamReader(fin);

                DataId = reader.ReadLine();
                Date = DateTime.Parse(reader.ReadLine());
                CountXTicks = Convert.ToInt32(reader.ReadLine());
                CountYTicks = Convert.ToInt32(reader.ReadLine());
                XTick = Convert.ToDouble(reader.ReadLine());
                YTick = Convert.ToDouble(reader.ReadLine());

                Grid = new System.Numerics.Complex[CountXTicks, CountYTicks];

                for (int i = 0; i < CountXTicks; ++i)
                {
                    for (int j = 0; j < CountYTicks; ++j)
                    {
                        Double real = Convert.ToDouble(reader.ReadLine());
                        Double imaginary= Convert.ToDouble(reader.ReadLine());
                        Grid[i, j] = new System.Numerics.Complex(real, imaginary);
                    }
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
