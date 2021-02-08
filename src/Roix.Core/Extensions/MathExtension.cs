using System;

namespace Roix.Core.Extensions
{
    static class MathExtension
    {
        public static int RoundToInt(double value) => (int)Math.Round(value);

        public static int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));

        public static double Clamp(double value, double min, double max) => Math.Max(min, Math.Min(max, value));
    }
}
