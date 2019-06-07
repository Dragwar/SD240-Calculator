using System;
using My_Calculator.Helpers.Enums;

namespace My_Calculator.Helpers
{
    public class TemperatureConversion
    {
        public const string SampleFormulaCelsiusToFahrenheit = "(21°C × 9/5) + 32 = 69.8°F";
        public const string SampleFormulaFahrenheitToCelsius = "(21°F − 32) × 5/9 = -6.111°C";

        public double ValueToConvert { get; private set; }
        public TemperatureTypeEnum CurrentTemperatureType { get; private set; }
        public double Celsius => ConvertTemperature(ValueToConvert, CurrentTemperatureType, TemperatureTypeEnum.Celsius);
        public double Fahrenheit => ConvertTemperature(ValueToConvert, CurrentTemperatureType, TemperatureTypeEnum.Fahrenheit);

        public TemperatureConversion(double initialValueToConvert, TemperatureTypeEnum initialTemperatureType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentTemperatureType = initialTemperatureType;
        }

        public TemperatureConversion SetValueToConvert(double newValueToConvert, TemperatureTypeEnum newTemperatureType)
        {
            ValueToConvert = newValueToConvert;
            CurrentTemperatureType = newTemperatureType;
            return this;
        }

        public static double ConvertTemperature(double valueToConvert, TemperatureTypeEnum valueToConvertTemperatureType, TemperatureTypeEnum convertTo)
        {
            double fahrenheit = 0;
            double celsius = 0;

            switch (valueToConvertTemperatureType)
            {
                case TemperatureTypeEnum.Celsius: fahrenheit = (valueToConvert * (9.00 / 5.00)) + 32.00; break;
                case TemperatureTypeEnum.Fahrenheit: celsius = (valueToConvert - 32.00) * (5.00 / 9.00); break;
                default: throw new ArgumentException($"TemperatureTypeEnum doesn't support: {valueToConvertTemperatureType}");
            }

            switch (convertTo)
            {
                case TemperatureTypeEnum.Celsius when valueToConvertTemperatureType == convertTo: return valueToConvert;
                case TemperatureTypeEnum.Fahrenheit when valueToConvertTemperatureType == convertTo: return valueToConvert;
                case TemperatureTypeEnum.Celsius: return celsius;
                case TemperatureTypeEnum.Fahrenheit: return fahrenheit;
                default: throw new NotImplementedException($"TemperatureTypeEnum doesn't support: {convertTo}");
            }
        }
    }
}
