using System;
using System.Collections.Generic;

public static class Algorithms
{
    public static List<T> Filter<T>(List<T> src, Func<T, bool> keep)
    {
        List<T> result = new List<T>();
        foreach (T item in src)
        {
            if (keep(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public static List<TOut> Map<TIn, TOut>(List<TIn> src, Func<TIn, TOut> convert)
    {
        List<TOut> result = new List<TOut>();
        foreach (TIn item in src)
        {
            result.Add(convert(item));
        }
        return result;
    }

    public static TAcc Aggregate<T, TAcc>(List<T> src, TAcc start, Func<TAcc, T, TAcc> step)
    {
        TAcc acc = start;
        foreach (T item in src)
        {
            acc = step(acc, item);
        }
        return acc;
    }

    public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> src, Func<T, TKey> keyOf)
    {
        Dictionary<TKey, List<T>> groups = new Dictionary<TKey, List<T>>();
        foreach (T item in src)
        {
            TKey key = keyOf(item);
            if (!groups.ContainsKey(key))
            {
                groups[key] = new List<T>();
            }
            groups[key].Add(item);
        }
        return groups;
    }
}
