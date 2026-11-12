using System;

public static class Sorts
{
    public static void InsertionSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        for (int i = 1; i < a.Length; i++)
        {
            T key = a[i];
            int j = i - 1;
            while (j >= 0)
            {
                comparisons++;
                if (cmp(a[j], key) > 0)
                {
                    a[j + 1] = a[j];
                    moves++;
                    j--;
                }
                else
                {
                    break;
                }
            }
            if (j + 1 != i)
            {
                a[j + 1] = key;
                moves++;
            }
        }
    }

    public static void MergeSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        if (a.Length <= 1) return;
        T[] buffer = new T[a.Length];
        MergeSortRec(a, buffer, 0, a.Length - 1, cmp, ref comparisons, ref moves);
    }

    private static void MergeSortRec<T>(T[] a, T[] buffer, int left, int right, Func<T, T, int> cmp, ref int comp, ref int mov)
    {
        if (left >= right) return;
        int mid = left + (right - left) / 2;
        MergeSortRec(a, buffer, left, mid, cmp, ref comp, ref mov);
        MergeSortRec(a, buffer, mid + 1, right, cmp, ref comp, ref mov);
        Merge(a, buffer, left, mid, right, cmp, ref comp, ref mov);
    }

    private static void Merge<T>(T[] a, T[] buffer, int left, int mid, int right, Func<T, T, int> cmp, ref int comp, ref int mov)
    {
        for (int k = left; k <= right; k++)
        {
            buffer[k] = a[k];
        }

        int i = left;
        int j = mid + 1;
        int dest = left;

        while (i <= mid && j <= right)
        {
            comp++;
            if (cmp(buffer[i], buffer[j]) <= 0)
            {
                a[dest++] = buffer[i++];
                mov++;
            }
            else
            {
                a[dest++] = buffer[j++];
                mov++;
            }
        }

        while (i <= mid)
        {
            a[dest++] = buffer[i++];
            mov++;
        }

        while (j <= right)
        {
            a[dest++] = buffer[j++];
            mov++;
        }
    }

    public static void BinaryInsertionSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        for (int i = 1; i < a.Length; i++)
        {
            T key = a[i];
            int lo = 0;
            int hi = i;

            while (lo < hi)
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                if (cmp(a[mid], key) <= 0)
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid;
                }
            }

            if (lo < i)
            {
                for (int j = i; j > lo; j--)
                {
                    a[j] = a[j - 1];
                    moves++;
                }
                a[lo] = key;
                moves++;
            }
        }
    }
}
