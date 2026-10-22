using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 16;
            int K = 1;
            int a = N % 3; // 1
            int b = N % 4; // 0
            int c = N % 5; // 1

            Console.WriteLine($"=== ЛАБОРАТОРНА РОБОТА №2 ===");
            Console.WriteLine($"Параметри: N = {N}, K = {K}, a = {a}, b = {b}, c = {c}\n");

    
            int[] array = GenerateArray(20, -9, 9, N);

            Console.Write("Початковий масив: ");
            PrintArray(array);
            Console.WriteLine("\n----------------------------------------\n");

           
            Console.WriteLine("--- Завдання 1: Агрегація ---");
            Task1_Aggregate(array);
            Console.WriteLine("\n----------------------------------------\n");

            
            Console.WriteLine("--- Завдання 2: Фільтрація (кратні 3) ---");
            int[] filtered = Task2_Filter(array, K);
            Console.Write("Відфільтрований масив: ");
            PrintArray(filtered);
            Console.WriteLine("\n----------------------------------------\n");

            Console.WriteLine("--- Завдання 3: Найдовша серія одинакових елементів ---");
            Task3_LongestSeries(array);
            Console.WriteLine("\n----------------------------------------\n");

            
            Console.WriteLine("--- Завдання 4: Циклічний зсув вліво ---");
            Console.Write("До:  ");
            PrintArray(array);
            Task4_ShiftLeft(array);
            Console.Write("Після: ");
            PrintArray(array);
            Console.WriteLine("\n----------------------------------------\n");

            Console.WriteLine("--- Завдання 5: Матриця 4x5 ---");
            int[,] matrix = GenerateMatrix(4, 5, -9, 9, N);
            Task5_Matrix(matrix);
        }

        
        static void PrintArray(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("[Масив порожній]");
                return;
            }
            Console.WriteLine("[" + string.Join(", ", arr) + "]");
        }

     
        static int[] GenerateArray(int size, int min, int max, int seed)
        {
            Random rnd = new Random(seed);
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = rnd.Next(min, max + 1);
            }
            return arr;
        }

        #region Завдання 1 · Агрегація
        static void Task1_Aggregate(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("Помилка: Масив порожній, обчислити характеристики неможливо.");
                return;
            }

            int sum = 0;
            int minVal = arr[0];
            int minIndex = 0;
            int maxVal = arr[0];
            int maxIndex = 0;
            int zeroCount = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                sum += arr[i];

                if (arr[i] < minVal)
                {
                    minVal = arr[i];
                    minIndex = i;
                }

                if (arr[i] > maxVal)
                {
                    maxVal = arr[i];
                    maxIndex = i;
                }

                if (arr[i] == 0)
                {
                    zeroCount++;
                }
            }

            double average = (double)sum / arr.Length;

            Console.WriteLine($"Сума: {sum}");
            Console.WriteLine($"Середнє: {average:F2}");
            Console.WriteLine($"Мінімум: {minVal} (індекс {minIndex})");
            Console.WriteLine($"Максимум: {maxVal} (індекс {maxIndex})");
            Console.WriteLine($"Кількість нулів: {zeroCount}");
        }
        #endregion

        #region Завдання 2 · Фільтрація
        static int[] Task2_Filter(int[] arr, int K)
        {
            if (arr == null || arr.Length == 0) return new int[0];

            int divisor = (K % 4) + 2; // 1 % 4 + 2 = 3

            
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % divisor == 0)
                {
                    count++;
                }
            }

            int[] result = new int[count];
            int index = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % divisor == 0)
                {
                    result[index++] = arr[i];
                }
            }

            return result;
        }
        #endregion

        #region Завдання 3 · Серії
        static void Task3_LongestSeries(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("Помилка: Масив порожній.");
                return;
            }

            int maxLen = 1;
            int maxVal = arr[0];
            int maxStart = 0;

            int currentLen = 1;
            int currentStart = 0;

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] == arr[i - 1])
                {
                    currentLen++;
                }
                else
                {
                    if (currentLen > maxLen)
                    {
                        maxLen = currentLen;
                        maxVal = arr[currentStart];
                        maxStart = currentStart;
                    }
                    currentLen = 1;
                    currentStart = i;
                }
            }

            
            if (currentLen > maxLen)
            {
                maxLen = currentLen;
                maxVal = arr[currentStart];
                maxStart = currentStart;
            }

            Console.WriteLine($"Значення: {maxVal}, Довжина серії: {maxLen}, Початковий індекс: {maxStart}");
        }
        #endregion

        #region Завдання 4 · Перестановка на місці
        static void Task4_ShiftLeft(int[] arr)
        {
            if (arr == null || arr.Length <= 1) return;

            int first = arr[0];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }
            arr[arr.Length - 1] = first;
        }
        #endregion

        #region Завдання 5 · Матриця
        static int[,] GenerateMatrix(int rows, int cols, int min, int max, int seed)
        {
            Random rnd = new Random(seed);
            int[,] mat = new int[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    mat[r, c] = rnd.Next(min, max + 1);
                }
            }
            return mat;
        }

        static void Task5_Matrix(int[,] mat)
        {
            int rows = mat.GetLength(0);
            int cols = mat.GetLength(1);

            int[] rowSums = new int[rows];
            int maxSumRowIndex = 0;
            int maxSum = int.MinValue;

         
            Console.WriteLine("Матриця:");
            for (int r = 0; r < rows; r++)
            {
                int rSum = 0;
                for (int c = 0; c < cols; c++)
                {
                    Console.Write($"{mat[r, c],4} ");
                    rSum += mat[r, c];
                }
                rowSums[r] = rSum;
                Console.WriteLine($" | Сума рядка: {rSum}");

                if (rSum > maxSum)
                {
                    maxSum = rSum;
                    maxSumRowIndex = r;
                }
            }

        
            Console.WriteLine(new string('-', 35));
            Console.Write("Макс стовпця: ");
            for (int c = 0; c < cols; c++)
            {
                int colMax = mat[0, c];
                for (int r = 1; r < rows; r++)
                {
                    if (mat[r, c] > colMax)
                    {
                        colMax = mat[r, c];
                    }
                }
                Console.Write($"{colMax,4} ");
            }
            Console.WriteLine();

            Console.WriteLine($"\nРядок з найбільшою сумою: Індекс {maxSumRowIndex} (Сума = {maxSum})");
        }
        #endregion
    }
}

