using System;

public static class BrokenSearch
{
   public static int Break1<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }
    

    public static int Break2<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;
        int safetyCounter = 0;

        while (lo <= hi)
        {
            safetyCounter++;
            if (safetyCounter > 1000) break;

            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            if (res < 0) lo = mid; 
            else hi = mid - 1;
        }
        return -1;
    }

    public static int Break3<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = (lo + hi) / 2; 
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }

    public static int Break4<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }
}
