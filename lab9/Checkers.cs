using System;
using System.Collections.Generic;

public static class Checkers
{
    public static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            if (cmp(a[i], a[i + 1]) > 0) return false;
        }
        return true;
    }

    public static bool IsPermutationOf<T>(T[] original, T[] sorted)
    {
        if (original.Length != sorted.Length) return false;
        Dictionary<T, int> counts = new Dictionary<T, int>();
        foreach (T item in original)
        {
            if (!counts.ContainsKey(item)) counts[item] = 0;
            counts[item]++;
        }
        foreach (T item in sorted)
        {
            if (!counts.ContainsKey(item)) return false;
            counts[item]--;
            if (counts[item] < 0) return false;
        }
        return true;
    }
}
