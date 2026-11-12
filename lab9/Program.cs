using System;
using System.Diagnostics;
using System.Linq;

class Student
{
    public string Name;
    public string Group;
    public int Score;
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 16;
        int K = 1;
        int maxVal = 20 * K;
        Func<int, int, int> cmpInt = (x, y) => x.CompareTo(y);

        Console.WriteLine("=== ЗАВДАННЯ 1: Трасування для 7 елементів ===");
        RunTrace(N, K);

        Console.WriteLine("\n=== ЗАВДАННЯ 4: Порівняння (порівняння / переставляння) ===");
        RunTask4Comparisons(N, K, maxVal, cmpInt);

        Console.WriteLine("\n=== ЗАВДАННЯ 5: Заміри часу та стабільність ===");
        RunBenchmarksAndStability(N, maxVal, cmpInt);

        Console.WriteLine("\n=== КРАЙОВІ ВИПАДКИ ===");
        RunEdgeCases(cmpInt);
    }

    static void RunTrace(int N, int K)
    {
        int traceSize = K % 5 + 6;
        Random rnd = new Random(N);
        int[] traceArr = new int[traceSize];
        for (int i = 0; i < traceSize; i++) traceArr[i] = rnd.Next(0, 20 * K + 1);

        Console.WriteLine("Початковий масив: " + string.Join(", ", traceArr));
        int[] demo = (int[])traceArr.Clone();
        for (int i = 1; i < demo.Length; i++)
        {
            int key = demo[i];
            int j = i - 1;
            while (j >= 0 && demo[j] > key)
            {
                demo[j + 1] = demo[j];
                j--;
            }
            demo[j + 1] = key;
            Console.WriteLine($"Прохід {i}: " + string.Join(", ", demo));
        }
    }

    static int[] GenerateArray(int size, int maxVal, int seed)
    {
        Random rnd = new Random(seed);
        int[] arr = new int[size];
        for (int i = 0; i < size; i++) arr[i] = rnd.Next(0, maxVal + 1);
        return arr;
    }

    static void RunTask4Comparisons(int N, int K, int maxVal, Func<int, int, int> cmp)
    {
        int[] sizes = { 100, 1000, 10000 };
        Console.WriteLine("| n | Алгоритм | Випадковий | Відсортований | Навпаки | Майже |");
        Console.WriteLine("|---|---|---|---|---|---|");

        foreach (int n in sizes)
        {
            int[] randomArr = GenerateArray(n, maxVal, N);
            int[] sorted = (int[])randomArr.Clone();
            Array.Sort(sorted);

            int[] reversed = (int[])sorted.Clone();
            Array.Reverse(reversed);

            int[] almost = (int[])sorted.Clone();
            Random swapRnd = new Random(N);
            for (int s = 0; s < K; s++)
            {
                int idx1 = swapRnd.Next(n);
                int idx2 = swapRnd.Next(n);
                int tmp = almost[idx1];
                almost[idx1] = almost[idx2];
                almost[idx2] = tmp;
            }

            RunOn4Types(n, randomArr, sorted, reversed, almost, cmp, "Вставками", Sorts.InsertionSort);
            RunOn4Types(n, randomArr, sorted, reversed, almost, cmp, "Злиттям", Sorts.MergeSort);
            RunOn4Types(n, randomArr, sorted, reversed, almost, cmp, "Бінарні вставки", Sorts.BinaryInsertionSort);
        }
    }

    delegate void SortAction<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves);

    static void RunOn4Types(int n, int[] rand, int[] sort, int[] rev, int[] alm, Func<int, int, int> cmp, string name, SortAction<int> sortAlgo)
    {
        int[] a1 = (int[])rand.Clone(); sortAlgo(a1, cmp, out int c1, out int m1);
        int[] a2 = (int[])sort.Clone(); sortAlgo(a2, cmp, out int c2, out int m2);
        int[] a3 = (int[])rev.Clone(); sortAlgo(a3, cmp, out int c3, out int m3);
        int[] a4 = (int[])alm.Clone(); sortAlgo(a4, cmp, out int c4, out int m4);

        Console.WriteLine($"| {n} | {name} | {c1} / {m1} | {c2} / {m2} | {c3} / {m3} | {c4} / {m4} |");
    }

    static void RunBenchmarksAndStability(int N, int maxVal, Func<int, int, int> cmp)
    {
        Console.WriteLine("\nТаблиця часу (Release, мс):");
        Console.WriteLine("| n | Простий (зав. 1) | Швидкий (зав. 2) | Додатковий (зав. 3) | Array.Sort |");
        Console.WriteLine("|---|---|---|---|---|");

        int[] benchSizes = { 1000, 10000, 100000, 1000000 };
        foreach (int n in benchSizes)
        {
            int[] arr = GenerateArray(n, maxVal, N);

            double tIns = n <= 100000 ? MeasureTime(arr, Sorts.InsertionSort, cmp) : -1;
            double tMerge = MeasureTime(arr, Sorts.MergeSort, cmp);
            double tBinIns = n <= 100000 ? MeasureTime(arr, Sorts.BinaryInsertionSort, cmp) : -1;
            double tStd = MeasureStandardSortTime(arr);

            string insStr = tIns >= 0 ? $"{tIns:F2}" : "—";
            string binInsStr = tBinIns >= 0 ? $"{tBinIns:F2}" : "—";
            Console.WriteLine($"| {n} | {insStr} | {tMerge:F2} | {binInsStr} | {tStd:F2} |");
        }

        int limitN = 10000;
        while (limitN <= 500000)
        {
            int[] testArr = GenerateArray(limitN, maxVal, N);
            double ms = MeasureTime(testArr, Sorts.InsertionSort, cmp);
            if (ms > 1000.0)
            {
                Console.WriteLine($"\nМежа прийнятності простого сортування (> 1000 мс): n = {limitN} ({ms:F0} мс)");
                break;
            }
            limitN += 10000;
        }

        Console.WriteLine("\nПеревірка стабільності (Student[]):");
        Student[] baseStudents =
        {
            new Student { Name = "Андрієнко", Group = "КН-3/1", Score = 85 },
            new Student { Name = "Бондар", Group = "КН-3/1", Score = 70 },
            new Student { Name = "Василенко", Group = "КН-3/1", Score = 85 },
            new Student { Name = "Гриценко", Group = "КН-3/2", Score = 70 },
            new Student { Name = "Демченко", Group = "КН-3/1", Score = 90 },
            new Student { Name = "Коваленко", Group = "КН-3/2", Score = 85 }
        };

        Func<Student, Student, int> cmpScore = (x, y) => x.Score.CompareTo(y.Score);

        Student[] s1 = CloneStudents(baseStudents);
        Sorts.InsertionSort(s1, cmpScore, out _, out _);
        CheckStability("Простий (вставками)", s1);

        Student[] s2 = CloneStudents(baseStudents);
        Sorts.MergeSort(s2, cmpScore, out _, out _);
        CheckStability("Швидкий (злиттям)", s2);

        Student[] s3 = CloneStudents(baseStudents);
        Array.Sort(s3, (x, y) => cmpScore(x, y));
        CheckStability("Array.Sort", s3);

        Student[] s4 = CloneStudents(baseStudents).OrderBy(x => x.Score).ToArray();
        CheckStability("OrderBy", s4);
    }

    static Student[] CloneStudents(Student[] src)
    {
        Student[] res = new Student[src.Length];
        for (int i = 0; i < src.Length; i++)
        {
            res[i] = new Student { Name = src[i].Name, Group = src[i].Group, Score = src[i].Score };
        }
        return res;
    }

    static void CheckStability(string algoName, Student[] arr)
    {
        string counterExample = "немає";
        bool stable = true;
        for (int i = 0; i < arr.Length - 1; i++)
        {
            if (arr[i].Score == arr[i + 1].Score && arr[i].Name.CompareTo(arr[i + 1].Name) > 0)
            {
                stable = false;
                counterExample = $"{arr[i].Name} ({arr[i].Score}) перед {arr[i + 1].Name} ({arr[i + 1].Score})";
                break;
            }
        }
        Console.WriteLine($"{algoName}: {(stable ? "Стабільний" : "Нестабільний")} | Контрприклад: {counterExample}");
    }

    static double MeasureTime(int[] original, SortAction<int> sortAlgo, Func<int, int, int> cmp)
    {
        int[] dummy = (int[])original.Clone();
        sortAlgo(dummy, cmp, out _, out _);

        double best = double.MaxValue;
        for (int i = 0; i < 3; i++)
        {
            int[] arr = (int[])original.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            sortAlgo(arr, cmp, out _, out _);
            sw.Stop();
            if (sw.Elapsed.TotalMilliseconds < best) best = sw.Elapsed.TotalMilliseconds;
        }
        return best;
    }

    static double MeasureStandardSortTime(int[] original)
    {
        int[] dummy = (int[])original.Clone();
        Array.Sort(dummy);

        double best = double.MaxValue;
        for (int i = 0; i < 3; i++)
        {
            int[] arr = (int[])original.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            Array.Sort(arr);
            sw.Stop();
            if (sw.Elapsed.TotalMilliseconds < best) best = sw.Elapsed.TotalMilliseconds;
        }
        return best;
    }

    static void RunEdgeCases(Func<int, int, int> cmp)
    {
        TestEdge(new int[0], "порожній масив", cmp);
        TestEdge(new int[] { 42 }, "один елемент", cmp);
        TestEdge(new int[] { 1, 2 }, "два елементи в порядку", cmp);
        TestEdge(new int[] { 2, 1 }, "два елементи навпаки", cmp);
        TestEdge(new int[] { 5, 5, 5, 5 }, "усі елементи однакові", cmp);
        TestEdge(new int[] { 1, 1, 2, 2 }, "лише два різних значення, по пів масиву кожного", cmp);
    }

    static void TestEdge(int[] arr, string label, Func<int, int, int> cmp)
    {
        int[] a1 = (int[])arr.Clone();
        Sorts.InsertionSort(a1, cmp, out _, out _);

        int[] a2 = (int[])arr.Clone();
        Sorts.MergeSort(a2, cmp, out _, out _);

        int[] a3 = (int[])arr.Clone();
        Sorts.BinaryInsertionSort(a3, cmp, out _, out _);

        bool ok = Checkers.IsSorted(a1, cmp) && Checkers.IsPermutationOf(arr, a1)
               && Checkers.IsSorted(a2, cmp) && Checkers.IsPermutationOf(arr, a2)
               && Checkers.IsSorted(a3, cmp) && Checkers.IsPermutationOf(arr, a3);

        Console.WriteLine($"{label}: {(ok ? "Усі 3 пройшли IsSorted + IsPermutationOf" : "Помилка")}");
    }
}
