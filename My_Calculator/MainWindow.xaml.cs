using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace My_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<decimal> Numbers { get; set; } = new List<decimal>();
        public MainWindow()
        {
            InitializeComponent();

        }

        private void InputControlButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MathFunctionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Content as string, out int number))
            {
                Numbers.Add(number);
                MainOutputText.Text = string.Join("", Numbers);
            }
        }
    }
}
