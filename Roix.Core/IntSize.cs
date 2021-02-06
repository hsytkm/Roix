using System;

namespace Roix.Core
{
    public record IntSize
    {
        public int Width { get; }
        public int Height { get; }

        public IntSize(int width, int height)
        {
            if (width < 0 || height < 0) throw new ArgumentException(Messages.Size_WidthAndHeightCannotBeNegative);
            (Width, Height) = (width, height);
        }

        public void Deconstruct(out int width, out int height) => (width, height) = (Width, Height);

        public static explicit operator IntPoint(IntSize s) => new IntPoint(s.Width, s.Height);

        public IntSize GetReverse() => new IntSize(Height, Width);
    }
}
