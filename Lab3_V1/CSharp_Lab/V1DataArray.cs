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
        public System.Numerics.Complex? FieldAt(int jx, int jy)
        {
            if (jx < CountXTicks && jy < CountYTicks) return Grid[jx, jy];
            return null;
        }
        public bool Max_Field_Re(int jy, ref double min, ref double max)
        {
            if (jy >= CountYTicks) return false;
            min = max = Grid[0, jy].Real;
            for (int jx = 1; jx < CountXTicks; ++jx)
            {
                if (Grid[jx, jy].Real > max) max = Grid[jx, jy].Real;
                if (Grid[jx, jy].Real < min) min = Grid[jx, jy].Real;
            }
            return true;
        }
        public bool Max_Field_Im(int jy, ref double min, ref double max)
        {
            if (jy >= CountYTicks) return false;
            min = max = Grid[0, jy].Imaginary;
            for (int jx = 1; jx < CountXTicks; ++jx)
            {
                if (Grid[jx, jy].Imaginary > max) max = Grid[jx, jy].Imaginary;
                if (Grid[jx, jy].Imaginary < min) min = Grid[jx, jy].Imaginary;
            }
            return true;
        }
        [System.Runtime.InteropServices.DllImport("..\\..\\..\\..\\x64\\Debug\\CPP_DLL_V1.dll", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern
        void VM_Interpolate(int nx, double[] x, int ny, double[] y, int nsite, double[] y_new, ref int ret);
        public V1DataArray ToSmallerGrid(int ns)
        {
            int ret = -1;
            int nx = CountXTicks;
            double[] x = new double[2];
            x[0] = 0;
            x[1] = CountXTicks * XTick;
            int ny = 2 * CountYTicks;
            double[] y = new double[ny * nx];
            for (int j= 0; j < CountYTicks; ++j)
                for (int i = 0; i < CountXTicks; ++i)
                {
                    y[CountXTicks * 2 * j + i] = Grid[i, j].Real;
                    y[CountXTicks * (2 * j + 1) + i] = Grid[i, j].Imaginary;
                }

            double[] y_new = new double[ny * ns];

            VM_Interpolate(nx, x, ny, y, ns, y_new, ref ret);
            if (ret != 0) return null;

            V1DataArray ret_arr = new V1DataArray(DataId, Date);
            ret_arr.CountXTicks = ns;
            ret_arr.CountYTicks = CountYTicks;
            ret_arr.XTick = XTick * (CountXTicks - 1) / (ns - 1);
            ret_arr.YTick = YTick;
            ret_arr.Grid = new System.Numerics.Complex[ret_arr.CountXTicks, ret_arr.CountYTicks];

            for (int j = 0; j < ret_arr.CountYTicks; ++j)
                for (int i = 0; i < ret_arr.CountXTicks; ++i)
                {
                    double real = y_new[ret_arr.CountXTicks * 2 * j + i];
                    double imaginary = y_new[ret_arr.CountXTicks * (2 * j + 1) + i];
                    ret_arr.Grid[i, j] = new System.Numerics.Complex(real, imaginary);
                }
            return ret_arr;
        }
    }
}
