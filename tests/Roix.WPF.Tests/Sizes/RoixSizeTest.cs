using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixSizeTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(1.1, 2.2)]
        [InlineData(double.MaxValue, double.MaxValue)]
        public void Ctor(double w, double h)
        {
            var size = new RoixSize(w, h);
            size.IsEmpty.IsFalse();
            size.Width.Is(w);
            size.Height.Is(h);
        }

        [Theory]
        [InlineData(-1.1, 0)]
        [InlineData(0, -2.2)]
        [InlineData(double.MinValue, double.MinValue)]
        public void CtorïâêîÉ_ÉÅ(double w, double h)
        {
            Assert.Throws<ArgumentException>(() => new RoixSize(w, h));
        }

        [Fact]
        public void Empty()
        {
            var empty = RoixSize.Empty;
            empty.IsEmpty.IsTrue();
            empty.Width.Is(double.NegativeInfinity);
            empty.Height.Is(double.NegativeInfinity);
        }

        [Fact]
        public void Deconstruct()
        {
            var size = new RoixSize(1.1, 2.2);
            var (w, h) = size;
            w.Is(size.Width);
            h.Is(size.Height);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            double w = 1.1, h = 2.2;
            var size1 = new RoixSize(w, h);
            var size2 = new RoixSize(w, h);
            size1.Equals(size2).IsTrue();
            (size1 == size2).IsTrue();
            (size1 != size2).IsFalse();

            var obj2 = (object)size2;
            size1.Equals(obj2).IsTrue();

            size1 = RoixSize.Empty;
            size2 = RoixSize.Empty;
            size1.Equals(size2).IsTrue();
            (size1 == size2).IsTrue();
            (size1 != size2).IsFalse();
        }
        #endregion

        #region Casts
        [Fact]
        public void ToWindows()
        {
            var rs1 = new RoixSize(1.1, 2.2);
            System.Windows.Size ws1 = (System.Windows.Size)rs1;
            ws1.IsEmpty.Is(rs1.IsEmpty);
            ws1.Width.Is(rs1.Width);
            ws1.Height.Is(rs1.Height);

            var rs2 = RoixSize.Empty;
            System.Windows.Size ws2 = (System.Windows.Size)rs2;
            ws2.IsEmpty.Is(rs2.IsEmpty);
            ws2.Width.Is(rs2.Width);
            ws2.Height.Is(rs2.Height);
        }

        [Fact]
        public void ToRoix()
        {
            var ws1 = new System.Windows.Size(1.1, 2.2);
            RoixSize rs1 = (RoixSize)ws1;
            rs1.IsEmpty.Is(ws1.IsEmpty);
            rs1.Width.Is(ws1.Width);
            rs1.Height.Is(ws1.Height);

            var ws2 = System.Windows.Size.Empty;
            RoixSize rs2 = (RoixSize)ws2;
            rs2.IsEmpty.Is(ws2.IsEmpty);
            rs2.Width.Is(ws2.Width);
            rs2.Height.Is(ws2.Height);
        }

        [Fact]
        public void FromRoix()
        {
            var rs1 = new RoixSize(1.1, 2.2);
            RoixVector rv1 = (RoixVector)rs1;
            rv1.X.Is(rs1.Width);
            rv1.Y.Is(rs1.Height);

            RoixPoint rp1 = (RoixPoint)rs1;
            rp1.X.Is(rs1.Width);
            rp1.Y.Is(rs1.Height);

            Assert.Throws<ArgumentException>(() => (RoixVector)RoixSize.Empty);
            Assert.Throws<ArgumentException>(() => (RoixPoint)RoixSize.Empty);
        }

        #endregion

        #region Operators
        [Theory]
        [InlineData(0.5)]
        [InlineData(2)]
        public void OperatorèÊéZ(double ratio)
        {
            var src = new RoixSize(100, 200);

            var size1 = src * ratio;
            size1.Width.Is(src.Width * ratio);
            size1.Height.Is(src.Height * ratio);

            (src * 0).Is(RoixSize.Zero);
            (RoixSize.Zero * ratio).Is(RoixSize.Zero);
            (RoixSize.Empty * ratio).Is(RoixSize.Empty);
            Assert.Throws<ArgumentException>(() => src / -ratio);
        }

        [Theory]
        [InlineData(0.5)]
        [InlineData(2)]
        public void OperatorèúéZ(double ratio)
        {
            var src = new RoixSize(100, 200);

            var size1 = src / ratio;
            size1.Width.Is(src.Width / ratio);
            size1.Height.Is(src.Height / ratio);

            (RoixSize.Zero / ratio).Is(RoixSize.Zero);
            (RoixSize.Empty / ratio).Is(RoixSize.Empty);
            Assert.Throws<ArgumentException>(() => src / -ratio);
            Assert.Throws<DivideByZeroException>(() => src / 0);
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            new RoixSize(1.1, 0).IsZero.IsFalse();
            new RoixSize(0, 0).IsZero.IsTrue();
            RoixSize.Empty.IsZero.IsFalse();
            RoixSize.Zero.IsZero.IsTrue();
        }

        [Fact]
        public void IsValid()
        {
            new RoixSize(1.1, 2.2).IsValid.IsTrue();
            new RoixSize(1.1, 2.2).IsInvalid.IsFalse();

            new RoixSize(0, 0).IsValid.IsFalse();
            new RoixSize(0, 0).IsInvalid.IsTrue();

            RoixSize.Empty.IsValid.IsFalse();
            RoixSize.Empty.IsInvalid.IsTrue();
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(1, 1, 10, 10, true)]
        [InlineData(2, 2, 2, 2, true)]
        [InlineData(2, 2, 1, 1, false)]
        public void IsInside(double srcWidth, double srcHeight, double borderWidth, double borderHeight, bool isInside)
        {
            var src = new RoixSize(srcWidth, srcHeight);
            var border = new RoixSize(borderWidth, borderHeight);

            src.IsInside(border).Is(isInside);
            src.IsOutside(border).Is(!isInside);
        }
        #endregion

    }
}
