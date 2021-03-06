using Roix.SourceGenerator;
using System;
using System.Linq;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Rect.cs

    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixRect
    {
        readonly struct SourceValues
        {
            public readonly RoixPoint Location;
            public readonly RoixSize Size;
            public SourceValues(in RoixPoint point, in RoixSize size) => (Location, Size) = (point, size);
        }

        public static RoixRect Empty { get; } = new(new(double.PositiveInfinity, double.PositiveInfinity), RoixSize.Empty);

        #region ctor
        public RoixRect(double x, double y, double width, double height) => _values = new(new(x, y), new(width, height));
        public RoixRect(in RoixPoint point1, in RoixPoint point2)
        {
            var x = Math.Min(point1.X, point2.X);
            var y = Math.Min(point1.Y, point2.Y);
            var width = Math.Max(point1.X, point2.X) - x;
            var height = Math.Max(point1.Y, point2.Y) - y;
            _values = new SourceValues(new(x, y), new(width, height));    // exception will occur if the size is zero.
        }
        public RoixRect(in RoixPoint point, in RoixVector vector) : this(point, point + vector) { }

        public void Deconstruct(out double x, out double y, out double width, out double height) => (x, y, width, height) = (Location.X, Location.Y, Size.Width, Size.Height);
        #endregion

        #region implicit
        public static implicit operator RoixRect(System.Windows.Rect rect) => !rect.IsEmpty ? new(rect.X, rect.Y, rect.Width, rect.Height) : Empty;
        public static implicit operator System.Windows.Rect(in RoixRect rect) => !rect.IsEmpty ? new(rect.X, rect.Y, rect.Width, rect.Height) : System.Windows.Rect.Empty;
        #endregion

        #region explicit
        #endregion

        #region operator
        public static RoixRect operator *(in RoixRect rect, double scalar)
        {
            if (rect.IsEmpty) return Empty;
            if (scalar < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(rect.X * scalar, rect.Y * scalar, rect.Width * scalar, rect.Height * scalar);
        }

        public static RoixRect operator /(in RoixRect rect, double scalar)
        {
            if (rect.IsEmpty) return Empty;
            if (scalar == 0) throw new DivideByZeroException();
            if (scalar < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return rect * (1d / scalar);
        }

        public static RoixRect operator *(in RoixRect rect, in RoixRatioXY ratio)
        {
            if (rect.IsEmpty) return Empty;
            if (ratio.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(rect.X * ratio.X, rect.Y * ratio.Y, rect.Width * ratio.X, rect.Height * ratio.Y);
        }

        public static RoixRect operator /(in RoixRect rect, in RoixRatioXY ratio)
        {
            if (rect.IsEmpty) return Empty;
            if (ratio.IsIncludeZero) throw new DivideByZeroException();
            if (ratio.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(rect.X / ratio.X, rect.Y / ratio.Y, rect.Width / ratio.X, rect.Height / ratio.Y);
        }
        #endregion

        #region Properties
        public bool IsEmpty => this == Empty;
        #endregion

        #region Methods
        public System.Windows.Media.PointCollection ToPointCollection()
        {
            if (IsEmpty) throw new ArgumentException(ExceptionMessages.RectIsEmpty);
            return new(new[] { TopLeft, TopRight, BottomRight, BottomLeft }.Select(static x => (System.Windows.Point)x));
        }

        #endregion

    }
}
