using System;

namespace Roix.SourceGenerator
{
    // same as Generated Options(check RoixStructGeneratorAttributeTemplate.tt).
    [Flags]
    internal enum RoixStructGeneratorOptions
    {
        None = 0,
        XYPair = 1,
        WithBorder = 2,
        TypeInt = 4,
        Validate = 8,
        ArithmeticOperator1 = 16,
        ArithmeticOperator2 = 32,
        E7 = 64,
        E8 = 128,
        E9 = 256,
    }
}
