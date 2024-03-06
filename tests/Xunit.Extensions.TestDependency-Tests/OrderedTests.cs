using System.Collections.Generic;
using Xunit;
using Xunit.Extensions.TestDependency;

namespace Xunit.Extensions.TestDependency_Tests
{
    [Trait("xUnit", "Ordered")]
    [TestCaseOrderer(DependencyOrderer.TypeName, DependencyOrderer.AssemblyName)]
    public partial class OrderTests 
    {
        public static List<decimal> ExecutionOrder { get; set; } = new List<decimal>();

        [Fact]
        [TestDependency(nameof(OrderedTest1))]
        public void OrderedTest0()
        {
            ExecutionOrder.Add(0);
            Assert.Contains(1, ExecutionOrder);
            Assert.Contains(2.1M, ExecutionOrder);
            Assert.Contains(2.2M, ExecutionOrder);
            Assert.Contains(3, ExecutionOrder);
        }

        [Theory]
        [TestDependency(nameof(OrderedTest3))]
        [MemberData(nameof(Data))]
        public void OrderedTest2(decimal i)
        {
        
            ExecutionOrder.Add(i);
            Assert.DoesNotContain(0, ExecutionOrder);
            Assert.DoesNotContain(1, ExecutionOrder);
            Assert.Contains(3, ExecutionOrder);
        }

        public static TheoryData<decimal> Data => new TheoryData<decimal>()
        {
            { 2.1M },
            { 2.2M },
        };
    }

    public partial class OrderTests
    {
        [Fact]        
        public void OrderedTest3()
        {
            ExecutionOrder.Add(3);
            Assert.DoesNotContain(0, ExecutionOrder);
            Assert.DoesNotContain(1, ExecutionOrder);
            Assert.DoesNotContain(2.1m, ExecutionOrder);
            Assert.DoesNotContain(2.2m, ExecutionOrder);
        }

        [Fact]
        [TestDependency("OrderedTest2(i: 2.1)", "OrderedTest2(i: 2.2)")]
        public void OrderedTest1()
        {
            ExecutionOrder.Add(1);
            Assert.DoesNotContain(0, ExecutionOrder);
            Assert.Contains(2.1m, ExecutionOrder);
            Assert.Contains(2.1m, ExecutionOrder);
            Assert.Contains(2.2m, ExecutionOrder);
            Assert.Contains(3, ExecutionOrder);
        }
    }


}
