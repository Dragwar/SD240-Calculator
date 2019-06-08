using System;
using My_Calculator.Helpers.Enums;
using org.mariuszgromada.math.mxparser;

namespace My_Calculator.Helpers
{
    public class WeightConversion : IWeightConversion, IUnitConversion<WeightConversion, double, WeightTypeEnum>
    {
        public double ValueToConvert { get; private set; }
        public WeightTypeEnum CurrentUnitType { get; private set; }

        public double Milligrams => Convert(ValueToConvert, CurrentUnitType, WeightTypeEnum.Milligrams);
        public double Grams => Convert(ValueToConvert, CurrentUnitType, WeightTypeEnum.Grams);
        public double Kilograms => Convert(ValueToConvert, CurrentUnitType, WeightTypeEnum.Kilograms);
        public double Ounces => Convert(ValueToConvert, CurrentUnitType, WeightTypeEnum.Ounces);
        public double Pounds => Convert(ValueToConvert, CurrentUnitType, WeightTypeEnum.Pounds);

        public WeightConversion(double initialValueToConvert, WeightTypeEnum initialUnitType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentUnitType = initialUnitType;
        }
        public WeightConversion SetValueToConvert(double newValue, WeightTypeEnum newValueUnitType)
        {
            ValueToConvert = newValue;
            CurrentUnitType = newValueUnitType;
            return this;
        }

        public double Convert(double valueToConvert, WeightTypeEnum valueUnitType, WeightTypeEnum toConvertTo)
        {
            if (valueUnitType == toConvertTo)
            {
                return valueToConvert;
            }

            const string value = "value";
            const string valueType = "valueType";
            const string toType = "toType";

            Expression convertLength = new Expression($"({value} * {valueType}) / {toType}");
            convertLength.addArguments(new Argument(value, valueToConvert));

            string mXparserValueUnitType;
            switch (valueUnitType)
            {
                case WeightTypeEnum.Milligrams: mXparserValueUnitType = "[mg]"; break;
                case WeightTypeEnum.Grams: mXparserValueUnitType = "[gr]"; break;
                case WeightTypeEnum.Kilograms: mXparserValueUnitType = "[kg]"; break;
                case WeightTypeEnum.Ounces: mXparserValueUnitType = "[oz]"; break;
                case WeightTypeEnum.Pounds: mXparserValueUnitType = "[lb]"; break;
                default: throw new ArgumentException($"{nameof(WeightTypeEnum)} doesn't support: {valueUnitType}");
            }
            convertLength.addArguments(new Argument(valueType, mXparserValueUnitType));


            string mXparserToConvertTo;
            switch (toConvertTo)
            {
                case WeightTypeEnum.Milligrams: mXparserToConvertTo = "[mg]"; break;
                case WeightTypeEnum.Grams: mXparserToConvertTo = "[gr]"; break;
                case WeightTypeEnum.Kilograms: mXparserToConvertTo = "[kg]"; break;
                case WeightTypeEnum.Ounces: mXparserToConvertTo = "[oz]"; break;
                case WeightTypeEnum.Pounds: mXparserToConvertTo = "[lb]"; break;
                default: throw new ArgumentException($"{nameof(WeightTypeEnum)} doesn't support: {toConvertTo}");
            }
            convertLength.addArguments(new Argument(toType, mXparserToConvertTo));

            return convertLength.calculate();
        }
    }
}
