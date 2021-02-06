using System;

namespace Roix.Core
{
    public record IntCanvasRoi
    {
        public IntRect Roi { get; init; }
        public IntSize Canvas { get; init; }

        public IntCanvasRoi(IntRect roi, IntSize canvas) => (Roi, Canvas) = (roi, canvas);
    }
}
