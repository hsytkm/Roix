﻿using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderVector
    {
        readonly struct SourceValues
        {
            public readonly RoixVector Vector;
            public readonly RoixSize Border;
            public SourceValues(in RoixVector vector, in RoixSize border) => (Vector, Border) = (vector, border);
        }
        private RoixVector Value => _values.Vector;

        #region ctor
        public RoixBorderVector(double x, double y, double width, double height) : this(new(x, y), new(width, height)) { }

        private partial void Validate(in RoixBorderVector value)
        {
            if (value.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }
        #endregion

    }
}
