using System;
using System.Collections;

namespace InsertProject
{
    public class MyLinkedList<T> : IEnumerable
    {
        private MyLinkedList<T> _master;

        public int Count
        {
            get
            {
                return _counterNodes;
            }
        }

        public int _countOperations;
        public int _counterNodes;
        private MyNode<T> _head;
        private MyNode<T> _tail;
        private T _content;

        public IEnumerator GetEnumerator()
        {
            var current = _head;
            while (current != null)
            {
                yield return current.Content;
                current = current.Next;
            }
        }

        public void Add(T value)
        {
            var node = new MyNode<T>(this, value);
            _counterNodes++;

            if (_head == null)
            {
                _head = _tail = node;
            }
            else
            {
                _tail.Next = node;
            }
        }

        public MyNode<T> InsertAfter(MyNode<T> currentNode, T value)
        {
            if (currentNode == null)
                throw new ArgumentNullException(nameof(currentNode));

            var newNode = new MyNode<T>(this, value);
            var nodeAfter = currentNode.Next;
            currentNode.Next = newNode;
            newNode.Next = nodeAfter;

            if (newNode.Next == null)
                _tail = newNode;

            _countOperations++;
            _counterNodes++;

            return newNode;
        }

        public T Content
        {
            get
            {
                _master._countOperations++;
                return _content;
            }

            set
            {
                _master._countOperations++;
                _content = value;
            }
        }

        public MyNode<T> Insert(int index, T value)
        {
            if (index < 0 || index > Count)
                throw new ArgumentException("Index out of range");

            _countOperations++;

            if (index == 0)
            {
                var node = new MyNode<T>(this, value);
                node.Next = _head;
                _head = node;

                if (_tail == null)
                    _tail = node;

                _counterNodes++;
                return _head;
            }

            int i = 0;
            var current = _head;
            while (current != null && i < index - 1)
            {
                current = current.Next;
                i++;
                _countOperations++;
            }

            if (current == null)
                throw new ArgumentException("Index out of range");

            var newNode = new MyNode<T>(this, value);
            newNode.Next = current.Next;
            current.Next = newNode;

            if (newNode.Next == null)
                _tail = newNode;

            _counterNodes++;
            return newNode;
        }

        public MyLinkedList()
        {
            _countOperations = 0;
            _counterNodes = 0;
            _head = _tail = null;
        }
    }
}
