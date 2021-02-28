﻿using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    [SourceGenerator.RoixStructGenerator]
    public readonly partial struct RoixGaugeVector
    {
        readonly struct SourceValues
        {
            public readonly RoixVector Vector;
            public readonly RoixSize Border;
            public SourceValues(in RoixVector vector, in RoixSize border) => (Vector, Border) = (vector, border);
        }

        #region ctor
        public RoixGaugeVector(in RoixVector vector, in RoixSize border)
        {
            if (border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            _values = new(vector, border);
        }

        public RoixGaugeVector(double x, double y, double width, double height) => _values = new(new(x, y), new(width, height));
        #endregion

        #region implicit
        #endregion

        #region explicit
        //public static explicit operator RoixGaugePoint(in RoixGaugeVector gaugeVector) => new((RoixPoint)gaugeVector.Vector, gaugeVector.Border);
        #endregion

        #region operator
        #endregion

        #region Properties
        public bool IsInsideBorder => Vector.X.IsInside(0, Border.Width) && Vector.Y.IsInside(0, Border.Height);
        public bool IsOutsideBorder => !IsInsideBorder;
        #endregion

        #region Methods
        public RoixGaugeVector ConvertToNewGauge(in RoixSize newBorder)
        {
            if (Border.IsInvalid) return this;
            if (newBorder.IsInvalid) throw new ArgumentException(ExceptionMessages.SizeIsInvalid);

            var newVector = new RoixVector(Vector.X * newBorder.Width / Border.Width, Vector.Y * newBorder.Height / Border.Height);
            return new(newVector, newBorder);
        }
        #endregion

    }
}