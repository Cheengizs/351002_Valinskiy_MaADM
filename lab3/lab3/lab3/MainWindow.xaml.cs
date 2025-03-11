using OxyPlot;
using OxyPlot.Series;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot.Wpf;
using System.Collections.Immutable;
using OxyPlot.Axes;
using OxyPlot.Annotations;


namespace lab3
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int obrAmount = 10000;
        public double _pc1 { get; set; }
        public double _pc2 { get; set; }

        public Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            sldrPropOne.ValueChanged += sldrPropOne_OnValueChanged;
            sldrPropTwo.ValueChanged += sldrPropTwo_OnValueChanged;
        }

        public void sldrPropOne_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldrPropTwo.Value = 1 - ((Slider)sender).Value;
            ReDraw();
        }

        public void sldrPropTwo_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldrPropOne.Value = 1 - ((Slider)sender).Value;
        }

        public void ReDraw()
        {
            _pc1 = sldrPropOne.Value;
            _pc2 = sldrPropTwo.Value;
            myGrafik.Model = getTwoGraph();
        }

        public PlotModel getTwoGraph()
        {
            double[] group1 = new double[obrAmount];
            double[] group2 = new double[obrAmount];
            Random _random = new Random();

            for (int i = 0; i < obrAmount; i++)
            {
                group1[i] = _random.Next(-200, 500);
                group2[i] = _random.Next(0, 700);
            }
            Array.Sort(group1);
            Array.Sort(group2);

            double mu1 = group1.Average();
            double mu2 = group2.Average();
            double stdDev1 = Math.Sqrt(group1.Sum(x => Math.Pow(x - mu1, 2)) / obrAmount);
            double stdDev2 = Math.Sqrt(group2.Sum(x => Math.Pow(x - mu2, 2)) / obrAmount);

            PlotModel graph = new PlotModel();

            graph.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 500 });
            graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0 });

            LineSeries series1 = new LineSeries() { Color = OxyColors.Blue };
            LineSeries series2 = new LineSeries() { Color = OxyColors.Red };

            foreach (double x in group1)
                series1.Points.Add(new DataPoint(x, Gaussian(x, mu1, stdDev1) * _pc1));

            foreach (double x in group2)
                series2.Points.Add(new DataPoint(x, Gaussian(x, mu2, stdDev2) * _pc2));

            graph.Series.Add(series1);
            graph.Series.Add(series2);

            double currDot = -200;
            while (currDot < 700)
            {
                if (Gaussian(currDot, mu1, stdDev1) * _pc1 < Gaussian(currDot, mu2, stdDev2) * _pc2)
                    break;
                currDot += 0.01;
            }

            graph.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, Color = OxyColors.Gold, LineStyle = LineStyle.Solid, X = currDot });
            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Green };
            scatterSeries.Points.Add(new ScatterPoint(currDot, Gaussian(currDot, mu1, stdDev1) * _pc1, 5));
            graph.Series.Add(scatterSeries);

            double error1 = _pc1 == 1 ? 0 : 1 - CDF(currDot, mu1, stdDev1);
            double error2 = _pc1 == 0 ? 0 : CDF(currDot, mu2, stdDev2);

            //if (_pc1 == 1) error2 = 1;
            //else if (_pc1 == 0) error1 = 1;

            txtPropFalseAlarm.Text = error1.ToString("F5");
            txtPropSkip.Text = error2.ToString("F5");
            txtPropSum.Text = (error1 + error2).ToString("F5");

            return graph;
        }

        private double Gaussian(double x, double mean, double stdDev)
        {
            return (1 / (stdDev * Math.Sqrt(2 * Math.PI))) * Math.Exp(-0.5 * Math.Pow((x - mean) / stdDev, 2));
        }

        private double CDF(double x, double mean, double stdDev)
        {
            return 0.5 * (1 + Erf((x - mean) / (stdDev * Math.Sqrt(2))));
        }

        private double Erf(double x)
        {
            double t = 1.0 / (1.0 + 0.5 * Math.Abs(x));
            double tau = t * Math.Exp(-x * x - 1.26551223 + t * (1.00002368 + t * (0.37409196 + t * (0.09678418 +
                        t * (-0.18628806 + t * (0.27886807 + t * (-1.13520398 + t * (1.48851587 +
                        t * (-0.82215223 + t * 0.17087277)))))))));
            return x >= 0 ? 1 - tau : tau - 1;
        }


    }
}