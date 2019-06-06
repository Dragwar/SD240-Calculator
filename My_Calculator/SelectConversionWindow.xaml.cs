using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using My_Calculator.ConversionWindows;
using My_Calculator.Helpers.Enums;

namespace My_Calculator
{
    /// <summary>
    /// Interaction logic for SelectConversionWindow.xaml
    /// </summary>
    public partial class SelectConversionWindow : Window
    {
        public SelectConversionWindow()
        {
            InitializeComponent();
            BuildConversionTypeButtons();

            App.Current.MainWindow.WindowState = WindowState.Minimized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;

            Closed += SelectConversionWindow_Closed;

            Show();
        }

        private void SelectConversionWindow_Closed(object sender, EventArgs e)
        {
            List<Window> conversionWindowList = App.Current.Windows.OfType<Window>()
               .Where(window => window.GetType() != typeof(MainWindow) && window.Title.ToLower().Contains("conversion"))
               .ToList();

            if (!conversionWindowList.Any())
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void BuildConversionTypeButtons()
        {
            List<string> conversionTypes = Enum.GetNames(typeof(ConversionTypeEnum))
                .Cast<string>()
                .ToList();

            foreach (string convertType in conversionTypes)
            {
                Button btn = new Button
                {
                    Content = convertType,
                    Margin = conversionTypes.Last() != convertType ? new Thickness(0, 0, 0, 5) : new Thickness(0),
                    Style = App.Current.Resources["InputNumberButton"] as Style,
                };

                btn.Click += ConvertTypeButton_Click;

                MainStackPanel.Children.Add(btn);
            }
        }

        //~SelectConversionWindow()
        //{
        //    //MainStackPanel.Children
        //    //    .OfType<Button>()
        //    //    .ToList()
        //    //    .ForEach(btn => btn.Click -= ConvertTypeButton_Click);
        //}

        private void ConvertTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (Enum.TryParse(btn.Content as string, out ConversionTypeEnum selectedType))
                {
                    switch (selectedType)
                    {
                        case ConversionTypeEnum.BasicCalculator: CloseAllConversionWindowsAndOpenMainWindow(); return;

                        case ConversionTypeEnum.NumeralSystem:
                            List<NumeralSystemConversionWindow> list = App.Current.Windows
                                .OfType<NumeralSystemConversionWindow>()
                                .ToList();

                            NumeralSystemConversionWindow window = list.Any() ? list.First() : new NumeralSystemConversionWindow();
                            Close();
                            window.Activate();
                            break;
                        case ConversionTypeEnum.Weight: break;
                        case ConversionTypeEnum.Length: break;
                        case ConversionTypeEnum.Temperature: break;
                        case ConversionTypeEnum.FileSize: break;
                        case ConversionTypeEnum.Time: break;
                        default: throw new NotImplementedException("ConversionType is not found");
                    }
                }
            }
        }

        private void CloseAllConversionWindowsAndOpenMainWindow()
        {
            List<Window> conversionWindowList = App.Current.Windows.OfType<Window>()
                .Where(window => window.GetType() != typeof(MainWindow) && window.Title.ToLower().Contains("conversion"))
                .ToList();

            conversionWindowList.ForEach(window =>
            {
                window.Hide();
                window.Close();
            });

            App.Current.MainWindow.WindowState = WindowState.Normal;
        }
    }
}
