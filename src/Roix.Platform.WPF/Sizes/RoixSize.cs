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
            if (value.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
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
        public static RoixSize operator *(in RoixSize size, double scalar)
        {
            if (size.IsEmpty) return Empty;
            if (scalar < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(size.Width * scalar, size.Height * scalar);
        }

        public static RoixSize operator /(in RoixSize size, double scalar)
        {
            if (size.IsEmpty) return Empty;
            if (scalar == 0) throw new DivideByZeroException();
            if (scalar < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return size * (1d / scalar);
        }

        public static RoixSize operator *(in RoixSize size, in RoixRatioXY ratio)
        {
            if (size.IsEmpty) return Empty;
            if (ratio.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(size.X * ratio.X, size.Y * ratio.Y);
        }

        public static RoixSize operator /(in RoixSize size, in RoixRatioXY ratio)
        {
            if (size.IsEmpty) return Empty;
            if (ratio.IsIncludeZero) throw new DivideByZeroException();
            if (ratio.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(size.X / ratio.X, size.Y / ratio.Y);
        }

        public static RoixRatioXY operator /(in RoixSize size1, in RoixSize size2)
        {
            if (size1.IsEmpty || size2.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            if (size2.IsIncludeZero) throw new DivideByZeroException();
            if (size2.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(size1.Width / size2.Width, size1.Height / size2.Height);
        }
        #endregion

        public bool IsEmpty => this == Empty;

        /// <summary>Size の最小サイズを指定値で制限します</summary>
        public RoixSize ClipByMinimum(in RoixSize minSize)
        {
            if (IsEmpty) return Empty;
            if (minSize.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
            return new(Math.Max(Width, minSize.Width), Math.Max(Height, minSize.Height));
        }

        /// <summary>Size の最大サイズを指定値で制限します</summary>
        public RoixSize ClipByMaximum(in RoixSize maxSize)
        {
            if (IsEmpty) return Empty;
            if (maxSize.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
            return new(Math.Min(Width, maxSize.Width), Math.Min(Height, maxSize.Height));
        }

    }
}
