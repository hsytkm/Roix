using System;
using System.Runtime.CompilerServices;

namespace Roix.Wpf.Internals
{
    static class MathEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(this double value) => (int)Math.Round(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this double value) => (int)Math.Floor(value);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool IsInside(this double value, double min, double max) => min <= value && value <= max;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool IsOutside(this double value, double min, double max) => !IsInside(value, min, max);

    }
}
