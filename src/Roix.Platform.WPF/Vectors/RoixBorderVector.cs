using Roix.SourceGenerator;
using Roix.Wpf.Internals;
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
        public RoixBorderVector(double x, double y, double width, double height) => _values = new(new(x, y), new(width, height));

        private partial void Validate(in RoixBorderVector value)
        {
            if (value.Border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        }
        #endregion

        #region implicit
        #endregion

        #region explicit
        //public static explicit operator RoixBorderPoint(in RoixBorderVector borderVector) => new((RoixPoint)borderVector.Vector, borderVector.Border);
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion

    }
}
