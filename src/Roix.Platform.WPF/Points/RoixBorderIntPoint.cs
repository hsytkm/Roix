using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderIntPoint
    {
        readonly struct SourceValues
        {
            public readonly RoixIntPoint Point;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntPoint point, in RoixIntSize border) => (Point, Border) = (point, border);
        }

        private RoixIntPoint Value => _values.Point;

        #region ctor
        private partial void Validate(in RoixBorderIntPoint value)
        {
            if (value.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }
        #endregion

        #region implicit
        public static implicit operator RoixBorderPoint(in RoixBorderIntPoint borderPoint) => new(borderPoint.Point, borderPoint.Border);
        #endregion

        #region explicit
        public static explicit operator RoixBorderIntPoint(in RoixBorderPoint borderPoint) => new((RoixIntPoint)borderPoint.Point, (RoixIntSize)borderPoint.Border);
        #endregion

    }
}
