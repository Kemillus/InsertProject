using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InsertProject
{
    public static class SearchAnalysis
    {
        public static List<FileRecord> ReadRecords(string filename)
        {
            var xs = new XmlSerializer(typeof(List<FileRecord>));
            var fs = new FileStream(filename, FileMode.Open);
            return (List<FileRecord>)xs.Deserialize(fs);
        }

        public static List<StringDecorator> ReadFilterIds(string filename, ArrayDecorator<StringDecorator> arrayDecorator)
        {
            return File.ReadLines(filename)
                .Select(x => new StringDecorator(x, arrayDecorator))
                .ToList();
        }

        public static List<DataPoint> PerformSearch(List<FileRecord> records, ArrayDecorator<StringDecorator> filterDecorator, bool useBinarySearch)
        {
            if (useBinarySearch)
                filterDecorator.WrapFor(filterDecorator.GetSource().OrderBy(x => x).ToArray());

            var results = new List<DataPoint>();
            int[] percentages = { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 1 };

            foreach (var percent in percentages)
            {
                filterDecorator.ResetCountOperations();
                int size = records.Count * percent / 100;
                var subset = records.Take(size).ToList();

                var filteredRecords = subset.Where(r =>
                    useBinarySearch ? filterDecorator.Contains_BinarySearch(new StringDecorator(r.Id, filterDecorator))
                                   : filterDecorator.Contains_LinearSearch(new StringDecorator(r.Id, filterDecorator))
                ).ToList();

                results.Add(new DataPoint(size, filterDecorator.OperationCount));
                Console.WriteLine($"Size: {size}, Operations: {filterDecorator.OperationCount}");
            }

            return results;
        }
    }
}
