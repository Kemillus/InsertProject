using System;

namespace InsertProject
{
    public class ArrayDecorator<T>
    {
        private T[] _array;
        private int _size;
        public int WriteCount { get; private set; }
        public int ReadCount { get; private set; }

        private bool _isCountingEnabled = true; // Флаг для управления подсчетом операций

        public ArrayDecorator(int initialCapacity = 4)
        {
            _array = new T[initialCapacity];
            _size = 0;
        }

        public void EnableCounting()
        {
            _isCountingEnabled = true;
        }

        public void DisableCounting()
        {
            _isCountingEnabled = false;
        }

        public void ResetCountOperations()
        {
            WriteCount = 0;
            ReadCount = 0;
        }

        public void Insert(int index, T value)
        {
            if (index < 0 || index > _size)
                throw new IndexOutOfRangeException("Index out of range");

            if (_size == _array.Length)
                ResizeArray();

            for (int i = _size; i > index; i--)
            {
                _array[i] = _array[i - 1];
            }

            _array[index] = value;
            _size++;

            // Увеличиваем счетчик только если подсчет включен
            if (_isCountingEnabled)
            {
                WriteCount++;
            }
        }

        public void Swap(int index1, int index2)
        {
            if (index1 < 0 || index1 >= _size || index2 < 0 || index2 >= _size)
                throw new IndexOutOfRangeException("Index out of range");

            T temp = _array[index1];
            _array[index1] = _array[index2];
            _array[index2] = temp;

            // Увеличиваем счетчик только если подсчет включен
            if (_isCountingEnabled)
            {
                WriteCount += 3;
            }
        }

        public T Get(int index)
        {
            if (index < 0 || index >= _size)
                throw new IndexOutOfRangeException("Index out of range");

            // Увеличиваем счетчик только если подсчет включен
            if (_isCountingEnabled)
            {
                ReadCount++;
            }

            return _array[index];
        }

        public int Size => _size;

        private void ResizeArray()
        {
            int newSize = _array.Length * 2;
            T[] newArray = new T[newSize];
            Array.Copy(_array, newArray, _size);
            _array = newArray;
        }
    }
}
