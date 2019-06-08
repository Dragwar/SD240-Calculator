using System;
using My_Calculator.Helpers.Enums;
using org.mariuszgromada.math.mxparser;

namespace My_Calculator.Helpers
{
    public class LengthConversion : ILengthConversion, IUnitConversion<LengthConversion, double, LengthTypeEnum>
    {
        public double ValueToConvert { get; private set; }
        public LengthTypeEnum CurrentUnitType { get; private set; }

        public double Millimetres => Convert(ValueToConvert, CurrentUnitType, LengthTypeEnum.Millimetres);
        public double Centimeters => Convert(ValueToConvert, CurrentUnitType, LengthTypeEnum.Centimeters);
        public double Meters => Convert(ValueToConvert, CurrentUnitType, LengthTypeEnum.Meters);
        public double Kilometers => Convert(ValueToConvert, CurrentUnitType, LengthTypeEnum.Kilometers);
        public double Inches => Convert(ValueToConvert, CurrentUnitType, LengthTypeEnum.Inches);
        public double Feet => Convert(ValueToConvert, CurrentUnitType, LengthTypeEnum.Feet);

        public LengthConversion(double initialValueToConvert, LengthTypeEnum currentLengthType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentUnitType = currentLengthType;
        }

        public LengthConversion SetValueToConvert(double newValueToConvert, LengthTypeEnum newLengthType)
        {
            ValueToConvert = newValueToConvert;
            CurrentUnitType = newLengthType;
            return this;
        }

        public double Convert(double valueToConvert, LengthTypeEnum valueUnitType, LengthTypeEnum toConvertTo)
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
                case LengthTypeEnum.Millimetres: mXparserValueUnitType = "[mm]"; break;
                case LengthTypeEnum.Centimeters: mXparserValueUnitType = "[cm]"; break;
                case LengthTypeEnum.Meters: mXparserValueUnitType = "[m]"; break;
                case LengthTypeEnum.Kilometers: mXparserValueUnitType = "[km]"; break;
                case LengthTypeEnum.Inches: mXparserValueUnitType = "[inch]"; break;
                case LengthTypeEnum.Feet: mXparserValueUnitType = "[ft]"; break;
                default: throw new ArgumentException($"{nameof(LengthTypeEnum)} doesn't support: {valueUnitType}");
            }
            convertLength.addArguments(new Argument(valueType, mXparserValueUnitType));


            string mXparserToConvertTo;
            switch (toConvertTo)
            {
                case LengthTypeEnum.Millimetres: mXparserToConvertTo = "[mm]"; break;
                case LengthTypeEnum.Centimeters: mXparserToConvertTo = "[cm]"; break;
                case LengthTypeEnum.Meters: mXparserToConvertTo = "[m]"; break;
                case LengthTypeEnum.Kilometers: mXparserToConvertTo = "[km]"; break;
                case LengthTypeEnum.Inches: mXparserToConvertTo = "[inch]"; break;
                case LengthTypeEnum.Feet: mXparserToConvertTo = "[ft]"; break;
                default: throw new ArgumentException($"{nameof(LengthTypeEnum)} doesn't support: {toConvertTo}");
            }
            convertLength.addArguments(new Argument(toType, mXparserToConvertTo));


            return convertLength.calculate();
        }
    }
}
