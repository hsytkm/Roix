using Roix.SourceGenerator;
using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.XYPair | RoixStructGeneratorOptions.TypeInt | RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixIntSize
    {
        readonly struct SourceValues
        {
            public readonly int Width;
            public readonly int Height;
            public SourceValues(int width, int height) => (Width, Height) = (width, height);
        }

        private double X => Width;
        private double Y => Height;

        #region ctor
        private partial void Validate(in RoixIntSize value)
        {
            if (value.Width < 0 || value.Height < 0)
                throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
        }
        #endregion

        #region implicit
        public static implicit operator RoixSize(in RoixIntSize size) => new(size.Width, size.Height);
        public static implicit operator System.Windows.Size(in RoixIntSize size) => new(size.Width, size.Height);
        #endregion

        #region explicit
        public static explicit operator RoixIntSize(in RoixSize size) => !size.IsEmpty ? new(size.Width.RoundToInt(), size.Height.RoundToInt()) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        public static explicit operator RoixIntSize(System.Windows.Size size) => !size.IsEmpty ? new(size.Width.RoundToInt(), size.Height.RoundToInt()) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

    }
}
