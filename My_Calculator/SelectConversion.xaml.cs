using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using My_Calculator.Helpers.Enums;

namespace My_Calculator
{
    /// <summary>
    /// Interaction logic for SelectConversion.xaml
    /// </summary>
    public partial class SelectConversion : Window
    {
        public SelectConversion()
        {
            InitializeComponent();
            BuildConversionTypeButtons();

            App.Current.MainWindow.WindowState = WindowState.Minimized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;

            // Open MainWindow when SelectConversion (this) is closed
            Closed += (obj, e) => App.Current.MainWindow.WindowState = WindowState.Normal;

            Show();
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

        //~SelectConversion()
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
                        case ConversionTypeEnum.None: App.Current.MainWindow.WindowState = WindowState.Normal; Hide(); Close(); break;
                        case ConversionTypeEnum.NumeralSystem: break;
                        case ConversionTypeEnum.PercentToAndFromDecimal: break;
                        case ConversionTypeEnum.Weight: break;
                        case ConversionTypeEnum.Length: break;
                        case ConversionTypeEnum.Temperature: break;
                        case ConversionTypeEnum.FileSize: break;
                        case ConversionTypeEnum.Time: break;
                        default: break;
                    }
                }
            }
        }

    }
}
