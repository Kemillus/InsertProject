using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace InsertProject
{
    public partial class MainForm : Form
    {
        private List<DataPoint> _dataListWithoutInitialCapacity = new List<DataPoint>();
        private List<DataPoint> _dataListWithInitialCapacity = new List<DataPoint>();
        private List<DataPoint> _dataLinkedList = new List<DataPoint>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRun(object sender, EventArgs e)
        {
            TestAllSortingAlgorithms();
        }

        private void btnRun_Sorting(object sender, EventArgs e)
        {
            TestSortingAlgorithms(500);
        }

        private void btnRunShaker_Sorting(object sender, EventArgs e)
        {
            CollectSortingData(100);
        }

        public void CollectSortingData(int maxSize)
        {
            Random random = new Random();
            Dictionary<string, List<DataPoint>> results = new Dictionary<string, List<DataPoint>>
    {
        { "BubbleSort", new List<DataPoint>() },
        { "SelectionSort", new List<DataPoint>() },
        { "QuickSort", new List<DataPoint>() }
    };

            for (int size = 10; size <= maxSize; size += 10)
            {
                ArrayDecorator<int> array = new ArrayDecorator<int>(size);

                array.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    array.Insert(i, random.Next(1, 1000));
                }

                array.EnableCounting();
                BubbleSortsTest.BubbleSort(array);
                results["BubbleSort"].Add(new DataPoint(size, array.WriteCount + array.ReadCount));
                array.ResetCountOperations();

                array.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    array.Insert(i, random.Next(1, 1000));
                }

                array.EnableCounting();
                BubbleSortsTest.SelectionSort(array);
                results["SelectionSort"].Add(new DataPoint(size, array.WriteCount + array.ReadCount));
                array.ResetCountOperations();

                array.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    array.Insert(i, random.Next(1, 1000));
                }

                array.EnableCounting();
                BubbleSortsTest.QuickSort(array, 0, size - 1);
                results["QuickSort"].Add(new DataPoint(size, array.WriteCount + array.ReadCount));
                array.ResetCountOperations();
            }

            DrawChartForAllSortingAlgorithms(results);
        }

        private void TestSortingAlgorithms(int maxSize)
        {
            Random random = new Random();
            Dictionary<double, List<DataPoint>> resultsByScale = new Dictionary<double, List<DataPoint>>();

            double[] scales = { 1.2, 1.3, 1.4, 1.6, 1.8, 2.0 };

            foreach (double scale in scales)
            {
                List<DataPoint> results = new List<DataPoint>();

                for (int size = 10; size <= maxSize; size += 10)
                {
                    ArrayDecorator<int> arrayDecorator = new ArrayDecorator<int>(size);
                    arrayDecorator.DisableCounting();
                    for (int i = 0; i < size; i++)
                    {
                        arrayDecorator.Insert(i, random.Next(1, 100));
                    }
                    arrayDecorator.EnableCounting();
                    int operations = BubbleSortsTest.CombSort(arrayDecorator, scale);

                    results.Add(new DataPoint(size, operations));
                }

                resultsByScale[scale] = results;
            }

            DrawChart(resultsByScale);
        }

        private void TestAllSortingAlgorithms()
        {
            Random random = new Random();
            Dictionary<string, List<DataPoint>> resultsByAlgorithm = new Dictionary<string, List<DataPoint>>();

            resultsByAlgorithm["BubbleSort"] = new List<DataPoint>();
            resultsByAlgorithm["ShakerSort"] = new List<DataPoint>();
            resultsByAlgorithm["CombSort"] = new List<DataPoint>();

            for (int size = 10; size <= 100; size += 5)
            {
                ArrayDecorator<int> arrayDecorator = new ArrayDecorator<int>(size);

                arrayDecorator.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    arrayDecorator.Insert(i, random.Next(1, 100));
                }

                arrayDecorator.EnableCounting();
                int bubbleOperations = BubbleSortsTest.BubbleSort(arrayDecorator);
                resultsByAlgorithm["BubbleSort"].Add(new DataPoint(size, bubbleOperations));

                arrayDecorator = new ArrayDecorator<int>();
                arrayDecorator.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    arrayDecorator.Insert(i, random.Next(1, 100));
                }
                arrayDecorator.EnableCounting();
                int shakerOperations = BubbleSortsTest.ShakerSort(arrayDecorator);
                resultsByAlgorithm["ShakerSort"].Add(new DataPoint(size, shakerOperations));

                arrayDecorator = new ArrayDecorator<int>();
                arrayDecorator.DisableCounting();
                for (int i = 0; i < size; i++)
                {
                    arrayDecorator.Insert(i, random.Next(1, 100));
                }
                arrayDecorator.EnableCounting();
                int combOperations = BubbleSortsTest.CombSort(arrayDecorator, 1.3);
                resultsByAlgorithm["CombSort"].Add(new DataPoint(size, combOperations));
            }

            DrawChartForAllSortingAlgorithms(resultsByAlgorithm);
        }

        private void TestShakerSort()
        {
            Random random = new Random();
            List<DataPoint> results = new List<DataPoint>();

            for (int size = 10; size <= 100; size += 5)
            {
                ArrayDecorator<int> arrayDecorator = new ArrayDecorator<int>(size);

                arrayDecorator.DisableCounting();

                for (int i = 0; i < size; i++)
                {
                    arrayDecorator.Insert(i, random.Next(1, 100));
                }

                arrayDecorator.EnableCounting();

                int operations = BubbleSortsTest.ShakerSort(arrayDecorator);

                results.Add(new DataPoint(size, operations));
            }

            DrawChartForShakerSort(results);
        }

        private void TestArrayWithoutInitialCapacity()
        {
            ArrayDecorator<int> list = new ArrayDecorator<int>();
            for (int i = 0; i < 15000; i++)
            {
                list.Insert(0, i);
                _dataListWithoutInitialCapacity.Add(new DataPoint(i + 1, list.Size));
            }
        }

        private void DrawChart(Dictionary<double, List<DataPoint>> dataPointsByScale)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var area = chart.ChartAreas.Add("Area");
            area.AxisX.Title = "Array Size (N)";
            area.AxisY.Title = "Number of Operations";

            foreach (var scale in dataPointsByScale.Keys)
            {
                Series series = new Series($"Scale {scale}")
                {
                    ChartType = SeriesChartType.Line
                };

                foreach (var point in dataPointsByScale[scale])
                {
                    series.Points.AddXY(point.N, point.Operations);
                }

                chart.Series.Add(series);
            }
        }

        private void TestArrayWithInitialCapacity()
        {
            ArrayDecorator<int> list = new ArrayDecorator<int>(10000);
            for (int i = 0; i < 10000; i++)
            {
                list.Insert(0, i);
                _dataListWithInitialCapacity.Add(new DataPoint(i + 1, list.Size));
            }
        }

        private void TestLinkedList()
        {
            MyLinkedList<int> list = new MyLinkedList<int>();
            MyNode<int> node = null;

            for (int i = 0; i < 15000; i++)
            {
                if (i == 0)
                {
                    node = list.Insert(0, i);
                }

                else
                {
                    node = list.InsertAfter(node, i);
                }

                _dataLinkedList.Add(new DataPoint(i + 1, list._countOperations));
            }
        }

        private void SaveDataToCsv(object sender, EventArgs e)
        {
            string filePath = "results.csv";

            if (File.Exists(filePath))
            {
                var result = MessageBox.Show(
                    "Файл уже существует. Хотите заменить его?",
                    "Замена файла",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    MessageBox.Show("Операция записи отменена.", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Type;N;Operations");

                    foreach (var data in _dataListWithoutInitialCapacity)
                        writer.WriteLine($"List No Capacity;{data.N};{data.Operations}");

                    foreach (var data in _dataListWithInitialCapacity)
                        writer.WriteLine($"List With Capacity;{data.N};{data.Operations}");

                    foreach (var data in _dataLinkedList)
                        writer.WriteLine($"Linked List;{data.N};{data.Operations}");
                }

                MessageBox.Show("Данные успешно сохранены в файл results.csv.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Не удалось записать данные в файл. Возможно, файл уже открыт в другой программе. Пожалуйста, закройте файл и повторите попытку.",
                    "Ошибка доступа к файлу",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DrawChart()
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var area1 = chart.ChartAreas.Add("Area1");
            area1.AxisX.Title = "N";
            area1.AxisY.Title = "Operations";

            var area2 = chart.ChartAreas.Add("Area2");
            area2.AxisX.Title = "N";
            area2.AxisY.Title = "Operations";

            var area3 = chart.ChartAreas.Add("Area3");
            area3.AxisX.Title = "N";
            area3.AxisY.Title = "Operations";

            var series1 = new Series
            {
                Name = "List No Capacity",
                ChartArea = "Area1",
                Color = Color.Red,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in _dataListWithoutInitialCapacity)
                series1.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(series1);

            var series2 = new Series
            {
                Name = "List With Capacity",
                ChartArea = "Area2",
                Color = Color.Blue,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in _dataListWithInitialCapacity)
                series2.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(series2);

            var series3 = new Series
            {
                Name = "Linked List",
                ChartArea = "Area3",
                Color = Color.Green,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in _dataLinkedList)
                series3.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(series3);

            chart.Titles.Add("Зависимость количества операций от числа элементов");
            chart.ChartAreas[0].AxisX.Title = "Количество элементов (N)";
            chart.ChartAreas[0].AxisY.Title = "Количество операций";
        }

        private void DrawChartForShakerSort(List<DataPoint> dataPoints)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var area = chart.ChartAreas.Add("Area");
            area.AxisX.Title = "Array Size (N)";
            area.AxisY.Title = "Number of Operations";

            Series series = new Series("ShakerSort")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Orange,
                BorderColor = Color.Black,
                IsVisibleInLegend = true
            };

            foreach (var point in dataPoints)
            {
                series.Points.AddXY(point.N, point.Operations);
            }

            chart.Series.Add(series);
        }

        private void DrawChartForAllSortingAlgorithms(Dictionary<string, List<DataPoint>> dataPointsByAlgorithm)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var area = chart.ChartAreas.Add("Area");
            area.AxisX.Title = "Array Size (N)";
            area.AxisY.Title = "Number of Operations";

            foreach (var algorithm in dataPointsByAlgorithm.Keys)
            {
                Series series = new Series(algorithm)
                {
                    ChartType = SeriesChartType.Line
                };

                foreach (var point in dataPointsByAlgorithm[algorithm])
                {
                    series.Points.AddXY(point.N, point.Operations);
                }

                chart.Series.Add(series);
            }

            chart.Titles.Add("Зависимость количества операций от числа элементов");
        }

    }
}
