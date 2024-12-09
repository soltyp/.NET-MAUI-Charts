using MauiApp4;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using MauiShapes = Microsoft.Maui.Controls.Shapes;

namespace MauiApp4
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<TemperatureDataModel> TemperatureData { get; set; }

        public MainPage()
        {
            InitializeComponent();

            _databaseService = new DatabaseService();

            TemperatureData = new ObservableCollection<TemperatureDataModel>();
            //LoadTemperatureData();

            // Charts with db data
            BuildBarChart();
            BuildPieChart();


            //  Charts with sample data
            BuildSampleBarChart();
            BuildSamplePieChart();
        }

        //private void LoadTemperatureData()
        //{
        //    var data = _databaseService.GetTemperatureStatistics();

        //    TemperatureData.Clear();
        //    int maxCount = data.Values.Max();
        //    foreach (var item in data)
        //    {
        //        TemperatureData.Add(new TemperatureDataModel
        //        {
        //            Temperature = item.Key,
        //            Count = item.Value,
        //            NormalizedHeight = (item.Value / (double)maxCount) * 200 // Skalowanie słupków
        //        });
        //    }
        //}

        private void BuildBarChart()
        {
            BarChartGrid.Children.Clear();
            BarChartGrid.ColumnDefinitions.Clear();
            BarChartGrid.RowDefinitions.Clear();

            foreach (var _ in TemperatureData)
            {
                BarChartGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            }

            for (int i = 0; i < 2; i++)
            {
                BarChartGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            }

            for (int i = 0; i < TemperatureData.Count; i++)
            {
                var data = TemperatureData[i];

                var bar = new BoxView
                {
                    Color = GetRandomColorBarChart(), // Kolor losowy dla każdego słupka
                    HeightRequest = data.NormalizedHeight,
                    VerticalOptions = LayoutOptions.End
                };

                var labelTemperature = new Label
                {
                    Text = data.Temperature,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                var labelCount = new Label
                {
                    Text = data.Count.ToString(),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.End
                };

                BarChartGrid.Children.Add(bar);
                Grid.SetColumn(bar, i);
                Grid.SetRow(bar, 0);

                BarChartGrid.Children.Add(labelTemperature);
                Grid.SetColumn(labelTemperature, i);
                Grid.SetRow(labelTemperature, 1);

                BarChartGrid.Children.Add(labelCount);
                Grid.SetColumn(labelCount, i);
                Grid.SetRow(labelCount, 0);
            }
        }

        private Color GetRandomColorBarChart()
        {
            Random random = new Random();
            return Color.FromRgb(random.Next(0, 150), random.Next(0, 150), random.Next(0, 150));
        }



        private void BuildPieChart()
        {
            PieChartGrid.Children.Clear();

            double totalCount = TemperatureData.Sum(x => x.Count);
            double startAngle = 0;

            foreach (var data in TemperatureData)
            {
                double sliceAngle = (data.Count / totalCount) * 360;

                var pathFigure = new MauiShapes.PathFigure
                {
                    StartPoint = new Point(150, 150),
                };

                var arcPoint = GetArcPoint(150, 150, 100, startAngle + sliceAngle);

                pathFigure.Segments.Add(new MauiShapes.LineSegment { Point = GetArcPoint(150, 150, 100, startAngle) });
                pathFigure.Segments.Add(new MauiShapes.ArcSegment
                {
                    Point = arcPoint,
                    Size = new Size(100, 100),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = sliceAngle > 180
                });
                pathFigure.Segments.Add(new MauiShapes.LineSegment { Point = new Point(150, 150) });

                var pathGeometry = new MauiShapes.PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                var slicePath = new MauiShapes.Path
                {
                    Data = pathGeometry,
                    Fill = GetRandomColor(),
                    Stroke = Colors.Black,
                    StrokeThickness = 1
                };

                PieChartGrid.Children.Add(slicePath);

                double midAngle = startAngle + sliceAngle / 2;
                var labelPoint = GetArcPoint(150, 150, 120, midAngle);

                var label = new Label
                {
                    Text = $"{data.Temperature}: value: {data.Count}",
                    FontSize = 10,
                    TextColor = Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TranslationX = labelPoint.X - 150,
                    TranslationY = labelPoint.Y - 150
                };

                PieChartGrid.Children.Add(label);

                startAngle += sliceAngle;
            }
        }

        private Point GetArcPoint(double centerX, double centerY, double radius, double angle)
        {
            double radians = Math.PI * angle / 180.0;
            return new Point(centerX + radius * Math.Cos(radians), centerY + radius * Math.Sin(radians));
        }

        private Brush GetRandomColor()
        {
            Random random = new Random();
            return new SolidColorBrush(Color.FromRgb(random.Next(0, 150), random.Next(0, 150), random.Next(0, 150)));
        }

        private void BuildSamplePieChart()
        {
            // Przykładowe dane
            var sampleData = new List<TemperatureDataModel>
            {
                new TemperatureDataModel { Temperature = "36.1°C", Count = 10 },
                new TemperatureDataModel { Temperature = "36.5°C", Count = 15 },
                new TemperatureDataModel { Temperature = "37.0°C", Count = 20 }
            };

            var samplePieChartGrid = new Grid
            {
                WidthRequest = 300,
                HeightRequest = 300,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            double totalCount = sampleData.Sum(x => x.Count);
            double startAngle = 0;

            foreach (var data in sampleData)
            {
                // Proporcja kąta dla segmentu
                double sliceAngle = (data.Count / totalCount) * 360;

                // Tworzenie kształtu wycinka
                var pathFigure = new MauiShapes.PathFigure
                {
                    StartPoint = new Point(150, 150) // Środek koła
                };

                var arcPoint = GetArcPoint(150, 150, 100, startAngle + sliceAngle);

                pathFigure.Segments.Add(new MauiShapes.LineSegment { Point = GetArcPoint(150, 150, 100, startAngle) });
                pathFigure.Segments.Add(new MauiShapes.ArcSegment
                {
                    Point = arcPoint,
                    Size = new Size(100, 100), // Promień koła
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = sliceAngle > 180 // Czy wycinek ma być większy niż pół koła
                });
                pathFigure.Segments.Add(new MauiShapes.LineSegment { Point = new Point(150, 150) });

                var pathGeometry = new MauiShapes.PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                var slicePath = new MauiShapes.Path
                {
                    Data = pathGeometry,
                    Fill = GetRandomColor(), // Losowy kolor dla segmentu
                    Stroke = Colors.Black,
                    StrokeThickness = 1
                };

                samplePieChartGrid.Children.Add(slicePath);

                double midAngle = startAngle + sliceAngle / 2;
                var labelPoint = GetArcPoint(150, 150, 120, midAngle);

                var label = new Label
                {
                    Text = $"{data.Temperature}: value: {data.Count}",
                    FontSize = 10,
                    TextColor = Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TranslationX = labelPoint.X - 150,
                    TranslationY = labelPoint.Y - 150
                };

                samplePieChartGrid.Children.Add(label);

                startAngle += sliceAngle;
            }

            // Znajdź istniejący VerticalStackLayout wewnątrz ScrollView
            var scrollContent = (ScrollView)Content;
            var stackLayout = (VerticalStackLayout)scrollContent.Content;

            // Dodaj nową sekcję z przykładowym Pie Chart
            stackLayout.Children.Add(new Label
            {
                Text = "Sample Pie Chart",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center
            });

            stackLayout.Children.Add(samplePieChartGrid);
        }


        private void BuildSampleBarChart()
        {
            // Przykładowe dane
            var sampleData = new List<TemperatureDataModel>
    {
        new TemperatureDataModel { Temperature = "36.1°C", Count = 10, NormalizedHeight = (10 / 20.0) * 200 },
        new TemperatureDataModel { Temperature = "36.5°C", Count = 15, NormalizedHeight = (15 / 20.0) * 200 },
        new TemperatureDataModel { Temperature = "37.0°C", Count = 20, NormalizedHeight = (20 / 20.0) * 200 }
    };

            // Tworzenie nowej siatki na wykres
            var sampleBarChartGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 10,
                RowSpacing = 0
            };

            foreach (var _ in sampleData)
            {
                sampleBarChartGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            }

            for (int i = 0; i < 2; i++)
            {
                sampleBarChartGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            }

            for (int i = 0; i < sampleData.Count; i++)
            {
                var data = sampleData[i];

                var bar = new BoxView
                {
                    Color = GetRandomColorBarChart(),
                    HeightRequest = data.NormalizedHeight,
                    VerticalOptions = LayoutOptions.End
                };

                var labelTemperature = new Label
                {
                    Text = data.Temperature,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                var labelCount = new Label
                {
                    Text = data.Count.ToString(),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.End
                };

                sampleBarChartGrid.Children.Add(bar);
                Grid.SetColumn(bar, i);
                Grid.SetRow(bar, 0);

                sampleBarChartGrid.Children.Add(labelTemperature);
                Grid.SetColumn(labelTemperature, i);
                Grid.SetRow(labelTemperature, 1);

                sampleBarChartGrid.Children.Add(labelCount);
                Grid.SetColumn(labelCount, i);
                Grid.SetRow(labelCount, 0);
            }

            // Znajdź istniejący VerticalStackLayout wewnątrz ScrollView
            var scrollContent = (ScrollView)Content;
            var stackLayout = (VerticalStackLayout)scrollContent.Content;

            // Dodaj nową sekcję z przykładowym Bar Chart
            stackLayout.Children.Add(new Label
            {
                Text = "Sample Bar Chart",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center
            });

            stackLayout.Children.Add(sampleBarChartGrid);
        }
    }


    public class TemperatureDataModel
    {
        public string Temperature { get; set; }
        public int Count { get; set; }
        public double NormalizedHeight { get; set; }
    }
}
