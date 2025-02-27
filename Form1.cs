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

        private void btnRunTests_Click(object sender, EventArgs e)
        {
            TestArrayWithoutInitialCapacity();
            TestArrayWithInitialCapacity();
            TestLinkedList();
            SaveDataToCsv();
            DrawChart();
        }

        private void TestArrayWithoutInitialCapacity()
        {
            ArrayDecorator<int> list = new ArrayDecorator<int>();
            for (int i = 0; i < 30; i++)
            {
                list.Insert(0, i); // Вставка в начало
                _dataListWithoutInitialCapacity.Add(new DataPoint(i + 1, list.Size));
            }
        }

        private void TestArrayWithInitialCapacity()
        {
            ArrayDecorator<int> list = new ArrayDecorator<int>(10000);
            for (int i = 0; i < 15; i++)
            {
                list.Insert(0, i); // Вставка в начало
                _dataListWithInitialCapacity.Add(new DataPoint(i + 1, list.Size));
            }
        }

        private void TestLinkedList()
        {
            MyLinkedList<int> list = new MyLinkedList<int>();
            MyNode<int> node = null; // Ссылка на последний вставленный узел

            for (int i = 0; i < 30; i++)
            {
                if (i == 0)
                {
                    // Первый раз вставка по индексу
                    node = list.Insert(0, i);
                }
                else
                {
                    // Последующие вставки после предыдущего узла
                    node = list.InsertAfter(node, i); // Теперь метод возвращает MyNode<int>
                }

                // Сохраняем текущее состояние
                _dataLinkedList.Add(new DataPoint(i + 1, list._countOperations));
            }
        }

        private void SaveDataToCsv()
        {
            string filePath = "results.csv";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Type,N,Operations");
                foreach (var data in _dataListWithoutInitialCapacity)
                    writer.WriteLine($"ListNoCapacity,{data.N},{data.Operations}");
                foreach (var data in _dataListWithInitialCapacity)
                    writer.WriteLine($"ListWithCapacity,{data.N},{data.Operations}");
                foreach (var data in _dataLinkedList)
                    writer.WriteLine($"LinkedList,{data.N},{data.Operations}");
            }
        }

        private void DrawChart()
        {
            chart.Series.Clear();

            // Добавляем серию для списка без начальной емкости
            var series1 = new Series
            {
                Name = "ListNoCapacity",
                Color = Color.Red,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in _dataListWithoutInitialCapacity)
                series1.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(series1);

            // Добавляем точки к первой серии
            var pointSeries1 = new Series
            {
                Name = "ListNoCapacityPoints",
                Color = Color.Red,
                BorderColor = Color.Black,
                IsVisibleInLegend = false,
                ChartType = SeriesChartType.Point
            };
            foreach (var data in _dataListWithoutInitialCapacity)
                pointSeries1.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(pointSeries1);

            // Добавляем серию для списка с начальной емкостью
            var series2 = new Series
            {
                Name = "ListWithCapacity",
                Color = Color.Blue,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in _dataListWithInitialCapacity)
                series2.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(series2);

            // Добавляем точки ко второй серии
            var pointSeries2 = new Series
            {
                Name = "ListWithCapacityPoints",
                Color = Color.Blue,
                BorderColor = Color.Black,
                IsVisibleInLegend = false,
                ChartType = SeriesChartType.Point
            };
            foreach (var data in _dataListWithInitialCapacity)
                pointSeries2.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(pointSeries2);

            // Добавляем серию для односвязанного списка
            var series3 = new Series
            {
                Name = "LinkedList",
                Color = Color.Green,
                BorderColor = Color.Black,
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Line
            };
            foreach (var data in _dataLinkedList)
                series3.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(series3);

            // Добавляем точки к третьей серии
            var pointSeries3 = new Series
            {
                Name = "LinkedListPoints",
                Color = Color.Green,
                BorderColor = Color.Black,
                IsVisibleInLegend = false,
                ChartType = SeriesChartType.Point
            };
            foreach (var data in _dataLinkedList)
                pointSeries3.Points.AddXY(data.N, data.Operations);
            chart.Series.Add(pointSeries3);
        }
    }
}
