using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        
        Console.WriteLine("=== ЗАВДАННЯ 1: Частоти літер у тексті ===");
        string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, " +
                      "а можна записати кодом. Кодом алгоритм стає тоді, коли його " +
                      "виконує машина, а не людина. Одна задача — багато алгоритмів.";
        RunTask1(text);

        
        Console.WriteLine("\n=== ЗАВДАННЯ 2: Групування студентів за першою літерою прізвища ===");
        RunTask2();

       
        Console.WriteLine("\n=== ЗАВДАННЯ 3: Вилучення дублікатів із чисел ===");
        RunTask3();

     
        Console.WriteLine("\n=== ЗАВДАННЯ 4: Порівняння двох наборів чисел ===");
        RunTask4();

       
        Console.WriteLine("\n=== ЗАВДАННЯ 5: Порівняння продуктивності (Stopwatch) ===");
        RunTask5();
    }

     static void RunTask1(string text)
    {
        int threshold = 1 % 3 + 2; // K % 3 + 2 = 1 % 3 + 2 = 3
        Dictionary<char, int> freqDict = new Dictionary<char, int>();

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char lower = char.ToLower(c);
                if (freqDict.ContainsKey(lower))
                {
                    freqDict[lower]++;
                }
                else
                {
                    freqDict[lower] = 1;
                }
            }
        }

        Console.WriteLine($"Поріг частоти (не менше): {threshold}");
        Console.WriteLine("Елементи, що задовольняють умову:");
        int totalUnique = 0;

        foreach (var pair in freqDict)
        {
            totalUnique++;
            if (pair.Value >= threshold)
            {
                Console.WriteLine($"  '{pair.Key}': {pair.Value}");
            }
        }
        Console.WriteLine($"Загальна кількість різних літер: {totalUnique}");
    }


    static void RunTask2()
    {
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

        Dictionary<char, List<Student>> groups = new Dictionary<char, List<Student>>();

        foreach (var st in students)
        {
            char key = char.ToUpper(st.Name[0]);
            if (!groups.ContainsKey(key))
            {
                groups[key] = new List<Student>();
            }
            groups[key].Add(st);
        }

        foreach (var g in groups)
        {
            int totalScore = 0;
            foreach (var st in g.Value)
            {
                totalScore += st.Score;
            }
            double avgScore = (double)totalScore / g.Value.Count;

            Console.WriteLine($"Група (ключ '{g.Key}'): студентів = {g.Value.Count}, середній бал = {avgScore:F1}");
            foreach (var st in g.Value)
            {
                Console.WriteLine($"    - {st.Name} ({st.Group}), бал: {st.Score}");
            }
        }
    }

   
    static void RunTask3()
    {
        int limit = 1 % 10 + 6; 
        Random rnd = new Random(16);
        List<int> numbers = new List<int>();

        for (int i = 0; i < 25; i++)
        {
            numbers.Add(rnd.Next(1, limit + 1));
        }

        Console.WriteLine("Вихідний список (25 чисел):");
        Console.WriteLine(string.Join(", ", numbers));

        HashSet<int> setNoOrder = new HashSet<int>(numbers);
        List<int> result1 = new List<int>(setNoOrder);
        Console.WriteLine("Спосіб 1 (HashSet, без збереження порядку):");
        Console.WriteLine(string.Join(", ", result1));

     
        HashSet<int> seen = new HashSet<int>();
        List<int> result2 = new List<int>();
        foreach (int num in numbers)
        {
            if (seen.Add(num))
            {
                result2.Add(num);
            }
        }
        Console.WriteLine("Спосіб 2 (Зі збереженням порядку):");
        Console.WriteLine(string.Join(", ", result2));
    }

    
    static void RunTask4()
    {
        int maxVal = 20 + 1; // 20 + K = 21
        Random rnd1 = new Random(16);
        Random rnd2 = new Random(16 + 100);

        List<int> set1 = new List<int>();
        List<int> set2 = new List<int>();

        for (int i = 0; i < 30; i++)
        {
            set1.Add(rnd1.Next(1, maxVal + 1));
            set2.Add(rnd2.Next(1, maxVal + 1));
        }

        Console.WriteLine("Набір 1: " + string.Join(", ", set1));
        Console.WriteLine("Набір 2: " + string.Join(", ", set2));

    
        HashSet<int> hashSet2 = new HashSet<int>(set2);
        HashSet<int> hsResult = new HashSet<int>();
        foreach (int x in set1)
        {
            if (!hashSet2.Contains(x))
            {
                hsResult.Add(x);
            }
        }

    
        List<int> listResult = new List<int>();
        foreach (int x in set1)
        {
            bool found = false;
            foreach (int y in set2)
            {
                if (x == y)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
              
                bool alreadyInList = false;
                foreach (int r in listResult)
                {
                    if (r == x) { alreadyInList = true; break; }
                }
                if (!alreadyInList) listResult.Add(x);
            }
        }

        Console.WriteLine("Результат (HashSet): " + string.Join(", ", hsResult));
        Console.WriteLine("Результат (Списки):  " + string.Join(", ", listResult));
        
        bool match = hsResult.Count == listResult.Count;
        Console.WriteLine($"Результати обох способів збігаються: {match}");
    }

   
    static void RunTask5()
    {
        
        DummyWarmup();

        Stopwatch sw = new Stopwatch();

       
        Random r = new Random(42);
        List<int> largeList = new List<int>();
        HashSet<int> largeSet = new HashSet<int>();
        for (int i = 0; i < 100000; i++)
        {
            int val = r.Next();
            largeList.Add(val);
            largeSet.Add(val);
        }
        int[] searchItems = new int[10000];
        for (int i = 0; i < 10000; i++) searchItems[i] = r.Next();

       
        sw.Restart();
        int foundListCount = 0;
        foreach (var item in searchItems)
        {
            bool exists = false;
            foreach (var elem in largeList) { if (elem == item) { exists = true; break; } }
            if (exists) foundListCount++;
        }
        sw.Stop();
        long tList1 = sw.ElapsedMilliseconds;

        // Множина
        sw.Restart();
        int foundSetCount = 0;
        foreach (var item in searchItems)
        {
            if (largeSet.Contains(item)) foundSetCount++;
        }
        sw.Stop();
        long tSet1 = sw.ElapsedMilliseconds;

        string[] words = new string[50000];
        for (int i = 0; i < 50000; i++) words[i] = "word" + (i % 5000);

        
        sw.Restart();
        List<string> wList = new List<string>();
        List<int> cList = new List<int>();
        foreach (var w in words)
        {
            int idx = -1;
            for (int i = 0; i < wList.Count; i++)
            {
                if (wList[i] == w) { idx = i; break; }
            }
            if (idx == -1) { wList.Add(w); cList.Add(1); }
            else { cList[idx]++; }
        }
        sw.Stop();
        long tList2 = sw.ElapsedMilliseconds;

        // Словник
        sw.Restart();
        Dictionary<string, int> wDict = new Dictionary<string, int>();
        foreach (var w in words)
        {
            if (wDict.ContainsKey(w)) wDict[w]++;
            else wDict[w] = 1;
        }
        sw.Stop();
        long tDict2 = sw.ElapsedMilliseconds;


        int[] nums20k = new int[20000];
        for (int i = 0; i < 20000; i++) nums20k[i] = r.Next(1, 5000);

   
        sw.Restart();
        List<int> uniqueList = new List<int>();
        foreach (var n in nums20k)
        {
            bool exists = false;
            foreach (var u in uniqueList) { if (u == n) { exists = true; break; } }
            if (!exists) uniqueList.Add(n);
        }
        sw.Stop();
        long tList3 = sw.ElapsedMilliseconds;

        
        sw.Restart();
        HashSet<int> uniqueSet = new HashSet<int>(nums20k);
        sw.Stop();
        long tSet3 = sw.ElapsedMilliseconds;

        int[] setA = new int[5000];
        int[] setB = new int[5000];
        for (int i = 0; i < 5000; i++) { setA[i] = r.Next(1, 10000); setB[i] = r.Next(1, 10000); }

     
        sw.Restart();
        List<int> commonList = new List<int>();
        foreach (var a in setA)
        {
            foreach (var b in setB)
            {
                if (a == b)
                {
                    bool already = false;
                    foreach (var c in commonList) { if (c == a) { already = true; break; } }
                    if (!already) commonList.Add(a);
                    break;
                }
            }
        }
        sw.Stop();
        long tList4 = sw.ElapsedMilliseconds;

        
        sw.Restart();
        HashSet<int> hsA = new HashSet<int>(setA);
        HashSet<int> hsB = new HashSet<int>(setB);
        HashSet<int> commonSet = new HashSet<int>();
        foreach (var a in hsA)
        {
            if (hsB.Contains(a)) commonSet.Add(a);
        }
        sw.Stop();
        long tSet4 = sw.ElapsedMilliseconds;

        Console.WriteLine($"| Що міряємо | Список, мс | Словник/множина, мс | У скільки разів |");
        Console.WriteLine($"| :--- | :---: | :---: | :---: |");
        PrintRow("10k перевірок (100k елементів)", tList1, tSet1);
        PrintRow("Частоти 50k слів", tList2, tDict2);
        PrintRow("Дублікати з 20k чисел", tList3, tSet3);
        PrintRow("Спільні елементи (5k + 5k)", tList4, tSet4);
    }

    static void PrintRow(string name, long listMs, long dictMs)
    {
        double ratio = dictMs == 0 ? (listMs > 0 ? listMs : 1) : (double)listMs / dictMs;
        if (ratio < 1 && dictMs > 0) ratio = 1.0;
        Console.WriteLine($"| {name} | {listMs} | {dictMs} | {ratio:F1}x |");
    }

    static void DummyWarmup()
    {
        int sum = 0;
        for (int i = 0; i < 100000; i++) sum += i;
    }
}
