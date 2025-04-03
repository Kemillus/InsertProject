using System;
using System.Collections.Generic;

namespace InsertProject
{
    static class BubbleSortsTest
    {
        public static int ShakerSort<T>(ArrayDecorator<T> array) where T : IComparable<T>
        {
            int leftEdge = 0;
            int rightEdge = array.Size - 1;
            while (leftEdge <= rightEdge)
            {
                for (int i = rightEdge; i > leftEdge; i--)
                {
                    var t1 = array.Get(i - 1);
                    var t2 = array.Get(i);
                    if (IsGreaterThan(t1, t2))
                    {
                        array.Swap(i - 1, i);
                    }
                }

                leftEdge++;
                for (int i = leftEdge; i < rightEdge; i++)
                {
                    var t1 = array.Get(i);
                    var t2 = array.Get(i + 1);
                    if (IsGreaterThan(t1, t2))
                    {
                        array.Swap(i + 1, i);
                    }
                }
                rightEdge--;
            }
            return array.ReadCount + array.WriteCount;
        }

        public static int CombSort<T>(ArrayDecorator<T> arrayDecorator, double scale) where T : IComparable<T>
        {
            int delta = arrayDecorator.Size - 1;
            while (delta >= 1)
            {
                for (int i = 0; i + delta < arrayDecorator.Size; i++)
                {
                    var value1 = arrayDecorator.Get(i);
                    var value2 = arrayDecorator.Get(i + delta);

                    if (IsGreaterThan(value1, value2))
                    {
                        arrayDecorator.Swap(i, i + delta);
                    }
                }
                delta = (int)(delta / scale);
            }

            return arrayDecorator.ReadCount + arrayDecorator.WriteCount;
        }

        public static int BubbleSort<T>(ArrayDecorator<T> array) where T : IComparable<T>
        {
            int size = array.Size;

            for (int i = 0; i < size - 1; i++)
            {

                for (int j = 0; j < size - i - 1; j++)
                {
                    var t1 = array.Get(j);
                    var t2 = array.Get(j + 1);

                    if (IsGreaterThan(t1, t2))
                    {
                        array.Swap(j, j + 1);
                    }
                }
            }

            return array.ReadCount + array.WriteCount;
        }

        public static void SelectionSort<T>(ArrayDecorator<T> array) where T : IComparable<T>
        {
            for (int i = 0; i < array.Size - 1; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < array.Size; j++)
                {
                    if (array.Get(j).CompareTo(array.Get(minIndex)) < 0)
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    array.Swap(i, minIndex);
                }
            }
        }

        public static void QuickSort<T>(ArrayDecorator<T> array, int left, int right) where T : IComparable<T>
        {
            if (left >= right) return;

            int pivotIndex = Partition(array, left, right);

            QuickSort(array, left, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, right);
        }

        private static int Partition<T>(ArrayDecorator<T> array, int left, int right) where T : IComparable<T>
        {
            T pivot = array.Get(right);
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (array.Get(j).CompareTo(pivot) <= 0)
                {
                    i++;
                    array.Swap(i, j);
                }
            }

            array.Swap(i + 1, right);
            return i + 1;
        }

        private static bool IsGreaterThan<T>(T x, T y) where T : IComparable<T>
        {
            return x.CompareTo(y) > 0;
        }

        public static Dictionary<string, List<DataPoint>> CollectSortingData(int maxSize)
        {
            Random random = new Random();
            Dictionary<string, List<DataPoint>> results = new Dictionary<string, List<DataPoint>>
    {
        { "BubbleSort", new List<DataPoint>() },
        { "SelectionSort", new List<DataPoint>() },
        { "QuickSort", new List<DataPoint>() }
    };

            for (int size = 10; size <= maxSize; size += 10)
            {
                ArrayDecorator<int> array = new ArrayDecorator<int>(size);

                array.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    array.Insert(i, random.Next(1, 1000));
                }

                array.EnableCounting();
                BubbleSortsTest.BubbleSort(array);
                results["BubbleSort"].Add(new DataPoint(size, array.WriteCount + array.ReadCount));
                array.ResetCountOperations();

                array.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    array.Insert(i, random.Next(1, 1000));
                }

                array.EnableCounting();
                BubbleSortsTest.SelectionSort(array);
                results["SelectionSort"].Add(new DataPoint(size, array.WriteCount + array.ReadCount));
                array.ResetCountOperations();

                array.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    array.Insert(i, random.Next(1, 1000));
                }

                array.EnableCounting();
                BubbleSortsTest.QuickSort(array, 0, size - 1);
                results["QuickSort"].Add(new DataPoint(size, array.WriteCount + array.ReadCount));
                array.ResetCountOperations();
            }

            return results;
        }
    }
}