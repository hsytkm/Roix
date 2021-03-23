using System;

namespace Roix.SourceGenerator
{
    // same as Generated Options(check RoixStructGeneratorAttributeTemplate.tt).
    [Flags]
    internal enum RoixStructGeneratorOptions
    {
        None = 0x0000,

        // added automatically
        XYPair = 0x0001,
        Rect = 0x0002,
        WithBorder = 0x0004,
        TypeInt = 0x0008,
        HasParent = 0x0010,
        HasEmpty = 0x0020,
        TypeLine = 0x0040,
        // added automatically

        //Flag8 = 0x0080,

        Validate = 0x0100,
        ArithmeticOperator1 = 0x0200,
        ArithmeticOperator2 = 0x0400,

        //Flag12 = 0x0800,
        //Flag13 = 0x1000,
        //Flag14 = 0x2000,
        //Flag15 = 0x4000,
        //Flag16 = 0x8000,
    }
}
