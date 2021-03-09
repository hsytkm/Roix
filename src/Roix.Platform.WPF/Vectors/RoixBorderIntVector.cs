using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntVector
    {
        readonly struct SourceValues
        {
            public readonly RoixIntVector Vector;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntVector vector, in RoixIntSize border) => (Vector, Border) = (vector, border);
        }

        private RoixIntVector Value => _values.Vector;

        public static implicit operator RoixBorderVector(in RoixBorderIntVector borderVector) => new(borderVector.Vector, borderVector.Border);

    }
}
