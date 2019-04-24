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
        private void AddNumberToList(InputNumberEnum numberEnum)
        {
            Numbers.Add((int)numberEnum);
            MainOutputText.Text = string.Join("", Numbers);
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


        private void MathFunctionButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show($"This {(sender as Button)?.Name} is not implemented yet", $"{(sender as Button)?.Name}", MessageBoxButton.OK, MessageBoxImage.Warning);


        private void OperatorButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show($"This {(sender as Button)?.Name} is not implemented yet", $"{(sender as Button)?.Name}", MessageBoxButton.OK, MessageBoxImage.Warning);

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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab: break;

                //case Key.Return when (int)e.Key == 6: break;
                case Key.Enter: break;

                case Key.Escape: break;

                case Key.Space: break;

                case Key.Back: InputControlBackSpace(); break;

                case Key.Left: break;
                case Key.Up: break;
                case Key.Right: break;
                case Key.Down: break;

                case Key.D0: AddNumberToList(InputNumberEnum.Zero); break;
                case Key.D1: AddNumberToList(InputNumberEnum.One); break;
                case Key.D2: AddNumberToList(InputNumberEnum.Two); break;
                case Key.D3: AddNumberToList(InputNumberEnum.Three); break;
                case Key.D4: AddNumberToList(InputNumberEnum.Four); break;
                case Key.D5: AddNumberToList(InputNumberEnum.Five); break;
                case Key.D6: AddNumberToList(InputNumberEnum.Six); break;
                case Key.D7: AddNumberToList(InputNumberEnum.Seven); break;
                case Key.D8: AddNumberToList(InputNumberEnum.Eight); break;
                case Key.D9: AddNumberToList(InputNumberEnum.Nine); break;

                case Key.NumPad0: AddNumberToList(InputNumberEnum.Zero); break;
                case Key.NumPad1: AddNumberToList(InputNumberEnum.One); break;
                case Key.NumPad2: AddNumberToList(InputNumberEnum.Two); break;
                case Key.NumPad3: AddNumberToList(InputNumberEnum.Three); break;
                case Key.NumPad4: AddNumberToList(InputNumberEnum.Four); break;
                case Key.NumPad5: AddNumberToList(InputNumberEnum.Five); break;
                case Key.NumPad6: AddNumberToList(InputNumberEnum.Six); break;
                case Key.NumPad7: AddNumberToList(InputNumberEnum.Seven); break;
                case Key.NumPad8: AddNumberToList(InputNumberEnum.Eight); break;
                case Key.NumPad9: AddNumberToList(InputNumberEnum.Nine); break;

                //TODO: Make operator work
                case Key.Multiply: break;
                case Key.Add: break;
                case Key.Separator: break;
                case Key.Subtract: break;
                case Key.Decimal: break;
                case Key.Divide: break;

                default: break;
            }

        }
    }
}
