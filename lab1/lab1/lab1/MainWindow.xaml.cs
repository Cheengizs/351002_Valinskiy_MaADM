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

namespace lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void showAllDots(Canvas cnv, DotOnClaster[] arrOfDots, int pointsAmount)
        {
            Random random = new Random();
            for (int i = 0; i < pointsAmount; i++)
            {
                int x = random.Next(1, 260);
                int y = random.Next(1, 260);
                Ellipse dot = new Ellipse();
                dot.Fill = new SolidColorBrush(Colors.Black);
                dot.Width = 1;
                dot.Height = 1;
                cnv.Children.Add(dot);
                Canvas.SetLeft(dot, x);
                Canvas.SetTop(dot, y);

                arrOfDots[i].X = x;
                arrOfDots[i].Y = y;
                //arrOfDots[i].OneDot = dot;
            }
        }

        public void changeSecondClaster(Canvas cnv, DotOnClaster[] arrOfDots, int pointsAmount, int clastAmount)
        {
            Random rnd = new Random();
            DotOnClaster[] HeadClast = new DotOnClaster[clastAmount];
            DotOnClaster[] bufHeadClast = new DotOnClaster[clastAmount];
            for (int i = 0; i < clastAmount; i++)
            {
                int dotNumb = rnd.Next(0, pointsAmount - 1);
                bufHeadClast[i] = HeadClast[i] = arrOfDots[dotNumb];
                arrOfDots[dotNumb].ClastNumb = i + 1;
            }

            bool canQuit = false;

            while (!canQuit)
            {
                // Перебором отношу точку к какому-то кластеру
                for (int i = 0; i < pointsAmount; i++)
                {
                    double minDist = int.MaxValue;
                    double tempClast = 0;

                    for (int j = 0; j < clastAmount; j++)
                    {
                        double dist = Math.Sqrt(Math.Pow(arrOfDots[i].X - HeadClast[j].X, 2)
                            + Math.Pow(arrOfDots[i].Y - HeadClast[j].Y, 2));
                        if (minDist > dist)
                        {
                            minDist = dist;
                            tempClast = j + 1;
                        }
                    }
                    arrOfDots[i].ClastNumb = Convert.ToInt32(tempClast);
                }

                //Определяю новые центроиды
                for (int i = 0; i < clastAmount; i++)
                {
                    int xAvrg = 0, yAvrg = 0, dotAmount = 0;

                    for (int j = 0; j < pointsAmount; j++)
                    {
                        if (arrOfDots[j].ClastNumb == i + 1)
                        {
                            dotAmount++;
                            xAvrg += arrOfDots[j].X;
                            yAvrg += arrOfDots[j].Y;
                        }
                    }
                    HeadClast[i].X = Convert.ToInt32(xAvrg / dotAmount);
                    HeadClast[i].Y = Convert.ToInt32(yAvrg / dotAmount);
                }

                canQuit = true;

                for (int i = 0; i < clastAmount; i++)
                {
                    if (!(HeadClast[i].X == bufHeadClast[i].X && HeadClast[i].Y == bufHeadClast[i].Y))
                    {
                        canQuit = false;
                    }
                    bufHeadClast[i].X = HeadClast[i].X;
                    bufHeadClast[i].Y = HeadClast[i].Y;
                }

            }

            Color[] colors = new Color[]
            {
                 Color.FromArgb(255, 255, 0, 0),     // Красный
    Color.FromArgb(255, 0, 255, 0),     // Зеленый
    Color.FromArgb(255, 0, 0, 255),     // Синий
    Color.FromArgb(255, 255, 255, 0),    // Желтый
    Color.FromArgb(255, 255, 0, 255),    // Фиолетовый
    Color.FromArgb(255, 255, 165, 0),    // Оранжевый
    Color.FromArgb(255, 0, 255, 255),    // Циан
    Color.FromArgb(255, 255, 0, 100),    // Магента
    Color.FromArgb(255, 0, 0, 139),      // Темно-синий
    Color.FromArgb(255, 0, 100, 0),      // Темно-зеленый
    Color.FromArgb(255, 139, 0, 0),       // Темно-красный
    Color.FromArgb(255, 128, 128, 128),   // Серый
    Color.FromArgb(255, 173, 216, 230),   // Светло-синий
    Color.FromArgb(255, 144, 238, 144),   // Светло-зеленый
    Color.FromArgb(255, 165, 42, 42)      // Коричневый
            };

            for (int i = 0; i < pointsAmount; i++)
            {
                int clast = Convert.ToByte(arrOfDots[i].ClastNumb);
                Ellipse el = new Ellipse();
                el.Width = 1;
                el.Height = 1;
                el.Fill = new SolidColorBrush(colors[clast - 1]);
                cnv.Children.Add(el);
                Canvas.SetLeft(el, arrOfDots[i].X);
                Canvas.SetTop(el, arrOfDots[i].Y);
            }
            
            for (int i = 0; i < clastAmount; i++)
            {
                Ellipse el = new Ellipse();
                el.Width = 5;
                el.Height = 5;
                el.Fill = new SolidColorBrush(Colors.Gray);
                cnv.Children.Add(el);
                Canvas.SetLeft(el, HeadClast[i].X);
                Canvas.SetTop(el, HeadClast[i].Y);
            }


        }

        public struct DotOnClaster
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int ClastNumb { get; set; }
            //public Ellipse OneDot { get; set; }
            public DotOnClaster()
            {
                ClastNumb = 0;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            cnvFirstClaster.Children.Clear();
            cnvSecondClaster.Children.Clear();
            int pointsAmount = Convert.ToInt32(inputPointsAmount.Text);
            int clastersAmount = Convert.ToInt32(inputClastersAmount.Text);
            DotOnClaster[] dotArr = new DotOnClaster[pointsAmount];
            showAllDots(cnvFirstClaster, dotArr, pointsAmount);
            changeSecondClaster(cnvSecondClaster, dotArr, pointsAmount, clastersAmount);

        }
    }
}