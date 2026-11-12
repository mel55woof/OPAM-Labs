using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== ЗАВДАННЯ 1: Збалансованість дужок ===");
        string[] testStrings = new string[]
        {
            "(a[b]{c})",
            "([)]",
            ")(",
            "((",
            "",
            "(<[{}>"
        };

        foreach (var s in testStrings)
        {
            int errIdx = CheckBrackets(s);
            string status = errIdx == -1 ? "збалансований" : $"незбалансований (помилка на індексі {errIdx})";
            Console.WriteLine($"\"{s}\" -> {status}");
        }

        Console.WriteLine("\n=== ЗАВДАННЯ 2: Зворотний польський запис ===");
        string[] rpnTests = new string[]
        {
            "3 4 + 2 *",
            "5 3 -",
            "8 2 /",
            "2 3 4 * +",
            "5 0 /",
            "2 3 + 4 * 19 -"
        };

        foreach (var expr in rpnTests)
        {
            Console.Write($"\"{expr}\" -> ");
            EvaluateRPN(expr);
        }

        Console.WriteLine("\n=== ЗАВДАННЯ 3: Модель обслуговування (Схема c=1) ===");
        SimulateQueue();

        Console.WriteLine("\n=== ЗАВДАННЯ 4: Власний стек IntStack ===");
        TestCustomStack();

        Console.WriteLine("\n--- Перевірка Завдання 1 на власному CustomCharStack ---");
        foreach (var s in testStrings)
        {
            int errIdx = CheckBracketsCustom(s);
            string status = errIdx == -1 ? "збалансований" : $"незбалансований (індекс {errIdx})";
            Console.WriteLine($"\"{s}\" -> {status}");
        }
    }

    static int CheckBrackets(string input)
    {
        Stack<char> stack = new Stack<char>();
        Stack<int> indexStack = new Stack<int>();

        for (int i = 0; i < input.Length; i++)
        {
            char ch = input[i];
            if (ch == '(' || ch == '[' || ch == '{' || ch == '<')
            {
                stack.Push(ch);
                indexStack.Push(i);
            }
            else if (ch == ')' || ch == ']' || ch == '}' || ch == '>')
            {
                if (stack.Count == 0) return i;

                char top = stack.Pop();
                indexStack.Pop();
                if (!IsMatching(top, ch)) return i;
            }
        }

        if (stack.Count > 0)
        {
            return indexStack.Peek();
        }

        return -1;
    }

    static bool IsMatching(char open, char close)
    {
        return (open == '(' && close == ')') ||
               (open == '[' && close == ']') ||
               (open == '{' && close == '}') ||
               (open == '<' && close == '>');
    }

    static void EvaluateRPN(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            Console.WriteLine("Помилка: порожній вираз");
            return;
        }

        string[] tokens = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Stack<double> stack = new Stack<double>();

        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double val))
            {
                stack.Push(val);
            }
            else if (token == "+" || token == "-" || token == "*" || token == "/")
            {
                if (stack.Count < 2)
                {
                    Console.WriteLine("Помилка: забракло чисел для операції");
                    return;
                }

                double b = stack.Pop();
                double a = stack.Pop();

                if (token == "/" && b == 0)
                {
                    Console.WriteLine("Помилка: ділення на нуль");
                    return;
                }

                double res = token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    _ => 0
                };
                stack.Push(res);
            }
            else
            {
                Console.WriteLine($"Помилка: невідомий токен '{token}'");
                return;
            }
        }

        if (stack.Count == 1)
        {
            Console.WriteLine($"Результат = {stack.Pop()}");
        }
        else
        {
            Console.WriteLine("Помилка: забагато чисел у виразі");
        }
    }

    struct Request
    {
        public int Id;
        public int WorkUnits;
        public int ArrivalTime;
    }

    static void SimulateQueue()
    {
        int count = 8 + 1 % 5;
        Random rnd = new Random(16);

        Queue<Request> queue = new Queue<Request>();
        int currentArrival = 0;

        for (int i = 1; i <= count; i++)
        {
            currentArrival += rnd.Next(0, 4);
            int work = rnd.Next(1, 10);
            queue.Enqueue(new Request { Id = i, WorkUnits = work, ArrivalTime = currentArrival });
        }

        int currentTime = 0;
        int idleTime = 0;
        int maxQueueLength = 0;

        Console.WriteLine("№\tВага\tЧас приходу\tПочаток\tЗавершення");
        while (queue.Count > 0)
        {
            if (queue.Count > maxQueueLength) maxQueueLength = queue.Count;

            Request req = queue.Dequeue();

            if (currentTime < req.ArrivalTime)
            {
                idleTime += (req.ArrivalTime - currentTime);
                currentTime = req.ArrivalTime;
            }

            int startTime = currentTime;
            int endTime = startTime + req.WorkUnits;
            currentTime = endTime;

            Console.WriteLine($"{req.Id}\t{req.WorkUnits}\t{req.ArrivalTime}\t\t{startTime}\t{endTime}");
        }

        Console.WriteLine($"Загальний час простою обслуговувача: {idleTime} хв.");
        Console.WriteLine($"Максимальна довжина черги: {maxQueueLength}");
    }

    class IntStack
    {
        private int[] items;
        private int count;

        public int Count => count;
        public int Capacity => items.Length;

        public IntStack()
        {
            items = new int[3];
            count = 0;
        }

        public void Push(int x)
        {
            if (count == items.Length)
            {
                Array.Resize(ref items, items.Length * 2);
            }
            items[count++] = x;
        }

        public int Pop()
        {
            if (count == 0) throw new InvalidOperationException("Стек порожній!");
            return items[--count];
        }

        public int Peek()
        {
            if (count == 0) throw new InvalidOperationException("Стек порожній!");
            return items[count - 1];
        }
    }

    static void TestCustomStack()
    {
        IntStack stack = new IntStack();
        Console.WriteLine($"Початковий стек: Count={stack.Count}, Capacity={stack.Capacity}");

        for (int i = 1; i <= 4; i++)
        {
            stack.Push(i * 10);
            Console.WriteLine($"Push({i * 10}) -> Count={stack.Count}, Capacity={stack.Capacity}");
        }
    }

    class CustomCharStack
    {
        private char[] items = new char[3];
        private int count = 0;

        public int Count => count;

        public void Push(char x)
        {
            if (count == items.Length) Array.Resize(ref items, items.Length * 2);
            items[count++] = x;
        }

        public char Pop()
        {
            if (count == 0) throw new InvalidOperationException();
            return items[--count];
        }

        public char Peek()
        {
            if (count == 0) throw new InvalidOperationException();
            return items[count - 1];
        }
    }

    static int CheckBracketsCustom(string input)
    {
        CustomCharStack stack = new CustomCharStack();
        CustomCharStack indexStack = new CustomCharStack();

        for (int i = 0; i < input.Length; i++)
        {
            char ch = input[i];
            if (ch == '(' || ch == '[' || ch == '{' || ch == '<')
            {
                stack.Push(ch);
                indexStack.Push((char)i);
            }
            else if (ch == ')' || ch == ']' || ch == '}' || ch == '>')
            {
                if (stack.Count == 0) return i;
                char top = stack.Pop();
                indexStack.Pop();
                if (!IsMatching(top, ch)) return i;
            }
        }

        if (stack.Count > 0) return (int)indexStack.Peek();
        return -1;
    }
}
