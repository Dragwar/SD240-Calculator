using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using My_Calculator.Helpers.Enums;
using org.mariuszgromada.math.mxparser.mathcollection;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace My_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ButtonString = "Button";
        private readonly List<char> MainInputAllowedCharacters = new List<char>() { '-', '.', '+', 'E' };
        private readonly List<char> SecondaryInputAllowedCharacters = new List<char>() { '-', '.', '+', '*', '/', 'E' };


        private List<string> MainOutput { get; set; }
        private List<string> SecondaryOutput { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainOutput = new List<string>();
            SecondaryOutput = new List<string>();
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
        private void RemoveFromSecondaryOutputTextAndUpdate([Optional] int? index)
        {
            if (SecondaryOutput != null && SecondaryOutput.Any())
            {
                SecondaryOutput.RemoveAt(index ?? (SecondaryOutput.Count - 1));
                SecondaryOutputText.Text = string.Join("", SecondaryOutput);
            }
        }
        private void AddToSecondaryOutputTextAndUpdate(string str)
        {
            SecondaryOutput.Add(str);
            SecondaryOutputText.Text = string.Join("", SecondaryOutput);
        }
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
            double result,
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
                if (MainOutput.Any())
                {
                    string main = string.Join("", MainOutput);
                    Expression expr = new Expression(main);
                    double result = expr.calculate();
                    result *= -1; // toggle to positive/negative

                    // Display Result
                    try
                    {
                        _ = DeconstructResultAndDisplay(result, OutputTextBlockEnum.Main, (false, null, null));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, $"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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

                // Display Result
                try
                {
                    _ = DeconstructResultAndDisplay(result, OutputTextBlockEnum.Main, (true, null, null));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, $"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (MainOutput.Any() && !SecondaryOutput.Any() && mathOperator != ".")
            {
                TransferMainToSecondaryOutputWithOperator(mathOperator);
            }
            else if (MainOutput.Any() && SecondaryOutput.Any())
            {
                //! When the user presses a different operator than the "=" (still calculate result but keep the new operator)
                //! calculate the result then show result in the SecondaryInput
                string leftExpr = string.Join("", SecondaryOutput);
                string rightExpr = string.Join("", MainOutput);
                Expression expr = new Expression(leftExpr + rightExpr);

                double result = expr.calculate();

                try
                {
                    string output = DeconstructResultAndDisplay(result, OutputTextBlockEnum.Secondary, (true, null, mathOperator?[0]));
                    if (output != (result.ToString() + mathOperator))
                    {
                        throw new Exception("Generated incorrect output");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, $"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Add the operator to the end then show expression in the SecondaryOutput
        /// <para/>Example:
        /// (MainOutput          --> "24" -> "24+" -> "")
        /// (SecondaryOutput     --> ""   -> ""    -> "24+")
        /// <para/>"now waiting for right hand side of the expression"
        /// </summary>
        /// <param name="mathOperator">Not checked to be a valid operator inside please check before invoking</param>
        private void TransferMainToSecondaryOutputWithOperator(string mathOperator)
        {
            SecondaryOutput = new List<string>(MainOutput) { mathOperator as string };
            MainOutput.Clear();

            SecondaryOutputText.Text = string.Join("", SecondaryOutput);
            MainOutputText.Text = "";
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
                //case Key.Tab: break;
                case Key.Enter: HandleNewOperator("="); break;
                //case Key.Escape: break;
                //case Key.Space: break;

                case Key.Back: InputControlBackSpace(); break;

                //case Key.Left: break;
                //case Key.Up: break;
                //case Key.Right: break;
                //case Key.Down: break;

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

                case Key.Multiply: HandleNewOperator("*"); break;
                case Key.Add: HandleNewOperator("+"); break;
                case Key.Subtract: HandleNewOperator("-"); break;
                case Key.OemPeriod: HandleNewOperator("."); break;
                case Key.Divide: HandleNewOperator("/"); break;
                default: break;
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
        private void MathFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string btnTag = btn.Tag as string;
                bool validMathFunc = Enum.TryParse(btnTag, true, out MathFunctionEnum mathFunc);
                if (validMathFunc)
                {
                    switch (mathFunc)
                    {
                        case MathFunctionEnum.SquareRoot: SquareRoot(); break;

                        case MathFunctionEnum.Factorial: Factorial(); break;

                        case MathFunctionEnum.Exponent:

                            break;

                        case MathFunctionEnum.Modulus:

                            break;

                        default: throw new Exception($"{btnTag} is not recognized as a math Function");
                    }
                }
            }
        }


        #region Math Functions
        private void SquareRoot()
        {
            string main = string.Join("", MainOutput);
            Expression expr = new Expression(main);
            if (!expr.checkSyntax())
            {
                return;
            }
            double result = expr.calculate(); // will parse string of numbers
            result = MathFunctions.sqrt(result);
            string secOutput = $"sqrt({main})";

            // show work in secondary output (DON'T UPDATE SecondaryOutput because you want the user to be able to operate on the MainOutput)
            SecondaryOutputText.Text = secOutput;

            // show result in main
            string output = DeconstructResultAndDisplay(result, OutputTextBlockEnum.Main, (false, null, null));
            if (output != result.ToString())
            {
                throw new Exception($"{nameof(SquareRoot)} calculation error: did not match expected result ({output} != {result})");
            }
        }
        private void Factorial()
        {
            string stringExpression = string.Join("", MainOutput);
            Expression expr = new Expression(stringExpression);
            if (!expr.checkSyntax())
            {
                return;
            }
            double result = expr.calculate(); // will parse the string of numbers
            result = MathFunctions.factorial(result);

            string output = DeconstructResultAndDisplay(result, OutputTextBlockEnum.Main, (true, null, null));
            if (output == "")
            {
                MainOutput.Clear();
                MainOutputText.Text = result.ToString();
            }
            else if (output != result.ToString() && Math.Floor(result).ToString() != Math.Floor(double.Parse(output)).ToString())
            {
                throw new Exception($"{nameof(Factorial)} calculation error: did not match expected result ({output} != {result})");
            }
            SecondaryOutputText.Text = stringExpression;
            SecondaryOutput.Clear();
        }
        #endregion


        #region Not Implemented Yet
        private void HistoryButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This History Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
        private void HamburgerMenuButton_Click(object sender, RoutedEventArgs e) => MessageBox
            .Show("This Hamburger Menu is not implemented yet", $"{(sender as Button).Name}", MessageBoxButton.OK, MessageBoxImage.Warning);
        #endregion
    }
}
