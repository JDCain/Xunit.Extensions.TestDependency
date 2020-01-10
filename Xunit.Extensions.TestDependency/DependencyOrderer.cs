using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions.TestDependency
{
    /// <summary>
    /// Custom test case orderer which builds order based on TestDependencyAttribute data
    /// </summary>
    public class DependencyOrderer : ITestCaseOrderer, ITestCollectionOrderer
    {
        private readonly IMessageSink _diagnosticMessageSink;
        public const string TypeName = "Xunit.Extensions.TestDependency.DependencyOrderer";
        public const string AssemblyName = "Xunit.Extensions.TestDependency";

        public DependencyOrderer(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var enumerable = testCases.ToList();
            if (enumerable.Count() > 1)
            {
                try
                {
                    var y = enumerable.TSort(
                        x => x.TestMethod.Method
                            .GetCustomAttributes((typeof(TestDependencyAttribute).AssemblyQualifiedName)).FirstOrDefault()
                            ?.GetNamedArgument<IReadOnlyList<string>>("Tests"), x => x.DisplayName);
                    return y;
                }
                catch (Exception e)
                {
                    _diagnosticMessageSink.OnMessage(new DiagnosticMessage($@"TestDependencyException:'{e.Message}'"));
                }
            }

            return enumerable;
        }

        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            var y = testCollections.TSort(
                x => x.CollectionDefinition.GetCustomAttributes((typeof(TestDependencyAttribute).AssemblyQualifiedName))
                    .FirstOrDefault()?.GetNamedArgument<IReadOnlyList<string>>("Tests"), x => x.DisplayName);
            return y;
        }
    }
}
