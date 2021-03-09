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

        private RoixSize Value => _values.Size;

        public RoixBorderIntSize ToRoixInt(RoundingMode rounding = RoundingMode.Floor)
            => new(Size.ToRoixInt(rounding), Border.ToRoixInt(rounding));

        //public RoixIntSize ToRoixIntSize(bool isCheckBorder = true)
        //{
        //    if (isCheckBorder && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

        //    var srcSize = (RoixIntSize)Size;
        //    var intSize = (RoixIntSize)Border;
        //    if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

        //    var width = Math.Clamp(srcSize.Width, 0, intSize.Width - 1);
        //    var height = Math.Clamp(srcSize.Height, 0, intSize.Height - 1);
        //    return new(width, height);
        //}

    }
}
