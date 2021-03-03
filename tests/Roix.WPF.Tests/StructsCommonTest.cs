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
            Marshal.SizeOf<RoixBorderPoint>().Is(32);
            Marshal.SizeOf<RoixIntPoint>().Is(8);

            Marshal.SizeOf<System.Windows.Rect>().Is(32);
            Marshal.SizeOf<RoixRect>().Is(32);
            Marshal.SizeOf<RoixBorderRect>().Is(48);
            Marshal.SizeOf<RoixIntRect>().Is(16);

            Marshal.SizeOf<System.Windows.Size>().Is(16);
            Marshal.SizeOf<RoixSize>().Is(16);
            Marshal.SizeOf<RoixBorderSize>().Is(32);
            Marshal.SizeOf<RoixIntSize>().Is(8);

            Marshal.SizeOf<System.Windows.Vector>().Is(16);
            Marshal.SizeOf<RoixVector>().Is(16);
            Marshal.SizeOf<RoixBorderVector>().Is(32);
        }

    }
}
