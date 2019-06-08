using System;

namespace My_Calculator.Helpers
{
    public interface IUnitConversion<TConversionClassType, TValueDataType, TValueUnitTypeEnum>
        where TConversionClassType : class
        where TValueDataType : struct
        where TValueUnitTypeEnum : Enum
    {
        TValueDataType ValueToConvert { get; }
        TValueUnitTypeEnum CurrentUnitType { get; }
        TConversionClassType SetValueToConvert(TValueDataType value, TValueUnitTypeEnum valueUnitType);
        TValueDataType Convert(TValueDataType value, TValueUnitTypeEnum valueUnitType, TValueUnitTypeEnum toConvertTo);
    }
}
