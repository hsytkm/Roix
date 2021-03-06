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

        private RoixIntSize Value => _values.Size;

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
