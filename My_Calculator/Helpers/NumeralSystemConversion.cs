using System;
using My_Calculator.Helpers.Enums;

namespace My_Calculator.Helpers
{
    public class NumeralSystemConversion
    {
        public int NumberToConvert { get; set; }
        public string Hexadecimal => Convert.ToString(NumberToConvert, 16); // or NumberToConvert.ToString("X");
        public string Octal => Convert.ToString(NumberToConvert, 8);
        public string Binary => Convert.ToString(NumberToConvert, 2);


        public NumeralSystemConversion(int numberToConvert) => NumberToConvert = numberToConvert;


        public int ConvertToInt(NumeralSystemEnum numeralSystem, string valueToConvert)
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
