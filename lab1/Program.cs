using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введіть ваш номер у списку (N): ");
            if (!int.TryParse(Console.ReadLine(), out int N)) N = 16;

            Console.Write("Введіть першу літеру вашого імені: ");
            string nameInput = Console.ReadLine()?.Trim().ToUpper();
            int K = 1;
            if (!string.IsNullOrEmpty(nameInput))
            {
                string ukrAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
                int index = ukrAlphabet.IndexOf(nameInput[0]);
                if (index != -1) K = index + 1;
            }

            int a = N % 3;
            int b = N % 4;
            int c = N % 5;

            Console.WriteLine($"\nОбчислені параметри: N = {N}, K = {K}, a = {a}, b = {b}, c = {c}\n");

            while (true)
            {
                Console.WriteLine("=== МЕНЮ ЛАБОРАТОРНОЇ РОБОТИ №1 ===");
                Console.WriteLine("1 — Завдання 1: Перевірка дати, пора року та день у році");
                Console.WriteLine("2 — Завдання 2: 15 випадкових чисел, сума кратних K, сума всіх, кількість нулів");
                Console.WriteLine("3 — Завдання 3: Крайові випадки");
                Console.WriteLine("4 — Завдання 4: НСД (Два алгоритми)");
                Console.WriteLine("0 — Вихід");
                Console.Write("Оберіть пункт: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Помилка! Введіть число від 0 до 4.\n");
                    continue;
                }

                Console.WriteLine();
                switch (choice)
                {
                    case 1:
                        Task1();
                        break;
                    case 2:
                        Task2(N, K);
                        break;
                    case 3:
                        Task3();
                        break;
                    case 4:
                        Task4(N);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Невірний вибір!");
                        break;
                }
                Console.WriteLine("\n----------------------------------------\n");
            }
        }

        #region Завдання 1
        static void Task1()
        {
            Console.WriteLine("--- Завдання 1 ---");
            int day = ReadInt("Введіть день: ");
            int month = ReadInt("Введіть місяць: ");
            int year = ReadInt("Введіть рік: ");

            if (year < 1 || month < 1 || month > 12 || day < 1)
            {
                Console.WriteLine("Результат: Дата некоректна!");
                return;
            }

            bool isLeap = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);

            int daysInMonth;
            switch (month)
            {
                case 2:
                    daysInMonth = isLeap ? 29 : 28;
                    break;
                case 4: case 6: case 9: case 11:
                    daysInMonth = 30;
                    break;
                default:
                    daysInMonth = 31;
                    break;
            }

            if (day > daysInMonth)
            {
                Console.WriteLine($"Результат: Дата некоректна (у місяці {month} лише {daysInMonth} днів)!");
                return;
            }

            Console.WriteLine("Результат: Дата коректна!");

            string season = (month == 12 || month == 1 || month == 2) ? "Зима" :
                           (month >= 3 && month <= 5) ? "Весна" :
                           (month >= 6 && month <= 8) ? "Літо" : "Осінь";

            int dayOfYear = day;
            for (int m = 1; m < month; m++)
            {
                if (m == 2) dayOfYear += isLeap ? 29 : 28;
                else if (m == 4 || m == 6 || m == 9 || m == 11) dayOfYear += 30;
                else dayOfYear += 31;
            }

            Console.WriteLine($"Пора року: {season}");
            Console.WriteLine($"Номер дня від початку року: {dayOfYear}");
        }
        #endregion

        #region Завдання 2
        static void Task2(int N, int K)
        {
            Console.WriteLine("--- Завдання 2 ---");
            Console.WriteLine($"Генерація 15 чисел через new Random({N})...");

            Random rnd = new Random(N);

            int totalSum = 0;
            int zeroCount = 0;
            int specialSum = 0;

            Console.Write("Послідовність: ");
            for (int i = 0; i < 15; i++)
            {
                int val = rnd.Next(-50, 51);
                Console.Write(val + " ");

                totalSum += val;
                if (val == 0) zeroCount++;

                if (K != 0 && val % K == 0)
                {
                    specialSum += val;
                }
            }
            Console.WriteLine();

            Console.WriteLine($"\n[Спільна частина]");
            Console.WriteLine($"Сума всіх елементів: {totalSum}");
            Console.WriteLine($"Кількість нулів: {zeroCount}");

            Console.WriteLine($"\n[Індивідуальна частина]");
            Console.WriteLine($"Сума елементів, кратних K ({K}): {specialSum}");
        }
        #endregion

        #region Завдання 3
        static void Task3()
        {
            Console.WriteLine("--- Завдання 3 ---");
            Console.WriteLine("Аналіз крайових випадків наведено у звіті.");
        }
        #endregion

        #region Завдання 4
        static void Task4(int N)
        {
            Console.WriteLine("--- Завдання 4 ---");
            int A = 1000000 + N;
            int B = 999999 - N;

            Console.WriteLine($"Обчислення НСД для A = {A} та B = {B}");

            int iterBrute = 0;
            int gcdBrute = 1;
            int minVal = A < B ? A : B;

            for (int d = minVal; d >= 1; d--)
            {
                iterBrute++;
                if (A % d == 0 && B % d == 0)
                {
                    gcdBrute = d;
                    break;
                }
            }

            Console.WriteLine($"\n1. Перебір дільників: НСД = {gcdBrute}, Ітерацій = {iterBrute}");

            int iterEuclid = 0;
            int tempA = A;
            int tempB = B;

            while (tempB != 0)
            {
                iterEuclid++;
                int rem = tempA % tempB;
                tempA = tempB;
                tempB = rem;
            }
            int gcdEuclid = tempA;

            Console.WriteLine($"2. Алгоритм Евкліда: НСД = {gcdEuclid}, Ітерацій = {iterEuclid}");
            
            double times = (double)iterBrute / iterEuclid;
            Console.WriteLine($"\nВисновок: Алгоритм Евкліда ефективніший приблизно в {times:N0} разів!");
        }
        #endregion

        #region Допоміжний метод
        static int ReadInt(string prompt)
        {
            int val;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out val)) return val;
                Console.WriteLine("Помилка! Введіть ціле число.");
            }
        }
        #endregion
    }
}
