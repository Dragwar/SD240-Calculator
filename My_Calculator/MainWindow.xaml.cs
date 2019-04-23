using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using My_Calculator.Helpers.Enums;

namespace My_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ButtonString = "Button";
        public List<decimal> Numbers { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Numbers = new List<decimal>();
        }
        private bool CompareName<TEnum>(string name, TEnum enumToCompare) where TEnum : Enum => name == (enumToCompare.ToString() + ButtonString);
        private void InputControlClearAll()
        {
            TopOutputText.Text = "";
            MainOutputText.Text = "";
            Numbers.Clear();
        }
        private void InputControlClearEntry() => MessageBox
            .Show("Clear Entry is not implemented yet", $"ClearEntry{ButtonString}", MessageBoxButton.OK, MessageBoxImage.Warning);

        private void InputControlBackSpace()
        {
            if (Numbers.Count >= 1)
            {
                Numbers.RemoveAt(Numbers.Count - 1);
                MainOutputText.Text = string.Join("", Numbers);
            }
        }

        private void InputControlButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string btnName = btn.Name as string;
                bool validInputControl = false;

                if (!string.IsNullOrWhiteSpace(btnName))
                {
                    validInputControl = Enum.GetValues(typeof(InputControlEnum))
                        .Cast<int>()
                        .Any(@enum => CompareName(btnName, (InputControlEnum)@enum));
                }

                if (validInputControl && Enum.Parse(typeof(InputControlEnum), btnName.Replace(ButtonString, "")) is InputControlEnum useEnum)
                {
                    switch (useEnum)
                    {
                        case InputControlEnum.BackSpace: InputControlBackSpace(); break;
                        case InputControlEnum.ClearEntry: InputControlClearEntry(); break;
                        case InputControlEnum.ClearAll: InputControlClearAll(); break;

                        default: throw new Exception("this shouldn't happen");
                    }
                }
            }
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

        private void HistoryButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This History Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);

        private void HamburgerMenuButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This Hamburger Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
