using System;
using My_Calculator.Helpers.Enums;

namespace My_Calculator.Helpers
{
    public class TemperatureConversion : ITemperatureConversion, IUnitConversion<TemperatureConversion, double, TemperatureTypeEnum>
    {
        public const string SampleFormulaCelsiusToFahrenheit = "(21°C × 9/5) + 32 = 69.8°F";
        public const string SampleFormulaFahrenheitToCelsius = "(21°F − 32) × 5/9 = -6.111°C";

        public double ValueToConvert { get; private set; }
        public TemperatureTypeEnum CurrentUnitType { get; private set; }
        public double Celsius => Convert(ValueToConvert, CurrentUnitType, TemperatureTypeEnum.Celsius);
        public double Fahrenheit => Convert(ValueToConvert, CurrentUnitType, TemperatureTypeEnum.Fahrenheit);


        public TemperatureConversion(double initialValueToConvert, TemperatureTypeEnum initialTemperatureType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentUnitType = initialTemperatureType;
        }

        public TemperatureConversion SetValueToConvert(double newValueToConvert, TemperatureTypeEnum newTemperatureType)
        {
            ValueToConvert = newValueToConvert;
            CurrentUnitType = newTemperatureType;
            return this;
        }

        public double Convert(double valueToConvert, TemperatureTypeEnum valueToConvertTemperatureType, TemperatureTypeEnum toConvertTo)
        {
            if (valueToConvertTemperatureType == toConvertTo)
            {
                return valueToConvert;
            }

            double fahrenheit = 0;
            double celsius = 0;

            switch (valueToConvertTemperatureType)
            {
                case TemperatureTypeEnum.Celsius: fahrenheit = (valueToConvert * (9.00 / 5.00)) + 32.00; break;
                case TemperatureTypeEnum.Fahrenheit: celsius = (valueToConvert - 32.00) * (5.00 / 9.00); break;
                default: throw new ArgumentException($"{nameof(TemperatureTypeEnum)} doesn't support: {valueToConvertTemperatureType}");
            }

            switch (toConvertTo)
            {
                case TemperatureTypeEnum.Celsius: return celsius;
                case TemperatureTypeEnum.Fahrenheit: return fahrenheit;
                default: throw new NotImplementedException($"{nameof(TemperatureTypeEnum)} doesn't support: {toConvertTo}");
            }
        }
    }
}
