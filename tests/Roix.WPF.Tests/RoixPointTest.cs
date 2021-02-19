using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixPointTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void Ctor(double x, double y)
        {
            var point = new RoixPoint(x, y);
            point.X.Is(x);
            point.Y.Is(y);
        }

        [Fact]
        public void Deconstruct()
        {
            var point = new RoixPoint(1.1, 2.2);
            var (x, y) = point;
            x.Is(point.X);
            y.Is(point.Y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void WindowsToRoix(double x, double y)
        {
            var win1 = new System.Windows.Point(x, y);
            RoixPoint roix1 = win1;
            roix1.X.Is(win1.X);
            roix1.Y.Is(win1.Y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void RoixToWindows(double x, double y)
        {
            var roix1 = new RoixPoint(x, y);
            System.Windows.Point win1 = roix1;
            win1.X.Is(roix1.X);
            win1.Y.Is(roix1.Y);
        }

        [Fact]
        public void Equal()
        {
            double x = 1.1, y = 2.2;
            var p1 = new RoixPoint(x, y);
            var p2 = new RoixPoint(x, y);

            p1.Equals(p2).IsTrue();
            (p1 == p2).IsTrue();
            (p1 != p2).IsFalse();

            var obj2 = (object)p2;
            p1.Equals(obj2).IsTrue();
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1.49, 2.51, 1, 3)]
        [InlineData(-1.49, -2.51, -1, -3)]
        public void ToRoixIntPoint(double x, double y, int answerX, int answerY)
        {
            var ip = new RoixPoint(x, y).ToRoixIntPoint();
            ip.X.Is(answerX);
            ip.Y.Is(answerY);
        }

    }
}
