using System;
using System.Runtime.CompilerServices;

namespace Roix.Wpf
{
    public enum RoundingMode
    {
        Round, Floor, Ceiling
    }

    [Flags]
    public enum PointPosition
    {
        Same = 0x0000,
        Right = 0x0001,
        Left = 0x0002,
        Bottom = 0x0004,
        Top = 0x0008,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomRight = Bottom | Right,
        BottomLeft = Bottom | Left,
    };

    static class MathEx
    {
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

        public static PointPosition GetOppositePosition(this PointPosition pos) => pos switch
        {
            PointPosition.Same => PointPosition.Same,
            PointPosition.Left => PointPosition.Right,
            PointPosition.Right => PointPosition.Left,
            PointPosition.Bottom => PointPosition.Top,
            PointPosition.Top => PointPosition.Bottom,
            PointPosition.TopLeft => PointPosition.BottomRight,
            PointPosition.TopRight => PointPosition.BottomLeft,
            PointPosition.BottomRight => PointPosition.TopLeft,
            PointPosition.BottomLeft => PointPosition.TopRight,
            _ => throw new NotImplementedException(),
        };

        private static RoundingMode GetRoundingModeX(this PointPosition position)
        {
            if (position.HasFlag(PointPosition.Left)) return RoundingMode.Floor;
            if (position.HasFlag(PointPosition.Right)) return RoundingMode.Ceiling;
            throw new NotImplementedException();
        }

        private static RoundingMode GetRoundingModeY(this PointPosition position)
        {
            if (position.HasFlag(PointPosition.Top)) return RoundingMode.Floor;
            if (position.HasFlag(PointPosition.Bottom)) return RoundingMode.Ceiling;
            throw new NotImplementedException();
        }

        public static (RoundingMode X, RoundingMode Y) GetRoundingMode(this PointPosition position)
            => (GetRoundingModeX(position), GetRoundingModeY(position));


    }
}
