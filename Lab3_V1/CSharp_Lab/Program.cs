using System;
using Lab1;


namespace CSharp_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = new V1DataArray("some_data", new DateTime(2021, 12, 13), 3, 4, 1, 1, FdblComplexImpl.Poly3Degree);
            Console.WriteLine("Source array:\n" + arr.ToLongString("F1") + "\n");
            printMaxMin(arr);

            var new_arr = arr.ToSmallerGrid(7);
            if (new_arr == null)
            {
                Console.WriteLine("\nERROR: new_arr == null");
            }
            else
            {
                Console.WriteLine("\nExtended array:\n" + new_arr.ToLongString("F1") + "\n");
                printMaxMin(arr);
            }
        }
        static void printMaxMin(V1DataArray arr)
        {
            Console.WriteLine("Real values: ");
            double min = 0, max = 0;
            for(int j = 0; j < arr.CountYTicks; ++j)
            {
                if (!arr.Max_Field_Re(j, ref min, ref max))
                {
                    Console.WriteLine($"\nINVALID INDEX: {j}");
                }
                else
                {
                    Console.WriteLine($"jy = {j} \tMin: {min}  \tMax: {max}");
                }
            }

            Console.WriteLine("Imaginary values: ");
            for (int j = 0; j < arr.CountYTicks; ++j)
            {
                if (!arr.Max_Field_Im(j, ref min, ref max))
                {
                    Console.WriteLine($"\nINVALID INDEX: {j}");
                }
                else
                {
                    Console.WriteLine($"jy = {j} \tMin: {min}  \tMax: {max}");
                }
            }
        }
        
       
    }
}
