using System;
using My_Calculator.Helpers.Enums;

namespace My_Calculator.Helpers
{
    public class TimeConversion
    {
        public double ValueToConvert { get; private set; }
        public TimeTypeEnum CurrentTimeType { get; private set; }
        public double Hours => ConvertToTimeType(ValueToConvert, CurrentTimeType, TimeTypeEnum.Hours);
        public double Minutes => ConvertToTimeType(ValueToConvert, CurrentTimeType, TimeTypeEnum.Minutes);
        public double Seconds => ConvertToTimeType(ValueToConvert, CurrentTimeType, TimeTypeEnum.Seconds);

        public TimeConversion(double initialValueToConvert, TimeTypeEnum currentTimeType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentTimeType = currentTimeType;
        }

        public TimeConversion SetValueToConvert(double newValueToConvert, TimeTypeEnum newTimeType)
        {
            ValueToConvert = newValueToConvert;
            CurrentTimeType = newTimeType;            
            return this;
        }

        public static double ConvertToTimeType(double valueToConvert, TimeTypeEnum valueToConvertTimeType, TimeTypeEnum toConvertTo)
        {
            TimeSpan timeSpan;
            switch (valueToConvertTimeType)
            {
                case TimeTypeEnum.Seconds: timeSpan = TimeSpan.FromSeconds(valueToConvert); break;
                case TimeTypeEnum.Minutes: timeSpan = TimeSpan.FromMinutes(valueToConvert); break;
                case TimeTypeEnum.Hours: timeSpan = TimeSpan.FromHours(valueToConvert); break;
                default: throw new ArgumentException($"TimeEnum doesn't support: {valueToConvertTimeType}");
            }

            switch (toConvertTo)
            {
                case TimeTypeEnum.Seconds: return timeSpan.TotalSeconds;
                case TimeTypeEnum.Minutes: return timeSpan.TotalMinutes;
                case TimeTypeEnum.Hours: return timeSpan.TotalHours;
                default: throw new ArgumentException($"TimeEnum doesn't support: {toConvertTo}");
            }
        }
    }
}
