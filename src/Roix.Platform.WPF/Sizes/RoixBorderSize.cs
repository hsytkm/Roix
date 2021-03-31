using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderSize
    {
        readonly struct SourceValues
        {
            public readonly RoixSize Size;
            public readonly RoixSize Border;
            public SourceValues(in RoixSize size, in RoixSize border) => (Size, Border) = (size, border);
        }

        private RoixSize Value => Size;

        public RoixRatioXY ToRoixRatio() => Size / Border;

        /// <summary>Size の最小サイズを指定値で制限します</summary>
        public RoixBorderSize ClippedByMinimum(in RoixSize minSize) => new(Size.ClippedByMinimum(minSize), Border);

        /// <summary>Size の最大サイズを指定値で制限します</summary>
        public RoixBorderSize ClippedByMaximum(in RoixSize maxSize) => new(Size.ClippedByMaximum(maxSize), Border);

    }
}
