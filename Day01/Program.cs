using System;
using System.IO;
using System.Linq;

namespace DayOne
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Where(x =>!string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();
            const int expectedSum = 2020;

            int first = -1;
            int second = -1;
            int third = -1;
            bool breakCycle = false;

            for (int i = 0; i < input.Length; i++)
            {
                first = input[i];

                for (int j = 0; j < input.Length; j++)
                {
                    second = input[j];

                    for (int k = 0; k < input.Length; k++)
                    {
                        third = input[k];

                        if ((first + second + third) == expectedSum)
                        {
                            breakCycle = true;
                            break;
                        }
                    }

                    if (breakCycle)
                        break;

                }

                if (breakCycle)
                    break;
            }

            Console.WriteLine($"Found three integers that are together 2020: {first} & {second} & {third}");
            Console.WriteLine($"Sum of both: {first * second * third}");
        }
    }
}
