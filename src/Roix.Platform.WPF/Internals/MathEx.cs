using System;

namespace Roix.Wpf.Internals
{
    static class MathEx
    {
        public static double Clamp(this double value, double min, double max) => Math.Max(min, Math.Min(max, value));
        //public static int Clamp(int value, int min, int max) => (int)Math.Max(min, Math.Min(max, value));

        public static int RoundToInt(this double value) => (int)Math.Round(value);

    }
}
