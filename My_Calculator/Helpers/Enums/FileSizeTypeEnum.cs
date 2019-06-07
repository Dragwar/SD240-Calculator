using System;

namespace My_Calculator.Helpers.Enums
{
    [Flags]
    public enum FileSizeTypeEnum : long
    {
        Bit = 1,
        Bytes = 8,
        Kilobytes = 8_192,
        Megabytes = 8_388_608,
        Gigabytes = 8_589_934_592,
        Terabytes = 8_796_093_022_208,
        Petabytes = 9_007_199_254_740_992
    }
}