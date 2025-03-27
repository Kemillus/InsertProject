using System;

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

        private static bool IsGreaterThan<T>(T x, T y) where T : IComparable<T>
        {
            return x.CompareTo(y) > 0;
        }
    }
}