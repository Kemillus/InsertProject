using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Drawing;

namespace InsertProject
{
    public partial class MainForm : Form
    {
        private Button startButton;
        private Chart chart;
        private List<FileRecord> records;
        private ArrayDecorator<StringDecorator> filterDecorator;

        public MainForm()
        {
            this.Text = "Сравнение алгоритмов поиска";
            this.Size = new System.Drawing.Size(800, 600);

            startButton = new Button { Text = "Запустить анализ", Dock = DockStyle.Top };
            startButton.Click += StartAnalysis;

            chart = new Chart { Dock = DockStyle.Fill };
            chart.ChartAreas.Add(new ChartArea("MainArea"));
            chart.Legends.Add(new Legend("Поиск"));
            chart.Legends[0].Docking = Docking.Top;
            chart.Legends[0].Alignment = StringAlignment.Center;
            chart.Legends[0].Title = "Методы поиска";

            this.Controls.Add(chart);
            this.Controls.Add(startButton);
        }

        private void StartAnalysis(object sender, EventArgs e)
        {
            LoadData();

            var linearResults = SearchAnalysis.PerformSearch(records, filterDecorator, false);
            var binaryResults = SearchAnalysis.PerformSearch(records, filterDecorator, true);

            PlotResults(linearResults, binaryResults);
        }

        private void LoadData()
        {
            Console.WriteLine("Читаем данные...");

            records = SearchAnalysis.ReadRecords("records.xml")
                .Take(1000)
                .ToList();

            filterDecorator = new ArrayDecorator<StringDecorator>(new StringDecorator[0]);

            var filterList = SearchAnalysis.ReadFilterIds("filter.txt", filterDecorator)
                .Take(500)
                .ToList();

            filterDecorator.WrapFor(filterList.ToArray());
        }

        

        private void PlotResults(List<DataPoint> linearResults, List<DataPoint> binaryResults)
        {
            chart.Series.Clear();
            var seriesLinear = new Series("Линейный поиск") { ChartType = SeriesChartType.Line, Color = System.Drawing.Color.Red };
            var seriesBinary = new Series("Бинарный поиск") { ChartType = SeriesChartType.Line, Color = System.Drawing.Color.Blue };
            foreach (var point in linearResults)
                seriesLinear.Points.AddXY(point.N, point.Operations);

            foreach (var point in binaryResults)
                seriesBinary.Points.AddXY(point.N, point.Operations);

            chart.Series.Add(seriesLinear);
            chart.Series.Add(seriesBinary);
        }
    }
}
