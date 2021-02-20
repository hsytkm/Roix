//using System;

//namespace Roix.Wpf.Ranges
//{
//    public readonly struct RoixPlaneRange : IEquatable<RoixPlaneRange>
//    {
//        public readonly RoixRange AxisX;
//        public readonly RoixRange AxisY;

//        #region ctor
//        public RoixPlaneRange(RoixRange axisX, RoixRange axisY) => (AxisX, AxisY) = (axisX, axisY);

//        public void Deconstruct(out RoixRange axisX, out RoixRange axisY) => (axisX, axisY) = (AxisX, AxisY);
//        #endregion

//        #region Equals
//        public bool Equals(RoixPlaneRange other) => (AxisX, AxisY) == (other.AxisX, other.AxisY);
//        public override bool Equals(object? obj) => (obj is RoixPlaneRange other) && Equals(other);
//        public override int GetHashCode() => HashCode.Combine(AxisX, AxisY);
//        public static bool operator ==(in RoixPlaneRange left, in RoixPlaneRange right) => left.Equals(right);
//        public static bool operator !=(in RoixPlaneRange left, in RoixPlaneRange right) => !(left == right);
//        #endregion

//        public override string ToString() => $"{nameof(RoixPlaneRange)} {{ {nameof(AxisX)} = {AxisX}, {nameof(AxisY)} = {AxisY} }}";

//    }
//}
