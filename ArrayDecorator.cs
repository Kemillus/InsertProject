using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertProject
{
    public class ArrayDecorator<T>
    {
        private T[] _array;
        private int _size;

        public ArrayDecorator(int initialCapacity = 4)
        {
            _array = new T[initialCapacity];
            _size = 0;
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
        }

        public T Get(int index)
        {
            if (index < 0 || index >= _size)
                throw new IndexOutOfRangeException("Index out of range");

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
