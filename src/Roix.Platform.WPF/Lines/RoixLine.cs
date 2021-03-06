﻿using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{

    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixLine
    {
        readonly struct SourceValues
        {
            public readonly RoixPoint Point1;
            public readonly RoixPoint Point2;
            public SourceValues(in RoixPoint point1, in RoixPoint point2) => (Point1, Point2) = (point1, point2);
        }

        #region ctor
        public RoixLine(double x1, double y1, double x2, double y2) : this(new RoixPoint(x1, y1), new RoixPoint(x2, y2)) { }

        public RoixLine(in RoixPoint point, in RoixVector vector) : this(point, point + vector) { }

        public void Deconstruct(out double x1, out double y1, out double x2, out double y2)
            => (x1, y1, x2, y2) = (Point1.X, Point1.Y, Point2.X, Point2.Y);
        #endregion

        #region operator
        public static RoixLine operator +(in RoixLine line, RoixVector vector) => new(line.Point1 + vector, line.Point2 + vector);
        public static RoixLine operator -(in RoixLine line, RoixVector vector) => new(line.Point1 + (-vector), line.Point2 + (-vector));
        #endregion

        #region Methods

        ///// <summary>指定 border の内部に収めた IntRect を返します</summary>
        //public RoixRect GetClippedRect(in RoixSize border, bool isPointPriority = true)
        //    => isPointPriority ? GetClippedRectByPointPriority(border) : GetClippedRectBySizePriority(border);

        ///// <summary>Rect の最小サイズを指定値で制限します</summary>
        //public RoixRect ClippedSizeByMinimum(in RoixSize minSize) => new(Location, Size.ClippedByMinimum(minSize));

        ///// <summary>Rect の最大サイズを指定値で制限します</summary>
        //public RoixRect ClippedSizeByMaximum(in RoixSize maxSize) => new(Location, Size.ClippedByMaximum(maxSize));

        #endregion

    }
}
