using System;
using System.Runtime.CompilerServices;

namespace Roix.Wpf
{
    public enum RoundingMode
    {
        Round, Floor, Ceiling
    }

    static class MathEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(this double value) => (int)Math.Round(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this double value) => (int)Math.Floor(value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this double value, RoundingMode mode) => mode switch
        {
            RoundingMode.Floor => (int)Math.Floor(value),
            RoundingMode.Round => (int)Math.Round(value),
            RoundingMode.Ceiling => (int)Math.Ceiling(value),
            _ => throw new NotImplementedException(),
        };

    }
}
