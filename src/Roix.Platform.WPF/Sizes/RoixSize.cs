using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Size.cs
    public readonly struct RoixSize : IEquatable<RoixSize>
    {
        public static RoixSize Zero { get; } = new(0, 0);
        public static RoixSize Empty { get; } = new(double.NegativeInfinity);

        public readonly double Width;
        public readonly double Height;

        #region ctor
        private RoixSize(double value) => (Width, Height) = (value, value); // forEmpty

        public RoixSize(double width, double height)
        {
            if (width < 0 || height < 0) throw new ArgumentException("width and height cannot be negative value.");
            (Width, Height) = (width, height);
        }

        public void Deconstruct(out double width, out double height) => (width, height) = (Width, Height);
        #endregion

        #region Equals
        public bool Equals(RoixSize other) => (Width, Height) == (other.Width, other.Height);
        public override bool Equals(object? obj) => (obj is RoixSize other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Width, Height);
        public static bool operator ==(in RoixSize left, in RoixSize right) => left.Equals(right);
        public static bool operator !=(in RoixSize left, in RoixSize right) => !(left == right);
        #endregion

        public override string ToString() => $"{nameof(RoixSize)} {{ {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";

        #region implicit
        public static implicit operator RoixSize(System.Windows.Size size) => !size.IsEmpty ? new(size.Width, size.Height) : Empty;
        public static implicit operator System.Windows.Size(in RoixSize size) => !size.IsEmpty ? new(size.Width, size.Height) : System.Windows.Size.Empty;

        public static explicit operator RoixVector(in RoixSize size) => !size.IsEmpty ? new(size.Width, size.Height) : throw new ArgumentException("size is empty");
        public static explicit operator RoixPoint(in RoixSize size) => !size.IsEmpty ? new(size.Width, size.Height) : throw new ArgumentException("size is empty");
        #endregion

        #region Properties
        public readonly bool IsEmpty => this == Empty;
        public readonly bool IsZero => this == Zero;

        /// <summary>Length=0 is Invalid</summary>
        public readonly bool IsInvalid => IsEmpty || IsZero;

        public readonly bool IsValid => !IsInvalid;
        #endregion

        #region Methods
        public readonly bool IsInside(in RoixSize canvas) => ((RoixPoint)this).IsInside(canvas);
        public readonly bool IsOutside(in RoixSize canvas) => !IsInside(canvas);
        #endregion


    }
}
