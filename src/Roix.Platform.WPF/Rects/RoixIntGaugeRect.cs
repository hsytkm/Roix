//using Roix.Wpf.Internals;
//using System;

//namespace Roix.Wpf
//{
//    public readonly struct RoixIntGaugeRect : IEquatable<RoixIntGaugeRect>
//    {
//        public readonly RoixIntRect Roi { get; }
//        public readonly RoixIntSize Bounds { get; }

//        #region ctor
//        public RoixIntGaugeRect(in RoixIntRect roi, in RoixIntSize bounds) => (Roi, Bounds) = (roi, bounds);

//        public readonly void Deconstruct(out RoixIntRect roi, out RoixIntSize bounds) => (roi, bounds) = (Roi, Bounds);
//        #endregion

//        #region Equals
//        public readonly bool Equals(RoixIntGaugeRect other) => (Roi, Bounds) == (other.Roi, other.Bounds);
//        public readonly override bool Equals(object? obj) => (obj is RoixIntGaugeRect other) && Equals(other);
//        public readonly override int GetHashCode() => HashCode.Combine(Roi, Bounds);
//        public static bool operator ==(in RoixIntGaugeRect left, in RoixIntGaugeRect right) => left.Equals(right);
//        public static bool operator !=(in RoixIntGaugeRect left, in RoixIntGaugeRect right) => !(left == right);
//        #endregion

//        public readonly override string ToString() => $"{nameof(RoixIntGaugeRect)} {{ {nameof(Roi)} = {Roi}, {nameof(Bounds)} = {Bounds} }}";

//        #region implicit
//        public static implicit operator RoixIntGaugeRect(in RoixGaugeRect rect) => new(rect.Roi, rect.Bounds);
//        public static implicit operator RoixGaugeRect(in RoixIntGaugeRect rect) => new(rect.Roi, rect.Bounds);
//        #endregion

//    }
//}
