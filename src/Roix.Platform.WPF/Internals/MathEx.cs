using System;
using System.Runtime.CompilerServices;

namespace Roix.Wpf
{
    public enum RoundingMode
    {
        Round, Floor, Ceiling
    }

    [Flags]
    public enum PointDirection
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

        /// <summary>origin から見て point がどの方向にあるか判定(Windowの左上を原点として扱う)</summary>
        internal static PointDirection GetPointDirection(in this RoixPoint point, in RoixPoint origin) => (point - origin) switch
        {
            (0, 0) => PointDirection.Same,
            ( > 0, 0) => PointDirection.Right,
            ( < 0, 0) => PointDirection.Left,
            (0, < 0) => PointDirection.Top,
            (0, > 0) => PointDirection.Bottom,
            ( > 0, < 0) => PointDirection.TopRight,
            ( < 0, < 0) => PointDirection.TopLeft,
            ( < 0, > 0) => PointDirection.BottomLeft,
            ( > 0, > 0) => PointDirection.BottomRight,
            _ => throw new NotSupportedException(),
        };

        internal static PointDirection GetOppositeDirection(this PointDirection direction) => direction switch
        {
            PointDirection.Same => PointDirection.Same,
            PointDirection.Left => PointDirection.Right,
            PointDirection.Right => PointDirection.Left,
            PointDirection.Bottom => PointDirection.Top,
            PointDirection.Top => PointDirection.Bottom,
            PointDirection.TopLeft => PointDirection.BottomRight,
            PointDirection.TopRight => PointDirection.BottomLeft,
            PointDirection.BottomRight => PointDirection.TopLeft,
            PointDirection.BottomLeft => PointDirection.TopRight,
            _ => throw new NotImplementedException(),
        };

        private static RoundingMode GetRoundingModeX(this PointDirection direction)
        {
            if (direction.HasFlag(PointDirection.Left)) return RoundingMode.Floor;
            if (direction.HasFlag(PointDirection.Right)) return RoundingMode.Ceiling;
            throw new NotImplementedException();
        }

        private static RoundingMode GetRoundingModeY(this PointDirection direction)
        {
            if (direction.HasFlag(PointDirection.Top)) return RoundingMode.Floor;
            if (direction.HasFlag(PointDirection.Bottom)) return RoundingMode.Ceiling;
            throw new NotImplementedException();
        }

        public static (RoundingMode X, RoundingMode Y) GetRoundingMode(this PointDirection direction)
            => (GetRoundingModeX(direction), GetRoundingModeY(direction));

    }
}
