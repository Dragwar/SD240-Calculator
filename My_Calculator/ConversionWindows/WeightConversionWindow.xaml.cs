using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using My_Calculator.Helpers;
using My_Calculator.Helpers.Enums;

namespace My_Calculator.ConversionWindows
{
    /// <summary>
    /// Interaction logic for WeightConversionWindow.xaml
    /// </summary>
    public partial class WeightConversionWindow : Window
    {
        private WeightConversion WeightConversion { get; } = new WeightConversion(0, WeightTypeEnum.Milligrams);
        private List<string> MainOutput { get; set; } = new List<string>();

        public const string ButtonString = "Button";

        public WeightConversionWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;

            Closed += (object sender, EventArgs e) => SelectConversionWindow.CloseAllConversionWindowsAndOpenSelectConversionWindow();

            Show();
        }

        private bool CompareButtonName<TEnum>(string name, TEnum enumToCompare) where TEnum : Enum => name == (enumToCompare.ToString() + ButtonString);
        private void AddToMainOutputTextAndUpdate(string str)
        {
            MainOutput.Add(str);
            MainOutputText.Text = string.Join("", MainOutput);
        }
        private void RemoveFromMainOutputTextAndUpdate([Optional] int? index)
        {
            if (MainOutput != null && MainOutput.Any())
            {
                MainOutput.RemoveAt(index ?? (MainOutput.Count - 1));
                MainOutputText.Text = string.Join("", MainOutput);
            }
        }
        private void InputControlClearEntry()
        {
            MainOutputText.Text = "";
            MainOutput.Clear();
        }
        private void InputControlClearAll()
        {
            MainOutputText.Text = "";
            MainOutput.Clear();
            MilligramsCell.Content = "";
            GramsCell.Content = "";
            KilogramsCell.Content = "";
            OuncesCell.Content = "";
            PoundsCell.Content = "";
        }
        private void InputControlBackSpace() => RemoveFromMainOutputTextAndUpdate();


        private void HandleNewOperator(string mathOperator)
        {
            if (mathOperator == ".")
            {
                // only add on dot to MainOutput
                if (MainOutput.Contains("."))
                {
                    MessageBox.Show("A number cannot contain more then one decimal place", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MainOutput.Add(".");
                    MainOutputText.Text = string.Join("", MainOutput);
                }
            }
            else if (mathOperator == "=")
            {
                CalculateConversion();
            }
        }

        private void HamburgerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            List<SelectConversionWindow> selectConversionWindows = App.Current.Windows
                .OfType<SelectConversionWindow>()
                .ToList();

            SelectConversionWindow selectWindow;
            if (selectConversionWindows.Any())
            {
                selectWindow = selectConversionWindows.First();
                if (selectConversionWindows.Count >= 2)
                {
                    selectConversionWindows.Remove(selectWindow);
                    selectConversionWindows.ForEach(window =>
                    {
                        window.Hide();
                        window.Close();
                    });
                }
            }
            else
            {
                selectWindow = new SelectConversionWindow();
            }

            selectWindow.Activate();
        }

        private void UnitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => CalculateConversion();

        private void CalculateConversion()
        {
            if (IsLoaded && UnitComboBox.SelectedItem is ComboBoxItem item)
            {
                if (double.TryParse(MainOutputText.Text, out double currentValue))
                {
                    string currentFileSizeTypeString = item.Tag as string;
                    try
                    {
                        WeightTypeEnum currentWeightType = (WeightTypeEnum)Enum.Parse(typeof(WeightTypeEnum), currentFileSizeTypeString, true);
                        WeightConversion.SetValueToConvert(currentValue, currentWeightType);
                        MilligramsCell.Content = WeightConversion.Milligrams;
                        GramsCell.Content = WeightConversion.Grams;
                        KilogramsCell.Content = WeightConversion.Kilograms;
                        OuncesCell.Content = WeightConversion.Ounces;
                        PoundsCell.Content = WeightConversion.Pounds;
                    }
                    catch (OverflowException err)
                    {
                        MessageBox.Show($"{err.Message}", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (ArgumentException err)
                    {
                        MessageBox.Show($"The File Size type was not recognized.\n\n{err.Message}", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
                    // Check if the Button name matches
                    validInputControl = Enum.GetValues(typeof(InputControlEnum))
                        .Cast<int>()
                        .Any(myEnum => CompareButtonName(btnName, (InputControlEnum)myEnum));
                }

                if (validInputControl && Enum.Parse(typeof(InputControlEnum), btnName.Replace(ButtonString, "")) is InputControlEnum useEnum)
                {
                    // handle InputControl action
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

        private void InputNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                AddToMainOutputTextAndUpdate(btn.Content as string);
            }
        }
        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string operatorbtn = btn.Tag as string;
                HandleNewOperator(operatorbtn);
            }
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (MainOutputText.IsKeyboardFocused)
            {
                MainOutput.Clear();
                MainOutput.TrimExcess();
                MainOutput.AddRange(MainOutputText.Text.Trim().Select(c => c.ToString()));
                return;
            }

            // handle keys
            switch (e.Key)
            {
                case Key.Enter: CalculateConversion(); break;

                case Key.Back: InputControlBackSpace(); break;

                case Key.D0: AddToMainOutputTextAndUpdate("0"); break;
                case Key.D1: AddToMainOutputTextAndUpdate("1"); break;
                case Key.D2: AddToMainOutputTextAndUpdate("2"); break;
                case Key.D3: AddToMainOutputTextAndUpdate("3"); break;
                case Key.D4: AddToMainOutputTextAndUpdate("4"); break;
                case Key.D5: AddToMainOutputTextAndUpdate("5"); break;
                case Key.D6: AddToMainOutputTextAndUpdate("6"); break;
                case Key.D7: AddToMainOutputTextAndUpdate("7"); break;
                case Key.D8: AddToMainOutputTextAndUpdate("8"); break;
                case Key.D9: AddToMainOutputTextAndUpdate("9"); break;

                case Key.NumPad0: AddToMainOutputTextAndUpdate("0"); break;
                case Key.NumPad1: AddToMainOutputTextAndUpdate("1"); break;
                case Key.NumPad2: AddToMainOutputTextAndUpdate("2"); break;
                case Key.NumPad3: AddToMainOutputTextAndUpdate("3"); break;
                case Key.NumPad4: AddToMainOutputTextAndUpdate("4"); break;
                case Key.NumPad5: AddToMainOutputTextAndUpdate("5"); break;
                case Key.NumPad6: AddToMainOutputTextAndUpdate("6"); break;
                case Key.NumPad7: AddToMainOutputTextAndUpdate("7"); break;
                case Key.NumPad8: AddToMainOutputTextAndUpdate("8"); break;
                case Key.NumPad9: AddToMainOutputTextAndUpdate("9"); break;
                default: break;
            }
        }



        #region Not Implemented Yet
        private void HistoryButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This History Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
        #endregion

    }
}
