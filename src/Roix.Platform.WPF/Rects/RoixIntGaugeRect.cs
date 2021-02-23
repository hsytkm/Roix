//using Roix.Wpf.Internals;
//using System;

//namespace Roix.Wpf
//{
//    public readonly struct RoixIntGaugeRect : IEquatable<RoixIntGaugeRect>
//    {
//        public readonly RoixIntRect Roi { get; }
//        public readonly RoixIntSize Border { get; }

//        #region ctor
//        public RoixIntGaugeRect(in RoixIntRect roi, in RoixIntSize border) => (Roi, Border) = (roi, border);

//        public readonly void Deconstruct(out RoixIntRect roi, out RoixIntSize border) => (roi, border) = (Roi, Border);
//        #endregion

//        #region Equals
//        public readonly bool Equals(RoixIntGaugeRect other) => (Roi, Border) == (other.Roi, other.Border);
//        public readonly override bool Equals(object? obj) => (obj is RoixIntGaugeRect other) && Equals(other);
//        public readonly override int GetHashCode() => HashCode.Combine(Roi, Border);
//        public static bool operator ==(in RoixIntGaugeRect left, in RoixIntGaugeRect right) => left.Equals(right);
//        public static bool operator !=(in RoixIntGaugeRect left, in RoixIntGaugeRect right) => !(left == right);
//        #endregion

//        public readonly override string ToString() => $"{nameof(RoixIntGaugeRect)} {{ {nameof(Roi)} = {Roi}, {nameof(Border)} = {Border} }}";

//        #region implicit
//        public static implicit operator RoixIntGaugeRect(in RoixGaugeRect rect) => new(rect.Roi, rect.Border);
//        public static implicit operator RoixGaugeRect(in RoixIntGaugeRect rect) => new(rect.Roi, rect.Border);
//        #endregion

//    }
//}
