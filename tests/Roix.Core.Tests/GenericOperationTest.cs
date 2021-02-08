using Roix.Core.Extensions;
using System;
using Xunit;

namespace Roix.Core.Tests
{
    public class GenericOperationTest
    {
        [Fact]
        public void Add()
        {
            var opi = GenericOperation<int>.GetInstance();
            opi.Add(110, 13).Is(123);

            var opd = GenericOperation<double>.GetInstance();
            opd.Add(1.10, 0.13).Is(1.23);
        }

    }
}
