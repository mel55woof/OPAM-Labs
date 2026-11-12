using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 16;
        string X = "А-00";
        string Y = "А-99";
        string missingTarget = "Я-99";

        Func<string, string, int> cmp = (a, b) => a.CompareTo(b);

        string[] unsortedArray = GenerateCodes(100000, N);
        for (int i = 0; i < unsortedArray.Length; i++)
        {
            if (unsortedArray[i] == missingTarget) unsortedArray[i] = "Я-98";
        }
        string[] array = (string[])unsortedArray.Clone();
        array[0] = X;
        array[10] = X;
        array[20] = X;
        Array.Sort(array, (a, b) => cmp(a, b));

        Console.WriteLine("\n=== ЗАВДАННЯ 2: Бінарний пошук та чотири поломки ===");
        RunBinaryTests(array, unsortedArray, X, missingTarget, cmp);

        Console.WriteLine("\n=== ЗАВДАННЯ 3: Пошук межі (b = 0) ===");
        RunLowerBoundTests(array, X, cmp);

        Console.WriteLine("\n=== ЗАВДАННЯ 4: Додатковий варіант пошуку (c = 1) ===");
        RunAlternativeSearchTests(array, X, missingTarget, cmp);

        Console.WriteLine("\n=== ЗАВДАННЯ 5: Вимірювання та точка окупності ===");
        RunMeasurements(N, cmp);

        Console.WriteLine("\n=== КРАЙОВІ ВИПАДКИ ===");
        RunEdgeCases(cmp);
    }

    static string[] GenerateCodes(int size, int seed)
    {
        Random rnd = new Random(seed);
        string[] arr = new string[size];
        string[] ukrLetters = { "А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "І", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ю", "Я" };
        for (int i = 0; i < size; i++)
        {
            string l = ukrLetters[rnd.Next(ukrLetters.Length)];
            int d = rnd.Next(0, 100);
            arr[i] = $"{l}-{d:D2}";
        }
        return arr;
    }

    static void RunLinearTests(string[] array, string X, string missing, Func<string, string, int> cmp)
    {
        int i1 = Search.LinearSearch(array, X, cmp, out int c1);
        int i2 = Search.LinearSearch(array, array[0], cmp, out int c2);
        int i3 = Search.LinearSearch(array, missing, cmp, out int c3);

        Console.WriteLine($"1. Значення X ({X}): індекс = {i1}, порівнянь = {c1}");
        Console.WriteLine($"2. Перший елемент ({array[0]}): індекс = {i2}, порівнянь = {c2}");
        Console.WriteLine($"3. Відсутнє значення ({missing}): індекс = {i3}, порівнянь = {c3}");
    }

    static void RunBinaryTests(string[] array, string[] unsortedArray, string X, string missing, Func<string, string, int> cmp)
    {
        int i1 = Search.BinarySearch(array, X, cmp, out int c1);
        int i2 = Search.BinarySearch(array, array[0], cmp, out int c2);
        int i3 = Search.BinarySearch(array, missing, cmp, out int c3);

        Console.WriteLine($"Класичний бінарний -> X: індекс={i1} (пор. {c1}), Перший: індекс={i2} (пор. {c2}), Відсутній: індекс={i3} (пор. {c3})");

        string lastElem = array[array.Length - 1];
        int res1 = BrokenSearch.Break1(array, lastElem, cmp, out int b1);
        Console.WriteLine($"Поломка 1 (while lo < hi) для останнього ({lastElem}): індекс = {res1} (очікувалось {array.Length - 1}), порівнянь = {b1}");

        int res2 = BrokenSearch.Break2(array, missing, cmp, out int b2);
        Console.WriteLine($"Поломка 2 (lo = mid) для відсутнього ({missing}): цикл зупинено лічильником, порівнянь = {b2}, результат = {res2}");

        long maxN = (long)int.MaxValue + 1L;
        Console.WriteLine($"Поломка 3 (mid = (lo+hi)/2): ламається при розмірі >= {maxN / 2 + 1} (переповнення int при lo + hi > {int.MaxValue}). На 100 000 працює без збоїв.");

        int notFoundCount = 0;
        Random r = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            string existingTarget = unsortedArray[r.Next(unsortedArray.Length)];
            int res = BrokenSearch.Break4(unsortedArray, existingTarget, cmp, out _);
            if (res == -1) notFoundCount++;
        }
        Console.WriteLine($"Поломка 4 (невідсортований масив): із 100 наявних елементів повернули -1: {notFoundCount}");
    }

    static void RunLowerBoundTests(string[] array, string X, Func<string, string, int> cmp)
    {
        int firstIdx = Search.LowerBound(array, val => cmp(val, X) >= 0, out int compFirst);
        int afterIdx = Search.LowerBound(array, val => cmp(val, X) > 0, out int compAfter);
        int countX = afterIdx - firstIdx;

        int linearCount = 0;
        int linearComp = 0;
        for (int i = 0; i < array.Length; i++)
        {
            linearComp++;
            if (cmp(array[i], X) == 0) linearCount++;
        }

        Console.WriteLine($"Індекс першого входження X ({X}): індекс = {firstIdx}, порівнянь = {compFirst}");
        Console.WriteLine($"Кількість елементів == X: {countX} (бінарно порівнянь: {compFirst + compAfter}, лінійно: {linearComp})");

        int badIdx = Search.LowerBound(array, val => val.Contains('7'), out int compBad);
        int realFirst7 = -1;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Contains('7')) { realFirst7 = i; break; }
        }
        string foundVal = badIdx < array.Length ? array[badIdx] : "не знайдено";
        Console.WriteLine($"Неузгоджена умова (перший код із цифрою '7'): LowerBound дав індекс {badIdx} ('{foundVal}', пор. {compBad}), реальний перший індекс = {realFirst7} ('{array[realFirst7]}') -> Правильно: {badIdx == realFirst7}");
    }

    static void RunAlternativeSearchTests(string[] array, string X, string missing, Func<string, string, int> cmp)
    {
        int i1 = Search.BinarySearchNoEquality(array, X, cmp, out int c1);
        int i2 = Search.BinarySearchNoEquality(array, array[0], cmp, out int c2);
        int i3 = Search.BinarySearchNoEquality(array, missing, cmp, out int c3);
        Console.WriteLine($"Бінарний без перевірки рівності -> X: індекс={i1} (пор. {c1}), Перший: індекс={i2} (пор. {c2}), Відсутній: індекс={i3} (пор. {c3})");
    }

    static void RunMeasurements(int N, Func<string, string, int> cmp)
    {
        Console.WriteLine("Таблиця 1 (середнє по 100 випадкових запитах):");
        Console.WriteLine("| n | Лінійний | Бінарний | Відношення | Теорія: n/2 і log2 n |");
        Console.WriteLine("|---|---|---|---|---|");

        int[] sizes = { 1000, 10000, 100000, 1000000 };
        int[] theoLin = { 500, 5000, 50000, 500000 };
        int[] theoBin = { 10, 14, 17, 20 };

        for (int s = 0; s < sizes.Length; s++)
        {
            int n = sizes[s];
            string[] arr = GenerateCodes(n, N);
            Array.Sort(arr, (a, b) => cmp(a, b));

            Random qRnd = new Random(123);
            long sumLin = 0;
            long sumBin = 0;

            for (int q = 0; q < 100; q++)
            {
                string target = arr[qRnd.Next(n)];
                Search.LinearSearch(arr, target, cmp, out int cLin);
                Search.BinarySearch(arr, target, cmp, out int cBin);
                sumLin += cLin;
                sumBin += cBin;
            }

            double avgLin = sumLin / 100.0;
            double avgBin = sumBin / 100.0;
            double ratio = avgLin / avgBin;
            Console.WriteLine($"| {n} | {avgLin:F0} | {avgBin:F1} | {ratio:F1}x | {theoLin[s]} і {theoBin[s]} |");
        }

        Console.WriteLine("\nТаблиця 2 (час на 100 000 елементів, мін. із 3 прогонів):");
        Console.WriteLine("| q | Лінійно, мс | Сортування + бінарно, мс | Хто швидший |");
        Console.WriteLine("|---|---|---|---|");

        int[] qs = { 1, 5, 10, 20, 30, 50, 100 };
        string[] baseArr = GenerateCodes(100000, N);

        MeasureLinearBestOf3(baseArr, 1, N, cmp);
        MeasureSortBinaryBestOf3(baseArr, 1, N, cmp);

        foreach (int q in qs)
        {
            double tLin = MeasureLinearBestOf3(baseArr, q, N, cmp);
            double tSortBin = MeasureSortBinaryBestOf3(baseArr, q, N, cmp);
            string faster = tLin < tSortBin ? "Лінійний" : "Сортування + бінарно";
            Console.WriteLine($"| {q} | {tLin:F2} | {tSortBin:F2} | {faster} |");
        }
    }

    static double MeasureLinearBestOf3(string[] original, int q, int seed, Func<string, string, int> cmp)
    {
        double bestMs = double.MaxValue;
        for (int run = 0; run < 3; run++)
        {
            Random qRnd = new Random(seed + run);
            string[] queries = new string[q];
            for (int i = 0; i < q; i++) queries[i] = original[qRnd.Next(original.Length)];

            Stopwatch sw = Stopwatch.StartNew();
            for (int k = 0; k < q; k++)
            {
                Search.LinearSearch(original, queries[k], cmp, out _);
            }
            sw.Stop();
            double ms = sw.Elapsed.TotalMilliseconds;
            if (ms < bestMs) bestMs = ms;
        }
        return bestMs;
    }

    static double MeasureSortBinaryBestOf3(string[] original, int q, int seed, Func<string, string, int> cmp)
    {
        double bestMs = double.MaxValue;
        for (int run = 0; run < 3; run++)
        {
            string[] arr = (string[])original.Clone();
            Random qRnd = new Random(seed + run);
            string[] queries = new string[q];
            for (int i = 0; i < q; i++) queries[i] = original[qRnd.Next(original.Length)];

            Stopwatch sw = Stopwatch.StartNew();
            Array.Sort(arr, (a, b) => cmp(a, b));
            for (int k = 0; k < q; k++)
            {
                Search.BinarySearch(arr, queries[k], cmp, out _);
            }
            sw.Stop();
            double ms = sw.Elapsed.TotalMilliseconds;
            if (ms < bestMs) bestMs = ms;
        }
        return bestMs;
    }

    static void RunEdgeCases(Func<string, string, int> cmp)
    {
        string[] empty = new string[0];
        int r1 = Search.BinarySearch(empty, "А-00", cmp, out _);
        Console.WriteLine($"Порожній масив (пошук): {r1}");

        string[] single = { "А-00" };
        int r2 = Search.BinarySearch(single, "А-00", cmp, out _);
        Console.WriteLine($"Один елемент, він же шуканий: {r2}");

        string[] two = { "А-01", "А-02" };
        int r3 = Search.BinarySearch(two, "А-02", cmp, out _);
        Console.WriteLine($"Два елементи, шуканий — другий: {r3}");

        string[] test = { "Б-01", "Б-02", "Б-03" };
        int r4 = Search.LowerBound(test, x => cmp(x, "А-00") >= 0, out _);
        Console.WriteLine($"Шукане менше за всі елементи (межа): {r4}");

        int r5 = Search.LowerBound(test, x => cmp(x, "В-99") >= 0, out _);
        Console.WriteLine($"Шукане більше за всі елементи (межа): {r5}");

        string[] allSame = { "А-00", "А-00", "А-00", "А-00" };
        int firstSame = Search.LowerBound(allSame, x => cmp(x, "А-00") >= 0, out _);
        int lastSame = Search.LowerBound(allSame, x => cmp(x, "А-00") > 0, out _) - 1;
        Console.WriteLine($"Усі елементи однакові й рівні X: перша межа = {firstSame}, остання = {lastSame}");
    }
}
