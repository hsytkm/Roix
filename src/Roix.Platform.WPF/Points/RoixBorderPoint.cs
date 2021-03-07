using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderPoint
    {
        readonly struct SourceValues
        {
            public readonly RoixPoint Point;
            public readonly RoixSize Border;
            public SourceValues(in RoixPoint point, in RoixSize border) => (Point, Border) = (point, border);
        }

        private RoixPoint Value => _values.Point;

        #region ctor
        public RoixBorderPoint(double x, double y, double width, double height) => _values = new(new(x, y), new(width, height));

        private partial void Validate(in RoixBorderPoint value)
        {
            if (value.Border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        }
        #endregion

        #region implicit
        #endregion

        #region explicit
        public static explicit operator RoixBorderIntPoint(in RoixBorderPoint borderPoint) => new((RoixIntPoint)borderPoint.Point, (RoixIntSize)borderPoint.Border);
        //public static explicit operator RoixBorderSize(in RoixBorderPoint borderPoint) => new((RoixSize)borderPoint.Point, borderPoint.Border);
        //public static explicit operator RoixBorderVector(in RoixBorderPoint borderPoint) => new((RoixVector)borderPoint.Point, borderPoint.Border);
        #endregion

        #region operator
        public static RoixBorderPoint operator +(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector) => (borderPoint.Border == borderVector.Border) ? new(borderPoint.Point + borderVector.Vector, borderPoint.Border) : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);

        public static RoixBorderPoint operator -(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector) => (borderPoint.Border == borderVector.Border) ? new(borderPoint.Point - borderVector.Vector, borderPoint.Border) : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);

        public static RoixBorderVector operator -(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2) => (borderPoint1.Border == borderPoint2.Border) ? new(borderPoint1.Point - borderPoint2.Point, borderPoint1.Border) : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        #endregion

        #region Properties
        public RoixPoint ClippedRoixPoint => new(Math.Clamp(Point.X, 0, Border.Width), Math.Clamp(Point.Y, 0, Border.Height));
        #endregion

        #region Methods
        public RoixIntPoint ToRoixIntPoint(bool isCheckBorder = true)
        {
            if (isCheckBorder && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

            var srcPoint = (RoixIntPoint)Point;
            var intSize = (RoixIntSize)Border;
            if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

            var x = Math.Clamp(srcPoint.X, 0, intSize.Width - 1);
            var y = Math.Clamp(srcPoint.Y, 0, intSize.Height - 1);
            return new(x, y);
        }

        public RoixBorderRect CreateRoixBorderRect(in RoixVector vector) => new(new RoixRect(Point, vector), Border);
        public RoixBorderRect CreateRoixBorderRect(in RoixBorderVector borderVector)
        {
            if (Border != borderVector.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            return CreateRoixBorderRect(borderVector.Vector);
        }

        /// <summary>引数で指定した座標系(int)に変換する</summary>
        public RoixBorderIntPoint ConvertToRoixInt(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
        {
            if (this.Border.IsEmpty || this.Border.IsZero) return (RoixBorderIntPoint)this;

            var point1 = RoixIntPoint.Create(this.Point, this.Border, destIntSize, mode);
            var point2 = point1.GetClippedIntPoint(destIntSize);
            return new(point2, destIntSize);
        }

        /// <summary>引数で指定した座標系(int)の分解能に調整する</summary>
        public RoixBorderPoint AdjustRoixWithResolutionOfImage(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
        {
            if (this.Border.IsEmpty || this.Border.IsZero) return this;
            var point = this.Point.AdjustRoixWithResolutionOfImage(this.Border, destIntSize, mode);
            return new(point, this.Border);
        }

        #endregion

    }
}
