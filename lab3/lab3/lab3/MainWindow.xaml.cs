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

namespace lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            sldrPropOne.ValueChanged += sldrPropOne_OnValueChanged;
            sldrPropTwo.ValueChanged += sldrPropTwo_OnValueChanged;
        }

        public void sldrPropOne_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldrPropTwo.Value = 1 - (sender as Slider).Value;
        }

        public void sldrPropTwo_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldrPropOne.Value = 1 - ((Slider)sldrPropTwo).Value;

        }

        private void sldrPropTwo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}