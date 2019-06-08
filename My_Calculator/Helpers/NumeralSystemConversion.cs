using System;
using My_Calculator.Helpers.Enums;

namespace My_Calculator.Helpers
{
    public class NumeralSystemConversion : INumeralSystemConversion
    {
        public int NumberToConvert { get; set; }
        public string Hexadecimal => Convert.ToString(NumberToConvert, 16); // or NumberToConvert.ToString("X");
        public string Octal => Convert.ToString(NumberToConvert, 8);
        public string Binary => Convert.ToString(NumberToConvert, 2);


        public NumeralSystemConversion(int numberToConvert) => NumberToConvert = numberToConvert;


        public static double ConvertToPercent(double number) => number * 100;
        public static decimal ConvertToPercent(decimal number) => number * 100;
        public static double ConvertToDecimal(double percent) => percent / 100;
        public static decimal ConvertToDecimal(decimal percent) => percent / 100;


        public static int ConvertToInt(NumeralSystemEnum numeralSystem, string valueToConvert)
        {
            try
            {
                switch (numeralSystem)
                {
                    case NumeralSystemEnum.Binary: return Convert.ToInt32(valueToConvert, 2);
                    case NumeralSystemEnum.Octal: return Convert.ToInt32(valueToConvert, 8);
                    case NumeralSystemEnum.Integer: return Convert.ToInt32(valueToConvert, 10);
                    case NumeralSystemEnum.Hexadecimal: return Convert.ToInt32(valueToConvert, 16);
                    default: return default;
                }
            }
            catch
            {
                return default;
            }
        }
    }
}
