﻿using Roix.SourceGenerator;
using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntPoint
    {
        readonly struct SourceValues
        {
            public readonly RoixIntPoint Point;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntPoint point, in RoixIntSize border) => (Point, Border) = (point, border);
        }

        private RoixIntPoint Value => _values.Point;

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
