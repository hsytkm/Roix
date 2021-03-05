using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.TypeInt)]
    public readonly partial struct RoixIntRect
    {
        readonly struct SourceValues
        {
            public readonly RoixIntPoint Location;
            public readonly RoixIntSize Size;
            public SourceValues(in RoixIntPoint point, in RoixIntSize size) => (Location, Size) = (point, size);
        }

        #region ctor
        public RoixIntRect(int x, int y, int width, int height) => _values = new(new(x, y), new(width, height));
        public void Deconstruct(out int x, out int y, out int width, out int height) => (x, y, width, height) = (Location.X, Location.Y, Size.Width, Size.Height);
        #endregion

        #region implicit
        public static implicit operator RoixRect(in RoixIntRect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        public static implicit operator System.Windows.Rect(in RoixIntRect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);
        #endregion

        #region explicit
        public static explicit operator RoixIntRect(in RoixRect rect) => !rect.IsEmpty ? new((RoixIntPoint)rect.Location, (RoixIntSize)rect.Size) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        public static explicit operator RoixIntRect(System.Windows.Rect rect) => !rect.IsEmpty ? new((RoixIntPoint)rect.Location, (RoixIntSize)rect.Size) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        #endregion

        #region operator
        #endregion

        #region Properties
        public int X => Location.X;
        public int Y => Location.Y;
        public int Width => Size.Width;
        public int Height => Size.Height;
        public int Left => Location.X;
        public int Right => Location.X + Size.Width;
        public int Top => Location.Y;
        public int Bottom => Location.Y + Size.Height;
        public RoixIntPoint TopLeft => Location;
        public RoixIntPoint TopRight => new(Right, Top);
        public RoixIntPoint BottomLeft => new(Left, Bottom);
        public RoixIntPoint BottomRight => new(Right, Bottom);
        #endregion

    }
}
