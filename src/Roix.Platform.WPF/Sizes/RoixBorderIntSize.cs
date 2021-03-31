using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntSize
    {
        readonly struct SourceValues
        {
            public readonly RoixIntSize Size;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntSize size, in RoixIntSize border) => (Size, Border) = (size, border);
        }

        private RoixIntSize Value => Size;

        public static implicit operator RoixBorderSize(in RoixBorderIntSize borderSize) => new(borderSize.Size, borderSize.Border);

        /// <summary>Size の最小サイズを指定値で制限します</summary>
        public RoixBorderIntSize ClippedByMinimum(in RoixIntSize minSize) => new(Size.ClippedByMinimum(minSize), Border);

        /// <summary>Size の最大サイズを指定値で制限します</summary>
        public RoixBorderIntSize ClippedByMaximum(in RoixIntSize maxSize) => new(Size.ClippedByMaximum(maxSize), Border);

    }
}
