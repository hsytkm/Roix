using Roix.Core.Extensions;
using System;

namespace Roix.Core
{
    public record RoixBoundsRoi<T>(RoixRect<T> Roi, RoixRect<T> Bounds) where T : struct, IComparable<T>
    {
        public bool Contains() => Bounds.Contains(Roi);

        public RoixRect<T> ClipToBounds() => Roi.ClipToBounds(Bounds);
    }

    static class RoixBoundsRoiExtension
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    }

    public record RoixBoundsRoiInt : RoixBoundsRoi<int>
    {
        public RoixBoundsRoiInt(RoixRectInt roi, RoixRectInt bounds) : base(roi, bounds) { }
        public RoixBoundsRoiInt(RoixRectInt roi, RoixSizeInt size) : base(roi, new RoixRectInt(size)) { }
    }

    public record RoixBoundsRoiDouble : RoixBoundsRoi<double>
    {
        public RoixBoundsRoiDouble(RoixRectDouble roi, RoixRectDouble bounds) : base(roi, bounds) { }
        public RoixBoundsRoiDouble(RoixRectDouble roi, RoixSizeDouble size) : base(roi, new RoixRectDouble(size)) { }
    }

}
