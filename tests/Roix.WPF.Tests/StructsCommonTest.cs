using Roix.Wpf;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Roix.WPF.Tests
{
    public class StructsCommonTest
    {
        [Fact]
        public void StructSize()
        {
            Marshal.SizeOf<System.Windows.Point>().Is(16);
            Marshal.SizeOf<RoixPoint>().Is(16);
            Marshal.SizeOf<RoixGaugePoint>().Is(32);
            Marshal.SizeOf<RoixIntPoint>().Is(8);

            Marshal.SizeOf<System.Windows.Rect>().Is(32);
            Marshal.SizeOf<RoixRect>().Is(32);
            Marshal.SizeOf<RoixGaugeRect>().Is(48);
            Marshal.SizeOf<RoixIntRect>().Is(16);
            //Marshal.SizeOf<RoixIntGaugeRect>().Is(24);

            Marshal.SizeOf<System.Windows.Size>().Is(16);
            Marshal.SizeOf<RoixSize>().Is(16);
            Marshal.SizeOf<RoixGaugeSize>().Is(32);
            Marshal.SizeOf<RoixIntSize>().Is(8);

            Marshal.SizeOf<System.Windows.Vector>().Is(16);
            Marshal.SizeOf<RoixVector>().Is(16);
            Marshal.SizeOf<RoixGaugeVector>().Is(32);
        }

    }
}
