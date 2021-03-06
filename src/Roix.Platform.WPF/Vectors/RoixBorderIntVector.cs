using Roix.SourceGenerator;
using Roix.Wpf.Internals;
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

        #region ctor
        #endregion

        #region implicit
        #endregion

        #region explicit
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion

    }
}
