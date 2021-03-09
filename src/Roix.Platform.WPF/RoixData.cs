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

    //    #region ctor
    //    #endregion

    //    #region implicit
    //    #endregion

    //    #region explicit
    //    #endregion

    //    #region operator
    //    #endregion

    //    #region Properties
    //    #endregion

    //    #region Methods
    //    #endregion
    //}
}


/*  Roix Policy
 *      - RoixDouble と Windows は互換性があり implicit cast できる（Size に Empty の状態が存在する）
 *      - Windows と RoixInt 間は cast できない（RoixDouble を挟めば可能）
 *      - RoixInt → RoixDouble は implicit cast で Int側 に定義
 *      - RoixDouble → RoixInt は explicit cast で Int側 に定義（小数点は切り捨て固定）
 *      - Size(Double/Int) には Validate が存在する（Exception.CannotBeNegativeValue）
 *      - Border（Sizeを含む）にも Validate が存在する（Exception.SizeIsNegative）
 *      - Border なし版（RoixPoint / RoixSize など）は ToRoixBorder メソッドを持つ
 *
 *
 *  未
 *      - RoixDouble は ConvertToRoixInt メソッドを必ず持つ
 *
 *
 */
