using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab2
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

        public int action = 0;
        public int clast = 0;
        public bool isFinish = false;   
        private void btnShowDots_Click(object sender, RoutedEventArgs e)
        {
            action = 0;
            clast = 0;
            MainFunctions.headDots = null;
            MainFunctions.arrOfDotsSt = null;
            MainFunctions.DotsAmount = Convert.ToInt32(inputPointsAmount.Text);
            MainFunctions.initArrayOfDots(ref MainFunctions.arrOfDotsSt, MainFunctions.DotsAmount);
            MainFunctions.printDots(cnvOriginal, MainFunctions.arrOfDotsSt, MainFunctions.DotsAmount);

        }

        private void btnShowNextClast_Click(object sender, RoutedEventArgs e)
        {
            if (action == 0)
            {
                isFinish = false;
                MainFunctions.firstClass(MainFunctions.arrOfDotsSt, MainFunctions.DotsAmount);
                action++;
                clast++;
            } else if (action == 1)
            {
                MainFunctions.secondClass(MainFunctions.arrOfDotsSt, MainFunctions.DotsAmount);
                clast++;
                action++;
            } else
            {
                if (!isFinish)
                {
                if (MainFunctions.elseClass(MainFunctions.arrOfDotsSt, MainFunctions.DotsAmount)) clast++;
                else isFinish = true;

                }

            }
            

            MainFunctions.printDots(cnvSecondClaster, MainFunctions.arrOfDotsSt, MainFunctions.DotsAmount);
            txtClastAmount.Text = $"Количество кластеров: {clast}";
        }
    }
}