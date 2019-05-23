using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using My_Calculator.Helpers;
using My_Calculator.Helpers.Enums;
using org.mariuszgromada.math.mxparser.mathcollection;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace My_Calculator.ConversionWindows
{
    /// <summary>
    /// Interaction logic for NumeralSystemConversionWindow.xaml
    /// </summary>
    public partial class NumeralSystemConversionWindow : Window
    {
        private const string ButtonString = "Button";
        private readonly List<char> MainInputAllowedCharacters = new List<char>() { '-', '.', 'E', 'X', 'x', 'b', 'B' };
        private readonly List<char> SecondaryInputAllowedCharacters = new List<char>() { '-', '.', '+', 'E', 'X', 'x', 'b', 'B' };

        private List<string> MainOutput { get; set; }
        private List<string> SecondaryOutput { get; set; }
        private NumeralSystemConversion NumeralSystemConversion { get; } = new NumeralSystemConversion(0);
        public NumeralSystemConversionWindow()
        {
            InitializeComponent();
            MainOutput = new List<string>();
            SecondaryOutput = new List<string>();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;
            Show();
        }
        private bool CompareButtonName<TEnum>(string name, TEnum enumToCompare) where TEnum : Enum => name == (enumToCompare.ToString() + ButtonString);

        private void InputControlClearEntry()
        {
            MainOutputText.Text = "";
            MainOutput.Clear();
        }
        private void InputControlClearAll()
        {
            SecondaryOutputText.Text = "";
            MainOutputText.Text = "";
            MainOutput.Clear();
            SecondaryOutput.Clear();
        }
        private void InputControlBackSpace() => RemoveFromMainOutputTextAndUpdate();
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

        /// <summary>
        /// Break down (<see cref="double"/> <paramref name="result"/>) to a charArray to allow the user to delete single characters
        /// and then display the result on the selected (<see cref="OutputTextBlockEnum"/> output) window
        /// <para/>
        /// 
        /// </summary>
        /// <param name="result">calculation result</param>
        /// <param name="textBlock">What textBlock and textList to use</param>
        /// <param name="config">
        ///     <para/>[<see cref="Required"/>] ClearAllBeforeUpdateOutput: Clear input window, 
        ///     <para/>[<see cref="OptionalAttribute"/>] AllowedCharacters: what characters to keep during rebuild of the output, 
        ///     <para/>[<see cref="OptionalAttribute"/>] OperatorToAppend: what mathOperator to append
        /// </param>
        /// <returns>(<see cref="string"/> output)</returns>
        private string DeconstructResultAndDisplay(
            string result,
            OutputTextBlockEnum textBlock,
            (bool ClearAllBeforeUpdateOutput, List<char> AllowedCharacters, char? OperatorToAppend) config)
        {
            // is MainOutput TextBlock or SecondaryOutput TextBlock
            bool isMainOutput;
            switch (textBlock)
            {
                case OutputTextBlockEnum.Main:
                    //! default to private list of (MainInputAllowedCharacters) allowed operators
                    if (config.AllowedCharacters is null || !config.AllowedCharacters.Any())
                    {
                        config.AllowedCharacters = MainInputAllowedCharacters;
                    }
                    isMainOutput = true;
                    break;

                case OutputTextBlockEnum.Secondary:
                    //! default to private list (SecondaryInputAllowedCharacters) of allowed operators
                    if (config.AllowedCharacters is null || !config.AllowedCharacters.Any())
                    {
                        config.AllowedCharacters = SecondaryInputAllowedCharacters;
                    }
                    isMainOutput = false;
                    break;

                default: throw new Exception("This shouldn't happen");
            }


            // clear current text and values
            if (config.ClearAllBeforeUpdateOutput)
            {
                InputControlClearAll();
            }
            else
            {
                if (isMainOutput)
                {
                    MainOutput.Clear();
                    MainOutputText.Text = "";
                }
                else
                {
                    SecondaryOutput.Clear();
                    SecondaryOutputText.Text = "";
                }
            }

            // allows user to use backspace on each character in result string
            foreach (char num in result.ToCharArray())
            {
                if (char.IsDigit(num) || config.AllowedCharacters.Contains(num))
                {
                    if (isMainOutput)
                    {
                        MainOutput.Add($"{num}");
                    }
                    else
                    {
                        SecondaryOutput.Add($"{num}");
                    }
                }
            }

            string output;
            // rebuild output with new result
            if (isMainOutput)
            {
                output = string.Join("", MainOutput);
                MainOutputText.Text = output;
            }
            else
            {
                // append character (mathOperator) to SecondaryOutput
                if (config.OperatorToAppend.HasValue && config.AllowedCharacters.Contains(config.OperatorToAppend.Value))
                {
                    SecondaryOutput.Add(config.OperatorToAppend.Value.ToString());
                }
                output = string.Join("", SecondaryOutput);
                SecondaryOutputText.Text = output;
            }
            return output ?? throw new Exception("Failed To generate text output from result");
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
            else if (SecondaryOutputText.IsKeyboardFocused)
            {
                SecondaryOutput.Clear();
                SecondaryOutput.TrimExcess();
                SecondaryOutput.AddRange(SecondaryOutputText.Text.Trim().Select(c => c.ToString()));
                return;
            }

            // handle keys
            switch (e.Key)
            {
                case Key.Enter: /*TODO: Convert MainOutput string to int*/ break;
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
                if (operatorbtn == "plus-minus")
                {
                    if (MainOutput.Any())
                    {
                        string main = string.Join("", MainOutput);
                        Expression expr = new Expression(main);
                        string result = (expr.calculate() * -1).ToString(); // toggle to positive/negative

                        // Display Result
                        try
                        {
                            _ = DeconstructResultAndDisplay(result, OutputTextBlockEnum.Main, (false, null, null));
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show(error.Message, $"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }


        private void BinaryButton_Click(object sender, RoutedEventArgs e)
        {
            string main = string.Join("", MainOutput);
            main = string.IsNullOrWhiteSpace(main) ? "0" : main;

            MainOutput.Clear();

            try
            {
                NumeralSystemConversion.NumberToConvert = int.Parse(main);
            }
            catch (FormatException err)
            {
                MainOutputText.Text = $"{err.Message}";
                return;
            }
            catch (OverflowException err)
            {
                MainOutputText.Text = $"{err.Message}";
                return;
            }

            _ = DeconstructResultAndDisplay(NumeralSystemConversion.Binary, OutputTextBlockEnum.Main, (true, null, null));
            SecondaryOutputText.Text = $"{main} to Binary";
        }

        private void OctalButton_Click(object sender, RoutedEventArgs e)
        {
            string main = string.Join("", MainOutput);
            main = string.IsNullOrWhiteSpace(main) ? "0" : main;

            MainOutput.Clear();

            try
            {
                NumeralSystemConversion.NumberToConvert = int.Parse(main);
            }
            catch (FormatException err)
            {
                MainOutputText.Text = $"{err.Message}";
                return;
            }
            catch (OverflowException err)
            {
                MainOutputText.Text = $"{err.Message}";
                return;
            }

            _ = DeconstructResultAndDisplay(NumeralSystemConversion.Octal, OutputTextBlockEnum.Main, (true, null, null));
            SecondaryOutputText.Text = $"{main} to Octal";
        }

        private void HexadecimalButton_Click(object sender, RoutedEventArgs e)
        {
            string main = string.Join("", MainOutput);
            main = string.IsNullOrWhiteSpace(main) ? "0" : main;

            MainOutput.Clear();

            try
            {
                NumeralSystemConversion.NumberToConvert = int.Parse(main);
            }
            catch (FormatException err)
            {
                MainOutputText.Text = $"{err.Message}";
                return;
            }
            catch (OverflowException err)
            {
                MainOutputText.Text = $"{err.Message}";
                return;
            }

            _ = DeconstructResultAndDisplay(
                result: NumeralSystemConversion.Hexadecimal,
                textBlock: OutputTextBlockEnum.Main,
                config: (true, new List<char>(MainInputAllowedCharacters) { 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f' }, null));
            SecondaryOutputText.Text = $"{main} to Hexadecimal";
        }


        #region Not Implemented Yet
        private void HistoryButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This History Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
        #endregion
    }
}
