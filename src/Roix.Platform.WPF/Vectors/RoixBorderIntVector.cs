using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderIntVector
    {
        readonly struct SourceValues
        {
            public readonly RoixIntVector Vector;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntVector vector, in RoixIntSize border) => (Vector, Border) = (vector, border);
        }
        private RoixIntVector Value => _values.Vector;

        #region ctor
        private partial void Validate(in RoixBorderIntVector value)
        {
            if (value.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }
        #endregion

        #region implicit
        public static implicit operator RoixBorderVector(in RoixBorderIntVector borderVector) => new(borderVector.Vector, borderVector.Border);
        #endregion

        #region explicit
        public static explicit operator RoixBorderIntVector(in RoixBorderVector borderVector) => new((RoixIntVector)borderVector.Vector, (RoixIntSize)borderVector.Border);
        #endregion


    }
}
