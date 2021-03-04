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
    }
}
