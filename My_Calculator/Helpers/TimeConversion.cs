using System;
using My_Calculator.Helpers.Enums;

namespace My_Calculator.Helpers
{
    public class TimeConversion : ITimeConversion, IUnitConversion<TimeConversion, double, TimeTypeEnum>
    {
        public double ValueToConvert { get; private set; }
        public TimeTypeEnum CurrentUnitType { get; private set; }
        public double Hours => Convert(ValueToConvert, CurrentUnitType, TimeTypeEnum.Hours);
        public double Minutes => Convert(ValueToConvert, CurrentUnitType, TimeTypeEnum.Minutes);
        public double Seconds => Convert(ValueToConvert, CurrentUnitType, TimeTypeEnum.Seconds);

        public TimeConversion(double initialValueToConvert, TimeTypeEnum currentTimeType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentUnitType = currentTimeType;
        }

        public TimeConversion SetValueToConvert(double newValueToConvert, TimeTypeEnum newTimeType)
        {
            ValueToConvert = newValueToConvert;
            CurrentUnitType = newTimeType;
            return this;
        }

        public double Convert(double valueToConvert, TimeTypeEnum valueToConvertTimeType, TimeTypeEnum toConvertTo)
        {
            if (valueToConvertTimeType == toConvertTo)
            {
                return valueToConvert;
            }

            TimeSpan timeSpan;
            switch (valueToConvertTimeType)
            {
                case TimeTypeEnum.Seconds: timeSpan = TimeSpan.FromSeconds(valueToConvert); break;
                case TimeTypeEnum.Minutes: timeSpan = TimeSpan.FromMinutes(valueToConvert); break;
                case TimeTypeEnum.Hours: timeSpan = TimeSpan.FromHours(valueToConvert); break;
                default: throw new ArgumentException($"{nameof(TimeTypeEnum)} doesn't support: {valueToConvertTimeType}");
            }

            switch (toConvertTo)
            {
                case TimeTypeEnum.Seconds: return timeSpan.TotalSeconds;
                case TimeTypeEnum.Minutes: return timeSpan.TotalMinutes;
                case TimeTypeEnum.Hours: return timeSpan.TotalHours;
                default: throw new ArgumentException($"{nameof(TimeTypeEnum)} doesn't support: {toConvertTo}");
            }
        }
    }
}
