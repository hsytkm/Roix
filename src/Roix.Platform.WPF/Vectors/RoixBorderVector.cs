using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderVector
    {
        readonly struct SourceValues
        {
            public readonly RoixVector Vector;
            public readonly RoixSize Border;
            public SourceValues(in RoixVector vector, in RoixSize border) => (Vector, Border) = (vector, border);
        }

        private RoixVector Value => _values.Vector;

        public RoixBorderIntVector ToRoixInt(RoundingMode rounding = RoundingMode.Floor)
            => new(Vector.ToRoixInt(rounding), Border.ToRoixInt(rounding));

    }
}
