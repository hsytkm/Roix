﻿using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator1 | RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixIntVector
    {
        readonly struct SourceValues
        {
            public readonly int X;
            public readonly int Y;
            public SourceValues(int x, int y) => (X, Y) = (x, y);
        }

        #region ctor
        public RoixIntVector(double x, double y) : this(x.FloorToInt(), y.FloorToInt()) { }
        #endregion

        #region implicit
        public static implicit operator RoixVector(in RoixIntVector vector) => new(vector.X, vector.Y);
        #endregion

        #region explicit
        public static explicit operator RoixIntSize(in RoixIntVector vector) => new(vector.X, vector.Y);
        public static explicit operator RoixIntPoint(in RoixIntVector vector) => new(vector.X, vector.Y);
        #endregion

        #region operator
        public static RoixIntVector operator -(in RoixIntVector vector) => Zero - vector;
        #endregion

    }
}
