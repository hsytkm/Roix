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

        internal RoixSize Value => Size;

        #region implicit
        public static implicit operator RoixBorderIntSize(in RoixBorderSize borderSize) => new(borderSize.Size.ToRoixInt(), borderSize.Border.ToRoixInt());
        #endregion

        public RoixRatioXY ToRoixRatio() => Size / Border;

        /// <summary>Size の最小サイズを指定値で制限します</summary>
        public RoixBorderSize ClipByMinimum(in RoixSize minSize) => new(Size.ClipByMinimum(minSize), Border);

        /// <summary>Size の最大サイズを指定値で制限します</summary>
        public RoixBorderSize ClipByMaximum(in RoixSize maxSize) => new(Size.ClipByMaximum(maxSize), Border);

    }
}
