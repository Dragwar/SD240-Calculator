using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using My_Calculator.Helpers.Enums;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace My_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ButtonString = "Button";
        private readonly List<char> MainInputAllowedCharacters = new List<char>() { '-', '.', };
        private readonly List<char> SecondaryInputAllowedCharacters = new List<char>() { '-', '.', '+', '*', '/', };


        private List<string> MainOutput { get; set; }
        private List<string> SecondaryOutput { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainOutput = new List<string>();
            SecondaryOutput = new List<string>();
        }

        private void AddToMainOutputText(string str)
        {
            MainOutput.Add(str);
            MainOutputText.Text = string.Join("", MainOutput);
        }
        private void RemoveFromMainOutputText([Optional] int? index)
        {
            if (MainOutput != null && MainOutput.Count > 0)
            {
                MainOutput.RemoveAt(index ?? (MainOutput.Count - 1));
                MainOutputText.Text = string.Join("", MainOutput);
            }
        }
        private void RemoveFromSecondaryOutputText([Optional] int? index)
        {
            if (SecondaryOutput != null && SecondaryOutput.Count > 0)
            {
                SecondaryOutput.RemoveAt(index ?? (SecondaryOutput.Count - 1));
                SecondaryOutputText.Text = string.Join("", SecondaryOutput);
            }
        }
        private void AddToSecondaryOutputText(string str)
        {
            SecondaryOutput.Add(str);
            SecondaryOutputText.Text = string.Join("", SecondaryOutput);
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
        private void InputControlBackSpace() => RemoveFromMainOutputText();
        private void DeconstructResult(double result, OutputTextBlockEnum textBlock, (bool ClearAllBeforeUpdateOutput, List<char> AllowedCharacters, char? OperatorToAppend) config)
        {
            // is MainOutput TextBlock or SecondaryOutput TextBlock
            bool isMainOutput;
            switch (textBlock)
            {
                case OutputTextBlockEnum.Main:
                    //! default to private list of (MainInputAllowedCharacters) allowed operators
                    config.AllowedCharacters = (config.AllowedCharacters?.Count ?? -1) <= 0 ? MainInputAllowedCharacters : config.AllowedCharacters;
                    isMainOutput = true;
                    break;

                case OutputTextBlockEnum.Secondary:
                    //! default to private list (SecondaryInputAllowedCharacters) of allowed operators
                    config.AllowedCharacters = (config.AllowedCharacters?.Count ?? -1) <= 0 ? SecondaryInputAllowedCharacters : config.AllowedCharacters;
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
            foreach (char num in result.ToString().ToCharArray())
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

            // rebuild output with new result
            if (isMainOutput)
            {
                MainOutputText.Text = string.Join("", MainOutput);
            }
            else
            {
                // append character (mathOperator) to SecondaryOutput
                if (config.OperatorToAppend.HasValue && config.AllowedCharacters.Contains(config.OperatorToAppend.Value))
                {
                    SecondaryOutput.Add(config.OperatorToAppend.Value.ToString());
                }
                SecondaryOutputText.Text = string.Join("", SecondaryOutput);
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
                        .Any(@enum => CompareButtonName(btnName, (InputControlEnum)@enum));
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

        private void InputNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                AddToMainOutputText(btn.Content as string);
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
            else if (mathOperator == "plus-minus")
            {
                if (MainOutput.Count >= 1)
                {
                    string main = string.Join("", MainOutput);
                    Expression expr = new Expression(main);
                    double result = expr.calculate();
                    result *= -1; // toggle to positive/negative

                    DeconstructResult(result, OutputTextBlockEnum.Main, (false, null, null));
                }
            }
            else if (mathOperator == "=")
            {
                //! Calculate result
                string leftExpr = string.Join("", SecondaryOutput);
                string rightExpr = string.Join("", MainOutput);

                // allows user to use the plus-minus operator after pressing an operator ('+','-','/',...)
                if (rightExpr.StartsWith("-"))
                {
                    rightExpr = $"({rightExpr})"; //! surround with brackets for mXpareser to understand
                }

                Expression expr = new Expression(leftExpr + rightExpr);
                double result = expr.calculate();

                MessageBox.Show($"{expr.getExpressionString()} = {result}", "Result", MessageBoxButton.OK, MessageBoxImage.Information);

                DeconstructResult(result, OutputTextBlockEnum.Main, (true, null, null));
            }
            else if (MainOutput.Count >= 1 && SecondaryOutput.Count <= 0 && mathOperator != ".")
            {
                //! Add the operator to the end then show expression in the SecondaryOutput
                //! (example: MainOutput --> "24" -> "24+" -> "")
                //! (SecondaryOutput     --> ""   -> ""    -> "24+")
                //! [now waiting for right hand side of the expression]
                SecondaryOutput = new List<string>(MainOutput)
                {
                    mathOperator as string
                };
                MainOutput.Clear();

                SecondaryOutputText.Text = string.Join("", SecondaryOutput);
                MainOutputText.Text = "";
            }
            else if (MainOutput.Count >= 1 && SecondaryOutput.Count >= 1)
            {
                //! When the user presses a different operator than the "=" (still calculate result but keep the new operator)
                //! calculate the result then show result in the SecondaryInput
                string leftExpr = string.Join("", SecondaryOutput);
                string rightExpr = string.Join("", MainOutput);
                Expression expr = new Expression(leftExpr + rightExpr);

                double result = expr.calculate();

                DeconstructResult(result, OutputTextBlockEnum.Secondary, (true, null, mathOperator?[0]));
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab: break;
                case Key.Enter: HandleNewOperator("="); break;
                case Key.Escape: break;
                case Key.Space: break;

                case Key.Back: InputControlBackSpace(); break;

                case Key.Left: break;
                case Key.Up: break;
                case Key.Right: break;
                case Key.Down: break;

                case Key.D0: AddToMainOutputText("0"); break;
                case Key.D1: AddToMainOutputText("1"); break;
                case Key.D2: AddToMainOutputText("2"); break;
                case Key.D3: AddToMainOutputText("3"); break;
                case Key.D4: AddToMainOutputText("4"); break;
                case Key.D5: AddToMainOutputText("5"); break;
                case Key.D6: AddToMainOutputText("6"); break;
                case Key.D7: AddToMainOutputText("7"); break;
                case Key.D8: AddToMainOutputText("8"); break;
                case Key.D9: AddToMainOutputText("9"); break;

                case Key.NumPad0: AddToMainOutputText("0"); break;
                case Key.NumPad1: AddToMainOutputText("1"); break;
                case Key.NumPad2: AddToMainOutputText("2"); break;
                case Key.NumPad3: AddToMainOutputText("3"); break;
                case Key.NumPad4: AddToMainOutputText("4"); break;
                case Key.NumPad5: AddToMainOutputText("5"); break;
                case Key.NumPad6: AddToMainOutputText("6"); break;
                case Key.NumPad7: AddToMainOutputText("7"); break;
                case Key.NumPad8: AddToMainOutputText("8"); break;
                case Key.NumPad9: AddToMainOutputText("9"); break;

                case Key.Multiply: HandleNewOperator("*"); break;
                case Key.Add: HandleNewOperator("+"); break;
                case Key.Subtract: HandleNewOperator("-"); break;
                case Key.OemPeriod: HandleNewOperator("."); break;
                case Key.Divide: HandleNewOperator("/"); break;
                default: break;
            }


        }

        #region Not Implemented Yet
        private void HistoryButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This History Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
        private void HamburgerMenuButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This Hamburger Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
        private void MathFunctionButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show($"This {(sender as Button)?.Name} is not implemented yet", $"{(sender as Button)?.Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
        #endregion
    }
}
