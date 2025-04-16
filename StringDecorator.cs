using System;

namespace InsertProject
{
    public class StringDecorator : IComparable<StringDecorator>
    {
        private string _source;
        private ArrayDecorator<StringDecorator> _arrayStorage;

        public override string ToString() => _source;
        public int Length => _source.Length;

        public char this[int index]
        {
            get
            {
                _arrayStorage.AddOperations();
                return _source[index];
            }
        }

        public int CompareTo(StringDecorator other)
        {
            if (other == null) return 1;

            int len = Math.Min(this.Length, other.Length);
            long comparisonOperations = 0;

            for (int i = 0; i < len; i++)
            {
                char char1 = this[i];
                char char2 = other[i];
                comparisonOperations++;

                int delta = char1.CompareTo(char2);
                if (delta != 0)
                {
                    _arrayStorage.AddOperations(comparisonOperations + 1);
                    return delta;
                }
            }

            comparisonOperations++;
            int lengthComparison = this.Length.CompareTo(other.Length);

            _arrayStorage.AddOperations(comparisonOperations);
            return lengthComparison;
        }

        public StringDecorator(string source, ArrayDecorator<StringDecorator> array)
        {
            _source = source;
            _arrayStorage = array;
        }
    }
}
