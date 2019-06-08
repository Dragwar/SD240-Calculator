using System;
using My_Calculator.Helpers.Enums;
using org.mariuszgromada.math.mxparser;

namespace My_Calculator.Helpers
{
    public class FileSizeConversion : IFileSizeConversion, IUnitConversion<FileSizeConversion, double, FileSizeTypeEnum>
    {
        public double ValueToConvert { get; private set; }
        public FileSizeTypeEnum CurrentUnitType { get; private set; }

        public double Bits => Convert(ValueToConvert, CurrentUnitType, FileSizeTypeEnum.Bit);
        public double Bytes => Convert(ValueToConvert, CurrentUnitType, FileSizeTypeEnum.Bytes);
        public double Kilobytes => Convert(ValueToConvert, CurrentUnitType, FileSizeTypeEnum.Kilobytes);
        public double Megabytes => Convert(ValueToConvert, CurrentUnitType, FileSizeTypeEnum.Megabytes);
        public double Gigabytes => Convert(ValueToConvert, CurrentUnitType, FileSizeTypeEnum.Gigabytes);
        public double Terabytes => Convert(ValueToConvert, CurrentUnitType, FileSizeTypeEnum.Terabytes);
        public double Petabytes => Convert(ValueToConvert, CurrentUnitType, FileSizeTypeEnum.Petabytes);


        public FileSizeConversion(double initialValueToConvert, FileSizeTypeEnum currentFileSizeType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentUnitType = currentFileSizeType;
        }

        public FileSizeConversion SetValueToConvert(double newValueToConvert, FileSizeTypeEnum newFileSizeType)
        {
            ValueToConvert = newValueToConvert;
            CurrentUnitType = newFileSizeType;
            return this;
        }

        public double Convert(double valueToConvert, FileSizeTypeEnum valueToConvertFileSizeType, FileSizeTypeEnum toConvertTo)
        {
            if (valueToConvertFileSizeType == toConvertTo)
            {
                return valueToConvert;
            }

            const string value = "value";
            const string valueSizeType = "valueSizeType";
            const string toSizeType = "toSizeType";

            Expression convertFileSize = new Expression($"({value} * {valueSizeType}) / {toSizeType}");
            convertFileSize.addArguments(new Argument(value, valueToConvert));

            string mXparserValueToConvertFileSizeType;
            switch (valueToConvertFileSizeType)
            {
                case FileSizeTypeEnum.Bit: mXparserValueToConvertFileSizeType = "[b]"; break;
                case FileSizeTypeEnum.Bytes: mXparserValueToConvertFileSizeType = "[B]"; break;
                case FileSizeTypeEnum.Kilobytes: mXparserValueToConvertFileSizeType = "[kB]"; break;
                case FileSizeTypeEnum.Megabytes: mXparserValueToConvertFileSizeType = "[MB]"; break;
                case FileSizeTypeEnum.Gigabytes: mXparserValueToConvertFileSizeType = "[GB]"; break;
                case FileSizeTypeEnum.Terabytes: mXparserValueToConvertFileSizeType = "[TB]"; break;
                case FileSizeTypeEnum.Petabytes: mXparserValueToConvertFileSizeType = "[PB]"; break;
                default: throw new ArgumentException($"{nameof(FileSizeTypeEnum)} doesn't support: {valueToConvertFileSizeType}");
            }
            convertFileSize.addArguments(new Argument(valueSizeType, mXparserValueToConvertFileSizeType));

            string mXparserToConvertToFileSizeType;
            switch (toConvertTo)
            {
                case FileSizeTypeEnum.Bit: mXparserToConvertToFileSizeType = "[b]"; break;
                case FileSizeTypeEnum.Bytes: mXparserToConvertToFileSizeType = "[B]"; break;
                case FileSizeTypeEnum.Kilobytes: mXparserToConvertToFileSizeType = "[kB]"; break;
                case FileSizeTypeEnum.Megabytes: mXparserToConvertToFileSizeType = "[MB]"; break;
                case FileSizeTypeEnum.Gigabytes: mXparserToConvertToFileSizeType = "[GB]"; break;
                case FileSizeTypeEnum.Terabytes: mXparserToConvertToFileSizeType = "[TB]"; break;
                case FileSizeTypeEnum.Petabytes: mXparserToConvertToFileSizeType = "[PB]"; break;
                default: throw new ArgumentException($"{nameof(FileSizeTypeEnum)} doesn't support: {toConvertTo}");
            }
            convertFileSize.addArguments(new Argument(toSizeType, mXparserToConvertToFileSizeType));

            return convertFileSize.calculate();
        }
    }
}
