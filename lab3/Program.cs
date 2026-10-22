using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== ЗАВДАННЯ 1: Обхід каталогів ===");
        Console.Write("Введіть шлях до папки: ");
        string dirPath = Console.ReadLine();
        if (Directory.Exists(dirPath))
        {
            long totalSize = GetTotalFileSize(dirPath);
            Console.WriteLine($"Сумарний розмір усіх файлів: {totalSize} байт");
        }
        else
        {
            Console.WriteLine("Помилка: Вказана папка не існує!");
        }

        Console.WriteLine("\n=== ЗАВДАННЯ 2: Розбиття навпіл (Кількість > 1) ===");
        Random rand = new Random(16);
        int[] arr16 = new int[16];
        for (int i = 0; i < arr16.Length; i++)
        {
            arr16[i] = rand.Next(-20, 21);
        }

        Console.WriteLine("Масив: " + string.Join(", ", arr16));
        int countRecursive = CountGreaterThanOne(arr16, 0, arr16.Length - 1);
        int maxDepth = (int)Math.Ceiling(Math.Log2(arr16.Length)) + 1;
        Console.WriteLine($"Кількість елементів > 1 (рекурсивно): {countRecursive}");
        Console.WriteLine($"Максимальна глибина рекурсії: {maxDepth} (log2(16) + 1 = {Math.Log2(16) + 1})");

        Console.WriteLine("\n=== ЗАВДАННЯ 3: Перебір підмножин із сумою 11 ===");
        int[] setArr = new int[10];
        for (int i = 0; i < setArr.Length; i++)
        {
            setArr[i] = rand.Next(1, 10);
        }
        Console.WriteLine("Масив (10 чисел [1..9]): " + string.Join(", ", setArr));
        Console.WriteLine("Знайдені підмножини з сумою 11:");
        int subsetsCount = FindSubsets(setArr, 0, 11, new List<int>());
        Console.WriteLine($"Загальна кількість підмножин: {subsetsCount}");

        Console.WriteLine("\n=== ЗАВДАННЯ 4: Трасування (масив з 4 елементів) ===");
        int[] arr4 = { arr16[0], arr16[1], arr16[2], arr16[3] };
        Console.WriteLine("Тестовий масив з 4 елементів: " + string.Join(", ", arr4));
        TraceCountGreaterThanOne(arr4, 0, arr4.Length - 1, 0);

        Console.WriteLine("\n=== ЗАВДАННЯ 5: Ітеративна версія (Цикл) ===");
        int countIterative = CountGreaterThanOneIterative(arr16);
        Console.WriteLine($"Кількість елементів > 1 (ітеративно): {countIterative}");
        Console.WriteLine($"Результати збігаються: {countRecursive == countIterative}");
    }

    static long GetTotalFileSize(string path)
    {
        long total = 0;
        try
        {
            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo fi = new FileInfo(file);
                total += fi.Length;
            }

            foreach (string dir in Directory.GetDirectories(path))
            {
                total += GetTotalFileSize(dir);
            }
        }
        catch (UnauthorizedAccessException)
        {
            
        }
        return total;
    }


    static int CountGreaterThanOne(int[] a, int lo, int hi)
    {
        if (lo == hi)
            return a[lo] > 1 ? 1 : 0;

        int mid = (lo + hi) / 2;
        int leftCount = CountGreaterThanOne(a, lo, mid);
        int rightCount = CountGreaterThanOne(a, mid + 1, hi);

        return leftCount + rightCount;
    }


    static int FindSubsets(int[] arr, int index, int targetSum, List<int> current)
    {
        if (targetSum == 0)
        {
            Console.WriteLine("{" + string.Join(", ", current) + "}");
            return 1;
        }

        if (index >= arr.Length || targetSum < 0)
            return 0;

        int count = 0;


        current.Add(arr[index]);
        count += FindSubsets(arr, index + 1, targetSum - arr[index], current);

        current.RemoveAt(current.Count - 1);

        count += FindSubsets(arr, index + 1, targetSum, current);

        return count;
    }

   
    static int TraceCountGreaterThanOne(int[] a, int lo, int hi, int depth)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}enter Count({lo}..{hi})");

        int res;
        if (lo == hi)
        {
            res = a[lo] > 1 ? 1 : 0;
            Console.WriteLine($"{indent}  base  Count({lo}..{hi}) -> val={a[lo]}, res={res}");
        }
        else
        {
            int mid = (lo + hi) / 2;
            int left = TraceCountGreaterThanOne(a, lo, mid, depth + 1);
            int right = TraceCountGreaterThanOne(a, mid + 1, hi, depth + 1);
            res = left + right;
        }

        Console.WriteLine($"{indent}exit  Count({lo}..{hi}) -> {res}");
        return res;
    }

   
    static int CountGreaterThanOneIterative(int[] a)
    {
        int count = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] > 1)
                count++;
        }
        return count;
    }
}
