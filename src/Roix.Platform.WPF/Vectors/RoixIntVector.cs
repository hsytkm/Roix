using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixIntVector
    {
        readonly struct SourceValues
        {
            public readonly int X;
            public readonly int Y;
            public SourceValues(int x, int y) => (X, Y) = (x, y);
        }

        #region ctor
        public RoixIntVector(double x, double y) => _values = new(x.FloorToInt(), y.FloorToInt());
        #endregion

        #region implicit
        public static implicit operator RoixVector(in RoixIntVector vector) => new(vector.X, vector.Y);

        public static implicit operator System.Windows.Vector(in RoixIntVector vector) => new(vector.X, vector.Y);
        #endregion

        #region explicit
        public static explicit operator RoixIntVector(in RoixVector vector) => new(vector.X.RoundToInt(), vector.Y.RoundToInt());
        public static explicit operator RoixIntVector(System.Windows.Vector vector) => new(vector.X.RoundToInt(), vector.Y.RoundToInt());
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

    }
}
