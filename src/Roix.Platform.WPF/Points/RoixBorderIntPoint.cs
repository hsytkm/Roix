using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntPoint
    {
        readonly struct SourceValues
        {
            public readonly RoixIntPoint Point;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntPoint point, in RoixIntSize border) => (Point, Border) = (point, border);
        }

        internal RoixIntPoint Value => Point;

        public static implicit operator RoixBorderPoint(in RoixBorderIntPoint borderPoint) => new(borderPoint.Point, borderPoint.Border);

        #region operator
        public static RoixBorderIntPoint operator +(in RoixBorderIntPoint borderPoint, in RoixIntVector vector) => new(borderPoint.Point + vector, borderPoint.Border);
        public static RoixBorderIntPoint operator -(in RoixBorderIntPoint borderPoint, in RoixIntVector vector) => new(borderPoint.Point - vector, borderPoint.Border);
        public static RoixBorderIntVector operator -(in RoixBorderIntPoint borderPoint, in RoixIntPoint point) => new(borderPoint.Point - point, borderPoint.Border);

        public static RoixBorderIntPoint operator +(in RoixBorderIntPoint borderPoint, in RoixBorderIntVector borderVector) => (borderPoint.Border == borderVector.Border) ? borderPoint + borderVector.Vector : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        public static RoixBorderIntPoint operator -(in RoixBorderIntPoint borderPoint, in RoixBorderIntVector borderVector) => (borderPoint.Border == borderVector.Border) ? borderPoint - borderVector.Vector : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        public static RoixBorderIntVector operator -(in RoixBorderIntPoint borderPoint1, in RoixBorderIntPoint borderPoint2) => (borderPoint1.Border == borderPoint2.Border) ? borderPoint1 - borderPoint2.Point : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        #endregion

    }
}
