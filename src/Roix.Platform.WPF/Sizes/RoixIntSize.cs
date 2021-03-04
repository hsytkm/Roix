using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    [SourceGenerator.RoixStructGenerator]
    public readonly partial struct RoixIntSize
    {
        readonly struct SourceValues
        {
            public readonly int Width;
            public readonly int Height;
            public SourceValues(int width, int height) => (Width, Height) = (width, height);
        }

        #region ctor
        public RoixIntSize(int width, int height)
        {
            if (width < 0 || height < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            _values = new(width, height);
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
