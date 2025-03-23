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
            TestArrayWithoutInitialCapacity();
            TestArrayWithInitialCapacity();
            TestLinkedList();
            DrawChart();
        }

        private void btnRun_Sorting(object sender, EventArgs e)
        {
            TestSortingAlgorithms();
        }

        private void TestSortingAlgorithms()
        {
            Random random = new Random();
            List<DataPoint> classicData = new List<DataPoint>();
            List<DataPoint> optimizedData = new List<DataPoint>();
            List<DataPoint> sortedOptimizedData = new List<DataPoint>();

            for (int size = 5; size <= 100; size += 5)
            {
                // Создаём случайный массив
                ArrayDecorator<int> array = new ArrayDecorator<int>();
                for (int i = 0; i < size; i++)
                    array.Insert(i, random.Next(1, 100));

                // Классическая сортировка
                int classicOperations = BubbleSort.ClassicBubbleSort(array);
                classicData.Add(new DataPoint(size, classicOperations));

                // Оптимизированная сортировка
                array = new ArrayDecorator<int>();
                for (int i = 0; i < size; i++)
                    array.Insert(i, random.Next(1, 100));
                int optimizedOperations = BubbleSort.OptimizedBubbleSort(array);
                optimizedData.Add(new DataPoint(size, optimizedOperations));

                // Оптимизированная для упорядоченных массивов
                array = new ArrayDecorator<int>();
                for (int i = 0; i < size; i++)
                    array.Insert(i, random.Next(1, 100));
                int sortedOptimizedOperations = BubbleSort.OptimizedForSortedBubbleSort(array);
                sortedOptimizedData.Add(new DataPoint(size, sortedOptimizedOperations));
            }

            // Рисуем график
            DrawChart(classicData, optimizedData, sortedOptimizedData);
        }

        private void DrawChart(List<DataPoint> classicData, List<DataPoint> optimizedData, List<DataPoint> sortedOptimizedData)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var area = chart.ChartAreas.Add("Area");
            area.AxisX.Title = "Количество элементов (N)";
            area.AxisY.Title = "Количество операций";

            var seriesClassic = new Series
            {
                Name = "Классическая",
                ChartArea = "Area",
                Color = Color.Red,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in classicData)
                seriesClassic.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(seriesClassic);

            var seriesOptimized = new Series
            {
                Name = "Оптимизированная",
                ChartArea = "Area",
                Color = Color.Blue,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in optimizedData)
                seriesOptimized.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(seriesOptimized);

            var seriesSortedOptimized = new Series
            {
                Name = "Оптимизированная для упорядоченных",
                ChartArea = "Area",
                Color = Color.Green,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in sortedOptimizedData)
                seriesSortedOptimized.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(seriesSortedOptimized);

            chart.Titles.Add("Зависимость количества операций от числа элементов");
        }

        private void TestArrayWithoutInitialCapacity()
        {
            ArrayDecorator<int> list = new ArrayDecorator<int>();
            for (int i = 0; i < 15000; i++)
            {
                list.Insert(0, i); // Вставка в начало
                _dataListWithoutInitialCapacity.Add(new DataPoint(i + 1, list.Size));
            }
        }

        private void TestArrayWithInitialCapacity()
        {
            ArrayDecorator<int> list = new ArrayDecorator<int>(10000);
            for (int i = 0; i < 10000; i++)
            {
                list.Insert(0, i); // Вставка в начало
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
    }
}
