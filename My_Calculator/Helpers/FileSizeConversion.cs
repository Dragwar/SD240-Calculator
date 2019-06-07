using System;
using My_Calculator.Helpers.Enums;
using org.mariuszgromada.math.mxparser;

namespace My_Calculator.Helpers
{
    public class FileSizeConversion
    {
        public double ValueToConvert { get; private set; }
        public FileSizeTypeEnum CurrentFileSizeType { get; private set; }

        public double Bits => ConvertToFileSizeType(ValueToConvert, CurrentFileSizeType, FileSizeTypeEnum.Bit);
        public double Bytes => ConvertToFileSizeType(ValueToConvert, CurrentFileSizeType, FileSizeTypeEnum.Bytes);
        public double Kilobytes => ConvertToFileSizeType(ValueToConvert, CurrentFileSizeType, FileSizeTypeEnum.Kilobytes);
        public double Megabytes => ConvertToFileSizeType(ValueToConvert, CurrentFileSizeType, FileSizeTypeEnum.Megabytes);
        public double Gigabytes => ConvertToFileSizeType(ValueToConvert, CurrentFileSizeType, FileSizeTypeEnum.Gigabytes);
        public double Terabytes => ConvertToFileSizeType(ValueToConvert, CurrentFileSizeType, FileSizeTypeEnum.Terabytes);
        public double Petabytes => ConvertToFileSizeType(ValueToConvert, CurrentFileSizeType, FileSizeTypeEnum.Petabytes);

        public FileSizeConversion(double initialValueToConvert, FileSizeTypeEnum currentFileSizeType)
        {
            ValueToConvert = initialValueToConvert;
            CurrentFileSizeType = currentFileSizeType;
        }

        public FileSizeConversion SetValueToConvert(double newValueToConvert, FileSizeTypeEnum newFileSizeType)
        {
            ValueToConvert = newValueToConvert;
            CurrentFileSizeType = newFileSizeType;
            return this;
        }

        public static double ConvertToFileSizeType(double valueToConvert, FileSizeTypeEnum valueToConvertFileSizeType, FileSizeTypeEnum toConvertTo)
        {
            Expression convertFileSize = new Expression("(value * valueSizeType) / toSizeType");
            convertFileSize.addArguments(new Argument("value", valueToConvert));

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
                default: throw new ArgumentException($"FileSizeTypeEnum doesn't support: {valueToConvertFileSizeType}");
            }
            convertFileSize.addArguments(new Argument("valueSizeType", mXparserValueToConvertFileSizeType));

            string mXparserToConvertToFileSizeType;
            switch (toConvertTo)
            {
                case FileSizeTypeEnum.Bit when valueToConvertFileSizeType == FileSizeTypeEnum.Bit: return valueToConvert;
                case FileSizeTypeEnum.Bytes when valueToConvertFileSizeType == FileSizeTypeEnum.Bytes: return valueToConvert;
                case FileSizeTypeEnum.Kilobytes when valueToConvertFileSizeType == FileSizeTypeEnum.Kilobytes: return valueToConvert;
                case FileSizeTypeEnum.Megabytes when valueToConvertFileSizeType == FileSizeTypeEnum.Megabytes: return valueToConvert;
                case FileSizeTypeEnum.Gigabytes when valueToConvertFileSizeType == FileSizeTypeEnum.Gigabytes: return valueToConvert;
                case FileSizeTypeEnum.Terabytes when valueToConvertFileSizeType == FileSizeTypeEnum.Terabytes: return valueToConvert;
                case FileSizeTypeEnum.Petabytes when valueToConvertFileSizeType == FileSizeTypeEnum.Petabytes: return valueToConvert;

                case FileSizeTypeEnum.Bit: mXparserToConvertToFileSizeType = "[b]"; break;
                case FileSizeTypeEnum.Bytes: mXparserToConvertToFileSizeType = "[B]"; break;
                case FileSizeTypeEnum.Kilobytes: mXparserToConvertToFileSizeType = "[kB]"; break;
                case FileSizeTypeEnum.Megabytes: mXparserToConvertToFileSizeType = "[MB]"; break;
                case FileSizeTypeEnum.Gigabytes: mXparserToConvertToFileSizeType = "[GB]"; break;
                case FileSizeTypeEnum.Terabytes: mXparserToConvertToFileSizeType = "[TB]"; break;
                case FileSizeTypeEnum.Petabytes: mXparserToConvertToFileSizeType = "[PB]"; break;
                default: throw new ArgumentException($"FileSizeTypeEnum doesn't support: {toConvertTo}");
            }
            convertFileSize.addArguments(new Argument("toSizeType", mXparserToConvertToFileSizeType));

            return convertFileSize.calculate();
        }
    }
}
