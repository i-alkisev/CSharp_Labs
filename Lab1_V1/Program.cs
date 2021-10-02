using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = new V1DataArray("some_data", new DateTime(2021, 10, 2), 4, 3, 0.5, 1, FdblComplexImpl.F2);
            Console.WriteLine("V1DataArray.ToString:\n" + arr + "\n");
            Console.WriteLine("V1DataArray.ToLongString:\n" + arr.ToLongString("F1") + "\n");
            var lst = (V1DataList)arr;
            Console.WriteLine("V1DataList.ToString:\n" + lst + "\n");
            Console.WriteLine("V1DataList.ToLongString:\n" + lst.ToLongString("F1") + "\n");
            Console.WriteLine($" V1DataList.Count={lst.Count}\t V1DataList.AverageValue={lst.AverageValue}");
            Console.WriteLine($"V1DataArray.Count={arr.Count}\tV1DataArray.AverageValue={arr.AverageValue}\n");

            var myCollection = new V1MainCollection();
            myCollection.Add(new V1DataArray("empty_array", new DateTime(2000, 9, 4)));
            myCollection.Add(arr);
            myCollection.Add(new V1DataList("empty_list", new DateTime(2020, 1, 1)));
            lst = new V1DataList("new_list", new DateTime(2021, 10, 2));
            lst.Add(new DataItem(0, 0, new System.Numerics.Complex(1, 2)));
            lst.Add(new DataItem(1, 1, new System.Numerics.Complex(1, 0)));
            myCollection.Add(lst);
            Console.WriteLine("V1MainCollection.ToString:\n" + myCollection + "\n");
            Console.WriteLine("V1MainCollection.ToLongString:\n" + myCollection.ToLongString("F1") + "\n");

            Console.WriteLine("Count and AverageValue for each elem in V1MainCollection:\nElem_num\tCount\tAverageValue");
            for (int i = 0; i < myCollection.Count; ++i)
            {
                Console.WriteLine($"{i, 8}\t{myCollection[i].Count, 3}\t{myCollection[i].AverageValue, 10:F4}");
            }
        }
    }
}
