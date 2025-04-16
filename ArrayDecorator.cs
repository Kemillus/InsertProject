using System;
using System.Net.Sockets;

namespace InsertProject
{
    public class ArrayDecorator<T> where T : IComparable<T>
    {
        private T[] _array;
        private int _size;
        private long _operationCount;

        public long OperationCount => _operationCount;
        public int Size => _size;

        public ArrayDecorator(T[] array)
        {
            _array = array;
            _size = array.Length;
            _operationCount = 0;
        }

        public void AddOperations(long count = 1) => _operationCount += count;
        public void ResetCountOperations() => _operationCount = 0;

        public bool Contains_LinearSearch(T item)
        {
            for (int i = 0; i < _size; i++)
            {
                AddOperations();
                if (_array[i].CompareTo(item) == 0)
                    return true;
            }
            return false;
        }

        public bool Contains_BinarySearch(T item)
        {
            int left = 0, right = _size - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                AddOperations();
                int compare = _array[mid].CompareTo(item);
                AddOperations();

                if (compare == 0)
                    return true;
                else if (compare < 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            return false;
        }

        public void WrapFor(T[] another)
        {
            _array = another;
            _size = another.Length;
            _operationCount = 0;
        }

        public T[] GetSource()
        {
            return _array;
        }
    }
}
