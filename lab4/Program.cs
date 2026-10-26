using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<int> mainList = GenerateMainList(20, 16);
        Console.WriteLine("=== ОСНОВНИЙ СПИСОК ===");
        Console.WriteLine(string.Join(", ", mainList));

        Console.WriteLine("\n=== ЗАВДАННЯ 1: Побудова нового списку (кратні 3) ===");
        List<int> filteredList = FilterDivisibleByThree(mainList);
        Console.WriteLine("Результат: " + string.Join(", ", filteredList));
        Console.WriteLine($"Count: {filteredList.Count}");
        Console.WriteLine("Оригінальний список не змінився: " + (mainList.Count == 20));

        Console.WriteLine("\n=== ЗАВДАННЯ 2: Вилучення елементів, кратних 3 ===");
        
        List<int> copy1 = new List<int>(mainList);
        RemoveFalse(copy1);
        Console.WriteLine("Хибний варіант (з початку): " + string.Join(", ", copy1));

        List<int> copy2 = new List<int>(mainList);
        RemoveFromEnd(copy2);
        Console.WriteLine("Правильний варіант 2 (з кінця): " + string.Join(", ", copy2));

        List<int> copy3 = new List<int>(mainList);
        List<int> copy3Result = RemoveNewList(copy3);
        Console.WriteLine("Правильний варіант 3 (новий список): " + string.Join(", ", copy3Result));

        bool areEqual = AreListsEqual(copy2, copy3Result);
        Console.WriteLine($"Варіанти 2 і 3 дають однаковий результат: {areEqual}");

        Console.WriteLine("\n=== ЗАВДАННЯ 3: Злиття двох впорядкованих списків ===");

        List<int> sorted1 = GenerateSortedList(11, 16);
        List<int> sorted2 = GenerateSortedList(7, 116);

        Console.WriteLine("Список 1: " + string.Join(", ", sorted1));
        Console.WriteLine("Список 2: " + string.Join(", ", sorted2));

        List<int> merged = MergeSortedLists(sorted1, sorted2);
        Console.WriteLine("Злитий список: " + string.Join(", ", merged));
        Console.WriteLine("Впорядкованість злитого списку: " + IsSorted(merged));

        Console.WriteLine("\n=== ЗАВДАННЯ 4: Розділення списку ===");
        SplitList(mainList, out List<int> negatives, out List<int> nonNegatives);
        Console.WriteLine("Від'ємні: " + string.Join(", ", negatives));
        Console.WriteLine("Невід'ємні: " + string.Join(", ", nonNegatives));
        bool countMatches = (negatives.Count + nonNegatives.Count == mainList.Count);
        Console.WriteLine($"Сума Count обох списків дорівнює Count вихідного: {countMatches}");

        Console.WriteLine("\n=== ЗАВДАННЯ 5: Замір часу ===");
        RunBenchmark();
    }

    static List<int> GenerateMainList(int count, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(rnd.Next(-9, 10)); // [-9; 9]
        }
        return list;
    }

    static List<int> GenerateSortedList(int count, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        if (count <= 0) return list;

        int current = rnd.Next(1, 10);
        list.Add(current);
        for (int i = 1; i < count; i++)
        {
            current += rnd.Next(1, 10);
            list.Add(current);
        }
        return list;
    }

    static List<int> FilterDivisibleByThree(List<int> source)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] != 0 && source[i] % 3 == 0)
            {
                result.Add(source[i]);
            }
        }
        return result;
    }

    static void RemoveFalse(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != 0 && list[i] % 3 == 0)
            {
                list.RemoveAt(i);
        
            }
        }
    }

    static void RemoveFromEnd(List<int> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] != 0 && list[i] % 3 == 0)
            {
                list.RemoveAt(i);
            }
        }
    }

    static List<int> RemoveNewList(List<int> list)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == 0 || list[i] % 3 != 0)
            {
                result.Add(list[i]);
            }
        }
        return result;
    }

    static bool AreListsEqual(List<int> l1, List<int> l2)
    {
        if (l1.Count != l2.Count) return false;
        for (int i = 0; i < l1.Count; i++)
        {
            if (l1[i] != l2[i]) return false;
        }
        return true;
    }

    static List<int> MergeSortedLists(List<int> l1, List<int> l2)
    {
        List<int> result = new List<int>(l1.Count + l2.Count);
        int i = 0, j = 0;

        while (i < l1.Count && j < l2.Count)
        {
            if (l1[i] <= l2[j])
            {
                result.Add(l1[i]);
                i++;
            }
            else
            {
                result.Add(l2[j]);
                j++;
            }
        }


        while (i < l1.Count)
        {
            result.Add(l1[i]);
            i++;
        }

        while (j < l2.Count)
        {
            result.Add(l2[j]);
            j++;
        }

        return result;
    }

    static bool IsSorted(List<int> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i] > list[i + 1]) return false;
        }
        return true;
    }

    static void SplitList(List<int> source, out List<int> negatives, out List<int> nonNegatives)
    {
        negatives = new List<int>();
        nonNegatives = new List<int>();

        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] < 0)
                negatives.Add(source[i]);
            else
                nonNegatives.Add(source[i]);
        }
    }

    static void RunBenchmark()
    {
        int n = 100000;

        List<int> warmup = new List<int>();
        for (int i = 0; i < 1000; i++) warmup.Add(i);

        Stopwatch sw = new Stopwatch();

        sw.Start();
        List<int> list1 = new List<int>();
        for (int i = 0; i < n; i++) list1.Add(i);
        sw.Stop();
        long timeAddDefault = sw.ElapsedMilliseconds;

        sw.Restart();
        List<int> list2 = new List<int>(n);
        for (int i = 0; i < n; i++) list2.Add(i);
        sw.Stop();
        long timeAddCapacity = sw.ElapsedMilliseconds;

        sw.Restart();
        List<int> list3 = new List<int>();
        for (int i = 0; i < n; i++) list3.Add(i);

        List<int> listInsert = new List<int>();
        for (int i = 0; i < n; i++) listInsert.Insert(0, i);
        sw.Stop();
        long timeInsert = sw.ElapsedMilliseconds;


        sw.Restart();
        for (int i = 0; i < 1000; i++)
        {
            bool found = list1.Contains(-999); 
        }
        sw.Stop();
        long timeContains = sw.ElapsedMilliseconds;

        Console.WriteLine($"1. 100k Add (без ємності): {timeAddDefault} мс");
        Console.WriteLine($"2. 100k Add (з ємністю 100k): {timeAddCapacity} мс");
        Console.WriteLine($"3. 100k Insert(0, x): {timeInsert} мс");
        Console.WriteLine($"4. 1k Contains (у 100k елементах): {timeContains} мс");
    }
}
