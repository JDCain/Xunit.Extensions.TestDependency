# Xunit.Extensions.TestDependency
**Supports:** *.NET Core 1.x, .NET Core 2.x.*, .NET Core 3.x.* and *.NET 4.6.1+*
Works in parallel

[![Build Status](https://servus.visualstudio.com/Xunit.Extensions.TestDependency/_apis/build/status/JDCain.Xunit.Extensions.TestDependency?branchName=master&jobName=Job)](https://servus.visualstudio.com/Xunit.Extensions.TestDependency/_build/latest?definitionId=3&branchName=master)
## So you want to have dependent tests
You should not do it, but here is how to do it.

* Xunit can order tests only within the same class. 
  * Use partial classes to help keep tests organized.
* Classes can run in parellel against each other.
  * If using this with Selenium tests you will need to configure your WebDriver to have individual profiles per test else they will share the same instance.
* Test Name depends on the current DisplayName used by Xunit. If it is not set to method only then it expects the whole namespace with methodname.


## Using
You can decorate the assembly or class using TestCaseOrderer
```csharp
[assembly: TestCaseOrderer(DependencyOrderer.TypeName, DependencyOrderer.AssemblyName)]
```
```csharp
[TestCaseOrderer(DependencyOrderer.TypeName, DependencyOrderer.AssemblyName)]
public class Example
{
  //tests here
}
```
## Example from Project Unit Test:

```csharp
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
```
