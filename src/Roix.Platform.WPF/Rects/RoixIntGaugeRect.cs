﻿//using Roix.Wpf.Internals;
//using System;

//namespace Roix.Wpf
//{
//    public readonly struct RoixIntGaugeRect : IEquatable<RoixIntGaugeRect>
//    {
//        public readonly RoixIntRect Roi;
//        public readonly RoixIntSize Canvas;

//        #region ctor
//        public RoixIntGaugeRect(in RoixIntRect roi, in RoixIntSize canvas) => (Roi, Canvas) = (roi, canvas);

//        public void Deconstruct(out RoixIntRect roi, out RoixIntSize canvas) => (roi, canvas) = (Roi, Canvas);
//        #endregion

//        #region Equals
//        public bool Equals(RoixIntGaugeRect other) => (Roi, Canvas) == (other.Roi, other.Canvas);
//        public override bool Equals(object? obj) => (obj is RoixIntGaugeRect other) && Equals(other);
//        public override int GetHashCode() => HashCode.Combine(Roi, Canvas);
//        public static bool operator ==(in RoixIntGaugeRect left, in RoixIntGaugeRect right) => left.Equals(right);
//        public static bool operator !=(in RoixIntGaugeRect left, in RoixIntGaugeRect right) => !(left == right);
//        #endregion

//        public override string ToString() => $"{nameof(RoixIntGaugeRect)} {{ {nameof(Roi)} = {Roi}, {nameof(Canvas)} = {Canvas} }}";

//        #region implicit
//        public static implicit operator RoixIntGaugeRect(in RoixGaugeRect rect) => new(rect.Roi, rect.Canvas);
//        public static implicit operator RoixGaugeRect(in RoixIntGaugeRect rect) => new(rect.Roi, rect.Canvas);
//        #endregion

//    }
//}