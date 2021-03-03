using System;

namespace Roix.Wpf
{
    [SourceGenerator.RoixStructGenerator]
    public readonly partial struct RoixBorderSize
    {
        readonly struct SourceValues
        {
            public readonly RoixSize Size;
            public readonly RoixSize Border;
            public SourceValues(in RoixSize size, in RoixSize border) => (Size, Border) = (size, border);
        }

        #region ctor
        public RoixBorderSize(in RoixSize size, in RoixSize border)
        {
            if (border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            _values = new(size, border);
        }
        #endregion

        #region implicit
        #endregion

        #region explicit
        #endregion

        #region operator
        //public static RoixBorderSize operator *(in RoixBorderSize borderSize, double mul) => new(borderSize.Size * mul, borderSize.Border);

        //public static RoixBorderSize operator /(in RoixBorderSize borderSize, double div) => (div != 0) ? new(borderSize.Size / div, borderSize.Border) : throw new DivideByZeroException();
        #endregion

        #region Properties
        public bool IsInsideBorder => Size.IsInside(Border);
        public bool IsOutsideBorder => !IsInsideBorder;
        #endregion

        #region Methods
        public RoixBorderSize ConvertToNewBorder(in RoixSize newBorder)
        {
            if (Border.IsInvalid) return this;
            if (newBorder.IsInvalid) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);

            var newSize = new RoixSize(Size.Width * newBorder.Width / Border.Width, Size.Height * newBorder.Height / Border.Height);
            return new(newSize, newBorder);
        }

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
