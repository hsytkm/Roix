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

        internal RoixVector Value => Vector;

        #region implicit
        public static implicit operator RoixBorderIntVector(in RoixBorderVector borderVector) => new(borderVector.Vector.ToRoixInt(), borderVector.Border.ToRoixInt());
        #endregion

        public RoixRatioXY ToRoixRatio() => Vector / (RoixVector)Border;

    }
}
