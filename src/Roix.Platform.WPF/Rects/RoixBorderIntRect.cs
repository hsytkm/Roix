using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    //[RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntRect
    {
        readonly struct SourceValues
        {
            public readonly RoixIntRect Roi;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntRect roi, in RoixIntSize border) => (Roi, Border) = (roi, border);
        }

        //private RoixIntRect Value => _values.Roi;

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
