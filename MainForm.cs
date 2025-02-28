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
