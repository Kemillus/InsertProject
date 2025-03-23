using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertProject
{
    public class MyStack<T>
    {
        private MyLinkedList<T> _list;

        public MyStack()
        {
            _list = new MyLinkedList<T>();
        }

        public void Push(T item)
        {
            _list.Add(item);
        }

        public T Pop()
        {
            if (_list.Count == 0)
                throw new InvalidOperationException("Stack underflow");

            return _list.RemoveLast();
        }

        public T Peek()
        {
            if (_list.Count == 0)
                throw new InvalidOperationException("Stack is empty");

            return _list.PeekLast();
        }

        public bool IsEmpty()
        {
            return _list.Count == 0;
        }

        public int Count()
        {
            return _list.Count;
        }
    }
}
