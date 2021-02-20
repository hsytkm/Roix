//using System;

//namespace Roix.Wpf.Ranges
//{
//    public readonly struct RoixRange : IEquatable<RoixRange>
//    {
//        public readonly double Min;
//        public readonly double Max;

//        #region ctor
//        public RoixRange(double min, double max)
//        {
//            if (min > max) throw new ArgumentException("min and max are inverted.");
//            (Min, Max) = (min, max);
//        }

//        public void Deconstruct(out double min, out double max) => (min, max) = (Min, Max);
//        #endregion

//        #region Equals
//        public bool Equals(RoixRange other) => (Min, Max) == (other.Min, other.Max);
//        public override bool Equals(object? obj) => (obj is RoixRange other) && Equals(other);
//        public override int GetHashCode() => HashCode.Combine(Min, Max);
//        public static bool operator ==(in RoixRange left, in RoixRange right) => left.Equals(right);
//        public static bool operator !=(in RoixRange left, in RoixRange right) => !(left == right);
//        #endregion

//        public override string ToString() => $"{nameof(RoixRange)} {{ {nameof(Min)} = {Min}, {nameof(Max)} = {Max} }}";

//    }
//}
