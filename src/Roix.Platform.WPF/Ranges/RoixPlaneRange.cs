//using System;

//namespace Roix.Wpf.Ranges
//{
//    public readonly struct RoixPlaneRange : IEquatable<RoixPlaneRange>
//    {
//        public readonly RoixRange AxisX { get; }
//        public readonly RoixRange AxisY { get; }

//        #region ctor
//        public RoixPlaneRange(in RoixRange axisX, in RoixRange axisY) => (AxisX, AxisY) = (axisX, axisY);

//        public readonly void Deconstruct(out RoixRange axisX, out RoixRange axisY) => (axisX, axisY) = (AxisX, AxisY);
//        #endregion

//        #region Equals
//        public readonly bool Equals(RoixPlaneRange other) => (AxisX, AxisY) == (other.AxisX, other.AxisY);
//        public readonly override bool Equals(object? obj) => (obj is RoixPlaneRange other) && Equals(other);
//        public readonly override int GetHashCode() => HashCode.Combine(AxisX, AxisY);
//        public static bool operator ==(in RoixPlaneRange left, in RoixPlaneRange right) => left.Equals(right);
//        public static bool operator !=(in RoixPlaneRange left, in RoixPlaneRange right) => !(left == right);
//        #endregion

//        public readonly override string ToString() => $"{nameof(RoixPlaneRange)} {{ {nameof(AxisX)} = {AxisX}, {nameof(AxisY)} = {AxisY} }}";

//    }
//}
