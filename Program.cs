using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Линейно-конгруэнтный генератор (ЛКМ)
    class LCG
    {
        private long seed;
        private const long a = 1664525;  // множитель
        private const long c = 1013904223; // приращение
        private readonly long m = (long)Math.Pow(2, 32); // модуль
        public LCG(long seed)
        {
            this.seed = seed;
        }
        public int Next()
        {
            seed = (a * seed + c) % m;
            return (int)(seed & 0x7FFFFFFF);
        }
        public double NextDouble()
        {
            return (double)Next() / int.MaxValue;
        }
        public long GetSeed()
        {
            return seed;
        }
        public void SetSeed(long newSeed)
        {
            seed = newSeed;
        }
    }
    static void Main(string[] args)
    {
        LCG lcg = new LCG(DateTime.Now.Ticks);
        Random random = new Random();
        Console.WriteLine("LCG генератор:");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(lcg.NextDouble());
        }

        Console.WriteLine("\nБиблиотечный генератор (Random):");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(random.NextDouble());
        }
        TestUniformity(lcg, random, 10000);
        TestIndependence(lcg, 10000);
    }
    // тест на равномерность
    static void TestUniformity(LCG lcg, Random random, int iterations)
    {
        int[] lcgCounts = new int[10];
        int[] randomCounts = new int[10];
        for (int i = 0; i < iterations; i++)
        {
            lcgCounts[(int)(lcg.NextDouble() * 10)]++;
            randomCounts[(int)(random.NextDouble() * 10)]++;
        }
        Console.WriteLine("\nТест на равномерность (LCG):");
        Console.WriteLine(string.Join(" ", lcgCounts.Select(c => c.ToString())));

        Console.WriteLine("\nТест на равномерность (Random):");
        Console.WriteLine(string.Join(" ", randomCounts.Select(c => c.ToString())));
    }
    // тест на независимость (корреляция между соседними числами)
    static void TestIndependence(LCG lcg, int iterations)
    {
        double[] numbers = new double[iterations];
        long initialSeed = lcg.GetSeed();
        // генерируем последовательность чисел
        for (int i = 0; i < iterations; i++)
        {
            numbers[i] = lcg.NextDouble();
        }
        lcg.SetSeed(initialSeed);
        double correlation = ComputeCorrelation(numbers);
        Console.WriteLine($"\nКоэффициент корреляции (LCG): {correlation}");
    }
    // поиск корреляции для теста на независимость
    static double ComputeCorrelation(double[] numbers)
    {
        double meanX = numbers.Average();
        double meanY = numbers.Skip(1).Average();
        double covariance = 0;
        double varianceX = 0;
        double varianceY = 0;
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            double x = numbers[i] - meanX;
            double y = numbers[i + 1] - meanY;
            covariance += x * y;
            varianceX += x * x;
            varianceY += y * y;
        } 
        return covariance / Math.Sqrt(varianceX * varianceY);
    }
}