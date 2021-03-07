using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntRect
    {
        readonly struct SourceValues
        {
            public readonly RoixIntRect Roi;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntRect roi, in RoixIntSize border) => (Roi, Border) = (roi, border);
        }

        private RoixIntRect Value => _values.Roi;

        #region ctor
        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint1, in RoixBorderIntPoint borderPoint2)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            _values = new(new RoixIntRect(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border);
        }

        #endregion

        #region implicit
        public static implicit operator RoixBorderRect(in RoixBorderIntRect borderRect) => new(borderRect.Roi, borderRect.Border);
        #endregion

        #region explicit
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

        #region Methods

        /// <summary>Roiの最小サイズを指定値で制限する</summary>
        //public RoixBorderIntRect ClippedByMinimumSize(in RoixIntSize minSize) => new(Roi.ClippedByMinimumSize(minSize), Border);
        #endregion

    }
}
