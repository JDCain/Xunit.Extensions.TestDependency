using System.Collections.Generic;
using Xunit.Extensions.TestDependency;

namespace Xunit.Extensions.TestDependency_Tests
{
    [Trait("xUnit", "Ordered")]
    [TestCaseOrderer(DependencyOrderer.TypeName, DependencyOrderer.AssemblyName)]
    public partial class OrderTests
    {
        public static List<double> ExecutionOrder { get; set; } = new List<double>();

        [Fact]
        [TestDependency("OrderedTest1")]
        public void OrderedTest0()
        {
            ExecutionOrder.Add(0);
            Assert.Contains(1, ExecutionOrder);
            Assert.Contains(2.1, ExecutionOrder);
            Assert.Contains(2.2, ExecutionOrder);
            Assert.Contains(3, ExecutionOrder);
        }

        [Theory]
        [TestDependency("OrderedTest3")]
        [InlineData(2.1)]
        [InlineData(2.2)]
        public void OrderedTest2(double i)
        {
            ExecutionOrder.Add(i);
            Assert.DoesNotContain(0, ExecutionOrder);
            Assert.DoesNotContain(1, ExecutionOrder);
            Assert.Contains(3, ExecutionOrder);
        }
    }

    public partial class OrderTests
    {
        [Fact]
        public void OrderedTest3()
        {
            ExecutionOrder.Add(3);
            Assert.DoesNotContain(0, ExecutionOrder);
            Assert.DoesNotContain(1, ExecutionOrder);
            Assert.DoesNotContain(2.1, ExecutionOrder);
            Assert.DoesNotContain(2.2, ExecutionOrder);
        }

        [Fact]
        [TestDependency("OrderedTest2(i: 2.1)", "OrderedTest2(i: 2.2)")]
        public void OrderedTest1()
        {
            ExecutionOrder.Add(1);
            Assert.DoesNotContain(0, ExecutionOrder);
            Assert.Contains(2.1, ExecutionOrder);
            Assert.Contains(2.1, ExecutionOrder);
            Assert.Contains(2.2, ExecutionOrder);
            Assert.Contains(3, ExecutionOrder);
        }
    }
}
