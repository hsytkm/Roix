using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Size.cs
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixSize
    {
        readonly struct SourceValues
        {
            public readonly double Width;
            public readonly double Height;
            public SourceValues(double width, double height) => (Width, Height) = (width, height);
        }

        public static RoixSize Empty { get; } = new(double.NegativeInfinity);

        #region ctor
        private RoixSize(double value) => _values = new(value, value);  // forEmpty(skip Validate)

        private partial void Validate(in RoixSize value)
        {
            if (double.IsNegative(value.Width) || double.IsNegative(value.Height))
                throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
        }
        #endregion

        #region implicit
        public static implicit operator RoixSize(System.Windows.Size size) => !size.IsEmpty ? new(size.Width, size.Height) : Empty;
        public static implicit operator System.Windows.Size(in RoixSize size) => !size.IsEmpty ? new(size.Width, size.Height) : System.Windows.Size.Empty;
        #endregion

        #region explicit
        public static explicit operator RoixVector(in RoixSize size) => !size.IsEmpty ? new(size.Width, size.Height) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        public static explicit operator RoixPoint(in RoixSize size) => !size.IsEmpty ? new(size.Width, size.Height) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        #endregion

        #region operator
        public static RoixSize operator *(in RoixSize size, double mul)
        {
            if (size.IsEmpty) return Empty;
            if (mul < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(size.Width * mul, size.Height * mul);
        }

        public static RoixSize operator /(in RoixSize size, double div)
        {
            if (div == 0) throw new DivideByZeroException();
            return size * (1d / div);
        }
        #endregion

        #region Properties
        public bool IsEmpty => this == Empty;

        /// <summary>Length=0 is Invalid</summary>
        public bool IsInvalid => IsEmpty || IsZero;

        public bool IsValid => !IsInvalid;
        #endregion

        #region Methods
        public bool IsInside(in RoixSize border) => (0 <= Width && Width <= border.Width) && (0 <= Height && Height <= border.Height);
        public bool IsOutside(in RoixSize border) => !IsInside(border);
        #endregion


    }
}
