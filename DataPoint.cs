namespace InsertProject
{
    public class DataPoint
    {
        public double N { get; set; }
        public double Operations { get; set; }
        public DataPoint(int n, long operations)
        {
            N = n;
            Operations = operations;
        }
    }
}
