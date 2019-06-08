using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using My_Calculator.ConversionWindows;
using My_Calculator.Helpers;
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
                if (App.Current.MainWindow != null)
                {
                    App.Current.MainWindow.WindowState = WindowState.Normal;
                }
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

                        case ConversionTypeEnum.NumeralSystem: OpenConversionWindow<NumeralSystemConversionWindow>(); break;
                        case ConversionTypeEnum.Weight: break;
                        case ConversionTypeEnum.Length: OpenConversionWindow<LengthConversionWindow>(); break;
                        case ConversionTypeEnum.Temperature: OpenConversionWindow<TemperatureConversionWindow>(); break;
                        case ConversionTypeEnum.FileSize: OpenConversionWindow<FileSizeConversionWindow>(); break;
                        case ConversionTypeEnum.Time: OpenConversionWindow<TimeConversionWindow>(); break;
                        default: throw new NotImplementedException("ConversionType is not found");
                    }
                }
            }
        }

        private void OpenConversionWindow<TConversionWindow>()
            where TConversionWindow : Window, new()
        {
            List<TConversionWindow> list = App.Current.Windows
                .OfType<TConversionWindow>()
                .ToList();

            TConversionWindow window = list.Any() ? list.First() : new TConversionWindow();
            Close();
            window.Activate();
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

        public static void CloseAllConversionWindowsAndOpenSelectConversionWindow()
        {
            List<Window> conversionWindowList = App.Current.Windows.OfType<Window>()
               .Where(window => window.GetType() != typeof(MainWindow) && window.Title.ToLower().Contains("conversion"))
               .ToList();

            SelectConversionWindow foundWindow = conversionWindowList
                .FirstOrDefault(window => window.GetType() == typeof(SelectConversionWindow)) as SelectConversionWindow;

            if (foundWindow is null)
            {
                foundWindow = new SelectConversionWindow();
            }

            foundWindow.Activate();

            if (conversionWindowList.Any())
            {
                conversionWindowList.Remove(foundWindow);
                conversionWindowList.ForEach(window =>
                {
                    window.Hide();
                    window.Close();
                });
            }
        }
    }
}
