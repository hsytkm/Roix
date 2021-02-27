using System;

namespace Roix.Wpf
{
    //[SourceGenerator.RoixStructGenerator1]
    //public readonly partial struct RoixData1 : IEquatable<RoixData1>, IFormattable
    //{
    //    private readonly int _x;
    //    private readonly double _y;
    //    private readonly byte _z;
    //}

    [SourceGenerator.RoixStructGenerator2]
    public readonly partial struct RoixData2
    {
        readonly struct SourceValues
        {
            public readonly int X;
            public readonly double Y;
            public readonly byte Z;
            public SourceValues(int x, double y, byte z) => (X, Y, Z) = (x, y, z);
        }

        //private readonly SourceValues _values;
        //public RoixData2(int x, double y, byte z) => _values = new(x, y, z);
        //public int GetValue => _values.X + (int)_values.Y + (int)_values.Z;

    }
}
