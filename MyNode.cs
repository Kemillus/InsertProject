namespace InsertProject
{
    public class MyNode<T>
    {
        private MyLinkedList<T> _master;
        private T _content;
        private MyNode<T> _next;

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
        public MyNode<T> Next
        {
            get
            {
                _master._countOperations++;
                return _next;
            }
            set
            {
                _master._countOperations++;
                _next = value;
            }
        }
        
        public MyNode(MyLinkedList<T> master, T value)
        {
            _master = master;
            Content = value;
            Next = null;
            _master._counterNodes++;
        }
    }
}