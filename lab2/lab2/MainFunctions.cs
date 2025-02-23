using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab2
{
    struct DotOnClaster
    {
        public int Ycoord { get; set; }
        public int Xcoord { get; set; }
        public int ClastNumb { get; set; }
        public DotOnClaster()
        {
            ClastNumb = 0;
        }
    }

    internal static class MainFunctions
    {
        static Color[] colors = new Color[]
            {
                Color.FromArgb(255, 0, 0, 0),
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

        public static int DotsAmount { get; set; }
        public static DotOnClaster[]? arrOfDotsSt;
        public static List<DotOnClaster>? headDots;
        public static List<DotOnClaster>? bufHeadDots;

        public static void initArrayOfDots(ref DotOnClaster[] arrOfDots, int dotsAmount)
        {
            Random rnd = new Random();
            arrOfDots = new DotOnClaster[dotsAmount];
            for (int i = 0; i < dotsAmount; i++)
            {
                arrOfDots[i].Xcoord = rnd.Next(1, 310);
                arrOfDots[i].Ycoord = rnd.Next(1, 310);
                arrOfDots[i].ClastNumb = 0;
            }

        }

        public static void printDots(Canvas cnv, DotOnClaster[] arrOfDots, int dotsAmount)
        {
            cnv.Children.Clear();
            for (int i = 0; i < dotsAmount; i++)
            {
                Ellipse el = new Ellipse();

                bool isCenter = false;
                if (headDots is not null)
                    for (int j = 0; j < headDots.Count(); j++)
                    {
                        if (arrOfDots[i].Xcoord == headDots[j].Xcoord && arrOfDots[i].Ycoord == headDots[j].Ycoord)
                        {
                            isCenter = true;
                            break;
                        }
                    }


                if (isCenter)
                {
                    el.Width = 4;
                    el.Height = 4;
                    el.Fill = new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    el.Width = 1;
                    el.Height = 1;
                    el.Fill = new SolidColorBrush(colors[arrOfDots[i].ClastNumb]);
                }
                Canvas.SetLeft(el, arrOfDots[i].Xcoord);
                Canvas.SetTop(el, arrOfDots[i].Ycoord);

                cnv.Children.Add(el);
            }
        }

        public static double getDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static void firstClass(DotOnClaster[] arrOfDots, int dotsAmount)
        {
            Random rnd = new Random();
            headDots = new List<DotOnClaster>();
            headDots.Add(arrOfDots[rnd.Next(0, dotsAmount - 1)]);

            for (int i = 0; i < dotsAmount; i++)
            {
                arrOfDots[i].ClastNumb = 1;
            }

        }
        public static void secondClass(DotOnClaster[] arrOfDots, int dotsAmount)
        {
            int tempInd = 0;
            double tempMaxDist = int.MinValue;

            for (int i = 0; i < dotsAmount; i++)
            {

                double dist = Math.Sqrt(Math.Pow(arrOfDots[i].Xcoord - headDots[0].Xcoord, 2)
                            + Math.Pow(arrOfDots[i].Ycoord - headDots[0].Ycoord, 2));

                if (dist > tempMaxDist)
                {
                    tempMaxDist = dist;
                    tempInd = i;
                }
            }

            headDots.Add(arrOfDots[tempInd]);

            // Перебором отношу точку к какому-то кластеру
            for (int i = 0; i < dotsAmount; i++)
            {
                double minDist = int.MaxValue;
                int tempClast = 0;

                for (int j = 0; j < 2; j++)
                {
                    double dist = Math.Sqrt(Math.Pow(arrOfDots[i].Xcoord - headDots[j].Xcoord, 2)
                            + Math.Pow(arrOfDots[i].Ycoord - headDots[j].Ycoord, 2));

                    if (minDist > dist)
                    {
                        minDist = dist;
                        tempClast = j + 1;
                    }
                }
                arrOfDots[i].ClastNumb = tempClast;
            }
        }

        public static double calcAvrgDistHead()
        {
            int distanceAmount = 0;
            double distSum = 0;
            for (int i = 0; i < headDots.Count() - 1; i++)
            {
                for (int j = i + 1; j < headDots.Count; j++)
                {
                    distSum += Math.Sqrt(Math.Pow(headDots[i].Xcoord - headDots[j].Xcoord, 2)
                            + Math.Pow(headDots[i].Ycoord - headDots[j].Ycoord, 2));
                    distanceAmount++;
                }
            }

            return (0.5 * distSum) / distanceAmount;
        }


        public static bool elseClass(DotOnClaster[] arrOfDots, int dotsAmount)
        {
            double avrgDist = calcAvrgDistHead();
            double tempMaxDist = 0;
            int tempIndex = 0;


            for (int i = 0; i < dotsAmount; i++)
            {
                double tempDist = Math.Sqrt(Math.Pow(arrOfDots[i].Xcoord - headDots[arrOfDots[i].ClastNumb - 1].Xcoord, 2)
                            + Math.Pow(arrOfDots[i].Ycoord - headDots[arrOfDots[i].ClastNumb - 1].Ycoord, 2));

                if (tempDist > tempMaxDist)
                {
                    tempMaxDist = tempDist;
                    tempIndex = i;
                }
            }

            if (avrgDist < tempMaxDist)
            {
                headDots.Add(arrOfDots[tempIndex]);

                // Перебором отношу точку к какому-то кластеру
                for (int i = 0; i < dotsAmount; i++)
                {
                    double minDist = int.MaxValue;
                    int tempClast = 0;

                    for (int j = 0; j < headDots.Count(); j++)
                    {
                        double dist = Math.Sqrt(Math.Pow(arrOfDots[i].Xcoord - headDots[j].Xcoord, 2)
                                + Math.Pow(arrOfDots[i].Ycoord - headDots[j].Ycoord, 2));

                        if (minDist > dist)
                        {
                            minDist = dist;
                            tempClast = j + 1;
                        }
                    }
                    arrOfDots[i].ClastNumb = tempClast;
                }
                return true;
            }
            return false;

        }
    }
}
