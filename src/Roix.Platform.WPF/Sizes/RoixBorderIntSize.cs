using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderIntSize
    {
        readonly struct SourceValues
        {
            public readonly RoixIntSize Size;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntSize size, in RoixIntSize border) => (Size, Border) = (size, border);
        }

        private RoixIntSize Value => _values.Size;

        #region ctor
        private partial void Validate(in RoixBorderIntSize value)
        {
            if (value.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }
        #endregion

        #region implicit
        public static implicit operator RoixBorderSize(in RoixBorderIntSize borderSize) => new(borderSize.Size, borderSize.Border);
        #endregion

        #region explicit
        public static explicit operator RoixBorderIntSize(in RoixBorderSize borderSize) => new((RoixIntSize)borderSize.Size, (RoixIntSize)borderSize.Border);
        #endregion

    }
}
