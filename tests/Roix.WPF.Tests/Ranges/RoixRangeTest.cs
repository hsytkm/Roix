//using Roix.Wpf.Ranges;
//using System;
//using Xunit;

//namespace Roix.WPF.Tests
//{
//    public class RoixRangeTest
//    {
//        [Theory]
//        [InlineData(0, 0)]
//        [InlineData(1.1, 2.2)]
//        [InlineData(-1.1, 2.2)]
//        [InlineData(-4, -3)]
//        [InlineData(double.MinValue, double.MaxValue)]
//        public void Ctor(double min, double max)
//        {
//            var rr = new RoixRange(min, max);
//            rr.Min.Is(min);
//            rr.Max.Is(max);

//            if (min != max) Assert.Throws<ArgumentException>(() => new RoixRange(max, min));
//        }

//        [Fact]
//        public void Deconstruct()
//        {
//            var rr = new RoixRange(1.1, 2.2);
//            var (x, y) = rr;
//            x.Is(rr.Min);
//            y.Is(rr.Max);
//        }

//        [Fact]
//        public void Equal()
//        {
//            double x = 1.1, y = 2.2;
//            var p1 = new RoixRange(x, y);
//            var p2 = new RoixRange(x, y);

//            p1.Equals(p2).IsTrue();
//            (p1 == p2).IsTrue();
//            (p1 != p2).IsFalse();

//            var obj2 = (object)p2;
//            p1.Equals(obj2).IsTrue();
//        }

//    }
//}
