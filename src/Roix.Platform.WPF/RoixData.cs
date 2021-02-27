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

    // readonly struct SourceValues を内部に書いておけば、
    // プロパティ, Ctor, IEquatable<T>, IFormattable などを自動実装してくれる。
    //[SourceGenerator.RoixStructGenerator]
    //public readonly partial struct RoixData2
    //{
    //    readonly struct SourceValues
    //    {
    //        public readonly int X;
    //        public readonly double Y;
    //        public readonly byte Z;
    //        public SourceValues(int x, double y, byte z) => (X, Y, Z) = (x, y, z);
    //    }

    //    public int Sum()
    //    {
    //        return X + (int)Y + (int)Z;
    //    }
    //}
}
