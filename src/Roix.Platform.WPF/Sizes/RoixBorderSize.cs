using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderSize
    {
        readonly struct SourceValues
        {
            public readonly RoixSize Size;
            public readonly RoixSize Border;
            public SourceValues(in RoixSize size, in RoixSize border) => (Size, Border) = (size, border);
        }

        private RoixSize Value => _values.Size;

        #region ctor
        private partial void Validate(in RoixBorderSize value)
        {
            if (value.Border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        }
        #endregion

        #region implicit
        #endregion

        #region explicit
        public static explicit operator RoixBorderIntSize(in RoixBorderSize borderSize) => new((RoixIntSize)borderSize.Size, (RoixIntSize)borderSize.Border);
        #endregion

        #region operator
        //public static RoixBorderSize operator *(in RoixBorderSize borderSize, double mul) => new(borderSize.Size * mul, borderSize.Border);

        //public static RoixBorderSize operator /(in RoixBorderSize borderSize, double div) => (div != 0) ? new(borderSize.Size / div, borderSize.Border) : throw new DivideByZeroException();
        #endregion

        #region Properties
        #endregion

        #region Methods
        public RoixIntSize ToRoixIntSize(bool isCheckBorder = true)
        {
            if (isCheckBorder && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

            var srcSize = (RoixIntSize)Size;
            var intSize = (RoixIntSize)Border;
            if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

            var width = Math.Clamp(srcSize.Width, 0, intSize.Width - 1);
            var height = Math.Clamp(srcSize.Height, 0, intSize.Height - 1);
            return new(width, height);
        }
        #endregion

    }
}
