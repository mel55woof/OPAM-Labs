using System;
using System.Collections.Generic;

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

        Random rnd = new Random(N);
        List<int> numbers = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            numbers.Add(rnd.Next(-K, K + 1));
        }

        List<string> words = new List<string>
        {
            "алгоритм", "код", "оптимізація", "веб-розробка", "університет",
            "ідея", "об'єкт", "масив", "міні-проєкт", "абстракція",
            "елемент", "бізнес-логіка", "делегат", "ітерація"
        };

        List<Student> students = new List<Student>
        {
            new Student { Name = "Коваленко", Group = "КН-3/1", Score = 88 },
            new Student { Name = "Мельник",    Group = "КН-3/2", Score = 64 },
            new Student { Name = "Іщенко",     Group = "КН-3/1", Score = 93 },
            new Student { Name = "Бондар",     Group = "КН-3/1", Score = 71 },
            new Student { Name = "Савченко",   Group = "КН-3/2", Score = 55 },
            new Student { Name = "Ткачук",     Group = "КН-3/2", Score = 90 },
            new Student { Name = "Кравець",    Group = "КН-3/1", Score = 76 },
            new Student { Name = "Іванів",     Group = "КН-3/1", Score = 48 },
            new Student { Name = "Мороз",      Group = "КН-3/2", Score = 82 },
            new Student { Name = "Шевчук",     Group = "КН-3/1", Score = 67 },
            new Student { Name = "Бойко",      Group = "КН-3/2", Score = 95 },
            new Student { Name = "Кушнір",     Group = "КН-3/2", Score = 73 }
        };

        Console.WriteLine("=== ВИХІДНІ ДАНІ ===");
        Console.WriteLine("Числа: " + string.Join(", ", numbers));
        Console.WriteLine("Слова: " + string.Join(", ", words));

        RunTask1(numbers, words, students);
        RunTask2(numbers, words, students);
        RunTask3(numbers);
        RunTask4(students, words);
        RunTask5(K);
        RunEdgeCases();
    }

    static void RunTask1(List<int> numbers, List<string> words, List<Student> students)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 1: Filter<T> (a = 1) ===");

        List<int> mult3 = Algorithms.Filter(numbers, x => x % 3 == 0);
        Console.WriteLine($"Числа кратні 3 (Count={mult3.Count}): {string.Join(", ", mult3)}");

        string vowels = "аеєиіїоуюяАЕЄИІЇОУЮЯ";
        List<string> vowelWords = Algorithms.Filter(words, w => w.Length > 0 && vowels.IndexOf(w[0]) >= 0);
        Console.WriteLine($"Слова на голосну (Count={vowelWords.Count}): {string.Join(", ", vowelWords)}");

        List<Student> kn31 = Algorithms.Filter(students, s => s.Group == "КН-3/1");
        List<string> kn31Names = Algorithms.Map(kn31, s => s.Name);
        Console.WriteLine($"Студенти групи КН-3/1 (Count={kn31.Count}): {string.Join(", ", kn31Names)}");
    }

    static void RunTask2(List<int> numbers, List<string> words, List<Student> students)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 2: Map<TIn, TOut> (b = 0) ===");

        List<int> squares = Algorithms.Map(numbers, x => x * x);
        Console.WriteLine("Квадрати чисел: " + string.Join(", ", squares));

        List<int> lengths = Algorithms.Map(words, w => w.Length);
        Console.WriteLine("Довжини слів: " + string.Join(", ", lengths));

        List<string> surnames = Algorithms.Map(students, s => s.Name);
        Console.WriteLine("Прізвища студентів: " + string.Join(", ", surnames));
    }

    static void RunTask3(List<int> numbers)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 3: Aggregate<T, TAcc> (c = 1) ===");

        int sum = Algorithms.Aggregate(numbers, 0, (acc, x) => acc + x);
        Console.WriteLine($"1. Сума: {sum}");

        long product = Algorithms.Aggregate(numbers, 1L, (acc, x) => acc * x);
        Console.WriteLine($"2. Добуток: {product}");

        int max = Algorithms.Aggregate(numbers, numbers[0], (acc, x) => x > acc ? x : acc);
        Console.WriteLine($"3. Максимум: {max}");

        int maxAbs = Algorithms.Aggregate(numbers, 0, (acc, x) => Math.Abs(x) > acc ? Math.Abs(x) : acc);
        Console.WriteLine($"4. Найбільший за абсолютною величиною: {maxAbs}");
    }

    static void RunTask4(List<Student> students, List<string> words)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 4: GroupBy<T, TKey> ===");

        var byGroup = Algorithms.GroupBy(students, s => s.Group);
        Console.WriteLine("1. За навчальною групою:");
        foreach (var g in byGroup) Console.WriteLine($"   {g.Key}: {g.Value.Count} шт.");

        var byLetter = Algorithms.GroupBy(students, s => char.ToUpper(s.Name[0]));
        Console.WriteLine("2. За першою літерою прізвища:");
        foreach (var g in byLetter) Console.WriteLine($"   '{g.Key}': {g.Value.Count} шт.");

        var byScoreRange = Algorithms.GroupBy(students, s =>
        {
            if (s.Score < 60) return "0-59";
            if (s.Score < 75) return "60-74";
            if (s.Score < 90) return "75-89";
            return "90-100";
        });
        Console.WriteLine("3. За діапазоном балу:");
        foreach (var g in byScoreRange) Console.WriteLine($"   {g.Key}: {g.Value.Count} шт.");

        var byWordLength = Algorithms.GroupBy(words, w => w.Length);
        Console.WriteLine("4. Слова за довжиною:");
        foreach (var g in byWordLength) Console.WriteLine($"   Довжина {g.Key}: {g.Value.Count} шт.");
    }

    static void RunTask5(int K)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 5: Захоплення змінних ===");

        int threshold = K;
        Func<int, bool> isAbove = x => x > threshold;
        threshold = 1000;
        Console.WriteLine($"1. Змінна після лямбди (перевірка 50 > threshold): {isAbove(50)}");

        List<Func<int>> forActions = new List<Func<int>>();
        for (int i = 0; i < 3; i++)
        {
            forActions.Add(() => i);
        }
        Console.Write("2. Лічильник циклу for: ");
        foreach (var act in forActions) Console.Write(act() + " ");
        Console.WriteLine();

        List<Func<int>> foreachActions = new List<Func<int>>();
        int[] arr = { 0, 1, 2 };
        foreach (int item in arr)
        {
            foreachActions.Add(() => item);
        }
        Console.Write("3. Цикл foreach: ");
        foreach (var act in foreachActions) Console.Write(act() + " ");
        Console.WriteLine();
    }

    static void RunEdgeCases()
    {
        Console.WriteLine("\n=== КРАЙОВІ ВИПАДКИ ===");
        List<int> empty = new List<int>();
        Console.WriteLine($"Порожній Filter: {Algorithms.Filter(empty, x => x > 0).Count}");
        Console.WriteLine($"Порожній Map: {Algorithms.Map(empty, x => x * 2).Count}");
        Console.WriteLine($"Порожній GroupBy: {Algorithms.GroupBy(empty, x => x).Count}");

        string emptyMax = empty.Count == 0 ? "Помилка/Невизначено (порожній список)" : Algorithms.Aggregate(empty, empty[0], (a, b) => b > a ? b : a).ToString();
        Console.WriteLine($"Порожній Aggregate (максимум): {emptyMax}");

        List<int> noMatch = Algorithms.Filter(new List<int> { 1, 2, 4 }, x => x % 3 == 0);
        Console.WriteLine($"Жоден не підходить (Filter): {noMatch.Count}");

        var oneGroup = Algorithms.GroupBy(new List<int> { 10, 20, 30 }, x => "Всі");
        Console.WriteLine($"Усі в одній групі (GroupBy): груп = {oneGroup.Count}, елементів = {oneGroup["Всі"].Count}");

        List<int> single = new List<int> { 42 };
        Console.WriteLine($"Один елемент (Aggregate сума): {Algorithms.Aggregate(single, 0, (a, b) => a + b)}");
    }
}
