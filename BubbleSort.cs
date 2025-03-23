using System;

namespace InsertProject
{
    public static class BubbleSort
    {
        public static int ClassicBubbleSort<T>(ArrayDecorator<T> array) where T : IComparable<T>
        {
            int operations = 0;
            int n = array.Size;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    operations++;
                    if (IsGreaterThan(array.Get(j), array.Get(j + 1)))
                    {
                        var temp = array.Get(j);
                        array.Insert(j, array.Get(j + 1));
                        array.Insert(j + 1, temp);
                        operations += 3;
                    }
                }
            }

            return operations;
        }

        public static int OptimizedBubbleSort<T>(ArrayDecorator<T> array) where T : IComparable<T>
        {
            int operations = 0;
            int n = array.Size;

            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;

                for (int j = 0; j < n - i - 1; j++)
                {
                    operations++;
                    if (IsGreaterThan(array.Get(j), array.Get(j + 1)))
                    {
                        var temp = array.Get(j);
                        array.Insert(j, array.Get(j + 1));
                        array.Insert(j + 1, temp);
                        swapped = true;
                        operations += 3;
                    }
                }

                if (!swapped)
                    break;
            }

            return operations;
        }

        public static int OptimizedForSortedBubbleSort<T>(ArrayDecorator<T> array) where T : IComparable<T>
        {
            int operations = 0;
            int n = array.Size;
            int lastSwapIndex = n - 1;

            while (lastSwapIndex > 0)
            {
                int newLastSwapIndex = 0;

                for (int j = 0; j < lastSwapIndex; j++)
                {
                    operations++;
                    if (IsGreaterThan(array.Get(j), array.Get(j + 1)))
                    {
                        var temp = array.Get(j);
                        array.Insert(j, array.Get(j + 1));
                        array.Insert(j + 1, temp);
                        newLastSwapIndex = j;
                        operations += 3;
                    }
                }

                lastSwapIndex = newLastSwapIndex;
            }

            return operations;
        }

        private static bool IsGreaterThan<T>(T x, T y) where T : IComparable<T>
        {
            return x.CompareTo(y) > 0;
        }
    }
}
