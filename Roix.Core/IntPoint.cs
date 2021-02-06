using System;

namespace Roix.Core
{
    public record IntPoint
    {
        public int X { get; }
        public int Y { get; }

        public IntPoint(int x, int y) => (X, Y) = (x, y);

        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

        public static explicit operator IntSize(IntPoint p) => new IntSize(p.X, p.Y);

        // ◆この実装微妙
        public IntPoint GetReverse() => new IntPoint(Y, X);
    }
}
