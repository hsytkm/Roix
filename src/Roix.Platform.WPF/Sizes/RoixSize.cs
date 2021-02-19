﻿using System;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Size.cs
    public readonly struct RoixSize : IEquatable<RoixSize>
    {
        public static RoixSize Empty { get; } = new(true, double.NegativeInfinity, double.NegativeInfinity);

        public readonly bool IsEmpty;
        public readonly double Width;
        public readonly double Height;

        private RoixSize(bool isEmpty, double width, double height) => (IsEmpty, Width, Height) = (isEmpty, width, height);

        public RoixSize(double width, double height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("width and height cannot be negative value.");

            (IsEmpty, Width, Height) = (false, width, height);
        }

        public void Deconstruct(out double width, out double height) => (width, height) = !IsEmpty ? (Width, Height) : (Empty.Width, Empty.Height);

        public static implicit operator RoixSize(System.Windows.Size s) => !s.IsEmpty ? new(s.Width, s.Height) : Empty;
        public static implicit operator System.Windows.Size(in RoixSize s) => !s.IsEmpty ? new(s.Width, s.Height) : System.Windows.Size.Empty;

        public bool Equals(RoixSize other) => (IsEmpty, Width, Height) == (other.IsEmpty, other.Width, other.Height);
        public override bool Equals(object? obj) => (obj is RoixSize other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(IsEmpty, Width, Height);
        public static bool operator ==(in RoixSize left, in RoixSize right) => left.Equals(right);
        public static bool operator !=(in RoixSize left, in RoixSize right) => !(left == right);
        public override string ToString() => $"{nameof(RoixSize)} {{ {nameof(IsEmpty)} = {IsEmpty}, {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";

        /// <summary>Length=0 is Invalid</summary>
        public bool IsInvalid => IsEmpty || Width <= 0 || Height <= 0;

        public bool IsValid => !IsInvalid;

    }
}
