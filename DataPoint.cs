namespace InsertProject
{
    public class DataPoint
    {
        public int N { get; set; }
        public int Operations { get; set; }

        public DataPoint(int n, int operations)
        {
            N = n;
            Operations = operations;
        }
    }
}
