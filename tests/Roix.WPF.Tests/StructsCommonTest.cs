using Roix.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace Roix.WPF.Tests
{
    public class StructsCommonTest
    {
        [Fact]
        public void StructSize()
        {
            Marshal.SizeOf<RoixLine>().Is(32);
            Marshal.SizeOf<RoixBorderLine>().Is(48);
            Marshal.SizeOf<RoixIntLine>().Is(16);
            Marshal.SizeOf<RoixBorderIntLine>().Is(24);

            Marshal.SizeOf<System.Windows.Point>().Is(16);
            Marshal.SizeOf<RoixPoint>().Is(16);
            Marshal.SizeOf<RoixBorderPoint>().Is(32);
            Marshal.SizeOf<RoixIntPoint>().Is(8);
            Marshal.SizeOf<RoixBorderIntPoint>().Is(16);

            Marshal.SizeOf<RoixRatioXY>().Is(16);
            Marshal.SizeOf<RoixRatioXYWH>().Is(32);

            Marshal.SizeOf<System.Windows.Rect>().Is(32);
            Marshal.SizeOf<RoixRect>().Is(32);
            Marshal.SizeOf<RoixBorderRect>().Is(48);
            Marshal.SizeOf<RoixIntRect>().Is(16);
            Marshal.SizeOf<RoixBorderIntRect>().Is(24);

            Marshal.SizeOf<System.Windows.Size>().Is(16);
            Marshal.SizeOf<RoixSize>().Is(16);
            Marshal.SizeOf<RoixBorderSize>().Is(32);
            Marshal.SizeOf<RoixIntSize>().Is(8);
            Marshal.SizeOf<RoixBorderIntSize>().Is(16);

            Marshal.SizeOf<System.Windows.Vector>().Is(16);
            Marshal.SizeOf<RoixVector>().Is(16);
            Marshal.SizeOf<RoixBorderVector>().Is(32);
            Marshal.SizeOf<RoixIntVector>().Is(8);
            Marshal.SizeOf<RoixBorderIntVector>().Is(16);
        }

        private readonly static IReadOnlyList<Type> _roixTypes = new[]
        {
            typeof(RoixLine), typeof(RoixPoint), typeof(RoixRect), typeof(RoixSize), typeof(RoixVector),
            typeof(RoixIntLine), typeof(RoixIntPoint), typeof(RoixIntRect), typeof(RoixIntSize), typeof(RoixIntVector),
            typeof(RoixBorderLine), typeof(RoixBorderPoint), typeof(RoixBorderRect), typeof(RoixBorderSize), typeof(RoixBorderVector),
            typeof(RoixBorderIntLine), typeof(RoixBorderIntPoint), typeof(RoixBorderIntRect), typeof(RoixBorderIntSize), typeof(RoixBorderIntVector),
            typeof(RoixRatioXY), typeof(RoixRatioXYWH),
        };

        private static IReadOnlyList<Type> RoixDoubleTypes => _roixTypes.Where(t => !t.Name.Contains("Border") && !t.Name.Contains("Int") && !t.Name.Contains("Ratio")).ToList();
        private static IReadOnlyList<Type> RoixIntTypes => _roixTypes.Where(t => t.Name.Contains("RoixInt")).ToList();
        private static IReadOnlyList<Type> RoixBorderDoubleTypes => _roixTypes.Where(t => !t.Name.Contains("Int") && t.Name.Contains("Border")).ToList();
        private static IReadOnlyList<Type> RoixBorderIntTypes => _roixTypes.Where(t => t.Name.Contains("BorderInt")).ToList();

        [Fact]
        public void RoixDoubleTypes_HasMethods()
        {
            var types = RoixDoubleTypes;

            foreach (var t in types) t.HasMethod("ToRoixBorder").IsTrue();
            foreach (var t in types) t.HasMethod("ToRoixInt").IsTrue();
            foreach (var t in types) t.HasMethod("IsZero").IsTrue();
            foreach (var t in types) t.HasMethod("IsInside").IsTrue();
        }

        [Fact]
        public void RoixIntTypes_HasMethods()
        {
            var types = RoixIntTypes;

            foreach (var t in types) t.HasMethod("ToRoixBorder").IsTrue();
            foreach (var t in types) t.HasMethod("IsZero").IsTrue();
            foreach (var t in types) t.HasMethod("IsInside").IsTrue();
        }

        [Fact]
        public void RoixBorderDoubleTypes_HasMethods()
        {
            var types = RoixBorderDoubleTypes;

            foreach (var t in types) t.HasMethod("ToRoixInt").IsTrue();
            foreach (var t in types) t.HasMethod("ConvertToNewBorder").IsTrue();
            foreach (var t in types) t.HasMethod("IsZero").IsTrue();
            foreach (var t in types) t.HasMethod("IsInsideBorder").IsTrue();
        }

        [Fact]
        public void RoixBorderIntTypes_HasMethods()
        {
            var types = RoixBorderIntTypes;

            foreach (var t in types) t.HasMethod("ConvertToNewBorder").IsTrue();
            foreach (var t in types) t.HasMethod("IsZero").IsTrue();
            foreach (var t in types) t.HasMethod("IsInsideBorder").IsTrue();
        }
    }

    static class TypeExtension
    {
        internal static bool HasMethod(this Type type, string methodName) => type.GetMethods().Any(x => x?.ToString()?.Contains(methodName + "(") ?? false);
        //internal static bool HasMethodAll(this IEnumerable<Type> types, string methodName) => types.All(t => HasMethod(t, methodName));
    }
}
