using System;

namespace Roix.Core
{
    public record IntRect
    {
        public IntPoint Point { get; init; }
        public IntSize Size { get; init; }

        public IntRect(IntPoint point, IntSize size) => (Point, Size) = (point, size);

    }
}
